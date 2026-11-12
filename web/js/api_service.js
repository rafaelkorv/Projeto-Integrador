// api_service.js
// Camada de comunicação do frontend: concentra URLs, sessão e conversões HTTP para separar rede da lógica de tela.
export class ApiService {
    // Em desenvolvimento, a interface roda em outra porta e consome a API Spring Boot nesta origem.
    static API_URL = 'http://localhost:8080';
    //static API_URL = 'https://projeto-integrador-m4jn.onrender.com';
    //static API_URL = 'http://143.106.241.xx:8080';
    static STORAGE_KEY = 'usuarioLogado';

    static getStorage(tipo = 'session') {
        if (typeof window === 'undefined') return null;
        return tipo === 'session' ? window.sessionStorage : window.localStorage;
    }

    static getUsuarioLogado() {
        // A sessão fica no sessionStorage; a leitura de localStorage migra sessões antigas uma única vez.
        const storage = this.getStorage('session');
        const raw = storage ? storage.getItem(this.STORAGE_KEY) : null;

        if (raw) {
            try {
                const usuario = JSON.parse(raw);
                if (!usuario.authToken) {
                    storage.removeItem(this.STORAGE_KEY);
                    return null;
                }
                return usuario;
            } catch (error) {
                this.fazerLogout();
                return null;
            }
        }

        const legado = this.getStorage('local')?.getItem(this.STORAGE_KEY);
        if (legado) {
            try {
                const usuario = JSON.parse(legado);
                if (!usuario.authToken) {
                    this.getStorage('local')?.removeItem(this.STORAGE_KEY);
                    return null;
                }
                this.salvarSessao(usuario);
                this.getStorage('local')?.removeItem(this.STORAGE_KEY);
                return usuario;
            } catch (error) {
                this.getStorage('local')?.removeItem(this.STORAGE_KEY);
                return null;
            }
        }

        return null;
    }

    static salvarSessao(usuario) {
        const storage = this.getStorage('session');
        if (storage) {
            const rawSessaoAtual = storage.getItem(this.STORAGE_KEY);
            let sessaoAtual = null;
            if (rawSessaoAtual) {
                try {
                    sessaoAtual = JSON.parse(rawSessaoAtual);
                } catch (erro) {
                    console.error('Não foi possível preservar o token da sessão atual:', erro);
                }
            }
            const usuarioComToken = {
                ...usuario,
                authToken: usuario.authToken || sessaoAtual?.authToken
            };
            storage.setItem(this.STORAGE_KEY, JSON.stringify(usuarioComToken));
        }
        this.getStorage('local')?.removeItem(this.STORAGE_KEY);
    }

    static fazerLogout() {
        const storage = this.getStorage('session');
        const raw = storage ? storage.getItem(this.STORAGE_KEY) : null;
        if (raw) {
            try {
                const token = JSON.parse(raw).authToken;
                if (token) {
                    void this.request(`${this.API_URL}/auth/logout`, { method: 'POST' })
                        .catch(erro => console.error('Não foi possível encerrar a sessão no servidor:', erro));
                }
            } catch (erro) {
                console.error('Não foi possível ler a sessão antes de sair:', erro);
            }
        }
        storage?.removeItem(this.STORAGE_KEY);
        this.getStorage('local')?.removeItem(this.STORAGE_KEY);
    }

    static getAuthToken() {
        return this.getUsuarioLogado()?.authToken || null;
    }

    static request(input, init = {}) {
        const token = this.getAuthToken();
        if (!token) return window.fetch(input, init);

        const requestUrl = typeof input === 'string' ? input : input.url;
        const requestOrigin = new URL(requestUrl, window.location.href).origin;
        if (requestOrigin !== new URL(this.API_URL).origin) {
            return window.fetch(input, init);
        }

        const headers = new Headers(init.headers || {});
        headers.set('Authorization', `Bearer ${token}`);
        return window.fetch(input, { ...init, headers });
    }

    static getIdUsuarioLogado() {
        const user = this.getUsuarioLogado();
        return user ? (user.idUsuario || user.id_usuario || user.id) : null;
    }

