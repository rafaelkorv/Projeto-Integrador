import 'package:flutter/material.dart';
import '../models/post.dart';
import '../models/usuario.dart';
import '../services/api_service.dart';
import '../services/post_service.dart';
import '../services/usuario_service.dart';
import '../widgets/social_avatar.dart';
import 'post_detail_screen.dart';
import 'public_profile_page.dart';

/// Busca unificada de pessoas e publicações.
/// Usa GET /usuarios e GET /posts (rotas já existentes no backend).
/// Não contém sugestões automáticas e não cria registros no servidor.
class SocialSearchPage extends StatefulWidget {
  const SocialSearchPage({super.key});

  @override
  State<SocialSearchPage> createState() => _SocialSearchPageState();
}

class _SocialSearchPageState extends State<SocialSearchPage> {
  static const Color rosa = Color(0xFFEA3F74);
  static const Color tinta = Color(0xFF242631);
  static const Color secundaria = Color(0xFF7D8190);

  final TextEditingController _busca = TextEditingController();
  final FocusNode _foco = FocusNode();
  List<Usuario> _usuarios = [];
  List<Post> _posts = [];
  bool _carregando = true;
  String? _erro;
  int _filtro = 0; // 0 Tudo, 1 Publicações, 2 Pessoas

  @override
  void initState() {
    super.initState();
    _carregar();
  }

  @override
  void dispose() {
    _busca.dispose();
    _foco.dispose();
    super.dispose();
  }

  Future<void> _carregar() async {
    if (mounted) setState(() { _carregando = true; _erro = null; });
    try {
      // As duas consultas começam juntas e usam os dados permitidos à sessão autenticada.
      final resultado = await Future.wait<Object>([
        PostService().listarPosts().then<Object>((valor) => valor),
        UsuarioService().listarUsuarios().then<Object>((valor) => valor),
      ]);
      if (!mounted) return;
      setState(() {
        _posts = resultado[0] as List<Post>;
        _usuarios = resultado[1] as List<Usuario>;
        _carregando = false;
      });
    } catch (e) {
      if (mounted) setState(() { _erro = 'Não foi possível carregar a pesquisa: $e'; _carregando = false; });
    }
  }

  static String _normalizar(String valor) => valor.toLowerCase().trim()
      .replaceAll(RegExp(r'[áàâãä]'), 'a')
      .replaceAll(RegExp(r'[éèêë]'), 'e')
      .replaceAll(RegExp(r'[íìîï]'), 'i')
      .replaceAll(RegExp(r'[óòôõö]'), 'o')
      .replaceAll(RegExp(r'[úùûü]'), 'u')
      .replaceAll('ç', 'c');

  void _abrirPessoa(Usuario usuario) {
    if (usuario.idUsuario == null) return;
    _foco.unfocus();
    Navigator.of(context).push(MaterialPageRoute<void>(
      builder: (_) => PublicProfilePage(usuarioId: usuario.idUsuario!),
    ));
  }

  void _abrirPost(Post post) {
    _foco.unfocus();
    final nomes = <int, String>{
      for (final u in _usuarios)
        if (u.idUsuario != null) u.idUsuario!: u.nome,
    };
    Navigator.of(context).push(MaterialPageRoute<void>(
      builder: (_) => PostDetailScreen(
        post: post,
        nomeAutor: nomes[post.idUsuario] ?? 'Membro do SocialJoin',
        nomesUsuarios: nomes,
      ),
    ));
  }

  Widget _tituloSecao(String titulo, int quantidade) => Padding(
    padding: const EdgeInsets.fromLTRB(4, 21, 4, 10),
    child: Row(children: [
      Text(titulo, style: const TextStyle(color: tinta, fontSize: 17,
          fontWeight: FontWeight.w800, letterSpacing: -0.35)),
      const SizedBox(width: 9),
      Text('$quantidade', style: const TextStyle(color: secundaria,
          fontSize: 13, fontWeight: FontWeight.w600)),
    ]),
  );

