import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import '../models/evento.dart';
import '../services/evento_service.dart';
import '../services/auth_service.dart';

/// Tela de Eventos Mobile — Fiel à Estilização Real da Tela WEB (.page-header, kicker viva mais fora da tela, .card, .btn-primary, modal web-style).
/// Fonte da Verdade: web/style.css
class EventosPage extends StatefulWidget {
  final VoidCallback? onVerMapa;

  const EventosPage({
    super.key,
    this.onVerMapa,
  });

  @override
  State<EventosPage> createState() => _EventosPageState();
}

class _EventosPageState extends State<EventosPage> {
  final EventoService _service = EventoService();
  List<Evento> _eventos = [];
  List<Evento> _eventosFiltrados = [];
  Map<int, int> _totalParticipantesPorEvento = {};
  Set<int> _eventosInscritos = {};

  bool _carregando = true;
  String? _erro;
  String _filtroStatus = 'TODOS';
  final TextEditingController _buscaCtrl = TextEditingController();

  @override
  void initState() {
    super.initState();
    _carregarEventos();
  }

  @override
  void dispose() {
    _buscaCtrl.dispose();
    super.dispose();
  }

  Future<void> _carregarEventos() async {
    setState(() {
      _carregando = true;
      _erro = null;
    });

    try {
      final lista = await _service.listarEventos();

      if (mounted) {
        setState(() {
          _eventos = lista;
          _carregando = false;
          _aplicarFiltros();
        });
      }

      _carregarMetadadosEventos(lista);
    } catch (e) {
      if (mounted) {
        setState(() {
          _erro = 'Não foi possível carregar os eventos da rede.';
          _carregando = false;
        });
      }
    }
  }

  Future<void> _carregarMetadadosEventos(List<Evento> lista) async {
    final Map<int, int> mapaContagem = Map.from(_totalParticipantesPorEvento);
    final Set<int> inscritos = Set.from(_eventosInscritos);

    await Future.wait(lista.map((ev) async {
      try {
        final total = await _service.contarParticipantes(ev.id);
        mapaContagem[ev.id] = total;

        if (AuthService.logado && AuthService.idUsuario != null) {
          final ids = await _service.listarIdsParticipantes(ev.id);
          if (ids.contains(AuthService.idUsuario)) {
            inscritos.add(ev.id);
          }
        }
      } catch (_) {}
    }));

    if (mounted) {
      setState(() {
        _totalParticipantesPorEvento = mapaContagem;
        _eventosInscritos = inscritos;
        _aplicarFiltros();
      });
    }
  }

