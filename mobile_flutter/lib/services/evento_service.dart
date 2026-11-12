import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'authenticated_http.dart' as http;
import '../models/evento.dart';
import 'api_service.dart';

class EventoService {
  String get baseUrl => ApiService.baseUrl;

  /// Ao falhar, NÃO retorna lista vazia: a interface precisa distinguir
  /// "nenhum evento" de "servidor desligado/CORS".
  Future<List<Evento>> listarEventos() => _listar('$baseUrl/api/eventos');
  Future<List<Evento>> listarEventosPorUsuario(int idUsuario) =>
      _listar('$baseUrl/api/eventos/usuario/$idUsuario');

  Future<List<Evento>> _listar(String url) async {
    try {
      final response = await http.get(Uri.parse(url)).timeout(const Duration(seconds: 15));
      if (response.statusCode != 200) {
        throw Exception('HTTP ${response.statusCode} em $url');
      }
      final decoded = jsonDecode(utf8.decode(response.bodyBytes));
      final dynamic lista = decoded is List ? decoded :
          decoded is Map ? decoded['content'] : null;
      if (lista is! List) throw const FormatException('Esperava lista de eventos da API.');
      return lista.whereType<Map>().map((e) =>
          Evento.fromJson(Map<String, dynamic>.from(e))).toList();
    } catch (e) {
      if (kDebugMode) debugPrint('ERRO LISTAR EVENTOS ($url): $e');
      rethrow;
    }
  }

  Future<Evento?> buscarEventoPorId(int id) async {
    try {
      final response = await http.get(Uri.parse('$baseUrl/api/eventos/$id'))
          .timeout(const Duration(seconds: 15));
      if (response.statusCode == 200) {
        final data = jsonDecode(utf8.decode(response.bodyBytes));
        if (data is Map) return Evento.fromJson(Map<String, dynamic>.from(data));
      }
    } catch (e) { if (kDebugMode) debugPrint('Erro ao buscar evento: $e'); }
    return null;
  }

  Future<Evento?> criarEvento(Map<String, dynamic> dados) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/api/eventos'),
        headers: {'Content-Type': 'application/json; charset=UTF-8'},
        body: jsonEncode(dados),
      ).timeout(const Duration(seconds: 15));
      if (response.statusCode == 200 || response.statusCode == 201) {
        final data = jsonDecode(utf8.decode(response.bodyBytes));
        if (data is Map) return Evento.fromJson(Map<String, dynamic>.from(data));
      }
      if (kDebugMode) debugPrint('CRIAR EVENTO: ${response.statusCode} ${response.body}');
    } catch (e) { if (kDebugMode) debugPrint('Erro criando evento: $e'); }
    return null;
  }

  Future<bool> participarEvento(int idEvento, int idUsuario) async {
    try {
      final response = await http.post(Uri.parse(
        '$baseUrl/api/eventos/$idEvento/participar/$idUsuario',
      )).timeout(const Duration(seconds: 10));
      if (kDebugMode) debugPrint('PARTICIPAR evento $idEvento: HTTP ${response.statusCode} - ${response.body}');
      if ([200, 201, 204].contains(response.statusCode)) return true;
      if (response.statusCode == 409) {
        final ids = await listarIdsParticipantes(idEvento);
        return ids.contains(idUsuario); // Apenas se o servidor confirmar a inscrição.
      }
      return false;
    } catch (e) { if (kDebugMode) debugPrint('Erro ao participar: $e'); return false; }
  }

  Future<bool> sairEvento(int idEvento, int idUsuario) async {
    try {
      final response = await http.delete(Uri.parse(
        '$baseUrl/api/eventos/$idEvento/participar/$idUsuario',
      )).timeout(const Duration(seconds: 10));
      if (kDebugMode) debugPrint('SAIR evento $idEvento: HTTP ${response.statusCode} - ${response.body}');
      return [200, 204].contains(response.statusCode);
    } catch (e) { if (kDebugMode) debugPrint('Erro ao sair do evento: $e'); return false; }
  }

  Future<int> contarParticipantes(int idEvento) async {
    try {
      final response = await http.get(Uri.parse(
        '$baseUrl/api/eventos/$idEvento/participantes/quantidade',
      )).timeout(const Duration(seconds: 8));
      if (response.statusCode != 200) return 0;
      final body = utf8.decode(response.bodyBytes).trim();
      final direto = int.tryParse(body);
      if (direto != null) return direto;
      final data = jsonDecode(body);
      if (data is num) return data.toInt();
      if (data is Map) {
        final count = data['quantidade'] ?? data['total'] ?? data['count'];
        return count is num ? count.toInt() : int.tryParse('$count') ?? 0;
      }
    } catch (e) { if (kDebugMode) debugPrint('Erro ao contar participantes: $e'); }
    return 0;
  }

  Future<List<int>> listarIdsParticipantes(int idEvento) async {
    try {
      final response = await http.get(Uri.parse(
        '$baseUrl/api/eventos/$idEvento/participantes',
      )).timeout(const Duration(seconds: 8));
      if (response.statusCode != 200) return [];
      final data = jsonDecode(utf8.decode(response.bodyBytes));
      if (data is! List) return [];
      return data.map((e) {
        final dynamic valor = e is Map
            ? e['usuarioId'] ?? e['idUsuario'] ?? e['id_usuario'] ?? e['id']
            : e;
        if (valor is num) return valor.toInt();
        return int.tryParse('$valor') ?? 0;
      }).where((v) => v > 0).toList();
    } catch (e) { if (kDebugMode) debugPrint('Erro ao listar participantes: $e'); }
    return [];
  }
}
