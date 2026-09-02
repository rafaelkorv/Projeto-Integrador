import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import '../models/comunidade.dart';
import '../services/comunidade_service.dart';
import '../services/auth_service.dart';

/// Tela de Comunidades Mobile — Fiel à Estilização Real da Tela WEB (.comunidade-card, .comunidade-mark, top-stripe, .comunidade-stats, .btn-primary).
/// Fonte da Verdade: web/style.css
class CommunitiesPage extends StatefulWidget {
  const CommunitiesPage({super.key});

  @override
  State<CommunitiesPage> createState() => _CommunitiesPageState();
}

class _CommunitiesPageState extends State<CommunitiesPage> {
  final ComunidadeService _service = ComunidadeService();
  List<Comunidade> _comunidades = [];
  List<Comunidade> _comunidadesFiltradas = [];
  bool _carregando = true;
  String? _erro;
  String _filtro = 'TODAS';
  final TextEditingController _buscaCtrl = TextEditingController();

  @override
  void initState() {
    super.initState();
    _carregarComunidades();
  }

  @override
  void dispose() {
    _buscaCtrl.dispose();
    super.dispose();
  }

  Future<void> _carregarComunidades() async {
    setState(() {
      _carregando = true;
      _erro = null;
    });

    try {
      final lista = await _service.listarComunidades();
      if (mounted) {
        setState(() {
          _comunidades = lista;
          _carregando = false;
          _aplicarFiltros();
        });
      }
    } catch (e) {
      if (mounted) {
        setState(() {
          _erro = 'Não foi possível carregar as comunidades.';
          _carregando = false;
        });
      }
    }
  }

  void _aplicarFiltros() {
    final query = _buscaCtrl.text.trim().toLowerCase();
    setState(() {
      _comunidadesFiltradas = _comunidades.where((c) {
        final bool isMembro = c.isMembro(AuthService.idUsuario);
        final bool matchFiltro = _filtro == 'TODAS' || (_filtro == 'MINHAS' && isMembro);

        final bool matchTexto = query.isEmpty ||
            c.nome.toLowerCase().contains(query) ||
            (c.descricao != null && c.descricao!.toLowerCase().contains(query));

        return matchFiltro && matchTexto;
      }).toList();
    });
  }

  Future<void> _participar(Comunidade c) async {
    if (!AuthService.logado || AuthService.idUsuario == null) {
      _snack('Faça login para participar de comunidades.',
          cor: const Color(0xFFEA3F74));
      return;
    }

    final ok = await _service.participarComunidade(
        c.id, AuthService.idUsuario!);

    _snack(
      ok
          ? 'Você entrou em "${c.nome}"!'
          : 'Não foi possível entrar na comunidade.',
      cor: ok ? const Color(0xFF10B981) : const Color(0xFFC93659),
    );

    if (ok) _carregarComunidades();
  }

  Future<void> _sair(Comunidade c) async {
    if (!AuthService.logado || AuthService.idUsuario == null) return;

    final ok = await _service.sairComunidade(
        c.id, AuthService.idUsuario!, AuthService.idUsuario!);

    _snack(
      ok ? 'Você saiu de "${c.nome}".' : 'Não foi possível sair.',
      cor: ok ? const Color(0xFF6B7280) : const Color(0xFFC93659),
    );

    if (ok) _carregarComunidades();
  }