    // --- FORMATAÇÃO DE URL DE FOTO (GOOGLE DRIVE / LOCAL) ---
    static formatarUrlFotoPerfil(fotoPerfil) {
        if (!fotoPerfil) return null;
        if (typeof fotoPerfil !== 'string') return null;
        const foto = fotoPerfil.trim();
        if (!foto) return null;
        if (foto.startsWith('http://') || foto.startsWith('https://') || foto.startsWith('data:')) {
            return foto;
        }
        if (foto.startsWith('uploads/')) {
            return `${this.API_URL}/${foto}`;
        }
        if (foto.startsWith('/google/drive/image/') || foto.startsWith('google/drive/image/')) {
            return `${this.API_URL}/${foto.replace(/^\//, '')}`;
        }
        // Identificador de arquivo do Google Drive
        return `${this.API_URL}/google/drive/image/${foto}`;
    }

    // --- USUÁRIOS ---
    static async listarUsuarios() { return await (await fetch(`${this.API_URL}/usuarios`)).json(); }
    static async criarUsuario(
        nomeCompleto,
        nome,
        username,
        dataNascimento,
        email,
        telefone,
        senha,
        interesses = []
    ) {
        return await fetch(`${this.API_URL}/usuarios`, {
            method: 'POST', headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ nomeCompleto, nome, username, dataNascimento, email: email || null, telefone: telefone || null, senha, interesses: interesses || [] })
        });
    }

    // --- POSTS E COMENTÁRIOS COM PAGINAÇÃO ---
    static async listarPosts(page = null, size = 10) {
        const url = (page !== null && page !== undefined)
            ? `${this.API_URL}/posts?page=${page}&size=${size}`
            : `${this.API_URL}/posts`;
        const resp = await fetch(url);
        if (!resp.ok) return [];
        return await resp.json();
    }

    static async buscarPostPorId(idPost) {
        const resposta = await fetch(`${this.API_URL}/posts/${idPost}`);
        if (!resposta.ok) throw new Error('Post não encontrado');
        return await resposta.json();
    }

    static async criarPost(titulo, conteudo, idUsuario, idComunidade = null) {
        let bodyObj = { titulo, conteudo, idUsuario: parseInt(idUsuario) };
        if (idComunidade !== null) { bodyObj.idComunidade = parseInt(idComunidade); } // Se tiver comunidade, envia junto

        return await fetch(`${this.API_URL}/posts`, {
            method: 'POST', headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(bodyObj)
        });
    }

    // As imagens são recursos separados do post: primeiro se cria a publicação, depois cada arquivo é enviado como multipart.
    static async listarImagensPost() {
        const resposta = await fetch(`${this.API_URL}/imagens-post`);
        if (!resposta.ok) throw new Error('Não foi possível carregar as imagens dos posts.');
        return resposta.json();
    }

    static async enviarImagemPost(idPost, imagem) {
        const formData = new FormData();
        formData.append('idPost', idPost);
        formData.append('imagem', imagem);

        return fetch(`${this.API_URL}/imagens-post/upload`, {
            method: 'POST',
            body: formData
        });
    }

    static async listarComentarios() { return await (await fetch(`${this.API_URL}/comentarios`)).json(); }
    static async listarComentariosPorPost(idPost) {
        try {
            const resp = await fetch(`${this.API_URL}/comentarios/post/${idPost}`);
            if (!resp.ok) return [];
            return await resp.json();
        } catch (e) {
            return [];
        }
    }
    static async criarComentario(conteudo, idUsuario, idPost) {
        return await fetch(`${this.API_URL}/comentarios`, {
            method: 'POST', headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ conteudo, idUsuario: parseInt(idUsuario), idPost: parseInt(idPost) })
        });
    }

    static async listarVotos() {
        return await (await fetch(`${this.API_URL}/votos`)).json();
    }

    static async criarVoto(idUsuario, idPost, tipo = 'like') {
        return await fetch(`${this.API_URL}/votos`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ idUsuario, idPost, tipo })
        });
    }

    static async deletarVoto(idVoto) {
        return await fetch(`${this.API_URL}/votos/${idVoto}`, { method: 'DELETE' });
    }

    static async listarPostsSalvos() {
        return await (await fetch(`${this.API_URL}/posts_salvos`)).json();
    }

    static async salvarPost(idUsuario, idPost) {
        return await fetch(`${this.API_URL}/posts_salvos`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ idUsuario, idPost })
        });
    }

    static async removerPostSalvo(idSalvo) {
        return await fetch(`${this.API_URL}/posts_salvos/${idSalvo}`, { method: 'DELETE' });
    }

    // --- COMUNIDADES ---
    static async listarComunidades() {
        return await (await fetch(`${this.API_URL}/comunidades`)).json();
    }

    static async criarComunidadeAPI(nome, descricao, criadorId, categoria, cor) {
        return await fetch(`${this.API_URL}/comunidades`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                nome,
                descricao,
                criadorId,
                categoria: categoria || null,
                cor: cor || '#EA3F74'
            })
        });
    }

    static async enviarImagemComunidade(idComunidade, imagem) {
        const dados = new FormData();
        dados.append('imagem', imagem);
        return fetch(`${this.API_URL}/comunidades/${idComunidade}/imagem`, {
            method: 'POST',
            body: dados
        });
    }

    // --- EVENTOS  ---
    static async listarEventos() {
        const response = await fetch(`${this.API_URL}/api/eventos`);
        return await response.json();
    }

    static async criarEventoAPI(eventoData) {
        return await fetch(`${this.API_URL}/api/eventos`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(eventoData)
        });
    }

    static async calcularPagamento(idEvento) {
        const response = await fetch(
            `${this.API_URL}/api/pagamentos/eventos/${idEvento}/calcular`
        );

        if (!response.ok) {
            throw new Error('Erro ao calcular o valor do pagamento');
        }

        return await response.json();
    }

    static async atualizarComunidade(idComunidade, idUsuario, nome, descricao, categoria, cor) {
        return await fetch(`${this.API_URL}/comunidades/${idComunidade}/usuario/${idUsuario}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                nome,
                descricao,
                categoria: categoria || null,
                cor: cor || '#EA3F74'
            })
        });
    }

    static async excluirComunidade(idComunidade, idUsuario) {
        return await fetch(`${this.API_URL}/comunidades/${idComunidade}/usuario/${idUsuario}`, { method: "DELETE" });
    }

    static async removerMembro(idComunidade, idMembro, idSolicitante) {
        return await fetch(
            `${this.API_URL}/comunidades/${idComunidade}/membros/${idMembro}/usuario/${idSolicitante}`,
            {
                method: "DELETE"
            }
        );
    }

    static async deletarPost(idPost, idUsuario) {
        return await fetch(
            `${this.API_URL}/posts/${idPost}/usuario/${idUsuario}`,
            {
                method: "DELETE"
            }
        );
    }

    static async atualizarEvento(idEvento, idUsuario, eventoData) {
        return await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/usuario/${idUsuario}`,
            {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(eventoData)
            }
        );
    }

    static async excluirEvento(idEvento, idUsuario) {
        return await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/usuario/${idUsuario}`,
            {
                method: "DELETE"
            }
        );
    }

    static async cancelarEvento(idEvento, idUsuario) {
        return await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/cancelar/usuario/${idUsuario}`,
            {
                method: "PUT"
            }
        );
    }

    static async participarEvento(idEvento, idUsuario) {
        return await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/participar/${idUsuario}`,
            {
                method: "POST"
            }
        );
    }

    static async sairEvento(idEvento, idUsuario) {
        return await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/participar/${idUsuario}`,
            {
                method: "DELETE"
            }
        );
    }

    static async mensagemErroResposta(resposta, mensagemPadrao) {
        const corpo = await resposta.text();
        if (/unknown column|doesn't exist|does not exist|no such table/i.test(corpo)) {
            return 'O banco ainda não tem as alterações de eventos. Aplique as migrações pendentes em java/database/migrations e reinicie o backend.';
        }

        try {
            const erro = JSON.parse(corpo);
            if (typeof erro.message === 'string' && erro.message.trim()) {
                return erro.message;
            }
        } catch {
            if (corpo.trim()) return corpo.trim();
        }

        return mensagemPadrao;
    }

    static async entrarNaListaEspera(idEvento, idUsuario) {
        return await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/lista-espera/${idUsuario}`,
            { method: 'POST' }
        );
    }

    static async sairDaListaEspera(idEvento, idUsuario) {
        return await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/lista-espera/${idUsuario}`,
            { method: 'DELETE' }
        );
    }

    static async estaNaListaEspera(idEvento, idUsuario) {
        const response = await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/lista-espera/usuario/${idUsuario}`
        );
        if (!response.ok) {
            throw new Error(await this.mensagemErroResposta(response, 'Erro ao consultar lista de espera'));
        }
        return await response.json();
    }

    static async salvarEventoAgenda(idEvento, idUsuario) {
        return await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/salvo/usuario/${idUsuario}`,
            { method: 'POST' }
        );
    }

    static async removerEventoAgenda(idEvento, idUsuario) {
        return await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/salvo/usuario/${idUsuario}`,
            { method: 'DELETE' }
        );
    }

    static async estaEventoSalvo(idEvento, idUsuario) {
        const response = await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/salvo/usuario/${idUsuario}`
        );
        if (!response.ok) {
            throw new Error(await this.mensagemErroResposta(response, 'Erro ao consultar agenda'));
        }
        return await response.json();
    }

    static async listarAgendaEventos(idUsuario) {
        const response = await fetch(`${this.API_URL}/api/eventos/agenda/${idUsuario}`);
        if (!response.ok) throw new Error('Erro ao carregar agenda');
        return await response.json();
    }

    static async listarNotificacoesEventos(idUsuario) {
        const response = await fetch(`${this.API_URL}/api/notificacoes/usuario/${idUsuario}`);
        if (!response.ok) throw new Error('Erro ao carregar notificações');
        return await response.json();
    }

    static async marcarNotificacaoLida(idNotificacao, idUsuario) {
        return await fetch(
            `${this.API_URL}/api/notificacoes/${idNotificacao}/lida/usuario/${idUsuario}`,
            { method: 'PUT' }
        );
    }

    static async obterResumoOrganizador(idEvento, idUsuario) {
        const response = await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/organizador/${idUsuario}/resumo`
        );
        if (!response.ok) throw new Error('Erro ao carregar indicadores do evento');
        return await response.json();
    }

    static async exportarParticipantesEvento(idEvento, idUsuario) {
        const response = await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/organizador/${idUsuario}/participantes.csv`
        );
        if (!response.ok) throw new Error('Erro ao exportar participantes');
        return await response.blob();
    }

    static async listarParticipantesEvento(idEvento) {
        const response = await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/participantes`
        );
        if (!response.ok) {
            throw new Error(await this.mensagemErroResposta(
                response,
                'Não foi possível carregar as inscrições do evento.'
            ));
        }
        return response.json();
    }

    static async contarParticipantesEvento(idEvento) {
        const resposta = await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/participantes/quantidade`
        );

        return await resposta.json();
    }

    static async removerParticipanteEvento(
        idEvento,
        idParticipante,
        idSolicitante
    ) {
        return await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/participantes/${idParticipante}/usuario/${idSolicitante}`,
            {
                method: "DELETE"
            }
        );
    }

    static async validarIngressoEvento(idEvento, idSolicitante, tokenIngresso) {
        return await fetch(
            `${this.API_URL}/api/eventos/${idEvento}/validar-ingresso/usuario/${idSolicitante}`,
            {
                method: "PUT",
                headers: {
                    "Content-Type": "text/plain"
                },
                body: tokenIngresso
            }
        );
    }

    static async buscarEventos({
        texto = null,
        categoria = null,
        comunidadeId = null,
        pago = null,
        dataInicio = null,
        dataFim = null,
        formato = null,
        lat = null,       // <--- Novo: Latitude do usuário
        lng = null,       // <--- Novo: Longitude do usuário
        raioKm = 50,      // <--- Novo: Raio de busca em km (padrão 50km)
        page = 0,
        size = 12
    } = {}) {

        const params = new URLSearchParams();

        if (texto) params.append("texto", texto);
        if (categoria) params.append("categoria", categoria);
        if (comunidadeId) params.append("comunidadeId", comunidadeId);
        if (pago !== null) params.append("pago", String(pago));
        if (dataInicio) params.append("dataInicio", dataInicio);
        if (dataFim) params.append("dataFim", dataFim);
        if (formato) params.append("formato", formato);

        // Envia a geolocalização se estiver disponível
        if (lat !== null && lng !== null) {
            params.append("lat", lat);
            params.append("lng", lng);
            params.append("raioKm", raioKm);
        }

        params.append("page", page);
        params.append("size", size);

        const response = await fetch(
            `${this.API_URL}/api/eventos/buscar?${params.toString()}`
        );

        if (!response.ok) {
            throw new Error("Erro ao buscar eventos");
        }

        return await response.json();
    }

    static async buscarCoordenadasPorEndereco(endereco) {
        if (!endereco || !endereco.trim()) return null;
        try {
            const url = `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(endereco.trim())}&limit=1`;
            const resp = await fetch(url, { headers: { 'Accept-Language': 'pt-BR,pt;q=0.9,en;q=0.8' } });
            if (!resp.ok) return null;
            const dados = await resp.json();
            if (dados && dados.length > 0) {
                return {
                    lat: parseFloat(dados[0].lat),
                    lng: parseFloat(dados[0].lon),
                    displayName: dados[0].display_name
                };
            }
            return null;
        } catch (e) {
            console.warn("Erro ao buscar coordenadas por endere�o:", e);
            return null;
        }
    }

    static async buscarEnderecoPorCoordenadas(lat, lng) {
        if (lat === null || lat === undefined || lng === null || lng === undefined) return null;
        try {
            const url = `https://nominatim.openstreetmap.org/reverse?format=json&lat=${lat}&lon=${lng}&zoom=18&addressdetails=1`;
            const resp = await fetch(url, { headers: { 'Accept-Language': 'pt-BR,pt;q=0.9,en;q=0.8' } });
            if (!resp.ok) return null;
            const dados = await resp.json();
            if (dados && dados.display_name) {
                const addr = dados.address || {};
                const rua = addr.road || addr.street || addr.pedestrian || addr.suburb || '';
                const num = addr.house_number ? `, ${addr.house_number}` : '';
                const bairro = addr.neighbourhood || addr.suburb ? ` - ${addr.neighbourhood || addr.suburb}` : '';
                const cidade = addr.city || addr.town || addr.village || addr.municipality || '';
                const estado = addr.state ? ` - ${addr.state}` : '';
                if (rua && cidade) {
                    return `${rua}${num}${bairro}, ${cidade}${estado}`;
                }
                return dados.display_name;
            }
            return null;
        } catch (e) {
            console.warn("Erro ao buscar endere�o reverso:", e);
            return null;
        }
    }

    static calcularDistanciaKm(lat1, lon1, lat2, lon2) {
        if (lat1 == null || lon1 == null || lat2 == null || lon2 == null) return null;
        const nLat1 = Number(lat1);
        const nLon1 = Number(lon1);
        const nLat2 = Number(lat2);
        const nLon2 = Number(lon2);
        if (isNaN(nLat1) || isNaN(nLon1) || isNaN(nLat2) || isNaN(nLon2)) return null;

        const R = 6371; // Raio m�dio da Terra em km
        const dLat = (nLat2 - nLat1) * Math.PI / 180;
        const dLon = (nLon2 - nLon1) * Math.PI / 180;
        const a = 
            Math.sin(dLat / 2) * Math.sin(dLat / 2) +
            Math.cos(nLat1 * Math.PI / 180) * Math.cos(nLat2 * Math.PI / 180) * 
            Math.sin(dLon / 2) * Math.sin(dLon / 2);
        const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        return R * c;
    }

    static obterLocalizacaoAtual() {
        return new Promise((resolve) => {
            if (!navigator.geolocation) {
                console.warn("Geolocalização não é suportada pelo navegador.");
                resolve(null);
                return;
            }

            navigator.geolocation.getCurrentPosition(
                (posicao) => {
                    resolve({
                        lat: posicao.coords.latitude,
                        lng: posicao.coords.longitude
                    });
                },
                (erro) => {
                    console.warn("Permissão de localização negada ou indisponível:", erro.message);
                    resolve(null); // Retorna null sem quebrar a aplicação
                },
                { timeout: 10000 } // Tempo limite de 10 segundos
            );
        });
    }

    static async buscarUsuarioPorId(idUsuario) {

        const resposta = await fetch(
            `${this.API_URL}/usuarios/${idUsuario}`
        );

        if (!resposta.ok) {
            throw new Error("Usuário não encontrado");
        }

        return await resposta.json();
    }

    static async atualizarPerfil(idUsuario, nome, username, bio) {

        return fetch(
            `${this.API_URL}/usuarios/${idUsuario}/perfil`,
            {
                method: 'PUT',

                headers: {
                    'Content-Type': 'application/json'
                },

                body: JSON.stringify({
                    nome,
                    username,
                    bio
                })
            }
        );
    }

    static async listarPostsPorUsuario(idUsuario) {

        const resposta = await fetch(
            `${this.API_URL}/posts/usuario/${idUsuario}`
        );

        if (!resposta.ok) {
            throw new Error("Erro ao buscar publicações do usuário");
        }

        return await resposta.json();
    }

    static async listarComunidadesPorUsuario(idUsuario) {

        const resposta = await fetch(
            `${this.API_URL}/comunidades/usuario/${idUsuario}`
        );

        if (!resposta.ok) {
            throw new Error("Erro ao buscar comunidades do usuário");
        }

        return await resposta.json();
    }

    static async listarEventosPorUsuario(idUsuario) {

        const resposta = await fetch(
            `${this.API_URL}/api/eventos/usuario/${idUsuario}`
        );

        if (!resposta.ok) {
            throw new Error("Erro ao buscar eventos do usuário");
        }

        return await resposta.json();
    }

    static async listarEventosParticipando(idUsuario) {
        const resposta = await fetch(
            `${this.API_URL}/api/eventos/participando/${idUsuario}`
        );

        if (!resposta.ok) {
            throw new Error("Erro ao buscar eventos do usuário");
        }

        return await resposta.json();
    }

    static async enviarCapaEvento(idEvento, capa) {
        const dados = new FormData();
        dados.append('capa', capa);
        return fetch(`${this.API_URL}/api/eventos/${idEvento}/capa`, {
            method: 'POST',
            body: dados
        });
    }

    static async buscarEventoPorId(idEvento) {

        const resposta = await fetch(
            `${this.API_URL}/api/eventos/${idEvento}`
        );

        if (!resposta.ok) {
            throw new Error("Evento não encontrado");
        }

        return await resposta.json();
    }

    static async login(email, senha) {

        return fetch(
            `${this.API_URL}/usuarios/login`,
            {
                method: 'POST',

                headers: {
                    'Content-Type': 'application/json'
                },

                body: JSON.stringify({
                    email,
                    senha
                })
            }
        );
    }

    static async atualizarFotoPerfil(idUsuario, foto) {

        const formData = new FormData();

        formData.append('foto', foto);

        return fetch(
            `${this.API_URL}/usuarios/${idUsuario}/foto`,
            {
                method: 'POST',
                body: formData
            }
        );
    }

    static async criarOuBuscarConversa(idUsuario, idOutroUsuario) {
        const resposta = await fetch(
            `${this.API_URL}/conversas`,
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    idUsuario,
                    idOutroUsuario
                })
            }
        );

        if (!resposta.ok) {
            const erro = await resposta.text();
            throw new Error(erro || 'Erro ao criar conversa');
        }

        return await resposta.json();
    }

    static async listarConversasDoUsuario(idUsuario) {
        const resposta = await fetch(
            `${this.API_URL}/conversas/usuario/${idUsuario}`
        );

        if (!resposta.ok) {
            throw new Error('Erro ao buscar conversas');
        }

        return await resposta.json();
    }

    static async enviarMensagem(idConversa, idRemetente, conteudo) {
        const resposta = await fetch(
            `${this.API_URL}/mensagens`,
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    idConversa,
                    idRemetente,
                    conteudo
                })
            }
        );

        if (!resposta.ok) {
            const erro = await resposta.text();
            throw new Error(erro || 'Erro ao enviar mensagem');
        }

        return await resposta.json();
    }

    static async listarMensagens(idConversa) {
        const resposta = await fetch(
            `${this.API_URL}/mensagens/conversa/${idConversa}`
        );

        if (!resposta.ok) {
            throw new Error('Erro ao buscar mensagens');
        }

        return await resposta.json();
    }
}

const fetch = (input, init) => ApiService.request(input, init);