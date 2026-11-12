import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'authenticated_http.dart' as http;
import 'package:http_parser/http_parser.dart';
import '../models/post.dart';
import 'api_service.dart';

class PostService {
  String get baseUrl => ApiService.baseUrl;

  // Rota REAL: ImagensPostController @RequestMapping('/imagens-post')
  // @PostMapping('/upload') com @RequestParam idPost e imagem.
  static const String _uploadPath = '/imagens-post/upload';
  static const String _uploadField = 'imagem';

  Future<List<Post>> listarPosts({int? page, int size = 10}) async {
    final uri = Uri.parse(page == null
        ? '$baseUrl/posts'
        : '$baseUrl/posts?page=$page&size=$size');
    final response = await http.get(uri).timeout(const Duration(seconds: 15));
    if (response.statusCode != 200) {
      throw Exception('Não foi possível buscar os posts (HTTP ${response.statusCode}).');
    }
    final dynamic data = jsonDecode(utf8.decode(response.bodyBytes));
    final dynamic lista = data is List ? data : (data is Map ? data['content'] : null);
    if (lista is! List) {
      throw const FormatException('A API não retornou uma lista de posts.');
    }
    final posts = lista.whereType<Map>().map((v) =>
        Post.fromJson(Map<String, dynamic>.from(v))).toList();
    return _comImagens(posts);
  }

