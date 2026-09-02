import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import '../services/auth_service.dart';
import '../services/usuario_service.dart';
import 'home_screen.dart';
import 'register_screen.dart';

/// Tela de Login Mobile — Fiel à Estilização Real da Tela WEB (.auth-modal, .auth-kicker, .auth-intro, .auth-input-wrap, .auth-submit).
/// Fonte da Verdade: web/style.css
class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final emailController = TextEditingController();
  final senhaController = TextEditingController();
  final UsuarioService _service = UsuarioService();
  String erro = '';
  bool isLoading = false;
  bool _senhaVisivel = false;

  @override
  void dispose() {
    emailController.dispose();
    senhaController.dispose();
    super.dispose();
  }

  Future<void> _login() async {
    if (emailController.text.trim().isEmpty ||
        senhaController.text.isEmpty) {
      setState(() => erro = 'Preencha e-mail e senha.');
      return;
    }

    setState(() {
      isLoading = true;
      erro = '';
    });

    final usuario = await _service.login(
      emailController.text.trim(),
      senhaController.text,
    );

    if (!mounted) return;

    if (usuario == null || usuario.idUsuario == null) {
      setState(() {
        erro = 'E-mail ou senha incorretos.';
        isLoading = false;
      });
      return;
    }

    AuthService.fazerLogin(
      idUsuario: usuario.idUsuario!,
      nome: usuario.nome,
      email: usuario.email.isNotEmpty ? usuario.email : emailController.text.trim(),
      username: usuario.username,
      bio: usuario.bio,
    );

    Navigator.pushReplacement(
      context,
      MaterialPageRoute(builder: (_) => const HomeScreen()),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF7F8FA),
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        leading: IconButton(
          icon: const Icon(Icons.arrow_back_rounded, color: Color(0xFF202124), size: 22),
          onPressed: () => Navigator.pop(context),
        ),
      ),
      body: SafeArea(
        child: Center(
          child: SingleChildScrollView(
            padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 16),
            child: Container(
              padding: const EdgeInsets.all(28),
              decoration: BoxDecoration(
                color: Colors.white,
                borderRadius: BorderRadius.circular(16),
                border: Border.all(color: const Color(0xFFE5E7EB)),
                boxShadow: const [
                  BoxShadow(
                    color: Color.fromRGBO(43, 23, 32, 0.15),
                    blurRadius: 35,
                    offset: Offset(0, 16),
                  ),
                ],
              ),
              child: Column(
                mainAxisSize: MainAxisSize.min,
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  // Kicker (.auth-kicker da Web)
                  Text(
                    'BEM-VINDO DE VOLTA',
                    style: GoogleFonts.manrope(
                      color: const Color(0xFFEA3F74),
                      fontSize: 12,
                      fontWeight: FontWeight.w800,
                      letterSpacing: 0.8,
                    ),
                  ),
                  const SizedBox(height: 6),

                  // Intro Title (.auth-intro h2 da Web)
                  Text(
                    'Entrar no SocialJoin',
                    style: GoogleFonts.manrope(
                      fontSize: 26,
                      fontWeight: FontWeight.w700,
                      color: const Color(0xFF202124),
                      letterSpacing: -0.5,
                    ),
                  ),
                  const SizedBox(height: 4),
                  Text(
                    'Informe seus dados para acessar sua conta.',
                    style: GoogleFonts.manrope(
                      fontSize: 13,
                      color: const Color(0xFF6B7280),
                    ),
                  ),
                  const SizedBox(height: 28),

                  // Campo Email (.auth-field & .auth-input-wrap da Web)
                  Text(
                    'E-mail',
                    style: GoogleFonts.manrope(
                      fontSize: 13,
                      fontWeight: FontWeight.w700,
                      color: const Color(0xFF202124),
                    ),
                  ),
                  const SizedBox(height: 6),
                  TextField(
                    controller: emailController,
                    keyboardType: TextInputType.emailAddress,
                    style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF202124)),
                    decoration: InputDecoration(
                      hintText: 'seuemail@exemplo.com',
                      prefixIcon: const Icon(Icons.email_outlined, size: 20, color: Color(0xFFEA3F74)),
                    ),
                  ),
                  const SizedBox(height: 16),

                  // Campo Senha (.auth-field & .auth-input-wrap da Web)
                  Text(
                    'Senha',
                    style: GoogleFonts.manrope(
                      fontSize: 13,
                      fontWeight: FontWeight.w700,
                      color: const Color(0xFF202124),
                    ),
                  ),
                  const SizedBox(height: 6),
                  TextField(
                    controller: senhaController,
                    obscureText: !_senhaVisivel,
                    onSubmitted: (_) => _login(),
                    style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF202124)),
                    decoration: InputDecoration(
                      hintText: 'Sua senha',
                      prefixIcon: const Icon(Icons.lock_outline_rounded, size: 20, color: Color(0xFFEA3F74)),
                      suffixIcon: IconButton(
                        icon: Icon(
                          _senhaVisivel ? Icons.visibility_off_outlined : Icons.visibility_outlined,
                          size: 19,
                          color: const Color(0xFF6B7280),
                        ),
                        onPressed: () => setState(() => _senhaVisivel = !_senhaVisivel),
                      ),
                    ),
                  ),
                  const SizedBox(height: 12),

                  // Feedback de erro (.auth-feedback da Web)
                  if (erro.isNotEmpty)
                    Container(
                      padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                      margin: const EdgeInsets.only(bottom: 12),
                      decoration: BoxDecoration(
                        color: const Color(0xFFF7F8FA),
                        borderRadius: BorderRadius.circular(9),
                        border: Border.all(color: const Color(0xFFF2B9C9)),
                      ),
                      child: Text(
                        erro,
                        style: GoogleFonts.manrope(
                          color: const Color(0xFFC93659),
                          fontSize: 13,
                        ),
                      ),
                    ),

                  const SizedBox(height: 12),

                  // Botão Entrar (.btn-primary .auth-submit da Web)
                  SizedBox(
                    height: 46,
                    child: ElevatedButton(
                      style: ElevatedButton.styleFrom(
                        backgroundColor: const Color(0xFFEA3F74),
                        foregroundColor: Colors.white,
                        elevation: 0,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(10),
                        ),
                      ),
                      onPressed: isLoading ? null : _login,
                      child: isLoading
                          ? const SizedBox(
                              height: 20,
                              width: 20,
                              child: CircularProgressIndicator(
                                  color: Colors.white, strokeWidth: 2))
                          : Text(
                              'Entrar',
                              style: GoogleFonts.manrope(
                                fontSize: 14,
                                fontWeight: FontWeight.w600,
                              ),
                            ),
                    ),
                  ),
                  const SizedBox(height: 20),

                  const Divider(color: Color(0xFFE5E7EB)),
                  const SizedBox(height: 16),

                  // Link alternativo (.toggle-auth da Web)
                  Center(
                    child: TextButton(
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(builder: (_) => const RegisterScreen()),
                        ).then((_) => setState(() {}));
                      },
                      child: Text(
                        'Não tem uma conta? Crie uma agora',
                        style: GoogleFonts.manrope(
                          color: const Color(0xFFEA3F74),
                          fontSize: 13,
                          fontWeight: FontWeight.w700,
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
