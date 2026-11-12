import 'dart:convert';
import 'dart:typed_data';
import 'package:flutter/foundation.dart';
import 'authenticated_http.dart' as http;
import 'package:http_parser/http_parser.dart';
import '../models/usuario.dart';
import 'api_service.dart';

class UsuarioService {
  String get baseUrl => ApiService.baseUrl;

  Future<Usuario?> login(String email, String senha) async {
    try {
      final res = await http.post(Uri.parse('$baseUrl/usuarios/login'),
        headers: {'Content-Type': 'application/json; charset=UTF-8'},
        body: jsonEncode({'email': email, 'senha': senha}),
      ).timeout(const Duration(seconds: 15));
      if (kDebugMode) {
        debugPrint('LOGIN HTTP ${res.statusCode}: ${_resumo(res.body)}');
      }
      if (res.statusCode != 200 || res.body.trim().isEmpty || res.body.trim() == 'null') {
        return null;
      }
      final data = jsonDecode(utf8.decode(res.bodyBytes));
      if (data is! Map) {
        return null;
      }
      final usuario = Usuario.fromJson(Map<String, dynamic>.from(data));
      if (usuario.email.isEmpty) {
        usuario.email = email;
      }
      return usuario;
    } catch (e) {
      if (kDebugMode) {
        debugPrint('Erro no login: $e');
      }
      return null;
    }
  }

  Future<Map<String, dynamic>> criarUsuario({
    required String nome,
    required String username,
    required String email,
    required String senha,
    String? dataNascimento,
    String? nomeCompleto,
  }) async {
    try {
      final payload = {
        'nome': nome,
        'nomeCompleto': nomeCompleto?.isNotEmpty == true ? nomeCompleto : nome,
        'username': username,
        'email': email,
        'senha': senha,
        'dataNascimento': dataNascimento?.isNotEmpty == true ? dataNascimento : '2000-01-01',
      };
      final response = await http.post(Uri.parse('$baseUrl/usuarios'),
        headers: {'Content-Type': 'application/json; charset=UTF-8'},
        body: jsonEncode(payload),
      ).timeout(const Duration(seconds: 18));
      if (kDebugMode) {
        debugPrint('CADASTRO HTTP ${response.statusCode}: ${_resumo(response.body)}');
      }
      if (response.statusCode == 200 || response.statusCode == 201) {
        if (response.body.trim().isEmpty) {
          return {'sucesso': false, 'erro': 'Conta criada, mas a API não retornou os dados. Faça login.'};
        }
        final data = jsonDecode(utf8.decode(response.bodyBytes));
        return {'sucesso': true, 'usuario': Usuario.fromJson(Map<String, dynamic>.from(data))};
      }
      return {'sucesso': false, 'erro': 'Não foi possível concluir o cadastro (HTTP ${response.statusCode}). ${_resumo(response.body)}'};
    } catch (e) {
      if (kDebugMode) {
        debugPrint('CADASTRO ERRO: $e');
      }
      return {'sucesso': false, 'erro': 'Não foi possível conectar ao servidor: $e'};
    }
  }

