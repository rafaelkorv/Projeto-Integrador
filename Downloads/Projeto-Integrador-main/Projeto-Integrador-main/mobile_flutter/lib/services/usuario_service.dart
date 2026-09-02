import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import '../models/usuario.dart';
import 'api_service.dart';

class UsuarioService {
  String get baseUrl => ApiService.baseUrl;

  /// Login via POST /usuarios/login — retorna UsuarioPerfilDTO
  Future<Usuario?> login(String email, String senha) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/usuarios/login'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({'email': email, 'senha': senha}),
      ).timeout(const Duration(seconds: 10));

      if (kDebugMode) {
        print('=== LOGIN RESPONSE ===');
        print('Status: ${response.statusCode}');
        print('Body: ${response.body}');
      }

      if (response.statusCode == 200 &&
          response.body.isNotEmpty &&
          response.body != 'null') {
        final json = jsonDecode(utf8.decode(response.bodyBytes));
        final user = Usuario.fromJson(json);
        user.email = email; // Garante que o email esteja presente
        return user;
      }
      return null;
    } catch (e) {
      if (kDebugMode) print('Erro no login: $e');
      return null;
    }
  }

  /// Cadastro via POST /usuarios — envia nome, nomeCompleto, username, email, senha, dataNascimento
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
        'nomeCompleto': (nomeCompleto != null && nomeCompleto.isNotEmpty)
            ? nomeCompleto
            : nome,
        'username': username,
        'email': email,
        'senha': senha,
        'dataNascimento': (dataNascimento != null && dataNascimento.isNotEmpty)
            ? dataNascimento
            : '2000-01-01',
      };

      final response = await http.post(
        Uri.parse('$baseUrl/usuarios'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode(payload),
      ).timeout(const Duration(seconds: 10));

      if (kDebugMode) {
        print('=== CADASTRO RESPONSE ===');
        print('Status: ${response.statusCode}');
        print('Body: ${response.body}');
      }

      if (response.statusCode == 200 || response.statusCode == 201) {
        final json = jsonDecode(utf8.decode(response.bodyBytes));
        return {'sucesso': true, 'usuario': Usuario.fromJson(json)};
      }

      String mensagemErro = '';
      try {
        final Map<String, dynamic> errorJson =
            jsonDecode(utf8.decode(response.bodyBytes));
        if (errorJson.containsKey('message') &&
            errorJson['message'] != null &&
            errorJson['message'].toString().isNotEmpty) {
          mensagemErro = errorJson['message'].toString();
        } else if (errorJson.containsKey('error') &&
            errorJson['error'] != null &&
            errorJson['error'].toString().isNotEmpty) {
          mensagemErro = errorJson['error'].toString();
        }
      } catch (_) {
        if (response.body.isNotEmpty) {
          mensagemErro = response.body;
        }
      }

      // Sanitiza erros técnicos garantindo mensagens amigáveis ao usuário
      final bodyLower = mensagemErro.toLowerCase();
      if (bodyLower.contains('dataintegrity') ||
          bodyLower.contains('internal server error') ||
          bodyLower.contains('500') ||
          response.statusCode == 500) {
        mensagemErro = 'Não foi possível concluir o cadastro. Verifique os dados informados.';
      } else if (bodyLower.contains('username') ||
          bodyLower.contains('usuário') ||
          bodyLower.contains('usuario')) {
        mensagemErro = 'Este nome de usuário já está cadastrado.';
      } else if (bodyLower.contains('email')) {
        mensagemErro = 'Este e-mail já está cadastrado.';
      } else if (bodyLower.contains('nascimento') || bodyLower.contains('date')) {
        mensagemErro = 'Data de nascimento inválida.';
      } else if (mensagemErro.isEmpty || response.statusCode == 400) {
        mensagemErro = 'Dados de cadastro inválidos. Verifique as informações.';
      }

      return {'sucesso': false, 'erro': mensagemErro};
    } catch (e) {
      if (kDebugMode) print('Erro no cadastro: $e');
      return {
        'sucesso': false,
        'erro': 'Não foi possível conectar ao servidor. Verifique sua conexão e tente novamente.'
      };
    }
  }

  /// Busca perfil completo do usuário por ID
  Future<Usuario?> buscarPorId(int id) async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/usuarios/$id'),
      ).timeout(const Duration(seconds: 8));

      if (response.statusCode == 200) {
        return Usuario.fromJson(
            jsonDecode(utf8.decode(response.bodyBytes)));
      }
      return null;
    } catch (e) {
      if (kDebugMode) print('Erro ao buscar usuário: $e');
      return null;
    }
  }

  /// Atualiza nome e bio do perfil via PUT /usuarios/{id}/perfil
  Future<bool> atualizarPerfil(int id, String nome, String bio) async {
    try {
      final response = await http.put(
        Uri.parse('$baseUrl/usuarios/$id/perfil'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({'nome': nome, 'bio': bio}),
      ).timeout(const Duration(seconds: 8));

      return response.statusCode == 200;
    } catch (e) {
      if (kDebugMode) print('Erro ao atualizar perfil: $e');
      return false;
    }
  }

  Future<List<Usuario>> listarUsuarios() async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/usuarios'),
      ).timeout(const Duration(seconds: 8));

      if (response.statusCode == 200) {
        final List lista = jsonDecode(utf8.decode(response.bodyBytes));
        return lista.map((e) => Usuario.fromJson(e)).toList();
      }
    } catch (e) {
      if (kDebugMode) print('Erro ao listar usuários: $e');
    }
    return [];
  }

  Future<void> deletarUsuario(int id) async {
    try {
      await http.delete(Uri.parse('$baseUrl/usuarios/$id'));
    } catch (e) {
      if (kDebugMode) print('Erro ao deletar usuário: $e');
    }
  }

  /// Atualiza foto de perfil via POST /usuarios/{id}/foto (Google Drive)
  Future<Map<String, dynamic>> atualizarFotoPerfil({
    required int idUsuario,
    String? caminhoArquivo,
    Uint8List? bytes,
    required String nomeArquivo,
  }) async {
    try {
      final uri = Uri.parse('$baseUrl/usuarios/$idUsuario/foto');
      final request = http.MultipartRequest('POST', uri);

      if (bytes != null) {
        request.files.add(
          http.MultipartFile.fromBytes(
            'foto',
            bytes,
            filename: nomeArquivo,
          ),
        );
      } else if (caminhoArquivo != null) {
        request.files.add(
          await http.MultipartFile.fromPath(
            'foto',
            caminhoArquivo,
            filename: nomeArquivo,
          ),
        );
      } else {
        return {'sucesso': false, 'erro': 'Nenhum arquivo selecionado.'};
      }

      final streamedResponse =
          await request.send().timeout(const Duration(seconds: 30));
      final response = await http.Response.fromStream(streamedResponse);

      if (kDebugMode) {
        print('=== UPLOAD FOTO RESPONSE ===');
        print('Status: ${response.statusCode}');
        print('Body: ${response.body}');
      }

      if (response.statusCode == 200) {
        final json = jsonDecode(utf8.decode(response.bodyBytes));
        final user = Usuario.fromJson(json);
        return {'sucesso': true, 'usuario': user};
      }

      String msg = 'Não foi possível alterar a foto.';
      try {
        final err = jsonDecode(utf8.decode(response.bodyBytes));
        if (err is Map && err.containsKey('error') && err['error'] != null) {
          msg = err['error'].toString();
        }
      } catch (_) {
        if (response.body.isNotEmpty) msg = response.body;
      }
      return {'sucesso': false, 'erro': msg};
    } catch (e) {
      if (kDebugMode) print('Erro ao atualizar foto de perfil: $e');
      return {
        'sucesso': false,
        'erro': 'Erro de conexão ao enviar a foto para o servidor.'
      };
    }
  }
}