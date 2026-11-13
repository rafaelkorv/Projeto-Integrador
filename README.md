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
Para habilitar a função de seguir pessoas, aplique também `java/database/migrations/20261112_user_follows.sql`.
Antes de executar esta versão do backend, aplique `java/database/migrations/20261112_cloudinary_image_storage.sql` para adicionar as colunas que guardam os identificadores das imagens no Cloudinary.

### 2. Backend (Java)
Abra um terminal na pasta raiz do repositório e execute:

```powershell
Set-Location .\java
.\mvnw.cmd spring-boot:run
```
A API estará disponível em `http://localhost:8080`.

O login e o cadastro retornam um token de sessão com validade de 12 horas. As operações de escrita da API exigem esse token; após atualizar o backend, reiniciá-lo ou expirar a sessão, entre novamente. As sessões ficam em memória e são invalidadas ao reiniciar o backend.

#### Configuração inicial do Cloudinary

O backend usa o SDK `cloudinary-http5`. Para criar o cliente, configure `CLOUDINARY_CLOUD_NAME`, `CLOUDINARY_API_KEY` e `CLOUDINARY_API_SECRET` como variáveis de ambiente no sistema ou no serviço de hospedagem. Não coloque os valores reais no repositório. O cliente só é criado quando `CLOUDINARY_CLOUD_NAME` está definido; nesse caso, as outras duas variáveis também são obrigatórias.

Os uploads de perfil e de postagem são enviados pelo backend ao Cloudinary; o banco guarda o endereço seguro e o identificador necessário para substituir ou remover cada imagem. Os arquivos locais de perfil e postagem já existentes continuam acessíveis, mas não são migrados automaticamente. Sem as variáveis do Cloudinary, o backend inicia normalmente, mas os endpoints desses uploads respondem que o serviço não está configurado.

### 3. Web
Com o backend ativo, abra outro terminal na raiz do repositório e inicie um servidor HTTP para a aplicação web:

```powershell
py -m http.server 5500 --directory web
```

Abra `http://localhost:5500` no navegador. Não abra `web/index.html` diretamente como arquivo: a aplicação usa módulos ES e precisa ser servida por HTTP.

A API usa `http://localhost:8080` quando o frontend roda em `localhost` ou `127.0.0.1` e, nos demais casos, usa a origem HTTPS publicada como padrão. Se a API estiver em outro domínio, configure a meta tag `api-base-url` no `web/index.html` com a origem HTTPS correspondente. Para uma demonstração local, confirme que o banco contém usuários e eventos futuros de status agendado.
No backend, defina `APP_CORS_ALLOWED_ORIGINS` como uma lista separada por vírgulas contendo apenas as origens reais do frontend; em desenvolvimento, o padrão permite `http://localhost:5500` e `http://127.0.0.1:5500`.

As rotas de login e cadastro aplicam limites por endereço IP em memória (10 tentativas de login por 15 minutos e 5 cadastros por hora). Reiniciar o backend reinicia esses limites; uma implantação com múltiplas instâncias deve usar um limitador compartilhado.

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
