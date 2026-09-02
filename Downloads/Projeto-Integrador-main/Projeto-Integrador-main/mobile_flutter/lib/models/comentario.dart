// lib/models/comentario.dart

class Comentario {
  int? idComentario;
  String conteudo;
  int idUsuario;
  int idPost;

  Comentario({
    this.idComentario,
    required this.conteudo,
    required this.idUsuario,
    required this.idPost,
  });

  factory Comentario.fromJson(Map<String, dynamic> json) {
    return Comentario(
      idComentario: json['idComentario'] ?? json['id_comentario'],
      conteudo: json['conteudo'] ?? '',
      idUsuario: json['idUsuario'] ?? json['id_usuario'],
      idPost: json['idPost'] ?? json['id_post'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      "idComentario": idComentario,
      "conteudo": conteudo,
      "idUsuario": idUsuario,
      "idPost": idPost,
    };
  }
}