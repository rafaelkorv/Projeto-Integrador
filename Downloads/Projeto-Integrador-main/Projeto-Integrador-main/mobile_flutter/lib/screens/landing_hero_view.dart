import 'package:flutter/material.dart';
import '../services/auth_service.dart';

class LandingHeroView extends StatefulWidget {
  final VoidCallback onExplorar;
  final VoidCallback onEventos;
  final VoidCallback? onMapa;
  final VoidCallback onComunidades;
  final VoidCallback onLogin;
  final VoidCallback onRegister;

  const LandingHeroView({
    super.key,
    required this.onExplorar,
    required this.onEventos,
    this.onMapa,
    required this.onComunidades,
    required this.onLogin,
    required this.onRegister,
  });

  @override
  State<LandingHeroView> createState() => _LandingHeroViewState();
}

class _LandingHeroViewState extends State<LandingHeroView>
    with SingleTickerProviderStateMixin {
  late final AnimationController _animController;
  late final Animation<double> _fadeAnimation;
  late final Animation<Offset> _slideAnimation;
  late final Animation<double> _scaleAnimation;

  @override
  void initState() {
    super.initState();
    _animController = AnimationController(
      vsync: this,
      duration: const Duration(milliseconds: 800),
    );

    _fadeAnimation = CurvedAnimation(
      parent: _animController,
      curve: const Interval(0.0, 0.8, curve: Curves.easeOutCubic),
    );

    _slideAnimation = Tween<Offset>(
      begin: const Offset(0, 0.06),
      end: Offset.zero,
    ).animate(
      CurvedAnimation(
        parent: _animController,
        curve: const Interval(0.1, 1.0, curve: Curves.easeOutCubic),
      ),
    );

    _scaleAnimation = Tween<double>(
      begin: 0.97,
      end: 1.0,
    ).animate(
      CurvedAnimation(
        parent: _animController,
        curve: const Interval(0.1, 1.0, curve: Curves.easeOutCubic),
      ),
    );

    _animController.forward();
  }

  @override
  void dispose() {
    _animController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final size = MediaQuery.of(context).size;
    final bool isDesktop = size.width > 800;

    return SingleChildScrollView(
      physics: const BouncingScrollPhysics(),
      child: Center(
        child: ConstrainedBox(
          constraints: const BoxConstraints(maxWidth: 1140),
          child: Padding(
            padding: EdgeInsets.symmetric(
              horizontal: isDesktop ? 40 : 20,
              vertical: isDesktop ? 36 : 20,
            ),
            child: FadeTransition(
              opacity: _fadeAnimation,
              child: SlideTransition(
                position: _slideAnimation,
                child: ScaleTransition(
                  scale: _scaleAnimation,
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: [
                      // Badge Pill Superior Inspirado em Antigravity
                      _buildTopTag(),
                      const SizedBox(height: 20),

                      // Título Grande e Impactante
                      _buildMainTitle(isDesktop),
                      const SizedBox(height: 16),

                      // Subtítulo Explicativo Clean
                      _buildSubtitle(isDesktop),
                      const SizedBox(height: 32),

                      // Botões de Ação Principais (CTAs)
                      _buildActionButtons(isDesktop),
                      const SizedBox(height: 36),

                      // Imagem Principal (slaq.png) em Card com Acabamento Premium
                      _buildHeroImageCard(isDesktop),
                      const SizedBox(height: 44),

                      // 3 Pilares Visuais (Recursos do SocialJoin)
                      _buildFeaturePillars(isDesktop),
                      const SizedBox(height: 40),

                      // Banner de Convite Inferior
                      if (!AuthService.logado) ...[
                        _buildBottomAuthPrompt(isDesktop),
                        const SizedBox(height: 20),
                      ],
                    ],
                  ),
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }

  /// Badge sutil no topo do Hero
  Widget _buildTopTag() {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
      decoration: BoxDecoration(
        color: const Color(0xFFFDF0F4),
        borderRadius: BorderRadius.circular(30),
        border: Border.all(
          color: const Color(0xFFF9ACC6).withValues(alpha: 0.6),
          width: 1,
        ),
        boxShadow: [
          BoxShadow(
            color: const Color(0xFFEA3F74).withValues(alpha: 0.06),
            blurRadius: 12,
            offset: const Offset(0, 4),
          ),
        ],
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Container(
            width: 8,
            height: 8,
            decoration: const BoxDecoration(
              color: Color(0xFFEA3F74),
              shape: BoxShape.circle,
            ),
          ),
          const SizedBox(width: 8),
          const Flexible(
            child: Text(
              'A rede definitiva para viver momentos inesquecíveis',
              style: TextStyle(
                color: Color(0xFFEA3F74),
                fontSize: 13,
                fontWeight: FontWeight.w600,
                letterSpacing: -0.2,
              ),
              overflow: TextOverflow.ellipsis,
            ),
          ),
        ],
      ),
    );
  }

  /// Título grande com palavra destacada em gradiente rosa
  Widget _buildMainTitle(bool isDesktop) {
    final double fontSize = isDesktop ? 54 : 34;

    return RichText(
      textAlign: TextAlign.center,
      text: TextSpan(
        style: TextStyle(
          fontFamily: 'Roboto',
          fontSize: fontSize,
          fontWeight: FontWeight.w800,
          color: const Color(0xFF0F172A),
          letterSpacing: -1.4,
          height: 1.15,
        ),
        children: [
          const TextSpan(text: 'Encontre pessoas.\nViva '),
          WidgetSpan(
            alignment: PlaceholderAlignment.baseline,
            baseline: TextBaseline.alphabetic,
            child: ShaderMask(
              shaderCallback: (bounds) => const LinearGradient(
                colors: [Color(0xFFF9ACC6), Color(0xFFEA3F74)],
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ).createShader(bounds),
              child: Text(
                'experiências.',
                style: TextStyle(
                  fontFamily: 'Roboto',
                  fontSize: fontSize,
                  fontWeight: FontWeight.w900,
                  color: Colors.white,
                  letterSpacing: -1.4,
                  height: 1.15,
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  /// Subtítulo explicativo com espaçamento confortável
  Widget _buildSubtitle(bool isDesktop) {
    return ConstrainedBox(
      constraints: const BoxConstraints(maxWidth: 620),
      child: Text(
        'Conecte-se com pessoas que compartilham seus interesses, descubra eventos exclusivos perto de você e viva a melhor experiência social.',
        textAlign: TextAlign.center,
        style: TextStyle(
          fontSize: isDesktop ? 18 : 15,
          fontWeight: FontWeight.w400,
          color: const Color(0xFF64748B),
          height: 1.55,
          letterSpacing: -0.2,
        ),
      ),
    );
  }

  /// Botões de Ação Principais (CTAs)
  Widget _buildActionButtons(bool isDesktop) {
    return Wrap(
      spacing: 14,
      runSpacing: 12,
      alignment: WrapAlignment.center,
      crossAxisAlignment: WrapCrossAlignment.center,
      children: [
        // CTA Primário - Antigravity Dark Pill Button com toque rosa
        Container(
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(30),
            gradient: const LinearGradient(
              colors: [Color(0xFF1E293B), Color(0xFF0F172A)],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
            boxShadow: [
              BoxShadow(
                color: const Color(0xFF0F172A).withValues(alpha: 0.18),
                blurRadius: 18,
                offset: const Offset(0, 6),
              ),
            ],
          ),
          child: ElevatedButton(
            onPressed: widget.onExplorar,
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.transparent,
              shadowColor: Colors.transparent,
              foregroundColor: Colors.white,
              padding: EdgeInsets.symmetric(
                horizontal: isDesktop ? 28 : 22,
                vertical: isDesktop ? 18 : 14,
              ),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(30),
              ),
            ),
            child: const Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Icon(Icons.explore_rounded, color: Color(0xFFF9ACC6), size: 20),
                SizedBox(width: 10),
                Text(
                  'Explorar SocialJoin',
                  style: TextStyle(
                    fontSize: 15,
                    fontWeight: FontWeight.w700,
                    letterSpacing: -0.2,
                  ),
                ),
                SizedBox(width: 6),
                Icon(Icons.arrow_forward_rounded, color: Colors.white70, size: 18),
              ],
            ),
          ),
        ),

        // CTA Secundário - Agenda de Eventos
        Container(
          decoration: BoxDecoration(
            color: Colors.white,
            borderRadius: BorderRadius.circular(30),
            border: Border.all(color: const Color(0xFFE2E8F0), width: 1.2),
            boxShadow: [
              BoxShadow(
                color: Colors.black.withValues(alpha: 0.03),
                blurRadius: 10,
                offset: const Offset(0, 3),
              ),
            ],
          ),
          child: TextButton(
            onPressed: widget.onEventos,
            style: TextButton.styleFrom(
              foregroundColor: const Color(0xFF0F172A),
              padding: EdgeInsets.symmetric(
                horizontal: isDesktop ? 24 : 18,
                vertical: isDesktop ? 18 : 14,
              ),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(30),
              ),
            ),
            child: const Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Icon(Icons.calendar_month_rounded, color: Color(0xFFEA3F74), size: 19),
                SizedBox(width: 8),
                Text(
                  'Ver Eventos',
                  style: TextStyle(
                    fontSize: 15,
                    fontWeight: FontWeight.w600,
                    color: Color(0xFF334155),
                    letterSpacing: -0.2,
                  ),
                ),
              ],
            ),
          ),
        ),

        // CTA Terciário - Comunidades
        Container(
          decoration: BoxDecoration(
            color: const Color(0xFFFDF0F4),
            borderRadius: BorderRadius.circular(30),
            border: Border.all(
              color: const Color(0xFFF9ACC6).withValues(alpha: 0.6),
              width: 1.2,
            ),
          ),
          child: TextButton(
            onPressed: widget.onComunidades,
            style: TextButton.styleFrom(
              foregroundColor: const Color(0xFFEA3F74),
              padding: EdgeInsets.symmetric(
                horizontal: isDesktop ? 22 : 16,
                vertical: isDesktop ? 18 : 14,
              ),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(30),
              ),
            ),
            child: const Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Icon(Icons.groups_rounded, color: Color(0xFFEA3F74), size: 19),
                SizedBox(width: 8),
                Text(
                  'Comunidades',
                  style: TextStyle(
                    fontSize: 15,
                    fontWeight: FontWeight.w700,
                    color: Color(0xFFEA3F74),
                    letterSpacing: -0.2,
                  ),
                ),
              ],
            ),
          ),
        ),

        // CTA Quaternário - Mapa
        if (widget.onMapa != null)
          Container(
            decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.circular(30),
              border: Border.all(color: const Color(0xFFE2E8F0), width: 1.2),
              boxShadow: [
                BoxShadow(
                  color: Colors.black.withValues(alpha: 0.03),
                  blurRadius: 10,
                  offset: const Offset(0, 3),
                ),
              ],
            ),
            child: TextButton(
              onPressed: widget.onMapa,
              style: TextButton.styleFrom(
                foregroundColor: const Color(0xFF0F172A),
                padding: EdgeInsets.symmetric(
                  horizontal: isDesktop ? 22 : 16,
                  vertical: isDesktop ? 18 : 14,
                ),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(30),
                ),
              ),
              child: const Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Icon(Icons.map_outlined, color: Color(0xFFEA3F74), size: 19),
                  SizedBox(width: 8),
                  Text(
                    'Mapa',
                    style: TextStyle(
                      fontSize: 15,
                      fontWeight: FontWeight.w600,
                      color: Color(0xFF334155),
                      letterSpacing: -0.2,
                    ),
                  ),
                ],
              ),
            ),
          ),
      ],
    );
  }

  /// Imagem Principal (slaq.png) com Card Premium e Badges Flutuantes
  Widget _buildHeroImageCard(bool isDesktop) {
    return Container(
      width: double.infinity,
      constraints: BoxConstraints(
        maxHeight: isDesktop ? 480 : 320,
      ),
      decoration: BoxDecoration(
        borderRadius: BorderRadius.circular(28),
        boxShadow: [
          // Sombra ambiente suave com tom rosa
          BoxShadow(
            color: const Color(0xFFEA3F74).withValues(alpha: 0.12),
            blurRadius: 36,
            spreadRadius: -4,
            offset: const Offset(0, 16),
          ),
          BoxShadow(
            color: Colors.black.withValues(alpha: 0.08),
            blurRadius: 20,
            offset: const Offset(0, 8),
          ),
        ],
      ),
      child: ClipRRect(
        borderRadius: BorderRadius.circular(28),
        child: Stack(
          alignment: Alignment.center,
          children: [
            // Imagem slaq.png
            Image.asset(
              'assets/images/slaq.png',
              width: double.infinity,
              height: double.infinity,
              fit: BoxFit.cover,
              errorBuilder: (context, error, stackTrace) {
                return Container(
                  width: double.infinity,
                  height: double.infinity,
                  decoration: const BoxDecoration(
                    gradient: LinearGradient(
                      colors: [Color(0xFF1E293B), Color(0xFFEA3F74)],
                      begin: Alignment.topLeft,
                      end: Alignment.bottomRight,
                    ),
                  ),
                  child: const Center(
                    child: Icon(Icons.celebration, color: Colors.white70, size: 64),
                  ),
                );
              },
            ),

            // Sutil Overlay Gradiente para contraste
            Container(
              decoration: BoxDecoration(
                gradient: LinearGradient(
                  colors: [
                    Colors.black.withValues(alpha: 0.35),
                    Colors.transparent,
                    const Color(0xFF0F172A).withValues(alpha: 0.65),
                  ],
                  begin: Alignment.topCenter,
                  end: Alignment.bottomCenter,
                  stops: const [0.0, 0.45, 1.0],
                ),
              ),
            ),

            // Sutil Brilho Rosa Decorativo
            Positioned(
              top: -40,
              right: -40,
              child: Container(
                width: 200,
                height: 200,
                decoration: BoxDecoration(
                  shape: BoxShape.circle,
                  color: const Color(0xFFF9ACC6).withValues(alpha: 0.35),
                ),
              ),
            ),

            // Badge Flutuante Superior: "+2.4k pessoas conectadas hoje"
            Positioned(
              top: isDesktop ? 20 : 14,
              left: isDesktop ? 20 : 14,
              child: Container(
                padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 8),
                decoration: BoxDecoration(
                  color: Colors.black.withValues(alpha: 0.55),
                  borderRadius: BorderRadius.circular(20),
                  border: Border.all(
                    color: Colors.white.withValues(alpha: 0.2),
                    width: 1,
                  ),
                ),
                child: const Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Text('🔥', style: TextStyle(fontSize: 14)),
                    SizedBox(width: 6),
                    Text(
                      '+2.4k pessoas conectadas hoje',
                      style: TextStyle(
                        color: Colors.white,
                        fontSize: 12,
                        fontWeight: FontWeight.w600,
                        letterSpacing: -0.2,
                      ),
                    ),
                  ],
                ),
              ),
            ),

            // Card Flutuante Inferior com Ação Rápida
            Positioned(
              bottom: isDesktop ? 20 : 14,
              left: isDesktop ? 20 : 14,
              right: isDesktop ? 20 : 14,
              child: Container(
                padding: EdgeInsets.symmetric(
                  horizontal: isDesktop ? 20 : 14,
                  vertical: isDesktop ? 14 : 10,
                ),
                decoration: BoxDecoration(
                  color: Colors.black.withValues(alpha: 0.65),
                  borderRadius: BorderRadius.circular(18),
                  border: Border.all(
                    color: Colors.white.withValues(alpha: 0.18),
                    width: 1,
                  ),
                ),
                child: Row(
                  children: [
                    Container(
                      padding: const EdgeInsets.all(8),
                      decoration: BoxDecoration(
                        gradient: const LinearGradient(
                          colors: [Color(0xFFF9ACC6), Color(0xFFEA3F74)],
                        ),
                        borderRadius: BorderRadius.circular(12),
                      ),
                      child: const Icon(
                        Icons.local_fire_department_rounded,
                        color: Colors.white,
                        size: 20,
                      ),
                    ),
                    const SizedBox(width: 12),
                    const Expanded(
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          Text(
                            'Descubra o que está rolando agora',
                            style: TextStyle(
                              color: Colors.white,
                              fontSize: 13,
                              fontWeight: FontWeight.bold,
                            ),
                          ),
                          Text(
                            'Eventos, encontros e conversas em tempo real',
                            style: TextStyle(
                              color: Colors.white70,
                              fontSize: 11,
                            ),
                            overflow: TextOverflow.ellipsis,
                          ),
                        ],
                      ),
                    ),
                    const SizedBox(width: 8),
                    InkWell(
                      onTap: widget.onExplorar,
                      borderRadius: BorderRadius.circular(12),
                      child: Container(
                        padding: const EdgeInsets.symmetric(
                          horizontal: 12,
                          vertical: 8,
                        ),
                        decoration: BoxDecoration(
                          color: const Color(0xFFEA3F74),
                          borderRadius: BorderRadius.circular(12),
                        ),
                        child: const Row(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            Text(
                              'Acessar',
                              style: TextStyle(
                                color: Colors.white,
                                fontSize: 12,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                            SizedBox(width: 4),
                            Icon(Icons.chevron_right, color: Colors.white, size: 16),
                          ],
                        ),
                      ),
                    ),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  /// 3 Pilares Visuais do SocialJoin (Clean & Modern)
  Widget _buildFeaturePillars(bool isDesktop) {
    final features = [
      {
        'icon': Icons.dynamic_feed_rounded,
        'title': 'Feed em Tempo Real',
        'desc': 'Compartilhe momentos, fotos e acompanhe novidades da sua rede.',
        'action': widget.onExplorar,
        'tag': 'Feed Aberto',
      },
      {
        'icon': Icons.map_outlined,
        'title': 'Eventos & Mapa',
        'desc': 'Encontre festivais, encontros de tech, música e cultura perto de você.',
        'action': widget.onEventos,
        'tag': 'Agenda Ativa',
      },
      {
        'icon': Icons.hub_outlined,
        'title': 'Comunidades',
        'desc': 'Participe de grupos de tecnologia, esportes, arte e lazer.',
        'action': widget.onComunidades,
        'tag': 'Grupos Reais',
      },
    ];

    if (!isDesktop) {
      return Column(
        children: features
            .map((f) => Padding(
                  padding: const EdgeInsets.only(bottom: 12),
                  child: _buildFeatureCard(f),
                ))
            .toList(),
      );
    }

    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: features
          .map((f) => Expanded(
                child: Padding(
                  padding: const EdgeInsets.symmetric(horizontal: 8),
                  child: _buildFeatureCard(f),
                ),
              ))
          .toList(),
    );
  }

  Widget _buildFeatureCard(Map<String, dynamic> f) {
    return InkWell(
      onTap: f['action'] as VoidCallback,
      borderRadius: BorderRadius.circular(20),
      child: Container(
        padding: const EdgeInsets.all(20),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(20),
          border: Border.all(color: const Color(0xFFE2E8F0)),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withValues(alpha: 0.02),
              blurRadius: 12,
              offset: const Offset(0, 4),
            ),
          ],
        ),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Container(
                  padding: const EdgeInsets.all(10),
                  decoration: BoxDecoration(
                    color: const Color(0xFFFDF0F4),
                    borderRadius: BorderRadius.circular(14),
                  ),
                  child: Icon(
                    f['icon'] as IconData,
                    color: const Color(0xFFEA3F74),
                    size: 22,
                  ),
                ),
                Container(
                  padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 3),
                  decoration: BoxDecoration(
                    color: const Color(0xFFF1F5F9),
                    borderRadius: BorderRadius.circular(12),
                  ),
                  child: Text(
                    f['tag'] as String,
                    style: const TextStyle(
                      color: Color(0xFF64748B),
                      fontSize: 11,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                ),
              ],
            ),
            const SizedBox(height: 14),
            Text(
              f['title'] as String,
              style: const TextStyle(
                fontSize: 16,
                fontWeight: FontWeight.bold,
                color: Color(0xFF0F172A),
                letterSpacing: -0.3,
              ),
            ),
            const SizedBox(height: 6),
            Text(
              f['desc'] as String,
              style: const TextStyle(
                fontSize: 13,
                color: Color(0xFF64748B),
                height: 1.4,
              ),
            ),
            const SizedBox(height: 12),
            const Row(
              children: [
                Text(
                  'Ver detalhes',
                  style: TextStyle(
                    fontSize: 13,
                    fontWeight: FontWeight.w700,
                    color: Color(0xFFEA3F74),
                  ),
                ),
                SizedBox(width: 4),
                Icon(
                  Icons.arrow_forward_rounded,
                  color: Color(0xFFEA3F74),
                  size: 14,
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  /// Banner de Convite Inferior
  Widget _buildBottomAuthPrompt(bool isDesktop) {
    return Container(
      width: double.infinity,
      padding: EdgeInsets.symmetric(
        horizontal: isDesktop ? 36 : 20,
        vertical: isDesktop ? 28 : 20,
      ),
      decoration: BoxDecoration(
        gradient: const LinearGradient(
          colors: [Color(0xFFFFF1F5), Color(0xFFFDF4F7)],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(24),
        border: Border.all(
          color: const Color(0xFFF9ACC6).withValues(alpha: 0.5),
        ),
      ),
      child: isDesktop
          ? Row(
              children: [
                const Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        'Faça parte da nossa comunidade',
                        style: TextStyle(
                          fontSize: 18,
                          fontWeight: FontWeight.w800,
                          color: Color(0xFF0F172A),
                          letterSpacing: -0.3,
                        ),
                      ),
                      SizedBox(height: 4),
                      Text(
                        'Crie sua conta gratuitamente e conecte-se hoje mesmo.',
                        style: TextStyle(
                          fontSize: 14,
                          color: Color(0xFF64748B),
                        ),
                      ),
                    ],
                  ),
                ),
                const SizedBox(width: 20),
                _buildAuthButtonsRow(),
              ],
            )
          : Column(
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                const Text(
                  'Faça parte da nossa comunidade',
                  textAlign: TextAlign.center,
                  style: TextStyle(
                    fontSize: 17,
                    fontWeight: FontWeight.w800,
                    color: Color(0xFF0F172A),
                    letterSpacing: -0.3,
                  ),
                ),
                const SizedBox(height: 4),
                const Text(
                  'Crie sua conta gratuitamente ou faça login.',
                  textAlign: TextAlign.center,
                  style: TextStyle(
                    fontSize: 13,
                    color: Color(0xFF64748B),
                  ),
                ),
                const SizedBox(height: 16),
                _buildAuthButtonsRow(),
              ],
            ),
    );
  }

  Widget _buildAuthButtonsRow() {
    return Wrap(
      spacing: 10,
      runSpacing: 10,
      alignment: WrapAlignment.center,
      children: [
        OutlinedButton(
          onPressed: widget.onLogin,
          style: OutlinedButton.styleFrom(
            foregroundColor: const Color(0xFF0F172A),
            side: const BorderSide(color: Color(0xFFCBD5E1)),
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
            padding: const EdgeInsets.symmetric(horizontal: 18, vertical: 12),
          ),
          child: const Text(
            'Entrar',
            style: TextStyle(fontWeight: FontWeight.w700, fontSize: 13),
          ),
        ),
        ElevatedButton(
          onPressed: widget.onRegister,
          style: ElevatedButton.styleFrom(
            backgroundColor: const Color(0xFFEA3F74),
            foregroundColor: Colors.white,
            elevation: 0,
            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
            padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 12),
          ),
          child: const Text(
            'Criar Conta',
            style: TextStyle(fontWeight: FontWeight.w700, fontSize: 13),
          ),
        ),
      ],
    );
  }
}