  Future<Usuario?> buscarPorId(int id) async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/usuarios/$id'))
          .timeout(const Duration(seconds: 12));
      if (response.statusCode != 200) {
        return null;
      }
      final data = jsonDecode(utf8.decode(response.bodyBytes));
      if (data is! Map) {
        return null;
      }
      return Usuario.fromJson(Map<String, dynamic>.from(data));
    } catch (e) {
      if (kDebugMode) {
        debugPrint('Buscar usuário: $e');
      }
      return null;
    }
  }

  String? ultimoErroAtualizacaoPerfil;

  Future<bool> atualizarPerfil(int id, String nome, String bio, {String? novoUsername}) async {
    ultimoErroAtualizacaoPerfil = null;
    try {
      final atual = await buscarPorId(id);
      final username = novoUsername?.trim() ?? atual?.username;
      if (username == null || username.trim().isEmpty) {
        ultimoErroAtualizacaoPerfil = 'Não foi possível obter o username atual do servidor.';
        return false;
      }
      // V5 permite trocar username ou manter o atual. O Java verifica duplicidade.
      final response = await http.put(Uri.parse('$baseUrl/usuarios/$id/perfil'),
        headers: {'Content-Type': 'application/json; charset=UTF-8'},
        body: jsonEncode({'nome': nome, 'bio': bio, 'username': username}),
      ).timeout(const Duration(seconds: 12));
      if (response.statusCode == 200 || response.statusCode == 204) return true;
      ultimoErroAtualizacaoPerfil = 'HTTP ${response.statusCode}: ${_resumo(response.body)}';
      if (kDebugMode) {
        debugPrint('EDITAR PERFIL: $ultimoErroAtualizacaoPerfil');
      }
      return false;
    } catch (e) {
      ultimoErroAtualizacaoPerfil = '$e';
      if (kDebugMode) {
        debugPrint('Atualizar perfil: $e');
      }
      return false;
    }
  }

  Future<List<Usuario>> listarUsuarios() async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/usuarios'))
          .timeout(const Duration(seconds: 12));
      if (response.statusCode != 200) {
        return [];
      }
      final data = jsonDecode(utf8.decode(response.bodyBytes));
      final lista = data is List ? data : (data is Map ? data['content'] : null);
      if (lista is! List) {
        return [];
      }
      return lista.whereType<Map>().map((e) => Usuario.fromJson(Map<String, dynamic>.from(e))).toList();
    } catch (e) {
      if (kDebugMode) {
        debugPrint('Listar usuários: $e');
      }
      return [];
    }
  }

  // Mantém assinatura antiga; não deve estar exposto a usuários comuns na UI.
  Future<void> deletarUsuario(int id) async {
    final response = await http.delete(Uri.parse('$baseUrl/usuarios/$id'))
        .timeout(const Duration(seconds: 12));
    if (response.statusCode != 200 && response.statusCode != 204) {
      throw Exception('Exclusão recusada (HTTP ${response.statusCode}).');
    }
  }

  static const String _perfilPath = String.fromEnvironment(
    'PROFILE_IMAGE_UPLOAD_PATH', defaultValue: '/usuarios/{id}/foto');
  static const String _perfilField = String.fromEnvironment(
    'PROFILE_IMAGE_FIELD', defaultValue: 'foto');

  static MediaType _mimeDaImagem(Uint8List dados) {
    if (dados.length >= 3 && dados[0] == 255 && dados[1] == 216 && dados[2] == 255) {
      return MediaType('image', 'jpeg');
    }
    if (dados.length >= 8 && dados[0] == 137 && dados[1] == 80 &&
        dados[2] == 78 && dados[3] == 71 && dados[4] == 13 &&
        dados[5] == 10 && dados[6] == 26 && dados[7] == 10) {
      return MediaType('image', 'png');
    }
    if (dados.length >= 12 && String.fromCharCodes(dados.sublist(0, 4)) == 'RIFF' &&
        String.fromCharCodes(dados.sublist(8, 12)) == 'WEBP') {
      return MediaType('image', 'webp');
    }
    throw const FormatException('Formato de foto inválido. Use JPG, PNG ou WEBP.');
  }

  Future<Map<String, dynamic>> atualizarFotoPerfil({
    required int idUsuario,
    String? caminhoArquivo,
    Uint8List? bytes,
    required String nomeArquivo,
  }) async {
    try {
      if (bytes == null) {
        return {'sucesso': false, 'erro': 'Selecione uma imagem válida.'};
      }
      if (bytes.isEmpty || bytes.length > 5 * 1024 * 1024) {
        return {'sucesso': false, 'erro': 'A foto deve ter até 5 MB.'};
      }
      final mime = _mimeDaImagem(bytes);
      final caminhoUpload = _perfilPath.replaceAll('{id}', '$idUsuario');
      final uri = Uri.parse('$baseUrl$caminhoUpload');
      final request = http.MultipartRequest('POST', uri);
      request.files.add(http.MultipartFile.fromBytes(_perfilField, bytes,
        filename: nomeArquivo, contentType: mime));
      final stream = await request.send().timeout(const Duration(seconds: 35));
      final response = await http.Response.fromStream(stream);
      if (kDebugMode) {
        debugPrint('FOTO $uri: HTTP ${response.statusCode}; MIME $mime; ${_resumo(response.body)}');
      }
      if (response.statusCode != 200 && response.statusCode != 201) {
        return {'sucesso': false,
          'erro': 'Servidor recusou foto (HTTP ${response.statusCode}). ${_resumo(response.body)}'};
      }
      // GET /usuarios/{id} comprova que o caminho foi persistido no banco.
      final atualizado = await buscarPorId(idUsuario);
      final caminho = atualizado?.fotoPerfil;
      if (caminho == null || caminho.trim().isEmpty) {
        return {'sucesso': false,
          'erro': 'A API aceitou o upload, mas GET /usuarios/$idUsuario não retornou a foto.'};
      }
      final url = ApiService.formatarUrlFotoPerfil(caminho);
      if (url == null) {
        return {'sucesso': false, 'erro': 'A URL da foto retornada pelo Java é inválida.'};
      }
      final teste = await http.get(Uri.parse(url)).timeout(const Duration(seconds: 15));
      if (teste.statusCode != 200) {
        return {'sucesso': false,
          'erro': 'A foto foi registrada no banco, mas GET $url respondeu HTTP ${teste.statusCode}. '
              'Confira se o arquivo existe na pasta uploads/perfis do backend em execução.'};
      }
      return {'sucesso': true, 'usuario': atualizado};
    } catch (e) {
      if (kDebugMode) {
        debugPrint('UPLOAD FOTO ERRO: $e');
      }
      return {'sucesso': false, 'erro': 'Erro ao enviar a foto: $e'};
    }
  }

  static String _resumo(String msg) =>
      msg.length > 250 ? '${msg.substring(0, 250)}...' : msg;
}
