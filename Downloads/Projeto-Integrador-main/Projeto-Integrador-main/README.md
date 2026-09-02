# Projeto Integrador - Rede Social (PI-4DSN)

Este é um projeto de rede social full-stack desenvolvido como Projeto Integrador. O sistema conta com um backend robusto em Java Spring Boot e uma aplicação móvel moderna desenvolvida em Flutter.

## 🚀 Tecnologias Utilizadas

### Backend
- **Linguagem:** Java 17
- **Framework:** Spring Boot 4.x
- **Persistência:** Spring Data JPA / Hibernate
- **Banco de Dados:** MySQL
- **Gerenciamento de Dependências:** Maven
- **Utilitários:** Lombok

### Mobile
- **Framework:** Flutter
- **Linguagem:** Dart
- **Comunicação:** Pacote `http` para integração com API REST

## 📋 Funcionalidades

- **Usuários:** Cadastro, login e perfis.
- **Postagens:** Criação de posts com suporte a imagens e visualização em feed.
- **Interações:** Sistema de likes/dislikes (votos), comentários e compartilhamentos.
- **Comunidades:** Criação e gestão de comunidades temáticas.
- **Estatísticas:** Monitoramento de uso e engajamento do usuário.
- **Posts Salvos:** Funcionalidade para favoritar e salvar postagens.

## 📁 Estrutura do Projeto

```text
.
├── Projeto-Integrador/
│   ├── java/             # Código fonte do Backend (Spring Boot)
│   ├── mobile_flutter/   # Código fonte do App Mobile (Flutter)
│   └── database/         # Scripts SQL de inicialização
└── GEMINI.md             # Instruções de contexto para o agente IA
```

## ⚙️ Configuração e Execução

### 1. Banco de Dados
O projeto utiliza MySQL. Você pode encontrar o script de criação das tabelas em:
`Projeto-Integrador/database/schema.sql`

Certifique-se de configurar as credenciais do banco no arquivo:
`Projeto-Integrador/java/src/main/resources/application.properties`

### 2. Backend (Java)
Para rodar o servidor:
```bash
cd Projeto-Integrador/java
./mvnw spring-boot:run
```
A API estará disponível em `http://localhost:8080`.

### 3. Mobile (Flutter)
Para rodar o aplicativo:
```bash
cd Projeto-Integrador/mobile_flutter
flutter pub get
flutter run
```
*Nota: Se estiver usando um emulador Android, lembre-se de que `localhost` aponta para o próprio emulador. Altere o `baseUrl` em `lib/services/api_service.dart` para o IP da sua máquina se necessário.*

## 🛠️ Endpoints Principais (API)

- `/usuarios`: Gestão de usuários e perfis.
- `/posts`: Criação e listagem de postagens.
- `/comentarios`: Interação em posts.
- `/comunidades`: Exploração e criação de grupos.
- `/votos`: Sistema de curtidas.

---
Desenvolvido como parte do currículo do COTIL.
