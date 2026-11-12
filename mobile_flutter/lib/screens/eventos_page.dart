import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'package:latlong2/latlong.dart';
import 'package:geolocator/geolocator.dart';
import '../services/authenticated_http.dart' as http;
import 'dart:convert';
import '../models/evento.dart';
import '../services/evento_service.dart';
import '../services/auth_service.dart';
import '../services/api_service.dart';
import 'login_screen.dart';

/// Redesign mobile: descoberta > mapa > eventos > ações.
class EventosPage extends StatefulWidget {
  final VoidCallback? onVerMapa;
  const EventosPage({super.key, this.onVerMapa});
  @override
  State<EventosPage> createState() => _EventosPageState();
}

class _EventosPageState extends State<EventosPage> {
  static const roxo = Color(0xFFEA3F74);
  static const escuro = Color(0xFF202124);
  static const cinza = Color(0xFF6B7280);
  final EventoService _service = EventoService();
  final TextEditingController _busca = TextEditingController();
  List<Evento> _eventos = [];
  bool _carregando = true;
  String? _erro;
  String _categoria = 'Todos';
  final Set<int> _inscritosConfirmados = <int>{};
  final Set<int> _operacoesEmAndamento = <int>{};

  @override
  void initState() { super.initState(); _carregar(); }
  @override
  void dispose() { _busca.dispose(); super.dispose(); }

  List<Evento> get _filtrados => _eventos.where((e) {
    final q = _busca.text.trim().toLowerCase();
    final bate = q.isEmpty || e.titulo.toLowerCase().contains(q) ||
        e.localEvento.toLowerCase().contains(q) ||
        (e.descricao ?? '').toLowerCase().contains(q);
    return bate && (_categoria == 'Todos' || (e.categoria ?? 'Outros') == _categoria);
  }).toList();

  List<String> get _categorias {
    final set = _eventos.map((e) => e.categoria?.trim() ?? 'Outros')
        .where((s) => s.isNotEmpty).toSet().toList()..sort();
    return ['Todos', ...set];
  }

  Future<void> _carregar() async {
    if (mounted) setState(() { _carregando = true; _erro = null; });
    try {
      final lista = await _service.listarEventos();
      if (mounted) setState(() { _eventos = lista; _carregando = false; });
    } catch (e) {
      if (mounted) setState(() { _erro = 'Não foi possível carregar os eventos. Confira a API: $e'; _carregando = false; });
    }
  }

