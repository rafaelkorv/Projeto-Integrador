import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:image_picker/image_picker.dart';
import '../models/usuario.dart';
import '../models/post.dart';
import '../models/evento.dart';
import '../models/comunidade.dart';
import '../services/auth_service.dart';
import '../services/usuario_service.dart';
import '../services/post_service.dart';
import '../services/evento_service.dart';
import '../services/comunidade_service.dart';
import '../services/api_service.dart';

/// Tela de Perfil Mobile — Fiel à Estilização Real da Tela WEB (.perfil-tabs, .perfil-tab.active::after, .avatar, .card, .btn-primary).
/// Fonte da Verdade: web/style.css
class ProfilePage extends StatefulWidget {
  const ProfilePage({super.key});

  @override
  State<ProfilePage> createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage>
    with SingleTickerProviderStateMixin {
  final UsuarioService _usuarioService = UsuarioService();
  final PostService _postService = PostService();
  final EventoService _eventoService = EventoService();
  final ComunidadeService _comunidadeService = ComunidadeService();

  Usuario? _usuario;
  List<Post> _userPosts = [];
  List<Evento> _userEventos = [];
  List<Comunidade> _userComunidades = [];

  bool _carregando = true;
  bool _enviandoFoto = false;
  String? _erro;
  late TabController _tabController;

  @override
  void initState() {
    super.initState();
    _tabController = TabController(length: 4, vsync: this);
    _carregarDadosCompletos();
  }

  @override
  void dispose() {
    _tabController.dispose();
    super.dispose();
  }

  Future<void> _carregarDadosCompletos() async {
    if (!AuthService.logado || AuthService.idUsuario == null) {
      setState(() {
        _erro = 'Faça login para ver seu perfil.';
        _carregando = false;
      });
      return;
    }

    final id = AuthService.idUsuario!;
    setState(() {
      _carregando = true;
      _erro = null;
    });

    try {
      final userFuture = _usuarioService.buscarPorId(id);
      final postsFuture = _postService.listarPostsPorUsuario(id);
      final eventosFuture = _eventoService.listarEventosPorUsuario(id);
      final comunidadesFuture = _comunidadeService.listarComunidadesPorUsuario(id);

      final results = await Future.wait([
        userFuture,
        postsFuture,
        eventosFuture,
        comunidadesFuture,
      ]);

      if (mounted) {
        setState(() {
          _usuario = results[0] as Usuario?;
          _userPosts = results[1] as List<Post>;
          _userEventos = results[2] as List<Evento>;
          _userComunidades = results[3] as List<Comunidade>;
          _carregando = false;

          if (_usuario == null) {
            _usuario = Usuario(
              idUsuario: AuthService.idUsuario,
              nome: AuthService.nomeUsuario ?? 'Usuário',
              email: AuthService.emailUsuario ?? '',
              senha: '',
              username: AuthService.username,
              bio: AuthService.bio,
            );
          } else {
            if (_usuario!.nome.isNotEmpty) AuthService.nomeUsuario = _usuario!.nome;
            if (_usuario!.username != null) AuthService.username = _usuario!.username;
            if (_usuario!.bio != null) AuthService.bio = _usuario!.bio;
          }
        });
      }
    } catch (e) {
      if (mounted) {
        setState(() {
          _erro = 'Não foi possível carregar as informações do perfil.';
          _carregando = false;
        });
      }
    }
  }

  Future<void> _alterarFotoPerfil() async {
    if (!AuthService.logado || AuthService.idUsuario == null) return;

    try {
      final picker = ImagePicker();
      final picked = await picker.pickImage(
        source: ImageSource.gallery,
        maxWidth: 1200,
        maxHeight: 1200,
        imageQuality: 85,
      );

      if (picked == null) return;

      final length = await picked.length();
      if (length > 5 * 1024 * 1024) {
        if (mounted) {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(
              content: Text('A imagem deve ter no máximo 5 MB.',
                  style: GoogleFonts.manrope()),
              backgroundColor: const Color(0xFFC93659),
            ),
          );
        }
        return;
      }

      setState(() => _enviandoFoto = true);

      final bytes = await picked.readAsBytes();
      final res = await _usuarioService.atualizarFotoPerfil(
        idUsuario: AuthService.idUsuario!,
        caminhoArquivo: picked.path,
        bytes: bytes,
        nomeArquivo: picked.name,
      );

      if (!mounted) return;
      setState(() => _enviandoFoto = false);

      if (res['sucesso'] == true && res['usuario'] != null) {
        final Usuario userAtualizado = res['usuario'];
        setState(() {
          _usuario?.fotoPerfil = userAtualizado.fotoPerfil;
        });
        AuthService.fotoPerfil = userAtualizado.fotoPerfil;

        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Foto de perfil atualizada com sucesso!',
                style: GoogleFonts.manrope()),
            backgroundColor: const Color(0xFF10B981),
          ),
        );
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text(res['erro'] ?? 'Erro ao alterar foto.',
                style: GoogleFonts.manrope()),
            backgroundColor: const Color(0xFFC93659),
          ),
        );
      }
    } catch (e) {
      if (mounted) {
        setState(() => _enviandoFoto = false);
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Erro ao selecionar foto.',
                style: GoogleFonts.manrope()),
            backgroundColor: const Color(0xFFC93659),
          ),
        );
      }
    }
  }

  void _abrirEditar() {
    if (_usuario == null) return;

    final nomeCtrl = TextEditingController(text: _usuario!.nome);
    final bioCtrl = TextEditingController(text: _usuario!.bio ?? '');
    bool salvando = false;

    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (ctx) => StatefulBuilder(
        builder: (ctx, setModal) => Container(
          padding: EdgeInsets.only(
            bottom: MediaQuery.of(ctx).viewInsets.bottom + 20,
            left: 20,
            right: 20,
            top: 20,
          ),
          decoration: const BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.vertical(top: Radius.circular(16)),
          ),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            mainAxisSize: MainAxisSize.min,
            children: [
              Center(
                child: Container(
                  width: 36,
                  height: 4,
                  decoration: BoxDecoration(
                    color: const Color(0xFFE5E7EB),
                    borderRadius: BorderRadius.circular(2),
                  ),
                ),
              ),
              const SizedBox(height: 20),

              Text(
                'Editar Perfil',
                style: GoogleFonts.manrope(
                  fontSize: 20,
                  fontWeight: FontWeight.w700,
                  color: const Color(0xFF202124),
                ),
              ),
              const SizedBox(height: 20),

              TextField(
                controller: nomeCtrl,
                style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF202124)),
                decoration: _inputDecoration('Nome Completo', Icons.person_outline_rounded),
              ),
              const SizedBox(height: 12),

              TextField(
                controller: bioCtrl,
                maxLines: 3,
                maxLength: 300,
                style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF202124)),
                decoration: _inputDecoration('Biografia (opcional)', Icons.info_outline_rounded),
              ),
              const SizedBox(height: 6),

              Text(
                'Seu username @${_usuario!.username ?? ''} não pode ser alterado.',
                style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 12),
              ),
              const SizedBox(height: 20),

              SizedBox(
                height: 46,
                child: ElevatedButton(
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFFEA3F74),
                    foregroundColor: Colors.white,
                    elevation: 0,
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                  ),
                  onPressed: salvando
                      ? null
                      : () async {
                          if (nomeCtrl.text.trim().isEmpty) {
                            ScaffoldMessenger.of(context).showSnackBar(
                              SnackBar(
                                content: Text('O nome não pode ser vazio.', style: GoogleFonts.manrope()),
                                backgroundColor: const Color(0xFFC93659),
                              ),
                            );
                            return;
                          }

                          setModal(() => salvando = true);

                          final messenger = ScaffoldMessenger.of(context);
                          final nav = Navigator.of(ctx);

                          final ok = await _usuarioService.atualizarPerfil(
                            AuthService.idUsuario!,
                            nomeCtrl.text.trim(),
                            bioCtrl.text.trim(),
                          );

                          if (!mounted) return;
                          nav.pop();

                          if (ok) {
                            AuthService.nomeUsuario = nomeCtrl.text.trim();
                            AuthService.bio = bioCtrl.text.trim();

                            messenger.showSnackBar(
                              SnackBar(
                                content: Text('Perfil atualizado com sucesso!', style: GoogleFonts.manrope()),
                                backgroundColor: const Color(0xFF10B981),
                              ),
                            );

                            _carregarDadosCompletos();
                          } else {
                            messenger.showSnackBar(
                              SnackBar(
                                content: Text('Erro ao salvar. Tente novamente.', style: GoogleFonts.manrope()),
                                backgroundColor: const Color(0xFFC93659),
                              ),
                            );
                          }
                        },
                  child: salvando
                      ? const SizedBox(
                          height: 20,
                          width: 20,
                          child: CircularProgressIndicator(color: Colors.white, strokeWidth: 2),
                        )
                      : Text(
                          'Salvar Alterações',
                          style: GoogleFonts.manrope(fontSize: 14, fontWeight: FontWeight.w700),
                        ),
                ),
              ),
              const SizedBox(height: 8),
            ],
          ),
        ),
      ),
    );
  }

  InputDecoration _inputDecoration(String label, IconData icon) {
    return InputDecoration(
      labelText: label,
      prefixIcon: Icon(icon, size: 18, color: const Color(0xFFEA3F74)),
      filled: true,
      fillColor: Colors.white,
      border: OutlineInputBorder(borderRadius: BorderRadius.circular(10), borderSide: const BorderSide(color: Color(0xFFE5E7EB))),
      enabledBorder: OutlineInputBorder(borderRadius: BorderRadius.circular(10), borderSide: const BorderSide(color: Color(0xFFE5E7EB))),
      focusedBorder: OutlineInputBorder(borderRadius: BorderRadius.circular(10), borderSide: const BorderSide(color: Color(0xFFEA3F74), width: 1.5)),
      contentPadding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
      labelStyle: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 13),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF5F7FF),
      body: _carregando
          ? const Center(child: CircularProgressIndicator(color: Color(0xFFEA3F74)))
          : _erro != null
              ? _buildErro()
              : _buildPerfil(),
    );
  }

  Widget _buildErro() {
    return Center(
      child: Padding(
        padding: const EdgeInsets.all(32),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const Icon(Icons.person_off_rounded, size: 52, color: Color(0xFF6B7280)),
            const SizedBox(height: 16),
            Text(
              _erro!,
              textAlign: TextAlign.center,
              style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 14),
            ),
            const SizedBox(height: 20),
            if (AuthService.logado)
              ElevatedButton(
                onPressed: _carregarDadosCompletos,
                child: const Text('Tentar novamente'),
              ),
          ],
        ),
      ),
    );
  }

  Widget _buildPerfil() {
    final usuario = _usuario!;
    final inicial = usuario.nome.isNotEmpty ? usuario.nome[0].toUpperCase() : 'U';

    return RefreshIndicator(
      onRefresh: _carregarDadosCompletos,
      color: const Color(0xFFEA3F74),
      child: NestedScrollView(
        headerSliverBuilder: (context, innerBoxIsScrolled) => [
          // Header Card Fiel à Web
          SliverToBoxAdapter(
            child: Padding(
              padding: const EdgeInsets.all(16),
              child: Container(
                padding: const EdgeInsets.all(20),
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
                  children: [
                    Row(
                      crossAxisAlignment: CrossAxisAlignment.center,
                      children: [
                        // Avatar com foto do Google Drive e botão de upload
                        Stack(
                          clipBehavior: Clip.none,
                          children: [
                            GestureDetector(
                              onTap: _enviandoFoto ? null : _alterarFotoPerfil,
                              child: CircleAvatar(
                                radius: 32,
                                backgroundColor: const Color(0xFFEA3F74),
                                backgroundImage: (ApiService.formatarUrlFotoPerfil(usuario.fotoPerfil) != null)
                                    ? NetworkImage(ApiService.formatarUrlFotoPerfil(usuario.fotoPerfil)!)
                                    : null,
                                child: _enviandoFoto
                                    ? const SizedBox(
                                        width: 24,
                                        height: 24,
                                        child: CircularProgressIndicator(
                                          color: Colors.white,
                                          strokeWidth: 2,
                                        ),
                                      )
                                    : (ApiService.formatarUrlFotoPerfil(usuario.fotoPerfil) == null)
                                        ? Text(
                                            inicial,
                                            style: GoogleFonts.manrope(
                                              color: Colors.white,
                                              fontSize: 24,
                                              fontWeight: FontWeight.w800,
                                            ),
                                          )
                                        : null,
                              ),
                            ),
                            Positioned(
                              bottom: -2,
                              right: -2,
                              child: GestureDetector(
                                onTap: _enviandoFoto ? null : _alterarFotoPerfil,
                                child: Container(
                                  padding: const EdgeInsets.all(5),
                                  decoration: BoxDecoration(
                                    color: const Color(0xFFEA3F74),
                                    shape: BoxShape.circle,
                                    border: Border.all(
                                      color: Colors.white,
                                      width: 2,
                                    ),
                                  ),
                                  child: const Icon(
                                    Icons.camera_alt_rounded,
                                    size: 13,
                                    color: Colors.white,
                                  ),
                                ),
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(width: 16),
                        Expanded(
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                usuario.nome,
                                style: GoogleFonts.manrope(
                                  fontSize: 20,
                                  fontWeight: FontWeight.w700,
                                  color: const Color(0xFF202124),
                                ),
                              ),
                              if (usuario.username != null && usuario.username!.isNotEmpty) ...[
                                Text(
                                  '@${usuario.username}',
                                  style: GoogleFonts.manrope(
                                    color: const Color(0xFF6B7280),
                                    fontSize: 13,
                                  ),
                                ),
                              ],
                            ],
                          ),
                        ),
                        OutlinedButton(
                          onPressed: _abrirEditar,
                          style: OutlinedButton.styleFrom(
                            foregroundColor: const Color(0xFFEA3F74),
                            side: const BorderSide(color: Color(0xFFF9ACC6)),
                            padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 8),
                            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
                          ),
                          child: Text('Editar', style: GoogleFonts.manrope(fontSize: 12, fontWeight: FontWeight.w700)),
                        ),
                      ],
                    ),

                    if (usuario.bio != null && usuario.bio!.isNotEmpty) ...[
                      const SizedBox(height: 14),
                      Align(
                        alignment: Alignment.centerLeft,
                        child: Text(
                          usuario.bio!,
                          style: GoogleFonts.manrope(
                            color: const Color(0xFF6B7280),
                            fontSize: 13,
                            height: 1.5,
                          ),
                        ),
                      ),
                    ],

                    const SizedBox(height: 16),

                    // Estatísticas em linha estilo Web
                    Container(
                      padding: const EdgeInsets.symmetric(vertical: 12),
                      decoration: const BoxDecoration(
                        border: Border(
                          top: BorderSide(color: Color(0xFFE5E7EB), width: 1.0),
                        ),
                      ),
                      child: Row(
                        mainAxisAlignment: MainAxisAlignment.spaceAround,
                        children: [
                          _buildStatItem('Publicações', _userPosts.length.toString()),
                          _buildVerticalDivider(),
                          _buildStatItem('Eventos', _userEventos.length.toString()),
                          _buildVerticalDivider(),
                          _buildStatItem('Comunidades', _userComunidades.length.toString()),
                        ],
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ),

          // TabBar Fiel ao `.perfil-tabs` da Web CSS
          SliverPersistentHeader(
            pinned: true,
            delegate: _SliverAppBarDelegate(
              TabBar(
                controller: _tabController,
                indicatorColor: const Color(0xFFEA3F74),
                indicatorWeight: 3,
                labelColor: const Color(0xFFEA3F74),
                unselectedLabelColor: const Color(0xFF6B7280),
                labelStyle: GoogleFonts.manrope(fontWeight: FontWeight.w800, fontSize: 13),
                unselectedLabelStyle: GoogleFonts.manrope(fontWeight: FontWeight.w600, fontSize: 13),
                tabs: const [
                  Tab(text: 'Sobre'),
                  Tab(text: 'Posts'),
                  Tab(text: 'Eventos'),
                  Tab(text: 'Grupos'),
                ],
              ),
            ),
          ),
        ],
        body: TabBarView(
          controller: _tabController,
          children: [
            _buildTabSobre(usuario),
            _buildTabPosts(),
            _buildTabEventos(),
            _buildTabComunidades(),
          ],
        ),
      ),
    );
  }

  Widget _buildStatItem(String label, String value) {
    return Column(
      children: [
        Text(
          value,
          style: GoogleFonts.manrope(
            color: const Color(0xFF202124),
            fontSize: 16,
            fontWeight: FontWeight.w800,
          ),
        ),
        Text(
          label,
          style: GoogleFonts.manrope(
            color: const Color(0xFF6B7280),
            fontSize: 11,
            fontWeight: FontWeight.w600,
          ),
        ),
      ],
    );
  }

  Widget _buildVerticalDivider() {
    return Container(
      height: 20,
      width: 1,
      color: const Color(0xFFE5E7EB),
    );
  }

  Widget _buildTabSobre(Usuario usuario) {
    final emailExibicao = (usuario.email.isNotEmpty)
        ? usuario.email
        : (AuthService.emailUsuario ?? 'Não informado');

    return SingleChildScrollView(
      padding: const EdgeInsets.fromLTRB(16, 16, 16, 90),
      child: Column(
        children: [
          Container(
            decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.circular(14),
              border: Border.all(color: const Color(0xFFE5E7EB)),
            ),
            child: Column(
              children: [
                _infoItem(Icons.person_outline_rounded, 'Nome Completo', usuario.nome),
                _divider(),
                _infoItem(Icons.alternate_email_rounded, 'Nome de Usuário', usuario.username != null ? '@${usuario.username}' : 'Não definido'),
                _divider(),
                _infoItem(Icons.email_outlined, 'E-mail', emailExibicao),
                if (usuario.bio != null && usuario.bio!.isNotEmpty) ...[
                  _divider(),
                  _infoItem(Icons.info_outline_rounded, 'Biografia', usuario.bio!),
                ],
              ],
            ),
          ),
          const SizedBox(height: 20),

          // Botão Logout
          SizedBox(
            width: double.infinity,
            height: 44,
            child: OutlinedButton.icon(
              onPressed: () {
                showDialog(
                  context: context,
                  builder: (ctx) => AlertDialog(
                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(14)),
                    title: Text('Sair da conta', style: GoogleFonts.manrope(fontWeight: FontWeight.w700, fontSize: 18)),
                    content: Text('Tem certeza que deseja sair?', style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF6B7280))),
                    actions: [
                      TextButton(
                        onPressed: () => Navigator.pop(ctx),
                        child: Text('Cancelar', style: GoogleFonts.manrope(color: const Color(0xFF6B7280))),
                      ),
                      ElevatedButton(
                        style: ElevatedButton.styleFrom(
                          backgroundColor: const Color(0xFFC93659),
                          foregroundColor: Colors.white,
                          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
                        ),
                        onPressed: () {
                          Navigator.pop(ctx);
                          AuthService.fazerLogout();
                          setState(() {});
                          ScaffoldMessenger.of(context).showSnackBar(
                            SnackBar(
                              content: Text('Você saiu da conta.', style: GoogleFonts.manrope()),
                            ),
                          );
                        },
                        child: const Text('Sair'),
                      ),
                    ],
                  ),
                );
              },
              icon: const Icon(Icons.logout_rounded, size: 18),
              label: Text('Sair da conta', style: GoogleFonts.manrope(fontWeight: FontWeight.w600, fontSize: 14)),
              style: OutlinedButton.styleFrom(
                foregroundColor: const Color(0xFFC93659),
                side: const BorderSide(color: Color(0xFFF2B9C9)),
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildTabPosts() {
    if (_userPosts.isEmpty) {
      return _buildEmptyTab(
        icon: Icons.article_outlined,
        title: 'Nenhuma publicação',
        subtitle: 'Você ainda não compartilhou nenhum post.',
      );
    }

    return ListView.builder(
      padding: const EdgeInsets.fromLTRB(16, 16, 16, 90),
      itemCount: _userPosts.length,
      itemBuilder: (context, index) {
        final p = _userPosts[index];
        return Container(
          margin: const EdgeInsets.only(bottom: 12),
          padding: const EdgeInsets.all(16),
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(14),
            border: Border.all(color: const Color(0xFFE5E7EB)),
          ),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                p.titulo,
                style: GoogleFonts.manrope(
                  fontWeight: FontWeight.w700,
                  fontSize: 16,
                  color: const Color(0xFF202124),
                ),
              ),
              const SizedBox(height: 6),
              Text(
                p.conteudo,
                style: GoogleFonts.manrope(
                  color: const Color(0xFF6B7280),
                  fontSize: 13,
                  height: 1.45,
                ),
              ),
            ],
          ),
        );
      },
    );
  }

  Widget _buildTabEventos() {
    if (_userEventos.isEmpty) {
      return _buildEmptyTab(
        icon: Icons.event_busy_rounded,
        title: 'Nenhum evento criado',
        subtitle: 'Você ainda não organizou nenhum evento.',
      );
    }

    return ListView.builder(
      padding: const EdgeInsets.fromLTRB(16, 16, 16, 90),
      itemCount: _userEventos.length,
      itemBuilder: (context, index) {
        final e = _userEventos[index];
        return Container(
          margin: const EdgeInsets.only(bottom: 12),
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(14),
            border: Border.all(color: const Color(0xFFE5E7EB)),
          ),
          child: ListTile(
            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
            leading: Container(
              padding: const EdgeInsets.all(10),
              decoration: BoxDecoration(
                color: const Color(0xFFF7F8FA),
                borderRadius: BorderRadius.circular(10),
              ),
              child: const Icon(Icons.event, color: Color(0xFFEA3F74), size: 20),
            ),
            title: Text(
              e.titulo,
              style: GoogleFonts.manrope(fontWeight: FontWeight.w700, color: const Color(0xFF202124), fontSize: 14),
            ),
            subtitle: Text(
              '${e.dataFormatada} • ${e.localEvento}',
              style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 12),
            ),
          ),
        );
      },
    );
  }

  Widget _buildTabComunidades() {
    if (_userComunidades.isEmpty) {
      return _buildEmptyTab(
        icon: Icons.groups_outlined,
        title: 'Nenhuma comunidade',
        subtitle: 'Você ainda não faz parte de nenhuma comunidade.',
      );
    }

    return ListView.builder(
      padding: const EdgeInsets.fromLTRB(16, 16, 16, 90),
      itemCount: _userComunidades.length,
      itemBuilder: (context, index) {
        final c = _userComunidades[index];
        return Container(
          margin: const EdgeInsets.only(bottom: 12),
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(14),
            border: Border.all(color: const Color(0xFFE5E7EB)),
          ),
          child: ListTile(
            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
            leading: Container(
              padding: const EdgeInsets.all(10),
              decoration: BoxDecoration(
                color: const Color(0xFFF7F8FA),
                borderRadius: BorderRadius.circular(10),
              ),
              child: const Icon(Icons.hub_rounded, color: Color(0xFFEA3F74), size: 20),
            ),
            title: Text(
              c.nome,
              style: GoogleFonts.manrope(fontWeight: FontWeight.w700, color: const Color(0xFF202124), fontSize: 14),
            ),
            subtitle: Text(
              '${c.totalMembros} membros',
              style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 12),
            ),
          ),
        );
      },
    );
  }

  Widget _buildEmptyTab({
    required IconData icon,
    required String title,
    required String subtitle,
  }) {
    return Center(
      child: Padding(
        padding: const EdgeInsets.all(32),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(icon, size: 48, color: const Color(0xFF6B7280)),
            const SizedBox(height: 12),
            Text(
              title,
              style: GoogleFonts.manrope(
                fontWeight: FontWeight.w700,
                fontSize: 15,
                color: const Color(0xFF202124),
              ),
            ),
            const SizedBox(height: 4),
            Text(
              subtitle,
              textAlign: TextAlign.center,
              style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 13),
            ),
          ],
        ),
      ),
    );
  }

  Widget _infoItem(IconData icon, String label, String value) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 14),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Icon(icon, size: 18, color: const Color(0xFFEA3F74)),
          const SizedBox(width: 12),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  label,
                  style: GoogleFonts.manrope(
                      color: const Color(0xFF6B7280),
                      fontSize: 11,
                      fontWeight: FontWeight.w700),
                ),
                const SizedBox(height: 2),
                Text(
                  value,
                  style: GoogleFonts.manrope(
                      color: const Color(0xFF202124),
                      fontSize: 14,
                      fontWeight: FontWeight.w500),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _divider() {
    return const Divider(height: 1, thickness: 1, color: Color(0xFFE5E7EB), indent: 16);
  }
}

class _SliverAppBarDelegate extends SliverPersistentHeaderDelegate {
  final TabBar _tabBar;
  _SliverAppBarDelegate(this._tabBar);

  @override
  double get minExtent => _tabBar.preferredSize.height;
  @override
  double get maxExtent => _tabBar.preferredSize.height;

  @override
  Widget build(BuildContext context, double shrinkOffset, bool overlapsContent) {
    return Container(
      color: const Color(0xFFF7F8FA),
      child: _tabBar,
    );
  }

  @override
  bool shouldRebuild(_SliverAppBarDelegate oldDelegate) {
    return false;
  }
}
