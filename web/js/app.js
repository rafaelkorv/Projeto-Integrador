import { ApiService } from './api_service.js?v=event-cycle-3';

// Controlador central da interface: mantém o estado em memória, atualiza o DOM e delega o acesso ao servidor ao ApiService.
class AppController {
    constructor() {
        this.listaUsuariosCompleta = [];
        this.usuariosMap = {};
        this.usuariosFotosMap = {};
        this.imagensPorPost = {};
        this.previewPostUrls = [];
        this.imagensPostSelecionadas = [];
        this.previewFotoPerfilUrl = null;
        this.comunidadesMap = {}; // Guarda id -> nome da comunidade
        this.listaComunidades = [];
        this.comentariosPorPost = {};
        this.posts = [];
        this.paginaFeed = 0;
        this.tamanhoPaginaFeed = 10;
        this.temMaisPosts = true;
        this.carregandoMaisPosts = false;
        this.votos = [];
        this.postsSalvos = [];
        this.filtroFeed = 'todos';
        this.ordenacaoFeed = 'recentes';
        this.comunidadeFeedSelecionada = null;
        this.modoCadastro = false;
        this.telaBoasVindasVisivel = true;
        this.filtroComunidades = 'todas';
        this.comunidadeAtivaId = null;
        this.usuarioParticipaDaComunidade = false;
        this.comunidadeEditando = null;
        this.eventoEditando = null;
        this.listaEventos = [];
        this.paginaEventos = 0;
        this.totalPaginasEventos = 0;
        this.tamanhoPaginaEventos = 12;
        this.interessesSelecionados = [];
        this.usuariosSeguidos = new Set();
        this.usuariosSeguirEmAtualizacao = new Set();
        this.erroCarregarUsuariosSeguidos = false;
        this.mensagemErroUsuariosSeguidos = '';
        this.filtrosEventos = {
            texto: null,
            categoria: null,
            formato: null,
            pago: null,
            dataInicio: null,
            dataFim: null
        };
        this.timerBuscaEventos = null;
        this.viewAntesDoPerfil = 'feed';
        this.visaoEventos = 'descobrir';
        this.agendaIniciadaNaEntrada = false;
        this.listaConversas = [];
        this.conversaAtiva = null;
        this.chatSocket = null;
        this.chatSidebarAberto = true;
        this.filtroPessoas = 'todas';
        this.paginaPessoas = 0;
        this.tamanhoPaginaPessoas = 12;
        this.pessoasFiltradas = [];
        this.coordenadasUsuario = null;
        this.filtroProximidadeAtivo = false;
        this.timerNotificacoes = null;
    }
    // Ponto de entrada da aplicação: registra eventos da interface e decide entre boas-vindas e sessão existente.
    async inicializar() {

        // Listener para Scroll Infinito no Feed
        window.addEventListener('scroll', () => {
            const feedView = document.getElementById('view-feed');
            if (feedView && feedView.classList.contains('active')) {
                const scrollY = window.scrollY || window.pageYOffset;
                const visibleHeight = window.innerHeight;
                const totalHeight = document.documentElement.scrollHeight || document.body.scrollHeight;
                if (scrollY + visibleHeight >= totalHeight - 350) {
                    this.carregarMaisPosts();
                }
            }
        });

        document.addEventListener('keydown', (evento) => {
            if (evento.key === 'Escape') {
                document.querySelector('.account-menu')?.classList.remove('open');
                document.querySelectorAll('.modal-overlay').forEach(modal => {
                    if (modal.style.display === 'flex') {
                        modal.style.display = 'none';
                        if (modal.id === 'alterar-senha-modal') {
                            this.limparFormularioAlteracaoSenha();
                        }
                    }
                });
            }
        });

        document.addEventListener('click', (evento) => {
            if (!evento.target.closest('.account-menu')) {
                document.querySelector('.account-menu')?.classList.remove('open');
            }
        });

        const chatForm =
            document.getElementById('chat-message-form');

        if (chatForm) {
            chatForm.addEventListener(
                'submit',
                (event) => this.enviarMensagemChat(event)
            );
        }

        const chatCloseBtn =
            document.getElementById('chat-close-btn');

        if (chatCloseBtn) {
            chatCloseBtn.addEventListener(
                'click',
                () => this.fecharConversa()
            );
        }

        const chatToggleBtn =
            document.getElementById('chat-toggle-btn');

        if (chatToggleBtn) {
            chatToggleBtn.addEventListener(
                'click',
                () => this.alternarChatSidebar()
            );
        }

        const chatSearchInput =
            document.getElementById('chat-search-input');

        if (chatSearchInput) {
            chatSearchInput.addEventListener(
                'input',
                (event) => this.filtrarConversas(event.target.value)
            );
        }

        document.getElementById('editar-perfil-foto')?.addEventListener(
            'change',
            (evento) => this.previsualizarFotoPerfil(evento.target.files?.[0])
        );

        document.getElementById('post-imagens')?.addEventListener(
            'change',
            (evento) => this.previsualizarImagensPost(evento.target.files)
        );

        const usuarioLogado =
            ApiService.getUsuarioLogado();

        console.log(
            "Usuário encontrado ao iniciar:",
            usuarioLogado
        );

        this.atualizarMenuLateral();

        if (usuarioLogado) {

            // PRIMEIRO mostra o sistema
            this.esconderTelaBoasVindas();
            this.conectarWebSocketChat();

            // A agenda é a tela principal e não deve esperar o carregamento do feed.
            this.agendaIniciadaNaEntrada = true;
            this.carregarEventos();

            // DEPOIS carrega os dados
            try {
                await this.carregarDadosGlobais();
                await this.carregarConversas();
                await this.carregarNotificacoesEventos();
                this.iniciarAtualizacaoNotificacoes();
                await this.abrirPostPeloLink();
            } catch (erro) {
                console.error(
                    "Erro ao carregar dados iniciais:",
                    erro
                );
            }

            return;
        }


        const parametros =
            new URLSearchParams(
                window.location.search
            );

        const postCompartilhado =
            parametros.get('post');

        if (postCompartilhado) {

            this.esconderTelaBoasVindas();

            await this.abrirPostPeloLink();

            return;
        }

        this.mostrarTelaBoasVindas();
    }

    fecharConversa() {
        const janela =
            document.getElementById('chat-window');

        if (janela) {
            janela.classList.remove('active');
        }

        this.conversaAtiva = null;
        const mobileToggle = document.getElementById('chat-mobile-toggle');
        if (mobileToggle) mobileToggle.hidden = false;
    }

    alternarChatSidebar() {
        const sidebar = document.getElementById('chat-sidebar');
        const toggleBtn = document.getElementById('chat-toggle-btn');

        if (!sidebar || !toggleBtn) {
            return;
        }

        if (window.matchMedia('(max-width: 760px)').matches) {
            sidebar.classList.remove('collapsed');
            const aberto = sidebar.classList.toggle('mobile-open');
            const mobileToggle = document.getElementById('chat-mobile-toggle');
            mobileToggle?.setAttribute('aria-expanded', String(aberto));
            toggleBtn.textContent = '×';
            toggleBtn.setAttribute('title', 'Fechar mensagens');
            toggleBtn.setAttribute('aria-label', 'Fechar mensagens');
            return;
        }

        this.chatSidebarAberto = !this.chatSidebarAberto;
        sidebar.classList.toggle('collapsed', !this.chatSidebarAberto);
        toggleBtn.textContent = this.chatSidebarAberto ? '‹' : '›';
        toggleBtn.setAttribute('title', this.chatSidebarAberto ? 'Recolher mensagens' : 'Expandir mensagens');
    }

    filtrarConversas(termoBusca = '') {
        const container = document.getElementById('chat-conversations');

        if (!container) {
            return;
        }

        const texto = termoBusca.trim().toLowerCase();
        const conversasFiltradas = this.listaConversas.filter((conversa) => {
            const nome = (conversa.nomeOutroUsuario || '').toLowerCase();
            const username = (conversa.usernameOutroUsuario || '').toLowerCase();
            return nome.includes(texto) || username.includes(texto);
        });

        this.renderizarConversas(conversasFiltradas, texto);
    }

    atualizarAvatarChat(elemento, nome, fotoPerfil) {
        const inicial = (nome || 'U').trim().charAt(0).toUpperCase() || 'U';
        elemento.replaceChildren();
        elemento.textContent = inicial;

        const urlFoto = ApiService.formatarUrlFotoPerfil(fotoPerfil);
        if (!urlFoto) return;

        const imagem = document.createElement('img');
        imagem.src = urlFoto;
        imagem.alt = '';
        imagem.loading = 'lazy';
        imagem.onerror = () => imagem.remove();
        elemento.appendChild(imagem);
    }

    // --- MENU E NAVEGAÇÃO ---
    mostrarTelaBoasVindas() {
        document.getElementById('welcome-screen').classList.add('active');
        document.getElementById('app-shell').classList.remove('active');
        this.telaBoasVindasVisivel = true;
        this.carregarEventosDestaque();
    }

    esconderTelaBoasVindas() {
        document.getElementById('welcome-screen').classList.remove('active');
        document.getElementById('app-shell').classList.add('active');
        this.telaBoasVindasVisivel = false;
    }

    async carregarEventosDestaque() {
        const container = document.getElementById('preview-events');
        const dataContainer = document.getElementById('preview-date');

        if (!container || !dataContainer) return;

        try {
            const resultado = await ApiService.buscarEventos({
                page: 0,
                size: 100
            });
            const agora = new Date();
            const eventos = (resultado.content || [])
                .filter(evento => {
                    if (!evento.dataEvento) return false;
                    const dataHora = new Date(
                        `${evento.dataEvento}T${evento.horarioInicio || '00:00'}`
                    );
                    return !Number.isNaN(dataHora.getTime()) && dataHora >= agora;
                })
                .sort((a, b) => {
                    const dataA = new Date(`${a.dataEvento}T${a.horarioInicio || '00:00'}`);
                    const dataB = new Date(`${b.dataEvento}T${b.horarioInicio || '00:00'}`);
                    return dataA - dataB;
                })
                .slice(0, 3);

            if (!eventos.length) {
                container.innerHTML = '<div class="preview-empty">Nenhum evento futuro agendado por enquanto.</div>';
                dataContainer.innerHTML = '<strong>--</strong><span>AGENDA<br><b>em breve</b></span>';
                return;
            }

            const primeiroEvento = eventos[0];
            const data = this.formatarDataDestaque(primeiroEvento.dataEvento);
            dataContainer.innerHTML = `
                <strong>${data.dia}</strong>
                <span>${data.mes}<br><b>${data.semana}</b></span>
            `;

            container.innerHTML = eventos.map((evento, index) => {
                const horario = evento.horarioInicio
                    ? evento.horarioInicio.substring(0, 5)
                    : '--:--';
                const local = evento.localEvento || 'Local a confirmar';
                const participantes = evento.quantidadeParticipantes || 0;
                const limite = evento.limiteParticipantes
                    ? ` · ${participantes}/${evento.limiteParticipantes} vagas`
                    : '';

                return `
                    <div class="preview-event ${index === 0 ? 'preview-event-highlight' : ''}">
                        <div class="preview-event-time">${horario}</div>
                        <div><strong>${this.escaparHtmlDestaque(evento.titulo || 'Evento')}</strong><span>${this.escaparHtmlDestaque(local)}${limite}</span></div>
                        <i class="material-icons">chevron_right</i>
                    </div>
                `;
            }).join('');
        } catch (erro) {
            console.warn('Não foi possível carregar a agenda inicial.', erro);
            container.innerHTML = '<div class="preview-empty">A agenda estará disponível em instantes.</div>';
        }
    }

    formatarDataDestaque(dataEvento) {
        if (!dataEvento) return { dia: '--', mes: 'AGENDA', semana: 'em breve' };

        const data = new Date(`${dataEvento}T00:00:00`);
        return {
            dia: String(data.getDate()).padStart(2, '0'),
            mes: data.toLocaleDateString('pt-BR', { month: 'short' }).replace('.', '').toUpperCase(),
            semana: data.toLocaleDateString('pt-BR', { weekday: 'long' })
        };
    }

