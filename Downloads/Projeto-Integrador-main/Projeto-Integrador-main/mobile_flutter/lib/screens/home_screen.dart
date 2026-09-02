import 'dart:async';
import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import '../services/auth_service.dart';
import '../services/notification_service.dart';
import 'feed_page.dart';
import 'eventos_page.dart';
import 'map_events_page.dart';
import 'communities_page.dart';
import 'usuarios_screen.dart';
import 'login_screen.dart';
import 'register_screen.dart';
import 'profile_page.dart';
import 'mobile_welcome_view.dart';
import 'mobile_event_search_page.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  int paginaAtual = 0;
  int? _eventoNotificacaoId;
  StreamSubscription<String>? _notificationSub;

  final titles = const [
    "Início",
    "Pesquisar",
    "Eventos",
    "Comunidades",
    "Perfil",
    "Mapa",
    "Usuários",
  ];

  @override
  void initState() {
    super.initState();
    NotificationService().inicializar();
    _notificationSub = NotificationService.onNotificationClick.stream.listen((payload) {
      final id = int.tryParse(payload);
      if (id != null && mounted) {
        setState(() {
          _eventoNotificacaoId = id;
          paginaAtual = 5; // Redireciona diretamente para o Mapa de Eventos
        });
      }
    });
  }

  @override
  void dispose() {
    _notificationSub?.cancel();
    super.dispose();
  }

  void _selecionarPagina(int index) {
    setState(() {
      paginaAtual = index;
      if (index != 5) {
        _eventoNotificacaoId = null;
      }
    });
  }

  void _abrirLogin() {
    Navigator.push(
      context,
      MaterialPageRoute(builder: (_) => const LoginScreen()),
    ).then((_) => setState(() {}));
  }

  void _abrirCadastro() {
    Navigator.push(
      context,
      MaterialPageRoute(builder: (_) => const RegisterScreen()),
    ).then((_) => setState(() {}));
  }

  void _fazerLogout() {
    AuthService.fazerLogout();
    setState(() {
      paginaAtual = 0;
    });
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text('Você saiu da sua conta.', style: GoogleFonts.manrope()),
        duration: const Duration(seconds: 2),
      ),
    );
  }

  Widget _buildBody() {
    switch (paginaAtual) {
      case 0:
        return const FeedPage();
      case 1:
        return MobileEventSearchPage(
          onVerNoMapa: (id) {
            setState(() {
              _eventoNotificacaoId = id;
              paginaAtual = 5;
            });
          },
        );
      case 2:
        return const EventosPage();
      case 3:
        return const CommunitiesPage();
      case 4:
        if (AuthService.logado) {
          return const ProfilePage();
        }
        return MobileWelcomeView(
          onLogin: _abrirLogin,
          onRegister: _abrirCadastro,
          onExplorar: () => _selecionarPagina(0),
        );
      case 5:
        return MapEventsPage(
          eventoInicialId: _eventoNotificacaoId,
          key: ValueKey(_eventoNotificacaoId),
        );
      case 6:
        return const UsuarioScreen();
      default:
        return const FeedPage();
    }
  }

  @override
  Widget build(BuildContext context) {
    final size = MediaQuery.of(context).size;
    final bool isDesktop = size.width > 920;

    return Scaffold(
      backgroundColor: const Color(0xFFF5F7FF),
      appBar: PreferredSize(
        preferredSize: const Size.fromHeight(72),
        child: Container(
          decoration: const BoxDecoration(
            gradient: LinearGradient(
              begin: Alignment.topCenter,
              end: Alignment.bottomCenter,
              colors: [
                Colors.white,
                Color(0xFFF9FAFF),
              ],
            ),
            border: Border(
              bottom: BorderSide(color: Color(0xFFE5E7EB), width: 1.0),
            ),
          ),
          child: SafeArea(
            child: Padding(
              padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 10),
              child: Row(
                children: [
                  // Logotipo SocialJoin (Web brand mark style)
                  Flexible(
                    child: _buildBrandLogo(),
                  ),

                  if (isDesktop) ...[
                    const SizedBox(width: 16),
                    Expanded(
                      child: Center(
                        child: SingleChildScrollView(
                          scrollDirection: Axis.horizontal,
                          child: Row(
                            mainAxisSize: MainAxisSize.min,
                            children: [
                              _buildDesktopNavLink(0, "Início"),
                              _buildDesktopNavLink(1, "Pesquisar"),
                              _buildDesktopNavLink(2, "Eventos"),
                              _buildDesktopNavLink(5, "Mapa"),
                              _buildDesktopNavLink(3, "Comunidades"),
                              _buildDesktopNavLink(6, "Usuários"),
                            ],
                          ),
                        ),
                      ),
                    ),
                  ] else ...[
                    const Spacer(),
                  ],

                  // Ações Rápidas do Topo (Web style header icons)
                  Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      _buildHeaderIconButton(
                        icon: Icons.search_rounded,
                        isActive: paginaAtual == 1,
                        onTap: () => _selecionarPagina(1),
                        tooltip: 'Pesquisar Eventos',
                      ),
                      _buildHeaderIconButton(
                        icon: paginaAtual == 5 ? Icons.map_rounded : Icons.map_outlined,
                        isActive: paginaAtual == 5,
                        onTap: () => _selecionarPagina(5),
                        tooltip: 'Mapa de Eventos',
                      ),
                      const SizedBox(width: 6),

                      // Account Trigger (.account-trigger from Web CSS)
                      if (AuthService.logado)
                        GestureDetector(
                          onTap: () => _selecionarPagina(4),
                          child: Container(
                            padding: const EdgeInsets.all(3),
                            decoration: BoxDecoration(
                              color: Colors.white,
                              borderRadius: BorderRadius.circular(999),
                              border: Border.all(
                                color: paginaAtual == 4 ? const Color(0xFFEA3F74) : const Color(0xFFE5E7EB),
                                width: 1.5,
                              ),
                            ),
                            child: CircleAvatar(
                              backgroundColor: const Color(0xFFEA3F74),
                              radius: 14,
                              child: Text(
                                (AuthService.nomeUsuario ?? 'U')[0].toUpperCase(),
                                style: GoogleFonts.manrope(
                                  color: Colors.white,
                                  fontWeight: FontWeight.w800,
                                  fontSize: 12,
                                ),
                              ),
                            ),
                          ),
                        )
                      else
                        InkWell(
                          onTap: _abrirLogin,
                          borderRadius: BorderRadius.circular(10),
                          child: Container(
                            padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 7),
                            decoration: BoxDecoration(
                              color: const Color(0xFFEA3F74),
                              borderRadius: BorderRadius.circular(10),
                            ),
                            child: Text(
                              'Entrar',
                              style: GoogleFonts.manrope(
                                color: Colors.white,
                                fontSize: 13,
                                fontWeight: FontWeight.w700,
                              ),
                            ),
                          ),
                        ),

                      const SizedBox(width: 4),

                      Builder(
                        builder: (scaffoldContext) => _buildHeaderIconButton(
                          icon: Icons.menu_rounded,
                          isActive: false,
                          onTap: () => Scaffold.of(scaffoldContext).openEndDrawer(),
                          tooltip: 'Menu',
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
      endDrawer: _buildDrawer(isDesktop),
      body: _buildBody(),

      // Navigation bar estilo Web mobile (.nav-menu da Web CSS)
      bottomNavigationBar: isDesktop ? null : _buildWebStyleBottomNav(),
    );
  }

  Widget _buildHeaderIconButton({
    required IconData icon,
    required bool isActive,
    required VoidCallback onTap,
    String? tooltip,
  }) {
    return Container(
      margin: const EdgeInsets.only(left: 2),
      decoration: BoxDecoration(
        color: isActive ? const Color(0xFFEA3F74).withValues(alpha: 0.08) : Colors.white,
        borderRadius: BorderRadius.circular(12),
        border: Border.all(
          color: isActive ? const Color(0xFFF7B7CD) : const Color(0xFFE5E7EB),
          width: 1,
        ),
      ),
      child: IconButton(
        constraints: const BoxConstraints(minWidth: 38, minHeight: 38),
        padding: EdgeInsets.zero,
        icon: Icon(
          icon,
          color: isActive ? const Color(0xFFEA3F74) : const Color(0xFF202124),
          size: 22,
        ),
        onPressed: onTap,
        tooltip: tooltip,
      ),
    );
  }

  /// Navigation Bar Fiel ao `.nav-menu` e `.nav-btn` da versão Web em telas mobile
  Widget _buildWebStyleBottomNav() {
    final int navIndex = _mapPaginaParaBottomNav(paginaAtual);

    return Container(
      padding: const EdgeInsets.fromLTRB(12, 8, 12, 12),
      decoration: const BoxDecoration(
        color: Colors.white,
        border: Border(
          top: BorderSide(color: Color(0xFFE5E7EB), width: 1.0),
        ),
      ),
      child: SafeArea(
        child: Container(
          padding: const EdgeInsets.all(6),
          decoration: BoxDecoration(
            color: const Color(0xFFF5F7FF),
            borderRadius: BorderRadius.circular(18),
          ),
          child: Row(
            children: [
              _buildNavTabItem(0, Icons.dynamic_feed_rounded, "Feed", navIndex == 0),
              _buildNavTabItem(1, Icons.search_rounded, "Buscar", navIndex == 1),
              _buildNavTabItem(2, Icons.event_rounded, "Eventos", navIndex == 2),
              _buildNavTabItem(3, Icons.groups_rounded, "Grupos", navIndex == 3),
              _buildNavTabItem(4, Icons.person_rounded, AuthService.logado ? "Perfil" : "Conta", navIndex == 4),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildNavTabItem(int index, IconData icon, String label, bool isActive) {
    return Expanded(
      child: InkWell(
        onTap: () {
          final pageIndex = _mapBottomNavParaPagina(index);
          _selecionarPagina(pageIndex);
        },
        child: AnimatedContainer(
          duration: const Duration(milliseconds: 180),
          padding: const EdgeInsets.symmetric(vertical: 8),
          margin: const EdgeInsets.symmetric(horizontal: 2),
          decoration: BoxDecoration(
            color: isActive ? Colors.white : Colors.transparent,
            borderRadius: BorderRadius.circular(14),
            boxShadow: isActive
                ? const [
                    BoxShadow(
                      color: Color.fromRGBO(234, 63, 116, 0.16),
                      blurRadius: 14,
                      offset: Offset(0, 5),
                    ),
                  ]
                : null,
          ),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Icon(
                icon,
                color: isActive ? const Color(0xFFEA3F74) : const Color(0xFF6B7280),
                size: 22,
              ),
              const SizedBox(height: 3),
              Text(
                label,
                style: GoogleFonts.manrope(
                  fontSize: 11,
                  fontWeight: isActive ? FontWeight.w800 : FontWeight.w600,
                  color: isActive ? const Color(0xFFEA3F74) : const Color(0xFF6B7280),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }

  int _mapPaginaParaBottomNav(int page) {
    switch (page) {
      case 0:
        return 0; // Feed
      case 1:
        return 1; // Pesquisar
      case 2:
        return 2; // Eventos
      case 3:
        return 3; // Grupos/Comunidades
      case 4:
        return 4; // Perfil / Conta
      default:
        return 0;
    }
  }

  int _mapBottomNavParaPagina(int navIndex) {
    switch (navIndex) {
      case 0:
        return 0; // Feed
      case 1:
        return 1; // Pesquisar
      case 2:
        return 2; // Eventos
      case 3:
        return 3; // Comunidades
      case 4:
        return 4; // Perfil / Welcome
      default:
        return 0;
    }
  }

  /// Logo Fiel ao `.welcome-brand-mark` e `.brand` da Web
  Widget _buildBrandLogo() {
    return InkWell(
      onTap: () => _selecionarPagina(0),
      borderRadius: BorderRadius.circular(11),
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 2, vertical: 2),
        child: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            Container(
              width: 34,
              height: 34,
              decoration: BoxDecoration(
                color: const Color(0xFFEA3F74),
                borderRadius: BorderRadius.circular(11),
                boxShadow: const [
                  BoxShadow(
                    color: Color.fromRGBO(234, 63, 116, 0.25),
                    blurRadius: 14,
                    offset: Offset(0, 6),
                  ),
                ],
              ),
              child: const Icon(
                Icons.hub_rounded,
                color: Colors.white,
                size: 18,
              ),
            ),
            const SizedBox(width: 8),
            Flexible(
              child: FittedBox(
                fit: BoxFit.scaleDown,
                child: RichText(
                  text: TextSpan(
                    style: GoogleFonts.manrope(
                      fontSize: 20,
                      fontWeight: FontWeight.w800,
                      letterSpacing: -0.5,
                      color: const Color(0xFF202124),
                    ),
                    children: const [
                      TextSpan(text: 'Social'),
                      TextSpan(
                        text: 'Join',
                        style: TextStyle(
                          color: Color(0xFFEA3F74),
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildDesktopNavLink(int index, String label) {
    final bool isSelected = paginaAtual == index;

    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 4),
      child: TextButton(
        onPressed: () => _selecionarPagina(index),
        style: TextButton.styleFrom(
          backgroundColor: isSelected ? const Color(0xFFF7F8FA) : Colors.transparent,
          foregroundColor: isSelected ? const Color(0xFFEA3F74) : const Color(0xFF6B7280),
          padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 10),
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(10),
          ),
        ),
        child: Text(
          label,
          style: GoogleFonts.manrope(
            fontSize: 14,
            fontWeight: isSelected ? FontWeight.w800 : FontWeight.w600,
          ),
        ),
      ),
    );
  }

  Widget _buildDrawer(bool isDesktop) {
    return Drawer(
      backgroundColor: Colors.white,
      child: Column(
        children: [
          SafeArea(
            bottom: false,
            child: Padding(
              padding: const EdgeInsets.fromLTRB(20, 20, 20, 16),
              child: Row(
                children: [
                  Container(
                    width: 38,
                    height: 38,
                    decoration: BoxDecoration(
                      color: const Color(0xFFEA3F74),
                      borderRadius: BorderRadius.circular(11),
                      boxShadow: const [
                        BoxShadow(
                          color: Color.fromRGBO(234, 63, 116, 0.25),
                          blurRadius: 12,
                          offset: Offset(0, 4),
                        ),
                      ],
                    ),
                    child: const Icon(Icons.hub_rounded, color: Colors.white, size: 20),
                  ),
                  const SizedBox(width: 12),
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          'SocialJoin',
                          style: GoogleFonts.manrope(
                            color: const Color(0xFF202124),
                            fontSize: 18,
                            fontWeight: FontWeight.w800,
                          ),
                        ),
                        Text(
                          'Sua rede social de eventos',
                          style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 12),
                          overflow: TextOverflow.ellipsis,
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ),
          ),
          const Divider(color: Color(0xFFE5E7EB), height: 1),
          const SizedBox(height: 8),

          Expanded(
            child: ListView(
              padding: const EdgeInsets.symmetric(horizontal: 12),
              children: [
                _buildDrawerItem(0, Icons.dynamic_feed_outlined, Icons.dynamic_feed_rounded, "Início"),
                _buildDrawerItem(1, Icons.search_rounded, Icons.search_rounded, "Pesquisar"),
                _buildDrawerItem(2, Icons.calendar_month_outlined, Icons.calendar_month_rounded, "Eventos"),
                _buildDrawerItem(5, Icons.map_outlined, Icons.map_rounded, "Mapa"),
                _buildDrawerItem(3, Icons.groups_outlined, Icons.groups_rounded, "Comunidades"),
                if (isDesktop)
                  _buildDrawerItem(6, Icons.people_outline_rounded, Icons.people_rounded, "Usuários"),
                if (AuthService.logado)
                  _buildDrawerItem(4, Icons.person_outline_rounded, Icons.person_rounded, "Meu Perfil"),
              ],
            ),
          ),

          const Divider(color: Color(0xFFE5E7EB), height: 1),

          Padding(
            padding: const EdgeInsets.all(16),
            child: AuthService.logado
                ? Container(
                    padding: const EdgeInsets.all(12),
                    decoration: BoxDecoration(
                      color: const Color(0xFFF7F8FA),
                      borderRadius: BorderRadius.circular(12),
                      border: Border.all(color: const Color(0xFFE5E7EB)),
                    ),
                    child: Row(
                      children: [
                        CircleAvatar(
                          backgroundColor: const Color(0xFFEA3F74),
                          radius: 18,
                          child: Text(
                            (AuthService.nomeUsuario ?? 'U')[0].toUpperCase(),
                            style: GoogleFonts.manrope(color: Colors.white, fontWeight: FontWeight.bold),
                          ),
                        ),
                        const SizedBox(width: 10),
                        Expanded(
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                AuthService.nomeUsuario ?? 'Usuário',
                                style: GoogleFonts.manrope(
                                  color: const Color(0xFF202124),
                                  fontWeight: FontWeight.w700,
                                  fontSize: 13,
                                ),
                                overflow: TextOverflow.ellipsis,
                              ),
                              Text(
                                AuthService.username != null ? '@${AuthService.username}' : 'Conectado',
                                style: GoogleFonts.manrope(color: const Color(0xFFEA3F74), fontSize: 11, fontWeight: FontWeight.w600),
                              ),
                            ],
                          ),
                        ),
                        IconButton(
                          icon: const Icon(Icons.logout_rounded, color: Color(0xFFC93659), size: 20),
                          onPressed: () {
                            Navigator.pop(context);
                            _fazerLogout();
                          },
                          tooltip: 'Sair',
                        ),
                      ],
                    ),
                  )
                : Column(
                    children: [
                      SizedBox(
                        width: double.infinity,
                        height: 44,
                        child: ElevatedButton(
                          style: ElevatedButton.styleFrom(
                            backgroundColor: const Color(0xFFEA3F74),
                            foregroundColor: Colors.white,
                            elevation: 0,
                            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                          ),
                          onPressed: () {
                            Navigator.pop(context);
                            _abrirLogin();
                          },
                          child: Text("Fazer Login", style: GoogleFonts.manrope(fontSize: 14, fontWeight: FontWeight.w700)),
                        ),
                      ),
                      const SizedBox(height: 8),
                      SizedBox(
                        width: double.infinity,
                        height: 44,
                        child: OutlinedButton(
                          style: OutlinedButton.styleFrom(
                            foregroundColor: const Color(0xFF202124),
                            side: const BorderSide(color: Color(0xFFE5E7EB)),
                            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                          ),
                          onPressed: () {
                            Navigator.pop(context);
                            _abrirCadastro();
                          },
                          child: Text("Criar Conta", style: GoogleFonts.manrope(fontSize: 14, fontWeight: FontWeight.w700)),
                        ),
                      ),
                    ],
                  ),
          ),
        ],
      ),
    );
  }

  Widget _buildDrawerItem(int index, IconData icon, IconData activeIcon, String title) {
    final bool isSelected = paginaAtual == index;

    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 2),
      child: ListTile(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
        tileColor: isSelected ? const Color(0xFFF7F8FA) : Colors.transparent,
        dense: true,
        onTap: () {
          _selecionarPagina(index);
          Navigator.pop(context);
        },
        leading: Icon(
          isSelected ? activeIcon : icon,
          color: isSelected ? const Color(0xFFEA3F74) : const Color(0xFF6B7280),
          size: 20,
        ),
        title: Text(
          title,
          style: GoogleFonts.manrope(
            color: isSelected ? const Color(0xFFEA3F74) : const Color(0xFF202124),
            fontSize: 14,
            fontWeight: isSelected ? FontWeight.w800 : FontWeight.w600,
          ),
        ),
      ),
    );
  }
}
