class AuthService {
  static bool logado = false;
  static int? idUsuario;
  static String? nomeUsuario;
  static String? emailUsuario;
  static String? username;
  static String? bio;
  static String? fotoPerfil;

  static void fazerLogin({
    required int idUsuario,
    required String nome,
    required String email,
    String? username,
    String? bio,
    String? fotoPerfil,
  }) {
    AuthService.logado = true;
    AuthService.idUsuario = idUsuario;
    AuthService.nomeUsuario = nome;
    AuthService.emailUsuario = email;
    AuthService.username = username;
    AuthService.bio = bio;
    AuthService.fotoPerfil = fotoPerfil;
  }

  static void fazerLogout() {
    logado = false;
    idUsuario = null;
    nomeUsuario = null;
    emailUsuario = null;
    username = null;
    bio = null;
    fotoPerfil = null;
  }
}