import 'dart:async';
import 'package:flutter/material.dart';
import 'package:flutter_map/flutter_map.dart';
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

class _MapEventsPageState extends State<MapEventsPage> {
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

  // Estado de Localização do Usuário
  Position? _posicaoUsuario;
  bool _permissaoConcedida = false;
  String? _statusPermissaoMensagem;
  StreamSubscription<Position>? _posicaoSubscription;

  final List<String> _categorias = const [
    'Todos',
    'Tecnologia',
    'Design',
    'Games',
    'Acadêmico',
    'Comunidade',
  ];

  // Coordenadas geográficas de referência para distribuição dos eventos
  final List<LatLng> _locaisReferencia = const [
    LatLng(-23.5615, -46.6560), // Av. Paulista
    LatLng(-23.5855, -46.6815), // Faria Lima
    LatLng(-23.5874, -46.6576), // Parque Ibirapuera
    LatLng(-23.5165, -46.6186), // Expo Center Norte
    LatLng(-23.5489, -46.6388), // Centro Histórico
    LatLng(-23.5505, -46.6333), // Praça da Sé
    LatLng(-23.5700, -46.6400), // Paraíso
    LatLng(-23.6000, -46.6900), // Eng. Luís Carlos Berrini
    LatLng(-23.5550, -46.6620), // Jardins
    LatLng(-23.5350, -46.6750), // Barra Funda / Perdizes
  ];

  @override
  void initState() {
    super.initState();
    _mapController = MapController();
    _notificationService.inicializar();
    _carregarEventos();
    _inicializarLocalizacao();
  }

  @override
  void dispose() {
    _posicaoSubscription?.cancel();
    super.dispose();
  }

  /// Inicializa a verificação de localização de forma transparente e consentida
  Future<void> _inicializarLocalizacao({bool forcarDialogo = false}) async {
    try {
      LocationPermission permissao = await _locationService.verificarPermissao();

      if (permissao == LocationPermission.denied && forcarDialogo) {
        // Exibe diálogo explicativo antes da solicitação nativa
        final aceitou = await _exibirDialogoConsentimentoLocalizacao();
        if (aceitou == true) {
          permissao = await _locationService.solicitarPermissao();
        }
      } else if (permissao == LocationPermission.denied) {
        permissao = await _locationService.solicitarPermissao();
      }

      if (permissao == LocationPermission.always || permissao == LocationPermission.whileInUse) {
        final pos = await _locationService.obterPosicaoAtual();
        if (mounted) {
          setState(() {
            _permissaoConcedida = true;
            _posicaoUsuario = pos;
            _statusPermissaoMensagem = null;
          });

          // Inicia escuta controlada para economia de bateria
          _iniciarEscutaPosicao();

          // Se tiver posição e nenhum evento específico requisitado, recentraliza
          if (pos != null && widget.eventoInicialId == null) {
            _mapController.move(LatLng(pos.latitude, pos.longitude), 14.5);
          }

          // Verifica proximidade de eventos públicos
          _verificarProximidade();
        }
      } else {
        if (mounted) {
          setState(() {
            _permissaoConcedida = false;
            _posicaoUsuario = null;
            _statusPermissaoMensagem = 'Ative sua localização para encontrar eventos próximos de você.';
          });
        }
      }
    } catch (e) {
      if (mounted) {
        setState(() {
          _permissaoConcedida = false;
        });
      }
    }
  }

