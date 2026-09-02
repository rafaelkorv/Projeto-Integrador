// api_service.js
export class ApiService {
    static API_URL = 'http://localhost:8080';
    //static API_URL = 'https://projeto-integrador-m4jn.onrender.com';
    //static API_URL = 'http://143.106.241.xx:8080';
    static STORAGE_KEY = 'usuarioLogado';

    static getStorage(tipo = 'session') {
        if (typeof window === 'undefined') return null;
        return tipo === 'session' ? window.sessionStorage : window.localStorage;
    }

    static getUsuarioLogado() {
        const storage = this.getStorage('session');
        const raw = storage ? storage.getItem(this.STORAGE_KEY) : null;

        if (raw) {
            try {
                return JSON.parse(raw);
            } catch (error) {
                this.fazerLogout();
                return null;
            }
        }

        const legado = this.getStorage('local')?.getItem(this.STORAGE_KEY);
        if (legado) {
            try {
                const usuario = JSON.parse(legado);
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
        if (storage) storage.setItem(this.STORAGE_KEY, JSON.stringify(usuario));
        this.getStorage('local')?.removeItem(this.STORAGE_KEY);
    }

    static fazerLogout() {
        this.getStorage('session')?.removeItem(this.STORAGE_KEY);
        this.getStorage('local')?.removeItem(this.STORAGE_KEY);
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

    static async listarParticipantesEvento(idEvento) {
        return await (
            await fetch(
                `${this.API_URL}/api/eventos/${idEvento}/participantes`
            )
        ).json();
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
        status = null,
        categoria = null,
        comunidadeId = null,
        dataInicio = null,
        dataFim = null,
        page = 0,
        size = 12
    } = {}) {

        const params = new URLSearchParams();

        if (texto) {
            params.append("texto", texto);
        }

        if (status) {
            params.append("status", status);
        }

        if (categoria) {
            params.append("categoria", categoria);
        }

        if (comunidadeId) {
            params.append("comunidadeId", comunidadeId);
        }

        if (dataInicio) {
            params.append("dataInicio", dataInicio);
        }

        if (dataFim) {
            params.append("dataFim", dataFim);
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