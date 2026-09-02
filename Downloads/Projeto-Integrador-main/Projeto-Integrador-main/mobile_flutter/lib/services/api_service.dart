class ApiService {
  static const String baseUrl = 'http://localhost:8080';

  //PC do COTIL:
  //static const String baseUrl = 'http://143.106.241.xx:8080';

  //API Render:
  //static const String baseUrl = 'https://projeto-integrador-m4jn.onrender.com';

  //Só comentar a primeira linha e descomentar a que for usar

  static String usuarios = '$baseUrl/usuarios';
  static String posts = '$baseUrl/posts';
  static String comentarios = '$baseUrl/comentarios';
  static String comunidades = '$baseUrl/comunidades';
  static String eventos = '$baseUrl/api/eventos';

  /// Formata URL de foto de perfil (Google Drive ou caminho local)
  static String? formatarUrlFotoPerfil(String? fotoPerfil) {
    if (fotoPerfil == null || fotoPerfil.trim().isEmpty) return null;
    final foto = fotoPerfil.trim();
    if (foto.startsWith('http://') || foto.startsWith('https://')) {
      return foto;
    }
    if (foto.startsWith('uploads/')) {
      return '$baseUrl/$foto';
    }
    if (foto.startsWith('/google/drive/image/') || foto.startsWith('google/drive/image/')) {
      final limpo = foto.startsWith('/') ? foto.substring(1) : foto;
      return '$baseUrl/$limpo';
    }
    // fileId do Google Drive
    return '$baseUrl/google/drive/image/$foto';
  }
}
