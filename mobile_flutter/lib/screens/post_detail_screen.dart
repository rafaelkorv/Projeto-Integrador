import 'dart:convert';
import 'dart:typed_data';
import 'package:flutter/material.dart';
import '../services/authenticated_http.dart' as http;
import 'package:image_picker/image_picker.dart';
import '../models/post.dart';
import '../models/usuario.dart';
import '../services/auth_service.dart';
import '../services/api_service.dart';
import '../services/usuario_service.dart';
import '../services/social_attachments_service.dart';
import '../widgets/social_avatar.dart';
import 'public_profile_page.dart';

class PostDetailScreen extends StatefulWidget {
  final Post post;
  final String nomeAutor;
  final Map<int, String> nomesUsuarios;
  const PostDetailScreen({super.key, required this.post,
    required this.nomeAutor, required this.nomesUsuarios});
  @override
  State<PostDetailScreen> createState() => _PostDetailScreenState();
}

class _PostDetailScreenState extends State<PostDetailScreen> {
  final _controller = TextEditingController();
  final _anexos = SocialAttachmentsService();
  final Map<int, Usuario> _usuarios = {};
  List<Map<String, dynamic>> _comentarios = [];
  Map<int, List<String>> _imagens = {};
  Uint8List? _preview;
  String? _nomeArquivo;
  bool _loading = true;
  bool _enviando = false;
  String? _erro;

  @override
  void initState() { super.initState(); _carregar(); }
  @override
  void dispose() { _controller.dispose(); super.dispose(); }

  static int? _numero(dynamic v) => v is num ? v.toInt() : int.tryParse('$v');
  Future<void> _carregar() async {
    final postId = widget.post.idPost;
    if (postId == null) return;
    setState(() { _loading = true; _erro = null; });
    try {
      final res = await http.get(Uri.parse('${ApiService.baseUrl}/comentarios/post/$postId'));
      if (res.statusCode != 200) throw Exception('Comentários: HTTP ${res.statusCode}');
      final data = jsonDecode(utf8.decode(res.bodyBytes));
      final list = data is List ? data.whereType<Map>().map((v) =>
          Map<String, dynamic>.from(v)).toList() : <Map<String, dynamic>>[];
      Map<int, List<String>> imgs = {};
      List<Usuario> pessoas = [];
      try { imgs = await _anexos.listarImagensComentarios(); } catch (_) {}
      try { pessoas = await UsuarioService().listarUsuarios(); } catch (_) {}
      if (!mounted) return;
      setState(() {
        _comentarios = list;
        _imagens = imgs;
        _usuarios..clear()..addEntries(pessoas.where((u) => u.idUsuario != null)
          .map((u) => MapEntry(u.idUsuario!, u)));
        _loading = false;
      });
    } catch (e) {
      if (mounted) setState(() { _loading = false; _erro = '$e'; });
    }
  }