  void _snack(String text) {
    if (mounted) ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text(text)));
  }

  bool _cancelado(Evento evento) {
    final situacao = evento.status.toUpperCase();
    return situacao == 'CANCELADO' || situacao == 'ENCERRADO' || situacao == 'FINALIZADO';
  }

  Future<void> _alternarParticipacao(Evento evento) async {
    if (!AuthService.logado || AuthService.idUsuario == null) {
      await Navigator.of(context).push(MaterialPageRoute(builder: (_) => const LoginScreen()));
      return;
    }
    if (_operacoesEmAndamento.contains(evento.id) || _cancelado(evento)) return;
    setState(() => _operacoesEmAndamento.add(evento.id));
    try {
      final usuario = AuthService.idUsuario!;
      final jaInscrito = _inscritosConfirmados.contains(evento.id);
      final sucesso = jaInscrito
          ? await _service.sairEvento(evento.id, usuario)
          : await _service.participarEvento(evento.id, usuario);
      if (!mounted) return;
      if (sucesso) {
        setState(() {
          if (jaInscrito) {
            _inscritosConfirmados.remove(evento.id);
          } else {
            _inscritosConfirmados.add(evento.id);
          }
        });
        _snack(jaInscrito ? 'Você saiu do evento.' : 'Participação confirmada!');
      } else {
        _snack('Não foi possível atualizar sua inscrição. Confira a API e as regras do evento.');
      }
    } finally {
      if (mounted) setState(() => _operacoesEmAndamento.remove(evento.id));
    }
  }

  Future<void> _abrirDetalhes(Evento evento) async {
    int? total;
    bool? inscrito;
    bool enviando = false;
    String? erro;
    if (AuthService.idUsuario != null) {
      try {
        final resultados = await Future.wait([
          _service.listarIdsParticipantes(evento.id),
          _service.contarParticipantes(evento.id),
        ]);
        if (resultados[0] is List<int>) {
          inscrito = (resultados[0] as List<int>).contains(AuthService.idUsuario);
          if (mounted) {
            setState(() {
            if (inscrito == true) {
              _inscritosConfirmados.add(evento.id);
            } else {
              _inscritosConfirmados.remove(evento.id);
            }
          });
          }
        }
        if (resultados[1] is int) total = resultados[1] as int;
      } catch (_) {}
    }
    if (!mounted) return;
    await showModalBottomSheet<void>(context: context, isScrollControlled: true,
      useSafeArea: true, backgroundColor: Colors.transparent,
      builder: (sheetContext) => StatefulBuilder(builder: (ctx, modalSetState) =>
        Container(constraints: BoxConstraints(maxHeight: MediaQuery.of(ctx).size.height * .9),
          decoration: const BoxDecoration(color: Colors.white,
            borderRadius: BorderRadius.vertical(top: Radius.circular(28))),
          child: SingleChildScrollView(padding: const EdgeInsets.fromLTRB(22,12,22,30),
            child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
              Center(child: Container(width: 42, height: 5,
                decoration: BoxDecoration(color: const Color(0xFFD9DCE5), borderRadius: BorderRadius.circular(20)))),
              const SizedBox(height: 22),
              Text(evento.categoria ?? 'EVENTO LOCAL', style: const TextStyle(
                color: roxo, fontSize: 11, letterSpacing: 1, fontWeight: FontWeight.w900)),
              const SizedBox(height: 8),
              Text(evento.titulo, style: const TextStyle(fontSize: 27,
                fontWeight: FontWeight.w900, color: escuro, letterSpacing: -.7)),
              const SizedBox(height: 16),
              _info(Icons.calendar_month_outlined, evento.dataFormatada),
              _info(Icons.schedule_rounded, '${evento.horarioFormatado} - ${evento.horarioFim.length >= 5 ? evento.horarioFim.substring(0,5) : evento.horarioFim}'),
              _info(Icons.location_on_outlined, evento.localEvento),
              if (total != null) _info(Icons.people_outline, '$total participantes'),
              if (evento.descricao != null && evento.descricao!.isNotEmpty) ...[
                const SizedBox(height: 22),
                const Text('Sobre o evento', style: TextStyle(fontSize: 17, fontWeight: FontWeight.w800)),
                const SizedBox(height: 9),
                Text(evento.descricao!, style: const TextStyle(height: 1.5, color: cinza)),
              ],
              if (erro != null) ...[
                const SizedBox(height: 12),
                Text(erro!, style: const TextStyle(color: Color(0xFFCD3459), fontWeight: FontWeight.w600)),
              ],
              const SizedBox(height: 26),
              Row(children: [
                if (widget.onVerMapa != null) ...[
                  Expanded(child: OutlinedButton.icon(
                    onPressed: () { Navigator.pop(ctx); widget.onVerMapa?.call(); },
                    icon: const Icon(Icons.map_outlined), label: const Text('Mapa'))),
                  const SizedBox(width: 10),
                ],
                Expanded(flex: 2, child: FilledButton.icon(
                  onPressed: (enviando || _cancelado(evento)) ? null : () async {
                    if (!AuthService.logado || AuthService.idUsuario == null) {
                      Navigator.pop(ctx);
                      await Navigator.of(context).push(MaterialPageRoute(builder: (_) => const LoginScreen()));
                      return;
                    }
                    modalSetState(() { enviando = true; erro = null; });
                    // Não presumir inscrição: confiar apenas no retorno do servidor.
                    final id = AuthService.idUsuario!;
                    final ok = inscrito == true
                      ? await _service.sairEvento(evento.id, id)
                      : await _service.participarEvento(evento.id, id);
                    if (!ctx.mounted) return;
                    if (ok) {
                      modalSetState(() {
                        inscrito = !(inscrito ?? false);
                        total = (total ?? 0) + (inscrito == true ? 1 : -1);
                        if (total! < 0) total = 0;
                        enviando = false;
                      });
                      if (mounted) {
                        setState(() {
                        if (inscrito == true) {
                          _inscritosConfirmados.add(evento.id);
                        } else {
                          _inscritosConfirmados.remove(evento.id);
                        }
                      });
                      }
                      _snack(inscrito == true ? 'Inscrição confirmada!' : 'Inscrição cancelada.');
                    } else {
                      modalSetState(() {
                        enviando = false;
                        erro = 'A API recusou a participação. Verifique se /api/eventos/${evento.id}/participar/$id existe e se as inscrições estão abertas.';
                      });
                    }
                  },
                  icon: enviando ? const SizedBox(width: 16, height: 16,
                    child: CircularProgressIndicator(strokeWidth: 2, color: Colors.white)) :
                    Icon(inscrito == true ? Icons.check_circle_outline_rounded : Icons.confirmation_number_outlined),
                  label: Text(_cancelado(evento) ? 'Evento encerrado' : (inscrito == true ? 'Inscrito · Sair' : 'Participar agora')),
                )),
              ]),
            ]))),
      ),
    );
  }

  Widget _info(IconData icon, String value) => Padding(
    padding: const EdgeInsets.only(bottom: 12),
    child: Row(children: [Icon(icon, color: roxo, size: 19),
      const SizedBox(width: 12), Expanded(child: Text(value,
        style: const TextStyle(color: escuro, fontWeight: FontWeight.w600)))]));

  Future<void> _criarEvento() async {
    if (!AuthService.logado || AuthService.idUsuario == null) {
      await Navigator.of(context).push(MaterialPageRoute(builder: (_) => const LoginScreen()));
      return;
    }
    final titulo = TextEditingController();
    final descricao = TextEditingController();
    final local = TextEditingController();
    final limiteCtrl = TextEditingController();
    LatLng? pontoEvento;
    String categoria = 'Tecnologia';
    bool exigeCheckin = false;
    DateTime? data;
    TimeOfDay? inicio;
    TimeOfDay? fim;
    bool salvando = false;
    String? erro;
    try {
      await showModalBottomSheet<void>(context: context, isScrollControlled: true,
        useSafeArea: true, backgroundColor: Colors.white,
        builder: (modalCtx) => StatefulBuilder(builder: (ctx, setModalState) {
          String hora(TimeOfDay? h) => h == null ? 'Selecionar horário' :
            '${h.hour.toString().padLeft(2,'0')}:${h.minute.toString().padLeft(2,'0')}';
          String dataFormatada() => data == null ? 'Escolher data' :
            '${data!.day.toString().padLeft(2,'0')}/${data!.month.toString().padLeft(2,'0')}/${data!.year}';
          return Padding(padding: EdgeInsets.fromLTRB(22,24,22,MediaQuery.of(ctx).viewInsets.bottom + 24),
            child: SingleChildScrollView(child: Column(mainAxisSize: MainAxisSize.min,
              crossAxisAlignment: CrossAxisAlignment.stretch, children: [
                const Text('Criar um evento', style: TextStyle(fontSize: 24, fontWeight: FontWeight.w900)),
                const SizedBox(height: 7),
                const Text('Convide pessoas para um momento especial.',
                  style: TextStyle(color: cinza)),
                const SizedBox(height: 20),
                TextField(controller: titulo, maxLength: 120,
                  decoration: const InputDecoration(labelText: 'Nome do evento', prefixIcon: Icon(Icons.event_note_outlined))),
                const SizedBox(height: 8),
                TextField(controller: local,
                  decoration: const InputDecoration(labelText: 'Endereço ou local', prefixIcon: Icon(Icons.location_on_outlined))),
                const SizedBox(height: 10),
                OutlinedButton.icon(
                  onPressed: () async {
                    final ponto = await Navigator.of(ctx).push<LatLng>(
                      MaterialPageRoute(builder: (_) => _SeletorPontoMapa(
                        pontoInicial: pontoEvento,
                      )),
                    );
                    if (ponto != null && ctx.mounted) {
                      setModalState(() => pontoEvento = ponto);
                    }
                  },
                  icon: const Icon(Icons.place_outlined),
                  label: Text(pontoEvento == null
                      ? 'Escolher ponto no mapa'
                      : 'Local selecionado · alterar'),
                ),
                if (pontoEvento != null)
                  Padding(
                    padding: const EdgeInsets.only(top: 5, bottom: 5),
                    child: Text(
                      'Latitude: ${pontoEvento!.latitude.toStringAsFixed(6)} · Longitude: ${pontoEvento!.longitude.toStringAsFixed(6)}',
                      style: const TextStyle(fontSize: 12, color: cinza),
                    ),
                  ),
                const SizedBox(height: 14),
                DropdownButtonFormField<String>(
                  initialValue: categoria,
                  decoration: const InputDecoration(
                    labelText: 'Categoria',
                    prefixIcon: Icon(Icons.category_outlined),
                  ),
                  items: const [
                    'Tecnologia', 'Arte', 'Música', 'Esportes',
                    'Educação', 'Networking', 'Cultura',
                  ].map((c) => DropdownMenuItem<String>(
                    value: c, child: Text(c),
                  )).toList(),
                  onChanged: (nova) {
                    if (nova != null) setModalState(() => categoria = nova);
                  },
                ),
                const SizedBox(height: 14),
                TextField(controller: descricao, maxLines: 3,
                  decoration: const InputDecoration(labelText: 'Descrição', alignLabelWithHint: true)),
                const SizedBox(height: 14),
                TextField(
                  controller: limiteCtrl,
                  keyboardType: TextInputType.number,
                  decoration: const InputDecoration(
                    labelText: 'Limite de participantes (opcional)',
                    prefixIcon: Icon(Icons.groups_outlined),
                  ),
                ),
                CheckboxListTile(
                  contentPadding: EdgeInsets.zero,
                  value: exigeCheckin,
                  onChanged: (valor) => setModalState(() => exigeCheckin = valor ?? false),
                  title: const Text('Exigir check-in de participantes'),
                  controlAffinity: ListTileControlAffinity.leading,
                  activeColor: roxo,
                ),
                const SizedBox(height: 14),
                OutlinedButton.icon(onPressed: () async {
                  final escolhido = await showDatePicker(context: ctx,
                    initialDate: DateTime.now().add(const Duration(days: 1)),
                    firstDate: DateTime.now(), lastDate: DateTime.now().add(const Duration(days: 365 * 3)));
                  if (escolhido != null) setModalState(() => data = escolhido);
                }, icon: const Icon(Icons.calendar_month_outlined), label: Text(dataFormatada())),
                const SizedBox(height: 8),
                Row(children: [
                  Expanded(child: OutlinedButton(onPressed: () async {
                    final h = await showTimePicker(context: ctx, initialTime: TimeOfDay.now());
                    if (h != null) setModalState(() => inicio = h);
                  }, child: Text('Início ${hora(inicio)}'))),
                  const SizedBox(width: 8),
                  Expanded(child: OutlinedButton(onPressed: () async {
                    final h = await showTimePicker(context: ctx, initialTime: TimeOfDay.now());
                    if (h != null) setModalState(() => fim = h);
                  }, child: Text('Fim ${hora(fim)}'))),
                ]),
                if (erro != null) Padding(padding: const EdgeInsets.symmetric(vertical: 12),
                  child: Text(erro!, style: const TextStyle(color: Colors.red))),
                const SizedBox(height: 12),
                FilledButton(onPressed: salvando ? null : () async {
                  if (titulo.text.trim().isEmpty || local.text.trim().isEmpty ||
                      data == null || inicio == null || fim == null) {
                    setModalState(() => erro = 'Informe nome, local, data e horários.'); return;
                  }
                  if (pontoEvento == null) {
                    setModalState(() => erro = 'Escolha no mapa onde o evento acontecerá.');
                    return;
                  }
                  final a = inicio!.hour * 60 + inicio!.minute;
                  final b = fim!.hour * 60 + fim!.minute;
                  final inicioData = DateTime(data!.year,data!.month,data!.day,inicio!.hour,inicio!.minute);
                  if (b <= a || !inicioData.isAfter(DateTime.now())) {
                    setModalState(() => erro = 'Horários inválidos ou evento no passado.'); return;
                  }
                  final limiteTexto = limiteCtrl.text.trim();
                  final limite = limiteTexto.isEmpty ? null : int.tryParse(limiteTexto);
                  if (limiteTexto.isNotEmpty && (limite == null || limite < 1)) {
                    setModalState(() => erro = 'O limite deve ser um número positivo.');
                    return;
                  }
                  setModalState(() { salvando = true; erro = null; });
                  String time(TimeOfDay h) => '${h.hour.toString().padLeft(2,'0')}:${h.minute.toString().padLeft(2,'0')}:00';
                  final ev = await _service.criarEvento({
                    'titulo': titulo.text.trim(), 'descricao': descricao.text.trim(),
                    'localEvento': local.text.trim(),
                    'latitude': pontoEvento!.latitude,
                    'longitude': pontoEvento!.longitude,
                    'categoria': categoria,
                    'dataEvento': '${data!.year}-${data!.month.toString().padLeft(2,'0')}-${data!.day.toString().padLeft(2,'0')}',
                    'horarioInicio': time(inicio!), 'horarioFim': time(fim!),
                    'criadorId': AuthService.idUsuario,
                    'comunidadeId': null,
                    'limiteParticipantes': limite,
                    'encerramentoInscricoes': null,
                    'precoIngresso': 0.0,
                    'exigeCheckin': exigeCheckin,
                  });
                  if (!ctx.mounted) return;
                  if (ev == null) { setModalState(() {
                    salvando = false; erro = 'A API não aceitou esse evento. Confira a resposta no console.';
                  }); return; }
                  Navigator.pop(ctx);
                  _snack('Evento publicado!');
                  _carregar();
                }, child: Text(salvando ? 'Publicando...' : 'Criar evento')),
              ])));
        }));
    } finally {
      titulo.dispose();
      descricao.dispose();
      local.dispose();
      limiteCtrl.dispose();
    }
  }

  Widget _mapaPreview() {
    final localizados = _eventos.where((ev) => ev.latitude != null && ev.longitude != null).toList();
    final centro = localizados.isEmpty ? const LatLng(-22.5646, -47.4017)
      : LatLng(localizados.first.latitude!, localizados.first.longitude!);
    return ClipRRect(borderRadius: BorderRadius.circular(14), child: Container(
      decoration: const BoxDecoration(color: Colors.white),
      child: Column(crossAxisAlignment: CrossAxisAlignment.stretch, children: [
        SizedBox(height: 180, child: FlutterMap(
          options: MapOptions(initialCenter: centro, initialZoom: 12.5),
          children: [
            TileLayer(urlTemplate: 'https://tile.openstreetmap.org/{z}/{x}/{y}.png',
              userAgentPackageName: 'socialjoin.mobile'),
            MarkerLayer(markers: localizados.map((ev) => Marker(
              point: LatLng(ev.latitude!,ev.longitude!), width: 44, height: 44,
              child: GestureDetector(onTap: () => _abrirDetalhes(ev), child: Container(
                decoration: const BoxDecoration(color: roxo, shape: BoxShape.circle),
                child: const Icon(Icons.celebration_rounded, color: Colors.white, size: 22))),
            )).toList()),
            const RichAttributionWidget(attributions: [TextSourceAttribution('OpenStreetMap contributors')]),
          ],
        )),
        Padding(padding: const EdgeInsets.fromLTRB(16,14,12,14), child: Row(children: [
          Container(padding: const EdgeInsets.all(10),
            decoration: BoxDecoration(color: const Color(0xFFFFF1F6), borderRadius: BorderRadius.circular(12)),
            child: const Icon(Icons.map_rounded, color: roxo)),
          const SizedBox(width: 12),
          Expanded(child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
            const Text('Explore no mapa', style: TextStyle(fontSize: 15, fontWeight: FontWeight.w800)),
            Text(localizados.isEmpty ? 'Locais sem coordenadas na API' :
              '${localizados.length} eventos geolocalizados',
              style: const TextStyle(fontSize: 12, color: cinza)),
          ])),
          IconButton(onPressed: widget.onVerMapa,
            tooltip: 'Abrir mapa completo', icon: const Icon(Icons.arrow_forward_rounded)),
        ])),
      ]),
    ));
  }

  Widget _card(Evento evento) {
    final imagem = ApiService.formatarUrlMidia(evento.imagemCapa);
    final inscrito = _inscritosConfirmados.contains(evento.id);
    final ocupada = _operacoesEmAndamento.contains(evento.id);
    final encerrado = _cancelado(evento);
    return Container(
      margin: const EdgeInsets.only(bottom: 12),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(14),
        border: Border.all(color: const Color(0xFFE5E7EB)),
      ),
      child: Column(crossAxisAlignment: CrossAxisAlignment.stretch, children: [
        if (imagem != null)
          ClipRRect(
            borderRadius: const BorderRadius.vertical(top: Radius.circular(13)),
            child: Image.network(imagem, height: 172, width: double.infinity,
              fit: BoxFit.cover, errorBuilder: (_, __, ___) => const SizedBox.shrink()),
          ),
        Padding(
          padding: const EdgeInsets.fromLTRB(16, 14, 16, 16),
          child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
            Row(children: [
              Container(
                padding: const EdgeInsets.symmetric(horizontal: 9, vertical: 5),
                decoration: BoxDecoration(
                  color: const Color(0xFFFFF1F6),
                  borderRadius: BorderRadius.circular(7),
                ),
                child: Text((evento.categoria ?? 'EVENTO').toUpperCase(),
                  style: const TextStyle(color: roxo, fontSize: 10, fontWeight: FontWeight.w800)),
              ),
              const Spacer(),
              if (inscrito) const Row(children: [
                Icon(Icons.check_circle_rounded, color: Color(0xFF16A34A), size: 16),
                SizedBox(width: 4),
                Text('Inscrito', style: TextStyle(color: Color(0xFF16A34A), fontSize: 11, fontWeight: FontWeight.w700)),
              ]) else if (encerrado)
                const Text('Encerrado', style: TextStyle(color: cinza, fontSize: 11)),
            ]),
            const SizedBox(height: 11),
            Text(evento.titulo, maxLines: 2, overflow: TextOverflow.ellipsis,
              style: const TextStyle(fontSize: 18, fontWeight: FontWeight.w800,
                color: escuro, letterSpacing: -0.4)),
            const SizedBox(height: 10),
            Row(children: [
              const Icon(Icons.calendar_today_outlined, size: 16, color: cinza),
              const SizedBox(width: 8),
              Expanded(child: Text('${evento.dataFormatada} às ${evento.horarioFormatado}',
                style: const TextStyle(fontSize: 13, color: cinza))),
            ]),
            const SizedBox(height: 7),
            Row(children: [
              const Icon(Icons.location_on_outlined, size: 17, color: cinza),
              const SizedBox(width: 7),
              Expanded(child: Text(evento.localEvento, maxLines: 1, overflow: TextOverflow.ellipsis,
                style: const TextStyle(fontSize: 13, color: cinza))),
            ]),
            const SizedBox(height: 15),
            Row(children: [
              Expanded(child: OutlinedButton(
                onPressed: () => _abrirDetalhes(evento),
                child: const Text('Ver detalhes'),
              )),
              const SizedBox(width: 10),
              Expanded(child: FilledButton.icon(
                onPressed: encerrado || ocupada ? null : () => _alternarParticipacao(evento),
                icon: ocupada
                    ? const SizedBox(width: 15, height: 15, child: CircularProgressIndicator(strokeWidth: 2, color: Colors.white))
                    : Icon(inscrito ? Icons.check_rounded : Icons.confirmation_number_outlined, size: 18),
                label: Text(encerrado ? 'Encerrado' : (inscrito ? 'Sair do evento' : 'Participar')),
              )),
            ]),
          ]),
        ),
      ]),
    );
  }

  @override
  Widget build(BuildContext context) => Scaffold(
    backgroundColor: const Color(0xFFF7F8FA),
    body: RefreshIndicator(
      onRefresh: _carregar,
      child: CustomScrollView(slivers: [
        SliverToBoxAdapter(child: Padding(
          padding: const EdgeInsets.fromLTRB(16, 20, 16, 14),
          child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
            Row(crossAxisAlignment: CrossAxisAlignment.center, children: [
              const Expanded(child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
                Text('Descubra eventos', style: TextStyle(fontSize: 26, fontWeight: FontWeight.w800,
                  color: escuro, letterSpacing: -0.8)),
                SizedBox(height: 3),
                Text('Encontros e experiências na sua região',
                  style: TextStyle(fontSize: 13, color: cinza)),
              ])),
              IconButton.filled(
                tooltip: 'Criar evento',
                onPressed: _criarEvento,
                icon: const Icon(Icons.add_rounded),
              ),
            ]),
            const SizedBox(height: 18),
            TextField(
              controller: _busca,
              onChanged: (_) => setState(() {}),
              decoration: const InputDecoration(
                hintText: 'Busque eventos, locais ou interesses',
                prefixIcon: Icon(Icons.search_rounded),
              ),
            ),
            const SizedBox(height: 13),
            if (_eventos.isNotEmpty)
              SizedBox(height: 43, child: ListView.separated(
                scrollDirection: Axis.horizontal,
                itemCount: _categorias.length,
                separatorBuilder: (_, __) => const SizedBox(width: 7),
                itemBuilder: (_, i) {
                  final categoria = _categorias[i];
                  return ChoiceChip(
                    label: Text(categoria),
                    selected: categoria == _categoria,
                    onSelected: (_) => setState(() => _categoria = categoria),
                  );
                },
              )),
            const SizedBox(height: 18),
            Row(children: [
              const Expanded(child: Text('Explore no mapa',
                style: TextStyle(fontWeight: FontWeight.w800, fontSize: 17, color: escuro))),
              TextButton.icon(
                onPressed: widget.onVerMapa,
                label: const Text('Abrir mapa'),
                icon: const Icon(Icons.arrow_forward_rounded, size: 17),
              ),
            ]),
            const SizedBox(height: 8),
            _mapaPreview(),
            const SizedBox(height: 22),
            Row(children: [
              const Expanded(child: Text('Próximos eventos',
                style: TextStyle(fontSize: 19, fontWeight: FontWeight.w800, color: escuro))),
              if (!_carregando && _erro == null)
                Text('${_filtrados.length} disponíveis', style: const TextStyle(fontSize: 12, color: cinza)),
            ]),
            const SizedBox(height: 12),
          ]),
        )),
        if (_carregando)
          const SliverToBoxAdapter(child: Padding(
            padding: EdgeInsets.symmetric(vertical: 42),
            child: Center(child: CircularProgressIndicator()),
          ))
        else if (_erro != null)
          SliverToBoxAdapter(child: Padding(
            padding: const EdgeInsets.all(24),
            child: Column(children: [
              const Icon(Icons.cloud_off_rounded, size: 34, color: cinza),
              const SizedBox(height: 10),
              Text(_erro!, textAlign: TextAlign.center),
              const SizedBox(height: 12),
              FilledButton(onPressed: _carregar, child: const Text('Tentar novamente')),
            ]),
          ))
        else if (_filtrados.isEmpty)
          const SliverToBoxAdapter(child: Padding(
            padding: EdgeInsets.all(32),
            child: Center(child: Text('Nenhum evento encontrado com esses filtros.')),
          ))
        else
          SliverPadding(
            padding: const EdgeInsets.fromLTRB(16, 0, 16, 80),
            sliver: SliverList.builder(
              itemCount: _filtrados.length,
              itemBuilder: (_, i) => _card(_filtrados[i]),
            ),
          ),
      ]),
    ),
  );
}


