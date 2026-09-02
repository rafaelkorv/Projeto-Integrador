import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';

/// Tela de Boas-Vindas Mobile — Fiel à Estilização Real da Tela WEB (welcome-screen, eyebrow, copy h1, preview-window, welcome-btn).
/// Fonte da Verdade: web/style.css
class MobileWelcomeView extends StatelessWidget {
  final VoidCallback onLogin;
  final VoidCallback onRegister;
  final VoidCallback onExplorar;

  const MobileWelcomeView({
    super.key,
    required this.onLogin,
    required this.onRegister,
    required this.onExplorar,
  });

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      body: SafeArea(
        child: SingleChildScrollView(
          child: Container(
            decoration: const BoxDecoration(
              gradient: LinearGradient(
                colors: [Color(0xFFFFF9FB), Color(0xFFFFF1F6)],
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
            ),
            padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 24),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                const SizedBox(height: 12),

                // Brand Mark no Topo (.welcome-brand-mark da Web)
                Row(
                  children: [
                    Container(
                      width: 36,
                      height: 36,
                      decoration: BoxDecoration(
                        color: const Color(0xFFEA3F74),
                        borderRadius: BorderRadius.circular(11),
                        boxShadow: const [
                          BoxShadow(
                            color: Color.fromRGBO(234, 63, 116, 0.25),
                            blurRadius: 18,
                            offset: Offset(0, 8),
                          ),
                        ],
                      ),
                      child: const Icon(
                        Icons.hub_rounded,
                        color: Colors.white,
                        size: 20,
                      ),
                    ),
                    const SizedBox(width: 10),
                    RichText(
                      text: TextSpan(
                        style: GoogleFonts.manrope(
                          fontSize: 20,
                          fontWeight: FontWeight.w800,
                          color: const Color(0xFF202124),
                        ),
                        children: const [
                          TextSpan(text: 'Social'),
                          TextSpan(
                            text: 'Join',
                            style: TextStyle(color: Color(0xFFEA3F74)),
                          ),
                        ],
                      ),
                    ),
                    const Spacer(),
                    TextButton(
                      onPressed: onLogin,
                      child: Text(
                        'Entrar',
                        style: GoogleFonts.manrope(
                          color: const Color(0xFFEA3F74),
                          fontSize: 13,
                          fontWeight: FontWeight.w800,
                        ),
                      ),
                    ),
                  ],
                ),

                const SizedBox(height: 36),

                // Eyebrow Tag estilo Web (.welcome-eyebrow)
                Row(
                  mainAxisSize: MainAxisSize.min,
                  children: [
                    Container(
                      width: 28,
                      height: 2,
                      color: const Color(0xFFEA3F74),
                    ),
                    const SizedBox(width: 9),
                    Text(
                      'A SUA REDE SOCIAL DE EVENTOS',
                      style: GoogleFonts.manrope(
                        color: const Color(0xFFEA3F74),
                        fontSize: 12,
                        fontWeight: FontWeight.w800,
                        letterSpacing: 1.1,
                      ),
                    ),
                  ],
                ),

                const SizedBox(height: 16),

                // Copy Headline H1 estilo Web (.welcome-copy h1)
                RichText(
                  text: TextSpan(
                    style: GoogleFonts.manrope(
                      fontSize: 38,
                      fontWeight: FontWeight.w800,
                      letterSpacing: -1.5,
                      color: const Color(0xFF202124),
                      height: 1.05,
                    ),
                    children: const [
                      TextSpan(text: 'Conecte-se.\nParticipe. '),
                      TextSpan(
                        text: 'Viva.',
                        style: TextStyle(
                          color: Color(0xFFEA3F74),
                          fontStyle: FontStyle.italic,
                        ),
                      ),
                    ],
                  ),
                ),

                const SizedBox(height: 14),

                Text(
                  'Conecte-se com pessoas reais, descubra encontros marcantes e viva experiências inesquecíveis.',
                  style: GoogleFonts.manrope(
                    fontSize: 15,
                    color: const Color(0xFF6B7280),
                    height: 1.55,
                  ),
                ),

                const SizedBox(height: 28),

                // Preview Window Fiel à WEB (.preview-window da Web)
                Container(
                  padding: const EdgeInsets.all(20),
                  decoration: BoxDecoration(
                    color: Colors.white.withValues(alpha: 0.92),
                    borderRadius: BorderRadius.circular(18),
                    border: Border.all(color: const Color(0xFFEA3F74).withValues(alpha: 0.15)),
                    boxShadow: const [
                      BoxShadow(
                        color: Color.fromRGBO(120, 34, 66, 0.16),
                        blurRadius: 40,
                        offset: Offset(0, 18),
                      ),
                    ],
                  ),
                  child: Column(
                    children: [
                      Row(
                        children: [
                          const Icon(Icons.local_fire_department_rounded, color: Color(0xFFEA3F74), size: 18),
                          const SizedBox(width: 6),
                          Text(
                            'ACONTECENDO AGORA',
                            style: GoogleFonts.manrope(
                              fontSize: 11,
                              fontWeight: FontWeight.w800,
                              color: const Color(0xFFEA3F74),
                              letterSpacing: 0.5,
                            ),
                          ),
                          const Spacer(),
                          Container(
                            padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 3),
                            decoration: BoxDecoration(
                              color: const Color(0xFFECFDF5),
                              borderRadius: BorderRadius.circular(8),
                            ),
                            child: Text(
                              '+2.4k ativos',
                              style: GoogleFonts.manrope(
                                color: const Color(0xFF10B981),
                                fontSize: 10,
                                fontWeight: FontWeight.w800,
                              ),
                            ),
                          ),
                        ],
                      ),
                      const SizedBox(height: 12),
                      const Divider(color: Color(0xFFE5E7EB), height: 1),
                      const SizedBox(height: 12),

                      _buildPreviewEventRow(
                        time: '19:30',
                        title: 'Tech Meetup & Networking',
                        location: 'Paulista, São Paulo',
                        avatars: ['M', 'S', 'A'],
                      ),
                      const SizedBox(height: 8),

                      _buildPreviewEventRow(
                        time: '21:00',
                        title: 'Festival de Música & Arte',
                        location: 'Parque Ibirapuera',
                        avatars: ['G', 'L'],
                      ),
                    ],
                  ),
                ),

                const SizedBox(height: 32),

                // Welcome Actions (.welcome-actions e .welcome-btn)
                Column(
                  children: [
                    SizedBox(
                      width: double.infinity,
                      height: 48,
                      child: ElevatedButton(
                        onPressed: onRegister,
                        style: ElevatedButton.styleFrom(
                          backgroundColor: const Color(0xFFEA3F74),
                          foregroundColor: Colors.white,
                          elevation: 0,
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(10),
                          ),
                        ),
                        child: Text(
                          'Criar nova conta',
                          style: GoogleFonts.manrope(
                            fontSize: 14,
                            fontWeight: FontWeight.w700,
                          ),
                        ),
                      ),
                    ),
                    const SizedBox(height: 10),
                    SizedBox(
                      width: double.infinity,
                      height: 48,
                      child: OutlinedButton(
                        onPressed: onLogin,
                        style: OutlinedButton.styleFrom(
                          foregroundColor: const Color(0xFFEA3F74),
                          side: const BorderSide(color: Color(0xFFF9ACC6), width: 1),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(10),
                          ),
                        ),
                        child: Text(
                          'Entrar com e-mail',
                          style: GoogleFonts.manrope(
                            fontSize: 14,
                            fontWeight: FontWeight.w700,
                          ),
                        ),
                      ),
                    ),
                    const SizedBox(height: 14),
                    TextButton(
                      onPressed: onExplorar,
                      child: Row(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          Text(
                            'Explorar como visitante',
                            style: GoogleFonts.manrope(
                              color: const Color(0xFF6B7280),
                              fontSize: 13,
                              fontWeight: FontWeight.w700,
                            ),
                          ),
                          const SizedBox(width: 4),
                          const Icon(Icons.arrow_forward_rounded, size: 16, color: Color(0xFF6B7280)),
                        ],
                      ),
                    ),
                  ],
                ),
                const SizedBox(height: 16),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildPreviewEventRow({
    required String time,
    required String title,
    required String location,
    required List<String> avatars,
  }) {
    return Row(
      children: [
        Text(
          time,
          style: GoogleFonts.manrope(
            color: const Color(0xFFEA3F74),
            fontSize: 12,
            fontWeight: FontWeight.w800,
          ),
        ),
        const SizedBox(width: 12),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                title,
                style: GoogleFonts.manrope(
                  fontSize: 13,
                  fontWeight: FontWeight.w700,
                  color: const Color(0xFF202124),
                ),
                overflow: TextOverflow.ellipsis,
              ),
              Text(
                location,
                style: GoogleFonts.manrope(
                  fontSize: 11,
                  color: const Color(0xFF6B7280),
                ),
                overflow: TextOverflow.ellipsis,
              ),
            ],
          ),
        ),
        const SizedBox(width: 8),

        // Avatares (.preview-avatars da Web CSS)
        SizedBox(
          width: avatars.length * 16.0 + 8.0,
          height: 24,
          child: Stack(
            children: avatars.asMap().entries.map((entry) {
              final idx = entry.key;
              final letter = entry.value;
              return Positioned(
                left: idx * 14.0,
                child: CircleAvatar(
                  radius: 11,
                  backgroundColor: idx == 0
                      ? const Color(0xFFE47D9D)
                      : idx == 1
                          ? const Color(0xFFEA3F74)
                          : const Color(0xFF202124),
                  child: Text(
                    letter,
                    style: GoogleFonts.manrope(
                      color: Colors.white,
                      fontSize: 9,
                      fontWeight: FontWeight.w800,
                    ),
                  ),
                ),
              );
            }).toList(),
          ),
        ),
      ],
    );
  }
}
