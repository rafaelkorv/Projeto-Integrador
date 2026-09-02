import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import '../models/post.dart';
import '../services/post_service.dart';
import '../services/auth_service.dart';

/// Tela de Criação de Post Mobile — Fiel à Estilização Real da Tela WEB (.card, .btn-primary, .modal input, Manrope).
/// Fonte da Verdade: web/style.css
class CreatePostPage extends StatefulWidget {
  const CreatePostPage({super.key});

  @override
  State<CreatePostPage> createState() => _CreatePostPageState();
}

class _CreatePostPageState extends State<CreatePostPage> {
  final tituloController = TextEditingController();
  final conteudoController = TextEditingController();
  final PostService service = PostService();
  bool salvando = false;

  Future<void> salvar() async {
    if (!AuthService.logado || AuthService.idUsuario == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Erro: Usuário não está logado!', style: GoogleFonts.manrope())),
      );
      return;
    }

    if (tituloController.text.trim().isEmpty || conteudoController.text.trim().isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Por favor, preencha o título e o conteúdo.', style: GoogleFonts.manrope())),
      );
      return;
    }

    setState(() {
      salvando = true;
    });

    Post post = Post(
      titulo: tituloController.text.trim(),
      conteudo: conteudoController.text.trim(),
      idUsuario: AuthService.idUsuario,
    );

    bool sucesso = await service.criarPost(post);

    if (!mounted) return;

    setState(() {
      salvando = false;
    });

    if (sucesso) {
      Navigator.pop(context, true);
    } else {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Falha ao salvar no banco de dados.', style: GoogleFonts.manrope())),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF7F8FA),
      appBar: AppBar(
        title: Text('Novo Post', style: GoogleFonts.manrope(fontWeight: FontWeight.w700, fontSize: 18)),
        elevation: 0,
        backgroundColor: Colors.white,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16),
        child: Container(
          padding: const EdgeInsets.all(24),
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(14),
            border: Border.all(color: const Color(0xFFE5E7EB)),
            boxShadow: const [
              BoxShadow(
                color: Color.fromRGBO(17, 24, 39, 0.07),
                blurRadius: 30,
                offset: Offset(0, 12),
              ),
            ],
          ),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  Container(
                    padding: const EdgeInsets.all(8),
                    decoration: BoxDecoration(
                      color: const Color(0xFFF7F8FA),
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: const Icon(Icons.edit_note_rounded, color: Color(0xFFEA3F74), size: 22),
                  ),
                  const SizedBox(width: 10),
                  Text(
                    'Criar Publicação',
                    style: GoogleFonts.manrope(
                      fontSize: 18,
                      fontWeight: FontWeight.w700,
                      color: const Color(0xFF202124),
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 20),
              TextField(
                controller: tituloController,
                style: GoogleFonts.manrope(fontWeight: FontWeight.w700, fontSize: 16, color: const Color(0xFF202124)),
                decoration: InputDecoration(
                  labelText: 'Título da publicação',
                  labelStyle: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 13),
                  prefixIcon: const Icon(Icons.title, color: Color(0xFFEA3F74), size: 18),
                ),
              ),
              const SizedBox(height: 16),
              TextField(
                controller: conteudoController,
                maxLines: 6,
                style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF202124)),
                decoration: InputDecoration(
                  labelText: 'O que você gostaria de compartilhar?',
                  alignLabelWithHint: true,
                  labelStyle: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 13),
                  prefixIcon: const Padding(
                    padding: EdgeInsets.only(bottom: 100),
                    child: Icon(Icons.article_outlined, color: Color(0xFFEA3F74), size: 18),
                  ),
                ),
              ),
              const SizedBox(height: 24),
              SizedBox(
                width: double.infinity,
                height: 46,
                child: salvando
                    ? const Center(child: CircularProgressIndicator(color: Color(0xFFEA3F74)))
                    : ElevatedButton(
                        onPressed: salvar,
                        style: ElevatedButton.styleFrom(
                          backgroundColor: const Color(0xFFEA3F74),
                          foregroundColor: Colors.white,
                          elevation: 0,
                          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                        ),
                        child: Text('Publicar no Feed', style: GoogleFonts.manrope(fontSize: 14, fontWeight: FontWeight.w700)),
                      ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}