    // Escapa caracteres especiais antes de interpolar dados da API em innerHTML, reduzindo risco de injeção de marcação.
    escaparHtmlDestaque(valor) {
        return String(valor).replace(/[&<>'"]/g, caractere => ({
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            "'": '&#39;',
            '"': '&quot;'
        }[caractere]));
    }

    selecionarOpcao(opcao) {
        if (opcao === 'login') {
            this.modoCadastro = false;
            this.atualizarAuthUI();
            this.abrirModal('auth-modal');
        } else if (opcao === 'cadastro') {
            this.modoCadastro = true;
            this.atualizarAuthUI();
            this.abrirModal('auth-modal');
        } else {
            this.esconderTelaBoasVindas();
            this.atualizarMenuLateral();
            this.carregarDadosGlobais();
        }
    }

    async explorarEventos() {
        this.esconderTelaBoasVindas();
        this.atualizarMenuLateral();
        await this.carregarDadosGlobais();
        this.mudarAba('eventos');
    }

    getInteressesDisponiveis() {
        return [
            'Tecnologia', 'Arte', 'Música', 'Esportes', 'Networking',
            'Cultura', 'Educação', 'Games', 'Gastronomia', 'Saúde',
            'Filmes', 'Fotografia', 'Natureza', 'Inovação', 'Social'
        ];
    }

    getInteressesSelecionados() {
        const usuarioLogado = ApiService.getUsuarioLogado();
        if (usuarioLogado && Array.isArray(usuarioLogado.interesses) && usuarioLogado.interesses.length) {
            this.carregarConversas();
            return usuarioLogado.interesses;
        }

        return [...this.interessesSelecionados];
    }

    toggleInteresse(interesse) {
        const selecionados = new Set(this.getInteressesSelecionados());

        if (selecionados.has(interesse)) {
            selecionados.delete(interesse);
        } else {
            if (selecionados.size >= 5) {
                this.mostrarAuthFeedback('Você pode escolher até 5 interesses.');
                return;
            }
            selecionados.add(interesse);
        }

        this.interessesSelecionados = [...selecionados];
        this.renderizarSelecaoInteresses();
        this.mostrarAuthFeedback('');
    }

    renderizarSelecaoInteresses() {
        const container = document.getElementById('auth-interesses-list');
        if (!container) return;

        const selecionados = new Set(this.getInteressesSelecionados());
        container.innerHTML = this.getInteressesDisponiveis()
            .map(interesse => `
                <button
                    type="button"
                    class="interesse-chip ${selecionados.has(interesse) ? 'active' : ''}"
                    data-interesse="${interesse}"
                    onclick="app.toggleInteresse('${interesse}')"
                >
                    ${interesse}
                </button>
            `)
            .join('');
    }

    atualizarAuthUI() {
        const modal = document.querySelector('.auth-modal');
        const titulo = document.getElementById('auth-title');
        const subtitulo = document.getElementById('auth-subtitle');
        const submit = document.getElementById('auth-submit');
        const forgot = document.getElementById('auth-forgot');
        const emailLabel = document.getElementById('auth-email-label') || document.querySelector('label[for="auth-email"]');
        const emailInput = document.getElementById('auth-email');

        if (!modal || !titulo || !submit) return;

        modal.classList.toggle('register-mode', this.modoCadastro);
        titulo.innerText = this.modoCadastro ? 'Crie seu espaço' : 'Acesso ao Painel';
        subtitulo.innerText = this.modoCadastro
            ? 'Monte seu perfil e encontre eventos que combinam com você.'
            : 'Entre para acompanhar eventos, comunidades e conversas.';
        submit.innerText = this.modoCadastro ? 'Criar minha conta' : 'Entrar';
        forgot.style.display = this.modoCadastro ? 'none' : 'block';

        if (emailLabel) {
            emailLabel.innerText = this.modoCadastro ? 'E-mail' : 'E-mail ou telefone';
        }
        if (emailInput) {
            emailInput.placeholder = this.modoCadastro ? 'seu@email.com' : 'seu@email.com ou (00) 00000-0000';
            emailInput.setAttribute('autocomplete', this.modoCadastro ? 'email' : 'username');
        }

        document.getElementById('auth-senha').setAttribute(
            'autocomplete',
            this.modoCadastro ? 'new-password' : 'current-password'
        );

        document.getElementById('auth-tab-login')?.classList.toggle('active', !this.modoCadastro);
        document.getElementById('auth-tab-register')?.classList.toggle('active', this.modoCadastro);
        this.mostrarAuthFeedback('');
        this.atualizarForcaSenha();
        if (this.modoCadastro) this.renderizarSelecaoInteresses();
    }
    mudarAba(nomeAba) {
        if (nomeAba === 'agenda') return;
        this.ativarAba(nomeAba);
    }

    ativarAba(nomeAba) {
        this.salvarViewAtual(nomeAba);
        document.querySelectorAll('.view-section').forEach(el => el.classList.remove('active'));
        document.querySelectorAll('.nav-btn').forEach(el => el.classList.remove('active'));

        document.getElementById(`view-${nomeAba}`).classList.add('active');
        if (document.getElementById(`tab-${nomeAba}`)) {
            document.getElementById(`tab-${nomeAba}`).classList.add('active');
        }

        this.comunidadeAtivaId = null; // Reseta se sair da tela de subreddit

        if (nomeAba === 'busca') this.filtrarUsuarios();
        if (nomeAba === 'feed') {
            this.renderizarFeedGeral();
            this.carregarEventosFeed();
        }
        if (nomeAba === 'comunidades') this.carregarComunidades();
        if (nomeAba === 'eventos') this.carregarEventos();
        if (nomeAba === 'agenda') this.carregarAgendaEventos();
        if (nomeAba === 'notificacoes') this.carregarNotificacoesEventos();
    }

    atualizarMenuLateral() {
        const container = document.getElementById('header-buttons');
        const user = ApiService.getUsuarioLogado();
        if (user) {
            const inicial = (user.nome || 'U').charAt(0).toUpperCase();
            const fotoUrl = ApiService.formatarUrlFotoPerfil(user.fotoPerfil);
            const fotoUrlComCache = fotoUrl
                ? `${fotoUrl}?v=${Date.now()}`
                : null;
            const avatarHtml = fotoUrl
                ? `<img src="${this.escaparHtmlDestaque(fotoUrlComCache)}" class="account-avatar" style="object-fit:cover; border-radius:50%; width:28px; height:28px;" onerror="this.remove()">`
                : `<span class="account-avatar">${this.escaparHtmlDestaque(inicial)}</span>`;

            container.innerHTML = `
    <div class="account-menu">
        <button class="account-trigger" aria-haspopup="true" aria-expanded="false" onclick="app.alternarMenuConta(event)">
            ${avatarHtml}
            <span class="account-name">${this.escaparHtmlDestaque(user.nome || 'Minha conta')}</span>
            <i class="material-icons account-chevron">expand_more</i>
        </button>
        <div class="account-dropdown" role="menu">
            <div class="account-dropdown-label">Conta conectada</div>
            <button class="account-menu-item" role="menuitem" onclick="app.abrirMeuPerfil()">
                <i class="material-icons">person_outline</i> Meu perfil
            </button>
              <button
        class="account-menu-item"
        role="menuitem"
        onclick="app.abrirItensSalvos()"
    >
        <i class="material-icons">
            bookmark_border
        </i>

        Itens salvos
    </button>
    <button class="account-menu-item" role="menuitem" onclick="app.abrirMinhaAgenda()">
        <i class="material-icons">calendar_month</i>
        Minha agenda
    </button>
     <button
        class="account-menu-item"
        role="menuitem"
        onclick="app.abrirConfiguracoes()"
    >
        <i class="material-icons">
            settings
        </i>

        Configurações
    </button>


    <div class="account-menu-divider"></div>
            <button class="account-menu-item account-menu-logout" role="menuitem" onclick="app.fazerLogout()">
                <i class="material-icons">logout</i> Sair do sistema
            </button>
        </div>
    </div>
`;
        } else {
            container.innerHTML = `<button class="btn-primary" style="width:100%;" onclick="app.abrirModal('auth-modal')">Fazer Login</button>`;
        }
    }

    alternarMenuConta(evento) {
        evento.stopPropagation();
        const menu = document.querySelector('.account-menu');
        const aberto = menu.classList.toggle('open');
        menu.querySelector('.account-trigger').setAttribute('aria-expanded', aberto);
    }

    abrirModal(id) {
        const modal = document.getElementById(id);
        if (!modal) return;

        if (id === 'evento-modal') {
            if (!ApiService.getUsuarioLogado()) {
                this.definirModoAuth(false);
                this.abrirModal('auth-modal');
                this.mostrarAuthFeedback("Faça login para organizar um evento.");
                return;
            }
            this.prepararModalEvento();
        }

        modal.style.display = 'flex';
        if (id === 'auth-modal') {
            setTimeout(() => {
                const targetId = this.modoCadastro ? 'auth-nome-completo' : 'auth-email';
                document.getElementById(targetId)?.focus();
            }, 0);
        } else if (id === 'evento-modal') {
            setTimeout(() => document.getElementById('evento-titulo')?.focus(), 0);
        }
    }

    abrirModalEvento() {
        this.abrirModal('evento-modal');
    }

    prepararModalEvento() {
        this.popularComunidadesEvento(this.comunidadeAtivaId || null);
        this.mostrarEventoFeedback('');

        const hojeIso = new Date().toISOString().split('T')[0];
        const dataInput = document.getElementById('evento-data');
        if (dataInput && !dataInput.value) {
            dataInput.min = hojeIso;
        }
    }

    popularComunidadesEvento(selecionadaId = null) {
        const select = document.getElementById('evento-comunidade-id');
        if (!select) return;

        select.innerHTML = '<option value="">Selecione uma comunidade (opcional)</option>';
        if (Array.isArray(this.listaComunidades)) {
            this.listaComunidades.forEach(com => {
                const id = com.idComunidade || com.id;
                const nome = com.nome || 'Comunidade';
                const selected = (selecionadaId && String(id) === String(selecionadaId)) ? 'selected' : '';
                select.innerHTML += `<option value="${id}" ${selected}>${this.escaparHtmlDestaque(nome)}</option>`;
            });
        }
        if (selecionadaId) {
            select.value = selecionadaId;
        }
    }

    atualizarPreviewCapaEvento(input) {
        const file = input?.files?.[0];
        const previewBox = document.getElementById('evento-capa-preview-box');
        const nomeEl = document.getElementById('evento-capa-nome');
        if (!previewBox || !nomeEl) return;

        if (file) {
            nomeEl.innerText = file.name;
            const reader = new FileReader();
            reader.onload = (e) => {
                previewBox.style.backgroundImage = `linear-gradient(rgba(0,0,0,0.45), rgba(0,0,0,0.65)), url('${e.target.result}')`;
                previewBox.classList.add('has-preview');
            };
            reader.readAsDataURL(file);
        } else {
            nomeEl.innerText = 'Clique para selecionar uma imagem';
            previewBox.style.backgroundImage = 'none';
            previewBox.classList.remove('has-preview');
        }
    }

    mostrarEventoFeedback(mensagem) {
        const feedback = document.getElementById('evento-feedback');
        if (!feedback) return;
        feedback.innerText = mensagem;
        feedback.classList.toggle('visible', Boolean(mensagem));
        if (mensagem) {
            feedback.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
        }
    }

    async mensagemErroApi(resposta, mensagemPadrao) {
        return ApiService.mensagemErroResposta(resposta, mensagemPadrao);
    }

    definirEventoLoading(carregando) {
        const botao = document.getElementById('btn-criar-evento');
        if (!botao) return;
        botao.disabled = carregando;
        botao.classList.toggle('loading', carregando);
        botao.innerText = carregando ? 'Agendando evento...' : 'Agendar Evento';
    }


    // ============================================================
    // GEOLOCALIZAÇÃO E ENDEREÇO REAL DE EVENTOS
    // ============================================================

    atualizarBadgeCoordsEvento(badgeId, lat, lng, textoStatus = null) {
        const badge = document.getElementById(badgeId);
        if (!badge) return;
        const textoEl = badge.querySelector('span');

        badge.classList.remove('com-coords', 'sem-coords', 'buscando');

        if (textoStatus === 'buscando') {
            badge.classList.add('buscando');
            badge.querySelector('.material-icons').innerText = 'hourglass_empty';
            if (textoEl) textoEl.innerText = 'Localizando endereço no mapa...';
        } else if (lat !== null && lat !== undefined && lat !== '' && lng !== null && lng !== undefined && lng !== '') {
            badge.classList.add('com-coords');
            badge.querySelector('.material-icons').innerText = 'check_circle';
            const latFmt = Number(lat).toFixed(4);
            const lngFmt = Number(lng).toFixed(4);
            if (textoEl) textoEl.innerText = `Localizado! Coordenadas para o mapa: ${latFmt}, ${lngFmt}`;
        } else {
            badge.classList.add('sem-coords');
            badge.querySelector('.material-icons').innerText = 'location_off';
            if (textoEl) textoEl.innerText = textoStatus || 'Nenhuma coordenada detectada. Digite o endereço ou clique em "Usar meu GPS".';
        }
    }

    alternarFormatoCriacaoEvento(formato) {
        const label = document.getElementById('evento-local-label');
        const input = document.getElementById('evento-local');
        const icone = document.getElementById('evento-local-icone');
        const wrapCoords = document.getElementById('wrap-evento-coordenadas');
        const btnBuscar = document.getElementById('btn-buscar-coords-evento');

        if (formato === 'online') {
            if (label) label.innerHTML = 'Link da transmissão / Plataforma <span class="required">*</span>';
            if (input) input.placeholder = 'Ex: https://meet.google.com/xyz ou Link do YouTube';
            if (icone) icone.innerText = 'videocam';
            if (wrapCoords) wrapCoords.style.display = 'none';
            if (btnBuscar) btnBuscar.style.display = 'none';
            const lat = document.getElementById('evento-latitude');
            const lng = document.getElementById('evento-longitude');
            if (lat) lat.value = '';
            if (lng) lng.value = '';
        } else {
            if (label) label.innerHTML = 'Endereço do evento <span class="required">*</span>';
            if (input) input.placeholder = 'Ex: Av. Paulista, 1000 - Bela Vista, São Paulo';
            if (icone) icone.innerText = 'place';
            if (wrapCoords) wrapCoords.style.display = 'flex';
            if (btnBuscar) btnBuscar.style.display = 'flex';
        }
    }

    alternarFormatoEdicaoEvento(formato) {
        const label = document.getElementById('editar-evento-local-label');
        const input = document.getElementById('editar-evento-local');
        const icone = document.getElementById('editar-evento-local-icone');
        const wrapCoords = document.getElementById('wrap-editar-evento-coordenadas');
        const btnBuscar = document.getElementById('btn-buscar-coords-edicao');

        if (formato === 'online') {
            if (label) label.innerHTML = 'Link da transmissão / Plataforma <span class="required">*</span>';
            if (input) input.placeholder = 'Ex: https://meet.google.com/xyz ou Link do YouTube';
            if (icone) icone.innerText = 'videocam';
            if (wrapCoords) wrapCoords.style.display = 'none';
            if (btnBuscar) btnBuscar.style.display = 'none';
            const lat = document.getElementById('editar-evento-latitude');
            const lng = document.getElementById('editar-evento-longitude');
            if (lat) lat.value = '';
            if (lng) lng.value = '';
        } else {
            if (label) label.innerHTML = 'Endereço do evento <span class="required">*</span>';
            if (input) input.placeholder = 'Ex: Av. Paulista, 1000 - Bela Vista, São Paulo';
            if (icone) icone.innerText = 'place';
            if (wrapCoords) wrapCoords.style.display = 'flex';
            if (btnBuscar) btnBuscar.style.display = 'flex';
        }
    }

    async capturarCoordenadasEvento() {
        this.atualizarBadgeCoordsEvento('evento-coords-badge', null, null, 'buscando');
        const coords = await ApiService.obterLocalizacaoAtual();
        if (!coords) {
            this.atualizarBadgeCoordsEvento('evento-coords-badge', null, null, 'Permissão de GPS negada ou indisponível.');
            this.mostrarFeedbackGlobal('Não foi possível obter sua localização via GPS. Verifique a permissão do seu navegador.');
            return;
        }

        const latInput = document.getElementById('evento-latitude');
        const lngInput = document.getElementById('evento-longitude');
        if (latInput) latInput.value = coords.lat;
        if (lngInput) lngInput.value = coords.lng;
        this.atualizarBadgeCoordsEvento('evento-coords-badge', coords.lat, coords.lng);

        const inputLocal = document.getElementById('evento-local');
        if (inputLocal && !inputLocal.value.trim()) {
            const endereco = await ApiService.buscarEnderecoPorCoordenadas(coords.lat, coords.lng);
            if (endereco) inputLocal.value = endereco;
        }
    }

    async capturarCoordenadasEdicaoEvento() {
        this.atualizarBadgeCoordsEvento('editar-evento-coords-badge', null, null, 'buscando');
        const coords = await ApiService.obterLocalizacaoAtual();
        if (!coords) {
            this.atualizarBadgeCoordsEvento('editar-evento-coords-badge', null, null, 'Permissão de GPS negada ou indisponível.');
            this.mostrarFeedbackGlobal('Não foi possível obter sua localização via GPS. Verifique a permissão do seu navegador.');
            return;
        }

        const latInput = document.getElementById('editar-evento-latitude');
        const lngInput = document.getElementById('editar-evento-longitude');
        if (latInput) latInput.value = coords.lat;
        if (lngInput) lngInput.value = coords.lng;
        this.atualizarBadgeCoordsEvento('editar-evento-coords-badge', coords.lat, coords.lng);

        const inputLocal = document.getElementById('editar-evento-local');
        if (inputLocal && !inputLocal.value.trim()) {
            const endereco = await ApiService.buscarEnderecoPorCoordenadas(coords.lat, coords.lng);
            if (endereco) inputLocal.value = endereco;
        }
    }

    async buscarCoordenadasPorEnderecoDigitado() {
        const inputLocal = document.getElementById('evento-local');
        const endereco = inputLocal ? inputLocal.value.trim() : '';
        if (!endereco || /^(https?:\/\/|www\.)/i.test(endereco)) return;

        this.atualizarBadgeCoordsEvento('evento-coords-badge', null, null, 'buscando');
        const res = await ApiService.buscarCoordenadasPorEndereco(endereco);
        if (res) {
            const latInput = document.getElementById('evento-latitude');
            const lngInput = document.getElementById('evento-longitude');
            if (latInput) latInput.value = res.lat;
            if (lngInput) lngInput.value = res.lng;
            this.atualizarBadgeCoordsEvento('evento-coords-badge', res.lat, res.lng);
        } else {
            this.atualizarBadgeCoordsEvento('evento-coords-badge', null, null, 'Endereço não localizado no mapa. Ajuste o texto ou use o GPS.');
        }
    }

    async buscarCoordenadasEdicaoEvento() {
        const inputLocal = document.getElementById('editar-evento-local');
        const endereco = inputLocal ? inputLocal.value.trim() : '';
        if (!endereco || /^(https?:\/\/|www\.)/i.test(endereco)) return;

        this.atualizarBadgeCoordsEvento('editar-evento-coords-badge', null, null, 'buscando');
        const res = await ApiService.buscarCoordenadasPorEndereco(endereco);
        if (res) {
            const latInput = document.getElementById('editar-evento-latitude');
            const lngInput = document.getElementById('editar-evento-longitude');
            if (latInput) latInput.value = res.lat;
            if (lngInput) lngInput.value = res.lng;
            this.atualizarBadgeCoordsEvento('editar-evento-coords-badge', res.lat, res.lng);
        } else {
            this.atualizarBadgeCoordsEvento('editar-evento-coords-badge', null, null, 'Endereço não localizado no mapa. Ajuste o texto ou use o GPS.');
        }
    }

    aoAlterarEnderecoEvento() {
        const formato = document.getElementById('evento-formato-local')?.value;
        if (formato !== 'online') {
            this.buscarCoordenadasPorEnderecoDigitado();
        }
    }

    aoAlterarEnderecoEdicaoEvento() {
        const formato = document.getElementById('editar-evento-formato-local')?.value;
        if (formato !== 'online') {
            this.buscarCoordenadasEdicaoEvento();
        }
    }

    async alternarFiltroProximidade() {
        this.filtroProximidadeAtivo = !this.filtroProximidadeAtivo;
        const btn = document.getElementById('btn-localizacao-filtro');
        const selectRaio = document.getElementById('filtro-evento-raio');

        if (this.filtroProximidadeAtivo) {
            btn?.classList.add('ativo');
            if (selectRaio) selectRaio.style.display = 'block';

            if (!this.coordenadasUsuario) {
                if (btn) btn.innerHTML = '<i class="material-icons">hourglass_empty</i> Obtendo GPS...';
                this.coordenadasUsuario = await ApiService.obterLocalizacaoAtual();
                if (!this.coordenadasUsuario) {
                    this.mostrarFeedbackGlobal("Não foi possível acessar a localização. Verifique se o GPS está ativo e permitido no navegador.");
                    this.filtroProximidadeAtivo = false;
                    btn?.classList.remove('ativo');
                    if (selectRaio) selectRaio.style.display = 'none';
                    if (btn) btn.innerHTML = '<i class="material-icons">my_location</i> Perto de mim';
                    return;
                }
            }
            if (btn) btn.innerHTML = '<i class="material-icons">check_circle</i> Perto de mim';
        } else {
            btn?.classList.remove('ativo');
            if (selectRaio) selectRaio.style.display = 'none';
            if (btn) btn.innerHTML = '<i class="material-icons">my_location</i> Perto de mim';
        }

        await this.filtrarEventos();
    }

    limparFormularioEvento() {
        const titulo = document.getElementById('evento-titulo');
        if (titulo) titulo.value = '';
        const desc = document.getElementById('evento-desc');
        if (desc) desc.value = '';
        const cat = document.getElementById('evento-categoria');
        if (cat) cat.value = '';
        const data = document.getElementById('evento-data');
        if (data) data.value = '';
        const inicio = document.getElementById('evento-horario-inicio');
        if (inicio) inicio.value = '';
        const fim = document.getElementById('evento-horario-fim');
        if (fim) fim.value = '';
        const enc = document.getElementById('evento-encerramento-inscricoes');
        if (enc) enc.value = '';
        const local = document.getElementById('evento-local');
        if (local) local.value = '';
        const com = document.getElementById('evento-comunidade-id');
        if (com) com.value = '';
        const lim = document.getElementById('evento-limite');
        if (lim) lim.value = '';
        const check = document.getElementById('evento-checkin');
        if (check) check.checked = false;
        const capaInput = document.getElementById('evento-capa');
        if (capaInput) capaInput.value = '';
        const preco =
            document.getElementById('evento-preco');

        if (preco) {
            preco.value = '';
        }
        this.atualizarPreviewCapaEvento(capaInput);
        const fmt = document.getElementById('evento-formato-local');
        if (fmt) fmt.value = 'presencial';
        this.alternarFormatoCriacaoEvento('presencial');
        const lat = document.getElementById('evento-latitude');
        if (lat) lat.value = '';
        const lng = document.getElementById('evento-longitude');
        if (lng) lng.value = '';
        this.atualizarBadgeCoordsEvento('evento-coords-badge', null, null);
        this.mostrarEventoFeedback('');
    }

    fecharModal(id) {
        document.getElementById(id).style.display = 'none';
        if (id === 'alterar-senha-modal') {
            this.limparFormularioAlteracaoSenha();
        }
        if (id === 'post-modal') {
            const inputImagens = document.getElementById('post-imagens');
            if (inputImagens) inputImagens.value = '';
            this.limparPrevisualizacoesPost();
        }
    }

    abrirModalPost() {

        const idUsuario =
            ApiService.getIdUsuarioLogado();

        if (!idUsuario) {
            return this.mostrarFeedbackGlobal("Faça login primeiro.");
        }


        // Se estiver dentro de uma comunidade,
        // só permite publicar se participar dela.
        if (
            this.comunidadeAtivaId &&
            !this.usuarioParticipaDaComunidade
        ) {
            return this.mostrarFeedbackGlobal(
                "Você precisa entrar na comunidade antes de publicar."
            );
        }


        const tituloEl =
            document.getElementById('modal-post-title');

        if (this.comunidadeAtivaId) {

            tituloEl.innerText =
                `Postar em: ${this.comunidadesMap[
                this.comunidadeAtivaId
                ]
                }`;

        } else {

            tituloEl.innerText =
                "Nova Publicação Global";
        }


        const seletorComunidade =
            document.getElementById(
                'post-comunidade-id'
            );


        if (seletorComunidade) {

            seletorComunidade.innerHTML = `
            <option value="">
                Publicar no feed global
            </option>
        `;


            /*
             * Mostra somente comunidades
             * das quais o usuário participa.
             */
            const comunidadesPermitidas =
                this.listaComunidades.filter(
                    comunidade => {

                        const membros =
                            comunidade.membros || [];

                        const usuarioEhMembro =
                            membros.some(
                                membro =>
                                    String(
                                        membro.idUsuario ||
                                        membro.id
                                    ) ===
                                    String(idUsuario)
                            );


                        /*
                         * Também permite ao criador
                         * publicar na própria comunidade.
                         */
                        const idCriador =
                            comunidade.criador?.idUsuario ||
                            comunidade.criador?.id;

                        const usuarioEhCriador =
                            String(idCriador) ===
                            String(idUsuario);


                        return (
                            usuarioEhMembro ||
                            usuarioEhCriador
                        );
                    }
                );


            comunidadesPermitidas.forEach(
                comunidade => {

                    const id =
                        comunidade.idComunidade ||
                        comunidade.id;

                    seletorComunidade.innerHTML += `
                    <option value="${id}">
                        ${this.escaparHtmlDestaque(
                        comunidade.nome
                    )
                        }
                    </option>
                `;
                }
            );


            /*
             * Se o usuário abriu o modal
             * de dentro de uma comunidade,
             * ela já fica selecionada.
             */
            if (this.comunidadeAtivaId) {

                seletorComunidade.value =
                    this.comunidadeAtivaId;
            }
        }


        this.abrirModal('post-modal');
    }

    // --- AUTH --- (Igual ao anterior, resumido para focar na lógica)
    alternarAuth() {
        this.modoCadastro = !this.modoCadastro;
        this.atualizarAuthUI();
    }

    definirModoAuth(modoCadastro) {
        this.modoCadastro = modoCadastro;
        this.atualizarAuthUI();
    }

    mostrarAuthFeedback(mensagem) {
        const feedback = document.getElementById('auth-feedback');
        if (!feedback) return;
        feedback.innerText = mensagem;
        feedback.classList.toggle('visible', Boolean(mensagem));
    }

    alternarVisibilidadeSenha(id, botao) {
        const input = document.getElementById(id);
        if (!input) return;
        const visivel = input.type === 'text';
        input.type = visivel ? 'password' : 'text';
        botao.setAttribute('aria-label', visivel ? 'Mostrar senha' : 'Ocultar senha');
        botao.querySelector('.material-icons').innerText = visivel ? 'visibility' : 'visibility_off';
    }

    atualizarForcaSenha() {
        const indicador = document.getElementById('auth-password-strength');
        const senha = document.getElementById('auth-senha')?.value || '';
        if (!indicador) return;

        if (!this.modoCadastro || !senha) {
            indicador.innerText = '';
            indicador.className = 'password-strength';
            return;
        }

        const pontuacao = [
            senha.length >= 8,
            /[A-Z]/.test(senha),
            /[0-9]/.test(senha),
            /[^A-Za-z0-9]/.test(senha)
        ].filter(Boolean).length;
        const nivel = pontuacao <= 1 ? 'weak' : pontuacao <= 3 ? 'medium' : 'strong';
        const texto = nivel === 'weak' ? 'Senha fraca' : nivel === 'medium' ? 'Senha média' : 'Senha forte';
        indicador.innerText = texto;
        indicador.className = `password-strength ${nivel}`;
    }

    definirAuthLoading(carregando) {
        const botao = document.getElementById('auth-submit');
        if (!botao) return;
        botao.disabled = carregando;
        botao.classList.toggle('loading', carregando);
        botao.innerText = carregando ? 'Aguarde...' : (this.modoCadastro ? 'Criar minha conta' : 'Entrar');
    }

    recuperarSenha() {
        this.mostrarAuthFeedback('A recuperação de senha ainda precisa ser habilitada no servidor.');
    }

    getInteresseScore(item, interessesUsuario) {
        if (!interessesUsuario || !interessesUsuario.length) return 0;

        const texto = `${item?.categoria || ''} ${item?.titulo || ''} ${item?.nome || ''} ${item?.descricao || ''}`.toLowerCase();
        let score = 0;

        interessesUsuario.forEach(interesse => {
            const termo = interesse.toLowerCase();
            if ((item?.categoria || '').toLowerCase().includes(termo)) score += 3;
            if (texto.includes(termo)) score += 1;
        });

        return score;
    }

    ordenarPorInteresses(lista) {
        const interessesUsuario = this.getInteressesSelecionados();
        if (!interessesUsuario.length || !lista?.length) return lista;

        return [...lista].sort((a, b) => this.getInteresseScore(b, interessesUsuario) - this.getInteresseScore(a, interessesUsuario));
    }

    async processarAuth() {
        const nomeCompleto =
            document.getElementById('auth-nome-completo').value.trim();
        const e = document.getElementById('auth-email').value.trim();
        const s = document.getElementById('auth-senha').value;
        const n = document.getElementById('auth-nome').value.trim();
        const username =
            document.getElementById('auth-username').value.trim().replace(/^@+/, '');
        const dataNascimento =
            document.getElementById('auth-data-nascimento').value;
        const telefone =
            document.getElementById('auth-telefone').value.trim();
        const confirmarSenha =
            document.getElementById('auth-confirmar-senha')?.value || '';
        const interesses = this.getInteressesSelecionados();

        this.mostrarAuthFeedback('');

        if (!s || (!this.modoCadastro && !e)) {
            return this.mostrarAuthFeedback(
                this.modoCadastro
                    ? 'Informe uma senha para continuar.'
                    : 'Informe seu e-mail ou telefone e sua senha para continuar.'
            );
        }

        if (!this.modoCadastro) {
            const credencial = e;
            if (!credencial) {
                return this.mostrarAuthFeedback('Informe seu e-mail ou telefone para continuar.');
            }
            if (credencial.includes('@') && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(credencial)) {
                return this.mostrarAuthFeedback('Informe um e-mail válido.');
            }
            if (!credencial.includes('@') && credencial.replace(/\D/g, '').length < 10) {
                return this.mostrarAuthFeedback('Informe um e-mail ou telefone válido.');
            }
        }

        try {
            if (this.modoCadastro) {
                if (
                    !nomeCompleto ||
                    !n ||
                    !username ||
                    !dataNascimento ||
                    !s
                ) {
                    return this.mostrarAuthFeedback('Preencha os campos obrigatórios do cadastro.');
                }

                if (!e) {
                    return this.mostrarAuthFeedback('Informe seu e-mail para criar a conta.');
                }

                if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(e)) {
                    return this.mostrarAuthFeedback('Informe um e-mail válido.');
                }

                if (!/^[a-zA-Z0-9._-]{3,30}$/.test(username)) {
                    return this.mostrarAuthFeedback('O username deve ter de 3 a 30 caracteres, sem espaços.');
                }

                const nascimento = new Date(`${dataNascimento}T00:00:00`);
                const hoje = new Date();
                const idade = hoje.getFullYear() - nascimento.getFullYear() -
                    ((hoje.getMonth() < nascimento.getMonth() ||
                        (hoje.getMonth() === nascimento.getMonth() && hoje.getDate() < nascimento.getDate())) ? 1 : 0);

                if (Number.isNaN(nascimento.getTime()) || idade < 13 || idade > 120) {
                    return this.mostrarAuthFeedback('Informe uma data de nascimento válida (idade mínima de 13 anos).');
                }

                if (telefone && telefone.replace(/\D/g, '').length < 10) {
                    return this.mostrarAuthFeedback('Informe um telefone válido ou deixe em branco.');
                }

                if (s.length < 8) {
                    return this.mostrarAuthFeedback('A senha precisa ter pelo menos 8 caracteres.');
                }

                if (s !== confirmarSenha) {
                    return this.mostrarAuthFeedback('As senhas não coincidem.');
                }

                this.definirAuthLoading(true);
                const res = await ApiService.criarUsuario(
                    nomeCompleto,
                    n,
                    username,
                    dataNascimento,
                    e,
                    telefone || null,
                    s,
                    interesses
                );
                if (res.ok) {
                    const usuario = await res.json();
                    usuario.interesses = interesses;
                    this.interessesSelecionados = [...interesses];
                    ApiService.salvarSessao(usuario);
                    this.posLogin();
                } else this.mostrarAuthFeedback('Não foi possível criar a conta. Verifique seus dados.');
            } else {
                const credencial = e;

                this.definirAuthLoading(true);
                const res = await ApiService.login(credencial, s);

                if (res.ok) {
                    const user = await res.json();
                    ApiService.salvarSessao(user);
                    this.posLogin();
                } else {
                    this.mostrarAuthFeedback('E-mail/telefone ou senha incorretos.');
                }
            }
        } catch (err) {
            console.error(err);
            this.mostrarAuthFeedback('Não foi possível conectar ao servidor. Tente novamente.');
        } finally {
            this.definirAuthLoading(false);
        }
    }

    posLogin() {
        const usuario = ApiService.getUsuarioLogado();
        this.interessesSelecionados = Array.isArray(usuario?.interesses) ? [...usuario.interesses] : [];
        this.fecharModal('auth-modal');
        this.esconderTelaBoasVindas();
        this.atualizarMenuLateral();
        this.carregarDadosGlobais();
        this.carregarConversas();
        this.carregarNotificacoesEventos();
        this.conectarWebSocketChat();
        this.iniciarAtualizacaoNotificacoes();
    }

    iniciarAtualizacaoNotificacoes() {
        if (!this.timerNotificacoes) {
            this.timerNotificacoes = setInterval(() => {
                if (ApiService.getIdUsuarioLogado()) {
                    this.carregarNotificacoesEventos();
                }
            }, 60000);
        }
    }

    fazerLogout() {

        ApiService.fazerLogout();
        this.interessesSelecionados = [];

        this.atualizarMenuLateral();

        this.mudarAba('feed');

        this.agendaIniciadaNaEntrada = false;
        this.mostrarTelaBoasVindas();
    }

    // --- CARREGAMENTO GLOBAL E PAGINADO ---
    // Carrega em paralelo os dados compartilhados por várias telas para reduzir esperas e chamadas sequenciais.
    async carregarDadosGlobais() {
        try {
            const [usuarios, comunidades, comentarios, votos, salvos, imagensPost] = await Promise.all([
                ApiService.listarUsuarios(),
                ApiService.listarComunidades().catch(() => []),
                ApiService.listarComentarios().catch(() => []),
                ApiService.listarVotos().catch(() => []),
                ApiService.listarPostsSalvos().catch(() => []),
                typeof ApiService.listarImagensPost === 'function'
                    ? ApiService.listarImagensPost().catch(() => [])
                    : Promise.resolve([])
            ]);

            this.listaUsuariosCompleta = usuarios;
            this.listaUsuariosCompleta.forEach(u => {
                const id = u.idUsuario || u.id;
                this.usuariosMap[id] = u.nome;
                this.usuariosFotosMap[id] = u.fotoPerfil;
            });

            this.listaComunidades = comunidades;
            comunidades.forEach(c =>
                this.comunidadesMap[c.idComunidade || c.id] = c.nome
            );

            this.comentariosPorPost = {};
            comentarios.forEach(c => {
                let idPost = c.idPost || c.id_post;
                if (!this.comentariosPorPost[idPost]) this.comentariosPorPost[idPost] = [];
                this.comentariosPorPost[idPost].push(c);
            });

            this.votos = votos;
            this.postsSalvos = salvos;
            this.imagensPorPost = imagensPost.reduce((porPost, imagem) => {
                // Agrupar por ID permite montar a galeria de cada post sem consultar a API a cada card.
                const idPost = imagem.idPost || imagem.id_post;
                if (!porPost[idPost]) porPost[idPost] = [];
                porPost[idPost].push(imagem);
                return porPost;
            }, {});
            await this.carregarUsuariosSeguidos();
            this.renderizarSugestoesPessoas();

            // Carrega os primeiros posts sob demanda (primeira página)
            await this.carregarMaisPosts(true);

            // Atualiza a view que estiver visível depois dos dados compartilhados carregarem.
            if (this.comunidadeAtivaId) this.renderizarSubreddit(this.comunidadeAtivaId);
            else if (document.getElementById('view-eventos')?.classList.contains('active') && !this.agendaIniciadaNaEntrada) {
                this.carregarEventos();
            } else {
                this.renderizarFeedGeral();
                this.carregarEventosFeed();
            }

        } catch (e) {
            console.error(e);
            const activeView = document.querySelector('.view-section.active');
            if (activeView) {
                activeView.querySelector('.global-load-error')?.remove();

                const feedback = document.createElement('div');
                feedback.className = 'global-load-error';
                feedback.setAttribute('role', 'alert');

                const message = document.createElement('span');
                message.textContent = 'Não foi possível carregar os dados. Verifique a conexão e tente novamente.';

                const retryButton = document.createElement('button');
                retryButton.type = 'button';
                retryButton.className = 'btn-ghost';
                retryButton.textContent = 'Tentar novamente';
                retryButton.addEventListener('click', () => {
                    feedback.remove();
                    this.carregarDadosGlobais();
                });

                feedback.append(message, retryButton);
                activeView.prepend(feedback);
            }
        }
    }

    async carregarMaisPosts(reset = false) {
        if (this.carregandoMaisPosts) return;
        if (!reset && !this.temMaisPosts) return;

        if (reset) {
            this.paginaFeed = 0;
            this.posts = [];
            this.temMaisPosts = true;
        }

        this.carregandoMaisPosts = true;
        const container = document.getElementById('feed-container');

        let loadingIndicator = document.getElementById('feed-loading-indicator');
        if (!loadingIndicator && container && container.parentNode) {
            loadingIndicator = document.createElement('div');
            loadingIndicator.id = 'feed-loading-indicator';
            loadingIndicator.style.cssText = 'text-align:center; padding:16px; color:var(--text-muted); font-size:14px;';
            loadingIndicator.innerHTML = '<span style="display:inline-block; width:14px; height:14px; border:2px solid var(--accent); border-top-color:transparent; border-radius:50%; animation:spin 1s linear infinite; vertical-align:middle; margin-right:8px;"></span>Carregando publicações...';
            container.parentNode.insertBefore(loadingIndicator, container.nextSibling);
        }
        if (loadingIndicator) loadingIndicator.style.display = 'block';

        try {
            const novosPosts = await ApiService.listarPosts(this.paginaFeed, this.tamanhoPaginaFeed);
            const postsArray = Array.isArray(novosPosts) ? novosPosts : (novosPosts.content || []);

            if (postsArray.length < this.tamanhoPaginaFeed) {
                this.temMaisPosts = false;
                if (loadingIndicator) {
                    loadingIndicator.innerHTML = (this.posts.length + postsArray.length > 0)
                        ? '<p style="color:var(--text-muted); font-size:13px; margin:16px 0;">Você chegou ao fim das publicações.</p>'
                        : '';
                }
            } else if (loadingIndicator) {
                loadingIndicator.style.display = 'none';
            }

            if (reset) {
                this.posts = postsArray;
            } else {
                const idsExistentes = new Set(this.posts.map(p => p.idPost || p.id));
                const postsFiltrados = postsArray.filter(p => !idsExistentes.has(p.idPost || p.id));
                this.posts = [...this.posts, ...postsFiltrados];
            }

            this.paginaFeed++;
            this.renderizarFeedGeral();
        } catch (erro) {
            console.error("Erro ao carregar posts paginados:", erro);
            if (loadingIndicator) loadingIndicator.style.display = 'none';
        } finally {
            this.carregandoMaisPosts = false;
        }
    }

    // --- LÓGICA DE SUBREDDIT E COMUNIDADES ---
    async carregarComunidades() {
        const container = document.getElementById('comunidades-container');
        try {
            const comunidades = await ApiService.listarComunidades();
            this.listaComunidades = comunidades;
            this.renderizarComunidades(comunidades);
        } catch (e) {
            container.innerHTML = "<div class='comunidade-empty'>Nenhuma comunidade pôde ser carregada.</div>";
        }
    }

    renderizarRecomendacoes() {
        const container = document.getElementById('feed-recomendados');
        if (!container) return;

        const interesses = this.getInteressesSelecionados();
        if (!interesses.length) {
            container.innerHTML = '<p class="feed-sidebar-empty">Escolha seus interesses no cadastro para receber sugestões personalizadas.</p>';
            return;
        }

        const recomendados = [];

        this.listaComunidades.forEach(comunidade => {
            const score = this.getInteresseScore(comunidade, interesses);
            if (score > 0) {
                recomendados.push({
                    tipo: 'comunidade',
                    titulo: comunidade.nome,
                    subtitulo: comunidade.categoria || 'Comunidade',
                    score,
                    id: comunidade.idComunidade || comunidade.id,
                    descricao: comunidade.descricao || 'Comunidade com afinidade ao seu perfil.'
                });
            }
        });

        this.listaEventos.forEach(evento => {
            const score = this.getInteresseScore(evento, interesses);
            if (score > 0) {
                recomendados.push({
                    tipo: 'evento',
                    titulo: evento.titulo,
                    subtitulo: evento.categoria || 'Evento',
                    score,
                    id: evento.id,
                    descricao: evento.localEvento || 'Evento com perfil parecido com você.'
                });
            }
        });

        recomendados.sort((a, b) => b.score - a.score);

        const itens = recomendados.slice(0, 4);

        if (!itens.length) {
            container.innerHTML = '<p class="feed-sidebar-empty">Ainda não temos sugestões estreitas para seus interesses.</p>';
            return;
        }

        container.innerHTML = itens.map(item => `
            <button class="feed-recomendado-item" onclick="${item.tipo === 'evento' ? `app.abrirDetalhesEvento(${item.id})` : `app.entrarSubreddit(${item.id})`}" type="button">
                <span>${this.escaparHtmlDestaque(item.subtitulo)}</span>
                <strong>${this.escaparHtmlDestaque(item.titulo)}</strong>
                <small>${this.escaparHtmlDestaque(item.descricao)}</small>
            </button>
        `).join('');
    }

    async carregarUsuariosSeguidos() {
        if (!ApiService.getIdUsuarioLogado()) {
            this.usuariosSeguidos.clear();
            this.erroCarregarUsuariosSeguidos = false;
            this.mensagemErroUsuariosSeguidos = '';
            return;
        }

        try {
            const idsSeguidos = await ApiService.listarUsuariosSeguidos();
            if (!Array.isArray(idsSeguidos)) {
                throw new Error('A resposta de pessoas seguidas não é uma lista.');
            }
            this.usuariosSeguidos = new Set(idsSeguidos.map(Number));
            this.erroCarregarUsuariosSeguidos = false;
            this.mensagemErroUsuariosSeguidos = '';
        } catch (erro) {
            console.error('Não foi possível carregar as pessoas seguidas:', erro);
            this.erroCarregarUsuariosSeguidos = true;
            this.mensagemErroUsuariosSeguidos = erro.message
                || 'Não foi possível carregar as pessoas que você segue.';
        }
    }

    async alternarSeguirUsuario(idUsuario) {
        const id = Number(idUsuario);
        if (!Number.isSafeInteger(id) || id <= 0) return;
        if (!ApiService.getIdUsuarioLogado()) {
            this.abrirModal('auth-modal');
            this.mostrarAuthFeedback('Entre na sua conta para seguir pessoas.');
            return;
        }
        if (this.usuariosSeguirEmAtualizacao.has(id)) return;

        const deixarDeSeguir = this.usuariosSeguidos.has(id);
        this.usuariosSeguirEmAtualizacao.add(id);
        this.renderizarSugestoesPessoas();
        this.atualizarBotoesSeguirUsuario(id);

        try {
            const resposta = deixarDeSeguir
                ? await ApiService.deixarDeSeguirUsuario(id)
                : await ApiService.seguirUsuario(id);
            if (!resposta.ok) {
                throw new Error(await this.mensagemErroApi(
                    resposta,
                    deixarDeSeguir
                        ? 'Não foi possível deixar de seguir esta pessoa.'
                        : 'Não foi possível seguir esta pessoa.'
                ));
            }

            if (deixarDeSeguir) {
                this.usuariosSeguidos.delete(id);
            } else {
                this.usuariosSeguidos.add(id);
            }
            this.erroCarregarUsuariosSeguidos = false;
            this.mensagemErroUsuariosSeguidos = '';
            this.mostrarFeedbackGlobal(
                deixarDeSeguir ? 'Você deixou de seguir esta pessoa.' : 'Você começou a seguir esta pessoa.'
            );
        } catch (erro) {
            console.error('Erro ao atualizar pessoa seguida:', erro);
            this.mostrarFeedbackGlobal(erro.message || 'Não foi possível atualizar as pessoas seguidas.');
        } finally {
            this.usuariosSeguirEmAtualizacao.delete(id);
            this.renderizarSugestoesPessoas();
            this.atualizarBotoesSeguirUsuario(id);
        }
    }

    atualizarBotoesSeguirUsuario(idUsuario) {
        const seguindo = this.usuariosSeguidos.has(idUsuario);
        const atualizando = this.usuariosSeguirEmAtualizacao.has(idUsuario);
        document.querySelectorAll(`[data-follow-user-id="${idUsuario}"]`).forEach(botao => {
            botao.disabled = atualizando;
            botao.setAttribute('aria-busy', String(atualizando));
            botao.setAttribute('aria-pressed', String(seguindo));
            botao.classList.toggle('seguindo', seguindo);
            if (botao.classList.contains('btn-perfil-seguir')) {
                botao.innerHTML = `
                    <i class="material-icons" aria-hidden="true">${seguindo ? 'check' : 'person_add'}</i>
                    ${seguindo ? 'Seguindo' : 'Seguir'}
                `;
            } else {
                botao.textContent = atualizando ? 'Atualizando...' : seguindo ? 'Seguindo' : 'Seguir';
            }
        });
    }

    renderizarSugestoesPessoas() {
        const container = document.getElementById('feed-sugestoes-pessoas');
        if (!container) return;

        const usuarioLogado = ApiService.getUsuarioLogado();
        const usuarioAtualId = usuarioLogado ? (usuarioLogado.idUsuario || usuarioLogado.id) : null;
        const interesses = this.getInteressesSelecionados();

        if (usuarioLogado && this.erroCarregarUsuariosSeguidos) {
            container.innerHTML = `
                <p class="feed-sidebar-empty">${this.escaparHtmlDestaque(this.mensagemErroUsuariosSeguidos)}</p>
                <button class="btn-ghost" type="button"
                    onclick="app.carregarUsuariosSeguidos().then(() => app.renderizarSugestoesPessoas())">
                    Tentar novamente
                </button>
            `;
            return;
        }

        const candidatos = this.listaUsuariosCompleta
            .filter(usuario => {
                const id = usuario.idUsuario || usuario.id;
                if (!id || Number(id) === Number(usuarioAtualId)) return false;

                const comunidades = this.listaComunidades.filter(comunidade =>
                    (comunidade.membros || []).some(membro => (membro.idUsuario || membro.id) == id)
                );
                const eventos = this.listaEventos.filter(evento => evento.criadorId == id);
                const textoPerfil = [
                    usuario.nome,
                    usuario.username,
                    usuario.bio,
                    ...comunidades.map(comunidade => `${comunidade.nome} ${comunidade.categoria || ''}`),
                    ...eventos.map(evento => `${evento.titulo} ${evento.categoria || ''}`),
                ].join(' ').toLowerCase();

                let score = 0;
                if (interesses.length) {
                    interesses.forEach(interesse => {
                        if (textoPerfil.includes(interesse.toLowerCase())) score += 2;
                    });
                }

                score += comunidades.length;
                score += eventos.length;
                return score > 0;
            })
            .map(usuario => {
                const id = usuario.idUsuario || usuario.id;
                const comunidades = this.listaComunidades.filter(comunidade =>
                    (comunidade.membros || []).some(membro => (membro.idUsuario || membro.id) == id)
                );
                const eventos = this.listaEventos.filter(evento => evento.criadorId == id);
                const categorias = [...new Set([
                    ...comunidades.map(comunidade => comunidade.categoria || 'Geral'),
                    ...eventos.map(evento => evento.categoria || 'Geral')
                ])];
                let score = 0;
                if (interesses.length) {
                    const textoPerfil = `${usuario.nome} ${usuario.bio || ''} ${usuario.username || ''} ${categorias.join(' ')}`.toLowerCase();
                    interesses.forEach(interesse => {
                        if (textoPerfil.includes(interesse.toLowerCase())) score += 2;
                    });
                }
                score += comunidades.length * 2;
                score += eventos.length * 2;
                return { usuario, score };
            })
            .sort((a, b) => b.score - a.score)
            .slice(0, 4);

        if (!candidatos.length) {
            container.innerHTML = '<p class="feed-sidebar-empty">Ainda não há perfis parecidos para sugerir no momento.</p>';
            return;
        }

        container.innerHTML = candidatos.map(({ usuario }) => {
            const id = usuario.idUsuario || usuario.id;
            const seguindo = this.usuariosSeguidos.has(Number(id));
            const atualizando = this.usuariosSeguirEmAtualizacao.has(Number(id));
            return `
                <div class="feed-pessoa-item">
                    <div class="feed-pessoa-item-top">
                        <div class="avatar avatar-small">${this.escaparHtmlDestaque((usuario.nome || 'U').charAt(0).toUpperCase())}</div>
                        <div>
                            <strong>${this.escaparHtmlDestaque(usuario.nome || 'Usuário')}</strong>
                            <span>@${this.escaparHtmlDestaque(usuario.username || 'usuario')}</span>
                        </div>
                    </div>
                    <p>${this.escaparHtmlDestaque((usuario.bio || 'Interessado em viver experiências incríveis.').slice(0, 72))}</p>
                    <button class="feed-pessoa-btn ${seguindo ? 'seguindo' : ''}"
                        data-follow-user-id="${Number(id)}"
                        aria-pressed="${seguindo}"
                        aria-busy="${atualizando}"
                        onclick="app.alternarSeguirUsuario(${Number(id)})"
                        type="button" ${atualizando ? 'disabled' : ''}>
                        ${atualizando ? 'Atualizando...' : !usuarioLogado ? 'Entrar para seguir' : seguindo ? 'Seguindo' : 'Seguir'}
                    </button>
                </div>
            `;
        }).join('');
    }

    renderizarComunidades(comunidades) {

        const container =
            document.getElementById(
                'comunidades-container'
            );

        if (!container) return;


        if (!comunidades.length) {

            let mensagem =
                'Nenhuma comunidade encontrada.';

            if (
                this.filtroComunidades ===
                'minhas'
            ) {
                mensagem =
                    'Você ainda não participa de nenhuma comunidade.';
            }

            if (
                this.filtroComunidades ===
                'criadas'
            ) {
                mensagem =
                    'Você ainda não criou nenhuma comunidade.';
            }

            container.innerHTML = `
            <div class="comunidade-empty">

                <div class="comunidade-empty-icon">
                    <i class="material-icons">
                        groups
                    </i>
                </div>

                <h3>
                    ${mensagem}
                </h3>

                <p>
                    Explore comunidades ou crie
                    um espaço para reunir pessoas
                    com os mesmos interesses.
                </p>

            </div>
        `;

            return;
        }


        const eventosPorComunidade =
            this.listaEventos.reduce(
                (total, evento) => {

                    if (evento.comunidadeId) {

                        total[evento.comunidadeId] =
                            (
                                total[
                                evento.comunidadeId
                                ] || 0
                            ) + 1;
                    }

                    return total;
                },
                {}
            );


        container.innerHTML = '';


        comunidades.forEach(comunidade => {

            const idCom =
                comunidade.idComunidade ||
                comunidade.id;

            const idUsuario =
                ApiService.getIdUsuarioLogado();

            const idAdministrador =
                comunidade.criador?.idUsuario ||
                comunidade.criador?.id;

            const ehAdministrador =
                String(idAdministrador) ===
                String(idUsuario);

            const participa =
                this.usuarioParticipaComunidade(
                    comunidade
                );

            const quantidadeMembros =
                comunidade.membros?.length || 0;

            const quantidadeEventos =
                eventosPorComunidade[idCom] || 0;


            const div =
                document.createElement('article');

            div.className =
                'comunidade-card';

            div.tabIndex = 0;

            div.setAttribute(
                'role',
                'link'
            );

            div.style.setProperty(
                '--community-color',
                comunidade.cor || '#EA3F74'
            );


            const imagemComunidade =
                comunidade.imagemComunidade
                    ? `
                    style="
                        background-image:
                        url('${this.urlCapaEvento(
                        comunidade.imagemComunidade
                    )}')
                    "
                `
                    : '';


            const statusHtml =
                ehAdministrador
                    ? `
                    <span class="
                        comunidade-status
                        administrando
                    ">
                        <i class="material-icons">
                            admin_panel_settings
                        </i>

                        Você administra
                    </span>
                `
                    : participa
                        ? `
                        <span class="
                            comunidade-status
                            participando
                        ">
                            <i class="material-icons">
                                check_circle
                            </i>

                            Participando
                        </span>
                    `
                        : `
                        <span class="
                            comunidade-status
                            descoberta
                        ">
                            Descobrir
                        </span>
                    `;


            div.innerHTML = `

            <div class="comunidade-card-top">

                <div
                    class="
                        comunidade-mark
                        ${comunidade.imagemComunidade
                    ? 'has-image'
                    : ''
                }
                    "
                    ${imagemComunidade}
                >
                    ${!comunidade.imagemComunidade
                    ? this.escaparHtmlDestaque(
                        (
                            comunidade.nome ||
                            'C'
                        )
                            .charAt(0)
                            .toUpperCase()
                    )
                    : ''
                }
                </div>

                <span class="comunidade-categoria">
                    ${this.escaparHtmlDestaque(
                    comunidade.categoria ||
                    'Geral'
                )}
                </span>

            </div>


            <div class="comunidade-card-content">

                <h2>
                    ${this.escaparHtmlDestaque(
                    comunidade.nome ||
                    'Comunidade'
                )}
                </h2>

                <p>
                    ${this.escaparHtmlDestaque(
                    comunidade.descricao ||
                    'Uma comunidade para trocar ideias e viver experiências.'
                )}
                </p>

            </div>


            <div class="comunidade-stats">

                <span>
                    <i class="material-icons">
                        group
                    </i>

                    ${quantidadeMembros}
                    ${quantidadeMembros === 1
                    ? 'membro'
                    : 'membros'
                }
                </span>

                <span>
                    <i class="material-icons">
                        event
                    </i>

                    ${quantidadeEventos}
                    ${quantidadeEventos === 1
                    ? 'evento'
                    : 'eventos'
                }
                </span>

            </div>


            <div class="comunidade-card-footer">

                <div class="comunidade-card-footer-info">

                    ${statusHtml}

                    <small>
                        Por
                        ${this.escaparHtmlDestaque(
                    comunidade.criador?.nome ||
                    'Administrador'
                )}
                    </small>

                </div>


                ${ehAdministrador
                    ? `
                            <button
                                type="button"
                                class="comunidade-gerenciar-btn"
                                onclick="
                                    app.gerenciarComunidade(
                                        ${idCom}
                                    )
                                "
                            >
                                Gerenciar
                            </button>
                        `
                    : `
                            <span
                                class="
                                    comunidade-card-arrow
                                "
                            >
                                <i class="material-icons">
                                    arrow_forward
                                </i>
                            </span>
                        `
                }

            </div>
        `;


            div.addEventListener(
                'click',
                evento => {

                    if (
                        evento.target.closest('button')
                    ) {
                        return;
                    }

                    this.entrarSubreddit(idCom);
                }
            );


            div.addEventListener(
                'keydown',
                evento => {

                    if (
                        evento.key !== 'Enter' &&
                        evento.key !== ' '
                    ) {
                        return;
                    }

                    evento.preventDefault();

                    this.entrarSubreddit(idCom);
                }
            );


            container.appendChild(div);
        });
    }

    usuarioParticipaComunidade(comunidade) {

        const idUsuario =
            ApiService.getIdUsuarioLogado();

        if (!idUsuario || !comunidade) {
            return false;
        }

        const idCriador =
            comunidade.criador?.idUsuario ||
            comunidade.criador?.id;

        const ehCriador =
            String(idCriador) ===
            String(idUsuario);

        const ehMembro =
            (comunidade.membros || [])
                .some(membro => {

                    const idMembro =
                        membro.idUsuario ||
                        membro.id;

                    return String(idMembro) ===
                        String(idUsuario);
                });

        return ehCriador || ehMembro;
    }

    filtrarComunidades() {

        const termo =
            document
                .getElementById('comunidades-search')
                ?.value
                .trim()
                .toLowerCase() || '';

        const ordem =
            document
                .getElementById('comunidades-ordenacao')
                ?.value || 'membros';

        const idUsuario =
            ApiService.getIdUsuarioLogado();

        let comunidades =
            [...(this.listaComunidades || [])];


        // =========================
        // BUSCA
        // =========================
        if (termo) {

            comunidades =
                comunidades.filter(comunidade => {

                    const texto = `
                    ${comunidade.nome || ''}
                    ${comunidade.descricao || ''}
                    ${comunidade.categoria || ''}
                `.toLowerCase();

                    return texto.includes(termo);
                });
        }


        // =========================
        // ABAS
        // =========================
        if (this.filtroComunidades === 'minhas') {

            comunidades =
                comunidades.filter(comunidade =>
                    this.usuarioParticipaComunidade(
                        comunidade
                    )
                );
        }


        if (this.filtroComunidades === 'criadas') {

            comunidades =
                comunidades.filter(comunidade => {

                    const idCriador =
                        comunidade.criador?.idUsuario ||
                        comunidade.criador?.id;

                    return String(idCriador) ===
                        String(idUsuario);
                });
        }


        // =========================
        // ORDENAÇÃO
        // =========================
        if (ordem === 'membros') {

            comunidades.sort(
                (a, b) =>
                    (b.membros?.length || 0) -
                    (a.membros?.length || 0)
            );
        }


        if (ordem === 'recentes') {

            comunidades.reverse();
        }


        if (ordem === 'nome') {

            comunidades.sort(
                (a, b) =>
                    (a.nome || '')
                        .localeCompare(
                            b.nome || '',
                            'pt-BR'
                        )
            );
        }


        this.renderizarComunidades(
            comunidades
        );
    }

    async criarComunidade() {
        if (!ApiService.getUsuarioLogado()) return this.mostrarFeedbackGlobal("Faça login primeiro.");

        const nome = document.getElementById('comunidade-nome').value.trim();
        const desc = document.getElementById('comunidade-desc').value.trim();
        const categoria = document.getElementById('comunidade-categoria').value;
        const cor = document.getElementById('comunidade-cor').value;
        const imagem = document.getElementById('comunidade-imagem').files[0];
        const criadorId = ApiService.getIdUsuarioLogado();

        const res = await ApiService.criarComunidadeAPI(nome, desc, criadorId, categoria, cor);

        if (res.ok) {
            const comunidadeCriada = await res.json();
            if (imagem) await ApiService.enviarImagemComunidade(comunidadeCriada.id, imagem);
            this.fecharModal('comunidade-modal');
            document.getElementById('comunidade-nome').value = '';
            document.getElementById('comunidade-desc').value = '';
            document.getElementById('comunidade-categoria').value = '';
            document.getElementById('comunidade-cor').value = '#EA3F74';
            document.getElementById('comunidade-imagem').value = '';
            this.carregarComunidades();
        } else {
            this.mostrarFeedbackGlobal("Não foi possível criar a comunidade.");
        }
    }

    entrarSubreddit(id, nome, desc) {

        this.salvarViewAtual('subreddit');

        sessionStorage.setItem(
            'comunidadeAtivaId',
            id
        );

        this.comunidadeAtivaId = id;

        const usuarioLogado =
            ApiService.getIdUsuarioLogado();

        const comunidade =
            (this.listaComunidades || [])
                .find(
                    c =>
                        (c.idComunidade || c.id) == id
                );

        if (!comunidade) {
            return;
        }


        nome =
            comunidade.nome ||
            nome ||
            'Comunidade';

        desc =
            comunidade.descricao ||
            desc ||
            '';


        const idAdministrador =
            comunidade.criador?.idUsuario ||
            comunidade.criador?.id;

        const ehAdministrador =
            String(idAdministrador) ===
            String(usuarioLogado);


        const ehMembro =
            (comunidade.membros || [])
                .some(membro => {

                    const idMembro =
                        membro.idUsuario ||
                        membro.id;

                    return String(idMembro) ===
                        String(usuarioLogado);
                });


        /*
         * Para a interface da própria comunidade,
         * o administrador também é considerado
         * participante.
         */
        this.usuarioParticipaDaComunidade =
            ehMembro ||
            ehAdministrador;


        // =========================
        // DADOS DA COMUNIDADE
        // =========================

        document.getElementById(
            'sub-titulo'
        ).innerText = nome;

        document.getElementById(
            'sub-desc'
        ).innerText = desc;


        const categoria =
            document.getElementById(
                'sub-categoria'
            );

        if (categoria) {
            categoria.innerText =
                comunidade.categoria ||
                'Geral';
        }


        // =========================
        // AVATAR / IMAGEM
        // =========================

        const visual =
            document.getElementById(
                'sub-community-visual'
            );

        if (visual) {

            visual.style.backgroundColor =
                comunidade.cor ||
                '#EA3F74';

            visual.style.backgroundImage =
                comunidade.imagemComunidade
                    ? `url('${this.urlCapaEvento(
                        comunidade.imagemComunidade
                    )}')`
                    : '';

            visual.innerText =
                comunidade.imagemComunidade
                    ? ''
                    : nome
                        .charAt(0)
                        .toUpperCase();

            visual.classList.toggle(
                'has-image',
                Boolean(
                    comunidade.imagemComunidade
                )
            );
        }


        // =========================
        // CONTAGENS
        // =========================

        const postsComunidade =
            (this.posts || [])
                .filter(post =>
                    (
                        post.idComunidade ||
                        post.id_comunidade
                    ) == id
                );

        const eventosComunidade =
            (this.listaEventos || [])
                .filter(evento =>
                    evento.comunidadeId == id
                );

        const totalMembros =
            comunidade.membros?.length || 0;


        document.getElementById(
            'sub-total-membros'
        ).innerText =
            totalMembros;

        document.getElementById(
            'sub-total-eventos'
        ).innerText =
            eventosComunidade.length;


        document.getElementById(
            'comunidade-tab-posts-count'
        ).innerText =
            postsComunidade.length;

        document.getElementById(
            'comunidade-tab-eventos-count'
        ).innerText =
            eventosComunidade.length;

        document.getElementById(
            'comunidade-tab-membros-count'
        ).innerText =
            totalMembros;


        // =========================
        // STATUS
        // =========================

        const status =
            document.getElementById(
                'sub-status'
            );

        if (status) {

            if (ehAdministrador) {

                status.className =
                    'comunidade-detalhe-status administrando';

                status.innerHTML = `
                <i class="material-icons">
                    admin_panel_settings
                </i>
                Você administra
            `;

            } else if (ehMembro) {

                status.className =
                    'comunidade-detalhe-status participando';

                status.innerHTML = `
                <i class="material-icons">
                    check_circle
                </i>
                Participando
            `;

            } else {

                status.className =
                    'comunidade-detalhe-status';

                status.innerHTML = '';
            }
        }


        // =========================
        // BOTÕES
        // =========================

        const btnParticipar =
            document.getElementById(
                'btn-participar'
            );

        const btnGerenciar =
            document.getElementById(
                'btn-gerenciar-comunidade'
            );

        const btnPostar =
            document.getElementById(
                'btn-postar-comunidade'
            );


        if (ehAdministrador) {

            btnParticipar.style.display =
                'none';

            btnGerenciar.style.display =
                'inline-flex';

            btnPostar.style.display =
                'inline-flex';

        } else {

            btnGerenciar.style.display =
                'none';

            btnParticipar.style.display =
                'inline-flex';

            btnParticipar.classList.toggle(
                'sair',
                ehMembro
            );

            btnParticipar.innerHTML =
                ehMembro
                    ? `
                    <i class="material-icons">
                        logout
                    </i>
                    Sair da comunidade
                `
                    : `
                    <i class="material-icons">
                        group_add
                    </i>
                    Entrar
                `;

            /*
             * Só membro pode postar.
             */
            btnPostar.style.display =
                ehMembro
                    ? 'inline-flex'
                    : 'none';
        }


        // =========================
        // MOSTRA VIEW
        // =========================

        document
            .querySelectorAll('.view-section')
            .forEach(el =>
                el.classList.remove('active')
            );

        document
            .getElementById('view-subreddit')
            .classList.add('active');


        this.mudarAbaComunidade('posts');
    }

    mudarAbaComunidade(aba) {

        document
            .querySelectorAll('.comunidade-tab')
            .forEach(tab =>
                tab.classList.remove('active')
            );

        document
            .getElementById(
                `comunidade-tab-${aba}`
            )
            ?.classList.add('active');


        if (aba === 'posts') {

            this.renderizarSubreddit(
                this.comunidadeAtivaId
            );

            return;
        }


        if (aba === 'membros') {

            const comunidade =
                this.listaComunidades.find(
                    c =>
                        (
                            c.idComunidade ||
                            c.id
                        ) ==
                        this.comunidadeAtivaId
                );

            this.montarMembrosComunidade(
                comunidade?.membros || []
            );

            return;
        }


        if (aba === 'eventos') {

            this.renderizarEventosComunidade();

        }
    }

    montarMembrosComunidade(membros) {

        const container =
            document.getElementById(
                'subreddit-feed-container'
            );

        if (!container) return;


        if (!membros.length) {

            container.innerHTML = `
            <div class="comunidade-aba-empty">

                <i class="material-icons">
                    group_off
                </i>

                <h3>
                    Nenhum membro encontrado
                </h3>

            </div>
        `;

            return;
        }


        container.innerHTML = `
        <div class="comunidade-membros-grid">

            ${membros.map(membro => {

            const id =
                membro.idUsuario ||
                membro.id;

            const foto =
                this.usuariosFotosMap[id];

            const urlFoto =
                ApiService.formatarUrlFotoPerfil(
                    foto
                );

            const inicial =
                (
                    membro.nome ||
                    'U'
                )
                    .charAt(0)
                    .toUpperCase();


            const avatar =
                urlFoto
                    ? `
                            <img
                                src="${this.escaparHtmlDestaque(urlFoto)}"
                                class="
                                    avatar
                                    comunidade-membro-avatar
                                "
                                alt=""
                                onerror="this.remove()"
                            >
                        `
                    : `
                            <div
                                class="
                                    avatar
                                    comunidade-membro-avatar
                                "
                            >
                                ${this.escaparHtmlDestaque(
                        inicial
                    )}
                            </div>
                        `;


            return `
                    <article
                        class="comunidade-membro-card"
                        tabindex="0"
                        role="link"
                        onclick="
                            app.abrirPerfil(${id})
                        "
                        onkeydown="
                            if (
                                event.key === 'Enter' ||
                                event.key === ' '
                            ) {
                                event.preventDefault();
                                app.abrirPerfil(${id});
                            }
                        "
                    >

                        ${avatar}

                        <div class="
                            comunidade-membro-info
                        ">

                            <strong>
                                ${this.escaparHtmlDestaque(
                membro.nome ||
                'Usuário'
            )}
                            </strong>

                            <span>
                                @${this.escaparHtmlDestaque(
                membro.username ||
                'usuario'
            )}
                            </span>

                        </div>


                        <i class="
                            material-icons
                            comunidade-membro-arrow
                        ">
                            chevron_right
                        </i>

                    </article>
                `;
        }).join('')}

        </div>
    `;
    }

    async participarComunidade() {
        const idLogado = ApiService.getIdUsuarioLogado();

        if (!idLogado) {
            return this.mostrarFeedbackGlobal("Faça login primeiro.");
        }

        const btn = document.getElementById('btn-participar');

        try {
            let resposta;

            // Se já participa, está tentando SAIR
            if (this.usuarioParticipaDaComunidade) {

                resposta = await ApiService.removerMembro(
                    this.comunidadeAtivaId,
                    idLogado,
                    idLogado
                );

            } else {

                // Se ainda não participa, está tentando ENTRAR
                resposta = await ApiService.request(
                    `${ApiService.API_URL}/comunidades/${this.comunidadeAtivaId}/participar/${idLogado}`,
                    {
                        method: "POST"
                    }
                );
            }

            if (!resposta.ok) {
                this.mostrarFeedbackGlobal("Não foi possível realizar esta ação.");
                return;
            }
            await this.carregarComunidades();

            this.entrarSubreddit(
                this.comunidadeAtivaId
            );

        } catch (e) {
            console.error(e);
            this.mostrarFeedbackGlobal("Erro ao atualizar participação na comunidade.");
        }
    }

    renderizarSubreddit(idComunidade) {
        // Filtra os posts para mostrar apenas os desta comunidade
        const postsDestaComunidade = this.posts.filter(p => p.idComunidade == idComunidade || p.id_comunidade == idComunidade);
        this.montarCardsPosts(postsDestaComunidade, 'subreddit-feed-container', false);
    }

    obterMinhasComunidades() {

        const idUsuario =
            ApiService.getIdUsuarioLogado();

        if (!idUsuario) {
            return [];
        }

        return (this.listaComunidades || [])
            .filter(comunidade => {

                const membros =
                    comunidade.membros || [];

                const ehMembro =
                    membros.some(membro => {

                        const idMembro =
                            membro.idUsuario ||
                            membro.id;

                        return String(idMembro) ===
                            String(idUsuario);
                    });

                const idCriador =
                    comunidade.criador?.idUsuario ||
                    comunidade.criador?.id;

                const ehCriador =
                    String(idCriador) ===
                    String(idUsuario);

                return ehMembro || ehCriador;
            });
    }

    renderizarFeedGeral() {

        let posts = [...this.posts];

        const idUsuario =
            ApiService.getIdUsuarioLogado();


        // =========================
        // FILTRO DE COMUNIDADES
        // =========================
        if (this.filtroFeed === 'comunidades') {

            const idsMinhasComunidades =
                this.obterIdsMinhasComunidades();

            posts = posts.filter(post => {

                const idComunidade =
                    post.idComunidade ||
                    post.id_comunidade;

                if (!idComunidade) {
                    return false;
                }

                // Só comunidades das quais participa
                if (
                    !idsMinhasComunidades.has(
                        String(idComunidade)
                    )
                ) {
                    return false;
                }

                // Se escolheu um chip específico
                if (
                    this.comunidadeFeedSelecionada !== null
                ) {
                    return (
                        String(idComunidade) ===
                        String(
                            this.comunidadeFeedSelecionada
                        )
                    );
                }

                return true;
            });
        }


        // =========================
        // MEUS POSTS
        // =========================
        if (this.filtroFeed === 'meus') {

            posts = posts.filter(post =>
                (
                    post.idUsuario ||
                    post.id_usuario
                ) == idUsuario
            );
        }


        // =========================
        // ORDENAÇÃO
        // =========================
        if (
            this.ordenacaoFeed === 'comentados'
        ) {

            posts.sort((a, b) => {

                const idA =
                    a.idPost || a.id;

                const idB =
                    b.idPost || b.id;

                return (
                    (
                        this.comentariosPorPost[idB] ||
                        []
                    ).length
                    -
                    (
                        this.comentariosPorPost[idA] ||
                        []
                    ).length
                );
            });
        }


        if (
            this.ordenacaoFeed === 'curtidos'
        ) {

            posts.sort((a, b) =>
                this.contarVotos(
                    b.idPost || b.id
                )
                -
                this.contarVotos(
                    a.idPost || a.id
                )
            );
        }


        // Atualiza elementos auxiliares
        this.renderizarFiltrosComunidadesFeed();
        this.atualizarSidebarFeed();


        // Estado vazio especial
        if (
            this.filtroFeed === 'comunidades' &&
            !this.obterMinhasComunidades().length
        ) {

            const container =
                document.getElementById(
                    'feed-container'
                );

            if (container) {
                container.innerHTML = `
                <div class="feed-comunidades-empty">

                    <div class="feed-empty-icon">
                        <i class="material-icons">
                            groups
                        </i>
                    </div>

                    <h3>
                        Encontre sua comunidade
                    </h3>

                    <p>
                        Você ainda não participa de
                        nenhuma comunidade.
                    </p>

                    <button
                        type="button"
                        class="btn-primary"
                        onclick="
                            app.mudarAba('comunidades')
                        "
                    >
                        Explorar comunidades
                    </button>

                </div>
            `;
            }

            return;
        }


        this.montarCardsPosts(
            posts,
            'feed-container',
            true
        );
    }

    mudarFiltroFeed(filtro) {

        this.filtroFeed = filtro;

        // Ao sair da aba Comunidades,
        // limpa o filtro interno.
        if (filtro !== 'comunidades') {
            this.comunidadeFeedSelecionada = null;
        }

        document
            .querySelectorAll('.feed-tab')
            .forEach(tab =>
                tab.classList.remove('active')
            );

        document
            .getElementById(
                `feed-tab-${filtro}`
            )
            ?.classList.add('active');

        this.renderizarFeedGeral();
    }

    mudarOrdenacaoFeed(ordenacao) {
        this.ordenacaoFeed = ordenacao;
        this.renderizarFeedGeral();
    }

    contarVotos(idPost) {
        return this.votos.filter(voto => voto.idPost == idPost && voto.tipo === 'like').length;
    }

    votoDoUsuario(idPost) {
        const idUsuario = ApiService.getIdUsuarioLogado();
        return this.votos.find(voto => voto.idPost == idPost && voto.idUsuario == idUsuario && voto.tipo === 'like');
    }

    postEstaSalvo(idPost) {
        const idUsuario = ApiService.getIdUsuarioLogado();
        return this.postsSalvos.find(post => post.idPost == idPost && post.idUsuario == idUsuario);
    }

    async alternarCurtida(idPost) {
        const idUsuario = ApiService.getIdUsuarioLogado();
        if (!idUsuario) return this.mostrarFeedbackGlobal('Faça login para curtir publicações.');
        const voto = this.votoDoUsuario(idPost);
        const resposta = voto
            ? await ApiService.deletarVoto(voto.idVoto)
            : await ApiService.criarVoto(idUsuario, idPost);
        if (!resposta.ok) return this.mostrarFeedbackGlobal('Não foi possível atualizar a curtida.');
        this.votos = voto ? this.votos.filter(item => item.idVoto !== voto.idVoto) : [...this.votos, await resposta.json()];
        this.renderizarFeedGeral();
    }

    async alternarPostSalvo(idPost) {
        const idUsuario = ApiService.getIdUsuarioLogado();
        if (!idUsuario) return this.mostrarFeedbackGlobal('Faça login para salvar publicações.');
        const salvo = this.postEstaSalvo(idPost);
        const resposta = salvo
            ? await ApiService.removerPostSalvo(salvo.id)
            : await ApiService.salvarPost(idUsuario, idPost);
        if (!resposta.ok) return this.mostrarFeedbackGlobal('Não foi possível atualizar os salvos.');
        this.postsSalvos = salvo ? this.postsSalvos.filter(item => item.id !== salvo.id) : [...this.postsSalvos, await resposta.json()];
        this.renderizarFeedGeral();
        if (
            document
                .getElementById('view-salvos')
                ?.classList.contains('active')
        ) {
            this.renderizarItensSalvos();
        }
    }

    async carregarEventosFeed() {
        const container = document.getElementById('feed-eventos-destaque');
        if (!container) return;
        try {
            const resultado = await ApiService.buscarEventos({ page: 0, size: 6 });
            const eventos = this.ordenarPorInteresses(resultado.content || []).slice(0, 3);
            container.innerHTML = eventos.length ? eventos.map(evento => `<button class="feed-evento-item" onclick="app.abrirDetalhesEvento(${evento.id})"><span>${evento.dataEvento ? evento.dataEvento.split('-').reverse().join('/') : '--'}</span><strong>${this.escaparHtmlDestaque(evento.titulo)}</strong><small>${this.escaparHtmlDestaque(evento.localEvento || 'Local a confirmar')}</small></button>`).join('') : '<p class="feed-sidebar-empty">Nenhum evento agendado.</p>';
        } catch (erro) {
            container.innerHTML = '<p class="feed-sidebar-empty">Agenda indisponível no momento.</p>';
        }
    }

    // --- FUNÇÕES DE POSTS/COMENTARIOS REUTILIZÁVEIS ---
    // O post é criado primeiro para obter seu ID; cada arquivo é enviado depois em multipart associado a esse ID.
    async enviarPost() {
        const idLogado = ApiService.getIdUsuarioLogado();
        const titulo = document.getElementById('post-titulo').value.trim();
        const conteudo = document.getElementById('post-conteudo').value.trim();
        const comunidadeSelecionada = document.getElementById('post-comunidade-id')?.value;
        const inputImagens = document.getElementById('post-imagens');
        const imagens = [...(inputImagens?.files || [])];
        const botaoPublicar = document.getElementById('post-publicar-btn');

        if (!idLogado) return this.mostrarFeedbackGlobal('Entre para publicar.');
        if (imagens.length > 4) return this.mostrarFeedbackGlobal('Escolha no máximo quatro imagens.');
        if (imagens.some(imagem => !['image/jpeg', 'image/png', 'image/webp'].includes(imagem.type))) {
            return this.mostrarFeedbackGlobal('Use imagens JPG, PNG ou WEBP.');
        }
        if (imagens.some(imagem => imagem.size > 5 * 1024 * 1024)) {
            return this.mostrarFeedbackGlobal('Cada imagem deve ter no máximo 5 MB.');
        }
        if (imagens.length && typeof ApiService.enviarImagemPost !== 'function') {
            return this.mostrarFeedbackGlobal('Atualize a página para habilitar o envio de imagens.');
        }

        if (botaoPublicar) {
            botaoPublicar.disabled = true;
            botaoPublicar.textContent = 'Publicando...';
        }

        try {
            const resposta = await ApiService.criarPost(
                titulo,
                conteudo,
                idLogado,
                comunidadeSelecionada || null
            );
            if (!resposta.ok) {
                return this.mostrarFeedbackGlobal('Não foi possível publicar o post.');
            }

            const post = await resposta.json();
            const idPost = post.idPost || post.id;
            const falhasUpload = [];

            for (const [indice, imagem] of imagens.entries()) {
                try {
                    const respostaImagem = await ApiService.enviarImagemPost(idPost, imagem);
                    if (!respostaImagem.ok) {
                        falhasUpload.push(`${imagem.name}: ${await respostaImagem.text()}`);
                    }
                } catch (erroUpload) {
                    falhasUpload.push(`${imagem.name}: ${erroUpload.message}`);
                }
            }

            this.fecharModal('post-modal');
            document.getElementById('post-titulo').value = '';
            document.getElementById('post-conteudo').value = '';
            document.getElementById('post-comunidade-id').value = '';
            if (inputImagens) inputImagens.value = '';
            this.limparPrevisualizacoesPost();
            await this.carregarDadosGlobais();

            if (falhasUpload.length) {
                this.mostrarFeedbackGlobal(`O post foi publicado, mas ${falhasUpload.length} imagem(ns) não foram enviadas:\n${falhasUpload.join('\n')}`);
            }
        } catch (erro) {
            console.error('Erro ao publicar post:', erro);
            this.mostrarFeedbackGlobal('Não foi possível publicar. Verifique a conexão e tente novamente.');
        } finally {
            if (botaoPublicar) {
                botaoPublicar.disabled = false;
                botaoPublicar.textContent = 'Publicar';
            }
        }
    }

    previsualizarFotoPerfil(foto) {
        if (!foto) return;
        if (!['image/jpeg', 'image/png', 'image/webp'].includes(foto.type)) {
            this.mostrarFeedbackGlobal('Use uma foto JPG, PNG ou WEBP.');
            return;
        }
        if (foto.size > 5 * 1024 * 1024) {
            this.mostrarFeedbackGlobal('A foto deve ter no máximo 5 MB.');
            return;
        }

        if (this.previewFotoPerfilUrl) URL.revokeObjectURL(this.previewFotoPerfilUrl);
        this.previewFotoPerfilUrl = URL.createObjectURL(foto);
        this.mostrarPreviewFotoPerfil(this.previewFotoPerfilUrl);
    }

    mostrarPreviewFotoPerfil(url) {
        const imagem = document.getElementById('editar-perfil-avatar-preview');
        const icone = document.getElementById('editar-perfil-avatar-icon');
        if (!imagem || !icone) return;

        if (url) {
            let urlComCacheBust = url;
            if (!url.startsWith('blob:')) {
                try {
                    const endereco = new URL(url, window.location.href);
                    endereco.searchParams.set('v', Date.now());
                    urlComCacheBust = endereco.href;
                } catch {
                    urlComCacheBust = url;
                }
            }
            imagem.src = urlComCacheBust;
            imagem.hidden = false;
            icone.hidden = true;
        } else {
            imagem.removeAttribute('src');
            imagem.hidden = true;
            icone.hidden = false;
        }
    }

    // Novas escolhas são acumuladas no estado até quatro arquivos; DataTransfer sincroniza a seleção do input.
    previsualizarImagensPost(arquivos) {
        const novasImagens = [...(arquivos || [])]
            .filter(arquivo => arquivo.type.startsWith('image/'));
        const assinaturasExistentes = new Set(
            this.imagensPostSelecionadas.map(arquivo =>
                `${arquivo.name}:${arquivo.size}:${arquivo.lastModified}`
            )
        );
        const novasUnicas = novasImagens.filter(arquivo => {
            const assinatura = `${arquivo.name}:${arquivo.size}:${arquivo.lastModified}`;
            if (assinaturasExistentes.has(assinatura)) return false;
            assinaturasExistentes.add(assinatura);
            return true;
        });

        const vagas = Math.max(0, 4 - this.imagensPostSelecionadas.length);
        if (novasUnicas.length > vagas) {
            this.mostrarFeedbackGlobal('Cada publicação aceita até quatro imagens. As primeiras disponíveis foram adicionadas.');
        }
        this.imagensPostSelecionadas.push(...novasUnicas.slice(0, vagas));

        const input = document.getElementById('post-imagens');
        if (input) {
            const transferencia = new DataTransfer();
            this.imagensPostSelecionadas.forEach(arquivo => transferencia.items.add(arquivo));
            input.files = transferencia.files;
        }

        this.renderizarPrevisualizacoesPost();
    }

    renderizarPrevisualizacoesPost() {
        this.previewPostUrls.forEach(url => URL.revokeObjectURL(url));
        this.previewPostUrls = [];
        const container = document.getElementById('post-imagens-preview');
        if (!container) return;
        container.replaceChildren();

        this.imagensPostSelecionadas.forEach((arquivo, indice) => {
            const url = URL.createObjectURL(arquivo);
            this.previewPostUrls.push(url);
            const item = document.createElement('div');
            item.className = 'post-image-preview-item';
            const imagem = document.createElement('img');
            imagem.src = url;
            imagem.alt = `Prévia: ${arquivo.name}`;
            const remover = document.createElement('button');
            remover.type = 'button';
            remover.className = 'post-image-remove';
            remover.setAttribute('aria-label', `Remover ${arquivo.name}`);
            remover.textContent = '×';
            remover.addEventListener('click', () => this.removerImagemPost(indice));
            item.append(imagem, remover);
            container.appendChild(item);
        });
    }

    removerImagemPost(indice) {
        this.imagensPostSelecionadas.splice(indice, 1);
        const input = document.getElementById('post-imagens');
        if (input) {
            const transferencia = new DataTransfer();
            this.imagensPostSelecionadas.forEach(arquivo => transferencia.items.add(arquivo));
            input.files = transferencia.files;
        }
        this.renderizarPrevisualizacoesPost();
    }

    limparPrevisualizacoesPost() {
        this.previewPostUrls.forEach(url => URL.revokeObjectURL(url));
        this.previewPostUrls = [];
        this.imagensPostSelecionadas = [];
        const input = document.getElementById('post-imagens');
        if (input) input.value = '';
        const container = document.getElementById('post-imagens-preview');
        if (container) container.replaceChildren();
    }

    // Converte os caminhos devolvidos pela API em URLs e gera a galeria usada no feed e no detalhe do post.
    renderizarImagensPost(idPost) {
        const imagens = (this.imagensPorPost[idPost] || []).slice(0, 4);
        const elementos = imagens.map((imagem, indice) => {
            const url = ApiService.formatarUrlFotoPerfil(imagem.url);
            if (!url) return '';

            try {
                const urlSegura = new URL(url, window.location.href);
                if (!['http:', 'https:'].includes(urlSegura.protocol)) return '';
                return `<img src="${this.escaparHtmlDestaque(urlSegura.href)}" alt="Imagem ${indice + 1} da publicação" loading="lazy">`;
            } catch {
                return '';
            }
        }).filter(Boolean);

        return elementos.length
            ? `<div class="post-images" role="group" aria-label="Imagens da publicação">${elementos.join('')}</div>`
            : '';
    }

    async enviarComentario(idPost, botao) {
        const idLogado = ApiService.getIdUsuarioLogado();

        if (!idLogado) {
            return this.mostrarFeedbackGlobal("Faça login primeiro!");
        }

        const input = botao.previousElementSibling;
        const conteudo = input.value.trim();

        if (!conteudo) {
            return this.mostrarFeedbackGlobal("Digite um comentário antes de enviar.");
        }

        const res = await ApiService.criarComentario(
            conteudo,
            idLogado,
            idPost
        );

        if (res.ok) {
            await this.carregarDadosGlobais();
        } else {
            this.mostrarFeedbackGlobal("Não foi possível enviar o comentário.");
        }
    }

    montarCardsPosts(listaPosts, idContainer, mostrarTagComunidade) {
        const container = document.getElementById(idContainer);
        container.innerHTML = '';
        if (listaPosts.length === 0) return container.innerHTML = '<p style="color:var(--text-muted)">Nenhuma publicação ainda.</p>';

        listaPosts.forEach(post => {
            let idAutor = post.idUsuario || post.id_usuario;
            let idPost = post.idPost || post.id;
            let idComunidade = post.idComunidade || post.id_comunidade;

            let nomeAutor = this.usuariosMap[idAutor] || 'Desconhecido';
            let fotoAutor = this.usuariosFotosMap[idAutor];
            let urlFotoAutor = ApiService.formatarUrlFotoPerfil(fotoAutor);
            let inicial = (nomeAutor || 'U').charAt(0).toUpperCase();

            let avatarHtml = urlFotoAutor
                ? `<img src="${this.escaparHtmlDestaque(urlFotoAutor)}" class="avatar" style="object-fit:cover;" onerror="this.remove()">`
                : `<div class="avatar">${this.escaparHtmlDestaque(inicial)}</div>`;

            let nomeComunidade = this.comunidadesMap[idComunidade];
            let dataPostagem = post.dataPostagem
                ? new Date(post.dataPostagem).toLocaleDateString('pt-BR')
                : 'Agora';

            const comentarios = this.comentariosPorPost[idPost] || [];
            let htmlComentarios = comentarios.slice(0, 2).map(c =>
                `<div class="comment-preview"><span>${this.escaparHtmlDestaque(this.usuariosMap[c.idUsuario || c.id_usuario] || 'User')}:</span>${this.escaparHtmlDestaque(c.conteudo)}</div>`
            ).join('');
            if (comentarios.length > 2) {
                htmlComentarios += `<button class="comments-more" onclick="app.abrirDetalhesPost(${idPost})">Ver todos os ${comentarios.length} comentários</button>`;
            }

            const idUsuarioLogado = ApiService.getIdUsuarioLogado();

            const comunidade = (this.listaComunidades || []).find(
                c => (c.idComunidade || c.id) == idComunidade
            );

            const idAdministrador = comunidade?.criador?.idUsuario;

            const podeExcluir =
                idUsuarioLogado &&
                (
                    idUsuarioLogado == idAutor ||
                    idUsuarioLogado == idAdministrador
                );

            const div = document.createElement('div');
            div.className = 'card post-card';
            div.tabIndex = 0;
            div.setAttribute('role', 'link');
            div.setAttribute('aria-label', `Abrir publicação: ${post.titulo}`);
            const curtido = this.votoDoUsuario(idPost);
            const salvo = this.postEstaSalvo(idPost);
            const totalCurtidas = this.contarVotos(idPost);
            const htmlImagensPost = this.renderizarImagensPost(idPost);
            const origemPostHtml =
                idComunidade && nomeComunidade
                    ? (
                        mostrarTagComunidade
                            ? `
                    em
                    <button
                        type="button"
                        class="post-community-link"
                        onclick="
                            event.stopPropagation();
                            app.abrirComunidadeDoFeed(
                                ${idComunidade}
                            );
                        "
                    >
                        ${this.escaparHtmlDestaque(
                                nomeComunidade
                            )}
                    </button>
                `
                            : this.escaparHtmlDestaque(
                                nomeComunidade
                            )
                    )
                    : 'Feed global';
            div.innerHTML = `
                <div class="post-header-area">
                    ${avatarHtml}
                    <div>
                        <span class="post-author">${this.escaparHtmlDestaque(nomeAutor)}</span>
                        <div class="post-meta">
    ${origemPostHtml}
    · ${dataPostagem}
</div>
                    </div>
                    
                 <div class="post-menu-wrapper">
                 

    <button
        type="button"
        class="post-more"
        aria-label="Mais opções"
        onclick="app.alternarMenuPost(event)"
    >
        <i class="material-icons">more_horiz</i>
    </button>

    <div
        id="post-menu-${idPost}"
        class="post-menu"
    >

        <button
            type="button"
            class="post-menu-item"
            onclick="
                event.stopPropagation();
                app.compartilharPost(${idPost});
            "
        >
            <i class="material-icons">link</i>
            <span>Copiar link</span>
        </button>

        ${podeExcluir ? `
            <button
                type="button"
                class="post-menu-item post-menu-delete"
                onclick="
                    event.stopPropagation();
                    app.excluirPost(${idPost});
                "
            >
                <i class="material-icons">delete_outline</i>
                <span>Excluir publicação</span>
            </button>
        ` : ''}

    </div>

</div>
                </div>
                <div class="post-title">${this.escaparHtmlDestaque(post.titulo)}</div>
               <div class="post-body">${this.escaparHtmlDestaque(post.conteudo)}</div>
               ${htmlImagensPost}
               ${htmlComentarios}

                <div class="post-actions">
                    <button class="post-action ${curtido ? 'active' : ''}" onclick="app.alternarCurtida(${idPost})"><i class="material-icons">${curtido ? 'favorite' : 'favorite_border'}</i><span>${totalCurtidas}</span></button>
                    <button class="post-action" onclick="document.getElementById('comentario-${idPost}').focus()"><i class="material-icons">chat_bubble_outline</i><span>${comentarios.length}</span></button>
                    <button class="post-action ${salvo ? 'active' : ''}" onclick="app.alternarPostSalvo(${idPost})"><i class="material-icons">${salvo ? 'bookmark' : 'bookmark_border'}</i></button>
                </div>

${ApiService.getIdUsuarioLogado() ? `
                    <div class="comment-input-container">
                       <input id="comentario-${idPost}" type="text" placeholder="Escreva um comentário..." onkeydown="if(event.key === 'Enter') app.enviarComentario(${idPost}, this.nextElementSibling)">
<button onclick="app.enviarComentario(${idPost}, this)">Enviar</button>
                    </div>` : ''}
            `;
            div.addEventListener('click', evento => {
                if (evento.target.closest('button, input, textarea, select')) return;
                this.abrirDetalhesPost(idPost);
            });
            div.addEventListener('keydown', evento => {
                if (evento.key !== 'Enter' && evento.key !== ' ') return;
                evento.preventDefault();
                this.abrirDetalhesPost(idPost);
            });
            container.appendChild(div);
        });
    }

    async abrirDetalhesPost(idPost) {
        let post = this.posts.find(item => (item.idPost || item.id) == idPost);
        if (!post) {
            try {
                post = await ApiService.buscarPostPorId(idPost);
            } catch (erro) {
                return this.mostrarFeedbackGlobal('Não foi possível abrir a publicação.');
            }
        }

        const container = document.getElementById('post-detalhes-conteudo');
        if (!container) return;

        // Busca comentários atualizados sob demanda para este post
        try {
            const comentariosPost = await ApiService.listarComentariosPorPost(idPost);
            if (comentariosPost && comentariosPost.length) {
                this.comentariosPorPost[idPost] = comentariosPost;
            }
        } catch (_) { }

        const idAutor = post.idUsuario || post.id_usuario;
        const nomeAutor = this.usuariosMap[idAutor] || 'Desconhecido';
        const fotoAutor = this.usuariosFotosMap[idAutor];
        const urlFotoAutor = ApiService.formatarUrlFotoPerfil(fotoAutor);
        const inicialAutor = (nomeAutor || 'U').charAt(0).toUpperCase();

        const avatarHeaderHtml = urlFotoAutor
            ? `<img src="${this.escaparHtmlDestaque(urlFotoAutor)}" class="avatar" style="object-fit:cover;" onerror="this.remove()">`
            : `<div class="avatar">${this.escaparHtmlDestaque(inicialAutor)}</div>`;

        const idComunidade = post.idComunidade || post.id_comunidade;
        const nomeComunidade = this.comunidadesMap[idComunidade];
        const idUsuarioLogado =
            ApiService.getIdUsuarioLogado();

        const comunidade =
            (this.listaComunidades || []).find(
                c =>
                    (c.idComunidade || c.id) == idComunidade
            );

        const idAdministrador =
            comunidade?.criador?.idUsuario ||
            comunidade?.criador?.id;

        const podeExcluir =
            idUsuarioLogado &&
            (
                idUsuarioLogado == idAutor ||
                idUsuarioLogado == idAdministrador
            );
        const comentarios = this.comentariosPorPost[idPost] || [];
        const curtido = this.votoDoUsuario(idPost);
        const salvo = this.postEstaSalvo(idPost);
        const dataPostagem = post.dataPostagem
            ? new Date(post.dataPostagem).toLocaleDateString('pt-BR')
            : 'Agora';
        const htmlImagensPost = this.renderizarImagensPost(idPost);

        container.innerHTML = `
            <article class="post-detail-card">
              <div class="post-detail-header">

    ${avatarHeaderHtml}

    <div class="post-detail-author-info">
        <strong>
            ${this.escaparHtmlDestaque(nomeAutor)}
        </strong>

        <span>
            ${nomeComunidade
                ? `em ${this.escaparHtmlDestaque(nomeComunidade)}`
                : 'Feed global'
            }
            · ${dataPostagem}
        </span>
    </div>


    <div class="post-menu-wrapper">

        <button
            type="button"
            class="post-more"
            aria-label="Mais opções"
           onclick="app.alternarMenuPost(event)"
        >
            <i class="material-icons">more_horiz</i>
        </button>

        <div
            class="post-menu"
        >

            <button
                type="button"
                class="post-menu-item"
                onclick="
                    event.stopPropagation();
                    app.compartilharPost(${idPost});
                "
            >
                <i class="material-icons">link</i>
                <span>Copiar link</span>
            </button>

            ${podeExcluir ? `
                <button
                    type="button"
                    class="post-menu-item post-menu-delete"
                    onclick="
                        event.stopPropagation();
                        app.excluirPost(${idPost});
                    "
                >
                    <i class="material-icons">
                        delete_outline
                    </i>

                    <span>
                        Excluir publicação
                    </span>
                </button>
            ` : ''}

        </div>

    </div>

</div>
                <h1>${this.escaparHtmlDestaque(post.titulo)}</h1>
                <div class="post-detail-body">${this.escaparHtmlDestaque(post.conteudo)}</div>
                ${htmlImagensPost}
                <div class="post-actions post-detail-actions">
                    <button class="post-action ${curtido ? 'active' : ''}" onclick="app.alternarCurtidaDetalhe(${idPost})"><i class="material-icons">${curtido ? 'favorite' : 'favorite_border'}</i><span>${this.contarVotos(idPost)}</span></button>
                    <button class="post-action ${salvo ? 'active' : ''}" onclick="app.alternarPostSalvoDetalhe(${idPost})"><i class="material-icons">${salvo ? 'bookmark' : 'bookmark_border'}</i><span>${salvo ? 'Salvo' : 'Salvar'}</span></button>
                </div>
                <section class="post-comments-section">
                    <div class="post-comments-heading"><h2>Comentários</h2><span>${comentarios.length}</span></div>
                    ${ApiService.getIdUsuarioLogado() ? `<div class="detail-comment-box"><input id="comentario-detalhe-${idPost}" type="text" placeholder="Escreva um comentário..." onkeydown="if(event.key === 'Enter') app.enviarComentarioDetalhe(${idPost})"><button onclick="app.enviarComentarioDetalhe(${idPost})">Comentar</button></div>` : '<p class="comments-login-note">Entre para participar da conversa.</p>'}
                    <div class="post-detail-comments">${comentarios.length ? comentarios.map(c => {
                const idC = c.idUsuario || c.id_usuario;
                const nomeC = this.usuariosMap[idC] || 'Usuário';
                const fotoC = this.usuariosFotosMap[idC];
                const urlFotoC = ApiService.formatarUrlFotoPerfil(fotoC);
                const iniC = (nomeC || 'U').charAt(0).toUpperCase();
                const avC = urlFotoC
                    ? `<img src="${this.escaparHtmlDestaque(urlFotoC)}" class="avatar detail-comment-avatar" style="object-fit:cover;" onerror="this.remove()">`
                    : `<div class="avatar detail-comment-avatar">${this.escaparHtmlDestaque(iniC)}</div>`;
                return `<div class="detail-comment">${avC}<div><strong>${this.escaparHtmlDestaque(nomeC)}</strong><p>${this.escaparHtmlDestaque(c.conteudo)}</p></div></div>`;
            }).join('') : '<p class="comments-empty">Ainda não há comentários. Seja o primeiro a participar.</p>'}</div>
                </section>
            </article>
        `;

        this.salvarViewAtual('post-detalhes');
        document.querySelectorAll('.view-section').forEach(el => el.classList.remove('active'));
        document.getElementById('view-post-detalhes').classList.add('active');
        document.querySelectorAll('.nav-btn').forEach(el => el.classList.remove('active'));
        document.getElementById('tab-feed')?.classList.add('active');
    }

    async enviarComentarioDetalhe(idPost) {
        const input = document.getElementById(`comentario-detalhe-${idPost}`);
        if (!input) return;
        const conteudo = input.value.trim();
        if (!conteudo) return;
        const resposta = await ApiService.criarComentario(conteudo, ApiService.getIdUsuarioLogado(), idPost);
        if (!resposta.ok) return this.mostrarFeedbackGlobal('Não foi possível enviar o comentário.');
        await this.carregarDadosGlobais();
        await this.abrirDetalhesPost(idPost);
    }

    async alternarCurtidaDetalhe(idPost) {
        await this.alternarCurtida(idPost);
        await this.abrirDetalhesPost(idPost);
    }

    async alternarPostSalvoDetalhe(idPost) {
        await this.alternarPostSalvo(idPost);
        await this.abrirDetalhesPost(idPost);
    }

    voltarDoPost() {
        document.querySelectorAll('.view-section').forEach(el => el.classList.remove('active'));
        document.getElementById('view-feed').classList.add('active');
        document.querySelectorAll('.nav-btn').forEach(el => el.classList.remove('active'));
        document.getElementById('tab-feed')?.classList.add('active');
        this.salvarViewAtual('feed');
        this.renderizarFeedGeral();
    }

    mudarFiltroPessoas(filtro) {

        this.filtroPessoas = filtro;

        document
            .querySelectorAll('.pessoas-filtro-btn')
            .forEach(botao =>
                botao.classList.remove('active')
            );

        document
            .getElementById(
                `pessoas-filtro-${filtro}`
            )
            ?.classList.add('active');

        this.filtrarUsuarios();
    }

    // --- ABA BUSCA ---
    filtrarUsuarios() {

        const termo =
            document
                .getElementById('search-input')
                ?.value
                .trim()
                .toLowerCase() || '';

        const idUsuarioLogado =
            ApiService.getIdUsuarioLogado();


        let pessoas =
            [...(this.listaUsuariosCompleta || [])];


        // Não mostrar a própria conta
        pessoas = pessoas.filter(usuario => {

            const id =
                usuario.idUsuario ||
                usuario.id;

            return String(id) !==
                String(idUsuarioLogado);
        });


        // =========================
        // BUSCA
        // =========================
        if (termo) {

            pessoas = pessoas.filter(usuario => {

                const nome =
                    usuario.nome || '';

                const username =
                    usuario.username || '';

                return (
                    nome
                        .toLowerCase()
                        .includes(termo)
                    ||
                    username
                        .toLowerCase()
                        .includes(termo)
                );
            });
        }


        // =========================
        // MINHAS COMUNIDADES
        // =========================
        if (this.filtroPessoas === 'comuns') {

            pessoas = pessoas.filter(usuario => {

                const id =
                    usuario.idUsuario ||
                    usuario.id;

                return (
                    this.obterComunidadesEmComum(id)
                        .length > 0
                );
            });
        }


        // =========================
        // ORGANIZADORES
        // =========================
        if (
            this.filtroPessoas ===
            'organizadores'
        ) {

            const organizadores =
                new Set(
                    (this.listaEventos || [])
                        .map(evento =>
                            String(
                                evento.criadorId
                            )
                        )
                );

            pessoas = pessoas.filter(usuario => {

                const id =
                    usuario.idUsuario ||
                    usuario.id;

                return organizadores.has(
                    String(id)
                );
            });
        }


        const usuarioLogado = ApiService.getUsuarioLogado();
        const interessesUsuario = Array.isArray(usuarioLogado?.interesses)
            ? usuarioLogado.interesses
            : this.interessesSelecionados;

        pessoas = pessoas
            .map(usuario => ({
                usuario,
                relevancia: this.calcularRelevanciaPessoa(
                    usuario,
                    interessesUsuario,
                    termo
                )
            }))
            .sort((a, b) =>
                b.relevancia - a.relevancia ||
                String(a.usuario.nome || '').localeCompare(
                    String(b.usuario.nome || ''),
                    'pt-BR',
                    { sensitivity: 'base' }
                )
            )
            .map(item => item.usuario);

        this.paginaPessoas = 0;
        this.pessoasFiltradas = pessoas;
        this.renderizarUsuarios(pessoas);
    }

    calcularRelevanciaPessoa(usuario, interessesUsuario, termoBusca) {
        const id = usuario.idUsuario || usuario.id;
        const comunidades = this.obterComunidadesDoUsuario(id);
        const comunidadesEmComum = this.obterComunidadesEmComum(id);
        const nome = String(usuario.nome || '').toLowerCase();
        const username = String(usuario.username || '').toLowerCase();
        const interessesDaPessoa = new Set(
            (Array.isArray(usuario.interesses) ? usuario.interesses : [])
                .map(interesse => String(interesse).trim().toLowerCase())
                .filter(Boolean)
        );
        const textoPerfil = [
            usuario.nome,
            usuario.username,
            usuario.bio,
            ...comunidades.flatMap(comunidade => [
                comunidade.nome,
                comunidade.categoria
            ])
        ]
            .filter(Boolean)
            .join(' ')
            .toLowerCase();

        let relevancia = comunidadesEmComum.length * 10;
        if (termoBusca) {
            if (nome === termoBusca || username === termoBusca) {
                relevancia += 2000;
            } else if (nome.startsWith(termoBusca) || username.startsWith(termoBusca)) {
                relevancia += 1000;
            } else {
                relevancia += 100;
            }
        }

        interessesUsuario.forEach(interesse => {
            const termo = String(interesse || '').trim().toLowerCase();
            if (!termo) return;

            if (interessesDaPessoa.has(termo)) {
                relevancia += 8;
            } else if (textoPerfil.includes(termo)) {
                relevancia += 2;
            }
        });

        return relevancia;
    }

    renderizarUsuarios(lista) {

        const container =
            document.getElementById(
                'users-container'
            );

        if (!container) return;

        container.innerHTML = '';

        const totalPaginas = Math.ceil(
            lista.length / this.tamanhoPaginaPessoas
        );
        this.paginaPessoas = Math.min(
            this.paginaPessoas,
            Math.max(totalPaginas - 1, 0)
        );

        const paginacao = document.getElementById('pessoas-paginacao');
        const informacaoPagina = document.getElementById('pessoas-pagina-info');
        const botaoAnterior = document.getElementById('btn-pessoas-anterior');
        const botaoProxima = document.getElementById('btn-pessoas-proxima');

        if (paginacao) paginacao.hidden = totalPaginas <= 1;
        if (informacaoPagina) {
            informacaoPagina.innerText =
                `Página ${totalPaginas ? this.paginaPessoas + 1 : 0} de ${totalPaginas}`;
        }
        if (botaoAnterior) botaoAnterior.disabled = this.paginaPessoas <= 0;
        if (botaoProxima) {
            botaoProxima.disabled =
                this.paginaPessoas >= totalPaginas - 1;
        }

        if (!lista.length) {

            container.innerHTML = `
            <div class="pessoas-empty">

                <div class="pessoas-empty-icon">
                    <i class="material-icons">
                        person_search
                    </i>
                </div>

                <h3>
                    Nenhuma pessoa encontrada
                </h3>

                <p>
                    Tente outro nome ou altere
                    os filtros de pessoas.
                </p>

            </div>
        `;

            return;
        }

        const inicio = this.paginaPessoas * this.tamanhoPaginaPessoas;
        const pessoasDaPagina = lista.slice(
            inicio,
            inicio + this.tamanhoPaginaPessoas
        );

        pessoasDaPagina.forEach(usuario => {

            const id =
                usuario.idUsuario ||
                usuario.id;

            const comunidades =
                this.obterComunidadesDoUsuario(id);

            const comunidadesEmComum =
                this.obterComunidadesEmComum(id);

            const eventos =
                (this.listaEventos || [])
                    .filter(evento =>
                        String(evento.criadorId) ===
                        String(id)
                    );


            // =========================
            // FOTO
            // =========================

            const foto =
                this.usuariosFotosMap[id];

            const urlFoto =
                ApiService.formatarUrlFotoPerfil(
                    foto
                );

            const inicial =
                (
                    usuario.nome ||
                    'U'
                )
                    .charAt(0)
                    .toUpperCase();


            const avatarHtml =
                urlFoto
                    ? `
                    <img
                        src="${this.escaparHtmlDestaque(urlFoto)}"
                        class="avatar pessoa-avatar"
                        alt=""
                        onerror="this.remove()"
                    >
                `
                    : `
                    <div
                        class="avatar pessoa-avatar"
                    >
                        ${this.escaparHtmlDestaque(
                        inicial
                    )}
                    </div>
                `;


            // =========================
            // CONTEXTO
            // =========================

            let contextoHtml = '';


            if (comunidadesEmComum.length) {

                const primeira =
                    comunidadesEmComum[0];

                const restantes =
                    comunidadesEmComum.length - 1;

                contextoHtml = `
                <div class="pessoa-contexto">

                    <i class="material-icons">
                        groups
                    </i>

                    <span>
                        Também participa de

                        <strong>
                            ${this.escaparHtmlDestaque(
                    primeira.nome ||
                    'uma comunidade'
                )}
                        </strong>

                        ${restantes > 0
                        ? ` e mais ${restantes}`
                        : ''
                    }
                    </span>

                </div>
            `;
            }

            else if (eventos.length) {

                contextoHtml = `
                <div class="pessoa-contexto">

                    <i class="material-icons">
                        event_available
                    </i>

                    <span>
                        Organiza
                        <strong>
                            ${eventos.length}
                            ${eventos.length === 1
                        ? 'evento'
                        : 'eventos'
                    }
                        </strong>
                    </span>

                </div>
            `;
            }


            // =========================
            // CARD
            // =========================

            const div =
                document.createElement('article');

            div.className =
                'pessoa-card';

            div.tabIndex = 0;

            div.setAttribute(
                'role',
                'link'
            );

            div.setAttribute(
                'aria-label',
                `Abrir perfil de ${usuario.nome ||
                'usuário'
                }`
            );


            div.innerHTML = `

            <div class="pessoa-card-main">

                ${avatarHtml}

                <div class="pessoa-card-identidade">

                    <h2>
                        ${this.escaparHtmlDestaque(
                usuario.nome ||
                'Usuário'
            )}
                    </h2>

                    <span>
                        @${this.escaparHtmlDestaque(
                usuario.username ||
                'usuario'
            )}
                    </span>

                </div>

            </div>


            <p class="pessoa-card-bio">
                ${this.escaparHtmlDestaque(
                usuario.bio ||
                'Participa da comunidade SocialJoin.'
            )}
            </p>


            ${contextoHtml}


            <div class="pessoa-card-stats">

                <span>
                    <i class="material-icons">
                        groups
                    </i>

                    ${comunidades.length}

                    ${comunidades.length === 1
                    ? 'comunidade'
                    : 'comunidades'
                }
                </span>


                <span>
                    <i class="material-icons">
                        event
                    </i>

                    ${eventos.length}

                    ${eventos.length === 1
                    ? 'evento'
                    : 'eventos'
                }
                </span>

            </div>


            <div class="pessoa-card-footer">

                <span class="pessoa-ver-perfil">
                    Ver perfil

                    <i class="material-icons">
                        arrow_forward
                    </i>
                </span>


                <button
                    type="button"
                    class="pessoa-mensagem-btn"
                    onclick="
                        event.stopPropagation();
                        app.iniciarConversa(${id});
                    "
                >
                    <i class="material-icons">
                        chat_bubble_outline
                    </i>

                    Mensagem
                </button>

            </div>
        `;


            // Card inteiro continua abrindo perfil
            div.addEventListener(
                'click',
                () =>
                    this.abrirPerfil(id)
            );


            div.addEventListener(
                'keydown',
                evento => {

                    if (
                        evento.target.closest('button')
                    ) {
                        return;
                    }

                    if (
                        evento.key !== 'Enter' &&
                        evento.key !== ' '
                    ) {
                        return;
                    }

                    evento.preventDefault();

                    this.abrirPerfil(id);
                }
            );


            container.appendChild(div);
        });
    }

    paginaAnteriorPessoas() {
        if (this.paginaPessoas <= 0) return;

        this.paginaPessoas--;
        this.renderizarUsuarios(this.pessoasFiltradas);
    }

    proximaPaginaPessoas() {
        const totalPaginas = Math.ceil(
            this.pessoasFiltradas.length / this.tamanhoPaginaPessoas
        );
        if (this.paginaPessoas >= totalPaginas - 1) return;

        this.paginaPessoas++;
        this.renderizarUsuarios(this.pessoasFiltradas);
    }

    // Busca somente a página atual com os filtros ativos; a API mantém a ordenação global entre páginas.
    async carregarEventos() {

        const container = document.getElementById('eventos-container');
        if (!container) return;

        container.innerHTML = 'Carregando eventos...';

        try {
            const resultado = await ApiService.buscarEventos({
                ...this.filtrosEventos,
                page: this.paginaEventos,
                size: this.tamanhoPaginaEventos
            });

            let eventos = resultado.content || [];

            eventos = this.ordenarPorInteresses(eventos);
            this.listaEventos = eventos;
            this.totalPaginasEventos = resultado.totalPages;
            this.renderizarRecomendacoes();
            this.renderizarSugestoesPessoas();

            await this.carregarEventoDestaqueMaisPopular();

            container.innerHTML = '';

            if (eventos.length === 0) {
                container.innerHTML =
                    '<p style="color:var(--text-muted); padding:16px;">Nenhum evento agendado no momento.</p>';
                this.totalPaginasEventos = 0;
                this.atualizarPaginacaoEventos();
                return;
            }

            const grupos = eventos.reduce((acumulado, evento) => {
                const chave = evento.dataEvento || 'sem-data';
                (acumulado[chave] ||= []).push(evento);
                return acumulado;
            }, {});

            for (const [data, eventosDoDia] of Object.entries(grupos)) {
                const grupo = document.createElement('section');
                grupo.className = 'eventos-grupo';
                grupo.innerHTML = `<h2 class="eventos-grupo-titulo">${this.formatarDataEvento(data)}</h2>`;
                const grade = document.createElement('div');
                grade.className = 'eventos-grid';
                grupo.appendChild(grade);
                container.appendChild(grupo);

                for (const evento of eventosDoDia) {

                    const div = document.createElement('div');
                    div.className = 'evento-card';

                    let badgeDistanciaHtml = '';
                    if (this.coordenadasUsuario && evento.latitude != null && evento.longitude != null) {
                        const distKm = ApiService.calcularDistanciaKm(
                            this.coordenadasUsuario.lat,
                            this.coordenadasUsuario.lng,
                            evento.latitude,
                            evento.longitude
                        );
                        if (distKm !== null) {
                            const textoDist = distKm < 1
                                ? `${Math.round(distKm * 1000)} m`
                                : `${distKm.toFixed(1)} km`;
                            badgeDistanciaHtml = `<span class="badge-distancia" title="Distância estimada de você"><i class="material-icons">near_me</i> ${textoDist}</span>`;
                        }
                    }

                    const statusDetalhes =
                        evento.situacaoTemporal ||
                        evento.status ||
                        'AGENDADO';

                    const statusDetalhesExibido =
                        statusDetalhes === 'ACONTECENDO_AGORA'
                            ? 'ACONTECENDO AGORA'
                            : statusDetalhes;
                    const eventoCancelado = statusDetalhes === 'CANCELADO';

                    const statusExibido =
                        statusDetalhes === 'ACONTECENDO_AGORA'
                            ? 'ACONTECENDO AGORA'
                            : statusDetalhes;

                    const categoriaExibida =
                        this.escaparHtmlDestaque(evento.categoria || 'Comunidade');
                    const tituloExibido =
                        this.escaparHtmlDestaque(evento.titulo || 'Evento');
                    const descricaoExibida =
                        this.escaparHtmlDestaque(evento.descricao || 'Sem descrição informada.');
                    const localExibido =
                        this.escaparHtmlDestaque(evento.localEvento || 'Local a confirmar');
                    const imagemCapaUrl = evento.imagemCapa
                        ? this.escaparHtmlDestaque(this.urlCapaEvento(evento.imagemCapa))
                        : '';

                    const dataFormatada = evento.dataEvento
                        ? evento.dataEvento.split('-').reverse().join('/')
                        : '';

                    const horarioInicioFormatado = evento.horarioInicio
                        ? evento.horarioInicio.substring(0, 5)
                        : '';

                    const horarioFimFormatado = evento.horarioFim
                        ? evento.horarioFim.substring(0, 5)
                        : '';

                    const nomeCriador =
                        this.usuariosMap[evento.criadorId] || 'Desconhecido';

                    const nomeComunidade = evento.comunidadeId
                        ? this.comunidadesMap[evento.comunidadeId] || 'Desconhecida'
                        : null;

                    const vagas = evento.limiteParticipantes
                        ? `${evento.limiteParticipantes} vagas`
                        : 'Sem limite de vagas';

                    const usaCheckin = evento.exigeCheckin
                        ? 'Sim'
                        : 'Não';

                    const quantidadeParticipantes =
                        evento.quantidadeParticipantes || 0;
                    const eventoLotado = evento.limiteParticipantes && quantidadeParticipantes >= evento.limiteParticipantes;
                    const precoIngresso = Number(evento.precoIngresso || 0);

                    const precoFormatado =
                        precoIngresso > 0
                            ? precoIngresso.toLocaleString('pt-BR', {
                                style: 'currency',
                                currency: 'BRL'
                            })
                            : 'Gratuito';
                    const idUsuario = ApiService.getIdUsuarioLogado();
                    const ehCriador = idUsuario && idUsuario == evento.criadorId;
                    const acao = ehCriador ? 'Organizador' : eventoLotado ? 'Evento lotado' : idUsuario ? 'Ver detalhes' : 'Entrar para participar';
                    div.innerHTML = `
                    <div class="evento-card-capa ${evento.imagemCapa ? '' : 'evento-card-capa-vazia'}" ${imagemCapaUrl ? `style="background-image:url('${imagemCapaUrl}')"` : ''}>
                        ${!evento.imagemCapa ? '<i class="material-icons">event</i>' : ''}
                        <span>${categoriaExibida}</span>
                    </div>
                    <div class="evento-card-body">
                        <div class="evento-card-meta"><span>${horarioInicioFormatado} - ${horarioFimFormatado}</span><b>${this.escaparHtmlDestaque(statusExibido)}</b></div>
                        <h3>${tituloExibido}</h3>
                        <p>${descricaoExibida}</p>
                       <div class="evento-card-info">

    <span>
    📍 ${localExibido}
    </span>

    <span>
        👥 ${evento.limiteParticipantes
                            ? `${quantidadeParticipantes}/${evento.limiteParticipantes}`
                            : `${quantidadeParticipantes}`} participantes
    </span>

    <span>
        🎟 ${precoFormatado}
    </span>

    ${badgeDistanciaHtml}

</div>
                        <button class="btn-primary evento-card-action" onclick="app.abrirDetalhesEvento(${evento.id})">${acao}</button>
                    </div>
                `;
                    grade.appendChild(div);
                }
            }


        } catch (e) {
            console.error(e);

            container.innerHTML =
                "<p style='padding:16px; color:red;'>Erro ao conectar com a API de Eventos.</p>";
        }
        this.atualizarPaginacaoEventos();
    }

    formatarDataEvento(data) {
        if (!data || data === 'sem-data') return 'Data a confirmar';
        return new Date(`${data}T00:00:00`).toLocaleDateString('pt-BR', {
            weekday: 'long', day: 'numeric', month: 'long'
        });
    }

    renderizarEventoDestaque(evento) {
        const container = document.getElementById('evento-destaque');
        if (!container) return;
        if (!evento) {
            container.innerHTML = '<div class="evento-destaque-vazio">Nenhum evento encontrado com esses filtros.</div>';
            return;
        }
        const data = evento.dataEvento ? new Date(`${evento.dataEvento}T00:00:00`).toLocaleDateString('pt-BR', { day: '2-digit', month: 'short' }).replace('.', '') : '--';
        const categoria = this.escaparHtmlDestaque(evento.categoria || 'Em destaque');
        const titulo = this.escaparHtmlDestaque(evento.titulo || 'Evento');
        const descricao = this.escaparHtmlDestaque(
            evento.descricao || 'Uma nova experiência está esperando por você.'
        );
        const local = this.escaparHtmlDestaque(evento.localEvento || 'Local a confirmar');
        const imagemCapaUrl = evento.imagemCapa
            ? this.escaparHtmlDestaque(this.urlCapaEvento(evento.imagemCapa))
            : '';
        const idEvento = Number(evento.idEvento || evento.id);
        container.innerHTML = `
            <div class="evento-destaque-copy"><span class="section-kicker">${categoria}</span><h2>${titulo}</h2><p>${descricao}</p><div class="evento-destaque-details"><span>📅 ${this.escaparHtmlDestaque(data)} · ${this.escaparHtmlDestaque(evento.horarioInicio?.substring(0, 5) || '--:--')}</span><span>📍 ${local}</span></div><button class="btn-primary" onclick="app.abrirDetalhesEvento(${idEvento})">Ver detalhes <i class="material-icons">arrow_forward</i></button></div><div class="evento-destaque-art" ${imagemCapaUrl ? `style="background-image:linear-gradient(135deg,rgba(43,23,32,.2),rgba(234,63,116,.5)),url('${imagemCapaUrl}');background-size:cover;background-position:center;"` : ''}><i class="material-icons">celebration</i></div>
        `;
    }

    urlCapaEvento(caminho) {
        if (typeof caminho !== 'string' || !caminho.trim()) return '';

        try {
            const baseUrl = `${ApiService.API_URL.replace(/\/$/, '')}/`;
            const url = new URL(caminho, baseUrl);
            const origemApi = new URL(baseUrl).origin;
            const eUrlAbsoluta = /^https?:\/\//i.test(caminho);

            if (!['http:', 'https:'].includes(url.protocol)) return '';
            if (!eUrlAbsoluta && url.origin !== origemApi) return '';

            return url.href;
        } catch {
            return '';
        }
    }

    async mudarVisaoEventos(visao) {
        this.visaoEventos = visao;
        document.querySelectorAll('.eventos-tab').forEach(tab => tab.classList.remove('active'));
        document.getElementById(`eventos-tab-${visao}`)?.classList.add('active');
        if (visao === 'meus') {
            await this.carregarMeusEventos();
        } else {
            await this.carregarEventos();
        }
    }

    async carregarMeusEventos() {
        const container = document.getElementById('eventos-container');
        if (!container) return;
        const idUsuario = ApiService.getIdUsuarioLogado();
        if (!idUsuario) {
            container.innerHTML = '<div class="evento-destaque-vazio">Entre para acompanhar os eventos dos quais você participa.</div>';
            return;
        }
        container.innerHTML = 'Carregando seus eventos...';
        try {
            const eventos = await ApiService.listarEventosParticipando(idUsuario);
            this.listaEventos = eventos;
            await this.carregarEventoDestaqueMaisPopular();
            container.innerHTML = eventos.length ? eventos.map(evento => `<div class="meu-evento-row"><div><strong>${this.escaparHtmlDestaque(evento.titulo || 'Evento')}</strong><span>${this.escaparHtmlDestaque(this.formatarDataEvento(evento.dataEvento))} · ${this.escaparHtmlDestaque(evento.localEvento || 'Local a confirmar')}</span></div><button class="btn-primary" onclick="app.abrirDetalhesEvento(${Number(evento.idEvento || evento.id)})">Ver evento</button></div>`).join('') : '<div class="evento-destaque-vazio">Você ainda não está participando de nenhum evento.</div>';
        } catch (erro) {
            console.error(erro);
            container.innerHTML = '<div class="evento-destaque-vazio">Não foi possível carregar seus eventos.</div>';
        }
    }

    async carregarAgendaEventos() {
        const container = document.getElementById('agenda-eventos-container');
        if (!container) return;
        const idUsuario = ApiService.getIdUsuarioLogado();
        if (!idUsuario) {
            container.innerHTML = `
                <div class="agenda-empty-state">
                    <span class="agenda-empty-icon"><i class="material-icons">bookmark_border</i></span>
                    <h2>Sua agenda começa aqui</h2>
                    <p>Entre na sua conta para guardar eventos e encontrá-los facilmente depois.</p>
                    <button class="btn-primary" onclick="app.abrirModal('auth-modal')">Entrar na conta</button>
                </div>
            `;
            return;
        }

        container.innerHTML = `
            <div class="agenda-loading-state">
                <span class="agenda-loading-spinner" aria-hidden="true"></span>
                <span>Carregando seus eventos...</span>
            </div>
        `;
        try {
            const eventos = await ApiService.listarAgendaEventos(idUsuario);
            const futuros = eventos
                .filter(evento => {
                    if (!evento.dataEvento || !evento.horarioFim) return false;
                    return new Date(`${evento.dataEvento}T${evento.horarioFim}`) > new Date()
                        && evento.status !== 'CANCELADO';
                })
                .sort((a, b) => new Date(`${a.dataEvento}T${a.horarioInicio || '00:00'}`)
                    - new Date(`${b.dataEvento}T${b.horarioInicio || '00:00'}`));

            if (!futuros.length) {
                container.innerHTML = `
                    <div class="agenda-empty-state">
                        <span class="agenda-empty-icon"><i class="material-icons">event_available</i></span>
                        <h2>Agenda livre por enquanto</h2>
                        <p>Explore os próximos encontros e salve os que despertarem seu interesse.</p>
                        <button class="btn-primary" onclick="app.mudarAba('eventos')">Descobrir eventos</button>
                    </div>
                `;
                return;
            }

            container.innerHTML = futuros.map(evento => `
                <article class="agenda-evento-card">
                    <div class="agenda-evento-data" aria-label="${this.escaparHtmlDestaque(this.formatarDataEvento(evento.dataEvento))}">
                        <span>${new Date(`${evento.dataEvento}T00:00:00`).toLocaleDateString('pt-BR', { month: 'short' }).replace('.', '')}</span>
                        <strong>${new Date(`${evento.dataEvento}T00:00:00`).getDate()}</strong>
                    </div>
                    <div class="agenda-evento-conteudo">
                        <div class="agenda-evento-etiquetas">
                            <span class="agenda-evento-categoria">${this.escaparHtmlDestaque(evento.categoria || 'Evento')}</span>
                            <span class="agenda-evento-salvo"><i class="material-icons" aria-hidden="true">bookmark</i> Na agenda</span>
                        </div>
                        <h2>${this.escaparHtmlDestaque(evento.titulo || 'Evento')}</h2>
                        <div class="agenda-evento-detalhes">
                            <span><i class="material-icons" aria-hidden="true">event</i>${this.escaparHtmlDestaque(this.formatarDataEvento(evento.dataEvento))}</span>
                            <span><i class="material-icons" aria-hidden="true">schedule</i>${this.escaparHtmlDestaque(evento.horarioInicio?.substring(0, 5) || '--:--')}</span>
                            <span><i class="material-icons" aria-hidden="true">place</i>${this.escaparHtmlDestaque(evento.localEvento || 'Local a confirmar')}</span>
                        </div>
                    </div>
                    <div class="agenda-evento-acoes">
                        <button class="agenda-action agenda-action-details" onclick="app.abrirDetalhesEvento(${Number(evento.id)})">
                            <i class="material-icons" aria-hidden="true">open_in_new</i><span>Detalhes</span>
                        </button>
                        <button class="agenda-action agenda-action-remove" aria-label="Remover ${this.escaparHtmlDestaque(evento.titulo || 'evento')} da agenda"
                            onclick="app.removerEventoDaAgenda(${Number(evento.id)})">
                            <i class="material-icons" aria-hidden="true">bookmark_remove</i><span>Remover</span>
                        </button>
                    </div>
                </article>
            `).join('');
        } catch (erro) {
            console.error('Erro ao carregar agenda de eventos:', erro);
            container.innerHTML = `
                <div class="agenda-empty-state agenda-error-state">
                    <span class="agenda-empty-icon"><i class="material-icons">error_outline</i></span>
                    <h2>Não foi possível carregar sua agenda</h2>
                    <p>Verifique sua conexão e tente novamente.</p>
                    <button class="btn-secondary" onclick="app.carregarAgendaEventos()">Tentar novamente</button>
                </div>
            `;
        }
    }

    async alternarEventoNaAgenda(idEvento, salvo) {
        const idUsuario = ApiService.getIdUsuarioLogado();
        if (!idUsuario) return this.mostrarFeedbackGlobal('Entre para salvar eventos na sua agenda.');

        try {
            const resposta = salvo
                ? await ApiService.removerEventoAgenda(idEvento, idUsuario)
                : await ApiService.salvarEventoAgenda(idEvento, idUsuario);
            if (!resposta.ok) {
                throw new Error(await this.mensagemErroApi(
                    resposta,
                    'Não foi possível alterar sua agenda.'
                ));
            }
            await this.abrirDetalhesEvento(idEvento);
            await this.carregarAgendaEventos();
            this.mostrarFeedbackGlobal(salvo
                ? 'Evento removido da agenda.'
                : 'Evento salvo na sua agenda.');
        } catch (erro) {
            console.error('Erro ao atualizar agenda de eventos:', erro);
            this.mostrarFeedbackGlobal(erro.message || 'Não foi possível atualizar sua agenda.');
        }
    }

    async removerEventoDaAgenda(idEvento) {
        const idUsuario = ApiService.getIdUsuarioLogado();
        if (!idUsuario) return;
        try {
            const resposta = await ApiService.removerEventoAgenda(idEvento, idUsuario);
            if (!resposta.ok) {
                throw new Error(await this.mensagemErroApi(
                    resposta,
                    'Não foi possível remover o evento da agenda.'
                ));
            }
            await this.carregarAgendaEventos();
        } catch (erro) {
            console.error('Erro ao remover evento da agenda:', erro);
            this.mostrarFeedbackGlobal(erro.message || 'Não foi possível remover o evento da agenda.');
        }
    }

    async exportarAgendaIcs() {
        const idUsuario = ApiService.getIdUsuarioLogado();
        if (!idUsuario) return this.mostrarFeedbackGlobal('Entre para exportar sua agenda.');
        try {
            const eventos = await ApiService.listarAgendaEventos(idUsuario);
            const icsEscape = valor => String(valor || '')
                .replace(/\\/g, '\\\\')
                .replace(/\n/g, '\\n')
                .replace(/,/g, '\\,')
                .replace(/;/g, '\\;');
            const linhas = [
                'BEGIN:VCALENDAR',
                'VERSION:2.0',
                'PRODID:-//SocialJoin//Agenda de eventos//PT-BR'
            ];
            eventos.filter(evento => evento.status !== 'CANCELADO'
                && evento.dataEvento && evento.horarioInicio)
                .forEach(evento => {
                    const dia = evento.dataEvento.replace(/-/g, '');
                    const inicio = (evento.horarioInicio || '00:00:00')
                        .replace(/:/g, '').slice(0, 6).padEnd(6, '0');
                    const fim = (evento.horarioFim || evento.horarioInicio || '00:00:00')
                        .replace(/:/g, '').slice(0, 6).padEnd(6, '0');
                    linhas.push(
                        'BEGIN:VEVENT',
                        `UID:socialjoin-event-${Number(evento.id)}@socialjoin`,
                        `DTSTAMP:${new Date().toISOString().replace(/[-:]/g, '').replace(/\.\d{3}/, '')}`,
                        `DTSTART;TZID=America/Sao_Paulo:${dia}T${inicio}`,
                        `DTEND;TZID=America/Sao_Paulo:${dia}T${fim}`,
                        `SUMMARY:${icsEscape(evento.titulo)}`,
                        `DESCRIPTION:${icsEscape(evento.descricao)}`,
                        `LOCATION:${icsEscape(evento.localEvento)}`,
                        'END:VEVENT'
                    );
                });
            linhas.push('END:VCALENDAR');
            const arquivo = new Blob([linhas.join('\r\n')], { type: 'text/calendar;charset=utf-8' });
            const url = URL.createObjectURL(arquivo);
            const link = document.createElement('a');
            link.href = url;
            link.download = 'minha-agenda-socialjoin.ics';
            link.click();
            URL.revokeObjectURL(url);
        } catch (erro) {
            console.error('Erro ao exportar agenda:', erro);
            this.mostrarFeedbackGlobal('Não foi possível exportar sua agenda.');
        }
    }

    async carregarNotificacoesEventos() {
        const container = document.getElementById('notificacoes-container');
        const contador = document.getElementById('notificacoes-nao-lidas');
        const notificacoesAbertas = document
            .getElementById('view-notificacoes')
            ?.classList.contains('active');
        if (!container) return;
        const idUsuario = ApiService.getIdUsuarioLogado();
        if (!idUsuario) {
            container.innerHTML = '<div class="evento-destaque-vazio">Entre para receber atualizações e lembretes dos eventos.</div>';
            if (contador) contador.hidden = true;
            return;
        }

        try {
            let notificacoes = await ApiService.listarNotificacoesEventos(idUsuario);
            if (notificacoesAbertas) {
                const exclusoesNaoLidas = notificacoes.filter(
                    notificacao => notificacao.tipo === 'EVENTO_EXCLUIDO' && !notificacao.lida
                );
                if (exclusoesNaoLidas.length > 0) {
                    try {
                        const respostas = await Promise.all(exclusoesNaoLidas.map(
                            notificacao => ApiService.marcarNotificacaoLida(notificacao.id, idUsuario)
                        ));
                        if (respostas.some(resposta => !resposta.ok)) {
                            throw new Error('Uma ou mais notificações não puderam ser marcadas como lidas.');
                        }
                        const idsMarcados = new Set(exclusoesNaoLidas.map(notificacao => notificacao.id));
                        notificacoes = notificacoes.map(notificacao => idsMarcados.has(notificacao.id)
                            ? { ...notificacao, lida: true }
                            : notificacao);
                        notificacoes = await ApiService.listarNotificacoesEventos(idUsuario);
                    } catch (erro) {
                        console.error('Erro ao marcar notificações de eventos excluídos como lidas:', erro);
                        this.mostrarFeedbackGlobal(
                            'Não foi possível marcar as notificações de eventos excluídos como visualizadas.'
                        );
                    }
                }
            }
            const naoLidas = notificacoes.filter(notificacao => !notificacao.lida).length;
            if (contador) {
                contador.textContent = naoLidas > 99 ? '99+' : String(naoLidas);
                contador.hidden = naoLidas === 0;
            }
            if (!notificacoes.length) {
                container.innerHTML = '<div class="evento-destaque-vazio">Você não tem notificações por enquanto.</div>';
                return;
            }
            container.innerHTML = notificacoes.map(notificacao => `
                <article class="notificacao-item ${notificacao.lida ? '' : 'nao-lida'}">
                    <div>
                        <strong>${this.escaparHtmlDestaque(notificacao.tipo.replaceAll('_', ' '))}</strong>
                        <p>${this.escaparHtmlDestaque(notificacao.mensagem)}</p>
                        <small>${this.escaparHtmlDestaque(new Date(notificacao.criadoEm).toLocaleString('pt-BR'))}</small>
                    </div>
                    ${notificacao.eventoId
                        ? `<button class="btn-ghost" onclick="app.abrirNotificacaoEvento(${Number(notificacao.id)}, ${Number(notificacao.eventoId)})">Ver evento</button>`
                        : ''}
                </article>
            `).join('');
        } catch (erro) {
            console.error('Erro ao carregar notificações:', erro);
            container.innerHTML = '<div class="evento-destaque-vazio">Não foi possível carregar suas notificações.</div>';
        }
    }

    async abrirNotificacaoEvento(idNotificacao, idEvento) {
        const idUsuario = ApiService.getIdUsuarioLogado();
        if (idUsuario) {
            const resposta = await ApiService.marcarNotificacaoLida(idNotificacao, idUsuario);
            if (!resposta.ok) {
                this.mostrarFeedbackGlobal('Não foi possível atualizar a notificação.');
                return;
            }
        }
        await this.carregarNotificacoesEventos();
        await this.abrirDetalhesEvento(idEvento);
    }

    async entrarNaListaEsperaEvento(idEvento, sair = false) {
        const idUsuario = ApiService.getIdUsuarioLogado();
        if (!idUsuario) return this.mostrarFeedbackGlobal('Entre para entrar na lista de espera.');
        try {
            const resposta = sair
                ? await ApiService.sairDaListaEspera(idEvento, idUsuario)
                : await ApiService.entrarNaListaEspera(idEvento, idUsuario);
            if (!resposta.ok) {
                const mensagem = await resposta.text();
                throw new Error(mensagem || 'Não foi possível alterar a lista de espera.');
            }
            await this.abrirDetalhesEvento(idEvento);
            this.mostrarFeedbackGlobal(sair
                ? 'Você saiu da lista de espera.'
                : 'Você entrou na lista de espera. Se uma vaga abrir, sua inscrição será confirmada automaticamente.');
        } catch (erro) {
            console.error('Erro na lista de espera:', erro);
            this.mostrarFeedbackGlobal(erro.message || 'Não foi possível atualizar a lista de espera.');
        }
    }

    async carregarResumoOrganizador(idEvento) {
        const campo = document.getElementById('evento-organizador-indicadores');
        if (!campo) return;
        const idUsuario = ApiService.getIdUsuarioLogado();
        if (!idUsuario) return;
        campo.textContent = 'Carregando inscrições...';
        try {
            const resumo = await ApiService.obterResumoOrganizador(idEvento, idUsuario);
            const limite = resumo.limiteVagas == null ? 'sem limite' : resumo.limiteVagas;
            campo.textContent = `${resumo.inscritos}/${limite} inscritos · ${resumo.presentes} presentes · ${resumo.listaEspera} na lista de espera`;
        } catch (erro) {
            console.error('Erro ao carregar resumo do evento:', erro);
            campo.textContent = 'Não foi possível carregar os indicadores.';
        }
    }

    async exportarParticipantesEvento() {
        if (!this.eventoEditando) return;
        const idEvento = this.eventoEditando.idEvento || this.eventoEditando.id;
        const idUsuario = ApiService.getIdUsuarioLogado();
        try {
            const arquivo = await ApiService.exportarParticipantesEvento(idEvento, idUsuario);
            const url = URL.createObjectURL(arquivo);
            const link = document.createElement('a');
            link.href = url;
            link.download = `participantes-evento-${idEvento}.csv`;
            link.click();
            URL.revokeObjectURL(url);
        } catch (erro) {
            console.error('Erro ao exportar participantes:', erro);
            this.mostrarFeedbackGlobal('Não foi possível exportar os participantes.');
        }
    }

    async criarEvento() {
        const idUsuario = ApiService.getIdUsuarioLogado();

        if (!idUsuario) {
            this.fecharModal('evento-modal');
            this.definirModoAuth(false);
            this.abrirModal('auth-modal');
            this.mostrarAuthFeedback("Faça login para organizar um evento!");
            return;
        }

        const titulo =
            document.getElementById('evento-titulo').value.trim();
        const descricao =
            document.getElementById('evento-desc').value.trim();
        const categoria =
            document.getElementById('evento-categoria').value;
        const capa =
            document.getElementById('evento-capa').files[0];
        const dataEvento =
            document.getElementById('evento-data').value;
        const horarioInicio =
            document.getElementById('evento-horario-inicio').value;
        const horarioFim =
            document.getElementById('evento-horario-fim').value;
        const encerramentoInscricoes =
            document.getElementById('evento-encerramento-inscricoes').value;
        const localEvento =
            document.getElementById('evento-local').value.trim();
        const comunidadeValor =
            document.getElementById('evento-comunidade-id').value;
        const limiteValor =
            document.getElementById('evento-limite').value;
        const precoValor =
            document.getElementById('evento-preco').value;

        const precoIngresso =
            precoValor
                ? parseFloat(precoValor)
                : 0;
        const taxaRepassadaComprador =
            document.getElementById('evento-taxa-repassada').value === 'true';
        const exigeCheckin =
            document.getElementById('evento-checkin').checked;

        this.mostrarEventoFeedback('');

        if (!titulo || !dataEvento || !horarioInicio || !horarioFim || !localEvento) {
            return this.mostrarEventoFeedback(
                "Preencha todos os campos obrigatórios (Título, Categoria, Data, Horários e Local)."
            );
        }

        if (!categoria) {
            return this.mostrarEventoFeedback("Selecione uma categoria para o evento.");
        }

        if (capa && !['image/jpeg', 'image/png', 'image/webp'].includes(capa.type)) {
            return this.mostrarEventoFeedback('O banner deve ser uma imagem JPG, PNG ou WEBP.');
        }

        if (capa && capa.size > 5 * 1024 * 1024) {
            return this.mostrarEventoFeedback('O banner deve ter no máximo 5 MB.');
        }

        const inicioEvento = new Date(`${dataEvento}T${horarioInicio}`);
        const fimEvento = new Date(`${dataEvento}T${horarioFim}`);
        const agora = new Date();

        if (fimEvento <= inicioEvento) {
            return this.mostrarEventoFeedback(
                "O horário de término deve ser posterior ao horário de início."
            );
        }

        if (inicioEvento <= agora) {
            return this.mostrarEventoFeedback(
                "A data e horário de início não podem estar no passado."
            );
        }

        if (encerramentoInscricoes) {
            const fimInscricoes = new Date(encerramentoInscricoes);
            if (fimInscricoes > inicioEvento) {
                return this.mostrarEventoFeedback(
                    "O encerramento das inscrições não pode acontecer após o início do evento."
                );
            }
            if (fimInscricoes <= agora) {
                return this.mostrarEventoFeedback(
                    "O encerramento das inscrições deve estar em uma data e horário futuros."
                );
            }
        }

        if (limiteValor && parseInt(limiteValor) < 1) {
            return this.mostrarEventoFeedback(
                "O limite de participantes deve ser de pelo menos 1 pessoa."
            );
        }

        if (precoIngresso < 0) {
            return this.mostrarEventoFeedback(
                "O valor do ingresso não pode ser negativo."
            );
        }

        let latitude = null;
        let longitude = null;
        const latValor = document.getElementById('evento-latitude')?.value;
        const lngValor = document.getElementById('evento-longitude')?.value;
        if (latValor && lngValor) {
            latitude = parseFloat(latValor);
            longitude = parseFloat(lngValor);
        } else {
            const formatoLocal = document.getElementById('evento-formato-local')?.value;
            if (formatoLocal !== 'online' && localEvento) {
                try {
                    const coords = await ApiService.buscarCoordenadasPorEndereco(localEvento);
                    if (coords) {
                        latitude = coords.lat;
                        longitude = coords.lng;
                    }
                } catch (e) {
                    console.warn('GeocodificaÃ§Ã£o automÃ¡tica:', e);
                }
            }
        }

        const novoEvento = {
            titulo,
            descricao,
            categoria: categoria || null,
            dataEvento,
            horarioInicio: horarioInicio + ":00",
            horarioFim: horarioFim + ":00",
            encerramentoInscricoes: encerramentoInscricoes
                ? encerramentoInscricoes + ":00"
                : null,
            localEvento,
            latitude,
            longitude,
            criadorId: parseInt(idUsuario),
            comunidadeId: comunidadeValor
                ? parseInt(comunidadeValor)
                : null,
            limiteParticipantes: limiteValor
                ? parseInt(limiteValor)
                : null,

            precoIngresso,
            taxaRepassadaComprador,

            exigeCheckin
        };

        try {
            this.definirEventoLoading(true);

            const res = await ApiService.criarEventoAPI(novoEvento);

            if (res.ok) {
                const eventoCriado = await res.json();
                const eventoId = eventoCriado.id || eventoCriado.idEvento;
                let bannerEnviado = true;
                if (capa && eventoId) {
                    try {
                        const respostaCapa = await ApiService.enviarCapaEvento(eventoId, capa);
                        bannerEnviado = respostaCapa.ok;
                    } catch (capaErr) {
                        bannerEnviado = false;
                        console.error("Erro ao enviar capa do evento:", capaErr);
                    }
                }

                this.fecharModal('evento-modal');
                this.limparFormularioEvento();
                await this.carregarEventos();
                this.mostrarFeedbackGlobal(capa && !bannerEnviado
                    ? "Evento publicado, mas o banner não foi enviado. Você pode tentar novamente editando o evento."
                    : "🎉 Evento publicado com sucesso!");
            } else {
                this.mostrarEventoFeedback("Não foi possível criar o evento. Verifique os dados e tente novamente.");
            }
        } catch (error) {
            console.error(error);
            this.mostrarEventoFeedback("Erro de conexão ao criar o evento. Tente novamente.");
        } finally {
            this.definirEventoLoading(false);
        }
    }

    gerenciarComunidade(idComunidade) {

        const comunidade = this.listaComunidades.find(
            c => (c.idComunidade || c.id) == idComunidade
        );

        if (!comunidade) return;

        this.comunidadeEditando = comunidade;

        document.getElementById("editar-comunidade-nome").value =
            comunidade.nome;

        document.getElementById("editar-comunidade-desc").value =
            comunidade.descricao;
        document.getElementById("editar-comunidade-categoria").value = comunidade.categoria || '';
        document.getElementById("editar-comunidade-cor").value = comunidade.cor || '#EA3F74';

        this.abrirModal("gerenciar-comunidade-modal");
    }

    async salvarEdicaoComunidade() {

        const nome = document.getElementById("editar-comunidade-nome").value.trim();
        const descricao = document.getElementById("editar-comunidade-desc").value.trim();
        const categoria = document.getElementById("editar-comunidade-categoria").value;
        const cor = document.getElementById("editar-comunidade-cor").value;

        const idComunidade =
            this.comunidadeEditando.idComunidade || this.comunidadeEditando.id;

        const idUsuario = ApiService.getIdUsuarioLogado();
        const imagem = document.getElementById("editar-comunidade-imagem")?.files?.[0];

        const resposta = await ApiService.atualizarComunidade(
            idComunidade,
            idUsuario,
            nome,
            descricao,
            categoria,
            cor
        );

        if (resposta.ok) {
            if (imagem) await ApiService.enviarImagemComunidade(idComunidade, imagem);
            this.fecharModal("gerenciar-comunidade-modal");
            document.getElementById("editar-comunidade-imagem").value = '';

            await this.carregarComunidades();

            if (this.comunidadeAtivaId == idComunidade) {
                this.entrarSubreddit(
                    idComunidade,
                    nome,
                    descricao
                );
            }

        } else {
            this.mostrarFeedbackGlobal("Você não tem permissão para editar esta comunidade.");
        }
    }



    async excluirComunidade(confirmado = false) {
        const idComunidade = this.comunidadeEditando?.idComunidade || this.comunidadeEditando?.id;
        const idUsuario = ApiService.getIdUsuarioLogado();

        if (!idComunidade) return;

        if (!confirmado) {
            this.abrirModalConfirmacao({
                titulo: 'Excluir Comunidade',
                mensagem: 'Tem certeza que deseja excluir esta comunidade? Esta ação não pode ser desfeita.',
                textoConfirmar: 'Excluir',
                classeBotao: 'btn-danger',
                onSuccess: () => this.excluirComunidade(true)
            });
            return;
        }

        const resposta = await ApiService.excluirComunidade(idComunidade, idUsuario);

        if (resposta.ok) {
            this.fecharModal("gerenciar-comunidade-modal");
            await this.carregarComunidades();
            this.mostrarFeedbackGlobal("Comunidade excluída com sucesso!");
        } else {
            this.mostrarFeedbackGlobal("Você não tem permissão para excluir esta comunidade.");
        }
    }



    abrirGerenciarMembros() {

        const lista = document.getElementById("lista-membros-comunidade");

        lista.innerHTML = "";

        this.comunidadeEditando.membros.forEach(membro => {

            const criadorId = this.comunidadeEditando.criador.idUsuario;

            lista.innerHTML += `
    <div class="membro-item">
        <span>${membro.nome}</span>

        ${membro.idUsuario !== criadorId
                    ? `<button class="btn-danger"
                        onclick="app.removerMembro(${membro.idUsuario})">
                        Remover
                   </button>`
                    : `<span style="color: gray;">Administrador</span>`
                }
    </div>
`;

        });

        this.abrirModal("gerenciar-membros-modal");
    }

    async removerMembro(idUsuario, confirmado = false) {
        const idComunidade = this.comunidadeEditando?.idComunidade || this.comunidadeEditando?.id;
        const idSolicitante = ApiService.getIdUsuarioLogado();

        if (!idComunidade || !idUsuario) return;

        if (!confirmado) {
            this.abrirModalConfirmacao({
                titulo: 'Remover Membro',
                mensagem: 'Deseja realmente remover este membro da comunidade?',
                textoConfirmar: 'Remover',
                classeBotao: 'btn-danger',
                onSuccess: () => this.removerMembro(idUsuario, true)
            });
            return;
        }

        const resposta = await ApiService.removerMembro(
            idComunidade,
            idUsuario,
            idSolicitante
        );

        if (resposta.ok) {
            this.mostrarFeedbackGlobal("Membro removido com sucesso!");
            await this.carregarComunidades();

            this.comunidadeEditando = this.listaComunidades.find(
                c => (c.idComunidade || c.id) == idComunidade
            );

            this.abrirGerenciarMembros();
        } else {
            this.mostrarFeedbackGlobal("Você não tem permissão para remover este membro.");
        }
    }
    alternarMenuPost(evento) {

        evento.stopPropagation();

        const botao = evento.currentTarget;

        const wrapper =
            botao.closest('.post-menu-wrapper');

        const menu =
            wrapper?.querySelector('.post-menu');

        if (!menu) return;

        document
            .querySelectorAll('.post-menu.open')
            .forEach(outroMenu => {

                if (outroMenu !== menu) {
                    outroMenu.classList.remove('open');
                }

            });

        menu.classList.toggle('open');
    }

    async compartilharPost(idPost) {

        const url = new URL(window.location.href);

        url.searchParams.set('post', idPost);

        const link = url.toString();

        try {

            await navigator.clipboard.writeText(link);

            this.mostrarFeedbackGlobal('Link da publicação copiado!');

        } catch (erro) {

            console.error(
                'Não foi possível copiar automaticamente:',
                erro
            );

            this.abrirModalCompartilharLink(link);
        }
    }

    abrirModalCompartilharLink(link) {
        const campoLink = document.getElementById('post-link-compartilhado');
        if (!campoLink) {
            this.mostrarFeedbackGlobal('Não foi possível exibir o link da publicação.', 'error');
            return;
        }

        campoLink.value = link;
        this.abrirModal('compartilhar-link-modal');
        setTimeout(() => {
            campoLink.focus();
            campoLink.select();
        }, 0);
    }

    async copiarLinkCompartilhado() {
        const campoLink = document.getElementById('post-link-compartilhado');
        if (!campoLink) return;

        try {
            await navigator.clipboard.writeText(campoLink.value);
            this.fecharModal('compartilhar-link-modal');
            this.mostrarFeedbackGlobal('Link da publicação copiado!', 'success');
        } catch (erro) {
            campoLink.focus();
            campoLink.select();

            try {
                if (document.execCommand('copy')) {
                    this.fecharModal('compartilhar-link-modal');
                    this.mostrarFeedbackGlobal('Link da publicação copiado!', 'success');
                } else {
                    this.mostrarFeedbackGlobal('O link está selecionado. Copie usando Ctrl+C.', 'info');
                }
            } catch (erroCopia) {
                console.error('Não foi possível copiar o link:', erroCopia || erro);
                this.mostrarFeedbackGlobal('O link está selecionado. Copie usando Ctrl+C.', 'info');
            }
        }
    }

    async excluirPost(idPost, confirmado = false) {
        if (!confirmado) {
            this.abrirModalConfirmacao({
                titulo: 'Excluir Publicação',
                mensagem: 'Deseja realmente excluir esta publicação?',
                textoConfirmar: 'Excluir',
                classeBotao: 'btn-danger',
                onSuccess: () => this.excluirPost(idPost, true)
            });
            return;
        }

        const idUsuario = ApiService.getIdUsuarioLogado();
        const resposta = await ApiService.deletarPost(idPost, idUsuario);

        if (resposta.ok) {
            await this.carregarDadosGlobais();

            if (this.comunidadeAtivaId) {
                this.renderizarSubreddit(this.comunidadeAtivaId);
            }
        } else {
            this.mostrarFeedbackGlobal("Você não tem permissão para excluir esta publicação.");
        }
    }

    async gerenciarEvento(idEvento) {

        let evento = this.listaEventos.find(
            e => (e.idEvento || e.id) == idEvento
        );

        if (!evento) {
            try {
                evento = await ApiService.buscarEventoPorId(idEvento);
            } catch (erro) {
                console.error('Erro ao carregar evento para edição:', erro);
                this.mostrarFeedbackGlobal(
                    erro.message || 'Não foi possível carregar o evento para edição.'
                );
                return;
            }
        }

        this.eventoEditando = evento;

        document.getElementById('editar-evento-titulo').value =
            evento.titulo || '';

        document.getElementById('editar-evento-descricao').value =
            evento.descricao || '';

        document.getElementById('editar-evento-categoria').value =
            evento.categoria || '';

        document.getElementById('editar-evento-data').value =
            evento.dataEvento || '';

        document.getElementById('editar-evento-horario-inicio').value =
            evento.horarioInicio
                ? evento.horarioInicio.substring(0, 5)
                : '';

        document.getElementById('editar-evento-horario-fim').value =
            evento.horarioFim
                ? evento.horarioFim.substring(0, 5)
                : '';

        document.getElementById('editar-evento-encerramento-inscricoes').value =
            evento.encerramentoInscricoes
                ? evento.encerramentoInscricoes.substring(0, 16)
                : '';

        document.getElementById('editar-evento-local').value =
            evento.localEvento || '';

        this.popularComunidadesEdicaoEvento(
            evento.comunidadeId
        );

        document.getElementById('editar-evento-limite').value =
            evento.limiteParticipantes || '';

        document.getElementById('editar-evento-preco').value =
            evento.precoIngresso || '';

        document.getElementById('editar-evento-taxa-repassada').value =
            String(evento.taxaRepassadaComprador === true);

        document.getElementById('editar-evento-checkin').checked =
            evento.exigeCheckin === true;

        const latEdit = document.getElementById('editar-evento-latitude');
        const lngEdit = document.getElementById('editar-evento-longitude');
        if (latEdit) latEdit.value = (evento.latitude != null) ? evento.latitude : '';
        if (lngEdit) lngEdit.value = (evento.longitude != null) ? evento.longitude : '';

        const isOnline = /^(https?:\/\/|www\.)/i.test(evento.localEvento || '');
        const formatoSelect = document.getElementById('editar-evento-formato-local');
        if (formatoSelect) formatoSelect.value = isOnline ? 'online' : 'presencial';
        this.alternarFormatoEdicaoEvento(isOnline ? 'online' : 'presencial');

        if (evento.latitude != null && evento.longitude != null) {
            this.atualizarBadgeCoordsEvento('editar-evento-coords-badge', evento.latitude, evento.longitude);
        } else {
            this.atualizarBadgeCoordsEvento('editar-evento-coords-badge', null, null);
        }

        this.abrirModal('gerenciar-evento-modal');
        this.carregarResumoOrganizador(idEvento);
    }

    async salvarEdicaoEvento() {

        if (!this.eventoEditando) {
            return;
        }

        const idUsuario = ApiService.getIdUsuarioLogado();

        const idEvento =
            this.eventoEditando.idEvento || this.eventoEditando.id;

        const titulo =
            document.getElementById('editar-evento-titulo').value.trim();

        const descricao =
            document.getElementById('editar-evento-descricao').value.trim();

        const categoria =
            document.getElementById('editar-evento-categoria').value;

        const dataEvento =
            document.getElementById('editar-evento-data').value;

        const horarioInicio =
            document.getElementById('editar-evento-horario-inicio').value;

        const horarioFim =
            document.getElementById('editar-evento-horario-fim').value;

        const encerramentoInscricoes =
            document.getElementById(
                'editar-evento-encerramento-inscricoes'
            ).value;

        const localEvento =
            document.getElementById('editar-evento-local').value.trim();

        const comunidadeValor =
            document.getElementById('editar-evento-comunidade').value;

        const limiteValor =
            document.getElementById('editar-evento-limite').value;

        const precoValor =
            document.getElementById('editar-evento-preco').value;
        const precoIngresso = precoValor ? Number(precoValor) : 0;
        const taxaRepassadaComprador =
            document.getElementById('editar-evento-taxa-repassada').value === 'true';

        const exigeCheckin =
            document.getElementById('editar-evento-checkin').checked;

        if (
            !titulo ||
            !dataEvento ||
            !horarioInicio ||
            !horarioFim ||
            !localEvento
        ) {
            return this.mostrarFeedbackGlobal(
                "Preencha título, data, horário de início, horário de fim e local."
            );
        }

        const inicioEvento = new Date(
            `${dataEvento}T${horarioInicio}`
        );

        const fimEvento = new Date(
            `${dataEvento}T${horarioFim}`
        );

        if (fimEvento <= inicioEvento) {
            return this.mostrarFeedbackGlobal(
                "O horário de fim deve ser depois do horário de início."
            );
        }

        if (!Number.isFinite(precoIngresso) || precoIngresso < 0) {
            return this.mostrarFeedbackGlobal("O valor do ingresso não pode ser negativo.");
        }

        if (encerramentoInscricoes) {

            const fimInscricoes =
                new Date(encerramentoInscricoes);

            if (fimInscricoes > inicioEvento) {
                return this.mostrarFeedbackGlobal(
                    "O encerramento das inscrições não pode acontecer depois do início do evento."
                );
            }
        }

        let latitude = null;
        let longitude = null;
        const latValor = document.getElementById('editar-evento-latitude')?.value;
        const lngValor = document.getElementById('editar-evento-longitude')?.value;
        if (latValor && lngValor) {
            latitude = parseFloat(latValor);
            longitude = parseFloat(lngValor);
        } else {
            const formatoLocal = document.getElementById('editar-evento-formato-local')?.value;
            if (formatoLocal !== 'online' && localEvento) {
                try {
                    const coords = await ApiService.buscarCoordenadasPorEndereco(localEvento);
                    if (coords) {
                        latitude = coords.lat;
                        longitude = coords.lng;
                    }
                } catch (e) {
                    console.warn('GeocodificaÃ§Ã£o automÃ¡tica na ediÃ§Ã£o:', e);
                }
            }
        }

        const eventoAtualizado = {
            titulo,
            descricao,
            categoria: categoria || null,
            dataEvento,

            horarioInicio: horarioInicio + ":00",
            horarioFim: horarioFim + ":00",

            encerramentoInscricoes:
                encerramentoInscricoes
                    ? encerramentoInscricoes + ":00"
                    : null,

            localEvento,
            latitude,
            longitude,

            comunidadeId:
                comunidadeValor
                    ? parseInt(comunidadeValor)
                    : null,

            limiteParticipantes:
                limiteValor
                    ? parseInt(limiteValor)
                    : null,

            precoIngresso,
            taxaRepassadaComprador,
            exigeCheckin
        };

        try {
            const resposta = await ApiService.atualizarEvento(
                idEvento,
                idUsuario,
                eventoAtualizado
            );

            if (!resposta.ok) {
                const mensagem = await this.mensagemErroApi(
                    resposta,
                    'Não foi possível atualizar o evento. Verifique os dados e tente novamente.'
                );
                this.mostrarFeedbackGlobal(mensagem);
                return;
            }

            this.fecharModal('gerenciar-evento-modal');
            this.eventoEditando = null;
            await this.carregarEventos();
            this.mostrarFeedbackGlobal('Evento atualizado com sucesso!');
        } catch (erro) {
            console.error('Erro ao atualizar evento:', erro);
            this.mostrarFeedbackGlobal(erro.message || 'Não foi possível atualizar o evento.');
        }
    }

    async excluirEvento(confirmado = false) {
        if (!this.eventoEditando) return;

        if (!confirmado) {
            this.abrirModalConfirmacao({
                titulo: 'Excluir Evento',
                mensagem: 'Tem certeza que deseja excluir este evento?',
                textoConfirmar: 'Excluir',
                classeBotao: 'btn-danger',
                onSuccess: () => this.excluirEvento(true)
            });
            return;
        }

        const idUsuario = ApiService.getIdUsuarioLogado();
        const idEvento = this.eventoEditando.idEvento || this.eventoEditando.id;

        const resposta = await ApiService.excluirEvento(idEvento, idUsuario);

        if (resposta.ok) {
            this.fecharModal('gerenciar-evento-modal');
            this.eventoEditando = null;
            await this.carregarEventos();
            this.mostrarFeedbackGlobal("Evento excluído com sucesso!");
        } else {
            this.mostrarFeedbackGlobal("Não foi possível excluir o evento.");
        }
    }

    async cancelarEvento(confirmado = false) {
        if (!this.eventoEditando) return;

        if (!confirmado) {
            this.abrirModalConfirmacao({
                titulo: 'Cancelar Evento',
                mensagem: 'Tem certeza que deseja cancelar este evento?',
                textoConfirmar: 'Confirmar Cancelamento',
                classeBotao: 'btn-danger',
                onSuccess: () => this.cancelarEvento(true)
            });
            return;
        }

        const idEvento = this.eventoEditando.idEvento || this.eventoEditando.id;
        const idUsuario = ApiService.getIdUsuarioLogado();

        const resposta = await ApiService.cancelarEvento(idEvento, idUsuario);

        if (resposta.ok) {
            this.fecharModal('gerenciar-evento-modal');
            this.eventoEditando = null;
            await this.carregarEventos();
            this.mostrarFeedbackGlobal("Evento cancelado com sucesso!");
        } else {
            this.mostrarFeedbackGlobal("Não foi possível cancelar o evento.");
        }
    }

    async participarEvento(idEvento) {

        const idUsuario = ApiService.getIdUsuarioLogado();

        if (!idUsuario) {
            return this.mostrarFeedbackGlobal("Faça login para participar do evento.");
        }

        try {
            const resposta = await ApiService.participarEvento(
                idEvento,
                idUsuario
            );
            if (!resposta.ok) {
                const mensagem = await this.mensagemErroApi(
                    resposta,
                    'Não foi possível participar do evento.'
                );
                this.mostrarFeedbackGlobal(mensagem);
                return;
            }
            await this.carregarEventos();
            await this.abrirDetalhesEvento(idEvento);
        } catch (erro) {
            console.error('Erro ao participar do evento:', erro);
            this.mostrarFeedbackGlobal(erro.message || 'Não foi possível participar do evento.');
        }
    }

    async sairDoEvento(idEvento, confirmado = false) {
        const idUsuario = ApiService.getIdUsuarioLogado();
        if (!idUsuario) return;

        if (!confirmado) {
            this.abrirModalConfirmacao({
                titulo: 'Sair do Evento',
                mensagem: 'Deseja cancelar sua participação neste evento?',
                textoConfirmar: 'Sair',
                classeBotao: 'btn-danger',
                onSuccess: () => this.sairDoEvento(idEvento, true)
            });
            return;
        }

        try {
            const resposta = await ApiService.sairEvento(idEvento, idUsuario);

            if (!resposta.ok) {
                const mensagem = await this.mensagemErroApi(
                    resposta,
                    'Não foi possível cancelar sua participação.'
                );
                this.mostrarFeedbackGlobal(mensagem);
                return;
            }

            await this.carregarEventos();
            await this.abrirDetalhesEvento(idEvento);
        } catch (erro) {
            console.error("Erro ao sair do evento:", erro);
            this.mostrarFeedbackGlobal(erro.message || 'Não foi possível cancelar sua participação.');
        }
    }

    async abrirParticipantesEvento() {

        if (!this.eventoEditando) {
            return;
        }

        const idEvento =
            this.eventoEditando.idEvento || this.eventoEditando.id;

        const participacoes =
            await ApiService.listarParticipantesEvento(idEvento);

        const lista =
            document.getElementById('lista-participantes-evento');

        lista.innerHTML = '';

        if (participacoes.length === 0) {
            lista.innerHTML =
                '<p style="color:var(--text-muted);">Nenhum participante inscrito.</p>';

            this.abrirModal('participantes-evento-modal');
            return;
        }

        participacoes.forEach(participacao => {

            const idUsuario = participacao.usuarioId;

            const nomeUsuario =
                this.usuariosMap[idUsuario] || 'Usuário desconhecido';

            const status =
                participacao.status || 'INSCRITO';

            const presente =
                status === 'PRESENTE';

            lista.innerHTML += `
        <div class="membro-item">
            <div>
                <span>${nomeUsuario}</span>

                <span style="
                    margin-left:8px;
                    font-size:12px;
                    font-weight:600;
                ">
                    ${presente ? 'PRESENTE ✓' : 'INSCRITO'}
                </span>
            </div>

            <button
                class="btn-danger"
                onclick="app.removerParticipanteEvento(${idUsuario})">
                Remover
            </button>
        </div>
    `;
        });

        this.abrirModal('participantes-evento-modal');
    }

    async removerParticipanteEvento(idParticipante, confirmado = false) {
        if (!this.eventoEditando) return;

        if (!confirmado) {
            this.abrirModalConfirmacao({
                titulo: 'Remover Participante',
                mensagem: 'Deseja remover este participante do evento?',
                textoConfirmar: 'Remover',
                classeBotao: 'btn-danger',
                onSuccess: () => this.removerParticipanteEvento(idParticipante, true)
            });
            return;
        }

        const idEvento = this.eventoEditando.idEvento || this.eventoEditando.id;
        const idSolicitante = ApiService.getIdUsuarioLogado();

        const resposta = await ApiService.removerParticipanteEvento(
            idEvento,
            idParticipante,
            idSolicitante
        );

        if (resposta.ok) {
            this.mostrarFeedbackGlobal("Participante removido com sucesso!");
            await this.abrirParticipantesEvento();
            await this.carregarEventos();
        } else {
            this.mostrarFeedbackGlobal("Não foi possível remover o participante.");
        }
    }

    atualizarPaginacaoEventos() {

        const info =
            document.getElementById('eventos-pagina-info');

        const btnAnterior =
            document.getElementById('btn-eventos-anterior');

        const btnProxima =
            document.getElementById('btn-eventos-proxima');

        if (!info || !btnAnterior || !btnProxima) {
            return;
        }

        const paginaAtual = this.paginaEventos + 1;

        const totalPaginas =
            this.totalPaginasEventos || 1;

        info.innerText =
            `Página ${paginaAtual} de ${totalPaginas}`;

        btnAnterior.disabled =
            this.paginaEventos <= 0;

        btnProxima.disabled =
            this.paginaEventos >=
            this.totalPaginasEventos - 1;
    }

    async paginaAnteriorEventos() {

        if (this.paginaEventos <= 0) {
            return;
        }

        this.paginaEventos--;

        await this.carregarEventos();
    }

    async proximaPaginaEventos() {

        if (
            this.paginaEventos >=
            this.totalPaginasEventos - 1
        ) {
            return;
        }

        this.paginaEventos++;

        await this.carregarEventos();
    }

    async filtrarEventos() {

        const texto =
            document.getElementById('filtro-evento-texto')
                .value.trim();

        const preco =
            document.getElementById('filtro-evento-preco')
                .value;

        const categoria =
            document.getElementById('filtro-evento-categoria')
                .value;

        const formato =
            document.getElementById('filtro-evento-formato')
                .value;

        const periodo =
            document.getElementById('filtro-evento-periodo')
                .value;

        const intervalo =
            document.getElementById('filtro-evento-intervalo');

        let dataInicio = null;
        let dataFim = null;

        const hoje = new Date();

        const formatarData = (data) => {
            const ano = data.getFullYear();
            const mes = String(data.getMonth() + 1).padStart(2, '0');
            const dia = String(data.getDate()).padStart(2, '0');

            return `${ano}-${mes}-${dia}`;
        };

        if (periodo === 'hoje') {

            dataInicio = formatarData(hoje);
            dataFim = formatarData(hoje);

            intervalo.style.display = 'none';

        } else if (periodo === 'semana') {

            const inicio = new Date(hoje);

            const diaSemana = inicio.getDay();

            const diferencaParaSegunda =
                diaSemana === 0 ? -6 : 1 - diaSemana;

            inicio.setDate(
                inicio.getDate() + diferencaParaSegunda
            );

            const fim = new Date(inicio);
            fim.setDate(inicio.getDate() + 6);

            dataInicio = formatarData(inicio);
            dataFim = formatarData(fim);

            intervalo.style.display = 'none';

        } else if (periodo === 'mes') {

            const inicio = new Date(
                hoje.getFullYear(),
                hoje.getMonth(),
                1
            );

            const fim = new Date(
                hoje.getFullYear(),
                hoje.getMonth() + 1,
                0
            );

            dataInicio = formatarData(inicio);
            dataFim = formatarData(fim);

            intervalo.style.display = 'none';

        } else if (periodo === 'personalizado') {

            intervalo.style.display = 'flex';

            dataInicio =
                document.getElementById(
                    'filtro-evento-data-inicio'
                ).value || null;

            dataFim =
                document.getElementById(
                    'filtro-evento-data-fim'
                ).value || null;

        } else {

            intervalo.style.display = 'none';
        }

        const latFiltro = (this.filtroProximidadeAtivo && this.coordenadasUsuario) ? this.coordenadasUsuario.lat : null;
        const lngFiltro = (this.filtroProximidadeAtivo && this.coordenadasUsuario) ? this.coordenadasUsuario.lng : null;
        const raioFiltro = Number(document.getElementById('filtro-evento-raio')?.value || 50);

        this.filtrosEventos = {
            texto: texto || null,

            pago: preco ? preco === 'pago' : null,

            categoria:
                categoria || null,

            formato: formato || null,

            dataInicio,
            dataFim,
            lat: latFiltro,
            lng: lngFiltro,
            raioKm: raioFiltro
        };

        this.paginaEventos = 0;

        if (this.visaoEventos === 'meus') await this.carregarMeusEventos();
        else await this.carregarEventos();
    }

    buscarEventosComDebounce() {

        clearTimeout(this.timerBuscaEventos);

        this.timerBuscaEventos = setTimeout(() => {
            this.filtrarEventos();
        }, 400);
    }

    async abrirDetalhesEvento(idEvento) {

        this.salvarViewAtual('evento-detalhes');

        sessionStorage.setItem(
            'eventoDetalhesId',
            idEvento
        );

        let evento = this.listaEventos.find(
            e => (e.idEvento || e.id) == idEvento
        );

        if (!evento) {
            try {
                evento =
                    await ApiService.buscarEventoPorId(idEvento);
            } catch (erro) {
                console.error(erro);
                return this.mostrarFeedbackGlobal("Não foi possível abrir este evento.");
            }
        }

        const idUsuarioLogado = ApiService.getIdUsuarioLogado();
        let quantidadeParticipantes = 0;
        let usuarioParticipa = false;
        let eventoSalvo = false;
        let usuarioNaListaEspera = false;
        try {
            const requisicoes = [
                ApiService.contarParticipantesEvento(idEvento)
            ];
            if (idUsuarioLogado) {
                requisicoes.push(
                    ApiService.listarEventosParticipando(idUsuarioLogado),
                    ApiService.estaEventoSalvo(idEvento, idUsuarioLogado),
                    ApiService.estaNaListaEspera(idEvento, idUsuarioLogado)
                );
            }

            const [totalParticipantes, eventosParticipando = [], salvo = false, naListaEspera = false] =
                await Promise.all(requisicoes);
            quantidadeParticipantes = Number(totalParticipantes) || 0;
            eventoSalvo = salvo;
            usuarioNaListaEspera = naListaEspera;
            usuarioParticipa = eventosParticipando.some(
                eventoParticipando => (eventoParticipando.idEvento || eventoParticipando.id) == idEvento
            );
        } catch (erro) {
            console.error('Erro ao carregar inscrições do evento:', erro);
            this.mostrarFeedbackGlobal(erro.message || 'Não foi possível carregar as informações do evento.');
            return;
        }

        const ehCriador =
            idUsuarioLogado &&
            idUsuarioLogado == evento.criadorId;

        const nomeCriador =
            this.usuariosMap[evento.criadorId] || 'Desconhecido';

        const nomeComunidade =
            evento.comunidadeId
                ? this.comunidadesMap[evento.comunidadeId]
                : null;

        const dataFormatada =
            evento.dataEvento
                ? evento.dataEvento.split('-').reverse().join('/')
                : '';
        const horarioInicioFormatado =
            evento.horarioInicio
                ? evento.horarioInicio.substring(0, 5)
                : '';

        const horarioFimFormatado =
            evento.horarioFim
                ? evento.horarioFim.substring(0, 5)
                : '';

        const eventoCancelado =
            evento.status === 'CANCELADO';

        const eventoLotado =
            evento.limiteParticipantes !== null &&
            quantidadeParticipantes >= evento.limiteParticipantes;

        let limiteInscricao;

        const precoIngresso = Number(evento.precoIngresso || 0);

        const eventoPago = precoIngresso > 0;

        const precoFormatado =
            eventoPago
                ? precoIngresso.toLocaleString('pt-BR', {
                    style: 'currency',
                    currency: 'BRL'
                })
                : 'Gratuito';

        if (evento.encerramentoInscricoes) {
            limiteInscricao = new Date(evento.encerramentoInscricoes);
        } else {
            limiteInscricao = new Date(
                `${evento.dataEvento}T${evento.horarioInicio}`
            );
        }

        const inscricoesEncerradas =
            new Date() >= limiteInscricao;

        const situacaoEvento =
            evento.situacaoTemporal ||
            evento.status ||
            'AGENDADO';

        const situacaoEventoExibida =
            situacaoEvento === 'ACONTECENDO_AGORA'
                ? 'ACONTECENDO AGORA'
                : situacaoEvento;

        const isLink = /^(https?:\/\/|www\.)/i.test(evento.localEvento || '');
        let urlTransmissao = '';
        if (isLink) {
            try {
                const url = new URL(
                    /^https?:\/\//i.test(evento.localEvento)
                        ? evento.localEvento
                        : `https://${evento.localEvento}`
                );
                if (['http:', 'https:'].includes(url.protocol)) {
                    urlTransmissao = this.escaparHtmlDestaque(url.href);
                }
            } catch {
                urlTransmissao = '';
            }
        }
        const eventoEncerrado =
            situacaoEvento === 'ENCERRADO';

        const container =
            document.getElementById('evento-detalhes-conteudo');

        const statusClasse =
            situacaoEvento === 'CANCELADO'
                ? 'cancelado'
                : situacaoEvento === 'ENCERRADO'
                    ? 'encerrado'
                    : situacaoEvento === 'ACONTECENDO_AGORA'
                        ? 'agora'
                        : 'agendado';

        const limiteParticipantesTexto =
            evento.limiteParticipantes
                ? `${quantidadeParticipantes} de ${evento.limiteParticipantes}`
                : `${quantidadeParticipantes}`;

        const limiteInscricaoFormatado =
            limiteInscricao.toLocaleString('pt-BR', {
                dateStyle: 'short',
                timeStyle: 'short'
            });

        container.innerHTML = `
    <article class="evento-detalhes-card">

                        ${evento.imagemCapa
                ? `
                <div
                    class="evento-detalhes-hero"
                    style="
                        background-image:
                        url('${this.escaparHtmlDestaque(this.urlCapaEvento(evento.imagemCapa))}');
                    "
                >
                    <div class="evento-detalhes-hero-overlay"></div>

                    ${evento.categoria
                    ? `
                            <span class="evento-detalhes-categoria">
                                ${this.escaparHtmlDestaque(evento.categoria)}
                            </span>
                        `
                    : ''
                }
                </div>
            `
                : `
                <div class="evento-detalhes-hero evento-detalhes-hero-vazio">
                    <i class="material-icons">event</i>

                    ${evento.categoria
                    ? `
                            <span class="evento-detalhes-categoria">
                                ${this.escaparHtmlDestaque(evento.categoria)}
                            </span>
                        `
                    : ''
                }
                </div>
            `
            }

        <div class="evento-detalhes-conteudo">

            <header class="evento-detalhes-header">

                <div class="evento-detalhes-titulo-area">

                    <span class="evento-detalhes-kicker">
                        <i class="material-icons">event_available</i>
                        Evento
                    </span>

                    <h1>
                        ${this.escaparHtmlDestaque(evento.titulo || 'Evento')}
                    </h1>

                    <p class="evento-detalhes-organizador">
                        Organizado por
                        <strong>
                            ${this.escaparHtmlDestaque(nomeCriador)}
                        </strong>
                    </p>

                </div>

                <span class="
                    evento-status-badge
                    ${statusClasse}
                ">
                    ${situacaoEventoExibida}
                </span>

            </header>


            <section class="evento-detalhes-resumo">

                <div class="evento-info-card">
                    <div class="evento-info-icon">
                        <i class="material-icons">
                            calendar_today
                        </i>
                    </div>

                    <div>
                        <span>Data</span>
                        <strong>
                            ${dataFormatada || 'A confirmar'}
                        </strong>
                    </div>
                </div>


                <div class="evento-info-card">
                    <div class="evento-info-icon">
                        <i class="material-icons">
                            schedule
                        </i>
                    </div>

                    <div>
                        <span>Horário</span>
                        <strong>
                            ${horarioInicioFormatado}
                            às
                            ${horarioFimFormatado}
                        </strong>
                    </div>
                </div>


                <div class="evento-info-card evento-info-local">
                    <div class="evento-info-icon">
                        <i class="material-icons">
                            place
                        </i>
                    </div>

                    <div>
                        <span>Local</span>
                        <strong>
                            ${this.escaparHtmlDestaque(
                evento.localEvento ||
                'A confirmar'
            )}
                        </strong>
                        ${urlTransmissao
                    ? `<a href="${urlTransmissao}" target="_blank" rel="noopener noreferrer" class="evento-rota-btn" aria-label="Acessar transmissão do evento">
                    <i class="material-icons">launch</i> Acessar transmissão
                </a>`
                : (evento.latitude != null && evento.longitude != null
                    ? `<a href="https://www.google.com/maps/search/?api=1&query=${evento.latitude},${evento.longitude}" target="_blank" rel="noopener noreferrer" class="evento-rota-btn" aria-label="Abrir rota no Google Maps" title="Abrir rota no Google Maps">
                        <i class="material-icons">directions</i> Abrir mapa
                    </a>`
                    : (evento.localEvento
                        ? `<a href="https://www.google.com/maps/search/?api=1&query=${encodeURIComponent(evento.localEvento)}" target="_blank" rel="noopener noreferrer" class="evento-rota-btn" aria-label="Abrir local no Google Maps" title="Abrir local no Google Maps">
                            <i class="material-icons">directions</i> Abrir mapa
                        </a>`
                        : ''))}
                    </div>
                </div>


                <div class="evento-info-card">
                    <div class="evento-info-icon">
                        <i class="material-icons">
                            groups
                        </i>
                    </div>

                    <div>
                        <span>Participantes</span>
                        <strong>
                            ${limiteParticipantesTexto}
                        </strong>
                    </div>
                </div>

                <div class="evento-info-card">
    <div class="evento-info-icon">
        <i class="material-icons">confirmation_number</i>
    </div>

    <div>
        <span>Ingresso</span>
        <strong>${precoFormatado}</strong>
    </div>
</div>

            </section>


            <section class="evento-detalhes-section">

                <div class="evento-detalhes-section-title">
                    <i class="material-icons">
                        subject
                    </i>

                    <div>
                        <span>Sobre o evento</span>
                        <small>
                            Informações e descrição
                        </small>
                    </div>
                </div>

                <p class="evento-detalhes-descricao">
                    ${this.escaparHtmlDestaque(
                            evento.descricao ||
                            'Nenhuma descrição foi adicionada para este evento.'
                        )}
                </p>

            </section>


            <section class="evento-detalhes-section">

                <div class="evento-detalhes-section-title">
                    <i class="material-icons">
                        info
                    </i>

                    <div>
                        <span>Informações adicionais</span>
                        <small>
                            Participação e acesso
                        </small>
                    </div>
                </div>

                <div class="evento-detalhes-adicionais">

                    ${nomeComunidade
                ? `
                            <div class="evento-detalhe-linha">
                                <i class="material-icons">
                                    forum
                                </i>

                                <div>
                                    <span>Comunidade</span>
                                    <strong>
                                        ${this.escaparHtmlDestaque(nomeComunidade)}
                                    </strong>
                                </div>
                            </div>
                        `
                : ''
            }

                    <div class="evento-detalhe-linha">
                        <i class="material-icons">
                            confirmation_number
                        </i>

                        <div>
                            <span>Controle de entrada</span>
                            <strong>
                                ${evento.exigeCheckin
                ? 'Check-in obrigatório pelo aplicativo'
                : 'Check-in não obrigatório'
            }
                            </strong>
                        </div>
                    </div>


                    <div class="evento-detalhe-linha">
                        <i class="material-icons">
                            event_busy
                        </i>

                        <div>
                            <span>Inscrições até</span>
                            <strong>
                                ${limiteInscricaoFormatado}
                            </strong>
                        </div>
                    </div>

                </div>

            </section>


            <footer class="evento-detalhes-footer">

                <div class="evento-detalhes-footer-texto">
                    <span>Participação</span>

                    <strong>
                        ${eventoCancelado
                ? 'Este evento foi cancelado'
                : eventoEncerrado
                    ? 'Este evento já foi encerrado'
                    : usuarioParticipa
                        ? 'Você está participando deste evento'
                        : usuarioNaListaEspera
                            ? 'Você está na lista de espera'
                        : 'Garanta sua participação'
            }
                    </strong>
                </div>


                <div class="evento-detalhes-actions">

                    ${idUsuarioLogado &&
                !ehCriador &&
                (usuarioParticipa || usuarioNaListaEspera || (!eventoCancelado && !eventoEncerrado))
                ? usuarioParticipa
                    ? `
                                <button
                                    type="button"
                                    class="evento-action-secondary"
                                    onclick="app.sairDoEvento(${idEvento})"
                                >
                                    <i class="material-icons">
                                        logout
                                    </i>

                                    Sair do evento
                                </button>
                            `
                    : usuarioNaListaEspera
                        ? `
                                <button type="button" class="evento-action-secondary"
                                    onclick="app.entrarNaListaEsperaEvento(${idEvento}, true)">
                                    <i class="material-icons">remove_circle_outline</i>
                                    Sair da lista de espera
                                </button>
                            `
                        : inscricoesEncerradas
                            ? `
                                    <button
                                        type="button"
                                        class="evento-action-disabled"
                                        disabled
                                    >
                                        Inscrições encerradas
                                    </button>
                                `
                        : eventoLotado
                            ? `
                                    <button type="button" class="evento-action-primary"
                                        onclick="app.entrarNaListaEsperaEvento(${idEvento})">
                                        <i class="material-icons">hourglass_top</i>
                                        Entrar na lista de espera
                                    </button>
                                `
                            : `
                                       <button
    type="button"
    class="btn-primary evento-participar-btn"
  onclick="app.acaoIngressoEvento(${idEvento})">
    <i class="material-icons">person_add_alt</i>
   ${eventoPago
                                ? `Comprar ingresso — ${precoFormatado}`
                                : 'Participar gratuitamente'}
</button>
                                    `
                : ''
            }

                    ${idUsuarioLogado
                ? `
                    <button type="button" class="evento-action-secondary"
                        onclick="app.alternarEventoNaAgenda(${idEvento}, ${eventoSalvo})">
                        <i class="material-icons">${eventoSalvo ? 'bookmark_remove' : 'bookmark_add'}</i>
                        ${eventoSalvo ? 'Remover da agenda' : 'Salvar na agenda'}
                    </button>
                `
                : ''
            }


                    ${ehCriador
                ? `
                            <button
                                type="button"
                                class="evento-action-primary"
                                onclick="app.gerenciarEvento(${idEvento})"
                            >
                                <i class="material-icons">
                                    settings
                                </i>

                                Gerenciar evento
                            </button>
                        `
                : ''
            }

                </div>

            </footer>

        </div>

    </article>
`;
        document.querySelectorAll('.view-section')
            .forEach(el => el.classList.remove('active'));

        document.getElementById('view-evento-detalhes')
            .classList.add('active');
    }

    voltarParaEventos() {
        this.mudarAba('eventos');
    }

    async abrirPerfil(idUsuario) {

        this.salvarViewAtual('perfil');
        sessionStorage.setItem(
            'perfilUsuarioId',
            idUsuario
        );

        try {

            const viewAtual =
                document.querySelector('.view-section.active');

            if (viewAtual && viewAtual.id !== 'view-perfil') {
                this.viewAntesDoPerfil =
                    viewAtual.id.replace('view-', '');
            }

            let usuario = null;
            let postsUsuario = [];
            let totalPostsUsuario = null;
            let eventosUsuario = null;


            // =========================
            // USUÁRIO
            // =========================

            try {

                usuario =
                    await ApiService.buscarUsuarioPorId(
                        idUsuario
                    );

            } catch (erro) {

                console.warn(
                    'Falha ao buscar usuário diretamente:',
                    erro
                );

                // Tenta usar os usuários já carregados
                usuario =
                    (this.listaUsuariosCompleta || [])
                        .find(usuario => {

                            const id =
                                usuario.idUsuario ||
                                usuario.id;

                            return String(id) ===
                                String(idUsuario);
                        });
            }


            if (!usuario) {
                throw new Error(
                    'Usuário não encontrado'
                );
            }


            // =========================
            // POSTS
            // =========================

            const [postsResultado, eventosResultado] = await Promise.allSettled([
                ApiService.listarPostsPorUsuario(idUsuario),
                ApiService.listarEventosPorUsuario(idUsuario)
            ]);

            if (
                postsResultado.status === 'fulfilled' &&
                Array.isArray(postsResultado.value)
            ) {
                postsUsuario = postsResultado.value;
                totalPostsUsuario = postsUsuario.length;
            } else {
                console.warn(
                    'Falha ao buscar posts do perfil:',
                    postsResultado.status === 'rejected'
                        ? postsResultado.reason
                        : new Error('A resposta de posts não é uma lista.')
                );

                postsUsuario =
                    (this.posts || [])
                        .filter(post => {
                            const idAutor =
                                post.idUsuario ||
                                post.id_usuario;

                            return String(idAutor) ===
                                String(idUsuario);
                        });
            }

            if (
                eventosResultado.status === 'fulfilled' &&
                Array.isArray(eventosResultado.value)
            ) {
                eventosUsuario = eventosResultado.value;
            } else {
                console.warn(
                    'Falha ao buscar eventos do perfil:',
                    eventosResultado.status === 'rejected'
                        ? eventosResultado.reason
                        : new Error('A resposta de eventos não é uma lista.')
                );
            }

            const idLogado =
                ApiService.getIdUsuarioLogado();

            const ehProprioPerfil =
                idLogado && idLogado == idUsuario;

            const nomeUsuario = usuario.nome || 'Usuário';
            const inicial = nomeUsuario.charAt(0).toUpperCase();
            const fotoUrl = ApiService.formatarUrlFotoPerfil(usuario.fotoPerfil);
            const fotoUrlComCache = fotoUrl
                ? `${fotoUrl}${fotoUrl.includes('?') ? '&' : '?'}v=${Date.now()}`
                : '';

            const container =
                document.getElementById('perfil-conteudo');
            const comunidadesUsuario =
                this.obterComunidadesDoUsuario(idUsuario);
            if (!container) {
                throw new Error(
                    'Container do perfil não encontrado'
                );
            }

            this.usuariosMap[idUsuario] = nomeUsuario;
            this.usuariosFotosMap[idUsuario] = usuario.fotoPerfil;

            const interessesPerfil = Array.isArray(usuario.interesses)
                ? usuario.interesses.filter(interesse =>
                    typeof interesse === 'string' && interesse.trim()
                )
                : [];
            const interessesLogado = ApiService.getUsuarioLogado()?.interesses;
            const interessesUsuario = Array.isArray(interessesLogado)
                ? interessesLogado
                : this.interessesSelecionados;
            const interessesUsuarioNormalizados = new Set(
                interessesUsuario.map(interesse =>
                    String(interesse).trim().toLowerCase()
                )
            );
            const interessesVisiveis = ehProprioPerfil
                ? interessesPerfil
                : idLogado
                    ? interessesPerfil.filter(interesse =>
                        interessesUsuarioNormalizados.has(
                            interesse.trim().toLowerCase()
                        )
                    )
                    : [];
            const comunidadesContexto = ehProprioPerfil
                ? comunidadesUsuario
                : idLogado
                    ? this.obterComunidadesEmComum(idUsuario)
                    : [];
            const interessesHtml = interessesVisiveis.length
                ? `
                    <div class="perfil-contexto-bloco">
                        <span class="perfil-contexto-titulo">
                            <i class="material-icons" aria-hidden="true">interests</i>
                            ${ehProprioPerfil ? 'Seus interesses' : 'Interesses em comum'}
                        </span>
                        <div class="perfil-tags">
                            ${interessesVisiveis.slice(0, 5).map(interesse => `
                                <span class="perfil-tag">${this.escaparHtmlDestaque(interesse)}</span>
                            `).join('')}
                            ${interessesVisiveis.length > 5
                        ? `<span class="perfil-tag perfil-tag-extra">+${interessesVisiveis.length - 5}</span>`
                        : ''}
                        </div>
                    </div>
                `
                : '';
            const comunidadesHtml = comunidadesContexto.length
                ? `
                    <div class="perfil-contexto-bloco">
                        <span class="perfil-contexto-titulo">
                            <i class="material-icons" aria-hidden="true">groups</i>
                            ${ehProprioPerfil ? 'Suas comunidades' : 'Comunidades em comum'}
                        </span>
                        <div class="perfil-tags">
                            ${comunidadesContexto.slice(0, 4).map(comunidade => `
                                <span class="perfil-tag">${this.escaparHtmlDestaque(comunidade.nome || 'Comunidade')}</span>
                            `).join('')}
                            ${comunidadesContexto.length > 4
                        ? `<span class="perfil-tag perfil-tag-extra">+${comunidadesContexto.length - 4}</span>`
                        : ''}
                        </div>
                    </div>
                `
                : '';
            const contextoSocialHtml = interessesHtml || comunidadesHtml
                ? `<div class="perfil-contexto-social">${interessesHtml}${comunidadesHtml}</div>`
                : idLogado && !ehProprioPerfil
                    ? `
                        <div class="perfil-contexto-vazio">
                            <i class="material-icons" aria-hidden="true">diversity_3</i>
                            Ainda não identificamos interesses ou comunidades em comum.
                        </div>
                    `
                    : '';

            container.innerHTML = `
                <section class="card perfil-hero">
                    <div class="perfil-capa" aria-hidden="true"></div>
                    <div class="perfil-hero-conteudo">
                        <div class="perfil-header">
                            <div class="perfil-identidade">
                                <div class="perfil-avatar">
                                    ${fotoUrlComCache
                    ? `
                                        <img src="${this.escaparHtmlDestaque(fotoUrlComCache)}"
                                            alt="" onerror="this.hidden=true; this.nextElementSibling.hidden=false">
                                        <span class="perfil-avatar-inicial" hidden>${this.escaparHtmlDestaque(inicial)}</span>
                                    `
                    : `<span class="perfil-avatar-inicial">${this.escaparHtmlDestaque(inicial)}</span>`
                }
                                </div>
                                <div class="perfil-identidade-texto">
                                    <h1>${this.escaparHtmlDestaque(nomeUsuario)}</h1>
                                    <span class="perfil-username">@${this.escaparHtmlDestaque(usuario.username || 'usuario')}</span>
                                </div>
                            </div>

                            <div class="perfil-header-acao">
                                ${ehProprioPerfil ? `
                                    <button type="button" class="btn-perfil-acao btn-perfil-editar"
                                        onclick="app.abrirEdicaoPerfil()">
                                        <i class="material-icons" aria-hidden="true">edit</i>
                                        Editar perfil
                                    </button>
                                ` : idLogado ? `
                                    <button type="button"
                                        class="btn-perfil-acao btn-perfil-seguir ${this.usuariosSeguidos.has(Number(idUsuario)) ? 'seguindo' : ''}"
                                        data-follow-user-id="${Number(idUsuario)}"
                                        aria-pressed="${this.usuariosSeguidos.has(Number(idUsuario))}"
                                        onclick="app.alternarSeguirUsuario(${Number(idUsuario)})">
                                        <i class="material-icons" aria-hidden="true">${this.usuariosSeguidos.has(Number(idUsuario)) ? 'check' : 'person_add'}</i>
                                        ${this.usuariosSeguidos.has(Number(idUsuario)) ? 'Seguindo' : 'Seguir'}
                                    </button>
                                    <button type="button" class="btn-perfil-acao btn-perfil-mensagem"
                                        onclick="app.iniciarConversa(${Number(idUsuario)})">
                                        <i class="material-icons" aria-hidden="true">chat_bubble_outline</i>
                                        Mensagem
                                    </button>
                                ` : ''}
                            </div>
                        </div>

                        <section class="perfil-sobre" aria-label="Sobre">
                            <h2>Sobre</h2>
                            <p>${this.escaparHtmlDestaque(
                    usuario.bio || 'Este usuário ainda não adicionou uma bio.'
                )}</p>
                        </section>

                        ${contextoSocialHtml}

                    </div>
                </section>

                <div class="perfil-tabs">
                    <div class="perfil-tab active" id="perfil-tab-publicacoes"
                        onclick="app.mostrarAbaPerfil('publicacoes', ${idUsuario})">
                        <span>Publicações</span>
                        <strong>${totalPostsUsuario === null ? '—' : totalPostsUsuario}</strong>
                    </div>
                    <div class="perfil-tab" id="perfil-tab-comunidades"
                        onclick="app.mostrarAbaPerfil('comunidades', ${idUsuario})">
                        <span>Comunidades</span>
                        <strong>${comunidadesUsuario.length}</strong>
                    </div>
                    <div class="perfil-tab" id="perfil-tab-eventos"
                        onclick="app.mostrarAbaPerfil('eventos', ${idUsuario})">
                        <span>Eventos</span>
                        <strong>${eventosUsuario === null ? '—' : eventosUsuario.length}</strong>
                    </div>
                </div>

                <div id="perfil-conteudo-abas"></div>
            `;

            document.querySelectorAll('.view-section')
                .forEach(el => el.classList.remove('active'));

            document.getElementById('view-perfil')
                .classList.add('active');

            this.renderizarPostsPerfil(postsUsuario);

        } catch (erro) {

            console.error(erro);
            this.mostrarFeedbackGlobal("Não foi possível carregar o perfil.");
        }

    }

    async abrirMeuPerfil() {

        const idUsuario =
            ApiService.getIdUsuarioLogado();

        if (!idUsuario) {
            return this.mostrarFeedbackGlobal("Faça login primeiro.");
        }

        await this.abrirPerfil(idUsuario);
    }

    voltarDoPerfil() {

        document.querySelectorAll('.view-section')
            .forEach(el => el.classList.remove('active'));

        const viewAnterior =
            document.getElementById(
                `view-${this.viewAntesDoPerfil}`
            );

        if (viewAnterior) {
            viewAnterior.classList.add('active');
        } else {
            document.getElementById('view-feed')
                .classList.add('active');
        }

        document.querySelectorAll('.nav-btn')
            .forEach(el => el.classList.remove('active'));

        const botaoAnterior =
            document.getElementById(
                `tab-${this.viewAntesDoPerfil}`
            );

        if (botaoAnterior) {
            botaoAnterior.classList.add('active');
        }
    }

    async abrirEdicaoPerfil() {

        const idUsuario =
            ApiService.getIdUsuarioLogado();

        if (!idUsuario) {
            return;
        }

        try {

            const usuario =
                await ApiService.buscarUsuarioPorId(idUsuario);

            document.getElementById(
                'editar-perfil-nome'
            ).value = usuario.nome || '';

            document.getElementById(
                'editar-perfil-username'
            ).value = usuario.username || '';

            document.getElementById(
                'editar-perfil-bio'
            ).value = usuario.bio || '';

            const fotoUrl = ApiService.formatarUrlFotoPerfil(usuario.fotoPerfil);
            this.mostrarPreviewFotoPerfil(fotoUrl);
            document.getElementById('editar-perfil-foto').value = '';

            this.abrirModal('editar-perfil-modal');

        } catch (erro) {

            console.error(erro);
            this.mostrarFeedbackGlobal("Não foi possível carregar os dados do perfil.");
        }
    }

    async salvarPerfil() {

        const idUsuario =
            ApiService.getIdUsuarioLogado();

        if (!idUsuario) {
            return;
        }

        const nome =
            document.getElementById(
                'editar-perfil-nome'
            ).value.trim();

        const username =
            document.getElementById(
                'editar-perfil-username'
            ).value.trim();

        const bio =
            document.getElementById(
                'editar-perfil-bio'
            ).value.trim();

        if (!nome) {
            return this.mostrarFeedbackGlobal("O nome não pode ficar vazio.");
        }

        if (!username) {
            return this.mostrarFeedbackGlobal("O username não pode ficar vazio.");
        }

        if (bio.length > 300) {
            return this.mostrarFeedbackGlobal(
                "A bio pode ter no máximo 300 caracteres."
            );
        }

        try {

            const resposta =
                await ApiService.atualizarPerfil(
                    idUsuario,
                    nome,
                    username,
                    bio
                );

            if (!resposta.ok) {
                return this.mostrarFeedbackGlobal(
                    "Não foi possível atualizar o perfil."
                );
            }

            const usuarioAtualizado =
                await resposta.json();

            // Atualiza também o usuário salvo no navegador
            ApiService.salvarSessao(usuarioAtualizado);

            this.fecharModal('editar-perfil-modal');

            // Atualiza o nome mostrado no menu lateral
            this.atualizarMenuLateral();

            // Renderiza novamente o perfil já atualizado
            await this.abrirPerfil(idUsuario);

            this.mostrarFeedbackGlobal("Perfil atualizado com sucesso!");

        } catch (erro) {

            console.error(erro);

            this.mostrarFeedbackGlobal(
                "Erro de conexão ao atualizar o perfil."
            );
        }
    }

    renderizarPostsPerfil(posts) {

        const container =
            document.getElementById('perfil-conteudo-abas');

        if (!container) {
            return;
        }

        if (!posts || posts.length === 0) {
            container.innerHTML = `
        <div class="perfil-empty">
            <div class="perfil-empty-icon">
                <i class="material-icons">article</i>
            </div>

            <strong>Nenhuma publicação ainda</strong>

            <span>
                As publicações deste usuário aparecerão aqui.
            </span>
        </div>
    `;
            return;
        }

        this.montarCardsPosts(
            posts,
            'perfil-conteudo-abas',
            false
        );
    }

    renderizarComunidadesPerfil(comunidades, idUsuario) {

        const container =
            document.getElementById('perfil-conteudo-abas');

        if (!container) {
            return;
        }

        if (!comunidades || comunidades.length === 0) {
            container.innerHTML = `
        <div class="perfil-empty">
            <div class="perfil-empty-icon">
                <i class="material-icons">groups</i>
            </div>

            <strong>Nenhuma comunidade ainda</strong>

            <span>
                As comunidades deste usuário aparecerão aqui.
            </span>
        </div>
    `;
            return;
        }

        container.innerHTML = '';

        comunidades.forEach(comunidade => {

            const idComunidade =
                comunidade.idComunidade || comunidade.id;

            const idAdministrador =
                comunidade.criador?.idUsuario ||
                comunidade.criador?.id;

            const ehAdministrador =
                idAdministrador == idUsuario;

            const div = document.createElement('div');

            div.className = 'list-item';

            div.innerHTML = `
            <div class="list-item-info">

                <div style="
                    display:flex;
                    align-items:center;
                    gap:8px;
                    margin-bottom:4px;
                ">
                    <h3 style="margin:0;">
                        ${comunidade.nome}
                    </h3>

                    ${ehAdministrador
                    ? `
                                <span style="
                                    font-size:11px;
                                    font-weight:700;
                                    padding:3px 7px;
                                    border-radius:12px;
                                    background:var(--bg-light);
                                    color:var(--primary-blue);
                                ">
                                    Administrador
                                </span>
                            `
                    : ''
                }
                </div>

                <p>
                    ${comunidade.descricao || 'Sem descrição.'}
                </p>

                <small style="color:var(--text-muted);">
                    ${comunidade.membros
                    ? comunidade.membros.length
                    : 0
                } membro(s)
                </small>

            </div>

            <button
                class="btn-primary"
                onclick="app.abrirComunidadeDoPerfil(${idComunidade})">
                Acessar
            </button>
        `;

            container.appendChild(div);
        });
    }
    renderizarEventoDestaque(evento) {
        const container =
            document.getElementById('evento-destaque');

        if (!container) return;

        if (!evento) {
            container.innerHTML = `
            <div class="evento-destaque-vazio">
                Nenhum evento futuro disponível no momento.
            </div>
        `;
            return;
        }

        const data = evento.dataEvento
            ? new Date(
                `${evento.dataEvento}T00:00:00`
            ).toLocaleDateString('pt-BR', {
                day: '2-digit',
                month: 'short'
            }).replace('.', '')
            : '--';

        const horario =
            evento.horarioInicio?.substring(0, 5) ||
            '--:--';

        const titulo =
            this.escaparHtmlDestaque(
                evento.titulo || 'Evento'
            );

        const descricao =
            this.escaparHtmlDestaque(
                evento.descricao ||
                'Confira os detalhes deste evento.'
            );

        const local =
            this.escaparHtmlDestaque(
                evento.localEvento ||
                'Local a confirmar'
            );

        const categoria =
            this.escaparHtmlDestaque(
                evento.categoria ||
                'Evento'
            );

        const imagemUrl = evento.imagemCapa
            ? this.escaparHtmlDestaque(
                this.urlCapaEvento(evento.imagemCapa)
            )
            : '';

        const imagem = imagemUrl
            ? `
            <div
                class="evento-destaque-imagem"
                style="
                    background-image:
                    url('${imagemUrl}');
                "
            ></div>
        `
            : `
            <div class="
                evento-destaque-imagem
                evento-destaque-imagem-vazia
            ">
                <i class="material-icons">
                    event
                </i>
            </div>
        `;

        const idEvento = Number(evento.idEvento || evento.id);

        container.innerHTML = `
        <div class="evento-destaque-container">

            ${imagem}

            <div class="evento-destaque-copy">

                <div class="evento-destaque-topo">
                    <span class="evento-destaque-badge">
                        ${categoria}
                    </span>

                    <span class="evento-destaque-data">
                        ${this.escaparHtmlDestaque(data)}
                    </span>
                </div>

                <h2 class="evento-destaque-titulo">
                    ${titulo}
                </h2>

                <p class="evento-destaque-descricao">
                    ${descricao}
                </p>

                <div class="evento-destaque-info">

                    <span>
                        <i class="material-icons">
                            schedule
                        </i>
                        ${this.escaparHtmlDestaque(horario)}
                    </span>

                    <span>
                        <i class="material-icons">
                            location_on
                        </i>
                        ${local}
                    </span>

                </div>

                <button
                    type="button"
                    class="evento-destaque-btn"
                    onclick="
                        app.abrirDetalhesEvento(${idEvento})
                    "
                >
                    Ver detalhes

                    <i class="material-icons">
                        arrow_forward
                    </i>
                </button>

            </div>

        </div>
    `;
    }

    async carregarEventoDestaqueMaisPopular() {
        try {
            const resultado = await ApiService.buscarEventos({
                page: 0,
                size: 100
            });

            const eventos =
                Array.isArray(resultado)
                    ? resultado
                    : (resultado.content || []);

            const agora = new Date();
            const eventosAtivos = eventos.filter(evento => {
                const situacaoTemporal = evento.situacaoTemporal;
                if (situacaoTemporal === 'ENCERRADO' || situacaoTemporal === 'CANCELADO'
                    || evento.status === 'CANCELADO'
                    || !evento.dataEvento || !evento.horarioFim) {
                    return false;
                }

                const fimEvento = new Date(
                    `${evento.dataEvento}T${evento.horarioFim}`
                );

                return !Number.isNaN(fimEvento.getTime()) && fimEvento > agora;
            });

            if (!eventosAtivos.length) {
                this.renderizarEventoDestaque(null);
                return;
            }

            const eventoMaisPopular =
                [...eventosAtivos].sort((a, b) => {
                    const participantesA =
                        Number(a.quantidadeParticipantes || 0);

                    const participantesB =
                        Number(b.quantidadeParticipantes || 0);

                    return participantesB - participantesA;
                })[0];

            this.renderizarEventoDestaque(
                eventoMaisPopular
            );

        } catch (erro) {
            console.error(
                'Erro ao carregar evento em destaque:',
                erro
            );

            this.renderizarEventoDestaque(null);
        }
    }

    async abrirEventoDoPerfil(idEvento) {
        await this.abrirDetalhesEvento(idEvento);
    }

    renderizarEventosPerfil(eventos) {

        const container =
            document.getElementById('perfil-conteudo-abas');

        if (!container) {
            return;
        }

        if (!eventos || eventos.length === 0) {
            container.innerHTML = `
        <div class="perfil-empty">
            <div class="perfil-empty-icon">
                <i class="material-icons">event</i>
            </div>

            <strong>Nenhum evento ainda</strong>

            <span>
                Os eventos deste usuário aparecerão aqui.
            </span>
        </div>
    `;
            return;
        }

        container.innerHTML = '';

        eventos.forEach(evento => {

            const idEvento =
                evento.idEvento || evento.id;

            const div =
                document.createElement('div');

            div.className = 'list-item';

            const dataEvento =
                evento.dataEvento ||
                evento.data ||
                evento.dataInicio;

            let dataFormatada = '';

            if (dataEvento) {

                try {
                    dataFormatada =
                        new Date(dataEvento)
                            .toLocaleDateString('pt-BR');
                } catch (erro) {
                    dataFormatada = dataEvento;
                }

            }

            div.innerHTML = `
            <div class="list-item-info">

                <div style="
                    display:flex;
                    align-items:center;
                    gap:8px;
                    margin-bottom:4px;
                ">

                    <h3 style="margin:0;">
                        ${this.escaparHtmlDestaque(evento.nome || evento.titulo || 'Evento')}
                    </h3>

                </div>

                <p>
                    ${this.escaparHtmlDestaque(evento.descricao || 'Sem descrição.')}
                </p>

                ${dataFormatada ? `
                    <small style="color:var(--text-muted);">
                        ${this.escaparHtmlDestaque(dataFormatada)}
                    </small>
                ` : ''}

            </div>

            <button
                class="btn-primary"
                onclick="app.abrirDetalhesEvento(${idEvento})"
            >
                Acessar
            </button>
        `;

            container.appendChild(div);

        });

    }

    async mostrarAbaPerfil(aba, idUsuario) {

        document.querySelectorAll('.perfil-tab')
            .forEach(tab => tab.classList.remove('active'));

        const abaSelecionada =
            document.getElementById(`perfil-tab-${aba}`);

        if (abaSelecionada) {
            abaSelecionada.classList.add('active');
        }

        if (aba === 'publicacoes') {

            const posts =
                await ApiService.listarPostsPorUsuario(idUsuario);

            this.renderizarPostsPerfil(posts);
            return;
        }

        if (aba === 'comunidades') {

            const comunidadesMembro =
                await ApiService.listarComunidadesPorUsuario(
                    idUsuario
                );
            const comunidadesCriadas =
                (this.listaComunidades || [])
                    .filter(comunidade =>
                        String(
                            comunidade.criador?.idUsuario ||
                            comunidade.criador?.id
                        ) === String(idUsuario)
                    );
            const comunidadesPorId = new Map();
            [...comunidadesMembro, ...comunidadesCriadas]
                .forEach(comunidade =>
                    comunidadesPorId.set(
                        String(comunidade.idComunidade || comunidade.id),
                        comunidade
                    )
                );

            this.renderizarComunidadesPerfil(
                [...comunidadesPorId.values()],
                idUsuario
            );

            return;
        }

        if (aba === 'eventos') {

            const eventos =
                await ApiService.listarEventosPorUsuario(
                    idUsuario
                );

            this.renderizarEventosPerfil(eventos);

            return;
        }
    }

    async abrirComunidadeDoPerfil(idComunidade) {

        await this.carregarComunidades();

        const comunidade =
            this.listaComunidades.find(
                c => (c.idComunidade || c.id) == idComunidade
            );

        if (!comunidade) {
            return this.mostrarFeedbackGlobal("Comunidade não encontrada.");
        }

        this.entrarSubreddit(
            idComunidade,
            comunidade.nome,
            comunidade.descricao
        );
    }

    // Envia a foto em multipart; a API persiste o arquivo e devolve o caminho salvo na sessão do navegador.
    async salvarFotoPerfil() {
        const idUsuario =
            ApiService.getIdUsuarioLogado();

        const input =
            document.getElementById('editar-perfil-foto');

        const foto = input?.files?.[0];

        if (!foto) {
            return this.mostrarFeedbackGlobal("Selecione uma imagem.");
        }

        if (!['image/jpeg', 'image/png', 'image/webp'].includes(foto.type)) {
            return this.mostrarFeedbackGlobal("Use uma foto JPG, PNG ou WEBP.");
        }

        if (foto.size > 5 * 1024 * 1024) {
            return this.mostrarFeedbackGlobal("A imagem deve ter no máximo 5 MB.");
        }
        try {
            const resposta =
                await ApiService.atualizarFotoPerfil(
                    idUsuario,
                    foto
                );

            if (!resposta.ok) {

                const erro = await resposta.text();

                console.error("Resposta do backend:", erro);

                return this.mostrarFeedbackGlobal(
                    "Não foi possível alterar a foto."
                );
            }

            const usuarioAtualizado =
                await resposta.json();

            const usuarioSessao =
                ApiService.getUsuarioLogado();

            ApiService.salvarSessao({
                ...usuarioSessao,
                fotoPerfil: usuarioAtualizado.fotoPerfil
            });

            this.usuariosFotosMap[idUsuario] = usuarioAtualizado.fotoPerfil;

            input.value = '';
            if (this.previewFotoPerfilUrl) {
                URL.revokeObjectURL(this.previewFotoPerfilUrl);
                this.previewFotoPerfilUrl = null;
            }
            const fotoAtualizadaUrl = ApiService.formatarUrlFotoPerfil(
                usuarioAtualizado.fotoPerfil
            );
            this.mostrarPreviewFotoPerfil(fotoAtualizadaUrl);

            this.fecharModal('editar-perfil-modal');

            await this.abrirPerfil(idUsuario);

            this.atualizarMenuLateral();
            this.renderizarFeedGeral();

            this.mostrarFeedbackGlobal("Foto alterada com sucesso!");
        } catch (erro) {

            console.error(
                "ERRO NO UPLOAD:",
                erro
            );

            this.mostrarFeedbackGlobal("Erro ao enviar a foto.");
        }
    }

    salvarViewAtual(view) {
        sessionStorage.setItem('viewAtual', view);
    }

    getViewSalva() {
        return sessionStorage.getItem('viewAtual') || 'feed';
    }

    async carregarConversas() {
        const idUsuario = ApiService.getIdUsuarioLogado();

        if (!idUsuario) {
            this.listaConversas = [];
            this.renderizarConversas();
            return;
        }

        try {
            this.listaConversas =
                await ApiService.listarConversasDoUsuario(idUsuario);

            this.renderizarConversas();

        } catch (erro) {
            console.error('Erro ao carregar conversas:', erro);
        }
    }

    renderizarConversas(conversas = this.listaConversas, termoBusca = '') {
        const container =
            document.getElementById('chat-conversations');

        if (!container) {
            return;
        }

        if (conversas.length === 0) {
            const mensagemVazia = termoBusca
                ? 'Nenhuma conversa encontrada.'
                : 'Nenhuma conversa ainda.';

            container.innerHTML = `
            <p class="chat-empty">
                ${mensagemVazia}
            </p>
        `;
            return;
        }

        container.innerHTML = '';

        conversas.forEach(conversa => {
            const item = document.createElement('div');

            item.className = 'chat-conversation-item';

            if (this.conversaAtiva && String(conversa.idConversa) === String(this.conversaAtiva.idConversa)) {
                item.classList.add('active');
            }

            const nome =
                conversa.nomeOutroUsuario || 'Usuário';

            const username =
                conversa.usernameOutroUsuario
                    ? `@${conversa.usernameOutroUsuario}`
                    : '';

            const avatar = document.createElement('div');
            avatar.className = 'chat-conversation-avatar';
            this.atualizarAvatarChat(
                avatar,
                nome,
                conversa.fotoPerfilOutroUsuario
            );

            const info = document.createElement('div');
            info.className = 'chat-conversation-info';
            const nomeElemento = document.createElement('strong');
            nomeElemento.textContent = nome;
            const usernameElemento = document.createElement('span');
            usernameElemento.textContent = username;
            info.append(nomeElemento, usernameElemento);
            item.append(avatar, info);

            item.addEventListener('click', () => {
                this.abrirConversa(conversa);
            });

            container.appendChild(item);
        });
    }

    async abrirConversa(conversa) {
        this.conversaAtiva = conversa;

        const janela =
            document.getElementById('chat-window');

        const nome =
            document.getElementById('chat-user-name');

        const username =
            document.getElementById('chat-user-username');

        const avatar =
            document.getElementById('chat-user-avatar');

        const status =
            document.getElementById('chat-user-status');

        if (!janela) {
            return;
        }

        const nomeExibicao =
            conversa.nomeOutroUsuario || 'Usuário';

        nome.textContent = nomeExibicao;

        username.textContent =
            conversa.usernameOutroUsuario
                ? `@${conversa.usernameOutroUsuario}`
                : 'Usuário';

        status.textContent = 'online';
        status.classList.add('active');

        this.atualizarAvatarChat(
            avatar,
            nomeExibicao,
            conversa.fotoPerfilOutroUsuario
        );

        janela.classList.add('active');
        const mobileToggle = document.getElementById('chat-mobile-toggle');
        if (mobileToggle && window.matchMedia('(max-width: 760px)').matches) {
            mobileToggle.hidden = true;
            document.getElementById('chat-sidebar')?.classList.remove('mobile-open');
            mobileToggle.setAttribute('aria-expanded', 'false');
        }

        await this.carregarMensagens(
            conversa.idConversa
        );
    }

    async carregarMensagens(idConversa) {
        const container =
            document.getElementById('chat-messages');

        if (!container) {
            return;
        }

        container.innerHTML = `
        <p class="chat-empty">
            Carregando...
        </p>
    `;

        try {
            const mensagens =
                await ApiService.listarMensagens(idConversa);

            this.renderizarMensagens(mensagens);

        } catch (erro) {
            console.error(
                'Erro ao carregar mensagens:',
                erro
            );

            container.innerHTML = `
            <p class="chat-empty">
                Não foi possível carregar as mensagens.
            </p>
        `;
        }
    }

    renderizarMensagens(mensagens) {
        const container =
            document.getElementById('chat-messages');

        if (!container) {
            return;
        }

        const idUsuario =
            ApiService.getIdUsuarioLogado();

        if (mensagens.length === 0) {
            container.innerHTML =
                `<p class="chat-empty">
                Nenhuma mensagem ainda.
            </p>`;

            return;
        }

        container.innerHTML = '';

        mensagens.forEach(mensagem => {

            const enviadaPorMim =
                String(mensagem.idRemetente) ===
                String(idUsuario);

            const linha =
                document.createElement('div');

            linha.className =
                enviadaPorMim
                    ? 'chat-message-row sent'
                    : 'chat-message-row received';


            const bolha =
                document.createElement('div');

            bolha.className =
                'chat-message-bubble';


            const autor =
                document.createElement('span');

            autor.className =
                'chat-message-author';

            autor.textContent =
                enviadaPorMim
                    ? 'Você'
                    : mensagem.nomeRemetente || 'Usuário';


            const conteudo =
                document.createElement('span');

            conteudo.className =
                'chat-message-content';

            conteudo.textContent =
                mensagem.conteudo;


            bolha.appendChild(autor);
            bolha.appendChild(conteudo);

            linha.appendChild(bolha);

            container.appendChild(linha);
        });

        container.scrollTop =
            container.scrollHeight;
    }

    async enviarMensagemChat(event) {
        event.preventDefault();

        if (!this.conversaAtiva) {
            return;
        }

        const form = document.getElementById('chat-message-form');
        const input = document.getElementById('chat-message-input');
        const botaoEnviar = form?.querySelector('button[type="submit"]');

        if (!input || !form) {
            return;
        }

        const conteudo = input.value.trim();

        if (!conteudo) {
            return;
        }

        const idUsuario = ApiService.getIdUsuarioLogado();

        form.classList.add('sending');
        input.disabled = true;
        botaoEnviar.disabled = true;
        botaoEnviar.textContent = 'Enviando...';

        try {
            await ApiService.enviarMensagem(
                this.conversaAtiva.idConversa,
                idUsuario,
                conteudo
            );

            input.value = '';
            await this.carregarMensagens(this.conversaAtiva.idConversa);
            input.focus();

        } catch (erro) {
            console.error(
                'Erro ao enviar mensagem:',
                erro
            );

            this.mostrarFeedbackGlobal(
                erro.message ||
                'Não foi possível enviar a mensagem.'
            );
        } finally {
            form.classList.remove('sending');
            input.disabled = false;
            botaoEnviar.disabled = false;
            botaoEnviar.textContent = 'Enviar';
            input.focus();
        }
    }

    async iniciarConversa(idOutroUsuario) {
        const idUsuario =
            ApiService.getIdUsuarioLogado();

        if (!idUsuario) {
            this.mostrarFeedbackGlobal('Você precisa estar logado para enviar mensagens.');
            return;
        }

        if (String(idUsuario) === String(idOutroUsuario)) {
            return;
        }

        try {
            const conversa =
                await ApiService.criarOuBuscarConversa(
                    idUsuario,
                    idOutroUsuario
                );

            // Atualiza a lista da lateral
            await this.carregarConversas();

            // Abre imediatamente a conversa criada/encontrada
            await this.abrirConversa(conversa);

        } catch (erro) {
            console.error(
                'Erro ao iniciar conversa:',
                erro
            );

            this.mostrarFeedbackGlobal(
                erro.message ||
                'Não foi possível iniciar a conversa.'
            );
        }
    }

    // O WebSocket entrega mensagens em tempo real; CONNECTING também é verificado para evitar conexões duplicadas.
    conectarWebSocketChat() {

        const idUsuario =
            ApiService.getIdUsuarioLogado();

        if (!idUsuario) {
            return;
        }

        if (
            this.chatSocket &&
            (
                this.chatSocket.readyState === WebSocket.CONNECTING ||
                this.chatSocket.readyState === WebSocket.OPEN
            )
        ) {
            return;
        }

        const url = new URL('/ws/chat', ApiService.API_URL);
        url.protocol = url.protocol === 'https:' ? 'wss:' : 'ws:';
        url.searchParams.set('usuarioId', idUsuario);
        url.searchParams.set('token', ApiService.getAuthToken());

        const socket = new WebSocket(url);
        this.chatSocket = socket;


        socket.addEventListener(
            'open',
            () => {
                console.log(
                    'CHAT WS: conectado'
                );
            }
        );

        socket.addEventListener(
            'message',
            (event) => {

                try {

                    const mensagem =
                        JSON.parse(event.data);

                    console.log(
                        'CHAT WS: mensagem recebida',
                        mensagem
                    );

                    this.receberMensagemWebSocket(
                        mensagem
                    );

                } catch (erro) {

                    console.error(
                        'CHAT WS: erro ao processar mensagem',
                        erro
                    );

                }

            }
        );


        socket.addEventListener(
            'close',
            (evento) => {
                const encerramentoNormal =
                    evento.code === 1000 || evento.code === 1001;

                console[encerramentoNormal ? 'info' : 'warn'](
                    `CHAT WS: conexão encerrada (${evento.code}${evento.reason ? `: ${evento.reason}` : ''})`
                );

                if (this.chatSocket === socket) {
                    this.chatSocket = null;
                }
            }
        );


        socket.addEventListener(
            'error',
            (erro) => {
                console.error(
                    'CHAT WS: erro',
                    erro
                );
            }
        );
    }

    async receberMensagemWebSocket(mensagem) {

        if (!mensagem) {
            return;
        }

        /*
         * Atualiza a lista lateral.
         * Por enquanto é simples e seguro.
         */
        await this.carregarConversas();


        /*
         * Se a conversa recebida estiver aberta,
         * recarrega o histórico imediatamente.
         */
        if (
            this.conversaAtiva &&
            String(this.conversaAtiva.idConversa) ===
            String(mensagem.idConversa)
        ) {

            await this.carregarMensagens(
                mensagem.idConversa
            );

        }

    }

    async abrirPostPeloLink() {

        const parametros =
            new URLSearchParams(
                window.location.search
            );

        const idPost =
            parametros.get('post');

        if (!idPost) {
            return false;
        }

        try {

            this.esconderTelaBoasVindas();

            await this.abrirDetalhesPost(idPost);

            return true;

        } catch (erro) {

            console.error(
                'Erro ao abrir publicação compartilhada:',
                erro
            );

            return false;
        }
    }

    popularComunidadesEdicaoEvento(selecionadaId = null) {

        const select =
            document.getElementById(
                'editar-evento-comunidade'
            );

        if (!select) return;

        select.innerHTML = `
        <option value="">
            Selecione uma comunidade (opcional)
        </option>
    `;

        if (Array.isArray(this.listaComunidades)) {

            this.listaComunidades.forEach(
                comunidade => {

                    const id =
                        comunidade.idComunidade ||
                        comunidade.id;

                    const nome =
                        comunidade.nome ||
                        'Comunidade';

                    select.innerHTML += `
                    <option value="${id}">
                        ${this.escaparHtmlDestaque(nome)}
                    </option>
                `;
                }
            );
        }

        if (selecionadaId) {
            select.value = String(selecionadaId);
        }
    }

    obterIdsMinhasComunidades() {

        return new Set(
            this.obterMinhasComunidades()
                .map(comunidade =>
                    String(
                        comunidade.idComunidade ||
                        comunidade.id
                    )
                )
        );
    }

    renderizarFiltrosComunidadesFeed() {

        const container =
            document.getElementById(
                'feed-comunidades-filtros'
            );

        if (!container) return;

        if (this.filtroFeed !== 'comunidades') {
            container.innerHTML = '';
            container.classList.remove('active');
            return;
        }

        const comunidades =
            this.obterMinhasComunidades();

        container.classList.add('active');

        if (!comunidades.length) {
            container.innerHTML = '';
            return;
        }

        const botaoTodas = `
        <button
            type="button"
            class="
                feed-comunidade-chip
                ${this.comunidadeFeedSelecionada === null
                ? 'active'
                : ''
            }
            "
            onclick="
                app.selecionarComunidadeFeed(null)
            "
        >
            Todas
        </button>
    `;

        const botoesComunidades =
            comunidades
                .map(comunidade => {

                    const id =
                        comunidade.idComunidade ||
                        comunidade.id;

                    const selecionada =
                        String(
                            this.comunidadeFeedSelecionada
                        ) === String(id);

                    return `
                    <button
                        type="button"
                        class="
                            feed-comunidade-chip
                            ${selecionada ? 'active' : ''}
                        "
                        onclick="
                            app.selecionarComunidadeFeed(${id})
                        "
                    >
                        ${this.escaparHtmlDestaque(
                        comunidade.nome || 'Comunidade'
                    )}
                    </button>
                `;
                })
                .join('');

        container.innerHTML =
            botaoTodas +
            botoesComunidades;
    }

    selecionarComunidadeFeed(idComunidade) {

        this.comunidadeFeedSelecionada =
            idComunidade === null
                ? null
                : String(idComunidade);

        this.renderizarFeedGeral();
    }

    abrirComunidadeDoFeed(idComunidade) {

        const comunidade =
            (this.listaComunidades || [])
                .find(c =>
                    (
                        c.idComunidade ||
                        c.id
                    ) == idComunidade
                );

        if (!comunidade) {
            return;
        }

        this.entrarSubreddit(
            idComunidade,
            comunidade.nome,
            comunidade.descricao
        );
    }

    atualizarSidebarFeed() {

        const kicker =
            document.getElementById(
                'feed-sidebar-recomendado-kicker'
            );

        const titulo =
            document.getElementById(
                'feed-sidebar-recomendado-titulo'
            );

        const container =
            document.getElementById(
                'feed-recomendados'
            );

        if (
            !kicker ||
            !titulo ||
            !container
        ) {
            return;
        }


        if (this.filtroFeed === 'comunidades') {

            kicker.innerText =
                'Sua rede';

            titulo.innerText =
                'Suas comunidades';

            const comunidades =
                this.obterMinhasComunidades();

            if (!comunidades.length) {

                container.innerHTML = `
                <p class="feed-sidebar-empty">
                    Participe de comunidades para
                    acompanhar suas conversas aqui.
                </p>

                <button
                    type="button"
                    class="feed-sidebar-link"
                    onclick="
                        app.mudarAba('comunidades')
                    "
                >
                    Explorar comunidades

                    <i class="material-icons">
                        arrow_forward
                    </i>
                </button>
            `;

                return;
            }


            container.innerHTML =
                comunidades
                    .slice(0, 5)
                    .map(comunidade => {

                        const id =
                            comunidade.idComunidade ||
                            comunidade.id;

                        const membros =
                            comunidade.membros?.length ||
                            0;

                        return `
                        <button
                            type="button"
                            class="
                                feed-minha-comunidade
                            "
                            onclick="
                                app.abrirComunidadeDoFeed(
                                    ${id}
                                )
                            "
                        >

                            <span
                                class="
                                    feed-minha-comunidade-avatar
                                "
                            >
                                ${this.escaparHtmlDestaque(
                            (
                                comunidade.nome ||
                                'C'
                            )
                                .charAt(0)
                                .toUpperCase()
                        )}
                            </span>

                            <span
                                class="
                                    feed-minha-comunidade-info
                                "
                            >
                                <strong>
                                    ${this.escaparHtmlDestaque(
                            comunidade.nome ||
                            'Comunidade'
                        )}
                                </strong>

                                <small>
                                    ${membros}
                                    membro(s)
                                </small>
                            </span>

                            <i class="material-icons">
                                chevron_right
                            </i>

                        </button>
                    `;
                    })
                    .join('');

            return;
        }


        // Feed normal
        kicker.innerText =
            'Para você';

        titulo.innerText =
            'Recomendado';

        this.renderizarRecomendacoes();
    }

    mudarFiltroComunidades(filtro) {

        this.filtroComunidades = filtro;

        document
            .querySelectorAll('.comunidades-tab')
            .forEach(tab =>
                tab.classList.remove('active')
            );

        document
            .getElementById(
                `comunidades-tab-${filtro}`
            )
            ?.classList.add('active');

        this.filtrarComunidades();
    }

    obterComunidadesDoUsuario(idUsuario) {

        if (!idUsuario) {
            return [];
        }

        return (this.listaComunidades || [])
            .filter(comunidade => {

                const idCriador =
                    comunidade.criador?.idUsuario ||
                    comunidade.criador?.id;

                const ehCriador =
                    String(idCriador) ===
                    String(idUsuario);

                const ehMembro =
                    (comunidade.membros || [])
                        .some(membro => {

                            const idMembro =
                                membro.idUsuario ||
                                membro.id;

                            return String(idMembro) ===
                                String(idUsuario);
                        });

                return ehCriador || ehMembro;
            });
    }

    obterComunidadesEmComum(idOutroUsuario) {

        const idUsuarioLogado =
            ApiService.getIdUsuarioLogado();

        if (!idUsuarioLogado) {
            return [];
        }

        const minhas =
            this.obterComunidadesDoUsuario(
                idUsuarioLogado
            );

        const idsOutroUsuario =
            new Set(
                this.obterComunidadesDoUsuario(
                    idOutroUsuario
                )
                    .map(comunidade =>
                        String(
                            comunidade.idComunidade ||
                            comunidade.id
                        )
                    )
            );

        return minhas.filter(comunidade => {

            const id =
                comunidade.idComunidade ||
                comunidade.id;

            return idsOutroUsuario.has(
                String(id)
            );
        });
    }

    renderizarEventosComunidade() {

        const container =
            document.getElementById(
                'subreddit-feed-container'
            );

        if (!container) return;


        const eventos =
            (this.listaEventos || [])
                .filter(evento =>
                    evento.comunidadeId ==
                    this.comunidadeAtivaId
                );


        if (!eventos.length) {

            container.innerHTML = `
            <div class="comunidade-aba-empty">

                <i class="material-icons">
                    event_busy
                </i>

                <h3>
                    Nenhum evento por enquanto
                </h3>

                <p>
                    Esta comunidade ainda não
                    possui eventos cadastrados.
                </p>

            </div>
        `;

            return;
        }


        container.innerHTML = `
        <div class="comunidade-eventos-lista">

            ${eventos.map(evento => {

            const idEvento =
                evento.idEvento ||
                evento.id;

            const data =
                evento.dataEvento
                    ? new Date(
                        `${evento.dataEvento}T00:00:00`
                    )
                        .toLocaleDateString(
                            'pt-BR',
                            {
                                day: '2-digit',
                                month: 'short'
                            }
                        )
                        .replace('.', '')
                    : '--';

            const horario =
                evento.horarioInicio
                    ?.substring(0, 5) ||
                '--:--';

            return `
                    <article
                        class="comunidade-evento-card"
                        onclick="
                            app.abrirDetalhesEvento(
                                ${idEvento}
                            )
                        "
                    >

                        <div class="
                            comunidade-evento-data
                        ">
                            <strong>
                                ${data}
                            </strong>

                            <span>
                                ${horario}
                            </span>
                        </div>


                        <div class="
                            comunidade-evento-conteudo
                        ">

                            <span class="
                                comunidade-evento-categoria
                            ">
                                ${this.escaparHtmlDestaque(
                evento.categoria ||
                'Evento'
            )}
                            </span>

                            <h3>
                                ${this.escaparHtmlDestaque(
                evento.titulo ||
                'Evento'
            )}
                            </h3>

                            <p>
                                <i class="material-icons">
                                    place
                                </i>

                                ${this.escaparHtmlDestaque(
                evento.localEvento ||
                'Local a confirmar'
            )}
                            </p>

                        </div>


                        <i class="
                            material-icons
                            comunidade-evento-arrow
                        ">
                            arrow_forward
                        </i>

                    </article>
                `;
        }).join('')}

        </div>
    `;
    }

    abrirItensSalvos() {

        const idUsuario =
            ApiService.getIdUsuarioLogado();

        if (!idUsuario) {
            return;
        }


        // Fecha menu da conta
        document
            .querySelector('.account-menu')
            ?.classList.remove('open');


        document
            .querySelectorAll('.view-section')
            .forEach(view =>
                view.classList.remove('active')
            );


        document
            .querySelectorAll('.nav-btn')
            .forEach(botao =>
                botao.classList.remove('active')
            );


        document
            .getElementById('view-salvos')
            ?.classList.add('active');


        this.salvarViewAtual('salvos');

        this.renderizarItensSalvos();
    }

    abrirMinhaAgenda() {
        if (!ApiService.getUsuarioLogado()) {
            this.mostrarFeedbackGlobal('Entre na sua conta para acessar sua agenda.');
            return;
        }

        const menuConta = document.querySelector('.account-menu');
        menuConta?.classList.remove('open');
        menuConta?.querySelector('.account-trigger')?.setAttribute('aria-expanded', 'false');
        this.ativarAba('agenda');
    }

    renderizarItensSalvos() {

        const container =
            document.getElementById(
                'salvos-container'
            );

        if (!container) {
            return;
        }


        const idUsuario =
            ApiService.getIdUsuarioLogado();


        const idsSalvos =
            new Set(
                (this.postsSalvos || [])
                    .filter(item =>
                        String(item.idUsuario) ===
                        String(idUsuario)
                    )
                    .map(item =>
                        String(item.idPost)
                    )
            );


        const posts =
            (this.posts || [])
                .filter(post => {

                    const idPost =
                        post.idPost ||
                        post.id;

                    return idsSalvos.has(
                        String(idPost)
                    );
                });


        if (!posts.length) {

            container.innerHTML = `
            <div class="salvos-empty">

                <div class="salvos-empty-icon">
                    <i class="material-icons">
                        bookmark_border
                    </i>
                </div>

                <h3>
                    Nenhuma publicação salva
                </h3>

                <p>
                    Quando você salvar uma publicação,
                    ela aparecerá aqui.
                </p>

                <button
                    type="button"
                    class="btn-primary"
                    onclick="app.mudarAba('feed')"
                >
                    Explorar publicações
                </button>

            </div>
        `;

            return;
        }


        this.montarCardsPosts(
            posts,
            'salvos-container',
            true
        );
    }

    abrirConfiguracoes() {

        document
            .querySelector('.account-menu')
            ?.classList.remove('open');

        this.abrirModal(
            'configuracoes-modal'
        );
    }

    abrirAlteracaoSenhaPelasConfiguracoes() {
        this.fecharModal('configuracoes-modal');
        this.limparFormularioAlteracaoSenha();
        this.abrirModal('alterar-senha-modal');
        setTimeout(() => document.getElementById('senha-atual')?.focus(), 0);
    }

    limparFormularioAlteracaoSenha() {
        [
            ['senha-atual', 'Mostrar senha atual'],
            ['senha-nova', 'Mostrar nova senha'],
            ['senha-confirmacao', 'Mostrar confirmação da senha']
        ].forEach(([id, ariaLabel]) => {
            const input = document.getElementById(id);
            if (!input) return;
            input.value = '';
            input.type = 'password';
            const botao = input.closest('.auth-input-wrap')?.querySelector('.password-toggle');
            if (botao) {
                botao.setAttribute('aria-label', ariaLabel);
                botao.querySelector('.material-icons').innerText = 'visibility';
            }
        });
        this.mostrarSenhaFeedback('');
    }

    mostrarSenhaFeedback(mensagem) {
        const feedback = document.getElementById('senha-feedback');
        if (!feedback) return;
        feedback.textContent = mensagem;
        feedback.classList.toggle('visible', Boolean(mensagem));
    }

    async alterarSenha() {
        const senhaAtual = document.getElementById('senha-atual')?.value || '';
        const senhaNova = document.getElementById('senha-nova')?.value || '';
        const confirmacao = document.getElementById('senha-confirmacao')?.value || '';
        const botao = document.getElementById('alterar-senha-submit');

        this.mostrarSenhaFeedback('');
        if (!senhaAtual || !senhaNova || !confirmacao) {
            this.mostrarSenhaFeedback('Preencha os três campos para continuar.');
            return;
        }
        if (senhaNova.length < 8) {
            this.mostrarSenhaFeedback('A nova senha precisa ter pelo menos 8 caracteres.');
            return;
        }
        if (new TextEncoder().encode(senhaNova).length > 72) {
            this.mostrarSenhaFeedback('A nova senha pode ter no máximo 72 bytes.');
            return;
        }
        if (senhaNova !== confirmacao) {
            this.mostrarSenhaFeedback('A confirmação não corresponde à nova senha.');
            return;
        }

        if (botao) {
            botao.disabled = true;
            botao.textContent = 'Salvando...';
        }

        try {
            const resposta = await ApiService.alterarSenha(senhaAtual, senhaNova);
            if (!resposta.ok) {
                throw new Error(await this.mensagemErroApi(
                    resposta,
                    'Não foi possível alterar a senha.'
                ));
            }

            const usuarioAtualizado = await resposta.json();
            if (!usuarioAtualizado?.authToken) {
                this.fecharModal('alterar-senha-modal');
                this.fazerLogout();
                this.mostrarFeedbackGlobal('Senha alterada. Entre novamente para continuar.');
                return;
            }
            ApiService.salvarSessao(usuarioAtualizado);
            this.fecharModal('alterar-senha-modal');
            this.mostrarFeedbackGlobal('Senha alterada. As outras sessões foram encerradas.');
        } catch (erro) {
            console.error('Erro ao alterar senha:', erro);
            this.mostrarSenhaFeedback(erro.message || 'Não foi possível alterar a senha.');
        } finally {
            if (botao) {
                botao.disabled = false;
                botao.textContent = 'Salvar nova senha';
            }
        }
    }

    abrirEdicaoPerfilPelasConfiguracoes() {

        this.fecharModal(
            'configuracoes-modal'
        );

        this.abrirEdicaoPerfil();
    }

    abrirMeuPerfilPelasConfiguracoes() {

        this.fecharModal(
            'configuracoes-modal'
        );

        this.abrirMeuPerfil();
    }

    async acaoIngressoEvento(idEvento) {

        let evento = this.listaEventos.find(
            e => (e.idEvento || e.id) == idEvento
        );

        if (!evento) {
            try {
                evento = await ApiService.buscarEventoPorId(idEvento);
            } catch (erro) {
                console.error(erro);
                return this.mostrarFeedbackGlobal(
                    "Não foi possível carregar os dados do evento."
                );
            }
        }

        const preco = Number(evento.precoIngresso || 0);

        if (preco <= 0) {
            await this.participarEvento(idEvento);
            return;
        }

        this.eventoCompraId = idEvento;

        document.getElementById(
            'compra-ingresso-evento-nome'
        ).textContent = evento.titulo || 'Evento';

        try {
            const calculo = await ApiService.calcularPagamento(idEvento);
            const formatarMoeda = valor => Number(valor).toLocaleString('pt-BR', {
                style: 'currency',
                currency: 'BRL'
            });

            document.getElementById('compra-ingresso-subtotal').textContent =
                formatarMoeda(calculo.subtotal);
            document.getElementById('compra-ingresso-taxa').textContent =
                evento.taxaRepassadaComprador === true
                    ? formatarMoeda(calculo.taxaPlataforma)
                    : 'Absorvida pelo organizador';
            document.getElementById('compra-ingresso-total').textContent =
                formatarMoeda(calculo.totalComprador);

            this.abrirModal('compra-ingresso-modal');
        } catch (erro) {
            console.error('Erro ao calcular total da compra:', erro);
            this.eventoCompraId = null;
            this.mostrarFeedbackGlobal('Não foi possível calcular o valor total do ingresso.');
        }
    }

    async confirmarCompraIngresso() {

        if (!this.eventoCompraId) {
            return;
        }

        const idEvento = this.eventoCompraId;

        this.fecharModal('compra-ingresso-modal');

        try {

            await this.participarEvento(idEvento);

            this.eventoCompraId = null;

            this.mostrarFeedbackGlobal(
                "Compra simulada concluída! Seu ingresso foi registrado."
            );

        } catch (erro) {

            console.error(
                "Erro ao concluir compra:",
                erro
            );

            this.mostrarFeedbackGlobal(
                "Não foi possível concluir a compra do ingresso."
            );
        }

        // Obtém a posição do usuário no navegador
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    const userLat = position.coords.latitude;
                    const userLng = position.coords.longitude;

                    // Faz a chamada para a sua API enviando as coordenadas
                    carregarEventosProximos(userLat, userLng);
                },
                (error) => {
                    console.warn("Geolocalização recusada ou indisponível. Carregando sem filtro de distância.");
                    carregarEventosProximos(null, null);
                }
            );
        } else {
            carregarEventosProximos(null, null);
        }

        function carregarEventosProximos(lat, lng) {
            let url = `/api/eventos/buscar?page=0&size=12`;

            if (lat && lng) {
                url += `&lat=${lat}&lng=${lng}&raioKm=30`; // Busca eventos num raio de 30km
            }

            fetch(url)
                .then(res => res.json())
                .then(data => {
                    console.log("Eventos encontrados:", data.content);
                    // Renderiza os cards na tela
                });
        }
    }

    abrirModalConfirmacao({ titulo, mensagem, textoConfirmar = 'Confirmar', classeBotao = 'btn-primary', onSuccess }) {
        const modal = document.getElementById('modal-confirmacao');
        if (!modal) return;

        document.getElementById('modal-confirmacao-titulo').innerText = titulo;
        document.getElementById('modal-confirmacao-mensagem').innerText = mensagem;

        const btnConfirmar = document.getElementById('btn-modal-confirmar');
        btnConfirmar.innerText = textoConfirmar;
        btnConfirmar.className = `btn ${classeBotao}`;

        // Limpa listeners antigos atribuindo novo evento
        btnConfirmar.onclick = () => {
            this.fecharModal('modal-confirmacao');
            if (typeof onSuccess === 'function') onSuccess();
        };

        this.abrirModal('modal-confirmacao');
    }

    mostrarFeedbackGlobal(mensagem, tipo = null) {
        const container = document.getElementById('toast-container');
        const texto = String(mensagem || '').trim();
        if (!container || !texto) return;

        const textoNormalizado = texto
            .normalize('NFD')
            .replace(/[\u0300-\u036f]/g, '')
            .toLowerCase();
        const categoria = tipo || (
            /sucesso|salv[oa]|copiad[oa]|publicad[oa]|atualizad[oa]|excluid[oa]|removid[oa]|cancelad[oa]/.test(textoNormalizado)
                ? 'success'
                : /nao foi possivel|erro|falha|sem permissao|nao tem permissao/.test(textoNormalizado)
                    ? 'error'
                    : /login|entre para|selecione|escolha|digite|nao pode|maximo|verifique|obrigatorio|nao participa/.test(textoNormalizado)
                        ? 'warning'
                        : 'info'
        );
        const icones = {
            success: 'check_circle',
            error: 'error',
            warning: 'warning',
            info: 'info'
        };

        const aviso = document.createElement('div');
        aviso.className = `app-toast app-toast-${categoria}`;
        aviso.setAttribute('role', categoria === 'error' ? 'alert' : 'status');

        const icone = document.createElement('i');
        icone.className = 'material-icons app-toast-icon';
        icone.setAttribute('aria-hidden', 'true');
        icone.textContent = icones[categoria] || icones.info;

        const conteudo = document.createElement('p');
        conteudo.className = 'app-toast-message';
        conteudo.textContent = texto;

        const fechar = document.createElement('button');
        fechar.type = 'button';
        fechar.className = 'app-toast-close';
        fechar.setAttribute('aria-label', 'Fechar aviso');
        fechar.innerHTML = '<i class="material-icons" aria-hidden="true">close</i>';

        let temporizador;
        const remover = () => {
            clearTimeout(temporizador);
            aviso.classList.add('app-toast-saindo');
            setTimeout(() => aviso.remove(), 220);
        };
        fechar.addEventListener('click', remover);
        aviso.append(icone, conteudo, fechar);
        container.appendChild(aviso);
        temporizador = setTimeout(remover, 5000);
    }
}



window.app = new AppController();
window.app.inicializar();