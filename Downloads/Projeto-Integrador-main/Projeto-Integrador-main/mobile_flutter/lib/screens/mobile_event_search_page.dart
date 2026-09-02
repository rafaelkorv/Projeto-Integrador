import 'package:flutter/material.dart';
import '../models/evento.dart';
import '../services/evento_service.dart';
import '../services/auth_service.dart';
import 'map_events_page.dart';

/// Tela dedicada para Pesquisa e Exploração de Eventos no Mobile.
/// Oferece uma interface limpa, intuitiva e nativa para busca instantânea e filtros.
class MobileEventSearchPage extends StatefulWidget {
  final Function(int eventoId)? onVerNoMapa;

  const MobileEventSearchPage({
    super.key,
    this.onVerNoMapa,
  });

  @override
  State<MobileEventSearchPage> createState() => _MobileEventSearchPageState();
}

class _MobileEventSearchPageState extends State<MobileEventSearchPage> {
  final EventoService _service = EventoService();
  final TextEditingController _searchCtrl = TextEditingController();

  List<Evento> _todosEventos = [];
  List<Evento> _eventosFiltrados = [];
  Map<int, int> _totalParticipantes = {};
  Set<int> _eventosInscritos = {};

  bool _carregando = true;
  String? _erro;
  String _filtroAtual = 'TODOS'; // TODOS, AGENDADO, ACONTECENDO_AGORA, INSCRITO

  @override
  void initState() {
    super.initState();
    _carregarDados();
    _searchCtrl.addListener(_aplicarFiltros);
  }

  @override
  void dispose() {
    _searchCtrl.dispose();
    super.dispose();
  }

  Future<void> _carregarDados() async {
    setState(() {
      _carregando = true;
      _erro = null;
    });

    try {
      final lista = await _service.listarEventos();

      if (mounted) {
        setState(() {
          _todosEventos = lista;
          _carregando = false;
          _aplicarFiltros();
        });
      }

      // Carrega dados de participantes e inscrições em segundo plano sem bloquear a renderização inicial
      _carregarMetadadosEventos(lista);
    } catch (e) {
      if (mounted) {
        setState(() {
          _erro = 'Não foi possível carregar os eventos.';
          _carregando = false;
        });
      }
    }
  }

  Future<void> _carregarMetadadosEventos(List<Evento> lista) async {
    final Map<int, int> mapaContagem = Map.from(_totalParticipantes);
    final Set<int> inscritos = Set.from(_eventosInscritos);

    for (var ev in lista) {
      try {
        final total = await _service.contarParticipantes(ev.id);
        mapaContagem[ev.id] = total;

        if (AuthService.logado && AuthService.idUsuario != null) {
          final ids = await _service.listarIdsParticipantes(ev.id);
          if (ids.contains(AuthService.idUsuario)) {
            inscritos.add(ev.id);
          }
        }
      } catch (_) {
        // Ignora falhas isoladas de contagem
      }
    }

    if (mounted) {
      setState(() {
        _totalParticipantes = mapaContagem;
        _eventosInscritos = inscritos;
        _aplicarFiltros();
      });
    }
  }

  void _aplicarFiltros() {
    final query = _searchCtrl.text.trim().toLowerCase();

    setState(() {
      _eventosFiltrados = _todosEventos.where((ev) {
        final sit = ev.situacaoCalculada;
        final matchStatus = _filtroAtual == 'TODOS' ||
            (_filtroAtual == 'AGENDADO' && (sit == 'AGENDADO' || sit == 'ATIVO')) ||
            (_filtroAtual == 'ACONTECENDO_AGORA' && sit == 'ACONTECENDO_AGORA') ||
            (_filtroAtual == 'INSCRITO' && _eventosInscritos.contains(ev.id));

        final matchTexto = query.isEmpty ||
            ev.titulo.toLowerCase().contains(query) ||
            ev.localEvento.toLowerCase().contains(query) ||
            (ev.descricao != null && ev.descricao!.toLowerCase().contains(query));

        return matchStatus && matchTexto;
      }).toList();
    });
  }