  void _snack(String msg, {Color cor = const Color(0xFF10B981)}) {
    if (!mounted) return;
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(msg, style: GoogleFonts.manrope(fontWeight: FontWeight.w600)),
        backgroundColor: cor,
        duration: const Duration(seconds: 2),
      ),
    );
  }

  void _abrirCriarComunidade() {
    if (!AuthService.logado || AuthService.idUsuario == null) {
      _snack('Faça login para criar comunidades.',
          cor: const Color(0xFFEA3F74));
      return;
    }

    final nomeCtrl = TextEditingController();
    final descCtrl = TextEditingController();
    bool salvando = false;

    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (ctx) => StatefulBuilder(
        builder: (ctx, setModalState) => Container(
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
          child: SingleChildScrollView(
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
                  'Nova Comunidade',
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
                  decoration: _modalInput('Nome da Comunidade', Icons.groups_rounded),
                ),
                const SizedBox(height: 12),
                TextField(
                  controller: descCtrl,
                  maxLines: 3,
                  style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF202124)),
                  decoration: _modalInput('Descrição e objetivos', Icons.info_outline_rounded),
                ),
                const SizedBox(height: 24),

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
                                  content: Text('Informe um nome para a comunidade.', style: GoogleFonts.manrope()),
                                  backgroundColor: const Color(0xFFC93659),
                                ),
                              );
                              return;
                            }

                            setModalState(() => salvando = true);

                            final nav = Navigator.of(ctx);
                            final nova = await _service.criarComunidade(
                              nome: nomeCtrl.text.trim(),
                              descricao: descCtrl.text.trim(),
                              criadorId: AuthService.idUsuario!,
                            );

                            if (!mounted) return;
                            nav.pop();

                            if (nova != null) {
                              _snack('Comunidade "${nova.nome}" criada com sucesso!');
                              _carregarComunidades();
                            } else {
                              _snack('Erro ao criar comunidade no servidor.', cor: const Color(0xFFC93659));
                            }
                          },
                    child: salvando
                        ? const SizedBox(
                            height: 20,
                            width: 20,
                            child: CircularProgressIndicator(color: Colors.white, strokeWidth: 2),
                          )
                        : Text('Criar Comunidade', style: GoogleFonts.manrope(fontSize: 14, fontWeight: FontWeight.w700)),
                  ),
                ),
                const SizedBox(height: 8),
              ],
            ),
          ),
        ),
      ),
    );
  }

  InputDecoration _modalInput(String label, IconData icon) {
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

  IconData _iconeParaComunidade(String nome) {
    final n = nome.toLowerCase();
    if (n.contains('flutter') || n.contains('dev') || n.contains('code') || n.contains('tech') || n.contains('software')) {
      return Icons.code_rounded;
    }
    if (n.contains('design') || n.contains('ux') || n.contains('ui') || n.contains('arte')) {
      return Icons.palette_rounded;
    }
    if (n.contains('game') || n.contains('esport') || n.contains('jog')) {
      return Icons.sports_esports_rounded;
    }
    if (n.contains('estudo') || n.contains('academ') || n.contains('tcc') || n.contains('faculdade')) {
      return Icons.school_rounded;
    }
    if (n.contains('musica') || n.contains('música') || n.contains('som')) {
      return Icons.music_note_rounded;
    }
    if (n.contains('sport') || n.contains('futebol') || n.contains('basquet') || n.contains('corrida')) {
      return Icons.sports_rounded;
    }
    return Icons.hub_rounded;
  }

  Color _corParaIndex(int index) {
    const cores = [
      Color(0xFFEA3F74),
      Color(0xFF2563EB),
      Color(0xFF8B5CF6),
      Color(0xFF10B981),
      Color(0xFF06B6D4),
      Color(0xFFF59E0B),
      Color(0xFFEF4444),
    ];
    return cores[index % cores.length];
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF5F7FF),
      body: RefreshIndicator(
        onRefresh: _carregarComunidades,
        color: const Color(0xFFEA3F74),
        child: CustomScrollView(
          slivers: [
            // Page Header Fiel à Web (.page-header da Web)
            SliverToBoxAdapter(
              child: Padding(
                padding: const EdgeInsets.fromLTRB(16, 20, 16, 12),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          'ENCONTRE SUA TURMA',
                          style: GoogleFonts.manrope(
                            color: const Color(0xFFEA3F74),
                            fontSize: 11,
                            fontWeight: FontWeight.w800,
                            letterSpacing: 0.9,
                          ),
                        ),
                        const SizedBox(height: 2),
                        Text(
                          'Comunidades',
                          style: GoogleFonts.manrope(
                            fontSize: 24,
                            fontWeight: FontWeight.w700,
                            color: const Color(0xFF202124),
                          ),
                        ),
                      ],
                    ),
                    ElevatedButton.icon(
                      onPressed: _abrirCriarComunidade,
                      icon: const Icon(Icons.add_rounded, size: 16),
                      label: Text('Nova Comunidade', style: GoogleFonts.manrope(fontWeight: FontWeight.w700, fontSize: 13)),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: const Color(0xFFEA3F74),
                        foregroundColor: Colors.white,
                        elevation: 0,
                        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 10),
                      ),
                    ),
                  ],
                ),
              ),
            ),

            // Barra de Busca e Filtros (.search-bar da Web)
            SliverToBoxAdapter(
              child: Padding(
                padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
                child: TextField(
                  controller: _buscaCtrl,
                  onChanged: (_) => _aplicarFiltros(),
                  style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF202124)),
                  decoration: InputDecoration(
                    hintText: 'Buscar por nome ou interesse...',
                    hintStyle: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF6B7280)),
                    prefixIcon: const Icon(Icons.search_rounded, size: 18, color: Color(0xFF6B7280)),
                    suffixIcon: _buscaCtrl.text.isNotEmpty
                        ? IconButton(
                            icon: const Icon(Icons.clear, size: 18),
                            onPressed: () {
                              _buscaCtrl.clear();
                              _aplicarFiltros();
                            },
                          )
                        : null,
                  ),
                ),
              ),
            ),

            // Filter Tabs (.feed-tabs da Web)
            SliverToBoxAdapter(
              child: Padding(
                padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                child: Row(
                  children: [
                    _buildFilterTab('TODAS', 'Todas as Comunidades'),
                    if (AuthService.logado)
                      _buildFilterTab('MINHAS', 'Minhas Comunidades'),
                  ],
                ),
              ),
            ),

            // Listagem de Cards de Comunidade (.comunidade-card Fiel à Web)
            if (_carregando)
              const SliverFillRemaining(
                child: Center(child: CircularProgressIndicator(color: Color(0xFFEA3F74))),
              )
            else if (_erro != null)
              SliverFillRemaining(
                child: Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      const Icon(Icons.wifi_off_rounded, size: 48, color: Color(0xFF6B7280)),
                      const SizedBox(height: 12),
                      Text(_erro!, style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 14)),
                      const SizedBox(height: 16),
                      ElevatedButton(
                        onPressed: _carregarComunidades,
                        child: const Text('Tentar novamente'),
                      ),
                    ],
                  ),
                ),
              )
            else if (_comunidadesFiltradas.isEmpty)
              SliverFillRemaining(
                child: Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      const Icon(Icons.groups_outlined, size: 48, color: Color(0xFF6B7280)),
                      const SizedBox(height: 12),
                      Text('Nenhuma comunidade encontrada', style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 14, fontWeight: FontWeight.w700)),
                      const SizedBox(height: 16),
                      ElevatedButton(
                        onPressed: _abrirCriarComunidade,
                        child: const Text('Criar Primeira Comunidade'),
                      ),
                    ],
                  ),
                ),
              )
            else
              SliverPadding(
                padding: const EdgeInsets.fromLTRB(16, 8, 16, 90),
                sliver: SliverList(
                  delegate: SliverChildBuilderDelegate(
                    (context, index) {
                      final c = _comunidadesFiltradas[index];
                      final bool isMembro = c.isMembro(AuthService.idUsuario);
                      final cor = _corParaIndex(index);
                      final icone = _iconeParaComunidade(c.nome);

                      return _WebStyleComunidadeCard(
                        comunidade: c,
                        isMembro: isMembro,
                        cor: cor,
                        icone: icone,
                        onParticipar: () => _participar(c),
                        onSair: () => _sair(c),
                      );
                    },
                    childCount: _comunidadesFiltradas.length,
                  ),
                ),
              ),
          ],
        ),
      ),
    );
  }

  Widget _buildFilterTab(String key, String label) {
    final isSelected = _filtro == key;
    return GestureDetector(
      onTap: () {
        setState(() {
          _filtro = key;
          _aplicarFiltros();
        });
      },
      child: Container(
        margin: const EdgeInsets.only(right: 16),
        padding: const EdgeInsets.symmetric(vertical: 8),
        decoration: BoxDecoration(
          border: Border(
            bottom: BorderSide(
              color: isSelected ? const Color(0xFFEA3F74) : Colors.transparent,
              width: 3.0,
            ),
          ),
        ),
        child: Text(
          label,
          style: GoogleFonts.manrope(
            color: isSelected ? const Color(0xFFEA3F74) : const Color(0xFF6B7280),
            fontSize: 13,
            fontWeight: FontWeight.w800,
          ),
        ),
      ),
    );
  }
}

