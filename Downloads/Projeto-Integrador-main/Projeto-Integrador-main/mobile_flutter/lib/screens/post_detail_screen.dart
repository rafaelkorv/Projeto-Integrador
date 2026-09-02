import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import '../models/post.dart';
import '../models/comentario.dart';
import '../services/auth_service.dart';
import '../services/comentario_service.dart';

/// Tela de Detalhes da Publicação & Comentários Mobile — Fiel à Estilização Real da Tela WEB (.post-detail-card, .post-comments-heading, .detail-comment-box, .detail-comment).
/// Fonte da Verdade: web/style.css
class PostDetailScreen extends StatefulWidget {
  final Post post;
  final String nomeAutor;
  final Map<int, String> nomesUsuarios;

  const PostDetailScreen({
    super.key,
    required this.post,
    required this.nomeAutor,
    required this.nomesUsuarios,
  });

  @override
  State<PostDetailScreen> createState() => _PostDetailScreenState();
}

class _PostDetailScreenState extends State<PostDetailScreen> {
  final ComentarioService comentarioService = ComentarioService();
  final TextEditingController controller = TextEditingController();
  
  List<Comentario> comentarios = [];
  bool carregando = true;

  @override
  void initState() {
    super.initState();
    carregarComentarios();
  }

  Future<void> carregarComentarios() async {
    setState(() => carregando = true);
    try {
      final idPost = widget.post.idPost;
      final lista = idPost != null
          ? await comentarioService.listarPorPost(idPost)
          : await comentarioService.listarComentarios();

      if (mounted) {
        setState(() {
          comentarios = lista;
          carregando = false;
        });
      }
    } catch (e) {
      if (mounted) {
        setState(() => carregando = false);
      }
    }
  }

  Future<void> enviarComentario() async {
    if (controller.text.trim().isEmpty) return;
    
    if (!AuthService.logado || AuthService.idUsuario == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Você precisa fazer login para comentar!', style: GoogleFonts.manrope())),
      );
      return;
    }

    final novo = Comentario(
      conteudo: controller.text,
      idUsuario: AuthService.idUsuario!,
      idPost: widget.post.idPost ?? 0,
    );

    setState(() => carregando = true);
    final sucesso = await comentarioService.criarComentario(novo);
    
    if (!mounted) return;