  Future<void> _toggleParticipacao(Evento evento) async {
    if (!AuthService.logado || AuthService.idUsuario == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Faça login para confirmar presença nos eventos.'),
          backgroundColor: Color(0xFFEA3F74),
        ),
      );
      return;
    }

    final userId = AuthService.idUsuario!;
    final jaInscrito = _eventosInscritos.contains(evento.id);

    if (jaInscrito) {
      final ok = await _service.sairEvento(evento.id, userId);
      if (ok && mounted) {
        setState(() {
          _eventosInscritos.remove(evento.id);
          final atual = _totalParticipantes[evento.id] ?? 1;
          _totalParticipantes[evento.id] = (atual > 0) ? atual - 1 : 0;
        });
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(
            content: Text('Sua presença foi cancelada.'),
            backgroundColor: Color(0xFF64748B),
          ),
        );
      }
    } else {
      final ok = await _service.participarEvento(evento.id, userId);
      if (ok && mounted) {
        setState(() {
          _eventosInscritos.add(evento.id);
          final atual = _totalParticipantes[evento.id] ?? 0;
          _totalParticipantes[evento.id] = atual + 1;
        });
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: Text('Presença confirmada em "${evento.titulo}"! 🎉'),
            backgroundColor: const Color(0xFF10B981),
          ),
        );
      }
    }
  }

  void _abrirDetalhesEventoModal(Evento evento) {
    final bool jaInscrito = _eventosInscritos.contains(evento.id);
    final int total = _totalParticipantes[evento.id] ?? 0;
    final sit = evento.situacaoCalculada;
    final bool emAndamento = sit == 'ACONTECENDO_AGORA';

    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (ctx) => Container(
        padding: const EdgeInsets.all(20),
        decoration: const BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.vertical(top: Radius.circular(28)),
        ),
        child: SingleChildScrollView(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            mainAxisSize: MainAxisSize.min,
            children: [
              Center(
                child: Container(
                  width: 40,
                  height: 4,
                  decoration: BoxDecoration(
                    color: Colors.grey.shade300,
                    borderRadius: BorderRadius.circular(2),
                  ),
                ),
              ),
              const SizedBox(height: 16),

              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Container(
                    padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                    decoration: BoxDecoration(
                      color: const Color(0xFFFDF0F4),
                      borderRadius: BorderRadius.circular(12),
                    ),
                    child: Text(
                      evento.categoria ?? '🎉 EVENTO',
                      style: const TextStyle(
                        color: Color(0xFFEA3F74),
                        fontSize: 11,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                  Container(
                    padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                    decoration: BoxDecoration(
                      color: emAndamento ? const Color(0xFFFEF2F2) : const Color(0xFFF1F5F9),
                      borderRadius: BorderRadius.circular(12),
                    ),
                    child: Text(
                      emAndamento ? '🔥 ACONTECENDO AGORA' : 'STATUS: ${evento.status}',
                      style: TextStyle(
                        color: emAndamento ? const Color(0xFFDC2626) : const Color(0xFF64748B),
                        fontSize: 11,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 12),

              Text(
                evento.titulo,
                style: const TextStyle(
                  fontSize: 22,
                  fontWeight: FontWeight.w900,
                  color: Color(0xFF0F172A),
                  letterSpacing: -0.5,
                ),
              ),
              const SizedBox(height: 16),

              _buildDetailRow(Icons.calendar_month_outlined, 'Data', evento.dataFormatada),
              const SizedBox(height: 8),
              _buildDetailRow(Icons.schedule_outlined, 'Horário', '${evento.horarioFormatado} às ${evento.horarioFim.length >= 5 ? evento.horarioFim.substring(0, 5) : evento.horarioFim}'),
              const SizedBox(height: 8),
              _buildDetailRow(Icons.location_on_outlined, 'Local', evento.localEvento),
              const SizedBox(height: 8),
              _buildDetailRow(Icons.people_outline_rounded, 'Participantes', '$total confirmados ${evento.limiteParticipantes != null ? "(máx. ${evento.limiteParticipantes})" : ""}'),
              const SizedBox(height: 8),
              _buildDetailRow(Icons.verified_outlined, 'Entrada', evento.exigeCheckin ? 'Check-in obrigatório pelo app' : 'Entrada livre'),

              if (evento.descricao != null && evento.descricao!.isNotEmpty) ...[
                const SizedBox(height: 16),
                const Text('Sobre o Evento', style: TextStyle(fontSize: 15, fontWeight: FontWeight.bold, color: Color(0xFF0F172A))),
                const SizedBox(height: 6),
                Text(
                  evento.descricao!,
                  style: const TextStyle(fontSize: 14, color: Color(0xFF475569), height: 1.45),
                ),
              ],

              const SizedBox(height: 24),

              Row(
                children: [
                  Expanded(
                    child: OutlinedButton.icon(
                      onPressed: () {
                        Navigator.pop(ctx);
                        Navigator.push(
                          context,
                          MaterialPageRoute(builder: (_) => const MapEventsPage()),
                        );
                      },
                      icon: const Icon(Icons.map_outlined, size: 18),
                      label: const Text('Ver no Mapa'),
                      style: OutlinedButton.styleFrom(
                        foregroundColor: const Color(0xFF0F172A),
                        padding: const EdgeInsets.symmetric(vertical: 14),
                        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
                      ),
                    ),
                  ),
                  const SizedBox(width: 12),
                  Expanded(
                    child: ElevatedButton.icon(
                      onPressed: () {
                        Navigator.pop(ctx);
                        _toggleParticipacao(evento);
                      },
                      icon: Icon(jaInscrito ? Icons.check_circle_rounded : Icons.add_circle_outline_rounded, size: 18),
                      label: Text(jaInscrito ? 'Confirmado' : 'Participar'),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: jaInscrito ? const Color(0xFF10B981) : const Color(0xFFEA3F74),
                        foregroundColor: Colors.white,
                        elevation: 0,
                        padding: const EdgeInsets.symmetric(vertical: 14),
                        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
                      ),
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildDetailRow(IconData icon, String label, String value) {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Icon(icon, size: 18, color: const Color(0xFFEA3F74)),
        const SizedBox(width: 10),
        Expanded(
          child: RichText(
            text: TextSpan(
              style: const TextStyle(fontSize: 14, color: Color(0xFF334155)),
              children: [
                TextSpan(text: '$label: ', style: const TextStyle(fontWeight: FontWeight.bold, color: Color(0xFF0F172A))),
                TextSpan(text: value),
              ],
            ),
          ),
        ),
      ],
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFFAFAFC),
      body: SafeArea(
        child: Column(
          children: [
            // Header e Campo de Busca
            Padding(
              padding: const EdgeInsets.fromLTRB(16, 12, 16, 8),
              child: Column(
                children: [
                  Container(
                    height: 50,
                    decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.circular(25),
                      border: Border.all(color: const Color(0xFFE2E8F0), width: 1),
                      boxShadow: [
                        BoxShadow(
                          color: Colors.black.withValues(alpha: 0.03),
                          blurRadius: 10,
                          offset: const Offset(0, 3),
                        ),
                      ],
                    ),
                    child: TextField(
                      controller: _searchCtrl,
                      style: const TextStyle(fontSize: 14, color: Color(0xFF0F172A)),
                      decoration: InputDecoration(
                        hintText: 'Pesquisar evento, cidade ou interesse...',
                        hintStyle: const TextStyle(color: Color(0xFF94A3B8), fontSize: 14),
                        prefixIcon: const Icon(Icons.search_rounded, color: Color(0xFFEA3F74), size: 22),
                        suffixIcon: _searchCtrl.text.isNotEmpty
                            ? IconButton(
                                icon: const Icon(Icons.close_rounded, color: Color(0xFF94A3B8), size: 18),
                                onPressed: () {
                                  _searchCtrl.clear();
                                  _aplicarFiltros();
                                },
                              )
                            : null,
                        border: InputBorder.none,
                        contentPadding: const EdgeInsets.symmetric(vertical: 14),
                      ),
                    ),
                  ),
                  const SizedBox(height: 12),

                  // Chips de Filtro Horizontal + Botão Mapa
                  SingleChildScrollView(
                    scrollDirection: Axis.horizontal,
                    physics: const BouncingScrollPhysics(),
                    child: Row(
                      children: [
                        _buildFilterChip('TODOS', 'Todos'),
                        _buildFilterChip('AGENDADO', 'Em Breve'),
                        _buildFilterChip('ACONTECENDO_AGORA', '🔥 Agora'),
                        if (AuthService.logado) _buildFilterChip('INSCRITO', 'Meus Eventos'),

                        const SizedBox(width: 6),

                        // Botão para Mapa de Eventos
                        InkWell(
                          onTap: () {
                            Navigator.push(
                              context,
                              MaterialPageRoute(
                                builder: (_) => const MapEventsPage(),
                              ),
                            );
                          },
                          borderRadius: BorderRadius.circular(20),
                          child: Container(
                            padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
                            decoration: BoxDecoration(
                              color: const Color(0xFFFDF0F4),
                              borderRadius: BorderRadius.circular(20),
                              border: Border.all(color: const Color(0xFFF9ACC6).withValues(alpha: 0.6)),
                            ),
                            child: const Row(
                              children: [
                                Icon(Icons.map_outlined, color: Color(0xFFEA3F74), size: 16),
                                SizedBox(width: 6),
                                Text(
                                  'Ver no Mapa',
                                  style: TextStyle(
                                    color: Color(0xFFEA3F74),
                                    fontSize: 12,
                                    fontWeight: FontWeight.w700,
                                  ),
                                ),
                              ],
                            ),
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ),

            const Divider(color: Color(0xFFF1F5F9), height: 1),

            // Lista de Resultados
            Expanded(
              child: _carregando
                  ? const Center(child: CircularProgressIndicator(color: Color(0xFFEA3F74)))
                  : _erro != null
                      ? Center(
                          child: Column(
                            mainAxisAlignment: MainAxisAlignment.center,
                            children: [
                              const Icon(Icons.error_outline, color: Color(0xFFEF4444), size: 40),
                              const SizedBox(height: 12),
                              Text(_erro!, style: const TextStyle(color: Color(0xFF64748B))),
                              const SizedBox(height: 12),
                              ElevatedButton(
                                onPressed: _carregarDados,
                                style: ElevatedButton.styleFrom(
                                  backgroundColor: const Color(0xFFEA3F74),
                                  foregroundColor: Colors.white,
                                ),
                                child: const Text('Tentar novamente'),
                              ),
                            ],
                          ),
                        )
                      : _eventosFiltrados.isEmpty
                          ? Center(
                              child: Column(
                                mainAxisAlignment: MainAxisAlignment.center,
                                children: [
                                  Container(
                                    padding: const EdgeInsets.all(20),
                                    decoration: const BoxDecoration(
                                      color: Color(0xFFF1F5F9),
                                      shape: BoxShape.circle,
                                    ),
                                    child: const Icon(Icons.search_off_rounded, color: Color(0xFF94A3B8), size: 36),
                                  ),
                                  const SizedBox(height: 16),
                                  const Text(
                                    'Nenhum evento encontrado',
                                    style: TextStyle(
                                      fontSize: 16,
                                      fontWeight: FontWeight.bold,
                                      color: Color(0xFF0F172A),
                                    ),
                                  ),
                                  const SizedBox(height: 6),
                                  const Text(
                                    'Tente buscar por outro termo ou mude os filtros.',
                                    style: TextStyle(fontSize: 13, color: Color(0xFF64748B)),
                                  ),
                                ],
                              ),
                            )
                          : RefreshIndicator(
                              onRefresh: _carregarDados,
                              color: const Color(0xFFEA3F74),
                              child: ListView.builder(
                                padding: const EdgeInsets.fromLTRB(16, 12, 16, 80),
                                itemCount: _eventosFiltrados.length,
                                itemBuilder: (context, index) {
                                  final evento = _eventosFiltrados[index];
                                  final total = _totalParticipantes[evento.id] ?? 0;
                                  final jaInscrito = _eventosInscritos.contains(evento.id);

                                  return _buildMobileEventCard(evento, total, jaInscrito);
                                },
                              ),
                            ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildFilterChip(String key, String label) {
    final bool selecionado = _filtroAtual == key;

    return Padding(
      padding: const EdgeInsets.only(right: 8),
      child: ChoiceChip(
        label: Text(label),
        selected: selecionado,
        onSelected: (_) {
          setState(() {
            _filtroAtual = key;
            _aplicarFiltros();
          });
        },
        selectedColor: const Color(0xFFEA3F74),
        backgroundColor: Colors.white,
        side: BorderSide(
          color: selecionado ? const Color(0xFFEA3F74) : const Color(0xFFE2E8F0),
        ),
        labelStyle: TextStyle(
          color: selecionado ? Colors.white : const Color(0xFF475569),
          fontSize: 12,
          fontWeight: selecionado ? FontWeight.w700 : FontWeight.w500,
        ),
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
        showCheckmark: false,
        padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 6),
      ),
    );
  }

  Widget _buildMobileEventCard(Evento evento, int total, bool jaInscrito) {
    final sit = evento.situacaoCalculada;
    final bool emAndamento = sit == 'ACONTECENDO_AGORA';

    return Container(
      margin: const EdgeInsets.only(bottom: 14),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(20),
        border: Border.all(
          color: jaInscrito ? const Color(0xFFF9ACC6) : const Color(0xFFF1F5F9),
          width: jaInscrito ? 1.5 : 1.0,
        ),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withValues(alpha: 0.03),
            blurRadius: 12,
            offset: const Offset(0, 4),
          ),
        ],
      ),
      child: InkWell(
        borderRadius: BorderRadius.circular(20),
        onTap: () => _abrirDetalhesEventoModal(evento),
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Container(
                    width: 52,
                    height: 56,
                    decoration: BoxDecoration(
                      color: emAndamento ? const Color(0xFFFEF2F2) : const Color(0xFFFDF0F4),
                      borderRadius: BorderRadius.circular(14),
                      border: Border.all(
                        color: emAndamento ? const Color(0xFFFCA5A5) : const Color(0xFFF9ACC6),
                      ),
                    ),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(
                          emAndamento ? Icons.local_fire_department_rounded : Icons.event_rounded,
                          color: emAndamento ? const Color(0xFFEF4444) : const Color(0xFFEA3F74),
                          size: 24,
                        ),
                        const SizedBox(height: 2),
                        Text(
                          emAndamento ? 'AGORA' : 'EVENTO',
                          style: TextStyle(
                            fontSize: 9,
                            fontWeight: FontWeight.w900,
                            color: emAndamento ? const Color(0xFFDC2626) : const Color(0xFFEA3F74),
                          ),
                        ),
                      ],
                    ),
                  ),
                  const SizedBox(width: 12),

                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          evento.titulo,
                          style: const TextStyle(
                            fontSize: 16,
                            fontWeight: FontWeight.w800,
                            color: Color(0xFF0F172A),
                            letterSpacing: -0.4,
                          ),
                          maxLines: 2,
                          overflow: TextOverflow.ellipsis,
                        ),
                        const SizedBox(height: 4),
                        Row(
                          children: [
                            const Icon(Icons.location_on_outlined, color: Color(0xFF64748B), size: 14),
                            const SizedBox(width: 4),
                            Expanded(
                              child: Text(
                                evento.localEvento,
                                style: const TextStyle(
                                  fontSize: 13,
                                  color: Color(0xFF64748B),
                                  fontWeight: FontWeight.w500,
                                ),
                                maxLines: 1,
                                overflow: TextOverflow.ellipsis,
                              ),
                            ),
                          ],
                        ),
                      ],
                    ),
                  ),
                ],
              ),

              if (evento.descricao != null && evento.descricao!.isNotEmpty) ...[
                const SizedBox(height: 10),
                Text(
                  evento.descricao!,
                  style: const TextStyle(
                    fontSize: 13,
                    color: Color(0xFF475569),
                    height: 1.35,
                  ),
                  maxLines: 2,
                  overflow: TextOverflow.ellipsis,
                ),
              ],

              const SizedBox(height: 14),
              const Divider(color: Color(0xFFF1F5F9), height: 1),
              const SizedBox(height: 10),

              Row(
                children: [
                  Row(
                    children: [
                      SizedBox(
                        width: 44,
                        height: 22,
                        child: Stack(
                          children: [
                            Positioned(
                              left: 0,
                              child: CircleAvatar(
                                radius: 10,
                                backgroundColor: const Color(0xFFF9ACC6),
                                child: const Text('E', style: TextStyle(fontSize: 8, color: Colors.white, fontWeight: FontWeight.bold)),
                              ),
                            ),
                            Positioned(
                              left: 11,
                              child: CircleAvatar(
                                radius: 10,
                                backgroundColor: const Color(0xFFEA3F74),
                                child: const Text('V', style: TextStyle(fontSize: 8, color: Colors.white, fontWeight: FontWeight.bold)),
                              ),
                            ),
                            Positioned(
                              left: 22,
                              child: CircleAvatar(
                                radius: 10,
                                backgroundColor: const Color(0xFF0F172A),
                                child: const Text('P', style: TextStyle(fontSize: 8, color: Colors.white, fontWeight: FontWeight.bold)),
                              ),
                            ),
                          ],
                        ),
                      ),
                      const SizedBox(width: 6),
                      Text(
                        '$total confirmados',
                        style: const TextStyle(
                          fontSize: 12,
                          fontWeight: FontWeight.w700,
                          color: Color(0xFF0F172A),
                        ),
                      ),
                    ],
                  ),
                  const Spacer(),

                  SizedBox(
                    height: 36,
                    child: ElevatedButton(
                      onPressed: () => _toggleParticipacao(evento),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: jaInscrito ? const Color(0xFFECFDF5) : const Color(0xFFEA3F74),
                        foregroundColor: jaInscrito ? const Color(0xFF10B981) : Colors.white,
                        elevation: 0,
                        padding: const EdgeInsets.symmetric(horizontal: 14),
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(18),
                          side: BorderSide(
                            color: jaInscrito ? const Color(0xFFA7F3D0) : Colors.transparent,
                          ),
                        ),
                      ),
                      child: Row(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          Icon(
                            jaInscrito ? Icons.check_circle_rounded : Icons.add_circle_outline_rounded,
                            size: 16,
                            color: jaInscrito ? const Color(0xFF10B981) : Colors.white,
                          ),
                          const SizedBox(width: 6),
                          Text(
                            jaInscrito ? 'Confirmado' : 'Participar',
                            style: TextStyle(
                              fontSize: 13,
                              fontWeight: FontWeight.w700,
                              color: jaInscrito ? const Color(0xFF047857) : Colors.white,
                            ),
                          ),
                        ],
                      ),
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }
}
