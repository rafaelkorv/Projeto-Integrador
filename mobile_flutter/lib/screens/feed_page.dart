import 'dart:convert';
import '../services/authenticated_http.dart' as http;
import 'package:flutter/material.dart';
import '../models/post.dart';
import '../models/usuario.dart';
import '../services/auth_service.dart';
import '../services/post_service.dart';
import '../services/usuario_service.dart';
import '../services/api_service.dart';
import 'post_detail_screen.dart';
import 'public_profile_page.dart';
import '../widgets/social_avatar.dart';

class FeedPage extends StatefulWidget {
  const FeedPage({super.key});
  @override
  State<FeedPage> createState() => _FeedPageState();
}

class _FeedPageState extends State<FeedPage> {
  static const roxo = Color(0xFFEA3F74);
  static const escuro = Color(0xFF171B2B);
  final _postService = PostService();
  final _usuarioService = UsuarioService();
  final ScrollController _scroll = ScrollController();
  final Map<int,String> _nomes = {};
  final Map<int,String?> _fotos = {};
  final Map<int,String> _usernames = {};
  final Map<int,int> _indiceImagem = {};
  final Map<int, List<Map<String, dynamic>>> _comentariosPorPost = {};
  final List<Map<String, dynamic>> _votos = [];
  final Set<int> _curtindo = {};
  List<Post> _posts = [];
  bool _loading = true;
  bool _maisLoading = false;
  bool _temMais = true;
  String? _erro;
  int _pagina = 0;
  static const int _pageSize = 10;

  @override
  void initState() {
    super.initState();
    _scroll.addListener(() {
      if (_scroll.hasClients && _scroll.position.pixels > _scroll.position.maxScrollExtent - 220) _carregarMais();
    });
    _carregar();
  }
  @override
  void dispose() { _scroll.dispose(); super.dispose(); }

  Future<void> _carregar() async {
    if (mounted) setState(() { _loading = true; _erro = null; });
    try {
      final posts = await _postService.listarPosts(page: 0, size: _pageSize);
      // A lista de usuários serve apenas para nomes e fotos. Uma falha nela não deve ocultar posts.
      List<Usuario> usuarios = [];
      try { usuarios = await _usuarioService.listarUsuarios(); } catch (_) {}
      if (!mounted) return;
      setState(() {
        _posts = posts;
        _pagina = 1;
        _temMais = posts.length == _pageSize;
        _nomes.clear(); _fotos.clear(); _usernames.clear();
        for (final u in usuarios) {
          if (u.idUsuario != null) { _nomes[u.idUsuario!] = u.nome; _fotos[u.idUsuario!] = u.fotoPerfil; _usernames[u.idUsuario!] = u.username ?? ''; }
        }
        _loading = false;
      });
    _carregarSocial();
    } catch (e) {
      if (mounted) setState(() { _erro = 'Feed indisponível: $e'; _loading = false; });
    }
  }

  Future<void> _carregarSocial() async {
    try {
      final respostas = await Future.wait([
        http.get(Uri.parse('${ApiService.baseUrl}/comentarios')),
        http.get(Uri.parse('${ApiService.baseUrl}/votos')),
      ]);
      if (!mounted) return;
      final comments = <int, List<Map<String, dynamic>>>{};
      if (respostas[0].statusCode == 200) {
        final raw = jsonDecode(utf8.decode(respostas[0].bodyBytes));
        if (raw is List) for (final item in raw.whereType<Map>()) {
          final id = int.tryParse('${item['idPost']}');
          if (id != null) comments.putIfAbsent(id, () => []).add(Map<String, dynamic>.from(item));
        }
      }
      final votos = <Map<String, dynamic>>[];
      if (respostas[1].statusCode == 200) {
        final raw = jsonDecode(utf8.decode(respostas[1].bodyBytes));
        if (raw is List) votos.addAll(raw.whereType<Map>().map((v) => Map<String, dynamic>.from(v)));
      }
      if (mounted) setState(() {
        _comentariosPorPost..clear()..addAll(comments);
        _votos..clear()..addAll(votos);
      });
    } catch (_) { /* Comentários e votos não impedem o feed de abrir. */ }
  }

