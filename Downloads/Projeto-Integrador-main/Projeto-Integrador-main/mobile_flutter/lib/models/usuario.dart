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
  });

  factory Usuario.fromJson(Map<String, dynamic> json) {
    return Usuario(
      idUsuario: json['idUsuario'] ?? json['id_usuario'],
      nome: json['nome'] ?? '',
      nomeCompleto: json['nomeCompleto'] ?? json['nome_completo'],
      email: json['email'] ?? '',
      senha: json['senha'] ?? '',
      username: json['username'],
      bio: json['bio'],
      dataNascimento: json['dataNascimento'] ?? json['data_nascimento'],
      fotoPerfil: json['fotoPerfil'] ?? json['foto_perfil'],
    );
  }

  Map<String, dynamic> toJson() {
    final map = <String, dynamic>{
      'nome': nome,
      'email': email,
      'senha': senha,
    };
    if (idUsuario != null) map['idUsuario'] = idUsuario;
    if (nomeCompleto != null) map['nomeCompleto'] = nomeCompleto;
    if (username != null) map['username'] = username;
    if (bio != null) map['bio'] = bio;
    if (dataNascimento != null) map['dataNascimento'] = dataNascimento;
    if (fotoPerfil != null) map['fotoPerfil'] = fotoPerfil;
    return map;
  }
}