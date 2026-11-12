import 'dart:async';

import 'package:flutter/foundation.dart';
import 'package:http/http.dart' as http;

import 'api_service.dart';

class AuthService {
  static bool logado = false;
  static int? idUsuario;
  static String? nomeUsuario;
  static String? emailUsuario;
  static String? username;
  static String? bio;
  static String? fotoPerfil;
  static String? authToken;

  static void fazerLogin({
    required int idUsuario,
    required String nome,
    required String email,
    String? username,
    String? bio,
    String? fotoPerfil,
    required String? authToken,
  }) {
    AuthService.logado = true;
    AuthService.idUsuario = idUsuario;
    AuthService.nomeUsuario = nome;
    AuthService.emailUsuario = email;
    AuthService.username = username;
    AuthService.bio = bio;
    AuthService.fotoPerfil = fotoPerfil;
    AuthService.authToken = authToken;
  }

  static void fazerLogout() {
    final token = authToken;
    if (token != null) {
      unawaited(http.post(
        Uri.parse('${ApiService.baseUrl}/auth/logout'),
        headers: {'Authorization': 'Bearer $token'},
      ).then<void>((_) {}).catchError((Object erro) {
        if (kDebugMode) debugPrint('Não foi possível encerrar a sessão no servidor: $erro');
      }));
    }
    logado = false;
    idUsuario = null;
    nomeUsuario = null;
    emailUsuario = null;
    username = null;
    bio = null;
    fotoPerfil = null;
    authToken = null;
  }
}