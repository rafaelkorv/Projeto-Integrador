import 'dart:async';
import 'package:flutter/material.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter_local_notifications/flutter_local_notifications.dart';

class NotificationService {
  static final NotificationService _instance = NotificationService._internal();
  factory NotificationService() => _instance;
  NotificationService._internal();

  FlutterLocalNotificationsPlugin? _pluginInstance;
  bool _inicializado = false;

  FlutterLocalNotificationsPlugin? get _plugin {
    try {
      _pluginInstance ??= FlutterLocalNotificationsPlugin();
      return _pluginInstance;
    } catch (_) {
      return null;
    }
  }

  /// Stream para escutar toques nas notificações e navegar diretamente ao evento
  static final StreamController<String> onNotificationClick = StreamController<String>.broadcast();

  /// Inicializa as configurações de notificação do Android e iOS
  Future<void> inicializar() async {
    if (_inicializado) return;

    const androidSettings = AndroidInitializationSettings('@mipmap/ic_launcher');
    const iosSettings = DarwinInitializationSettings(
      requestAlertPermission: true,
      requestBadgePermission: true,
      requestSoundPermission: true,
    );

    const initSettings = InitializationSettings(
      android: androidSettings,
      iOS: iosSettings,
    );

    try {
      final plugin = _plugin;
      if (plugin != null) {
        await plugin.initialize(
          settings: initSettings,
          onDidReceiveNotificationResponse: (NotificationResponse response) {
            final payload = response.payload;
            if (payload != null && payload.isNotEmpty) {
              if (kDebugMode) {
                print('=== NOTIFICAÇÃO CLICADA COM PAYLOAD: $payload ===');
              }
              onNotificationClick.add(payload);
            }
          },
        );
      }
      _inicializado = true;
    } catch (e) {
      if (kDebugMode) {
        print('Notificação operando em modo fallback/headless: $e');
      }
    }
  }

  /// Solicita permissão de notificação (Android 13+ e iOS)
  Future<bool> solicitarPermissao() async {
    try {
      final plugin = _plugin;
      if (plugin == null) return false;

      final androidPlugin = plugin.resolvePlatformSpecificImplementation<
          AndroidFlutterLocalNotificationsPlugin>();
      if (androidPlugin != null) {
        final concedida = await androidPlugin.requestNotificationsPermission();
        return concedida ?? false;
      }

      final iosPlugin = plugin.resolvePlatformSpecificImplementation<
          IOSFlutterLocalNotificationsPlugin>();
      if (iosPlugin != null) {
        final concedida = await iosPlugin.requestPermissions(
          alert: true,
          badge: true,
          sound: true,
        );
        return concedida ?? false;
      }
    } catch (e) {
      if (kDebugMode) print('Erro ao solicitar permissão de notificação: $e');
    }
    return true;
  }

  /// Dispara a notificação nativa de evento público próximo
  Future<void> mostrarNotificacaoEventoProximo({
    required int id,
    required String titulo,
    required double distanciaMetros,
  }) async {
    await inicializar();

    final distanciaFormatada = distanciaMetros < 1000
        ? '${distanciaMetros.round()} m'
        : '${(distanciaMetros / 1000).toStringAsFixed(1)} km';

    const androidDetails = AndroidNotificationDetails(
      'eventos_proximos',
      'Eventos Próximos',
      channelDescription: 'Avisos sobre eventos públicos acontecendo perto de você',
      importance: Importance.high,
      priority: Priority.high,
      icon: '@mipmap/ic_launcher',
      color: Color(0xFFEA3F74),
      playSound: true,
      enableVibration: true,
    );

    const iosDetails = DarwinNotificationDetails(
      presentAlert: true,
      presentBadge: true,
      presentSound: true,
    );

    const notificationDetails = NotificationDetails(
      android: androidDetails,
      iOS: iosDetails,
    );

    try {
      final plugin = _plugin;
      if (plugin != null) {
        await plugin.show(
          id: id, // ID único do evento para não sobrepor notificações
          title: '🎉 Tem um evento perto de você!',
          body: '$titulo está a $distanciaFormatada de você. Toque para conferir!',
          notificationDetails: notificationDetails,
          payload: id.toString(),
        );
      }
    } catch (e) {
      if (kDebugMode) {
        print('Erro ao emitir notificação de evento próximo: $e');
      }
    }
  }
}
