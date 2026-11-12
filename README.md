# Projeto Integrador - Rede Social (PI-4DSN)

Este é um projeto de rede social full-stack desenvolvido como Projeto Integrador. O sistema inclui uma aplicação web, um backend em Java Spring Boot e um aplicativo móvel em Flutter.

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

### Web
- **Tecnologias:** HTML, CSS e JavaScript (módulos ES)
- **Comunicação:** API REST do backend

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
├── java/                 # Backend Spring Boot
├── mobile_flutter/       # Aplicativo Flutter
├── web/                  # Aplicação web
└── desktop/              # Painel administrativo desktop
```

## ⚙️ Configuração e Execução

### 1. Banco de Dados
O backend utiliza MySQL. Configure o acesso ao banco no arquivo:
`java/src/main/resources/application.properties`

Antes de iniciar o backend, confirme que o serviço MySQL está ativo e que o banco configurado existe. Não há um script `database/schema.sql` neste repositório.
Antes de iniciar uma versão com a opção de repasse da taxa do ingresso, aplique manualmente a migração `java/database/migrations/20261111_add_taxa_repassada_comprador.sql` no banco configurado.
Para ativar fila de espera, agenda, notificações e lembretes de eventos, aplique também `java/database/migrations/20261112_event_management_features.sql`. Os lembretes são notificações internas na web, geradas automaticamente até 24 horas antes; não há envio por e-mail/push nem processamento de pagamentos reais.

### 2. Backend (Java)
Abra um terminal na pasta raiz do repositório e execute:

```powershell
Set-Location .\java
.\mvnw.cmd spring-boot:run
```
A API estará disponível em `http://localhost:8080`.

### 3. Web
Com o backend ativo, abra outro terminal na raiz do repositório e inicie um servidor HTTP para a aplicação web:

```powershell
py -m http.server 5500 --directory web
```

Abra `http://localhost:5500` no navegador. Não abra `web/index.html` diretamente como arquivo: a aplicação usa módulos ES e precisa ser servida por HTTP.

A URL da API usada pelo frontend está configurada em `web/js/api_service.js` (`http://localhost:8080`). Para uma demonstração, confirme que o banco contém usuários e eventos futuros de status agendado.

### 4. Mobile (Flutter)
Para rodar o aplicativo:
```bash
cd mobile_flutter
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