  Map<String, dynamic>? _meuVoto(int id) {
    for (final voto in _votos) {
      if (int.tryParse('${voto['idPost']}') == id &&
          int.tryParse('${voto['idUsuario']}') == AuthService.idUsuario &&
          (voto['tipo'] ?? 'like').toString().toLowerCase() == 'like') return voto;
    }
    return null;
  }

  int _qtdCurtidas(int id) => _votos.where((v) =>
      int.tryParse('${v['idPost']}') == id &&
      (v['tipo'] ?? 'like').toString().toLowerCase() == 'like').length;

  Future<void> _alternarCurtida(Post post) async {
    final id = post.idPost;
    if (id == null) return;
    if (!AuthService.logado || AuthService.idUsuario == null) {
      ScaffoldMessenger.of(context).showSnackBar(const SnackBar(
        content: Text('Faça login para curtir publicações.')));
      return;
    }
    if (_curtindo.contains(id)) return;
    setState(() => _curtindo.add(id));
    try {
      final voto = _meuVoto(id);
      final http.Response r;
      if (voto != null) {
        r = await http.delete(Uri.parse('${ApiService.baseUrl}/votos/${voto['idVoto']}'));
      } else {
        r = await http.post(Uri.parse('${ApiService.baseUrl}/votos'),
          headers: {'Content-Type': 'application/json; charset=UTF-8'},
          body: jsonEncode({'idUsuario': AuthService.idUsuario, 'idPost': id, 'tipo': 'like'}));
      }
      if (r.statusCode < 200 || r.statusCode >= 300) {
        throw Exception('HTTP ${r.statusCode}: ${r.body}');
      }
      await _carregarSocial();
    } catch (e) {
      if (mounted) ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Não foi possível atualizar a curtida: $e')));
    } finally { if (mounted) setState(() => _curtindo.remove(id)); }
  }

  Future<void> _carregarMais() async {
    if (_maisLoading || !_temMais || _loading) return;
    _maisLoading = true;
    try {
      final lista = await _postService.listarPosts(page: _pagina, size: _pageSize);
      if (mounted) {
        setState(() {
          final ids = _posts.map((p) => p.idPost).toSet();
          final unicos = lista.where((p) => !ids.contains(p.idPost)).toList();
          _posts.addAll(unicos);
          _pagina++;
          _temMais = lista.length == _pageSize && unicos.isNotEmpty;
        });
      }
    } catch (e) { if (mounted) setState(() => _temMais = false); }
    finally { _maisLoading = false; }
  }

  void _detalhes(Post post) {
    Navigator.push(context, MaterialPageRoute(builder: (_) => PostDetailScreen(
      post: post, nomeAutor: _nomes[post.idUsuario] ?? 'Membro', nomesUsuarios: _nomes,
    ))).then((_) => _carregar());
  }

  void _perfil(int? idUsuario) {
    if (idUsuario == null) return;
    Navigator.of(context).push(MaterialPageRoute<void>(
      builder: (_) => PublicProfilePage(usuarioId: idUsuario),
    ));
  }