/// Card de Comunidade Fiel ao `.comunidade-card`, `.comunidade-mark`, top 4px stripe (`::before`), `.comunidade-stats`, `.btn-compact` da Web CSS
class _WebStyleComunidadeCard extends StatelessWidget {
  final Comunidade comunidade;
  final bool isMembro;
  final Color cor;
  final IconData icone;
  final VoidCallback onParticipar;
  final VoidCallback onSair;

  const _WebStyleComunidadeCard({
    required this.comunidade,
    required this.isMembro,
    required this.cor,
    required this.icone,
    required this.onParticipar,
    required this.onSair,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: const EdgeInsets.only(bottom: 16),
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
      child: ClipRRect(
        borderRadius: BorderRadius.circular(14),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Top Stripe (Faixa de Cor Superior de 4px Fiel ao CSS `.comunidade-card::before`)
            Container(
              height: 4,
              width: double.infinity,
              color: cor,
            ),

            Padding(
              padding: const EdgeInsets.all(20),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Top info
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      // .comunidade-mark (Container de ícone 44x44, radius 12px)
                      Container(
                        width: 44,
                        height: 44,
                        decoration: BoxDecoration(
                          color: cor,
                          borderRadius: BorderRadius.circular(12),
                        ),
                        child: Icon(icone, color: Colors.white, size: 22),
                      ),
                      Text(
                        'COMUNIDADE',
                        style: GoogleFonts.manrope(
                          color: cor,
                          fontSize: 10,
                          fontWeight: FontWeight.w800,
                          letterSpacing: 0.8,
                        ),
                      ),
                    ],
                  ),

                  const SizedBox(height: 12),

                  // Title h2
                  Text(
                    comunidade.nome,
                    style: GoogleFonts.manrope(
                      fontSize: 18,
                      fontWeight: FontWeight.w700,
                      color: const Color(0xFF202124),
                    ),
                  ),

                  const SizedBox(height: 6),

                  // Description p
                  Text(
                    comunidade.descricao != null && comunidade.descricao!.isNotEmpty
                        ? comunidade.descricao!
                        : 'Comunidade no SocialJoin',
                    style: GoogleFonts.manrope(
                      color: const Color(0xFF6B7280),
                      fontSize: 12,
                      height: 1.55,
                    ),
                    maxLines: 2,
                    overflow: TextOverflow.ellipsis,
                  ),

                  const SizedBox(height: 14),

                  // .comunidade-stats (11px text, border-top 1px solid #e5e7eb)
                  Container(
                    padding: const EdgeInsets.symmetric(vertical: 11),
                    decoration: const BoxDecoration(
                      border: Border(
                        top: BorderSide(color: Color(0xFFE5E7EB), width: 1.0),
                      ),
                    ),
                    child: Row(
                      children: [
                        Row(
                          children: [
                            const Icon(Icons.people_alt_outlined, size: 14, color: Color(0xFF6B7280)),
                            const SizedBox(width: 4),
                            Text(
                              '${comunidade.totalMembros} membros',
                              style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 11, fontWeight: FontWeight.w600),
                            ),
                          ],
                        ),
                        const SizedBox(width: 16),
                        Row(
                          children: [
                            const Icon(Icons.event_outlined, size: 14, color: Color(0xFF6B7280)),
                            const SizedBox(width: 4),
                            Text(
                              '0 eventos',
                              style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 11, fontWeight: FontWeight.w600),
                            ),
                          ],
                        ),
                      ],
                    ),
                  ),

                  // Footer com Criador & Botão de Ação (.comunidade-card-footer / .btn-compact da Web)
                  Row(
                    children: [
                      Text(
                        'Por ${comunidade.criador?.nome ?? 'Administrador'}',
                        style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 11),
                      ),
                      const Spacer(),
                      if (isMembro)
                        OutlinedButton(
                          onPressed: onSair,
                          style: OutlinedButton.styleFrom(
                            foregroundColor: const Color(0xFF6B7280),
                            side: const BorderSide(color: Color(0xFFE5E7EB)),
                            padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 8),
                            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
                          ),
                          child: Text('Sair', style: GoogleFonts.manrope(fontSize: 12, fontWeight: FontWeight.w700)),
                        )
                      else
                        ElevatedButton(
                          onPressed: onParticipar,
                          style: ElevatedButton.styleFrom(
                            backgroundColor: const Color(0xFFEA3F74),
                            foregroundColor: Colors.white,
                            elevation: 0,
                            padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
                          ),
                          child: Text('Acessar', style: GoogleFonts.manrope(fontSize: 12, fontWeight: FontWeight.w700)),
                        ),
                    ],
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
