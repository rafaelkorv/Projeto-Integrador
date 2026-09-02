import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import '../models/evento.dart';
import 'api_service.dart';

class EventoService {
  String get baseUrl => ApiService.baseUrl;

  /// Lista todos os eventos do banco com resiliência total a respostas List [...] ou Page { "content": [...] }
  /// de ambos os endpoints da API (/api/eventos e /api/eventos/buscar).
  Future<List<Evento>> listarEventos() async {
    // 1. Primeira tentativa: GET /api/eventos
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/api/eventos'),
      ).timeout(const Duration(seconds: 8));

      if (response.statusCode == 200) {
        final decoded = jsonDecode(utf8.decode(response.bodyBytes));
        List listaData = [];
        if (decoded is List) {
          listaData = decoded;
        } else if (decoded is Map && decoded['content'] is List) {
          listaData = decoded['content'] as List;
        }

        if (listaData.isNotEmpty) {
          return listaData
              .whereType<Map<String, dynamic>>()
              .map((e) => Evento.fromJson(e))
              .toList();
        }
      }
    } catch (e) {
      if (kDebugMode) print('Aviso em /api/eventos: $e');
    }

    // 2. Segunda tentativa: GET /api/eventos/buscar (Endpoint utilizado pela Web)
    try {
      final responseBusca = await http.get(
        Uri.parse('$baseUrl/api/eventos/buscar?size=100'),
      ).timeout(const Duration(seconds: 8));

      if (responseBusca.statusCode == 200) {
        final decoded = jsonDecode(utf8.decode(responseBusca.bodyBytes));
        List listaData = [];
        if (decoded is List) {
          listaData = decoded;
        } else if (decoded is Map && decoded['content'] is List) {
          listaData = decoded['content'] as List;
        }

        return listaData
            .whereType<Map<String, dynamic>>()
            .map((e) => Evento.fromJson(e))
            .toList();
      }
    } catch (e) {
      if (kDebugMode) print('Aviso em /api/eventos/buscar: $e');
    }

    return [];
  }

  /// Lista eventos criados pelo usuário
  Future<List<Evento>> listarEventosPorUsuario(int idUsuario) async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/api/eventos/usuario/$idUsuario'),
      ).timeout(const Duration(seconds: 8));

      if (response.statusCode == 200) {
        final decoded = jsonDecode(utf8.decode(response.bodyBytes));
        List listaData = [];
        if (decoded is List) {
          listaData = decoded;
        } else if (decoded is Map && decoded['content'] is List) {
          listaData = decoded['content'] as List;
        }

        return listaData
            .whereType<Map<String, dynamic>>()
            .map((e) => Evento.fromJson(e))
            .toList();
      }
    } catch (e) {
      if (kDebugMode) print('Erro ao listar eventos do usuário: $e');
    }
    return [];
  }

  /// Busca evento por ID
  Future<Evento?> buscarEventoPorId(int id) async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/api/eventos/$id'),
      ).timeout(const Duration(seconds: 8));

      if (response.statusCode == 200) {
        final decoded = jsonDecode(utf8.decode(response.bodyBytes));
        if (decoded is Map<String, dynamic>) {
          return Evento.fromJson(decoded);
        }
      }
    } catch (e) {
      if (kDebugMode) print('Erro ao buscar evento por id: $e');
    }
    return null;
  }

  /// Cria um novo evento no backend
  Future<Evento?> criarEvento(Map<String, dynamic> dados) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/api/eventos'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode(dados),
      ).timeout(const Duration(seconds: 10));

      if (response.statusCode == 200 || response.statusCode == 201) {
        final decoded = jsonDecode(utf8.decode(response.bodyBytes));
        if (decoded is Map<String, dynamic>) {
          return Evento.fromJson(decoded);
        }
      }
    } catch (e) {
      if (kDebugMode) print('Erro ao criar evento: $e');
    }
    return null;
  }

  /// Inscreve o usuário no evento
  Future<bool> participarEvento(int idEvento, int idUsuario) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/api/eventos/$idEvento/participar/$idUsuario'),
      ).timeout(const Duration(seconds: 8));

      return response.statusCode == 200 || response.statusCode == 201;
    } catch (e) {
      if (kDebugMode) print('Erro ao participar do evento: $e');
      return false;
    }
  }

  /// Remove a inscrição do usuário no evento
  Future<bool> sairEvento(int idEvento, int idUsuario) async {
    try {
      final response = await http.delete(
        Uri.parse('$baseUrl/api/eventos/$idEvento/participar/$idUsuario'),
      ).timeout(const Duration(seconds: 8));

      return response.statusCode == 200 || response.statusCode == 204;
    } catch (e) {
      if (kDebugMode) print('Erro ao sair do evento: $e');
      return false;
    }
  }

  /// Busca quantidade de participantes de um evento
  Future<int> contarParticipantes(int idEvento) async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/api/eventos/$idEvento/participantes/quantidade'),
      ).timeout(const Duration(seconds: 6));

      if (response.statusCode == 200) {
        return int.tryParse(response.body) ?? 0;
      }
    } catch (e) {
      if (kDebugMode) print('Erro ao contar participantes: $e');
    }
    return 0;
  }

  /// Lista IDs de usuários participantes do evento
  Future<List<int>> listarIdsParticipantes(int idEvento) async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/api/eventos/$idEvento/participantes'),
      ).timeout(const Duration(seconds: 6));

      if (response.statusCode == 200) {
        final decoded = jsonDecode(utf8.decode(response.bodyBytes));
        if (decoded is List) {
          return decoded
              .whereType<Map<String, dynamic>>()
              .map<int>((item) => (item['usuarioId'] ?? item['id'] as num?)?.toInt() ?? 0)
              .where((id) => id > 0)
              .toList();
        }
      }
    } catch (e) {
      if (kDebugMode) print('Erro ao listar participantes: $e');
    }
    return [];
  }
}
