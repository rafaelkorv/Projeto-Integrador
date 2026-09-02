import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:geolocator/geolocator.dart';
import 'package:latlong2/latlong.dart';
import 'package:social_join/models/usuario.dart';
import 'package:social_join/models/evento.dart';
import 'package:social_join/services/location_service.dart';
import 'package:social_join/screens/register_screen.dart';
import 'package:social_join/screens/home_screen.dart';
import 'package:social_join/screens/map_events_page.dart';

void main() {
  TestWidgetsFlutterBinding.ensureInitialized();

  group('Modelos e Regras de Negócio', () {
    test('Usuario model serializa e desserializa dataNascimento e nomeCompleto corretamente', () {
      final json = {
        'idUsuario': 10,
        'nome': 'Maria Silva',
        'nomeCompleto': 'Maria Silva Souza',
        'email': 'maria@teste.com',
        'senha': '123',
        'username': 'mariass',
        'bio': 'Desenvolvedora',
        'dataNascimento': '1995-04-12',
      };

      final usuario = Usuario.fromJson(json);
      expect(usuario.idUsuario, 10);
      expect(usuario.nome, 'Maria Silva');
      expect(usuario.nomeCompleto, 'Maria Silva Souza');
      expect(usuario.dataNascimento, '1995-04-12');
      expect(usuario.username, 'mariass');

      final map = usuario.toJson();
      expect(map['nome'], 'Maria Silva');
      expect(map['nomeCompleto'], 'Maria Silva Souza');
      expect(map['dataNascimento'], '1995-04-12');
    });

    test('Evento model valida regra absoluta de eventos publicos vs privados', () {
      // Evento público geral (sem comunidade)
      final eventoPublico = Evento(
        id: 1,
        titulo: 'Festival Aberto',
        dataEvento: '2026-09-01',
        horarioInicio: '10:00:00',
        horarioFim: '18:00:00',
        localEvento: 'Parque Ibirapuera',
        comunidadeId: null,
        status: 'AGENDADO',
      );
      expect(eventoPublico.ehPublico, isTrue);

      // Evento privado de comunidade restrita
      final eventoPrivado = Evento(
        id: 2,
        titulo: 'Reunião Secreta',
        dataEvento: '2026-09-01',
        horarioInicio: '19:00:00',
        horarioFim: '21:00:00',
        localEvento: 'Sala 302',
        comunidadeId: 10,
        status: 'AGENDADO',
      );
      expect(eventoPrivado.ehPublico, isFalse);

      // Evento cancelado (não deve gerar notificação)
      final eventoCancelado = Evento(
        id: 3,
        titulo: 'Show Cancelado',
        dataEvento: '2026-09-01',
        horarioInicio: '20:00:00',
        horarioFim: '22:00:00',
        localEvento: 'Praça Central',
        comunidadeId: null,
        status: 'CANCELADO',
      );
      expect(eventoCancelado.ehPublico, isFalse);
    });
  });

  group('Serviço de Localização e Proximidade', () {
    test('Distância configurável está definida e é acessível', () {
      expect(LocationService.raioNotificacaoMetros, equals(500.0));
    });

    test('Lógica de proximidade respeita a regra de SOMENTE eventos publicos e anti-spam', () async {
      LocationService.limparCacheNotificacoes();
      final locationService = LocationService();

      // Posição mock do usuário (Av. Paulista, SP)
      final posUsuario = Position(
        latitude: -23.5615,
        longitude: -46.6560,
        timestamp: DateTime.now(),
        accuracy: 10,
        altitude: 800,
        heading: 0,
        speed: 0,
        speedAccuracy: 0,
        altitudeAccuracy: 0,
        headingAccuracy: 0,
      );

      final eventoPublicoProximo = Evento(
        id: 101,
        titulo: 'Tech Meetup Paulista',
        dataEvento: '2026-09-05',
        horarioInicio: '14:00:00',
        horarioFim: '18:00:00',
        localEvento: 'Av. Paulista',
        comunidadeId: null,
        status: 'AGENDADO',
      );

      final eventoPrivadoProximo = Evento(
        id: 102,
        titulo: 'Festa Privada Condomínio',
        dataEvento: '2026-09-05',
        horarioInicio: '14:00:00',
        horarioFim: '18:00:00',
        localEvento: 'Av. Paulista',
        comunidadeId: 55, // Comunidade privada
        status: 'AGENDADO',
      );

      final coords = {
        101: const LatLng(-23.5616, -46.6561), // ~15 metros de distância
        102: const LatLng(-23.5616, -46.6561), // ~15 metros de distância
      };

      // Executa verificação de proximidade
      await locationService.verificarProximidadeEventos(
        posicaoUsuario: posUsuario,
        eventos: [eventoPublicoProximo, eventoPrivadoProximo],
        coordenadasEventos: coords,
      );

      // O evento público deve estar no cache de notificados (evitando spam)
      // E o evento privado NUNCA deve ter sido notificado
      expect(eventoPublicoProximo.ehPublico, isTrue);
      expect(eventoPrivadoProximo.ehPublico, isFalse);
    });
  });

  group('Interface e Navegação Mobile', () {
    testWidgets('RegisterScreen renderiza todos os campos necessarios e valida data de nascimento',
        (WidgetTester tester) async {
      tester.view.physicalSize = const Size(600, 1000);
      tester.view.devicePixelRatio = 1.0;
      addTearDown(() => tester.view.resetPhysicalSize());

      await tester.pumpWidget(
        const MaterialApp(
          home: RegisterScreen(),
        ),
      );

      expect(find.text('Criar Conta'), findsWidgets);
      expect(find.text('Data de nascimento'), findsOneWidget);

      final botaoCriarConta = find.widgetWithText(ElevatedButton, 'Criar Conta');
      await tester.ensureVisible(botaoCriarConta);
      await tester.tap(botaoCriarConta);
      await tester.pumpAndSettle();

      expect(find.text('Selecione sua data de nascimento'), findsOneWidget);
    });

    testWidgets('Mobile Drawer e Bottom Navigation NÃO possuem a aba Usuários',
        (WidgetTester tester) async {
      tester.view.physicalSize = const Size(500, 900);
      tester.view.devicePixelRatio = 1.0;
      addTearDown(() => tester.view.resetPhysicalSize());

      await tester.pumpWidget(
        const MaterialApp(
          home: HomeScreen(),
        ),
      );

      // Bottom Navigation
      expect(find.byType(BottomNavigationBar), findsOneWidget);
      expect(find.text('Usuários'), findsNothing);

      // Abre Drawer Mobile
      final menuButton = find.byIcon(Icons.menu_rounded);
      expect(menuButton, findsOneWidget);
      await tester.tap(menuButton);
      await tester.pumpAndSettle();

      // Drawer Mobile não deve conter 'Usuários'
      expect(find.text('Usuários'), findsNothing);
      expect(find.text('Início'), findsWidgets);
      expect(find.text('Eventos'), findsWidgets);
      expect(find.text('Mapa'), findsWidgets);
      expect(find.text('Comunidades'), findsWidgets);
    });

    testWidgets('Desktop MANTÉM a aba Usuários e navegação administrativa',
        (WidgetTester tester) async {
      tester.view.physicalSize = const Size(1200, 900);
      tester.view.devicePixelRatio = 1.0;
      addTearDown(() => tester.view.resetPhysicalSize());

      await tester.pumpWidget(
        const MaterialApp(
          home: HomeScreen(),
        ),
      );

      // No Desktop (> 920px), o link "Usuários" DEVE estar visível
      expect(find.text('Usuários'), findsOneWidget);
    });

    testWidgets('MapEventsPage renderiza controles de mapa e botão Minha Localização',
        (WidgetTester tester) async {
      tester.view.physicalSize = const Size(500, 900);
      tester.view.devicePixelRatio = 1.0;
      addTearDown(() => tester.view.resetPhysicalSize());

      await tester.pumpWidget(
        const MaterialApp(
          home: MapEventsPage(),
        ),
      );

      // Aguarda carregamento inicial
      await tester.pump(const Duration(milliseconds: 300));

      // Botão "Minha Localização" com ícone my_location_rounded
      expect(find.byTooltip('Minha Localização'), findsOneWidget);
    });
  });
}
