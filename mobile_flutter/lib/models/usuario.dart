class Usuario {
  int? idUsuario;
  String nome;
  String? nomeCompleto;
  String email;
  String senha;
  String? username;
  String? bio;
  String? dataNascimento;
  String? fotoPerfil;
  String? authToken;

  Usuario({
    this.idUsuario,
    required this.nome,
    this.nomeCompleto,
    required this.email,
    required this.senha,
    this.username,
    this.bio,
    this.dataNascimento,
    this.fotoPerfil,
    this.authToken,
  });

  static int? _id(dynamic value) {
    if (value is num) return value.toInt();
    if (value is String) return int.tryParse(value);
    return null;
  }

  factory Usuario.fromJson(Map<String, dynamic> json) {
    // Aceita DTO simples ou resposta com objeto 'usuario'.
    final dados = json['usuario'] is Map
        ? Map<String, dynamic>.from(json['usuario'] as Map)
        : json;
    return Usuario(
      idUsuario: _id(dados['idUsuario'] ?? dados['id_usuario'] ?? dados['id'] ??
          json['idUsuario'] ?? json['id']),
      nome: (dados['nome'] ?? dados['nomeCompleto'] ?? '').toString(),
      nomeCompleto: (dados['nomeCompleto'] ?? dados['nome_completo'])?.toString(),
      email: (dados['email'] ?? json['email'] ?? '').toString(),
      senha: (dados['senha'] ?? '').toString(),
      username: dados['username']?.toString(),
      bio: dados['bio']?.toString(),
      dataNascimento: (dados['dataNascimento'] ?? dados['data_nascimento'])?.toString(),
      fotoPerfil: (dados['fotoPerfil'] ?? dados['foto_perfil'])?.toString(),
      authToken: (dados['authToken'] ?? json['authToken'])?.toString(),
    );
  }

  Map<String, dynamic> toJson() => {
    'nome': nome,
    'email': email,
    'senha': senha,
    if (idUsuario != null) 'idUsuario': idUsuario,
    if (nomeCompleto != null) 'nomeCompleto': nomeCompleto,
    if (username != null) 'username': username,
    if (bio != null) 'bio': bio,
    if (dataNascimento != null) 'dataNascimento': dataNascimento,
    if (fotoPerfil != null) 'fotoPerfil': fotoPerfil,
  };
}
