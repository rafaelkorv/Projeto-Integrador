import 'package:flutter/material.dart';

class CommentsPage extends StatelessWidget {
  const CommentsPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Comentários')),
      body: const Center(
        child: Text('Comentários do post'),
      ),
    );
  }
}