import 'dart:async';
import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
import 'dart:ui' as ui;
import 'package:latlong2/latlong.dart';
import 'package:geolocator/geolocator.dart';
import '../models/evento.dart';
import '../services/evento_service.dart';
import '../services/auth_service.dart';
import '../services/location_service.dart';
import '../services/notification_service.dart';

class MapEventsPage extends StatefulWidget {
  final int? eventoInicialId;

  const MapEventsPage({
    super.key,
    this.eventoInicialId,
  });

  @override
  State<MapEventsPage> createState() => _MapEventsPageState();
}

class _MapEventsPageState extends State<MapEventsPage>
    with TickerProviderStateMixin {
  late final MapController _mapController;
  final EventoService _eventoService = EventoService();
  final LocationService _locationService = LocationService();
  final NotificationService _notificationService = NotificationService();

  List<Evento> _eventosReais = [];
  List<Evento> _eventosFiltrados = [];
  Map<int, LatLng> _coordenadasEventos = {};
  Map<int, int> _participantesContagem = {};
  Set<int> _eventosInscritos = {};

  int _selectedEventIndex = 0;
  String _selectedCategory = 'Todos';
  bool _carregando = true;
  String? _erro;

  // Localização
  Position? _posicaoUsuario;
  bool _permissaoConcedida = false;
  String? _statusPermissaoMensagem;
  StreamSubscription<Position>? _posicaoSubscription;

  // Animação dos marcadores
  late AnimationController _pulseController;
  late AnimationController _bounceController;
  late Animation<double> _pulseAnimation;
  late Animation<double> _bounceAnimation;

  final List<String> _categorias = const [
    'Todos',
    'Tecnologia',
    'Design',
    'Games',
    'Acadêmico',
    'Comunidade',
  ];

  final List<LatLng> _locaisReferencia = const [
    LatLng(-23.5615, -46.6560),
    LatLng(-23.5855, -46.6815),
    LatLng(-23.5874, -46.6576),
    LatLng(-23.5165, -46.6186),
    LatLng(-23.5489, -46.6388),
    LatLng(-23.5505, -46.6333),
    LatLng(-23.5700, -46.6400),
    LatLng(-23.6000, -46.6900),
    LatLng(-23.5550, -46.6620),
    LatLng(-23.5350, -46.6750),
  ];

  // Paleta de cores por categoria (estilo PoGO)
  static const Map<String, Color> _categoriaCores = {
    'Tecnologia': Color(0xFF3B82F6),
    'Design': Color(0xFFA855F7),
    'Games': Color(0xFFEF4444),
    'Acadêmico': Color(0xFFF59E0B),
    'Comunidade': Color(0xFF10B981),
    'Geral': Color(0xFFEA3F74),
  };

  static const Map<String, IconData> _categoriaIcones = {
    'Tecnologia': Icons.code_rounded,
    'Design': Icons.palette_rounded,
    'Games': Icons.sports_esports_rounded,
    'Acadêmico': Icons.school_rounded,
    'Comunidade': Icons.people_rounded,
    'Geral': Icons.star_rounded,
  };

  @override
  void initState() {
    super.initState();
    _mapController = MapController();

    // Animação de pulso contínuo (ping)
    _pulseController = AnimationController(
      vsync: this,
      duration: const Duration(milliseconds: 1500),
    )..repeat(reverse: false);
    _pulseAnimation = Tween<double>(begin: 0.0, end: 1.0).animate(
      CurvedAnimation(parent: _pulseController, curve: Curves.easeOut),
    );

    // Animação de bounce ao selecionar
    _bounceController = AnimationController(
      vsync: this,
      duration: const Duration(milliseconds: 400),
    );
    _bounceAnimation = TweenSequence<double>([
      TweenSequenceItem(tween: Tween(begin: 1.0, end: 1.3), weight: 30),
      TweenSequenceItem(tween: Tween(begin: 1.3, end: 0.9), weight: 30),
      TweenSequenceItem(tween: Tween(begin: 0.9, end: 1.05), weight: 20),
      TweenSequenceItem(tween: Tween(begin: 1.05, end: 1.0), weight: 20),
    ]).animate(CurvedAnimation(parent: _bounceController, curve: Curves.easeInOut));

    _notificationService.inicializar();

    // FIX: Aguarda o primeiro frame ser renderizado antes de carregar
    // eventos (evita chamar _mapController.move() antes do FlutterMap
    // estar montado, que causava o erro de async no runtime web).
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _carregarEventos();
      _inicializarLocalizacao();
    });
  }

  @override
  void dispose() {
    _posicaoSubscription?.cancel();
    _pulseController.dispose();
    _bounceController.dispose();
    super.dispose();
  }

  Future<void> _inicializarLocalizacao({bool forcarDialogo = false}) async {
    try {
      LocationPermission permissao = await _locationService.verificarPermissao();

      if (permissao == LocationPermission.denied && forcarDialogo) {
        final aceitou = await _exibirDialogoConsentimentoLocalizacao();
        if (aceitou == true) {
          permissao = await _locationService.solicitarPermissao();
        }
      } else if (permissao == LocationPermission.denied) {
        permissao = await _locationService.solicitarPermissao();
      }

      if (permissao == LocationPermission.always ||
          permissao == LocationPermission.whileInUse) {
        final pos = await _locationService.obterPosicaoAtual();
        if (mounted) {
          setState(() {
            _permissaoConcedida = true;
            _posicaoUsuario = pos;
            _statusPermissaoMensagem = null;
          });
          _iniciarEscutaPosicao();
          if (pos != null && widget.eventoInicialId == null) {
            _mapController.move(LatLng(pos.latitude, pos.longitude), 14.5);
          }
          _verificarProximidade();
        }
      } else {
        if (mounted) {
          setState(() {
            _permissaoConcedida = false;
            _posicaoUsuario = null;
            _statusPermissaoMensagem =
                'Ative sua localização para encontrar eventos próximos.';
          });
        }
      }
    } catch (e) {
      if (mounted) setState(() => _permissaoConcedida = false);
    }
  }

  Future<bool?> _exibirDialogoConsentimentoLocalizacao() {
    return showDialog<bool>(
      context: context,
      builder: (ctx) => AlertDialog(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
        title: const Row(
          children: [
            Icon(Icons.location_on_rounded, color: Color(0xFFEA3F74), size: 26),
            SizedBox(width: 10),
            Text('Sua Localização',
                style: TextStyle(fontWeight: FontWeight.bold, fontSize: 18)),
          ],
        ),
        content: const Text(
          'O SocialJoin precisa da sua permissão de localização para:\n\n'
          '• Mostrar sua posição atual no mapa\n'
          '• Calcular a distância até os eventos\n'
          '• Avisar quando houver eventos a menos de 500m\n\n'
          'Sua localização é processada com segurança.',
          style: TextStyle(fontSize: 14, color: Color(0xFF334155), height: 1.4),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(ctx, false),
            child: const Text('Agora não',
                style: TextStyle(color: Color(0xFF64748B))),
          ),
          ElevatedButton(
            style: ElevatedButton.styleFrom(
              backgroundColor: const Color(0xFFEA3F74),
              foregroundColor: Colors.white,
              shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12)),
            ),
            onPressed: () => Navigator.pop(ctx, true),
            child: const Text('Permitir Acesso'),
          ),
        ],
      ),
    );
  }

  void _iniciarEscutaPosicao() {
    _posicaoSubscription?.cancel();
    _posicaoSubscription =
        _locationService.ouvirPosicao(distanceFilter: 50).listen(
      (pos) {
        if (mounted) {
          setState(() => _posicaoUsuario = pos);
          _verificarProximidade();
        }
      },
      onError: (_) {},
    );
  }

  void _verificarProximidade() {
    if (_posicaoUsuario == null || _eventosReais.isEmpty) return;
    _locationService.verificarProximidadeEventos(
      posicaoUsuario: _posicaoUsuario!,
      eventos: _eventosReais,
      coordenadasEventos: _coordenadasEventos,
    );
  }

  Future<void> _carregarEventos() async {
    if (mounted) setState(() { _carregando = true; _erro = null; });

    try {
      final lista = await _eventoService.listarEventos();
      final Map<int, LatLng> mapaCoords = {};

      for (int i = 0; i < lista.length; i++) {
        mapaCoords[lista[i].id] = _locaisReferencia[i % _locaisReferencia.length];
      }

      if (!mounted) return;

      setState(() {
        _eventosReais = lista;
        _eventosFiltrados = List<Evento>.from(lista);
        _coordenadasEventos = mapaCoords;
        _selectedCategory = 'Todos';
        _selectedEventIndex = 0;
        _carregando = false;
        _erro = null;
      });

      // FIX: usa addPostFrameCallback para garantir que o FlutterMap
      // já está montado antes de chamar move() — evita o erro de
      // "controller used before map is ready" no Flutter Web.
      WidgetsBinding.instance.addPostFrameCallback((_) {
        if (!mounted) return;
        if (widget.eventoInicialId != null) {
          _focarEventoPorId(widget.eventoInicialId!);
        } else if (lista.isNotEmpty) {
          final coord = mapaCoords[lista.first.id] ?? _locaisReferencia.first;
          try { _mapController.move(coord, 13.5); } catch (_) {}
        }
      });

      _carregarDadosParticipacao(lista);
      _verificarProximidade();
    } catch (e, stack) {
      debugPrint('ERRO MAPA: $e\n$stack');
      if (!mounted) return;
      // FIX: Em vez de bloquear o mapa, mostra-o vazio com banner de aviso.
      // O usuário pode ver o mapa mesmo sem conexão com o backend.
      setState(() {
        _eventosReais = [];
        _eventosFiltrados = [];
        _coordenadasEventos = {};
        _carregando = false;
        _erro = null; // não bloqueia o mapa
        _statusPermissaoMensagem =
            'Sem conexão com o servidor. Verifique o backend.';
      });
    }
  }

  Future<void> _carregarDadosParticipacao(List<Evento> eventos) async {
    final Map<int, int> mapaParticipantes = {};
    final Set<int> inscritos = {};

    for (final evento in eventos) {
      try {
        mapaParticipantes[evento.id] =
            await _eventoService.contarParticipantes(evento.id);
      } catch (_) {
        mapaParticipantes[evento.id] = 0;
      }

      if (AuthService.logado && AuthService.idUsuario != null) {
        try {
          final ids = await _eventoService.listarIdsParticipantes(evento.id);
          if (ids.contains(AuthService.idUsuario)) inscritos.add(evento.id);
        } catch (_) {}
      }
    }

    if (!mounted) return;
    setState(() {
      _participantesContagem = mapaParticipantes;
      _eventosInscritos = inscritos;
    });
  }

  void _focarEventoPorId(int eventoId) {
    final idx = _eventosFiltrados.indexWhere((e) => e.id == eventoId);
    if (idx != -1) {
      _recenterToEvent(idx);
    } else {
      setState(() {
        _selectedCategory = 'Todos';
        _aplicarFiltroCategoria();
      });
      final idxNovo = _eventosFiltrados.indexWhere((e) => e.id == eventoId);
      if (idxNovo != -1) _recenterToEvent(idxNovo);
    }
  }

  String _detectarCategoria(Evento ev) {
    final text =
        '${ev.titulo} ${ev.descricao ?? ''} ${ev.localEvento}'.toLowerCase();
    if (text.contains('tech') ||
        text.contains('dev') ||
        text.contains('flutter') ||
        text.contains('código') ||
        text.contains('hackathon')) return 'Tecnologia';
    if (text.contains('design') ||
        text.contains('ui') ||
        text.contains('ux') ||
        text.contains('arte') ||
        text.contains('figma')) return 'Design';
    if (text.contains('game') ||
        text.contains('jogo') ||
        text.contains('esport') ||
        text.contains('play')) return 'Games';
    if (text.contains('acadêmico') ||
        text.contains('academico') ||
        text.contains('aula') ||
        text.contains('palestra') ||
        text.contains('curso') ||
        text.contains('workshop') ||
        text.contains('tcc')) return 'Acadêmico';
    return 'Comunidade';
  }

  void _aplicarFiltroCategoria() {
    setState(() {
      _eventosFiltrados = _selectedCategory == 'Todos'
          ? List.from(_eventosReais)
          : _eventosReais
              .where((ev) => _detectarCategoria(ev) == _selectedCategory)
              .toList();
      _selectedEventIndex = 0;
    });

    if (_eventosFiltrados.isNotEmpty) {
      final ev = _eventosFiltrados[0];
      final coord = _coordenadasEventos[ev.id] ?? _locaisReferencia[0];
      try { _mapController.move(coord, 14.0); } catch (_) {}
    }
  }

  void _recenterToEvent(int index) {
    if (index < 0 || index >= _eventosFiltrados.length) return;
    setState(() => _selectedEventIndex = index);
    _bounceController.forward(from: 0);
    final ev = _eventosFiltrados[index];
    final coord = _coordenadasEventos[ev.id] ?? _locaisReferencia[0];
    try { _mapController.move(coord, 14.8); } catch (_) {}
  }

  void _centralizarMinhaLocalizacao() async {
    if (_posicaoUsuario != null) {
      try {
        _mapController.move(
            LatLng(_posicaoUsuario!.latitude, _posicaoUsuario!.longitude), 15.5);
      } catch (_) {}
      _snack('Centralizado na sua posição atual.',
          cor: const Color(0xFF0F172A));
    } else {
      await _inicializarLocalizacao(forcarDialogo: true);
      if (_posicaoUsuario != null) {
        try {
          _mapController.move(
              LatLng(_posicaoUsuario!.latitude, _posicaoUsuario!.longitude), 15.5);
        } catch (_) {}
      }
    }
  }

  String _calcularDistanciaTexto(Evento ev) {
    final coord = _coordenadasEventos[ev.id];
    if (_posicaoUsuario != null && coord != null) {
      final distMetros = _locationService.calcularDistanciaMetros(
        lat1: _posicaoUsuario!.latitude,
        lon1: _posicaoUsuario!.longitude,
        lat2: coord.latitude,
        lon2: coord.longitude,
      );
      return distMetros < 1000
          ? '${distMetros.round()} m'
          : '${(distMetros / 1000).toStringAsFixed(1)} km';
    }
    return '1.5 km';
  }

  Future<void> _toggleParticipacao(Evento evento) async {
    if (!AuthService.logado || AuthService.idUsuario == null) {
      _snack('Faça login para participar de eventos.',
          cor: const Color(0xFFEA3F74));
      return;
    }

    final userId = AuthService.idUsuario!;
    final jaInscrito = _eventosInscritos.contains(evento.id);

    if (jaInscrito) {
      final ok = await _eventoService.sairEvento(evento.id, userId);
      if (ok) {
        setState(() {
          _eventosInscritos.remove(evento.id);
          final atual = _participantesContagem[evento.id] ?? 1;
          _participantesContagem[evento.id] = (atual > 0) ? atual - 1 : 0;
        });
        _snack('Presença cancelada.', cor: const Color(0xFF64748B));
      }
    } else {
      final ok = await _eventoService.participarEvento(evento.id, userId);
      if (ok) {
        setState(() {
          _eventosInscritos.add(evento.id);
          final atual = _participantesContagem[evento.id] ?? 0;
          _participantesContagem[evento.id] = atual + 1;
        });
        _snack('Presença confirmada em "${evento.titulo}"!',
            cor: const Color(0xFF10B981));
      }
    }
  }

  void _snack(String msg, {Color cor = const Color(0xFF10B981)}) {
    if (!mounted) return;
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
          content: Text(msg),
          backgroundColor: cor,
          duration: const Duration(seconds: 2)),
    );
  }

  // -------------------------------------------------------------------------
  // Constrói o marcador estilo Pokémon GO para cada evento
  // -------------------------------------------------------------------------
  Widget _buildEventMarker(int index, Evento event, bool isSelected) {
    final String categoria = _detectarCategoria(event);
    final Color cor =
        _categoriaCores[categoria] ?? const Color(0xFFEA3F74);
    final IconData icone =
        _categoriaIcones[categoria] ?? Icons.star_rounded;
    final bool isInscrito = _eventosInscritos.contains(event.id);

    return GestureDetector(
      onTap: () => _recenterToEvent(index),
      child: SizedBox(
        width: isSelected ? 160 : 68,
        height: isSelected ? 130 : 80,
        child: Stack(
          alignment: Alignment.topCenter,
          children: [
            // Label flutuante com o nome do evento (só no selecionado)
            if (isSelected)
              Positioned(
                top: 0,
                child: Container(
                  constraints: const BoxConstraints(maxWidth: 155),
                  padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 5),
                  decoration: BoxDecoration(
                    color: cor,
                    borderRadius: BorderRadius.circular(10),
                    boxShadow: [
                      BoxShadow(
                        color: cor.withValues(alpha: 0.5),
                        blurRadius: 8,
                        offset: const Offset(0, 2),
                      ),
                    ],
                  ),
                  child: Text(
                    event.titulo,
                    style: const TextStyle(
                      color: Colors.white,
                      fontSize: 10,
                      fontWeight: FontWeight.w700,
                      height: 1.2,
                    ),
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                    textAlign: TextAlign.center,
                  ),
                ),
              ),

            // Anel de pulso animado (só no selecionado)
            if (isSelected)
              AnimatedBuilder(
                animation: _pulseAnimation,
                builder: (_, __) {
                  final scale = 1.0 + _pulseAnimation.value * 0.8;
                  final opacity = 1.0 - _pulseAnimation.value;
                  return Positioned(
                    top: isSelected ? 30 : 0,
                    child: Transform.scale(
                      scale: scale,
                      child: Container(
                        width: 56,
                        height: 56,
                        decoration: BoxDecoration(
                          shape: BoxShape.circle,
                          border: Border.all(
                            color: cor.withValues(alpha: opacity * 0.7),
                            width: 3,
                          ),
                        ),
                      ),
                    ),
                  );
                },
              ),

            // Corpo principal do marcador
            Positioned(
              top: isSelected ? 30 : 0,
              child: isSelected
                  ? AnimatedBuilder(
                      animation: _bounceAnimation,
                      builder: (_, child) => Transform.scale(
                        scale: _bounceAnimation.value,
                        child: child,
                      ),
                      child: _buildMarkerBody(cor, icone, event, isSelected, isInscrito),
                    )
                  : _buildMarkerBody(cor, icone, event, isSelected, isInscrito),
            ),

            // Ponta triangular (seta para baixo)
            Positioned(
              top: isSelected ? 90 : 46,
              child: CustomPaint(
                size: const Size(16, 10),
                painter: _TrianglePainter(cor),
              ),
            ),

            // Sombra no chão
            Positioned(
              bottom: 4,
              child: Container(
                width: isSelected ? 28 : 20,
                height: isSelected ? 8 : 6,
                decoration: BoxDecoration(
                  borderRadius: BorderRadius.circular(50),
                  color: Colors.black.withValues(alpha: 0.25),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildMarkerBody(
      Color cor, IconData icone, Evento event, bool isSelected, bool isInscrito) {
    return Container(
      width: isSelected ? 56 : 44,
      height: isSelected ? 56 : 44,
      decoration: BoxDecoration(
        gradient: RadialGradient(
          colors: [
            cor.withValues(alpha: 0.9),
            cor,
          ],
          center: const Alignment(-0.3, -0.3),
        ),
        shape: BoxShape.circle,
        border: Border.all(
          color: Colors.white,
          width: isSelected ? 3.5 : 2.5,
        ),
        boxShadow: [
          BoxShadow(
            color: cor.withValues(alpha: isSelected ? 0.6 : 0.35),
            blurRadius: isSelected ? 18 : 8,
            spreadRadius: isSelected ? 3 : 1,
            offset: const Offset(0, 4),
          ),
          const BoxShadow(
            color: Colors.black26,
            blurRadius: 6,
            offset: Offset(0, 3),
          ),
        ],
      ),
      child: Stack(
        alignment: Alignment.center,
        children: [
          Icon(
            icone,
            color: Colors.white,
            size: isSelected ? 26 : 20,
          ),
          // Badge de inscrito (estrela verde)
          if (isInscrito)
            Positioned(
              right: 0,
              top: 0,
              child: Container(
                width: isSelected ? 18 : 14,
                height: isSelected ? 18 : 14,
                decoration: const BoxDecoration(
                  color: Color(0xFF10B981),
                  shape: BoxShape.circle,
                  boxShadow: [BoxShadow(color: Colors.black26, blurRadius: 3)],
                ),
                child: Icon(
                  Icons.check,
                  color: Colors.white,
                  size: isSelected ? 11 : 9,
                ),
              ),
            ),
        ],
      ),
    );
  }

  // -------------------------------------------------------------------------
  // Marcador do usuário estilo radar
  // -------------------------------------------------------------------------
  Widget _buildUserMarker() {
    return AnimatedBuilder(
      animation: _pulseAnimation,
      builder: (_, __) {
        final scale = 1.0 + _pulseAnimation.value * 1.2;
        final opacity = (1.0 - _pulseAnimation.value).clamp(0.0, 1.0);
        return SizedBox(
          width: 60,
          height: 60,
          child: Stack(
            alignment: Alignment.center,
            children: [
              // Anel de radar
              Transform.scale(
                scale: scale,
                child: Container(
                  width: 40,
                  height: 40,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    border: Border.all(
                      color: const Color(0xFF2563EB).withValues(alpha: opacity * 0.6),
                      width: 2,
                    ),
                  ),
                ),
              ),
              // Ponto central azul
              Container(
                width: 22,
                height: 22,
                decoration: BoxDecoration(
                  color: const Color(0xFF2563EB),
                  shape: BoxShape.circle,
                  border: Border.all(color: Colors.white, width: 3),
                  boxShadow: [
                    BoxShadow(
                      color: const Color(0xFF2563EB).withValues(alpha: 0.5),
                      blurRadius: 10,
                      spreadRadius: 2,
                    ),
                  ],
                ),
                child: const Icon(Icons.person, color: Colors.white, size: 12),
              ),
            ],
          ),
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    // FIX: o mapa renderiza imediatamente — sem tela de loading separada.
    // O spinner de eventos aparece como overlay no mapa enquanto carrega,
    // assim o usuário vê o mapa mesmo que a API demore ou falhe.

    final bool hasEvents = _eventosFiltrados.isNotEmpty;
    final Evento? selectedEvent =
        hasEvents && _selectedEventIndex < _eventosFiltrados.length
            ? _eventosFiltrados[_selectedEventIndex]
            : null;
    final bool isInscrito =
        selectedEvent != null && _eventosInscritos.contains(selectedEvent.id);
    final int participantes =
        selectedEvent != null ? (_participantesContagem[selectedEvent.id] ?? 0) : 0;
    final String categoriaSelected =
        selectedEvent != null ? _detectarCategoria(selectedEvent) : 'Geral';
    final String distanciaSelected =
        selectedEvent != null ? _calcularDistanciaTexto(selectedEvent) : '–';
    final Color corCategoria =
        _categoriaCores[categoriaSelected] ?? const Color(0xFFEA3F74);

    return Scaffold(
      backgroundColor: const Color(0xFF1A1A2E),
      body: Stack(
        children: [
          // ─── MAPA ───────────────────────────────────────────────────────
          FlutterMap(
            mapController: _mapController,
            options: MapOptions(
              initialCenter: _posicaoUsuario != null
                  ? LatLng(_posicaoUsuario!.latitude, _posicaoUsuario!.longitude)
                  : const LatLng(-23.5615, -46.6560),
              initialZoom: 13.5,
              minZoom: 3.0,
              maxZoom: 19.0,
            ),
            children: [
              // Tiles gratuitos CartoDB Voyager (sem necessidade de API key)
              TileLayer(
                urlTemplate:
                    'https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png',
                subdomains: const ['a', 'b', 'c', 'd'],
                maxZoom: 20,
                retinaMode: false,
              ),

              // Overlay escuro sobre os tiles para efeito dark mode
              // IgnorePointer garante que não intercepta toques do mapa
              ColoredBox(color: Color(0xCC1A1A2E)),

              // Marcadores (renderizados sobre o overlay, cores vibrantes)
              MarkerLayer(
                markers: [
                  // Marcador do usuário
                  if (_posicaoUsuario != null)
                    Marker(
                      width: 60,
                      height: 60,
                      point: LatLng(_posicaoUsuario!.latitude,
                          _posicaoUsuario!.longitude),
                      child: _buildUserMarker(),
                    ),

                  // Marcadores dos eventos (estilo PoGO)
                  ..._eventosFiltrados.asMap().entries.map((entry) {
                    final int index = entry.key;
                    final Evento event = entry.value;
                    final bool isSelected = index == _selectedEventIndex;
                    final LatLng coord =
                        _coordenadasEventos[event.id] ?? _locaisReferencia[0];

                    return Marker(
                      width: isSelected ? 160 : 68,
                      height: isSelected ? 130 : 80,
                      point: coord,
                      child: _buildEventMarker(index, event, isSelected),
                    );
                  }),
                ],
              ),
            ],
          ),

          // ─── LOADING OVERLAY (eventos carregando sobre o mapa) ──────────
          if (_carregando)
            Positioned(
              top: 70,
              left: 0,
              right: 0,
              child: Center(
                child: ClipRRect(
                  borderRadius: BorderRadius.circular(20),
                  child: BackdropFilter(
                    filter: ui.ImageFilter.blur(sigmaX: 12, sigmaY: 12),
                    child: Container(
                      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 10),
                      decoration: BoxDecoration(
                        color: const Color(0xFF1E1E2E).withValues(alpha: 0.7),
                        borderRadius: BorderRadius.circular(20),
                        border: Border.all(color: Colors.white.withValues(alpha: 0.15)),
                      ),
                      child: const Row(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          SizedBox(
                            width: 16, height: 16,
                            child: CircularProgressIndicator(
                              strokeWidth: 2.5,
                              color: Color(0xFFEA3F74),
                            ),
                          ),
                          SizedBox(width: 10),
                          Text('Carregando eventos...',
                              style: TextStyle(fontSize: 13, fontWeight: FontWeight.w600, color: Colors.white)),
                        ],
                      ),
                    ),
                  ),
                ),
              ),
            ),

          // ─── CHIPS DE CATEGORIA ─────────────────────────────────────────
          Positioned(
            top: 14,
            left: 0,
            right: 0,
            child: SingleChildScrollView(
              scrollDirection: Axis.horizontal,
              padding: const EdgeInsets.symmetric(horizontal: 16),
              child: Row(
                children: _categorias.map((cat) {
                  final isSelected = _selectedCategory == cat;
                  final corChip =
                      cat == 'Todos' ? const Color(0xFFEA3F74) : (_categoriaCores[cat] ?? const Color(0xFFEA3F74));
                  return Padding(
                    padding: const EdgeInsets.only(right: 8),
                    child: ClipRRect(
                      borderRadius: BorderRadius.circular(20),
                      child: BackdropFilter(
                        filter: ui.ImageFilter.blur(sigmaX: 8, sigmaY: 8),
                        child: AnimatedContainer(
                          duration: const Duration(milliseconds: 200),
                          child: FilterChip(
                            selected: isSelected,
                            label: Text(cat),
                            avatar: cat != 'Todos'
                                ? Icon(
                                    _categoriaIcones[cat],
                                    size: 14,
                                    color: isSelected ? Colors.white : corChip,
                                  )
                                : null,
                            labelStyle: TextStyle(
                              color: isSelected ? Colors.white : Colors.white.withValues(alpha: 0.9),
                              fontWeight:
                                  isSelected ? FontWeight.w700 : FontWeight.w500,
                              fontSize: 12,
                            ),
                            backgroundColor: const Color(0xFF1E1E2E).withValues(alpha: 0.6),
                            selectedColor: corChip,
                            elevation: 0,
                            shadowColor: Colors.transparent,
                            side: BorderSide(
                              color: isSelected ? corChip : Colors.white.withValues(alpha: 0.2),
                            ),
                            shape: RoundedRectangleBorder(
                                borderRadius: BorderRadius.circular(20)),
                            onSelected: (val) {
                              setState(() => _selectedCategory = cat);
                              _aplicarFiltroCategoria();
                            },
                          ),
                        ),
                      ),
                    ),
                  );
                }).toList(),
              ),
            ),
          ),

          // ─── BANNER LOCALIZAÇÃO DESATIVADA ──────────────────────────────
          if (_statusPermissaoMensagem != null && !_permissaoConcedida)
            Positioned(
              top: 70,
              left: 16,
              right: 16,
              child: ClipRRect(
                borderRadius: BorderRadius.circular(16),
                child: BackdropFilter(
                  filter: ui.ImageFilter.blur(sigmaX: 12, sigmaY: 12),
                  child: Container(
                    padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 10),
                    decoration: BoxDecoration(
                      color: const Color(0xFF1E1E2E).withValues(alpha: 0.7),
                      borderRadius: BorderRadius.circular(16),
                      border: Border.all(color: Colors.white.withValues(alpha: 0.15)),
                    ),
                    child: Row(
                      children: [
                        const Icon(Icons.location_off_rounded,
                            color: Color(0xFFEA3F74), size: 20),
                        const SizedBox(width: 10),
                        Expanded(
                          child: Text(
                            _statusPermissaoMensagem!,
                            style: TextStyle(
                                fontSize: 12,
                                color: Colors.white.withValues(alpha: 0.9),
                                fontWeight: FontWeight.w600),
                          ),
                        ),
                        TextButton(
                          onPressed: () =>
                              _inicializarLocalizacao(forcarDialogo: true),
                          style: TextButton.styleFrom(
                            foregroundColor: const Color(0xFFEA3F74),
                            padding: const EdgeInsets.symmetric(horizontal: 8),
                          ),
                          child: const Text('Ativar',
                              style: TextStyle(
                                  fontWeight: FontWeight.bold, fontSize: 12)),
                        ),
                      ],
                    ),
                  ),
                ),
              ),
            ),

          // ─── CONTROLES LATERAIS ─────────────────────────────────────────
          Positioned(
            right: 14,
            top: 130,
            child: Column(
              children: [
                _mapFab(
                  heroTag: 'loc_btn',
                  icon: Icons.my_location_rounded,
                  color: const Color(0xFF2563EB),
                  tooltip: 'Minha localização',
                  onTap: _centralizarMinhaLocalizacao,
                ),
                const SizedBox(height: 8),
                _mapFab(
                  heroTag: 'zoom_in_btn',
                  icon: Icons.add_rounded,
                  color: const Color(0xFF0F172A),
                  tooltip: 'Aproximar',
                  onTap: () {
                    final zoom = _mapController.camera.zoom + 1;
                    _mapController.move(_mapController.camera.center, zoom);
                  },
                ),
                const SizedBox(height: 8),
                _mapFab(
                  heroTag: 'zoom_out_btn',
                  icon: Icons.remove_rounded,
                  color: const Color(0xFF0F172A),
                  tooltip: 'Afastar',
                  onTap: () {
                    final zoom = _mapController.camera.zoom - 1;
                    _mapController.move(_mapController.camera.center, zoom);
                  },
                ),
                const SizedBox(height: 8),
                _mapFab(
                  heroTag: 'refresh_btn',
                  icon: Icons.refresh_rounded,
                  color: const Color(0xFF64748B),
                  tooltip: 'Recarregar',
                  onTap: _carregarEventos,
                ),
              ],
            ),
          ),

          // ─── CONTADOR DE EVENTOS ────────────────────────────────────────
          if (_eventosFiltrados.isNotEmpty)
            Positioned(
              left: 14,
              top: 130,
              child: ClipRRect(
                borderRadius: BorderRadius.circular(14),
                child: BackdropFilter(
                  filter: ui.ImageFilter.blur(sigmaX: 10, sigmaY: 10),
                  child: Container(
                    padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
                    decoration: BoxDecoration(
                      color: const Color(0xFF1E1E2E).withValues(alpha: 0.6),
                      borderRadius: BorderRadius.circular(14),
                      border: Border.all(color: Colors.white.withValues(alpha: 0.15)),
                    ),
                    child: Row(
                      mainAxisSize: MainAxisSize.min,
                      children: [
                        Container(
                          width: 8,
                          height: 8,
                          decoration: BoxDecoration(
                            color: corCategoria,
                            shape: BoxShape.circle,
                            boxShadow: [
                              BoxShadow(
                                color: corCategoria.withValues(alpha: 0.6),
                                blurRadius: 4,
                              ),
                            ],
                          ),
                        ),
                        const SizedBox(width: 6),
                        Text(
                          '${_eventosFiltrados.length} evento${_eventosFiltrados.length != 1 ? 's' : ''}',
                          style: const TextStyle(
                              color: Colors.white,
                              fontWeight: FontWeight.bold,
                              fontSize: 12),
                        ),
                      ],
                    ),
                  ),
                ),
              ),
            ),

          // ─── CARD EVENTO SELECIONADO ─────────────────────────────────────
          if (selectedEvent != null)
            Positioned(
              left: 14,
              right: 14,
              bottom: 16,
              child: _buildEventCard(
                selectedEvent,
                isInscrito,
                participantes,
                categoriaSelected,
                corCategoria,
                distanciaSelected,
              ),
            ),
        ],
      ),
    );
  }

  // ─── HELPER: FAB DO MAPA ─────────────────────────────────────────────────
  Widget _mapFab({
    required String heroTag,
    required IconData icon,
    required Color color,
    required String tooltip,
    required VoidCallback onTap,
  }) {
    return ClipRRect(
      borderRadius: BorderRadius.circular(14),
      child: BackdropFilter(
        filter: ui.ImageFilter.blur(sigmaX: 10, sigmaY: 10),
        child: Material(
          color: Colors.transparent,
          child: InkWell(
            onTap: onTap,
            borderRadius: BorderRadius.circular(14),
            child: Container(
              width: 42,
              height: 42,
              decoration: BoxDecoration(
                color: const Color(0xFF1E1E2E).withValues(alpha: 0.6),
                borderRadius: BorderRadius.circular(14),
                border: Border.all(color: Colors.white.withValues(alpha: 0.15)),
              ),
              child: Icon(icon, color: color == const Color(0xFF0F172A) ? Colors.white.withValues(alpha: 0.9) : color, size: 20),
            ),
          ),
        ),
      ),
    );
  }

  // ─── HELPER: CARD DO EVENTO ───────────────────────────────────────────────
  Widget _buildEventCard(
    Evento evento,
    bool isInscrito,
    int participantes,
    String categoria,
    Color corCategoria,
    String distancia,
  ) {
    return ClipRRect(
      borderRadius: BorderRadius.circular(24),
      child: BackdropFilter(
        filter: ui.ImageFilter.blur(sigmaX: 16, sigmaY: 16),
        child: Container(
          decoration: BoxDecoration(
            color: const Color(0xFF1E1E2E).withValues(alpha: 0.75),
            borderRadius: BorderRadius.circular(24),
            border: Border.all(color: Colors.white.withValues(alpha: 0.12)),
            boxShadow: [
              BoxShadow(
                color: corCategoria.withValues(alpha: 0.2),
                blurRadius: 24,
                offset: const Offset(0, 8),
              ),
            ],
          ),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              // Barra colorida gradiente no topo do card
              Container(
                height: 3,
                decoration: BoxDecoration(
                  gradient: LinearGradient(
                    colors: [corCategoria, corCategoria.withValues(alpha: 0.3), Colors.transparent],
                  ),
                  borderRadius: const BorderRadius.vertical(top: Radius.circular(24)),
                ),
              ),
              Padding(
                padding: const EdgeInsets.fromLTRB(18, 14, 18, 16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    // Header: Categoria + Público/Privado + Distância + Navegação
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Flexible(
                          child: Row(
                            mainAxisSize: MainAxisSize.min,
                            children: [
                              Container(
                                padding: const EdgeInsets.symmetric(
                                    horizontal: 10, vertical: 4),
                                decoration: BoxDecoration(
                                  color: corCategoria.withValues(alpha: 0.2),
                                  borderRadius: BorderRadius.circular(12),
                                  border: Border.all(color: corCategoria.withValues(alpha: 0.4)),
                                ),
                                child: Row(
                                  mainAxisSize: MainAxisSize.min,
                                  children: [
                                    Icon(
                                      _categoriaIcones[categoria] ?? Icons.star_rounded,
                                      size: 12,
                                      color: corCategoria,
                                    ),
                                    const SizedBox(width: 4),
                                    Text(
                                      categoria,
                                      style: TextStyle(
                                        color: corCategoria,
                                        fontSize: 11,
                                        fontWeight: FontWeight.bold,
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                              const SizedBox(width: 6),
                              Container(
                                padding: const EdgeInsets.symmetric(
                                    horizontal: 8, vertical: 4),
                                decoration: BoxDecoration(
                                  color: evento.ehPublico
                                      ? const Color(0xFF10B981).withValues(alpha: 0.15)
                                      : Colors.white.withValues(alpha: 0.1),
                                  borderRadius: BorderRadius.circular(12),
                                ),
                                child: Text(
                                  evento.ehPublico ? 'Público' : 'Comunidade',
                                  style: TextStyle(
                                    color: evento.ehPublico
                                        ? const Color(0xFF34D399)
                                        : Colors.white.withValues(alpha: 0.7),
                                    fontSize: 10,
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                              ),
                            ],
                          ),
                        ),
                        Row(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            Icon(Icons.near_me_rounded,
                                size: 13, color: corCategoria),
                            const SizedBox(width: 3),
                            Text(
                              distancia,
                              style: TextStyle(
                                color: corCategoria,
                                fontWeight: FontWeight.bold,
                                fontSize: 12,
                              ),
                            ),
                            const SizedBox(width: 10),
                            GestureDetector(
                              onTap: _selectedEventIndex > 0
                                  ? () => _recenterToEvent(_selectedEventIndex - 1)
                                  : null,
                              child: Icon(Icons.chevron_left_rounded,
                                  color: _selectedEventIndex > 0
                                      ? Colors.white.withValues(alpha: 0.9)
                                      : Colors.white.withValues(alpha: 0.2),
                                  size: 22),
                            ),
                            Text(
                              '${_selectedEventIndex + 1}/${_eventosFiltrados.length}',
                              style: TextStyle(
                                  color: Colors.white.withValues(alpha: 0.6),
                                  fontSize: 11,
                                  fontWeight: FontWeight.bold),
                            ),
                            GestureDetector(
                              onTap: _selectedEventIndex <
                                      _eventosFiltrados.length - 1
                                  ? () => _recenterToEvent(_selectedEventIndex + 1)
                                  : null,
                              child: Icon(Icons.chevron_right_rounded,
                                  color: _selectedEventIndex <
                                          _eventosFiltrados.length - 1
                                      ? Colors.white.withValues(alpha: 0.9)
                                      : Colors.white.withValues(alpha: 0.2),
                                  size: 22),
                            ),
                          ],
                        ),
                      ],
                    ),
                    const SizedBox(height: 10),

                    // Título
                    Text(
                      evento.titulo,
                      style: const TextStyle(
                        fontSize: 17,
                        fontWeight: FontWeight.bold,
                        color: Colors.white,
                      ),
                      maxLines: 1,
                      overflow: TextOverflow.ellipsis,
                    ),
                    const SizedBox(height: 6),

                    // Local
                    Row(
                      children: [
                        Icon(Icons.location_on_rounded,
                            size: 15, color: corCategoria),
                        const SizedBox(width: 5),
                        Expanded(
                          child: Text(
                            evento.localEvento,
                            style: TextStyle(
                                color: Colors.white.withValues(alpha: 0.7),
                                fontSize: 13,
                                fontWeight: FontWeight.w500),
                            overflow: TextOverflow.ellipsis,
                          ),
                        ),
                      ],
                    ),
                    const SizedBox(height: 5),

                    // Data e participantes
                    Row(
                      children: [
                        Icon(Icons.access_time_rounded,
                            size: 13, color: Colors.white.withValues(alpha: 0.4)),
                        const SizedBox(width: 4),
                        Text(
                          '${evento.dataFormatada} • ${evento.horarioFormatado}',
                          style: TextStyle(
                              color: Colors.white.withValues(alpha: 0.6), fontSize: 12),
                        ),
                        const SizedBox(width: 12),
                        Icon(Icons.people_outline_rounded,
                            size: 13, color: Colors.white.withValues(alpha: 0.4)),
                        const SizedBox(width: 4),
                        Text(
                          '$participantes inscritos',
                          style: TextStyle(
                              color: Colors.white.withValues(alpha: 0.6), fontSize: 12),
                        ),
                      ],
                    ),
                    const SizedBox(height: 14),

                    // Ações
                    Row(
                      children: [
                        Expanded(
                          child: OutlinedButton.icon(
                            onPressed: () => _snack(
                                'Traçando rota para ${evento.localEvento}...',
                                cor: const Color(0xFF0F172A)),
                            icon: const Icon(Icons.directions_rounded, size: 16),
                            label: const Text('Como Chegar'),
                            style: OutlinedButton.styleFrom(
                              foregroundColor: Colors.white.withValues(alpha: 0.9),
                              side: BorderSide(color: Colors.white.withValues(alpha: 0.2)),
                              shape: RoundedRectangleBorder(
                                  borderRadius: BorderRadius.circular(12)),
                              padding: const EdgeInsets.symmetric(vertical: 10),
                              textStyle: const TextStyle(
                                  fontSize: 12, fontWeight: FontWeight.w600),
                            ),
                          ),
                        ),
                        const SizedBox(width: 10),
                        Expanded(
                          child: isInscrito
                              ? OutlinedButton.icon(
                                  onPressed: () => _toggleParticipacao(evento),
                                  icon: const Icon(Icons.check_circle_rounded,
                                      size: 16, color: Color(0xFF34D399)),
                                  label: const Text('Inscrito (Sair)'),
                                  style: OutlinedButton.styleFrom(
                                    foregroundColor: const Color(0xFF34D399),
                                    side: const BorderSide(
                                        color: Color(0xFF34D399)),
                                    shape: RoundedRectangleBorder(
                                        borderRadius: BorderRadius.circular(12)),
                                    padding:
                                        const EdgeInsets.symmetric(vertical: 10),
                                    textStyle: const TextStyle(
                                        fontSize: 12,
                                        fontWeight: FontWeight.w600),
                                  ),
                                )
                              : ElevatedButton.icon(
                                  onPressed: () => _toggleParticipacao(evento),
                                  icon: const Icon(
                                      Icons.event_available_rounded,
                                      size: 16),
                                  label: const Text('Participar'),
                                  style: ElevatedButton.styleFrom(
                                    backgroundColor: corCategoria,
                                    foregroundColor: Colors.white,
                                    elevation: 0,
                                    shape: RoundedRectangleBorder(
                                        borderRadius: BorderRadius.circular(12)),
                                    padding:
                                        const EdgeInsets.symmetric(vertical: 10),
                                    textStyle: const TextStyle(
                                        fontSize: 12,
                                        fontWeight: FontWeight.w600),
                                  ),
                                ),
                        ),
                      ],
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

// ─── PAINTER: TRIÂNGULO DA PONTA DO MARCADOR ─────────────────────────────────
class _TrianglePainter extends CustomPainter {
  final Color color;
  const _TrianglePainter(this.color);

  @override
  void paint(Canvas canvas, Size size) {
    final paint = Paint()
      ..color = color
      ..style = PaintingStyle.fill;

    final path = ui.Path();
    path.moveTo(0, 0);
    path.lineTo(size.width, 0);
    path.lineTo(size.width / 2, size.height);
    path.close();

    canvas.drawPath(path, paint);
  }

  @override
  bool shouldRepaint(_TrianglePainter old) => old.color != color;
}