  /// Diálogo amigável de transparência sobre o uso da localização
  Future<bool?> _exibirDialogoConsentimentoLocalizacao() {
    return showDialog<bool>(
      context: context,
      builder: (ctx) => AlertDialog(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
        title: const Row(
          children: [
            Icon(Icons.location_on_rounded, color: Color(0xFFEA3F74), size: 26),
            SizedBox(width: 10),
            Text('Sua Localização', style: TextStyle(fontWeight: FontWeight.bold, fontSize: 18)),
          ],
        ),
        content: const Text(
          'O SocialJoin precisa da sua permissão de localização para:\n\n'
          '• Mostrar sua posição atual no mapa ("Estou aqui")\n'
          '• Calcular a distância até os eventos\n'
          '• Avisar quando houver eventos públicos a menos de 500m\n\n'
          'Sua localização é processada com segurança no próprio dispositivo.',
          style: TextStyle(fontSize: 14, color: Color(0xFF334155), height: 1.4),
        ),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(ctx, false),
            child: const Text('Agora não', style: TextStyle(color: Color(0xFF64748B))),
          ),
          ElevatedButton(
            style: ElevatedButton.styleFrom(
              backgroundColor: const Color(0xFFEA3F74),
              foregroundColor: Colors.white,
              shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
            ),
            onPressed: () => Navigator.pop(ctx, true),
            child: const Text('Permitir Acesso'),
          ),
        ],
      ),
    );
  }

  /// Escuta atualizações de localização com filtro de 50m (economia de bateria)
  void _iniciarEscutaPosicao() {
    _posicaoSubscription?.cancel();
    _posicaoSubscription = _locationService.ouvirPosicao(distanceFilter: 50).listen(
      (pos) {
        if (mounted) {
          setState(() {
            _posicaoUsuario = pos;
          });
          _verificarProximidade();
        }
      },
      onError: (_) {},
    );
  }

  /// Executa o cálculo de proximidade estritamente para eventos PÚBLICOS
  void _verificarProximidade() {
    if (_posicaoUsuario == null || _eventosReais.isEmpty) return;

    _locationService.verificarProximidadeEventos(
      posicaoUsuario: _posicaoUsuario!,
      eventos: _eventosReais,
      coordenadasEventos: _coordenadasEventos,
    );
  }

  Future<void> _carregarEventos() async {
    setState(() {
      _carregando = true;
      _erro = null;
    });

    try {
      final lista = await _eventoService.listarEventos();
      final Map<int, LatLng> mapaCoords = {};
      final Map<int, int> mapaParticipantes = {};
      final Set<int> inscritos = {};

      for (int i = 0; i < lista.length; i++) {
        final ev = lista[i];
        final coord = _locaisReferencia[i % _locaisReferencia.length];
        mapaCoords[ev.id] = coord;

        final total = await _eventoService.contarParticipantes(ev.id);
        mapaParticipantes[ev.id] = total;

        if (AuthService.logado && AuthService.idUsuario != null) {
          final ids = await _eventoService.listarIdsParticipantes(ev.id);
          if (ids.contains(AuthService.idUsuario)) {
            inscritos.add(ev.id);
          }
        }
      }

      if (mounted) {
        setState(() {
          _eventosReais = lista;
          _coordenadasEventos = mapaCoords;
          _participantesContagem = mapaParticipantes;
          _eventosInscritos = inscritos;
          _carregando = false;
          _aplicarFiltroCategoria();
        });

        // Se foi solicitado focar em um evento específico (ex: via notificação)
        if (widget.eventoInicialId != null) {
          _focarEventoPorId(widget.eventoInicialId!);
        } else if (_posicaoUsuario != null) {
          _mapController.move(LatLng(_posicaoUsuario!.latitude, _posicaoUsuario!.longitude), 14.5);
        } else if (_eventosFiltrados.isNotEmpty) {
          final firstCoord = mapaCoords[_eventosFiltrados[0].id] ?? _locaisReferencia[0];
          _mapController.move(firstCoord, 13.5);
        }

        _verificarProximidade();
      }
    } catch (e) {
      if (mounted) {
        setState(() {
          _erro = 'Não foi possível carregar os eventos no mapa.';
          _carregando = false;
        });
      }
    }
  }

  void _focarEventoPorId(int eventoId) {
    final idx = _eventosFiltrados.indexWhere((e) => e.id == eventoId);
    if (idx != -1) {
      _recenterToEvent(idx);
    } else {
      // Se estiver em outra categoria, reseta para "Todos"
      setState(() {
        _selectedCategory = 'Todos';
        _aplicarFiltroCategoria();
      });
      final idxNovo = _eventosFiltrados.indexWhere((e) => e.id == eventoId);
      if (idxNovo != -1) {
        _recenterToEvent(idxNovo);
      }
    }
  }

  String _detectarCategoria(Evento ev) {
    final text = '${ev.titulo} ${ev.descricao ?? ''} ${ev.localEvento}'.toLowerCase();
    if (text.contains('tech') || text.contains('dev') || text.contains('flutter') || text.contains('código') || text.contains('hackathon')) {
      return 'Tecnologia';
    }
    if (text.contains('design') || text.contains('ui') || text.contains('ux') || text.contains('arte') || text.contains('figma')) {
      return 'Design';
    }
    if (text.contains('game') || text.contains('jogo') || text.contains('esport') || text.contains('play')) {
      return 'Games';
    }
    if (text.contains('acadêmico') || text.contains('academico') || text.contains('aula') || text.contains('palestra') || text.contains('curso') || text.contains('workshop') || text.contains('tcc')) {
      return 'Acadêmico';
    }
    return 'Comunidade';
  }

  void _aplicarFiltroCategoria() {
    setState(() {
      if (_selectedCategory == 'Todos') {
        _eventosFiltrados = List.from(_eventosReais);
      } else {
        _eventosFiltrados = _eventosReais.where((ev) {
          return _detectarCategoria(ev) == _selectedCategory;
        }).toList();
      }
      _selectedEventIndex = 0;
    });

    if (_eventosFiltrados.isNotEmpty) {
      final ev = _eventosFiltrados[0];
      final coord = _coordenadasEventos[ev.id] ?? _locaisReferencia[0];
      _mapController.move(coord, 14.0);
    }
  }

  void _recenterToEvent(int index) {
    if (index < 0 || index >= _eventosFiltrados.length) return;
    setState(() {
      _selectedEventIndex = index;
    });
    final ev = _eventosFiltrados[index];
    final coord = _coordenadasEventos[ev.id] ?? _locaisReferencia[0];
    _mapController.move(coord, 14.8);
  }

  /// Centraliza a visualização na localização atual do usuário
  void _centralizarMinhaLocalizacao() async {
    if (_posicaoUsuario != null) {
      _mapController.move(LatLng(_posicaoUsuario!.latitude, _posicaoUsuario!.longitude), 15.5);
      _snack('Centralizado na sua posição atual.', cor: const Color(0xFF0F172A));
    } else {
      await _inicializarLocalizacao(forcarDialogo: true);
      if (_posicaoUsuario != null) {
        _mapController.move(LatLng(_posicaoUsuario!.latitude, _posicaoUsuario!.longitude), 15.5);
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
      if (distMetros < 1000) {
        return '${distMetros.round()} m';
      }
      return '${(distMetros / 1000).toStringAsFixed(1)} km';
    }
    return '1.5 km';
  }

  Future<void> _toggleParticipacao(Evento evento) async {
    if (!AuthService.logado || AuthService.idUsuario == null) {
      _snack('Faça login para participar de eventos.', cor: const Color(0xFFEA3F74));
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
        _snack('Presença confirmada em "${evento.titulo}"!', cor: const Color(0xFF10B981));
      }
    }
  }

  void _snack(String msg, {Color cor = const Color(0xFF10B981)}) {
    if (!mounted) return;
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(content: Text(msg), backgroundColor: cor, duration: const Duration(seconds: 2)),
    );
  }

  @override
  Widget build(BuildContext context) {
    if (_carregando) {
      return const Scaffold(
        backgroundColor: Color(0xFFF8FAFC),
        body: Center(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              CircularProgressIndicator(color: Color(0xFFEA3F74)),
              SizedBox(height: 16),
              Text('Carregando mapa interativo...',
                  style: TextStyle(color: Color(0xFF64748B), fontSize: 14)),
            ],
          ),
        ),
      );
    }

    if (_erro != null) {
      return Scaffold(
        backgroundColor: const Color(0xFFF8FAFC),
        body: Center(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const Icon(Icons.location_off_rounded, size: 52, color: Color(0xFFCBD5E1)),
              const SizedBox(height: 12),
              Text(_erro!, style: const TextStyle(color: Color(0xFF64748B), fontSize: 14)),
              const SizedBox(height: 16),
              ElevatedButton.icon(
                onPressed: _carregarEventos,
                icon: const Icon(Icons.refresh_rounded, size: 18),
                label: const Text('Tentar novamente'),
                style: ElevatedButton.styleFrom(
                  backgroundColor: const Color(0xFFEA3F74),
                  foregroundColor: Colors.white,
                  shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                ),
              ),
            ],
          ),
        ),
      );
    }

    final bool hasEvents = _eventosFiltrados.isNotEmpty;
    final Evento? selectedEvent = hasEvents && _selectedEventIndex < _eventosFiltrados.length
        ? _eventosFiltrados[_selectedEventIndex]
        : null;
    final bool isInscrito = selectedEvent != null && _eventosInscritos.contains(selectedEvent.id);
    final int participantes = selectedEvent != null ? (_participantesContagem[selectedEvent.id] ?? 0) : 0;
    final String categoriaSelected = selectedEvent != null ? _detectarCategoria(selectedEvent) : 'Geral';
    final String distanciaSelected = selectedEvent != null ? _calcularDistanciaTexto(selectedEvent) : '1.5 km';

    return Scaffold(
      backgroundColor: const Color(0xFFF8FAFC),
      body: Stack(
        children: [
          // MAPA INTERATIVO COM CAMADAS (OpenStreetMap)
          FlutterMap(
            mapController: _mapController,
            options: MapOptions(
              initialCenter: _posicaoUsuario != null
                  ? LatLng(_posicaoUsuario!.latitude, _posicaoUsuario!.longitude)
                  : const LatLng(-23.5615, -46.6560),
              initialZoom: 13.5,
              minZoom: 3.0,
              maxZoom: 18.0,
            ),
            children: [
              TileLayer(
                urlTemplate: 'https://tile.openstreetmap.org/{z}/{x}/{y}.png',
                userAgentPackageName: 'com.socialjoin.app',
              ),

              // CAMADA DE MARCADORES (Eventos + Marcador Único do Usuário "Estou aqui")
              MarkerLayer(
                markers: [
                  // 1. MARCADOR VISUAL DESTACADO DO USUÁRIO ("Estou aqui")
                  if (_posicaoUsuario != null)
                    Marker(
                      width: 70,
                      height: 70,
                      point: LatLng(_posicaoUsuario!.latitude, _posicaoUsuario!.longitude),
                      child: Tooltip(
                        message: 'Você está aqui',
                        child: Column(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            Container(
                              padding: const EdgeInsets.symmetric(horizontal: 6, vertical: 2),
                              decoration: BoxDecoration(
                                color: const Color(0xFF2563EB),
                                borderRadius: BorderRadius.circular(10),
                                boxShadow: const [
                                  BoxShadow(color: Colors.black26, blurRadius: 4, offset: Offset(0, 2)),
                                ],
                              ),
                              child: const Text(
                                'Estou aqui',
                                style: TextStyle(
                                  color: Colors.white,
                                  fontSize: 9,
                                  fontWeight: FontWeight.w800,
                                ),
                              ),
                            ),
                            const SizedBox(height: 2),
                            Container(
                              width: 26,
                              height: 26,
                              decoration: BoxDecoration(
                                color: const Color(0xFF2563EB),
                                shape: BoxShape.circle,
                                border: Border.all(color: Colors.white, width: 3),
                                boxShadow: [
                                  BoxShadow(
                                    color: const Color(0xFF2563EB).withValues(alpha: 0.45),
                                    blurRadius: 10,
                                    spreadRadius: 3,
                                  ),
                                ],
                              ),
                              child: const Center(
                                child: Icon(Icons.person, color: Colors.white, size: 14),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),

                  // 2. MARCADORES DE EVENTOS (Estilo Pin Rosa/Escuro)
                  ..._eventosFiltrados.asMap().entries.map((entry) {
                    final int index = entry.key;
                    final Evento event = entry.value;
                    final bool isSelected = index == _selectedEventIndex;
                    final LatLng coord = _coordenadasEventos[event.id] ?? _locaisReferencia[0];

                    return Marker(
                      width: isSelected ? 84 : 56,
                      height: isSelected ? 86 : 62,
                      point: coord,
                      child: GestureDetector(
                        onTap: () => _recenterToEvent(index),
                        child: Column(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            Container(
                              padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                              decoration: BoxDecoration(
                                color: isSelected ? const Color(0xFFEA3F74) : const Color(0xFF0F172A),
                                borderRadius: BorderRadius.circular(14),
                                boxShadow: [
                                  BoxShadow(
                                    color: isSelected
                                        ? const Color(0xFFEA3F74).withValues(alpha: 0.45)
                                        : Colors.black26,
                                    blurRadius: isSelected ? 12 : 4,
                                    offset: const Offset(0, 3),
                                  ),
                                ],
                              ),
                              child: Row(
                                mainAxisSize: MainAxisSize.min,
                                children: [
                                  Icon(
                                    event.ehPublico ? Icons.event_rounded : Icons.lock_outline_rounded,
                                    color: Colors.white,
                                    size: 12,
                                  ),
                                  const SizedBox(width: 4),
                                  Text(
                                    event.titulo.length > 10
                                        ? '${event.titulo.substring(0, 8)}...'
                                        : event.titulo,
                                    style: TextStyle(
                                      color: Colors.white,
                                      fontSize: isSelected ? 11 : 10,
                                      fontWeight: FontWeight.bold,
                                    ),
                                  ),
                                ],
                              ),
                            ),
                            Icon(
                              Icons.arrow_drop_down,
                              color: isSelected ? const Color(0xFFEA3F74) : const Color(0xFF0F172A),
                              size: isSelected ? 26 : 18,
                            ),
                          ],
                        ),
                      ),
                    );
                  }),
                ],
              ),
            ],
          ),

          // Categorias Chips no topo
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
                  return Padding(
                    padding: const EdgeInsets.only(right: 8),
                    child: FilterChip(
                      selected: isSelected,
                      label: Text(cat),
                      labelStyle: TextStyle(
                        color: isSelected ? Colors.white : const Color(0xFF334155),
                        fontWeight: isSelected ? FontWeight.w700 : FontWeight.w500,
                        fontSize: 12,
                      ),
                      backgroundColor: Colors.white,
                      selectedColor: const Color(0xFFEA3F74),
                      elevation: 3,
                      shadowColor: Colors.black12,
                      side: BorderSide(
                        color: isSelected ? const Color(0xFFEA3F74) : const Color(0xFFE2E8F0),
                      ),
                      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
                      onSelected: (val) {
                        setState(() {
                          _selectedCategory = cat;
                          _aplicarFiltroCategoria();
                        });
                      },
                    ),
                  );
                }).toList(),
              ),
            ),
          ),

          // Banner não intrusivo se localização estiver desativada
          if (_statusPermissaoMensagem != null && !_permissaoConcedida)
            Positioned(
              top: 70,
              left: 16,
              right: 16,
              child: Container(
                padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 10),
                decoration: BoxDecoration(
                  color: Colors.white,
                  borderRadius: BorderRadius.circular(16),
                  boxShadow: [
                    BoxShadow(color: Colors.black.withValues(alpha: 0.08), blurRadius: 10, offset: const Offset(0, 4)),
                  ],
                  border: Border.all(color: const Color(0xFFE2E8F0)),
                ),
                child: Row(
                  children: [
                    const Icon(Icons.location_off_rounded, color: Color(0xFFEA3F74), size: 20),
                    const SizedBox(width: 10),
                    Expanded(
                      child: Text(
                        _statusPermissaoMensagem!,
                        style: const TextStyle(fontSize: 12, color: Color(0xFF334155), fontWeight: FontWeight.w600),
                      ),
                    ),
                    TextButton(
                      onPressed: () => _inicializarLocalizacao(forcarDialogo: true),
                      style: TextButton.styleFrom(
                        foregroundColor: const Color(0xFFEA3F74),
                        padding: const EdgeInsets.symmetric(horizontal: 8),
                      ),
                      child: const Text('Ativar', style: TextStyle(fontWeight: FontWeight.bold, fontSize: 12)),
                    ),
                  ],
                ),
              ),
            ),

          // Botões de Controle e "Minha Localização" na Lateral Direita
          Positioned(
            right: 16,
            top: 130,
            child: Column(
              children: [
                // Botão "Minha Localização"
                FloatingActionButton.small(
                  heroTag: 'minha_localizacao_btn',
                  onPressed: _centralizarMinhaLocalizacao,
                  backgroundColor: Colors.white,
                  foregroundColor: const Color(0xFF2563EB), // Azul distintivo
                  elevation: 4,
                  tooltip: 'Minha Localização',
                  child: const Icon(Icons.my_location_rounded, size: 20),
                ),
                const SizedBox(height: 8),
                FloatingActionButton.small(
                  heroTag: 'zoom_in_btn',
                  onPressed: () {
                    final zoom = _mapController.camera.zoom + 1;
                    _mapController.move(_mapController.camera.center, zoom);
                  },
                  backgroundColor: Colors.white,
                  foregroundColor: const Color(0xFF0F172A),
                  elevation: 4,
                  child: const Icon(Icons.add_rounded),
                ),
                const SizedBox(height: 8),
                FloatingActionButton.small(
                  heroTag: 'zoom_out_btn',
                  onPressed: () {
                    final zoom = _mapController.camera.zoom - 1;
                    _mapController.move(_mapController.camera.center, zoom);
                  },
                  backgroundColor: Colors.white,
                  foregroundColor: const Color(0xFF0F172A),
                  elevation: 4,
                  child: const Icon(Icons.remove_rounded),
                ),
                const SizedBox(height: 8),
                FloatingActionButton.small(
                  heroTag: 'refresh_map_btn',
                  onPressed: _carregarEventos,
                  backgroundColor: Colors.white,
                  foregroundColor: const Color(0xFF64748B),
                  elevation: 4,
                  child: const Icon(Icons.refresh_rounded),
                ),
              ],
            ),
          ),

          // Card Flutuante Inferior com o Evento Selecionado
          if (selectedEvent != null)
            Positioned(
              left: 16,
              right: 16,
              bottom: 16,
              child: Card(
                elevation: 8,
                shadowColor: Colors.black26,
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(22)),
                color: Colors.white,
                child: Padding(
                  padding: const EdgeInsets.all(18),
                  child: Column(
                    mainAxisSize: MainAxisSize.min,
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      // Header do Card: Categoria, Tag Público/Privado, Distância e Navegação
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          Row(
                            children: [
                              Container(
                                padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                                decoration: BoxDecoration(
                                  color: const Color(0xFFFDF0F4),
                                  borderRadius: BorderRadius.circular(12),
                                  border: Border.all(color: const Color(0xFFFFCBD8)),
                                ),
                                child: Text(
                                  categoriaSelected,
                                  style: const TextStyle(
                                    color: Color(0xFFEA3F74),
                                    fontSize: 11,
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                              ),
                              const SizedBox(width: 6),
                              Container(
                                padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                                decoration: BoxDecoration(
                                  color: selectedEvent.ehPublico ? const Color(0xFFECFDF5) : const Color(0xFFF1F5F9),
                                  borderRadius: BorderRadius.circular(12),
                                  border: Border.all(
                                    color: selectedEvent.ehPublico ? const Color(0xFFA7F3D0) : const Color(0xFFE2E8F0),
                                  ),
                                ),
                                child: Text(
                                  selectedEvent.ehPublico ? 'Público' : 'Comunidade',
                                  style: TextStyle(
                                    color: selectedEvent.ehPublico ? const Color(0xFF059669) : const Color(0xFF64748B),
                                    fontSize: 10,
                                    fontWeight: FontWeight.bold,
                                  ),
                                ),
                              ),
                            ],
                          ),
                          Row(
                            children: [
                              const Icon(Icons.near_me_rounded, size: 14, color: Color(0xFFEA3F74)),
                              const SizedBox(width: 4),
                              Text(
                                distanciaSelected,
                                style: const TextStyle(
                                  color: Color(0xFFEA3F74),
                                  fontWeight: FontWeight.bold,
                                  fontSize: 12,
                                ),
                              ),
                              const SizedBox(width: 10),
                              GestureDetector(
                                onTap: () {
                                  if (_selectedEventIndex > 0) {
                                    _recenterToEvent(_selectedEventIndex - 1);
                                  }
                                },
                                child: Icon(Icons.chevron_left_rounded,
                                    color: _selectedEventIndex > 0 ? const Color(0xFF0F172A) : Colors.grey.shade300,
                                    size: 22),
                              ),
                              Text(
                                '${_selectedEventIndex + 1}/${_eventosFiltrados.length}',
                                style: const TextStyle(color: Color(0xFF64748B), fontSize: 11, fontWeight: FontWeight.bold),
                              ),
                              GestureDetector(
                                onTap: () {
                                  if (_selectedEventIndex < _eventosFiltrados.length - 1) {
                                    _recenterToEvent(_selectedEventIndex + 1);
                                  }
                                },
                                child: Icon(Icons.chevron_right_rounded,
                                    color: _selectedEventIndex < _eventosFiltrados.length - 1
                                        ? const Color(0xFF0F172A)
                                        : Colors.grey.shade300,
                                    size: 22),
                              ),
                            ],
                          ),
                        ],
                      ),
                      const SizedBox(height: 8),

                      // Título do Evento
                      Text(
                        selectedEvent.titulo,
                        style: const TextStyle(
                          fontSize: 17,
                          fontWeight: FontWeight.bold,
                          color: Color(0xFF0F172A),
                        ),
                        maxLines: 1,
                        overflow: TextOverflow.ellipsis,
                      ),
                      const SizedBox(height: 6),

                      // Local
                      Row(
                        children: [
                          const Icon(Icons.location_on_rounded, size: 16, color: Color(0xFFEF4444)),
                          const SizedBox(width: 6),
                          Expanded(
                            child: Text(
                              selectedEvent.localEvento,
                              style: const TextStyle(
                                  color: Color(0xFF475569), fontSize: 13, fontWeight: FontWeight.w500),
                              overflow: TextOverflow.ellipsis,
                            ),
                          ),
                        ],
                      ),
                      const SizedBox(height: 4),

                      // Data, Horário e Participantes
                      Row(
                        children: [
                          const Icon(Icons.access_time_rounded, size: 14, color: Color(0xFF64748B)),
                          const SizedBox(width: 4),
                          Text(
                            '${selectedEvent.dataFormatada} • ${selectedEvent.horarioFormatado}',
                            style: const TextStyle(color: Color(0xFF64748B), fontSize: 12),
                          ),
                          const SizedBox(width: 12),
                          const Icon(Icons.people_outline_rounded, size: 14, color: Color(0xFF64748B)),
                          const SizedBox(width: 4),
                          Text(
                            '$participantes inscritos',
                            style: const TextStyle(color: Color(0xFF64748B), fontSize: 12),
                          ),
                        ],
                      ),
                      const SizedBox(height: 14),

                      // Ações: Como Chegar e Participar
                      Row(
                        children: [
                          Expanded(
                            child: OutlinedButton.icon(
                              onPressed: () {
                                _snack('Traçando rota para ${selectedEvent.localEvento}...',
                                    cor: const Color(0xFF0F172A));
                              },
                              icon: const Icon(Icons.directions_rounded, size: 16),
                              label: const Text('Como Chegar'),
                              style: OutlinedButton.styleFrom(
                                foregroundColor: const Color(0xFF0F172A),
                                side: const BorderSide(color: Color(0xFFCBD5E1)),
                                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                                padding: const EdgeInsets.symmetric(vertical: 10),
                                textStyle: const TextStyle(fontSize: 12, fontWeight: FontWeight.w600),
                              ),
                            ),
                          ),
                          const SizedBox(width: 10),
                          Expanded(
                            child: isInscrito
                                ? OutlinedButton.icon(
                                    onPressed: () => _toggleParticipacao(selectedEvent),
                                    icon: const Icon(Icons.check_circle_rounded,
                                        size: 16, color: Color(0xFF10B981)),
                                    label: const Text('Inscrito (Sair)'),
                                    style: OutlinedButton.styleFrom(
                                      foregroundColor: const Color(0xFF10B981),
                                      side: const BorderSide(color: Color(0xFF10B981)),
                                      shape: RoundedRectangleBorder(
                                          borderRadius: BorderRadius.circular(12)),
                                      padding: const EdgeInsets.symmetric(vertical: 10),
                                      textStyle: const TextStyle(fontSize: 12, fontWeight: FontWeight.w600),
                                    ),
                                  )
                                : ElevatedButton.icon(
                                    onPressed: () => _toggleParticipacao(selectedEvent),
                                    icon: const Icon(Icons.event_available_rounded, size: 16),
                                    label: const Text('Participar'),
                                    style: ElevatedButton.styleFrom(
                                      backgroundColor: const Color(0xFFEA3F74),
                                      foregroundColor: Colors.white,
                                      elevation: 0,
                                      shape: RoundedRectangleBorder(
                                          borderRadius: BorderRadius.circular(12)),
                                      padding: const EdgeInsets.symmetric(vertical: 10),
                                      textStyle: const TextStyle(fontSize: 12, fontWeight: FontWeight.w600),
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
        ],
      ),
    );
  }
}