  Widget _card(Post post) {
    final autor = _nomes[post.idUsuario] ?? 'Membro do SocialJoin';
    final username = _usernames[post.idUsuario] ?? '';
    final foto = _fotos[post.idUsuario];
    final id = post.idPost ?? -1;
    final curtido = _meuVoto(id) != null;
    final comentarios = _comentariosPorPost[post.idPost] ?? const <Map<String, dynamic>>[];
    final imagens = <String>{
      ...post.imagens,
      if (post.imagemUrl != null && post.imagemUrl!.isNotEmpty) post.imagemUrl!,
    }.map(ApiService.formatarUrlMidia).whereType<String>().toList();
    final pagina = (_indiceImagem[id] ?? 0).clamp(0, imagens.isEmpty ? 0 : imagens.length - 1);

    return Container(
      margin: const EdgeInsets.only(bottom: 9),
      decoration: const BoxDecoration(color: Colors.white,
        border: Border(bottom: BorderSide(color: Color(0xFFEDEEF1)))),
      child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
        Padding(padding: const EdgeInsets.fromLTRB(16, 15, 9, 12),
          child: Row(children: [
            InkWell(onTap: () => _perfil(post.idUsuario),
              child: SocialAvatar(nome: autor, foto: foto, radius: 21)),
            const SizedBox(width: 11),
            Expanded(child: InkWell(onTap: () => _perfil(post.idUsuario),
              child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
                Text(autor, maxLines: 1, overflow: TextOverflow.ellipsis,
                  style: const TextStyle(color: escuro, fontSize: 14,
                    fontWeight: FontWeight.w800)),
                const SizedBox(height: 3),
                Text(username.isEmpty ? 'Membro do SocialJoin' : '@$username',
                  maxLines: 1, overflow: TextOverflow.ellipsis,
                  style: const TextStyle(fontSize: 11, color: Color(0xFF858B9B))),
              ]))),
            PopupMenuButton<String>(
              tooltip: 'Opções da publicação',
              padding: EdgeInsets.zero,
              icon: const Icon(Icons.more_horiz_rounded, color: Color(0xFF6A7180)),
              onSelected: (opcao) { if (opcao == 'abrir') _detalhes(post); },
              itemBuilder: (_) => const [
                PopupMenuItem(value: 'abrir', child: Text('Ver publicação')),
              ],
            ),
          ])),
        InkWell(onTap: () => _detalhes(post),
          child: Padding(padding: const EdgeInsets.fromLTRB(16, 0, 16, 14),
            child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
              if (post.titulo.trim().isNotEmpty)
                Text(post.titulo, style: const TextStyle(fontSize: 17,
                  fontWeight: FontWeight.w800, color: escuro,
                  letterSpacing: -0.35, height: 1.32)),
              if (post.conteudo.trim().isNotEmpty) ...[
                if (post.titulo.trim().isNotEmpty) const SizedBox(height: 7),
                Text(post.conteudo, maxLines: 5, overflow: TextOverflow.ellipsis,
                  style: const TextStyle(fontSize: 14, color: Color(0xFF424854),
                    height: 1.5)),
              ],
            ]))),
        if (imagens.isNotEmpty) ...[
          SizedBox(height: 280, child: PageView.builder(
            key: ValueKey('imagens_post_$id'),
            itemCount: imagens.length,
            onPageChanged: (index) => setState(() => _indiceImagem[id] = index),
            itemBuilder: (context, index) => GestureDetector(
              onTap: () => _detalhes(post),
              child: Container(color: const Color(0xFFF4F5F7),
                child: Image.network(imagens[index], fit: BoxFit.contain,
                  errorBuilder: (_, __, ___) => const Center(child: Icon(
                    Icons.broken_image_outlined, color: Color(0xFF9CA1AD), size: 35))),
              ),
            ),
          )),
          if (imagens.length > 1) Center(child: Padding(
            padding: const EdgeInsets.only(top: 10, bottom: 1),
            child: Row(mainAxisSize: MainAxisSize.min,
              children: List.generate(imagens.length, (index) => Container(
                height: index == pagina ? 7 : 5,
                width: index == pagina ? 7 : 5,
                margin: const EdgeInsets.symmetric(horizontal: 3),
                decoration: BoxDecoration(shape: BoxShape.circle,
                  color: index == pagina ? roxo : const Color(0xFFD1D4DC)),
              )),
            ),
          )),
        ],
        Padding(padding: const EdgeInsets.fromLTRB(10, 3, 10, 2),
          child: Row(children: [
            TextButton.icon(
              style: TextButton.styleFrom(foregroundColor: curtido ? roxo : const Color(0xFF565F6D)),
              onPressed: _curtindo.contains(id) ? null : () => _alternarCurtida(post),
              icon: Icon(curtido ? Icons.favorite_rounded : Icons.favorite_border_rounded, size: 22),
              label: Text('${_qtdCurtidas(id)}', style: const TextStyle(fontWeight: FontWeight.w700)),
            ),
            TextButton.icon(
              style: TextButton.styleFrom(foregroundColor: const Color(0xFF565F6D)),
              onPressed: () => _detalhes(post),
              icon: const Icon(Icons.mode_comment_outlined, size: 21),
              label: Text('${comentarios.length}', style: const TextStyle(fontWeight: FontWeight.w700)),
            ),
            const Spacer(),
            TextButton(onPressed: () => _detalhes(post),
              child: const Text('Ver publicação', style: TextStyle(color: Color(0xFF7A808D),
                fontSize: 11, fontWeight: FontWeight.w600))),
          ])),
        if (comentarios.isNotEmpty) Padding(
          padding: const EdgeInsets.fromLTRB(16, 0, 16, 14),
          child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
            if (comentarios.length > 2) InkWell(onTap: () => _detalhes(post),
              child: Padding(padding: const EdgeInsets.only(bottom: 8),
                child: Text('Ver todos os ${comentarios.length} comentários',
                  style: const TextStyle(color: Color(0xFF898F9C), fontSize: 12)))),
            ...comentarios.take(2).map((c) {
              final nome = _nomes[int.tryParse('${c['idUsuario']}')] ?? 'Membro';
              return Padding(padding: const EdgeInsets.only(bottom: 5),
                child: InkWell(onTap: () => _detalhes(post),
                  child: Text.rich(TextSpan(children: [
                    TextSpan(text: '$nome  ', style: const TextStyle(
                      color: escuro, fontWeight: FontWeight.w800)),
                    TextSpan(text: '${c['conteudo'] ?? ''}',
                      style: const TextStyle(color: Color(0xFF646B77))),
                  ]), maxLines: 2, overflow: TextOverflow.ellipsis,
                    style: const TextStyle(fontSize: 12, height: 1.4)),
                ));
            }),
          ]),
        ),
      ]),
    );
  }

  @override
  Widget build(BuildContext context) => Scaffold(
    backgroundColor: const Color(0xFFF7F8FA),
    body: _loading ? const Center(child: CircularProgressIndicator(color: roxo)) :
      _erro != null ? Center(child: Padding(padding: const EdgeInsets.all(24),
        child: Column(mainAxisSize: MainAxisSize.min, children: [
          const Icon(Icons.cloud_off_outlined, size: 45, color: roxo),
          const SizedBox(height: 16), Text(_erro!, textAlign: TextAlign.center),
          const SizedBox(height: 16), FilledButton(onPressed: _carregar,
            style: FilledButton.styleFrom(backgroundColor: roxo),
            child: const Text('Tentar novamente')),
        ]))) : RefreshIndicator(color: roxo, onRefresh: _carregar,
        child: ListView.builder(
          controller: _scroll,
          padding: const EdgeInsets.fromLTRB(0, 0, 0, 90),
          itemCount: _posts.length + 1 + (_posts.isEmpty ? 1 : 0) + (_maisLoading ? 1 : 0),
          itemBuilder: (context, index) {
            if (index == 0) return Padding(
              padding: const EdgeInsets.fromLTRB(18, 18, 18, 14),
              child: Row(children: [
                const Expanded(child: Text('Publicações',
                  style: TextStyle(color: escuro, fontSize: 21,
                    fontWeight: FontWeight.w900, letterSpacing: -0.5))),
                IconButton(tooltip: 'Atualizar publicações', onPressed: _carregar,
                  icon: const Icon(Icons.refresh_rounded,
                    color: Color(0xFF7B8291), size: 21)),
              ]),
            );
            if (_posts.isEmpty && index == 1) return const Padding(
              padding: EdgeInsets.fromLTRB(24, 65, 24, 20),
              child: Center(child: Column(children: [
                Icon(Icons.dynamic_feed_outlined, color: Color(0xFFC8CCD5), size: 45),
                SizedBox(height: 14),
                Text('Ainda não há publicações', style: TextStyle(
                  fontWeight: FontWeight.w800, fontSize: 16, color: escuro)),
                SizedBox(height: 8),
                Text('Use o botão + no topo para compartilhar algo.',
                  textAlign: TextAlign.center, style: TextStyle(color: Color(0xFF858B9B))),
              ])),
            );
            if (index > _posts.length) return const Padding(
              padding: EdgeInsets.all(20), child: Center(
                child: CircularProgressIndicator(color: roxo)));
            return _card(_posts[index - 1]);
          },
        )),
  );
}
