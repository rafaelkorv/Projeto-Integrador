import 'package:flutter/material.dart';

import '../models/post.dart';

import '../services/post_service.dart';
import '../services/auth_service.dart';

class PostFormScreen extends StatefulWidget {

  const PostFormScreen({super.key});

  @override
  State<PostFormScreen> createState() =>
      _PostFormScreenState();
}

class _PostFormScreenState
    extends State<PostFormScreen> {

  final tituloController =
  TextEditingController();

  final conteudoController =
  TextEditingController();

  final PostService service =
  PostService();

  Future<void> salvar() async {

    if (!AuthService.logado ||
        AuthService.idUsuario == null) {

      ScaffoldMessenger.of(context)
          .showSnackBar(

        const SnackBar(

          content:
          Text('Faça login'),
        ),
      );

      return;
    }

    Post post = Post(

      titulo:
      tituloController.text,

      conteudo:
      conteudoController.text,

      idUsuario:
      AuthService.idUsuario!,
    );

    await service.criarPost(post);

    if (!mounted) return;
    Navigator.pop(context, true);
  }

  @override
  Widget build(BuildContext context) {

    return Scaffold(

      appBar: AppBar(
        title: const Text(
          'Novo Post',
        ),
      ),

      body: Padding(

        padding: const EdgeInsets.all(20),

        child: Column(

          children: [

            TextField(

              controller:
              tituloController,

              decoration:
              const InputDecoration(
                labelText: 'Título',
              ),
            ),

            TextField(

              controller:
              conteudoController,

              decoration:
              const InputDecoration(
                labelText: 'Conteúdo',
              ),
            ),

            const SizedBox(height: 20),

            ElevatedButton(

              onPressed: salvar,

              child:
              const Text('Postar'),
            ),
          ],
        ),
      ),
    );
  }
}