/// Seletor de localização da criação de eventos.
/// A mesma API Java usada pelo Web espera latitude e longitude no evento.
class _SeletorPontoMapa extends StatefulWidget {
  final LatLng? pontoInicial;
  const _SeletorPontoMapa({this.pontoInicial});

  @override
  State<_SeletorPontoMapa> createState() => _SeletorPontoMapaState();
}

class _SeletorPontoMapaState extends State<_SeletorPontoMapa> {
  static const Color rosa = Color(0xFFEA3F74);
  final MapController _controller = MapController();
  final TextEditingController _endereco = TextEditingController();
  LatLng? _ponto;
  bool _mapPronto = false;
  bool _buscando = false;
  String? _erro;

  @override
  void initState() {
    super.initState();
    _ponto = widget.pontoInicial;
  }

  @override
  void dispose() {
    _controller.dispose();
    _endereco.dispose();
    super.dispose();
  }

  void _moverPara(LatLng posicao) {
    setState(() { _ponto = posicao; _erro = null; });
    if (_mapPronto) _controller.move(posicao, 16);
  }

  Future<void> _usarGps() async {
    setState(() { _buscando = true; _erro = null; });
    try {
      final habilitado = await Geolocator.isLocationServiceEnabled();
      if (!habilitado) throw Exception('Ative a localização no dispositivo.');
      var permissao = await Geolocator.checkPermission();
      if (permissao == LocationPermission.denied) {
        permissao = await Geolocator.requestPermission();
      }
      if (permissao == LocationPermission.denied ||
          permissao == LocationPermission.deniedForever) {
        throw Exception('Permissão de localização negada. Você ainda pode selecionar tocando no mapa.');
      }
      final pos = await Geolocator.getCurrentPosition(
        locationSettings: const LocationSettings(
          accuracy: LocationAccuracy.high,
          timeLimit: Duration(seconds: 18),
        ),
      );
      if (mounted) _moverPara(LatLng(pos.latitude, pos.longitude));
    } catch (e) {
      if (mounted) setState(() => _erro = 'Não foi possível usar o GPS: $e');
    } finally {
      if (mounted) setState(() => _buscando = false);
    }
  }