  void _aplicarFiltros() {
    final query = _buscaCtrl.text.trim().toLowerCase();
    setState(() {
      _eventosFiltrados = _eventos.where((ev) {
        final sit = ev.situacaoCalculada.toUpperCase();
        final matchStatus = _filtroStatus == 'TODOS' ||
            (_filtroStatus == 'AGENDADO' && (sit == 'AGENDADO' || sit == 'ATIVO' || sit == 'DISPONIVEL' || sit == 'PUBLICADO' || sit == 'INDEFINIDO')) ||
            (_filtroStatus == 'ACONTECENDO_AGORA' && (sit == 'ACONTECENDO_AGORA' || sit == 'EM_ANDAMENTO' || sit == 'AO VIVO')) ||
            (_filtroStatus == 'ENCERRADO' && (sit == 'ENCERRADO' || sit == 'FINALIZADO')) ||
            (_filtroStatus == 'INSCRITO' && _eventosInscritos.contains(ev.id));

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
      _mostrarSnack('Faça login para participar de eventos.', cor: const Color(0xFFEA3F74));
      return;
    }

    final userId = AuthService.idUsuario!;
    final jaInscrito = _eventosInscritos.contains(evento.id);

    if (jaInscrito) {
      final ok = await _service.sairEvento(evento.id, userId);
      if (ok && mounted) {
        setState(() {
          _eventosInscritos.remove(evento.id);
          final atual = _totalParticipantesPorEvento[evento.id] ?? 1;
          _totalParticipantesPorEvento[evento.id] = (atual > 0) ? atual - 1 : 0;
        });
        _mostrarSnack('Você cancelou sua presença no evento.', cor: const Color(0xFF6B7280));
      } else {
        _mostrarSnack('Não foi possível cancelar a presença.', cor: const Color(0xFFC93659));
      }
    } else {
      final ok = await _service.participarEvento(evento.id, userId);
      if (ok && mounted) {
        setState(() {
          _eventosInscritos.add(evento.id);
          final atual = _totalParticipantesPorEvento[evento.id] ?? 0;
          _totalParticipantesPorEvento[evento.id] = atual + 1;
        });
        _mostrarSnack('Presença confirmada em "${evento.titulo}"!', cor: const Color(0xFF10B981));
      } else {
        _mostrarSnack('Não foi possível confirmar presença.', cor: const Color(0xFFC93659));
      }
    }
  }

  void _mostrarSnack(String msg, {Color cor = const Color(0xFF10B981)}) {
    if (!mounted) return;
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(msg, style: GoogleFonts.manrope(fontWeight: FontWeight.w600)),
        backgroundColor: cor,
        duration: const Duration(seconds: 2),
      ),
    );
  }

  void _abrirDetalhesEventoModal(Evento evento) {
    final bool jaInscrito = _eventosInscritos.contains(evento.id);
    final int total = _totalParticipantesPorEvento[evento.id] ?? 0;
    final sit = evento.situacaoCalculada;
    final bool emAndamento = sit == 'ACONTECENDO_AGORA';

    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.transparent,
      builder: (ctx) => Container(
        padding: const EdgeInsets.all(24),
        decoration: const BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.vertical(top: Radius.circular(16)),
        ),
        child: SingleChildScrollView(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
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
              const SizedBox(height: 16),

              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text(
                    evento.categoria ?? '🎉 EVENTO',
                    style: GoogleFonts.manrope(
                      color: const Color(0xFFEA3F74),
                      fontSize: 11,
                      fontWeight: FontWeight.w800,
                      letterSpacing: 0.8,
                    ),
                  ),
                  Container(
                    padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                    decoration: BoxDecoration(
                      color: emAndamento ? const Color(0xFFFEF2F2) : const Color(0xFFF7F8FA),
                      borderRadius: BorderRadius.circular(6),
                      border: Border.all(color: const Color(0xFFE5E7EB)),
                    ),
                    child: Text(
                      emAndamento ? '🔥 ACONTECENDO AGORA' : 'STATUS: ${evento.status}',
                      style: GoogleFonts.manrope(
                        color: emAndamento ? const Color(0xFFC93659) : const Color(0xFF6B7280),
                        fontSize: 11,
                        fontWeight: FontWeight.w700,
                      ),
                    ),
                  ),
                ],
              ),
              const SizedBox(height: 12),

              Text(
                evento.titulo,
                style: GoogleFonts.manrope(
                  fontSize: 22,
                  fontWeight: FontWeight.w700,
                  color: const Color(0xFF202124),
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

              if (evento.descricao != null && evento.descricao!.isNotEmpty) ...[
                const SizedBox(height: 16),
                Text('Sobre o Evento', style: GoogleFonts.manrope(fontSize: 15, fontWeight: FontWeight.w700, color: const Color(0xFF202124))),
                const SizedBox(height: 6),
                Text(
                  evento.descricao!,
                  style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF6B7280), height: 1.5),
                ),
              ],

              const SizedBox(height: 24),

              Row(
                children: [
                  if (widget.onVerMapa != null)
                    Expanded(
                      child: OutlinedButton(
                        onPressed: () {
                          Navigator.pop(ctx);
                          widget.onVerMapa!();
                        },
                        style: OutlinedButton.styleFrom(
                          foregroundColor: const Color(0xFF202124),
                          side: const BorderSide(color: Color(0xFFE5E7EB)),
                          padding: const EdgeInsets.symmetric(vertical: 12),
                          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                        ),
                        child: Text('Ver no Mapa', style: GoogleFonts.manrope(fontWeight: FontWeight.w700)),
                      ),
                    ),
                  if (widget.onVerMapa != null) const SizedBox(width: 12),
                  Expanded(
                    child: ElevatedButton(
                      onPressed: () {
                        Navigator.pop(ctx);
                        _toggleParticipacao(evento);
                      },
                      style: ElevatedButton.styleFrom(
                        backgroundColor: jaInscrito ? const Color(0xFF10B981) : const Color(0xFFEA3F74),
                        foregroundColor: Colors.white,
                        elevation: 0,
                        padding: const EdgeInsets.symmetric(vertical: 12),
                        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                      ),
                      child: Text(jaInscrito ? 'Confirmado' : 'Participar', style: GoogleFonts.manrope(fontWeight: FontWeight.w700)),
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
              style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF6B7280)),
              children: [
                TextSpan(text: '$label: ', style: const TextStyle(fontWeight: FontWeight.w700, color: Color(0xFF202124))),
                TextSpan(text: value),
              ],
            ),
          ),
        ),
      ],
    );
  }

  void _abrirCriarEvento() {
    if (!AuthService.logado || AuthService.idUsuario == null) {
      _mostrarSnack('Faça login para criar eventos.', cor: const Color(0xFFEA3F74));
      return;
    }

    final tituloCtrl = TextEditingController();
    final localCtrl = TextEditingController();
    final descricaoCtrl = TextEditingController();
    DateTime? dataSelecionada;
    TimeOfDay? horarioInicioSelecionado;
    TimeOfDay? horarioFimSelecionado;
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
                  'Novo Evento',
                  style: GoogleFonts.manrope(
                    fontSize: 20,
                    fontWeight: FontWeight.w700,
                    color: const Color(0xFF202124),
                  ),
                ),
                const SizedBox(height: 20),

                _modalField(tituloCtrl, 'Título do Evento', Icons.event_rounded),
                const SizedBox(height: 12),
                _modalField(localCtrl, 'Local do Evento', Icons.location_on_outlined),
                const SizedBox(height: 12),
                _modalField(descricaoCtrl, 'Descrição detalhada', Icons.description_outlined, maxLines: 3),
                const SizedBox(height: 12),

                GestureDetector(
                  onTap: () async {
                    final d = await showDatePicker(
                      context: ctx,
                      initialDate: DateTime.now().add(const Duration(days: 1)),
                      firstDate: DateTime.now(),
                      lastDate: DateTime(2030),
                    );
                    if (d != null) setModalState(() => dataSelecionada = d);
                  },
                  child: Container(
                    padding: const EdgeInsets.all(12),
                    decoration: BoxDecoration(
                      border: Border.all(color: const Color(0xFFE5E7EB)),
                      borderRadius: BorderRadius.circular(10),
                      color: Colors.white,
                    ),
                    child: Row(
                      children: [
                        const Icon(Icons.calendar_today_outlined, size: 18, color: Color(0xFFEA3F74)),
                        const SizedBox(width: 10),
                        Text(
                          dataSelecionada != null
                              ? '${dataSelecionada!.day.toString().padLeft(2, '0')}/${dataSelecionada!.month.toString().padLeft(2, '0')}/${dataSelecionada!.year}'
                              : 'Selecionar data do evento',
                          style: GoogleFonts.manrope(
                            color: dataSelecionada != null ? const Color(0xFF202124) : const Color(0xFF6B7280),
                            fontSize: 14,
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
                const SizedBox(height: 12),

                Row(
                  children: [
                    Expanded(
                      child: GestureDetector(
                        onTap: () async {
                          final t = await showTimePicker(
                              context: ctx,
                              initialTime: const TimeOfDay(hour: 19, minute: 0));
                          if (t != null) setModalState(() => horarioInicioSelecionado = t);
                        },
                        child: Container(
                          padding: const EdgeInsets.all(12),
                          decoration: BoxDecoration(
                            border: Border.all(color: const Color(0xFFE5E7EB)),
                            borderRadius: BorderRadius.circular(10),
                            color: Colors.white,
                          ),
                          child: Row(
                            children: [
                              const Icon(Icons.schedule_rounded, size: 16, color: Color(0xFFEA3F74)),
                              const SizedBox(width: 6),
                              Text(
                                horarioInicioSelecionado != null ? horarioInicioSelecionado!.format(ctx) : 'Início',
                                style: GoogleFonts.manrope(fontSize: 13),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),
                    const SizedBox(width: 10),
                    Expanded(
                      child: GestureDetector(
                        onTap: () async {
                          final t = await showTimePicker(
                              context: ctx,
                              initialTime: const TimeOfDay(hour: 22, minute: 0));
                          if (t != null) setModalState(() => horarioFimSelecionado = t);
                        },
                        child: Container(
                          padding: const EdgeInsets.all(12),
                          decoration: BoxDecoration(
                            border: Border.all(color: const Color(0xFFE5E7EB)),
                            borderRadius: BorderRadius.circular(10),
                            color: Colors.white,
                          ),
                          child: Row(
                            children: [
                              const Icon(Icons.schedule_rounded, size: 16, color: Color(0xFFEA3F74)),
                              const SizedBox(width: 6),
                              Text(
                                horarioFimSelecionado != null ? horarioFimSelecionado!.format(ctx) : 'Fim',
                                style: GoogleFonts.manrope(fontSize: 13),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),
                  ],
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
                            if (tituloCtrl.text.trim().isEmpty ||
                                localCtrl.text.trim().isEmpty ||
                                dataSelecionada == null ||
                                horarioInicioSelecionado == null) {
                              _mostrarSnack('Preencha os campos obrigatórios.', cor: const Color(0xFFC93659));
                              return;
                            }

                            setModalState(() => salvando = true);

                            final ano = dataSelecionada!.year;
                            final mes = dataSelecionada!.month.toString().padLeft(2, '0');
                            final dia = dataSelecionada!.day.toString().padLeft(2, '0');
                            final dataIso = '$ano-$mes-$dia';

                            final hIni = '${horarioInicioSelecionado!.hour.toString().padLeft(2, '0')}:${horarioInicioSelecionado!.minute.toString().padLeft(2, '0')}:00';
                            final hFim = horarioFimSelecionado != null
                                ? '${horarioFimSelecionado!.hour.toString().padLeft(2, '0')}:${horarioFimSelecionado!.minute.toString().padLeft(2, '0')}:00'
                                : '${(horarioInicioSelecionado!.hour + 2) % 24}:${horarioInicioSelecionado!.minute.toString().padLeft(2, '0')}:00';

                            final novoEvento = await _service.criarEvento({
                              'titulo': tituloCtrl.text.trim(),
                              'localEvento': localCtrl.text.trim(),
                              'descricao': descricaoCtrl.text.trim().isEmpty ? null : descricaoCtrl.text.trim(),
                              'dataEvento': dataIso,
                              'horarioInicio': hIni,
                              'horarioFim': hFim,
                              'criadorId': AuthService.idUsuario,
                              'status': 'AGENDADO',
                              'exigeCheckin': false,
                            });

                            if (ctx.mounted) {
                              Navigator.pop(ctx);
                              if (novoEvento != null) {
                                _mostrarSnack('Evento criado com sucesso! 🎉');
                                _carregarEventos();
                              } else {
                                _mostrarSnack('Erro ao criar evento.', cor: const Color(0xFFC93659));
                              }
                            }
                          },
                    child: salvando
                        ? const SizedBox(height: 20, width: 20, child: CircularProgressIndicator(color: Colors.white, strokeWidth: 2))
                        : Text('Criar Evento', style: GoogleFonts.manrope(fontSize: 14, fontWeight: FontWeight.w700)),
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

  Widget _modalField(TextEditingController ctrl, String label, IconData icon, {int maxLines = 1}) {
    return TextField(
      controller: ctrl,
      maxLines: maxLines,
      style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF202124)),
      decoration: InputDecoration(
        labelText: label,
        prefixIcon: Icon(icon, size: 18, color: const Color(0xFFEA3F74)),
        filled: true,
        fillColor: Colors.white,
        border: OutlineInputBorder(borderRadius: BorderRadius.circular(10), borderSide: const BorderSide(color: Color(0xFFE5E7EB))),
        enabledBorder: OutlineInputBorder(borderRadius: BorderRadius.circular(10), borderSide: const BorderSide(color: Color(0xFFE5E7EB))),
        focusedBorder: OutlineInputBorder(borderRadius: BorderRadius.circular(10), borderSide: const BorderSide(color: Color(0xFFEA3F74), width: 1.5)),
        contentPadding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF7F8FA),
      body: RefreshIndicator(
        onRefresh: _carregarEventos,
        color: const Color(0xFFEA3F74),
        child: CustomScrollView(
          slivers: [
            // Page Header Fiel à Web (.page-header com kicker VIVA MAIS FORA DA TELA)
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
                          'VIVA MAIS FORA DA TELA',
                          style: GoogleFonts.manrope(
                            color: const Color(0xFFEA3F74),
                            fontSize: 11,
                            fontWeight: FontWeight.w800,
                            letterSpacing: 0.9,
                          ),
                        ),
                        const SizedBox(height: 2),
                        Text(
                          'Descubra seu próximo evento',
                          style: GoogleFonts.manrope(
                            fontSize: 22,
                            fontWeight: FontWeight.w700,
                            color: const Color(0xFF202124),
                          ),
                        ),
                      ],
                    ),
                    ElevatedButton.icon(
                      onPressed: _abrirCriarEvento,
                      icon: const Icon(Icons.add_rounded, size: 16),
                      label: Text('Novo Evento', style: GoogleFonts.manrope(fontWeight: FontWeight.w700, fontSize: 13)),
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

            // Banner Destaque (Fiel ao .evento-destaque da Web) se houver eventos
            if (_eventos.isNotEmpty)
              SliverToBoxAdapter(
                child: Padding(
                  padding: const EdgeInsets.fromLTRB(16, 4, 16, 12),
                  child: Container(
                    padding: const EdgeInsets.all(20),
                    decoration: BoxDecoration(
                      gradient: const LinearGradient(
                        colors: [Color(0xFF472632), Color(0xFFEA3F74)],
                        begin: Alignment.topLeft,
                        end: Alignment.bottomRight,
                      ),
                      borderRadius: BorderRadius.circular(16),
                      boxShadow: const [
                        BoxShadow(
                          color: Color.fromRGBO(120, 34, 66, 0.16),
                          blurRadius: 24,
                          offset: Offset(0, 8),
                        ),
                      ],
                    ),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          'EM DESTAQUE',
                          style: GoogleFonts.manrope(
                            color: const Color(0xFFF9ACC6),
                            fontSize: 11,
                            fontWeight: FontWeight.w800,
                            letterSpacing: 0.9,
                          ),
                        ),
                        const SizedBox(height: 6),
                        Text(
                          _eventos.first.titulo,
                          style: GoogleFonts.manrope(
                            color: Colors.white,
                            fontSize: 20,
                            fontWeight: FontWeight.w800,
                          ),
                          maxLines: 2,
                        ),
                        const SizedBox(height: 6),
                        Text(
                          _eventos.first.localEvento,
                          style: GoogleFonts.manrope(color: Colors.white70, fontSize: 13),
                        ),
                        const SizedBox(height: 14),
                        ElevatedButton.icon(
                          onPressed: () => _abrirDetalhesEventoModal(_eventos.first),
                          icon: const Icon(Icons.arrow_forward_rounded, size: 16),
                          label: const Text('Ver detalhes'),
                          style: ElevatedButton.styleFrom(
                            backgroundColor: const Color(0xFFF9ACC6),
                            foregroundColor: const Color(0xFF202124),
                            shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
              ),

            // Campo de Pesquisa (.search-bar da Web)
            SliverToBoxAdapter(
              child: Padding(
                padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
                child: TextField(
                  controller: _buscaCtrl,
                  onChanged: (_) => _aplicarFiltros(),
                  style: GoogleFonts.manrope(fontSize: 14, color: const Color(0xFF202124)),
                  decoration: InputDecoration(
                    hintText: 'Buscar por nome ou descrição...',
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
              child: SingleChildScrollView(
                scrollDirection: Axis.horizontal,
                padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                child: Row(
                  children: [
                    _buildFilterTab('TODOS', 'Descobrir'),
                    _buildFilterTab('AGENDADO', 'Agendados'),
                    _buildFilterTab('ACONTECENDO_AGORA', '🔥 Ao Vivo'),
                    _buildFilterTab('ENCERRADO', 'Encerrados'),
                    if (AuthService.logado) _buildFilterTab('INSCRITO', 'Meus eventos'),
                  ],
                ),
              ),
            ),

            // Lista de Cards de Eventos Fiel à Web (.card)
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
                        onPressed: _carregarEventos,
                        child: const Text('Tentar novamente'),
                      ),
                    ],
                  ),
                ),
              )
            else if (_eventosFiltrados.isEmpty)
              SliverFillRemaining(
                child: Center(
                  child: Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      const Icon(Icons.event_busy_rounded, size: 48, color: Color(0xFF6B7280)),
                      const SizedBox(height: 12),
                      Text('Nenhum evento encontrado', style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 14, fontWeight: FontWeight.w700)),
                      const SizedBox(height: 6),
                      Text('Tente ajustar os filtros.', style: GoogleFonts.manrope(color: const Color(0xFF6B7280), fontSize: 12)),
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
                      final ev = _eventosFiltrados[index];
                      final bool isInscrito = _eventosInscritos.contains(ev.id);
                      final int totalParticipantes = _totalParticipantesPorEvento[ev.id] ?? 0;

                      return _buildWebStyleEventCard(ev, totalParticipantes, isInscrito);
                    },
                    childCount: _eventosFiltrados.length,
                  ),
                ),
              ),
          ],
        ),
      ),
    );
  }

  Widget _buildFilterTab(String key, String label) {
    final isSelected = _filtroStatus == key;
    return GestureDetector(
      onTap: () {
        setState(() {
          _filtroStatus = key;
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

  /// Card de Evento Fiel ao `.card` da Web
  Widget _buildWebStyleEventCard(Evento evento, int total, bool jaInscrito) {
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
      child: Material(
        color: Colors.transparent,
        borderRadius: BorderRadius.circular(14),
        child: InkWell(
          borderRadius: BorderRadius.circular(14),
          onTap: () => _abrirDetalhesEventoModal(evento),
          child: Padding(
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    // Box com Ícone / Data do Evento
                    Container(
                      width: 48,
                      height: 48,
                      decoration: BoxDecoration(
                        color: const Color(0xFFF7F8FA),
                        borderRadius: BorderRadius.circular(10),
                        border: Border.all(color: const Color(0xFFE5E7EB)),
                      ),
                      child: const Column(
                        mainAxisAlignment: MainAxisAlignment.center,
                        children: [
                          Icon(Icons.event, color: Color(0xFFEA3F74), size: 22),
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
                            style: GoogleFonts.manrope(
                              fontSize: 16,
                              fontWeight: FontWeight.w700,
                              color: const Color(0xFF202124),
                            ),
                          ),
                          const SizedBox(height: 4),
                          Row(
                            children: [
                              const Icon(Icons.location_on_outlined, color: Color(0xFF6B7280), size: 14),
                              const SizedBox(width: 4),
                              Expanded(
                                child: Text(
                                  evento.localEvento,
                                  style: GoogleFonts.manrope(
                                    fontSize: 12,
                                    color: const Color(0xFF6B7280),
                                  ),
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
                  const SizedBox(height: 12),
                  Text(
                    evento.descricao!,
                    style: GoogleFonts.manrope(
                      fontSize: 13,
                      color: const Color(0xFF6B7280),
                      height: 1.45,
                    ),
                    maxLines: 2,
                    overflow: TextOverflow.ellipsis,
                  ),
                ],

                const SizedBox(height: 14),
                const Divider(color: Color(0xFFE5E7EB), height: 1),
                const SizedBox(height: 12),

                // Rodapé com Participantes e Botão (.btn-primary de 10px radius)
                Row(
                  children: [
                    const Icon(Icons.people_outline_rounded, size: 16, color: Color(0xFF6B7280)),
                    const SizedBox(width: 6),
                    Text(
                      '$total confirmados',
                      style: GoogleFonts.manrope(
                        fontSize: 12,
                        fontWeight: FontWeight.w600,
                        color: const Color(0xFF6B7280),
                      ),
                    ),
                    const Spacer(),

                    ElevatedButton(
                      onPressed: () => _toggleParticipacao(evento),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: jaInscrito ? const Color(0xFF10B981) : const Color(0xFFEA3F74),
                        foregroundColor: Colors.white,
                        elevation: 0,
                        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(10),
                        ),
                      ),
                      child: Text(
                        jaInscrito ? 'Confirmado' : 'Participar',
                        style: GoogleFonts.manrope(
                          fontSize: 12,
                          fontWeight: FontWeight.w700,
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
    );
  }
}
