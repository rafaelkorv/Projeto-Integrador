import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import '../models/comunidade.dart';
import 'api_service.dart';

class ComunidadeService {
  String get baseUrl => ApiService.baseUrl;

  /// Lista todas as comunidades do banco
  Future<List<Comunidade>> listarComunidades() async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/comunidades'),
      ).timeout(const Duration(seconds: 8));

      if (response.statusCode == 200) {
        final List lista =
            jsonDecode(utf8.decode(response.bodyBytes));
        return lista.map((e) => Comunidade.fromJson(e)).toList();
      }
    } catch (e) {
      if (kDebugMode) print('Erro ao listar comunidades: $e');
    }
    return [];
  }

  /// Lista comunidades em que o usuário é membro ou criador
  Future<List<Comunidade>> listarComunidadesPorUsuario(int idUsuario) async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/comunidades/usuario/$idUsuario'),
      ).timeout(const Duration(seconds: 8));

      if (response.statusCode == 200) {
        final List lista =
            jsonDecode(utf8.decode(response.bodyBytes));
        return lista.map((e) => Comunidade.fromJson(e)).toList();
      }
    } catch (e) {
      if (kDebugMode) print('Erro ao listar comunidades do usuário: $e');
    }
    return [];
  }

  /// Cria uma nova comunidade no backend
  Future<Comunidade?> criarComunidade({
    required String nome,
    required String descricao,
    required int criadorId,
  }) async {
    try {
      final response = await http.post(
        Uri.parse('$baseUrl/comunidades'),
        headers: {'Content-Type': 'application/json'},
        body: jsonEncode({
          'nome': nome,
          'descricao': descricao,
          'criadorId': criadorId,
        }),
      ).timeout(const Duration(seconds: 10));

      if (response.statusCode == 200 || response.statusCode == 201) {
        return Comunidade.fromJson(
            jsonDecode(utf8.decode(response.bodyBytes)));
      }
    } catch (e) {
      if (kDebugMode) print('Erro ao criar comunidade: $e');
    }
    return null;
  }

  /// Adiciona o usuário como membro da comunidade
  Future<bool> participarComunidade(
      int idComunidade, int idUsuario) async {
    try {
      final response = await http.post(
        Uri.parse(
            '$baseUrl/comunidades/$idComunidade/participar/$idUsuario'),
      ).timeout(const Duration(seconds: 8));

      return response.statusCode == 200 || response.statusCode == 201;
    } catch (e) {
      if (kDebugMode) print('Erro ao participar da comunidade: $e');
      return false;
    }
  }

  /// Remove o usuário como membro da comunidade
  Future<bool> sairComunidade(
      int idComunidade, int idMembro, int idSolicitante) async {
    try {
      final response = await http.delete(
        Uri.parse(
            '$baseUrl/comunidades/$idComunidade/membros/$idMembro/usuario/$idSolicitante'),
      ).timeout(const Duration(seconds: 8));

      return response.statusCode == 200 || response.statusCode == 204;
    } catch (e) {
      if (kDebugMode) print('Erro ao sair da comunidade: $e');
      return false;
    }
  }
}