  // Pesquisa manual, não automática: respeita limites do serviço público.
  Future<void> _buscarEndereco() async {
    final texto = _endereco.text.trim();
    if (texto.length < 4) {
      setState(() => _erro = 'Informe um endereço para buscar.');
      return;
    }
    setState(() { _buscando = true; _erro = null; });
    try {
      final uri = Uri.https('nominatim.openstreetmap.org', '/search', {
        'q': texto, 'format': 'json', 'limit': '1', 'countrycodes': 'br',
      });
      final resposta = await http.get(uri, headers: {
        'Accept': 'application/json',
        'Accept-Language': 'pt-BR',
      }).timeout(const Duration(seconds: 16));
      if (resposta.statusCode != 200) {
        throw Exception('Serviço de busca retornou HTTP ${resposta.statusCode}.');
      }
      final resultado = jsonDecode(utf8.decode(resposta.bodyBytes));
      if (resultado is! List || resultado.isEmpty) {
        throw Exception('Endereço não encontrado. Escolha diretamente no mapa.');
      }
      final item = resultado.first;
      if (item is! Map) throw const FormatException('Resposta de endereço inesperada.');
      final lat = double.tryParse('${item['lat']}');
      final lon = double.tryParse('${item['lon']}');
      if (lat == null || lon == null) throw const FormatException('Coordenadas ausentes.');
      if (mounted) _moverPara(LatLng(lat, lon));
    } catch (e) {
      if (mounted) setState(() => _erro = 'Não foi possível encontrar o endereço: $e');
    } finally {
      if (mounted) setState(() => _buscando = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Escolher local do evento')),
      body: Stack(children: [
        FlutterMap(
          mapController: _controller,
          options: MapOptions(
            initialCenter: _ponto ?? const LatLng(-22.5646, -47.4017),
            initialZoom: 14,
            onMapReady: () { _mapPronto = true; },
            onTap: (_, posicao) => _moverPara(posicao),
            onPositionChanged: (camera, hasGesture) {
              if (hasGesture && mounted) {
                setState(() => _ponto = camera.center);
              }
            },
          ),
          children: [
            TileLayer(
              urlTemplate: 'https://tile.openstreetmap.org/{z}/{x}/{y}.png',
              userAgentPackageName: 'socialjoin.mobile',
            ),
            const RichAttributionWidget(attributions: [
              TextSourceAttribution('OpenStreetMap contributors'),
            ]),
          ],
        ),
        // Como em um app de transporte: o marcador acompanha o centro do mapa.
        const Center(child: IgnorePointer(child: Padding(
          padding: EdgeInsets.only(bottom: 45),
          child: Icon(Icons.location_pin, color: rosa, size: 52),
        ))),
        Positioned(top: 12, left: 12, right: 12, child: SafeArea(
          bottom: false,
          child: Material(
            elevation: 4,
            color: Colors.white,
            borderRadius: BorderRadius.circular(15),
            child: Padding(
              padding: const EdgeInsets.symmetric(horizontal: 12),
              child: Row(children: [
                const Icon(Icons.search_rounded, color: rosa),
                const SizedBox(width: 8),
                Expanded(child: TextField(
                  controller: _endereco,
                  textInputAction: TextInputAction.search,
                  onSubmitted: (_) => _buscarEndereco(),
                  decoration: const InputDecoration(
                    hintText: 'Pesquisar endereço ou estabelecimento',
                    border: InputBorder.none,
                  ),
                )),
                IconButton(
                  onPressed: _buscando ? null : _buscarEndereco,
                  icon: const Icon(Icons.arrow_forward_rounded),
                ),
              ]),
            ),
          ),
        )),
        Positioned(right: 14, bottom: 185, child: FloatingActionButton.small(
          heroTag: 'gps-evento',
          backgroundColor: Colors.white,
          foregroundColor: rosa,
          onPressed: _buscando ? null : _usarGps,
          tooltip: 'Usar minha localização',
          child: const Icon(Icons.my_location_rounded),
        )),
        Positioned(left: 12, right: 12, bottom: 12,
          child: SafeArea(top: false, child: Card(
            elevation: 5,
            color: Colors.white,
            child: Padding(padding: const EdgeInsets.all(16),
              child: Column(mainAxisSize: MainAxisSize.min,
                crossAxisAlignment: CrossAxisAlignment.stretch, children: [
                const Text('Arraste o mapa ou toque no local exato.',
                  style: TextStyle(fontWeight: FontWeight.w700)),
                const SizedBox(height: 5),
                Text(_ponto == null ? 'Nenhum local marcado ainda.' :
                  '${_ponto!.latitude.toStringAsFixed(6)}, ${_ponto!.longitude.toStringAsFixed(6)}',
                  style: const TextStyle(fontSize: 12, color: Color(0xFF6B7280))),
                if (_erro != null) Padding(padding: const EdgeInsets.only(top: 6),
                  child: Text(_erro!, style: const TextStyle(
                    color: Color(0xFFC93659), fontSize: 12))),
                const SizedBox(height: 12),
                FilledButton.icon(
                  onPressed: _ponto == null || _buscando ? null
                    : () => Navigator.of(context).pop(_ponto),
                  icon: const Icon(Icons.check_circle_outline),
                  label: Text(_buscando ? 'Localizando...' : 'Confirmar localização'),
                ),
              ])),
          )),
        ),
      ]),
    );
  }
}
