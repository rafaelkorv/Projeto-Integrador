class MembroComunidade {
  final int? idUsuario;
  final String nome;
  final String? username;

  MembroComunidade({
    this.idUsuario,
    required this.nome,
    this.username,
  });

  factory MembroComunidade.fromJson(Map<String, dynamic> json) {
    return MembroComunidade(
      idUsuario: json['idUsuario'] ?? json['id_usuario'],
      nome: json['nome'] ?? '',
      username: json['username'],
    );
  }
}

class Comunidade {
  final int id;
  final String nome;
  final String? descricao;
  final MembroComunidade? criador;
  final List<MembroComunidade> membros;

  Comunidade({
    required this.id,
    required this.nome,
    this.descricao,
    this.criador,
    this.membros = const [],
  });

  int get totalMembros => membros.length;

  bool isMembro(int? idUsuario) {
    if (idUsuario == null) return false;
    return membros.any((m) => m.idUsuario == idUsuario);
  }

  factory Comunidade.fromJson(Map<String, dynamic> json) {
    final membrosJson = json['membros'] as List<dynamic>? ?? [];
    final membros = membrosJson
        .map((m) => MembroComunidade.fromJson(m as Map<String, dynamic>))
        .toList();

    MembroComunidade? criador;
    if (json['criador'] != null) {
      criador = MembroComunidade.fromJson(
          json['criador'] as Map<String, dynamic>);
    }

    return Comunidade(
      id: json['id'] ?? 0,
      nome: json['nome'] ?? '',
      descricao: json['descricao'],
      criador: criador,
      membros: membros,
    );
  }
}
