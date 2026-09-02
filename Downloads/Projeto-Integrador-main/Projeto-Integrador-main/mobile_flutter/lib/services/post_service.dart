import 'dart:convert';
import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;
import '../models/post.dart';
import 'api_service.dart';

class PostService {
  String get baseUrl => ApiService.baseUrl;

  Future<List<Post>> listarPosts({int? page, int size = 10}) async {
    try {
      final uri = (page != null)
          ? Uri.parse('$baseUrl/posts?page=$page&size=$size')
          : Uri.parse('$baseUrl/posts');

      final response = await http.get(uri).timeout(const Duration(seconds: 8));

      if (response.statusCode == 200) {
        final dynamic data = jsonDecode(utf8.decode(response.bodyBytes));
        final List lista = (data is List) ? data : (data['content'] ?? []);
        return lista.map((e) => Post.fromJson(e)).toList();
      }
    } catch (e) {
      if (kDebugMode) print('Erro ao listar posts: $e');
    }
    return [];
  }

  Future<List<Post>> listarPostsPorUsuario(int idUsuario) async {
    try {
      final response = await http.get(
        Uri.parse('$baseUrl/posts/usuario/$idUsuario'),
      ).timeout(const Duration(seconds: 8));

      if (response.statusCode == 200) {
        final List lista = jsonDecode(utf8.decode(response.bodyBytes));
        return lista.map((e) => Post.fromJson(e)).toList();
      }
    } catch (e) {
      if (kDebugMode) print('Erro ao listar posts do usuário: $e');
    }
    return [];
  }

  Future<bool> criarPost(Post post) async {
    try {
      final corpoJson = jsonEncode(post.toJson());

      final response = await http.post(
        Uri.parse('$baseUrl/posts'),
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          'Accept': 'application/json',
        },
        body: corpoJson,
      ).timeout(const Duration(seconds: 10));

      if (response.statusCode == 200 || response.statusCode == 201) {
        return true;
      } else {
        return false;
      }
    } catch (e) {
      if (kDebugMode) print('Erro ao criar post: $e');
      return false;
    }
  }

  Future<bool> deletarPost(int idPost, int idUsuario) async {
    try {
      final response = await http.delete(
        Uri.parse('$baseUrl/posts/$idPost/usuario/$idUsuario'),
      ).timeout(const Duration(seconds: 8));

      return response.statusCode == 200 || response.statusCode == 204;
    } catch (e) {
      if (kDebugMode) print('Erro ao deletar post: $e');
      return false;
    }
  }
}