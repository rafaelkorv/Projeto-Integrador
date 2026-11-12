import 'dart:convert';
import 'authenticated_http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';
import 'api_service.dart';

class ConversaResumo {
  final int idConversa;
  final int idOutroUsuario;
  final String nome;
  final String? username;
  final String? fotoPerfil;
  const ConversaResumo({required this.idConversa, required this.idOutroUsuario,
      required this.nome, this.username, this.fotoPerfil});

  factory ConversaResumo.fromJson(Map<String, dynamic> j) => ConversaResumo(
    idConversa: ChatService.numero(j['idConversa']),
    idOutroUsuario: ChatService.numero(j['idOutroUsuario']),
    nome: (j['nomeOutroUsuario'] ?? 'Usuário').toString(),
    username: j['usernameOutroUsuario']?.toString(),
    fotoPerfil: j['fotoPerfilOutroUsuario']?.toString(),
  );
}

class MensagemChat {
  final int? id;
  final int remetenteId;
  final int destinatarioId;
  final String texto;
  final DateTime? enviadaEm;
  final bool lida;
  const MensagemChat({this.id, required this.remetenteId, required this.destinatarioId,
    required this.texto, this.enviadaEm, this.lida = false});
  factory MensagemChat.fromJson(Map<String, dynamic> j) => MensagemChat(
    id: j['idMensagem'] == null ? null : ChatService.numero(j['idMensagem']),
    remetenteId: ChatService.numero(j['idRemetente']), destinatarioId: 0,
    texto: (j['conteudo'] ?? '').toString(),
    enviadaEm: DateTime.tryParse((j['dataEnvio'] ?? '').toString()),
    lida: j['lida'] == true,
  );
}

class ConversaComMensagens {
  final ConversaResumo conversa;
  final List<MensagemChat> mensagens;
  const ConversaComMensagens(this.conversa, this.mensagens);
  MensagemChat? get ultima => mensagens.isEmpty ? null : mensagens.last;
}

class ChatService {
  String get _base => ApiService.baseUrl;
  static int numero(dynamic v) => v is num ? v.toInt() : int.tryParse('$v') ?? 0;

  Future<List<ConversaResumo>> listarConversas(int eu) async {
    final res = await http.get(Uri.parse('$_base/conversas/usuario/$eu'))
        .timeout(const Duration(seconds: 15));
    if (res.statusCode != 200) throw Exception('Conversas: HTTP ${res.statusCode}');
    final lista = jsonDecode(utf8.decode(res.bodyBytes));
    if (lista is! List) throw const FormatException('Resposta inválida de conversas');
    return lista.whereType<Map>().map((x) => ConversaResumo.fromJson(
      Map<String, dynamic>.from(x))).where((x) => x.idConversa > 0).toList();
  }

  Future<ConversaResumo> buscarOuCriar(int eu, int outro) async {
    if (eu == outro) throw ArgumentError('Não é possível conversar consigo mesmo.');
    final res = await http.post(Uri.parse('$_base/conversas'),
      headers: {'Content-Type': 'application/json; charset=UTF-8'},
      body: jsonEncode({'idUsuario': eu, 'idOutroUsuario': outro}),
    ).timeout(const Duration(seconds: 15));
    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception('Não foi possível abrir conversa (HTTP ${res.statusCode}): ${res.body}');
    }
    final data = jsonDecode(utf8.decode(res.bodyBytes));
    if (data is! Map) throw const FormatException('Resposta inválida de conversa');
    return ConversaResumo.fromJson(Map<String, dynamic>.from(data));
  }

  Future<List<MensagemChat>> listarPorConversa(int idConversa) async {
    final res = await http.get(Uri.parse('$_base/mensagens/conversa/$idConversa'))
        .timeout(const Duration(seconds: 15));
    if (res.statusCode != 200) throw Exception('Mensagens: HTTP ${res.statusCode}');
    final dados = jsonDecode(utf8.decode(res.bodyBytes));
    if (dados is! List) throw const FormatException('Resposta inválida de mensagens');
    return dados.whereType<Map>().map((m) => MensagemChat.fromJson(
      Map<String, dynamic>.from(m))).toList();
  }

  Future<List<MensagemChat>> listar(int eu, int outro) async {
    final conversa = await buscarOuCriar(eu, outro);
    return listarPorConversa(conversa.idConversa);
  }

  Future<void> enviarPorConversa({required int idConversa, required int eu,
    required String texto}) async {
    if (texto.trim().isEmpty) return;
    final res = await http.post(Uri.parse('$_base/mensagens'),
      headers: {'Content-Type': 'application/json; charset=UTF-8'},
      body: jsonEncode({'idConversa': idConversa, 'idRemetente': eu, 'conteudo': texto.trim()}),
    ).timeout(const Duration(seconds: 15));
    if (res.statusCode != 200 && res.statusCode != 201) {
      throw Exception('Envio de mensagem: HTTP ${res.statusCode}: ${res.body}');
    }
  }

  Future<void> enviar({required int eu, required int outro, required String texto}) async {
    final conversa = await buscarOuCriar(eu, outro);
    await enviarPorConversa(idConversa: conversa.idConversa, eu: eu, texto: texto);
  }

  // Indicador local por dispositivo; backend atual não tem endpoint de leitura.
  String _chave(int eu, int conversaId) => 'sj_chat_seen_${eu}_$conversaId';
  Future<int> ultimaVista(int eu, int conversaId) async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getInt(_chave(eu, conversaId)) ?? 0;
  }
  Future<void> marcarLida(int eu, int conversaId, List<MensagemChat> mensagens) async {
    final maxId = mensagens.fold<int>(0, (a, m) => m.id != null && m.id! > a ? m.id! : a);
    if (maxId <= 0) return;
    final prefs = await SharedPreferences.getInstance();
    final anterior = prefs.getInt(_chave(eu, conversaId)) ?? 0;
    if (maxId > anterior) await prefs.setInt(_chave(eu, conversaId), maxId);
  }
  Future<int> naoLidas(int eu, int conversaId, List<MensagemChat> mensagens) async {
    final ultimo = await ultimaVista(eu, conversaId);
    return mensagens.where((m) => m.remetenteId != eu && (m.id ?? 0) > ultimo).length;
  }
  Future<List<ConversaComMensagens>> caixaDeEntrada(int eu) async {
    final conversas = await listarConversas(eu);
    final completos = await Future.wait(conversas.map((c) async {
      try { return ConversaComMensagens(c, await listarPorConversa(c.idConversa)); }
      catch (_) { return ConversaComMensagens(c, const []); }
    }));
    // Só conversas com ao menos uma mensagem, não toda a rede social.
    final ativos = completos.where((x) => x.mensagens.isNotEmpty).toList();
    ativos.sort((a, b) => (b.ultima?.enviadaEm ?? DateTime(1970))
        .compareTo(a.ultima?.enviadaEm ?? DateTime(1970)));
    return ativos;
  }
  Future<int> totalNaoLidas(int eu) async {
    final lista = await caixaDeEntrada(eu);
    final List<int> counts = await Future.wait<int>(lista.map((c) => naoLidas(eu,
        c.conversa.idConversa, c.mensagens)));
    return counts.fold<int>(0, (int total, int quantidade) => total + quantidade);
  }
}
