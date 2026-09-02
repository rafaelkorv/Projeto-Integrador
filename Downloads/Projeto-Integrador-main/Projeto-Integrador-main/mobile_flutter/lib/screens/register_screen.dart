import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import '../models/usuario.dart';
import '../services/usuario_service.dart';
import '../services/auth_service.dart';
import 'home_screen.dart';

/// Tela de Cadastro Mobile — Fiel à Estilização Real da Tela WEB (.auth-modal, .auth-kicker, .auth-intro, .auth-input-wrap, .auth-submit).
/// Fonte da Verdade: web/style.css
class RegisterScreen extends StatefulWidget {
  const RegisterScreen({super.key});

  @override
  State<RegisterScreen> createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  final _formKey = GlobalKey<FormState>();

  final nomeController = TextEditingController();
  final usernameController = TextEditingController();
  final dataNascimentoController = TextEditingController();
  final emailController = TextEditingController();
  final senhaController = TextEditingController();
  final confirmarSenhaController = TextEditingController();

  final UsuarioService _service = UsuarioService();

  DateTime? _dataNascimentoSelecionada;
  String _dataNascimentoIso = '';
  String erro = '';
  bool isLoading = false;
  bool _senhaVisivel = false;
  bool _confirmarSenhaVisivel = false;

  @override
  void dispose() {
    nomeController.dispose();
    usernameController.dispose();
    dataNascimentoController.dispose();
    emailController.dispose();
    senhaController.dispose();
    confirmarSenhaController.dispose();
    super.dispose();
  }

  Future<void> _selecionarDataNascimento() async {
    FocusScope.of(context).unfocus();

    final DateTime hoje = DateTime.now();
    final DateTime dataInicial = _dataNascimentoSelecionada ?? DateTime(2000, 1, 1);

    final DateTime? dataEscolhida = await showDatePicker(
      context: context,
      initialDate: dataInicial.isAfter(hoje) ? hoje : dataInicial,
      firstDate: DateTime(1900, 1, 1),
      lastDate: hoje,
      helpText: 'SELECIONE SUA DATA DE NASCIMENTO',
      cancelText: 'Cancelar',
      confirmText: 'Confirmar',
      builder: (context, child) {
        return Theme(
          data: Theme.of(context).copyWith(
            colorScheme: const ColorScheme.light(
              primary: Color(0xFFEA3F74),
              onPrimary: Colors.white,
              surface: Colors.white,
              onSurface: Color(0xFF202124),
            ),
            textButtonTheme: TextButtonThemeData(
              style: TextButton.styleFrom(
                foregroundColor: const Color(0xFFEA3F74),
                textStyle: GoogleFonts.manrope(fontWeight: FontWeight.w700),
              ),
            ),
          ),
          child: child!,
        );
      },
    );

    if (dataEscolhida != null) {
      setState(() {
        _dataNascimentoSelecionada = dataEscolhida;

        final dia = dataEscolhida.day.toString().padLeft(2, '0');
        final mes = dataEscolhida.month.toString().padLeft(2, '0');
        final ano = dataEscolhida.year.toString();
        dataNascimentoController.text = '$dia/$mes/$ano';

        _dataNascimentoIso = '$ano-$mes-$dia';
      });
    }
  }

  Future<void> _cadastrar() async {
    if (!_formKey.currentState!.validate()) return;

    if (_dataNascimentoIso.isEmpty) {
      setState(() {
        erro = 'Por favor, selecione sua data de nascimento.';
      });
      return;
    }

    setState(() {
      isLoading = true;
      erro = '';
    });

    final nome = nomeController.text.trim();
    final username = usernameController.text.trim().replaceAll('@', '');
    final email = emailController.text.trim();
    final senha = senhaController.text;

    final result = await _service.criarUsuario(
      nome: nome,
      nomeCompleto: nome,
      username: username,
      email: email,
      senha: senha,
      dataNascimento: _dataNascimentoIso,
    );

    if (!mounted) return;

    if (result['sucesso'] == true) {
      final novoUsuario = result['usuario'] as Usuario?;
      final id = novoUsuario?.idUsuario ?? 1;
      final nomeFinal = novoUsuario?.nome.isNotEmpty == true ? novoUsuario!.nome : nome;
      final usernameFinal = (novoUsuario?.username != null && novoUsuario!.username!.isNotEmpty)
          ? novoUsuario.username!
          : username;
      final emailFinal = (novoUsuario?.email != null && novoUsuario!.email.isNotEmpty)
          ? novoUsuario.email
          : email;

      AuthService.fazerLogin(
        idUsuario: id,
        nome: nomeFinal,
        email: emailFinal,
        username: usernameFinal,
        bio: novoUsuario?.bio,
      );

      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text('Conta criada com sucesso! Bem-vindo(a) ao SocialJoin!', style: GoogleFonts.manrope()),
          backgroundColor: const Color(0xFF10B981),
          duration: const Duration(seconds: 2),
        ),
      );