    if (sucesso) {
      controller.clear();
      FocusScope.of(context).unfocus();
      await carregarComentarios();
    } else {
      setState(() => carregando = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final inicialAutor = widget.nomeAutor.isNotEmpty ? widget.nomeAutor[0].toUpperCase() : '?';

    return Scaffold(
      backgroundColor: const Color(0xFFF7F8FA),
      appBar: AppBar(
        title: Text("Publicação", style: GoogleFonts.manrope(fontWeight: FontWeight.w700, fontSize: 18)),
        elevation: 0,
        backgroundColor: Colors.white,
      ),
      body: Column(
        children: [
          Expanded(
            child: SingleChildScrollView(
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // POST DETAIL CARD Fiel à Web (.post-detail-card)
                  Container(
                    decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.circular(16),
                      border: Border.all(color: const Color(0xFFE5E7EB)),
                      boxShadow: const [
                        BoxShadow(
                          color: Color.fromRGBO(17, 24, 39, 0.07),
                          blurRadius: 30,
                          offset: Offset(0, 12),
                        ),
                      ],
                    ),
                    child: Padding(
                      padding: const EdgeInsets.all(24),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Row(
                            children: [
                              CircleAvatar(
                                radius: 20,
                                backgroundColor: const Color(0xFFEA3F74),
                                child: Text(
                                  inicialAutor,
                                  style: GoogleFonts.manrope(
                                    color: Colors.white,
                                    fontWeight: FontWeight.bold,
                                    fontSize: 16,
                                  ),
                                ),
                              ),
                              const SizedBox(width: 12),
                              Expanded(
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      widget.nomeAutor,
                                      style: GoogleFonts.manrope(fontSize: 15, fontWeight: FontWeight.w700, color: const Color(0xFF202124)),
                                    ),
                                    Text('Autor da publicação', style: GoogleFonts.manrope(fontSize: 12, color: const Color(0xFF6B7280))),
                                  ],
                                ),
                              ),
                            ],
                          ),
                          const SizedBox(height: 20),
                          Text(
                            widget.post.titulo,
                            style: GoogleFonts.manrope(fontSize: 22, fontWeight: FontWeight.w700, color: const Color(0xFF202124)),
                          ),
                          const SizedBox(height: 12),
                          Text(
                            widget.post.conteudo,
                            style: GoogleFonts.manrope(fontSize: 15, color: const Color(0xFF202124), height: 1.65),
                          ),
                        ],
                      ),
                    ),
                  ),
                  
                  const SizedBox(height: 24),

                  // SEÇÃO DE COMENTÁRIOS Fiel à Web (.post-comments-section & .post-comments-heading)
                  Row(
                    children: [
                      Text(
                        "Comentários",
                        style: GoogleFonts.manrope(fontWeight: FontWeight.w700, fontSize: 19, color: const Color(0xFF202124)),
                      ),
                      const SizedBox(width: 8),
                      Container(
                        width: 24,
                        height: 24,
                        decoration: const BoxDecoration(
                          color: Color(0xFFF7F8FA),
                          shape: BoxShape.circle,
                        ),
                        child: Center(
                          child: Text(
                            '${comentarios.length}',
                            style: GoogleFonts.manrope(
                              color: const Color(0xFFEA3F74),
                              fontSize: 11,
                              fontWeight: FontWeight.w800,
                            ),
                          ),
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 16),
                  
                  carregando
                      ? const Center(child: Padding(padding: EdgeInsets.all(20), child: CircularProgressIndicator(color: Color(0xFFEA3F74))))
                      : comentarios.isEmpty
                          ? Container(
                              width: double.infinity,
                              padding: const EdgeInsets.all(20),
                              decoration: BoxDecoration(
                                color: Colors.white,
                                borderRadius: BorderRadius.circular(14),
                                border: Border.all(color: const Color(0xFFE5E7EB)),
                              ),
                              child: Text(
                                "Ainda não há comentários. Seja o primeiro a comentar!",
                                textAlign: TextAlign.center,
                                style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 13),
                              ),
                            )
                          : ListView.builder(
                              shrinkWrap: true,
                              physics: const NeverScrollableScrollPhysics(),
                              itemCount: comentarios.length,
                              itemBuilder: (context, index) {
                                final c = comentarios[index];
                                final nome = widget.nomesUsuarios[c.idUsuario] ?? "Desconhecido";
                                final inicial = nome.isNotEmpty ? nome[0].toUpperCase() : '?';

                                return Container(
                                  padding: const EdgeInsets.symmetric(vertical: 14),
                                  decoration: const BoxDecoration(
                                    border: Border(
                                      top: BorderSide(color: Color(0xFFE5E7EB), width: 1.0),
                                    ),
                                  ),
                                  child: Row(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: [
                                      CircleAvatar(
                                        radius: 16,
                                        backgroundColor: const Color(0xFFEA3F74),
                                        child: Text(
                                          inicial,
                                          style: GoogleFonts.manrope(
                                            color: Colors.white,
                                            fontSize: 12,
                                            fontWeight: FontWeight.bold,
                                          ),
                                        ),
                                      ),
                                      const SizedBox(width: 12),
                                      Expanded(
                                        child: Column(
                                          crossAxisAlignment: CrossAxisAlignment.start,
                                          children: [
                                            Text(
                                              nome,
                                              style: GoogleFonts.manrope(
                                                fontSize: 13,
                                                fontWeight: FontWeight.w700,
                                                color: const Color(0xFF202124),
                                              ),
                                            ),
                                            const SizedBox(height: 4),
                                            Text(
                                              c.conteudo,
                                              style: GoogleFonts.manrope(
                                                fontSize: 13,
                                                color: const Color(0xFF6B7280),
                                                height: 1.5,
                                              ),
                                            ),
                                          ],
                                        ),
                                      ),
                                    ],
                                  ),
                                );
                              },
                            ),
                ],
              ),
            ),
          ),
          
          // BARRA FIXA DE COMENTÁRIO (.detail-comment-box da Web CSS)
          Container(
            padding: EdgeInsets.only(
              left: 16, right: 16, top: 12,
              bottom: MediaQuery.of(context).padding.bottom + 12,
            ),
            decoration: const BoxDecoration(
              color: Colors.white,
              border: Border(top: BorderSide(color: Color(0xFFE5E7EB), width: 1.0)),
            ),
            child: Row(
              children: [
                Expanded(
                  child: TextField(
                    controller: controller,
                    style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF202124)),
                    decoration: InputDecoration(
                      hintText: "Escreva um comentário...",
                      hintStyle: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 13),
                      border: OutlineInputBorder(borderRadius: BorderRadius.circular(9), borderSide: const BorderSide(color: Color(0xFFE5E7EB))),
                      enabledBorder: OutlineInputBorder(borderRadius: BorderRadius.circular(9), borderSide: const BorderSide(color: Color(0xFFE5E7EB))),
                      focusedBorder: OutlineInputBorder(borderRadius: BorderRadius.circular(9), borderSide: const BorderSide(color: Color(0xFFEA3F74), width: 1.5)),
                      filled: true,
                      fillColor: Colors.white,
                      contentPadding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
                    ),
                  ),
                ),
                const SizedBox(width: 8),
                SizedBox(
                  height: 44,
                  child: ElevatedButton(
                    onPressed: enviarComentario,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: const Color(0xFFEA3F74),
                      foregroundColor: Colors.white,
                      elevation: 0,
                      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(9)),
                      padding: const EdgeInsets.symmetric(horizontal: 16),
                    ),
                    child: Text('Enviar', style: GoogleFonts.manrope(fontWeight: FontWeight.w800, fontSize: 13)),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}