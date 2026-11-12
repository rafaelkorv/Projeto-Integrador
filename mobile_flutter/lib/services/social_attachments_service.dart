import 'dart:convert';
import 'dart:typed_data';
import 'authenticated_http.dart' as http;
import 'package:http_parser/http_parser.dart';
import 'api_service.dart';

class SocialAttachmentsService {
  static const imagePrefix = '__SJ_IMAGE__:';
  static bool ehImagem(String texto) => texto.startsWith(imagePrefix);
  static String? imagemDaMensagem(String texto) => ehImagem(texto)
      ? ApiService.formatarUrlMidia(texto.substring(imagePrefix.length)) : null;

  static MediaType _tipo(Uint8List b) {
    if (b.length > 2 && b[0] == 255 && b[1] == 216 && b[2] == 255) {
      return MediaType('image', 'jpeg');
    }
    if (b.length > 7 && b[0] == 137 && b[1] == 80 && b[2] == 78 && b[3] == 71) {
      return MediaType('image', 'png');
    }
    if (b.length > 11 && String.fromCharCodes(b.sublist(0, 4)) == 'RIFF' &&
        String.fromCharCodes(b.sublist(8, 12)) == 'WEBP') {
      return MediaType('image', 'webp');
    }
    throw const FormatException('Use imagens JPG, PNG ou WEBP.');
  }

  Future<String> upload({required String tipo, required int id,
    required int usuarioId, required Uint8List bytes,
    required String nome}) async {
    if (bytes.isEmpty || bytes.length > 5 * 1024 * 1024) {
      throw const FormatException('A imagem deve ter no máximo 5 MB.');
    }
    if (tipo != 'chat' && tipo != 'comentarios') throw ArgumentError('tipo inválido');
    final path = tipo == 'chat' ? 'chat' : 'comentarios';
    final uri = Uri.parse('${ApiService.baseUrl}/api/socialjoin-attachments/$path/$id/$usuarioId');
    final request = http.MultipartRequest('POST', uri);
    request.files.add(http.MultipartFile.fromBytes('imagem', bytes,
      filename: nome, contentType: _tipo(bytes)));
    final result = await http.Response.fromStream(await request.send()
        .timeout(const Duration(seconds: 35)));
    if (result.statusCode != 200 && result.statusCode != 201) {
      throw Exception('Imagem não enviada (HTTP ${result.statusCode}): ${result.body}');
    }
    final json = jsonDecode(utf8.decode(result.bodyBytes));
    if (json is! Map || json['url'] is! String) {
      throw const FormatException('Upload sem URL');
    }
    return json['url'] as String;
  }

  Future<Map<int, List<String>>> listarImagensComentarios() async {
    final res = await http.get(Uri.parse('${ApiService.baseUrl}/api/socialjoin-attachments/comentarios'))
        .timeout(const Duration(seconds: 15));
    if (res.statusCode != 200) return {};
    final data = jsonDecode(utf8.decode(res.bodyBytes));
    final saida = <int, List<String>>{};
    if (data is List) {
      for (final x in data.whereType<Map>()) {
        final id = int.tryParse('${x['idComentario']}');
        final url = x['url']?.toString();
        if (id != null && url != null) saida.putIfAbsent(id, () => []).add(url);
      }
    }
    return saida;
  }
}