  Widget _pessoaCard(Usuario pessoa) => Material(
    color: Colors.white,
    child: InkWell(
      onTap: () => _abrirPessoa(pessoa),
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
        child: Row(children: [
          SocialAvatar(nome: pessoa.nome, foto: pessoa.fotoPerfil, radius: 25),
          const SizedBox(width: 13),
          Expanded(child: Column(crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(pessoa.nome, maxLines: 1, overflow: TextOverflow.ellipsis,
                style: const TextStyle(color: tinta, fontWeight: FontWeight.w800, fontSize: 14)),
              const SizedBox(height: 3),
              Text(pessoa.username == null || pessoa.username!.isEmpty
                  ? 'Perfil do SocialJoin' : '@${pessoa.username}',
                style: const TextStyle(color: secundaria, fontSize: 12)),
              if (pessoa.bio != null && pessoa.bio!.trim().isNotEmpty)
                Padding(padding: const EdgeInsets.only(top: 4),
                  child: Text(pessoa.bio!, maxLines: 1, overflow: TextOverflow.ellipsis,
                    style: const TextStyle(color: secundaria, fontSize: 12))),
            ],
          )),
          const Icon(Icons.chevron_right_rounded, color: Color(0xFFB6B9C2)),
        ]),
      ),
    ),
  );

  Widget _postCard(Post post, Map<int, Usuario> autores) {
    final autor = autores[post.idUsuario];
    final imagem = post.imagens.isNotEmpty ? post.imagens.first : post.imagemUrl;
    final url = imagem == null ? null : ApiService.formatarUrlMidia(imagem);
    return Material(color: Colors.white, child: InkWell(
      onTap: () => _abrirPost(post),
      child: Padding(padding: const EdgeInsets.all(14), child: Row(
        crossAxisAlignment: CrossAxisAlignment.start, children: [
          SocialAvatar(nome: autor?.nome ?? 'Membro', foto: autor?.fotoPerfil, radius: 19),
          const SizedBox(width: 12),
          Expanded(child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
            Text(autor?.nome ?? 'Membro do SocialJoin',
              maxLines: 1, overflow: TextOverflow.ellipsis,
              style: const TextStyle(fontSize: 12, color: secundaria, fontWeight: FontWeight.w600)),
            const SizedBox(height: 5),
            Text(post.titulo, maxLines: 2, overflow: TextOverflow.ellipsis,
              style: const TextStyle(fontSize: 15, color: tinta, fontWeight: FontWeight.w800)),
            if (post.conteudo.trim().isNotEmpty) Padding(
              padding: const EdgeInsets.only(top: 5),
              child: Text(post.conteudo, maxLines: 2, overflow: TextOverflow.ellipsis,
                style: const TextStyle(color: Color(0xFF616674), height: 1.3, fontSize: 13)),
            ),
          ])),
          if (url != null) ...[
            const SizedBox(width: 10),
            ClipRRect(borderRadius: BorderRadius.circular(10),
              child: Image.network(url, width: 72, height: 72, fit: BoxFit.cover,
                errorBuilder: (_, __, ___) => const SizedBox(width: 72, height: 72,
                  child: Icon(Icons.image_not_supported_outlined, color: secundaria)))),
          ],
        ])),
    ));
  }

  Widget _grupo(List<Widget> cards) => Container(
    clipBehavior: Clip.antiAlias,
    decoration: BoxDecoration(color: Colors.white,
      borderRadius: BorderRadius.circular(16),
      border: Border.all(color: const Color(0xFFEDEEF1))),
    child: Column(children: [
      for (int i = 0; i < cards.length; i++) ...[
        if (i > 0) const Divider(height: 1, indent: 13, endIndent: 13,
          color: Color(0xFFF0F0F3)),
        cards[i],
      ],
    ]),
  );

  @override
  Widget build(BuildContext context) {
    final consulta = _normalizar(_busca.text);
    final termos = consulta.split(RegExp(r'\s+')).where((t) => t.isNotEmpty).toList();
    final pessoas = consulta.isEmpty ? <Usuario>[] : _usuarios.where((u) {
      final texto = _normalizar('${u.nome} ${u.username ?? ''} ${u.bio ?? ''}');
      return termos.every(texto.contains);
    }).toList();
    final posts = consulta.isEmpty ? <Post>[] : _posts.where((p) {
      final texto = _normalizar('${p.titulo} ${p.conteudo}');
      return termos.every(texto.contains);
    }).toList();
    final autores = <int, Usuario>{
      for (final u in _usuarios) if (u.idUsuario != null) u.idUsuario!: u,
    };
    final mostrarPessoas = _filtro != 1;
    final mostrarPosts = _filtro != 2;
    final total = (mostrarPessoas ? pessoas.length : 0) +
      (mostrarPosts ? posts.length : 0);

    return Scaffold(
      backgroundColor: const Color(0xFFF7F8FA),
      body: Column(children: [
        Container(color: Colors.white, padding: const EdgeInsets.fromLTRB(16, 15, 16, 14),
          child: Column(children: [
            TextField(
              controller: _busca,
              focusNode: _foco,
              textInputAction: TextInputAction.search,
              onChanged: (_) => setState(() {}),
              decoration: InputDecoration(
                hintText: 'Buscar pessoas e publicações',
                hintStyle: const TextStyle(color: Color(0xFF9398A4), fontSize: 14),
                prefixIcon: const Icon(Icons.search_rounded, color: Color(0xFF858A96)),
                suffixIcon: _busca.text.isEmpty ? null : IconButton(
                  tooltip: 'Limpar pesquisa',
                  icon: const Icon(Icons.close_rounded, size: 19),
                  onPressed: () { _busca.clear(); setState(() {}); _foco.requestFocus(); },
                ),
                filled: true,
                fillColor: const Color(0xFFF4F5F8),
                contentPadding: const EdgeInsets.symmetric(vertical: 14),
                enabledBorder: OutlineInputBorder(borderSide: BorderSide.none,
                  borderRadius: BorderRadius.circular(15)),
                focusedBorder: OutlineInputBorder(
                  borderSide: const BorderSide(color: rosa, width: 1.4),
                  borderRadius: BorderRadius.circular(15)),
              ),
            ),
            const SizedBox(height: 13),
            Row(children: [
              _filtroBotao(0, 'Tudo'),
              const SizedBox(width: 7),
              _filtroBotao(1, 'Publicações'),
              const SizedBox(width: 7),
              _filtroBotao(2, 'Pessoas'),
            ]),
          ])),
        Expanded(child: _carregando
          ? const Center(child: CircularProgressIndicator(color: rosa))
          : _erro != null
            ? Center(child: Padding(padding: const EdgeInsets.all(25),
              child: Column(mainAxisSize: MainAxisSize.min, children: [
                const Icon(Icons.wifi_off_rounded, color: secundaria, size: 36),
                const SizedBox(height: 14), Text(_erro!, textAlign: TextAlign.center),
                const SizedBox(height: 14), OutlinedButton(onPressed: _carregar,
                  child: const Text('Tentar novamente')),
              ])))
            : consulta.isEmpty
              ? const Center(child: Padding(padding: EdgeInsets.all(30),
                  child: Column(mainAxisSize: MainAxisSize.min, children: [
                    Icon(Icons.search_rounded, color: Color(0xFFD0D2D9), size: 54),
                    SizedBox(height: 14),
                    Text('O que você está procurando?',
                      style: TextStyle(fontSize: 17, color: tinta, fontWeight: FontWeight.w800)),
                    SizedBox(height: 6),
                    Text('Encontre publicações, perfis e pessoas pelo nome ou conteúdo.',
                      textAlign: TextAlign.center,
                      style: TextStyle(color: secundaria, height: 1.5, fontSize: 13)),
                  ])))
              : RefreshIndicator(onRefresh: _carregar, color: rosa,
                child: ListView(padding: const EdgeInsets.fromLTRB(15, 7, 15, 95), children: [
                  Padding(padding: const EdgeInsets.fromLTRB(4, 12, 4, 0),
                    child: Text(total == 1 ? '1 resultado' : '$total resultados',
                      style: const TextStyle(fontSize: 12, color: secundaria))),
                  if (mostrarPessoas && pessoas.isNotEmpty) ...[
                    _tituloSecao('Pessoas', pessoas.length),
                    _grupo(pessoas.map(_pessoaCard).toList()),
                  ],
                  if (mostrarPosts && posts.isNotEmpty) ...[
                    _tituloSecao('Publicações', posts.length),
                    _grupo(posts.map((post) => _postCard(post, autores)).toList()),
                  ],
                  if (total == 0) const Padding(
                    padding: EdgeInsets.symmetric(vertical: 70, horizontal: 20),
                    child: Column(children: [
                      Icon(Icons.manage_search_rounded, color: Color(0xFFCBD0D8), size: 48),
                      SizedBox(height: 11),
                      Text('Nenhum resultado encontrado',
                        style: TextStyle(color: tinta, fontWeight: FontWeight.w800)),
                      SizedBox(height: 6),
                      Text('Experimente outro nome ou palavra-chave.',
                        textAlign: TextAlign.center,
                        style: TextStyle(color: secundaria)),
                    ]),
                  ),
                ])),
        ),
      ]),
    );
  }

  Widget _filtroBotao(int indice, String label) {
    final ativo = _filtro == indice;
    return InkWell(
      borderRadius: BorderRadius.circular(20),
      onTap: () => setState(() => _filtro = indice),
      child: Container(padding: const EdgeInsets.symmetric(horizontal: 15, vertical: 8),
        decoration: BoxDecoration(color: ativo ? rosa : const Color(0xFFF4F5F8),
          borderRadius: BorderRadius.circular(20)),
        child: Text(label, style: TextStyle(color: ativo ? Colors.white : tinta,
          fontWeight: ativo ? FontWeight.w800 : FontWeight.w600, fontSize: 12)),
      ),
    );
  }
}
