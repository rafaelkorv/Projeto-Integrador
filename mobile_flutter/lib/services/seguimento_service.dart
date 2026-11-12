import 'dart:convert';
import 'authenticated_http.dart' as http;
import 'api_service.dart';

class ResumoSeguimentos {
  final int seguidores;
  final int seguindo;
  final bool euSigo;

  const ResumoSeguimentos({required this.seguidores, required this.seguindo, required this.euSigo});

  factory ResumoSeguimentos.fromJson(Map<String, dynamic> json) {
    int numero(Object? value) => (value as num?)?.toInt() ?? 0;
    return ResumoSeguimentos(
      seguidores: numero(json['seguidores']),
      seguindo: numero(json['seguindo']),
      euSigo: json['euSigo'] == true,
    );
  }
}

/// API compartilhada de seguidores. Não modifica endpoints antigos.
class SeguimentoService {
  Uri _url(String caminho, [Map<String, String>? parametros]) =>
      Uri.parse('${ApiService.baseUrl}$caminho').replace(queryParameters: parametros);

  Future<ResumoSeguimentos> resumo(int idUsuario, {int? solicitanteId}) async {
    final response = await http.get(_url('/seguimentos/usuario/$idUsuario/resumo',
        solicitanteId == null ? null : {'solicitanteId': '$solicitanteId'}))
        .timeout(const Duration(seconds: 12));
    if (response.statusCode != 200) {
      throw Exception('Não foi possível consultar seguidores (HTTP ${response.statusCode}).');
    }
    return ResumoSeguimentos.fromJson(Map<String, dynamic>.from(jsonDecode(utf8.decode(response.bodyBytes)) as Map));
  }

  Future<List<int>> ids(int idUsuario, {required bool seguidores}) async {
    final tipo = seguidores ? 'seguidores' : 'seguindo';
    final response = await http.get(_url('/seguimentos/usuario/$idUsuario/$tipo'))
        .timeout(const Duration(seconds: 12));
    if (response.statusCode != 200) {
      throw Exception('Não foi possível consultar $tipo (HTTP ${response.statusCode}).');
    }
    final dados = jsonDecode(utf8.decode(response.bodyBytes));
    return (dados as List).map((e) => (e as num).toInt()).toList();
  }

  Future<ResumoSeguimentos> alterar({required int meuId, required int outroId, required bool seguir}) async {
    final uri = _url('/seguimentos/usuario/$meuId/seguir/$outroId');
    final response = await (seguir ? http.post(uri) : http.delete(uri))
        .timeout(const Duration(seconds: 12));
    if (response.statusCode != 200) {
      throw Exception('Não foi possível ${seguir ? 'seguir' : 'deixar de seguir'} (HTTP ${response.statusCode}).');
    }
    return ResumoSeguimentos.fromJson(Map<String, dynamic>.from(jsonDecode(utf8.decode(response.bodyBytes)) as Map));
  }
}
