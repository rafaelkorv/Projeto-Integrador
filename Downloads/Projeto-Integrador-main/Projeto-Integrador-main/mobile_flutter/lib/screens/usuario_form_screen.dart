import 'package:flutter/material.dart';
import '../models/usuario.dart';
import '../services/usuario_service.dart';

class UsuarioFormScreen extends StatefulWidget {
  final Usuario? usuario;

  const UsuarioFormScreen({
    super.key,
    this.usuario,
  });

  @override
  State<UsuarioFormScreen> createState() => _UsuarioFormScreenState();
}

class _UsuarioFormScreenState extends State<UsuarioFormScreen> {
  final nomeController = TextEditingController();
  final usernameController = TextEditingController();
  final emailController = TextEditingController();
  final senhaController = TextEditingController();

  final UsuarioService service = UsuarioService();

  bool editando = false;
  bool salvando = false;

  @override
  void initState() {
    super.initState();

    if (widget.usuario != null) {
      editando = true;
      nomeController.text = widget.usuario!.nome;
      usernameController.text = widget.usuario!.username ?? '';
      emailController.text = widget.usuario!.email;
      senhaController.text = widget.usuario!.senha;
    }
  }

  @override
  void dispose() {
    nomeController.dispose();
    usernameController.dispose();
    emailController.dispose();
    senhaController.dispose();
    super.dispose();
  }

  Future<void> salvar() async {
    if (nomeController.text.trim().isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Informe o nome do usuário.')),
      );
      return;
    }

    setState(() => salvando = true);

    if (editando && widget.usuario?.idUsuario != null) {
      await service.atualizarPerfil(
        widget.usuario!.idUsuario!,
        nomeController.text.trim(),
        widget.usuario!.bio ?? '',
      );
    } else {
      final username = usernameController.text.trim().isNotEmpty
          ? usernameController.text.trim()
          : emailController.text.split('@')[0];

      await service.criarUsuario(
        nome: nomeController.text.trim(),
        username: username,
        email: emailController.text.trim(),
        senha: senhaController.text,
      );
    }

    if (!mounted) return;
    Navigator.pop(context, true);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF8FAFC),
      appBar: AppBar(
        title: Text(
          editando ? 'Editar Usuário' : 'Novo Usuário',
          style: const TextStyle(fontWeight: FontWeight.bold),
        ),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(20),
        child: Container(
          padding: const EdgeInsets.all(20),
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(20),
            border: Border.all(color: const Color(0xFFE2E8F0)),
          ),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              TextField(
                controller: nomeController,
                decoration: _fieldDeco('Nome Completo', Icons.person_outline_rounded),
              ),
              const SizedBox(height: 14),
              if (!editando) ...[
                TextField(
                  controller: usernameController,
                  decoration: _fieldDeco('Nome de Usuário (@)', Icons.alternate_email_rounded),
                ),
                const SizedBox(height: 14),
                TextField(
                  controller: emailController,
                  keyboardType: TextInputType.emailAddress,
                  decoration: _fieldDeco('E-mail', Icons.email_outlined),
                ),
                const SizedBox(height: 14),
                TextField(
                  controller: senhaController,
                  obscureText: true,
                  decoration: _fieldDeco('Senha', Icons.lock_outline_rounded),
                ),
                const SizedBox(height: 24),
              ] else
                const SizedBox(height: 14),

              SizedBox(
                height: 50,
                child: ElevatedButton(
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFFEA3F74),
                    foregroundColor: Colors.white,
                    elevation: 0,
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(14)),
                  ),
                  onPressed: salvando ? null : salvar,
                  child: salvando
                      ? const SizedBox(
                          height: 20,
                          width: 20,
                          child: CircularProgressIndicator(color: Colors.white, strokeWidth: 2.5),
                        )
                      : const Text('Salvar', style: TextStyle(fontSize: 15, fontWeight: FontWeight.bold)),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  InputDecoration _fieldDeco(String label, IconData icon) {
    return InputDecoration(
      labelText: label,
      prefixIcon: Icon(icon, size: 18, color: const Color(0xFFEA3F74)),
      filled: true,
      fillColor: const Color(0xFFF8FAFC),
      border: OutlineInputBorder(borderRadius: BorderRadius.circular(14)),
      enabledBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(14),
        borderSide: BorderSide(color: Colors.grey.shade200),
      ),
      focusedBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(14),
        borderSide: const BorderSide(color: Color(0xFFEA3F74), width: 1.5),
      ),
      contentPadding: const EdgeInsets.symmetric(horizontal: 14, vertical: 14),
    );
  }
}