  Future<void> _selecionarFoto() async {
    final p = await ImagePicker().pickImage(source: ImageSource.gallery, imageQuality: 85);
    if (p == null) return;
    final bytes = await p.readAsBytes();
    if (bytes.length > 5 * 1024 * 1024) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Imagem maior que 5 MB.')));
      }
      return;
    }
    if (mounted) setState(() { _preview = bytes; _nomeArquivo = p.name; });
  }

  Future<void> _enviar() async {
    final texto = _controller.text.trim();
    if ((texto.isEmpty && _preview == null) || _enviando) return;
    if (!AuthService.logado || AuthService.idUsuario == null) {
      ScaffoldMessenger.of(context).showSnackBar(const SnackBar(content: Text('Faça login para comentar.')));
      return;
    }
    final idPost = widget.post.idPost;
    if (idPost == null) return;
    setState(() { _enviando = true; _erro = null; });
    try {
      final res = await http.post(Uri.parse('${ApiService.baseUrl}/comentarios'),
        headers: {'Content-Type': 'application/json; charset=UTF-8'},
        body: jsonEncode({'conteudo': texto.isEmpty ? '📷' : texto,
          'idUsuario': AuthService.idUsuario, 'idPost': idPost}));
      if (res.statusCode != 200 && res.statusCode != 201) {
        throw Exception('Comentário não publicado (HTTP ${res.statusCode}): ${res.body}');
      }
      if (_preview != null) {
        final criado = jsonDecode(utf8.decode(res.bodyBytes));
        final idComentario = criado is Map ? _numero(criado['idComentario'] ?? criado['id']) : null;
        if (idComentario == null) {
          throw const FormatException('Comentário publicado, mas o Java não devolveu idComentario. Foto não enviada.');
        }
        await _anexos.upload(tipo: 'comentarios', id: idComentario,
          usuarioId: AuthService.idUsuario!, bytes: _preview!, nome: _nomeArquivo ?? 'imagem.jpg');
      }
      _controller.clear();
      setState(() { _preview = null; _nomeArquivo = null; });
      await _carregar();
    } catch (e) {
      if (mounted) setState(() => _erro = '$e');
    } finally { if (mounted) setState(() => _enviando = false); }
  }

  void _perfil(int? id) {
    if (id == null) return;
    Navigator.push(context, MaterialPageRoute(builder: (_) => PublicProfilePage(usuarioId: id)));
  }

  @override
  Widget build(BuildContext context) {
    final urls = widget.post.imagens.isNotEmpty ? widget.post.imagens :
      (widget.post.imagemUrl == null ? <String>[] : <String>[widget.post.imagemUrl!]);
    return Scaffold(backgroundColor: const Color(0xFFF7F8FA),
      appBar: AppBar(title: const Text('Publicação'), backgroundColor: Colors.white),
      body: Column(children: [
        Expanded(child: RefreshIndicator(onRefresh: _carregar,
          child: ListView(padding: const EdgeInsets.all(16), children: [
            Card(color: Colors.white, elevation: 0, child: Padding(
              padding: const EdgeInsets.all(18), child: Column(
                crossAxisAlignment: CrossAxisAlignment.start, children: [
                InkWell(onTap: () => _perfil(widget.post.idUsuario),
                  child: Row(children: [
                    SocialAvatar(nome: widget.nomeAutor,
                      foto: _usuarios[widget.post.idUsuario]?.fotoPerfil),
                    const SizedBox(width: 10),
                    Expanded(child: Text(widget.nomeAutor,
                      style: const TextStyle(fontWeight: FontWeight.w800, fontSize: 15))),
                  ])),
                const SizedBox(height: 18),
                Text(widget.post.titulo, style: const TextStyle(fontWeight: FontWeight.w900, fontSize: 19)),
                const SizedBox(height: 9),
                Text(widget.post.conteudo, style: const TextStyle(height: 1.5)),
                if (urls.isNotEmpty) ...[
                  const SizedBox(height: 15),
                  SizedBox(height: 260, child: PageView(children: urls.map((f) =>
                    ClipRRect(borderRadius: BorderRadius.circular(12),
                      child: Image.network(ApiService.formatarUrlMidia(f) ?? '', fit: BoxFit.contain,
                        errorBuilder: (_, __, ___) => const Center(child: Text('Imagem indisponível'))))).toList())),
                  if (urls.length > 1) Padding(padding: const EdgeInsets.only(top: 8),
                    child: Center(child: Text('${urls.length} imagens • deslize para ver',
                      style: const TextStyle(color: Color(0xFF858C9C), fontSize: 12)))),
                ],
              ]))),
            const SizedBox(height: 17),
            Text('${_comentarios.length} comentários',
              style: const TextStyle(fontWeight: FontWeight.w900, fontSize: 19)),
            const SizedBox(height: 10),
            if (_loading) const Center(child: CircularProgressIndicator()),
            if (_erro != null) Padding(padding: const EdgeInsets.all(10),
              child: Text(_erro!, style: const TextStyle(color: Colors.red))),
            if (!_loading && _comentarios.isEmpty)
              const Padding(padding: EdgeInsets.all(14), child: Text('Seja o primeiro a comentar.')),
            ..._comentarios.map((c) {
              final id = _numero(c['idUsuario']);
              final cid = _numero(c['idComentario'] ?? c['id']);
              final user = _usuarios[id];
              final nome = user?.nome ?? widget.nomesUsuarios[id] ?? 'Usuário';
              final fotos = _imagens[cid] ?? const <String>[];
              return Card(color: Colors.white, elevation: 0, margin: const EdgeInsets.only(bottom: 8),
                child: Padding(padding: const EdgeInsets.all(13), child: Row(
                  crossAxisAlignment: CrossAxisAlignment.start, children: [
                    InkWell(onTap: () => _perfil(id), child: SocialAvatar(nome: nome,
                      foto: user?.fotoPerfil, radius: 17)),
                    const SizedBox(width: 10),
                    Expanded(child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
                      InkWell(onTap: () => _perfil(id), child: Text(nome,
                        style: const TextStyle(fontWeight: FontWeight.w800))),
                      const SizedBox(height: 5),
                      Text((c['conteudo'] ?? '').toString()),
                      ...fotos.map((f) => Padding(padding: const EdgeInsets.only(top: 7),
                        child: ClipRRect(borderRadius: BorderRadius.circular(9),
                          child: Image.network(ApiService.formatarUrlMidia(f) ?? '',
                            height: 210, fit: BoxFit.contain, alignment: Alignment.centerLeft,
                            errorBuilder: (_, __, ___) => const Text('Imagem indisponível'))))),
                    ])),
                  ])));
            }),
            const SizedBox(height: 30),
          ]))),
        if (AuthService.logado) Container(color: Colors.white,
          padding: EdgeInsets.fromLTRB(10, 8, 10, MediaQuery.of(context).viewInsets.bottom > 0 ? 8 : 12),
          child: Column(mainAxisSize: MainAxisSize.min, children: [
            if (_preview != null) Row(children: [
              ClipRRect(borderRadius: BorderRadius.circular(8),
                child: Image.memory(_preview!, width: 58, height: 58, fit: BoxFit.cover)),
              IconButton(onPressed: () => setState(() { _preview = null; _nomeArquivo = null; }),
                icon: const Icon(Icons.close)),
            ]),
            Row(children: [
              IconButton(onPressed: _enviando ? null : _selecionarFoto,
                icon: const Icon(Icons.add_photo_alternate_outlined, color: Color(0xFFEA3F74))),
              Expanded(child: TextField(controller: _controller, minLines: 1, maxLines: 3,
                decoration: const InputDecoration(hintText: 'Escreva um comentário...', border: InputBorder.none))),
              IconButton.filled(onPressed: _enviando ? null : _enviar,
                style: IconButton.styleFrom(backgroundColor: const Color(0xFFEA3F74)),
                icon: _enviando ? const SizedBox(width: 18, height: 18,
                  child: CircularProgressIndicator(color: Colors.white, strokeWidth: 2))
                : const Icon(Icons.arrow_upward_rounded)),
            ]),
          ])),
      ]),
    );
  }
}
