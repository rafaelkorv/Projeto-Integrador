import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import '../models/usuario.dart';
import '../services/usuario_service.dart';
import 'usuario_form_screen.dart';

/// Tela de Usuários / Pessoas Mobile — Fiel à Estilização Real da Tela WEB (.pessoa-card, .avatar, .search-bar, .btn-primary).
/// Fonte da Verdade: web/style.css
class UsuarioScreen extends StatefulWidget {
  const UsuarioScreen({super.key});

  @override
  State<UsuarioScreen> createState() => _UsuarioScreenState();
}

class _UsuarioScreenState extends State<UsuarioScreen> {
  final UsuarioService service = UsuarioService();
  final TextEditingController searchController = TextEditingController();

  List<Usuario> usuarios = [];
  List<Usuario> usuariosFiltrados = [];
  bool carregando = true;

  @override
  void initState() {
    super.initState();
    carregarUsuarios();
  }

  Future<void> carregarUsuarios() async {
    try {
      final lista = await service.listarUsuarios();
      setState(() {
        usuarios = lista;
        usuariosFiltrados = lista;
        carregando = false;
      });
    } catch (e) {
      setState(() {
        carregando = false;
      });
    }
  }

  void _filtrar(String query) {
    setState(() {
      if (query.trim().isEmpty) {
        usuariosFiltrados = usuarios;
      } else {
        usuariosFiltrados = usuarios.where((u) {
          return u.nome.toLowerCase().contains(query.toLowerCase()) ||
              u.email.toLowerCase().contains(query.toLowerCase());
        }).toList();
      }
    });
  }

  Future<void> deletarUsuario(int id) async {
    await service.deletarUsuario(id);
    carregarUsuarios();
  }

  Future<void> abrirFormulario([Usuario? usuario]) async {
    final resultado = await Navigator.push(
      context,
      MaterialPageRoute(
        builder: (_) => UsuarioFormScreen(
          usuario: usuario,
        ),
      ),
    );

    if (resultado == true) {
      carregarUsuarios();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF7F8FA),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () => abrirFormulario(),
        backgroundColor: const Color(0xFFEA3F74),
        elevation: 4,
        icon: const Icon(Icons.person_add_alt_1, color: Colors.white, size: 20),
        label: Text("Novo Usuário", style: GoogleFonts.manrope(color: Colors.white, fontWeight: FontWeight.w700, fontSize: 14)),
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Page Header (.pessoas-sugestoes h2 da Web)
            Text(
              'COMUNIDADE DE PESSOAS',
              style: GoogleFonts.manrope(
                color: const Color(0xFFEA3F74),
                fontSize: 11,
                fontWeight: FontWeight.w800,
                letterSpacing: 0.9,
              ),
            ),
            const SizedBox(height: 2),
            Text(
              'Pessoas',
              style: GoogleFonts.manrope(
                fontSize: 24,
                fontWeight: FontWeight.w700,
                color: const Color(0xFF202124),
              ),
            ),
            const SizedBox(height: 14),

            // Search Bar (.search-bar da Web)
            TextField(
              controller: searchController,
              onChanged: _filtrar,
              style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF202124)),
              decoration: InputDecoration(
                hintText: "Buscar usuário por nome ou email...",
                hintStyle: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF6B7280)),
                prefixIcon: const Icon(Icons.search_rounded, color: Color(0xFF6B7280), size: 20),
              ),
            ),
            const SizedBox(height: 16),

            Expanded(
              child: carregando
                  ? const Center(child: CircularProgressIndicator(color: Color(0xFFEA3F74)))
                  : usuariosFiltrados.isEmpty
                      ? Center(
                          child: Text(
                            'Nenhum usuário encontrado.',
                            style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 14),
                          ),
                        )
                      : ListView.builder(
                          padding: const EdgeInsets.only(bottom: 80),
                          itemCount: usuariosFiltrados.length,
                          itemBuilder: (context, index) {
                            final usuario = usuariosFiltrados[index];
                            final inicial = usuario.nome.isNotEmpty ? usuario.nome[0].toUpperCase() : '?';

                            return Container(
                              margin: const EdgeInsets.only(bottom: 12),
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
                              child: ListTile(
                                contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                                leading: CircleAvatar(
                                  radius: 20,
                                  backgroundColor: const Color(0xFFEA3F74),
                                  child: Text(
                                    inicial,
                                    style: GoogleFonts.manrope(color: Colors.white, fontWeight: FontWeight.bold, fontSize: 16),
                                  ),
                                ),
                                title: Text(
                                  usuario.nome,
                                  style: GoogleFonts.manrope(fontWeight: FontWeight.w700, color: const Color(0xFF202124), fontSize: 15),
                                ),
                                subtitle: Text(
                                  usuario.email,
                                  style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 12),
                                ),
                                trailing: Row(
                                  mainAxisSize: MainAxisSize.min,
                                  children: [
                                    IconButton(
                                      icon: const Icon(Icons.edit_outlined, color: Color(0xFFEA3F74), size: 18),
                                      onPressed: () => abrirFormulario(usuario),
                                      constraints: const BoxConstraints(minWidth: 34, minHeight: 34),
                                      padding: EdgeInsets.zero,
                                    ),
                                    IconButton(
                                      icon: const Icon(Icons.delete_outline, color: Color(0xFFC93659), size: 18),
                                      onPressed: () {
                                        if (usuario.idUsuario != null) {
                                          deletarUsuario(usuario.idUsuario!);
                                        }
                                      },
                                      constraints: const BoxConstraints(minWidth: 34, minHeight: 34),
                                      padding: EdgeInsets.zero,
                                    ),
                                  ],
                                ),
                              ),
                            );
                          },
                        ),
            ),
          ],
        ),
      ),
    );
  }
}