  Future<List<Post>> listarPostsPorUsuario(int idUsuario) async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/posts/usuario/$idUsuario'))
          .timeout(const Duration(seconds: 15));
      if (response.statusCode != 200) return [];
      final data = jsonDecode(utf8.decode(response.bodyBytes));
      final lista = data is List ? data : (data is Map ? data['content'] : null);
      if (lista is! List) return [];
      final posts = lista.whereType<Map>().map((v) =>
          Post.fromJson(Map<String, dynamic>.from(v))).toList();
      return _comImagens(posts);
    } catch (e) {
      if (kDebugMode) debugPrint('Erro ao buscar posts do usuário: $e');
      return [];
    }
  }

  // Rota real de consulta dos registros persistidos no banco.
  static const String _listImagesPath = '/imagens-post';

  Future<Map<int, List<String>>> listarImagensPosts() async {
    final imagens = <int, List<String>>{};
    // A rota adicional é SOMENTE leitura para fotos publicadas por patches
    // anteriores. Novos uploads sempre usam a rota real e gravam no banco.
    for (final caminho in <String>[_listImagesPath, '/api/socialjoin-media/posts']) {
      try {
        final response = await http.get(Uri.parse('$baseUrl$caminho'))
            .timeout(const Duration(seconds: 15));
        if (response.statusCode != 200) continue;
        final decoded = jsonDecode(utf8.decode(response.bodyBytes));
        final dynamic lista = decoded is List ? decoded
          : (decoded is Map ? (decoded['content'] ?? decoded['imagens']) : null);
        if (lista is! List) continue;
        for (final raw in lista) {
          if (raw is! Map) continue;
          final rawId = raw['idPost'] ?? raw['id_post'] ?? raw['postId'];
          final id = rawId is num ? rawId.toInt() : int.tryParse('$rawId');
          final url = raw['url'] ?? raw['imagemUrl'] ?? raw['caminho'];
          if (id == null || id <= 0 || url is! String || url.trim().isEmpty) continue;
          final destino = imagens.putIfAbsent(id, () => <String>[]);
          if (!destino.contains(url.trim())) destino.add(url.trim());
        }
      } catch (e) {
        if (kDebugMode) debugPrint('Listagem de imagens ($caminho): $e');
      }
    }
    return imagens;
  }

  Future<List<Post>> _comImagens(List<Post> posts) async {
    try {
      final mapeamento = await listarImagensPosts();
      for (final post in posts) {
        final imagensDaApi = mapeamento[post.idPost] ?? const <String>[];
        if (imagensDaApi.isEmpty) continue;
        post.imagens = {...post.imagens, ...imagensDaApi}.toList();
        post.imagemUrl ??= post.imagens.first;
      }
    } catch (e) {
      if (kDebugMode) debugPrint('Imagens de posts não disponíveis: $e');
    }
    return posts;
  }

  // Retorna o ID real criado. Nunca inventa ID ou cria um segundo post
  // se a etapa de upload falhar.
  Future<int> criarPostRetornandoId(Post post) async {
    final response = await http.post(
      Uri.parse('$baseUrl/posts'),
      headers: {'Content-Type': 'application/json; charset=UTF-8', 'Accept': 'application/json'},
      body: jsonEncode(post.toJson()),
    ).timeout(const Duration(seconds: 20));

    if (response.statusCode != 200 && response.statusCode != 201) {
      throw Exception('Falha ao criar publicação (HTTP ${response.statusCode}): '
          '${_resumo(response.body)}');
    }
    if (response.body.trim().isEmpty) {
      throw const FormatException('O post foi criado, mas a API não devolveu seu ID. '
          'Verifique no feed antes de tentar publicar novamente.');
    }
    final decoded = jsonDecode(utf8.decode(response.bodyBytes));
    final data = decoded is Map ? decoded : null;
    final idRaw = data?['idPost'] ?? data?['id'] ??
        (data?['post'] is Map ? data!['post']['idPost'] : null);
    final id = idRaw is num ? idRaw.toInt() : int.tryParse('$idRaw');
    if (id == null || id <= 0) {
      throw const FormatException('O post pode ter sido criado, mas a resposta não informou idPost. '
          'Confira o feed para evitar duplicação.');
    }
    return id;
  }

  // MIME identificado pelos bytes. O Java valida o MultipartFile.getContentType(),
  // então o padrão application/octet-stream do pacote http NÃO funciona.
  static MediaType _tipoImagem(Uint8List bytes) {
    if (bytes.length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF) {
      return MediaType('image', 'jpeg');
    }
    if (bytes.length >= 8 && bytes[0] == 137 && bytes[1] == 80 &&
        bytes[2] == 78 && bytes[3] == 71 && bytes[4] == 13 &&
        bytes[5] == 10 && bytes[6] == 26 && bytes[7] == 10) {
      return MediaType('image', 'png');
    }
    if (bytes.length >= 12 && String.fromCharCodes(bytes.sublist(0, 4)) == 'RIFF' &&
        String.fromCharCodes(bytes.sublist(8, 12)) == 'WEBP') {
      return MediaType('image', 'webp');
    }
    throw const FormatException('Formato de imagem inválido. Use JPG, PNG ou WEBP.');
  }

  Future<void> enviarImagemDoPost({
    required int idPost,
    required Uint8List bytes,
    required String nomeArquivo,
  }) async {
    if (idPost <= 0) throw ArgumentError('idPost inválido');
    if (bytes.isEmpty || bytes.length > 5 * 1024 * 1024) {
      throw const FormatException('A imagem deve ter de 1 byte a 5 MB.');
    }
    final tipo = _tipoImagem(bytes);
    final uri = Uri.parse('$baseUrl$_uploadPath');
    final request = http.MultipartRequest('POST', uri)
      ..fields['idPost'] = idPost.toString();
    request.files.add(http.MultipartFile.fromBytes(
      _uploadField, bytes, filename: nomeArquivo, contentType: tipo));
    final streamed = await request.send().timeout(const Duration(seconds: 35));
    final response = await http.Response.fromStream(streamed);
    if (kDebugMode) {
      debugPrint('UPLOAD POST $idPost -> $uri: HTTP ${response.statusCode} ${_resumo(response.body)}');
    }
    if (response.statusCode != 200 && response.statusCode != 201) {
      throw Exception('Upload da imagem falhou (HTTP ${response.statusCode}) em $_uploadPath. '
          'Campos: idPost=$idPost, imagem ($tipo). Resposta: ${_resumo(response.body)}');
    }
    // O controller real devolve a entidade ImagensPost, contendo idPost e url.
    if (response.body.trim().isNotEmpty) {
      try {
        final data = jsonDecode(utf8.decode(response.bodyBytes));
        if (data is Map && data['url'] != null) {
          if (kDebugMode) debugPrint('Imagem salva no banco: ${data['url']}');
        }
      } catch (_) {
        // HTTP 200/201 já confirmou o upload. Não repetir o upload por
        // erro de parsing e correr o risco de duplicar a foto.
      }
    }
  }

  static String _resumo(String texto) =>
      texto.length > 250 ? '${texto.substring(0, 250)}...' : texto;

  // Mantidos para não quebrar outras telas que já usam esses métodos.
  Future<void> criarPostOuFalhar(Post post) async {
    final response = await http.post(Uri.parse('$baseUrl/posts'),
      headers: {'Content-Type': 'application/json; charset=UTF-8', 'Accept': 'application/json'},
      body: jsonEncode(post.toJson()),
    ).timeout(const Duration(seconds: 20));
    if (response.statusCode != 200 && response.statusCode != 201) {
      throw Exception('Falha ao publicar (HTTP ${response.statusCode}): ${_resumo(response.body)}');
    }
  }

  Future<bool> criarPost(Post post) async {
    try { await criarPostOuFalhar(post); return true; }
    catch (e) { if (kDebugMode) debugPrint('$e'); return false; }
  }

  Future<bool> deletarPost(int idPost, int idUsuario) async {
    try {
      final response = await http.delete(Uri.parse('$baseUrl/posts/$idPost/usuario/$idUsuario'))
          .timeout(const Duration(seconds: 10));
      return response.statusCode == 200 || response.statusCode == 204;
    } catch (e) {
      if (kDebugMode) debugPrint('Deletar post: $e');
      return false;
    }
  }
}
