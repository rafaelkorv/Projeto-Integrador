import 'dart:async';
import 'package:flutter/foundation.dart';
import 'package:geolocator/geolocator.dart';
import 'package:latlong2/latlong.dart';
import '../models/evento.dart';
import 'notification_service.dart';

class LocationService {
  static final LocationService _instance = LocationService._internal();
  factory LocationService() => _instance;
  LocationService._internal();

  /// Distância máxima configurável para considerar um evento próximo (em metros)
  static const double raioNotificacaoMetros = 500.0;

  /// Cache anti-spam de IDs de eventos que já geraram notificação para o usuário
  static final Set<int> _eventosNotificados = <int>{};

  /// Limpa o histórico de notificações (ex: para logout ou reiniciar sessão)
  static void limparCacheNotificacoes() {
    _eventosNotificados.clear();
  }

  /// Verifica o status atual da permissão de localização
  Future<LocationPermission> verificarPermissao() async {
    try {
      final servicoHabilitado = await Geolocator.isLocationServiceEnabled();
      if (!servicoHabilitado) {
        return LocationPermission.denied;
      }
      return await Geolocator.checkPermission();
    } catch (e) {
      if (kDebugMode) print('Erro ao verificar permissão de localização: $e');
      return LocationPermission.denied;
    }
  }

  /// Solicita permissão nativa de localização ao usuário
  Future<LocationPermission> solicitarPermissao() async {
    try {
      final servicoHabilitado = await Geolocator.isLocationServiceEnabled();
      if (!servicoHabilitado) {
        if (kDebugMode) print('Serviço de localização desativado no dispositivo.');
      }
      LocationPermission permissao = await Geolocator.checkPermission();
      if (permissao == LocationPermission.denied) {
        permissao = await Geolocator.requestPermission();
      }
      return permissao;
    } catch (e) {
      if (kDebugMode) print('Erro ao solicitar permissão de localização: $e');
      return LocationPermission.denied;
    }
  }

  /// Obtém a localização atual do usuário de forma eficiente
  Future<Position?> obterPosicaoAtual() async {
    try {
      final permissao = await verificarPermissao();
      if (permissao == LocationPermission.denied ||
          permissao == LocationPermission.deniedForever) {
        return null;
      }

      return await Geolocator.getCurrentPosition(
        locationSettings: const LocationSettings(
          accuracy: LocationAccuracy.medium,
          timeLimit: Duration(seconds: 10),
        ),
      );
    } catch (e) {
      if (kDebugMode) print('Erro ao obter posição atual: $e');
      return null;
    }
  }

  /// Escuta atualizações de localização com filtro de distância para economizar bateria
  Stream<Position> ouvirPosicao({int distanceFilter = 50}) {
    final locationSettings = LocationSettings(
      accuracy: LocationAccuracy.medium,
      distanceFilter: distanceFilter, // Atualiza apenas após deslocamento de 50m
    );
    return Geolocator.getPositionStream(locationSettings: locationSettings);
  }

  /// Calcula a distância em metros entre duas coordenadas geográficas
  double calcularDistanciaMetros({
    required double lat1,
    required double lon1,
    required double lat2,
    required double lon2,
  }) {
    return Geolocator.distanceBetween(lat1, lon1, lat2, lon2);
  }

  /// LÓGICA DE PROXIMIDADE INTELIGENTE:
  /// Verifica se o usuário está próximo de algum evento e emite notificação
  /// REGRA ABSOLUTA: SOMENTE eventos PÚBLICOS (evento.ehPublico == true) geram notificações!
  Future<void> verificarProximidadeEventos({
    required Position posicaoUsuario,
    required List<Evento> eventos,
    required Map<int, LatLng> coordenadasEventos,
  }) async {
    for (final evento in eventos) {
      // 1. REGRA: Eventos privados NÃO podem gerar notificação de proximidade
      if (!evento.ehPublico) {
        if (kDebugMode) {
          print('Evento "${evento.titulo}" é privado/cancelado - ignorando proximidade.');
        }
        continue;
      }

      // Obtém coordenadas do evento
      final coord = coordenadasEventos[evento.id];
      if (coord == null) continue;

      // 2. Calcula a distância entre o usuário e o evento
      final distancia = calcularDistanciaMetros(
        lat1: posicaoUsuario.latitude,
        lon1: posicaoUsuario.longitude,
        lat2: coord.latitude,
        lon2: coord.longitude,
      );

      // 3. Verifica se está dentro do raio configurável (500 metros)
      if (distancia <= raioNotificacaoMetros) {
        // 4. REGRA ANTI-SPAM: Não notificar repetidamente o mesmo evento
        if (!_eventosNotificados.contains(evento.id)) {
          _eventosNotificados.add(evento.id);

          if (kDebugMode) {
            print('🎉 Notificando evento público próximo: "${evento.titulo}" a ${distancia.round()}m');
          }

          // Dispara notificação nativa no sistema operacional
          await NotificationService().mostrarNotificacaoEventoProximo(
            id: evento.id,
            titulo: evento.titulo,
            distanciaMetros: distancia,
          );
        }
      }
    }
  }
}