      Navigator.of(context).pushAndRemoveUntil(
        MaterialPageRoute(builder: (_) => const HomeScreen()),
        (route) => false,
      );
    } else {
      setState(() {
        erro = result['erro'] ?? 'Não foi possível concluir o cadastro. Verifique os dados.';
        isLoading = false;
      });
    }
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
        child: SingleChildScrollView(
          padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 12),
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
            child: Form(
              key: _formKey,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  // Kicker (.auth-kicker da Web)
                  Text(
                    'CRIE SUA CONTA NA REDE',
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
                    'Cadastrar no SocialJoin',
                    style: GoogleFonts.manrope(
                      fontSize: 26,
                      fontWeight: FontWeight.w700,
                      color: const Color(0xFF202124),
                      letterSpacing: -0.5,
                    ),
                  ),
                  const SizedBox(height: 4),
                  Text(
                    'Preencha seus dados para participar dos eventos.',
                    style: GoogleFonts.manrope(
                      fontSize: 13,
                      color: const Color(0xFF6B7280),
                    ),
                  ),
                  const SizedBox(height: 24),

                  // Campo: Nome Completo
                  _buildWebFormField(
                    controller: nomeController,
                    label: 'Nome Completo',
                    hint: 'Ex: Ana Clara Silva',
                    icon: Icons.person_outline_rounded,
                    keyboardType: TextInputType.name,
                    textCapitalization: TextCapitalization.words,
                    validator: (v) {
                      if (v == null || v.trim().isEmpty) return 'Informe seu nome completo';
                      if (v.trim().length < 2) return 'O nome deve ter pelo menos 2 letras';
                      return null;
                    },
                  ),
                  const SizedBox(height: 14),

                  // Campo: Nome de Usuário (@)
                  _buildWebFormField(
                    controller: usernameController,
                    label: 'Nome de usuário (@)',
                    hint: 'Ex: anaclara',
                    icon: Icons.alternate_email_rounded,
                    keyboardType: TextInputType.text,
                    validator: (v) {
                      if (v == null || v.trim().isEmpty) return 'Informe um nome de usuário';
                      final clean = v.trim().replaceAll('@', '');
                      if (clean.length < 3) return 'O usuário deve ter pelo menos 3 caracteres';
                      if (clean.contains(' ')) return 'O usuário não pode conter espaços';
                      return null;
                    },
                  ),
                  const SizedBox(height: 14),

                  // Campo: Data de Nascimento
                  _buildWebDateField(
                    controller: dataNascimentoController,
                    label: 'Data de nascimento',
                    hint: 'Selecione sua data',
                    icon: Icons.calendar_today_rounded,
                    onTap: _selecionarDataNascimento,
                    validator: (v) {
                      if (v == null || v.trim().isEmpty || _dataNascimentoIso.isEmpty) {
                        return 'Selecione sua data de nascimento';
                      }
                      return null;
                    },
                  ),
                  const SizedBox(height: 14),

                  // Campo: E-mail
                  _buildWebFormField(
                    controller: emailController,
                    label: 'E-mail',
                    hint: 'seuemail@exemplo.com',
                    icon: Icons.mail_outline_rounded,
                    keyboardType: TextInputType.emailAddress,
                    validator: (v) {
                      if (v == null || v.trim().isEmpty) return 'Informe seu e-mail';
                      final email = v.trim();
                      final emailRegExp = RegExp(r'^[^@\s]+@[^@\s]+\.[^@\s]+$');
                      if (!emailRegExp.hasMatch(email)) return 'Informe um e-mail válido';
                      return null;
                    },
                  ),
                  const SizedBox(height: 14),

                  // Campo: Senha
                  _buildWebFormField(
                    controller: senhaController,
                    label: 'Senha',
                    hint: 'Mínimo de 6 caracteres',
                    icon: Icons.lock_outline_rounded,
                    isPassword: true,
                    senhaVisivel: _senhaVisivel,
                    onToggleSenha: () => setState(() => _senhaVisivel = !_senhaVisivel),
                    validator: (v) {
                      if (v == null || v.isEmpty) return 'Informe uma senha';
                      if (v.length < 6) return 'A senha deve ter no mínimo 6 caracteres';
                      return null;
                    },
                  ),
                  const SizedBox(height: 14),

                  // Campo: Confirmar Senha
                  _buildWebFormField(
                    controller: confirmarSenhaController,
                    label: 'Confirmar Senha',
                    hint: 'Repita sua senha',
                    icon: Icons.lock_clock_outlined,
                    isPassword: true,
                    senhaVisivel: _confirmarSenhaVisivel,
                    onToggleSenha: () => setState(() => _confirmarSenhaVisivel = !_confirmarSenhaVisivel),
                    validator: (v) {
                      if (v == null || v.isEmpty) return 'Confirme sua senha';
                      if (v != senhaController.text) return 'As senhas não coincidem';
                      return null;
                    },
                  ),
                  const SizedBox(height: 16),

                  // Feedback de erro (.auth-feedback da Web)
                  if (erro.isNotEmpty) ...[
                    Container(
                      padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
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
                    const SizedBox(height: 16),
                  ],

                  // Botão Cadastrar (.btn-primary .auth-submit da Web)
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
                      onPressed: isLoading ? null : _cadastrar,
                      child: isLoading
                          ? const SizedBox(
                              height: 20,
                              width: 20,
                              child: CircularProgressIndicator(
                                color: Colors.white,
                                strokeWidth: 2,
                              ),
                            )
                          : Text(
                              'Criar Conta',
                              style: GoogleFonts.manrope(
                                fontSize: 14,
                                fontWeight: FontWeight.w600,
                              ),
                            ),
                    ),
                  ),
                  const SizedBox(height: 20),

                  Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Text(
                        'Já tem uma conta? ',
                        style: GoogleFonts.manrope(
                          color: const Color(0xFF6B7280),
                          fontSize: 13,
                        ),
                      ),
                      GestureDetector(
                        onTap: () => Navigator.pop(context),
                        child: Text(
                          'Entrar',
                          style: GoogleFonts.manrope(
                            color: const Color(0xFFEA3F74),
                            fontWeight: FontWeight.w700,
                            fontSize: 13,
                          ),
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildWebFormField({
    required TextEditingController controller,
    required String label,
    String? hint,
    required IconData icon,
    bool isPassword = false,
    bool? senhaVisivel,
    VoidCallback? onToggleSenha,
    TextInputType? keyboardType,
    TextCapitalization textCapitalization = TextCapitalization.none,
    String? Function(String?)? validator,
  }) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          label,
          style: GoogleFonts.manrope(
            fontSize: 13,
            fontWeight: FontWeight.w700,
            color: const Color(0xFF202124),
          ),
        ),
        const SizedBox(height: 6),
        TextFormField(
          controller: controller,
          obscureText: isPassword && !(senhaVisivel ?? false),
          keyboardType: keyboardType,
          textCapitalization: textCapitalization,
          validator: validator,
          style: GoogleFonts.manrope(
            fontSize: 14,
            color: const Color(0xFF202124),
          ),
          decoration: InputDecoration(
            hintText: hint,
            prefixIcon: Icon(icon, size: 20, color: const Color(0xFFEA3F74)),
            suffixIcon: isPassword
                ? IconButton(
                    icon: Icon(
                      (senhaVisivel ?? false)
                          ? Icons.visibility_off_outlined
                          : Icons.visibility_outlined,
                      size: 19,
                      color: const Color(0xFF6B7280),
                    ),
                    onPressed: onToggleSenha,
                  )
                : null,
          ),
        ),
      ],
    );
  }

  Widget _buildWebDateField({
    required TextEditingController controller,
    required String label,
    String? hint,
    required IconData icon,
    required VoidCallback onTap,
    String? Function(String?)? validator,
  }) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          label,
          style: GoogleFonts.manrope(
            fontSize: 13,
            fontWeight: FontWeight.w700,
            color: const Color(0xFF202124),
          ),
        ),
        const SizedBox(height: 6),
        TextFormField(
          controller: controller,
          readOnly: true,
          onTap: onTap,
          validator: validator,
          style: GoogleFonts.manrope(
            fontSize: 14,
            color: const Color(0xFF202124),
          ),
          decoration: InputDecoration(
            hintText: hint,
            prefixIcon: Icon(icon, size: 20, color: const Color(0xFFEA3F74)),
            suffixIcon: const Icon(
              Icons.calendar_month_rounded,
              color: Color(0xFFEA3F74),
              size: 19,
            ),
          ),
        ),
      ],
    );
  }
}
