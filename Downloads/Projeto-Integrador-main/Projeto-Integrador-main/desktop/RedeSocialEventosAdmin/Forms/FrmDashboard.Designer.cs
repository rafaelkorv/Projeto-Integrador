namespace RedeSocialEventosAdmin.Forms
{
  partial class FrmDashboard
  {
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblTitulo;
    private System.Windows.Forms.Label lblSubtitulo;
    private Guna.UI2.WinForms.Guna2Panel cardUsuarios;
    private System.Windows.Forms.Label lblTotalUsuarios;
    private System.Windows.Forms.Label lblCardUsuariosTitulo;
    private System.Windows.Forms.Label lblUsuariosSub;
    private FontAwesome.Sharp.IconPictureBox picIconUser;
    private Guna.UI2.WinForms.Guna2Panel cardEventos;
    private System.Windows.Forms.Label lblTotalEventos;
    private System.Windows.Forms.Label lblCardEventosTitulo;
    private System.Windows.Forms.Label lblEventosSub;
    private FontAwesome.Sharp.IconPictureBox picIconEventos;
    private Guna.UI2.WinForms.Guna2Panel cardComunidades;
    private System.Windows.Forms.Label lblTotalComunidades;
    private System.Windows.Forms.Label lblCardComunidadesTitulo;
    private System.Windows.Forms.Label lblComunidadesSub;
    private FontAwesome.Sharp.IconPictureBox picIconComunidades;
    private Guna.UI2.WinForms.Guna2Panel cardConteudo;
    private System.Windows.Forms.Label lblTotalPosts;
    private System.Windows.Forms.Label lblCardConteudoTitulo;
    private System.Windows.Forms.Label lblConteudoSub;
    private FontAwesome.Sharp.IconPictureBox picIconConteudo;
    private System.Windows.Forms.Label lblTituloEventosRecentes;
    private Guna.UI2.WinForms.Guna2DataGridView dgvProximosEventos;
    private System.Windows.Forms.Label lblTituloUsuariosRecentes;
    private Guna.UI2.WinForms.Guna2DataGridView dgvUltimosUsuarios;
    private Guna.UI2.WinForms.Guna2Button btnRefresh;

    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
      this.lblTitulo = new System.Windows.Forms.Label();
      this.lblSubtitulo = new System.Windows.Forms.Label();
      this.cardUsuarios = new Guna.UI2.WinForms.Guna2Panel();
      this.lblUsuariosSub = new System.Windows.Forms.Label();
      this.lblTotalUsuarios = new System.Windows.Forms.Label();
      this.lblCardUsuariosTitulo = new System.Windows.Forms.Label();
      this.picIconUser = new FontAwesome.Sharp.IconPictureBox();
      this.cardEventos = new Guna.UI2.WinForms.Guna2Panel();
      this.lblEventosSub = new System.Windows.Forms.Label();
      this.lblTotalEventos = new System.Windows.Forms.Label();
      this.lblCardEventosTitulo = new System.Windows.Forms.Label();
      this.picIconEventos = new FontAwesome.Sharp.IconPictureBox();
      this.cardComunidades = new Guna.UI2.WinForms.Guna2Panel();
      this.lblComunidadesSub = new System.Windows.Forms.Label();
      this.lblTotalComunidades = new System.Windows.Forms.Label();
      this.lblCardComunidadesTitulo = new System.Windows.Forms.Label();
      this.picIconComunidades = new FontAwesome.Sharp.IconPictureBox();
      this.cardConteudo = new Guna.UI2.WinForms.Guna2Panel();
      this.lblConteudoSub = new System.Windows.Forms.Label();
      this.lblTotalPosts = new System.Windows.Forms.Label();
      this.lblCardConteudoTitulo = new System.Windows.Forms.Label();
      this.picIconConteudo = new FontAwesome.Sharp.IconPictureBox();
      this.lblTituloEventosRecentes = new System.Windows.Forms.Label();
      this.dgvProximosEventos = new Guna.UI2.WinForms.Guna2DataGridView();
      this.lblTituloUsuariosRecentes = new System.Windows.Forms.Label();
      this.dgvUltimosUsuarios = new Guna.UI2.WinForms.Guna2DataGridView();
      this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
      this.cardUsuarios.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picIconUser)).BeginInit();
      this.cardEventos.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picIconEventos)).BeginInit();
      this.cardComunidades.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picIconComunidades)).BeginInit();
      this.cardConteudo.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picIconConteudo)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvProximosEventos)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvUltimosUsuarios)).BeginInit();
      this.SuspendLayout();
      // 
      // lblTitulo
      // 
      this.lblTitulo.AutoSize = true;
      this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
      this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.lblTitulo.Location = new System.Drawing.Point(25, 20);
      this.lblTitulo.Name = "lblTitulo";
      this.lblTitulo.Size = new System.Drawing.Size(262, 30);
      this.lblTitulo.TabIndex = 0;
      this.lblTitulo.Text = "Visão Geral da Operação";
      // 
      // lblSubtitulo
      // 
      this.lblSubtitulo.AutoSize = true;
      this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
      this.lblSubtitulo.Location = new System.Drawing.Point(27, 52);
      this.lblSubtitulo.Name = "lblSubtitulo";
      this.lblSubtitulo.Size = new System.Drawing.Size(430, 17);
      this.lblSubtitulo.TabIndex = 1;
      this.lblSubtitulo.Text = "Métricas em tempo real sobre usuários, eventos, comunidades e moderação.";
      // 
      // btnRefresh
      // 
      this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnRefresh.BorderRadius = 8;
      this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnRefresh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.btnRefresh.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
      this.btnRefresh.Location = new System.Drawing.Point(890, 20);
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new System.Drawing.Size(125, 40);
      this.btnRefresh.TabIndex = 7;
      this.btnRefresh.Text = " Atualizar";
      this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
      // 
      // cardUsuarios
      // 
      this.cardUsuarios.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
      this.cardUsuarios.BorderRadius = 12;
      this.cardUsuarios.BorderThickness = 1;
      this.cardUsuarios.Controls.Add(this.lblUsuariosSub);
      this.cardUsuarios.Controls.Add(this.lblTotalUsuarios);
      this.cardUsuarios.Controls.Add(this.lblCardUsuariosTitulo);
      this.cardUsuarios.Controls.Add(this.picIconUser);
      this.cardUsuarios.FillColor = System.Drawing.Color.White;
      this.cardUsuarios.Location = new System.Drawing.Point(25, 85);
      this.cardUsuarios.Name = "cardUsuarios";
      this.cardUsuarios.Size = new System.Drawing.Size(235, 115);
      this.cardUsuarios.TabIndex = 2;
      // 
      // lblUsuariosSub
      // 
      this.lblUsuariosSub.AutoSize = true;
      this.lblUsuariosSub.BackColor = System.Drawing.Color.Transparent;
      this.lblUsuariosSub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
      this.lblUsuariosSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
      this.lblUsuariosSub.Location = new System.Drawing.Point(15, 85);
      this.lblUsuariosSub.Name = "lblUsuariosSub";
      this.lblUsuariosSub.Size = new System.Drawing.Size(110, 15);
      this.lblUsuariosSub.TabIndex = 3;
      this.lblUsuariosSub.Text = "+0 hoje | 0 admins";
      // 
      // lblTotalUsuarios
      // 
      this.lblTotalUsuarios.AutoSize = true;
      this.lblTotalUsuarios.BackColor = System.Drawing.Color.Transparent;
      this.lblTotalUsuarios.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
      this.lblTotalUsuarios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.lblTotalUsuarios.Location = new System.Drawing.Point(12, 38);
      this.lblTotalUsuarios.Name = "lblTotalUsuarios";
      this.lblTotalUsuarios.Size = new System.Drawing.Size(33, 37);
      this.lblTotalUsuarios.TabIndex = 2;
      this.lblTotalUsuarios.Text = "0";
      // 
      // lblCardUsuariosTitulo
      // 
      this.lblCardUsuariosTitulo.AutoSize = true;
      this.lblCardUsuariosTitulo.BackColor = System.Drawing.Color.Transparent;
      this.lblCardUsuariosTitulo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.lblCardUsuariosTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
      this.lblCardUsuariosTitulo.Location = new System.Drawing.Point(15, 18);
      this.lblCardUsuariosTitulo.Name = "lblCardUsuariosTitulo";
      this.lblCardUsuariosTitulo.Size = new System.Drawing.Size(107, 15);
      this.lblCardUsuariosTitulo.TabIndex = 1;
      this.lblCardUsuariosTitulo.Text = "TOTAL DE USUÁRIOS";
      // 
      // picIconUser
      // 
      this.picIconUser.BackColor = System.Drawing.Color.Transparent;
      this.picIconUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.picIconUser.IconChar = FontAwesome.Sharp.IconChar.Users;
      this.picIconUser.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.picIconUser.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.picIconUser.IconSize = 40;
      this.picIconUser.Location = new System.Drawing.Point(180, 15);
      this.picIconUser.Name = "picIconUser";
      this.picIconUser.Size = new System.Drawing.Size(40, 40);
      this.picIconUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.picIconUser.TabIndex = 0;
      this.picIconUser.TabStop = false;
      // 
      // cardEventos
      // 
      this.cardEventos.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
      this.cardEventos.BorderRadius = 12;
      this.cardEventos.BorderThickness = 1;
      this.cardEventos.Controls.Add(this.lblEventosSub);
      this.cardEventos.Controls.Add(this.lblTotalEventos);
      this.cardEventos.Controls.Add(this.lblCardEventosTitulo);
      this.cardEventos.Controls.Add(this.picIconEventos);
      this.cardEventos.FillColor = System.Drawing.Color.White;
      this.cardEventos.Location = new System.Drawing.Point(275, 85);
      this.cardEventos.Name = "cardEventos";
      this.cardEventos.Size = new System.Drawing.Size(235, 115);
      this.cardEventos.TabIndex = 3;
      // 
      // lblEventosSub
      // 
      this.lblEventosSub.AutoSize = true;
      this.lblEventosSub.BackColor = System.Drawing.Color.Transparent;
      this.lblEventosSub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
      this.lblEventosSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.lblEventosSub.Location = new System.Drawing.Point(15, 85);
      this.lblEventosSub.Name = "lblEventosSub";
      this.lblEventosSub.Size = new System.Drawing.Size(140, 15);
      this.lblEventosSub.TabIndex = 3;
      this.lblEventosSub.Text = "0 agendados | 0 inscrições";
      // 
      // lblTotalEventos
      // 
      this.lblTotalEventos.AutoSize = true;
      this.lblTotalEventos.BackColor = System.Drawing.Color.Transparent;
      this.lblTotalEventos.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
      this.lblTotalEventos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.lblTotalEventos.Location = new System.Drawing.Point(12, 38);
      this.lblTotalEventos.Name = "lblTotalEventos";
      this.lblTotalEventos.Size = new System.Drawing.Size(33, 37);
      this.lblTotalEventos.TabIndex = 2;
      this.lblTotalEventos.Text = "0";
      // 
      // lblCardEventosTitulo
      // 
      this.lblCardEventosTitulo.AutoSize = true;
      this.lblCardEventosTitulo.BackColor = System.Drawing.Color.Transparent;
      this.lblCardEventosTitulo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.lblCardEventosTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
      this.lblCardEventosTitulo.Location = new System.Drawing.Point(15, 18);
      this.lblCardEventosTitulo.Name = "lblCardEventosTitulo";
      this.lblCardEventosTitulo.Size = new System.Drawing.Size(117, 15);
      this.lblCardEventosTitulo.TabIndex = 1;
      this.lblCardEventosTitulo.Text = "EVENTOS NA REDE";
      // 
      // picIconEventos
      // 
      this.picIconEventos.BackColor = System.Drawing.Color.Transparent;
      this.picIconEventos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
      this.picIconEventos.IconChar = FontAwesome.Sharp.IconChar.CalendarAlt;
      this.picIconEventos.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
      this.picIconEventos.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.picIconEventos.IconSize = 40;
      this.picIconEventos.Location = new System.Drawing.Point(180, 15);
      this.picIconEventos.Name = "picIconEventos";
      this.picIconEventos.Size = new System.Drawing.Size(40, 40);
      this.picIconEventos.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.picIconEventos.TabIndex = 0;
      this.picIconEventos.TabStop = false;
      // 
      // cardComunidades
      // 
      this.cardComunidades.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
      this.cardComunidades.BorderRadius = 12;
      this.cardComunidades.BorderThickness = 1;
      this.cardComunidades.Controls.Add(this.lblComunidadesSub);
      this.cardComunidades.Controls.Add(this.lblTotalComunidades);
      this.cardComunidades.Controls.Add(this.lblCardComunidadesTitulo);
      this.cardComunidades.Controls.Add(this.picIconComunidades);
      this.cardComunidades.FillColor = System.Drawing.Color.White;
      this.cardComunidades.Location = new System.Drawing.Point(525, 85);
      this.cardComunidades.Name = "cardComunidades";
      this.cardComunidades.Size = new System.Drawing.Size(235, 115);
      this.cardComunidades.TabIndex = 4;
      // 
      // lblComunidadesSub
      // 
      this.lblComunidadesSub.AutoSize = true;
      this.lblComunidadesSub.BackColor = System.Drawing.Color.Transparent;
      this.lblComunidadesSub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
      this.lblComunidadesSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(63)))), ((int)(((byte)(116)))));
      this.lblComunidadesSub.Location = new System.Drawing.Point(15, 85);
      this.lblComunidadesSub.Name = "lblComunidadesSub";
      this.lblComunidadesSub.Size = new System.Drawing.Size(126, 15);
      this.lblComunidadesSub.TabIndex = 3;
      this.lblComunidadesSub.Text = "Grupos e canais ativos";
      // 
      // lblTotalComunidades
      // 
      this.lblTotalComunidades.AutoSize = true;
      this.lblTotalComunidades.BackColor = System.Drawing.Color.Transparent;
      this.lblTotalComunidades.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
      this.lblTotalComunidades.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.lblTotalComunidades.Location = new System.Drawing.Point(12, 38);
      this.lblTotalComunidades.Name = "lblTotalComunidades";
      this.lblTotalComunidades.Size = new System.Drawing.Size(33, 37);
      this.lblTotalComunidades.TabIndex = 2;
      this.lblTotalComunidades.Text = "0";
      // 
      // lblCardComunidadesTitulo
      // 
      this.lblCardComunidadesTitulo.AutoSize = true;
      this.lblCardComunidadesTitulo.BackColor = System.Drawing.Color.Transparent;
      this.lblCardComunidadesTitulo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.lblCardComunidadesTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
      this.lblCardComunidadesTitulo.Location = new System.Drawing.Point(15, 18);
      this.lblCardComunidadesTitulo.Name = "lblCardComunidadesTitulo";
      this.lblCardComunidadesTitulo.Size = new System.Drawing.Size(95, 15);
      this.lblCardComunidadesTitulo.TabIndex = 1;
      this.lblCardComunidadesTitulo.Text = "COMUNIDADES";
      // 
      // picIconComunidades
      // 
      this.picIconComunidades.BackColor = System.Drawing.Color.Transparent;
      this.picIconComunidades.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(63)))), ((int)(((byte)(116)))));
      this.picIconComunidades.IconChar = FontAwesome.Sharp.IconChar.Comments;
      this.picIconComunidades.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(63)))), ((int)(((byte)(116)))));
      this.picIconComunidades.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.picIconComunidades.IconSize = 40;
      this.picIconComunidades.Location = new System.Drawing.Point(180, 15);
      this.picIconComunidades.Name = "picIconComunidades";
      this.picIconComunidades.Size = new System.Drawing.Size(40, 40);
      this.picIconComunidades.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.picIconComunidades.TabIndex = 0;
      this.picIconComunidades.TabStop = false;
      // 
      // cardConteudo
      // 
      this.cardConteudo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
      this.cardConteudo.BorderRadius = 12;
      this.cardConteudo.BorderThickness = 1;
      this.cardConteudo.Controls.Add(this.lblConteudoSub);
      this.cardConteudo.Controls.Add(this.lblTotalPosts);
      this.cardConteudo.Controls.Add(this.lblCardConteudoTitulo);
      this.cardConteudo.Controls.Add(this.picIconConteudo);
      this.cardConteudo.FillColor = System.Drawing.Color.White;
      this.cardConteudo.Location = new System.Drawing.Point(775, 85);
      this.cardConteudo.Name = "cardConteudo";
      this.cardConteudo.Size = new System.Drawing.Size(240, 115);
      this.cardConteudo.TabIndex = 5;
      // 
      // lblConteudoSub
      // 
      this.lblConteudoSub.AutoSize = true;
      this.lblConteudoSub.BackColor = System.Drawing.Color.Transparent;
      this.lblConteudoSub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
      this.lblConteudoSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
      this.lblConteudoSub.Location = new System.Drawing.Point(15, 85);
      this.lblConteudoSub.Name = "lblConteudoSub";
      this.lblConteudoSub.Size = new System.Drawing.Size(95, 15);
      this.lblConteudoSub.TabIndex = 3;
      this.lblConteudoSub.Text = "0 comentários";
      // 
      // lblTotalPosts
      // 
      this.lblTotalPosts.AutoSize = true;
      this.lblTotalPosts.BackColor = System.Drawing.Color.Transparent;
      this.lblTotalPosts.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
      this.lblTotalPosts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.lblTotalPosts.Location = new System.Drawing.Point(12, 38);
      this.lblTotalPosts.Name = "lblTotalPosts";
      this.lblTotalPosts.Size = new System.Drawing.Size(33, 37);
      this.lblTotalPosts.TabIndex = 2;
      this.lblTotalPosts.Text = "0";
      // 
      // lblCardConteudoTitulo
      // 
      this.lblCardConteudoTitulo.AutoSize = true;
      this.lblCardConteudoTitulo.BackColor = System.Drawing.Color.Transparent;
      this.lblCardConteudoTitulo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.lblCardConteudoTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
      this.lblCardConteudoTitulo.Location = new System.Drawing.Point(15, 18);
      this.lblCardConteudoTitulo.Name = "lblCardConteudoTitulo";
      this.lblCardConteudoTitulo.Size = new System.Drawing.Size(99, 15);
      this.lblCardConteudoTitulo.TabIndex = 1;
      this.lblCardConteudoTitulo.Text = "POSTS NA REDE";
      // 
      // picIconConteudo
      // 
      this.picIconConteudo.BackColor = System.Drawing.Color.Transparent;
      this.picIconConteudo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
      this.picIconConteudo.IconChar = FontAwesome.Sharp.IconChar.Stream;
      this.picIconConteudo.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
      this.picIconConteudo.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.picIconConteudo.IconSize = 40;
      this.picIconConteudo.Location = new System.Drawing.Point(185, 15);
      this.picIconConteudo.Name = "picIconConteudo";
      this.picIconConteudo.Size = new System.Drawing.Size(40, 40);
      this.picIconConteudo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.picIconConteudo.TabIndex = 0;
      this.picIconConteudo.TabStop = false;
      // 
      // lblTituloEventosRecentes
      // 
      this.lblTituloEventosRecentes.AutoSize = true;
      this.lblTituloEventosRecentes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
      this.lblTituloEventosRecentes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.lblTituloEventosRecentes.Location = new System.Drawing.Point(25, 220);
      this.lblTituloEventosRecentes.Name = "lblTituloEventosRecentes";
      this.lblTituloEventosRecentes.Size = new System.Drawing.Size(232, 21);
      this.lblTituloEventosRecentes.TabIndex = 6;
      this.lblTituloEventosRecentes.Text = "Próximos Eventos em Aberto";
      // 
      // dgvProximosEventos
      // 
      this.dgvProximosEventos.AllowUserToAddRows = false;
      this.dgvProximosEventos.AllowUserToDeleteRows = false;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
      this.dgvProximosEventos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvProximosEventos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
      | System.Windows.Forms.AnchorStyles.Left)));
      this.dgvProximosEventos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvProximosEventos.BackgroundColor = System.Drawing.Color.White;
      this.dgvProximosEventos.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dgvProximosEventos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
      this.dgvProximosEventos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvProximosEventos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.dgvProximosEventos.ColumnHeadersHeight = 35;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
      dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgvProximosEventos.DefaultCellStyle = dataGridViewCellStyle3;
      this.dgvProximosEventos.EnableHeadersVisualStyles = false;
      this.dgvProximosEventos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.dgvProximosEventos.Location = new System.Drawing.Point(25, 255);
      this.dgvProximosEventos.Name = "dgvProximosEventos";
      this.dgvProximosEventos.ReadOnly = true;
      this.dgvProximosEventos.RowHeadersVisible = false;
      this.dgvProximosEventos.RowTemplate.Height = 32;
      this.dgvProximosEventos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgvProximosEventos.Size = new System.Drawing.Size(485, 360);
      this.dgvProximosEventos.TabIndex = 7;
      this.dgvProximosEventos.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
      this.dgvProximosEventos.ThemeStyle.BackColor = System.Drawing.Color.White;
      this.dgvProximosEventos.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.dgvProximosEventos.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.dgvProximosEventos.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
      this.dgvProximosEventos.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.dgvProximosEventos.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
      this.dgvProximosEventos.ThemeStyle.ReadOnly = true;
      this.dgvProximosEventos.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
      this.dgvProximosEventos.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
      this.dgvProximosEventos.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.dgvProximosEventos.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.dgvProximosEventos.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
      this.dgvProximosEventos.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      // 
      // lblTituloUsuariosRecentes
      // 
      this.lblTituloUsuariosRecentes.AutoSize = true;
      this.lblTituloUsuariosRecentes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
      this.lblTituloUsuariosRecentes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.lblTituloUsuariosRecentes.Location = new System.Drawing.Point(525, 220);
      this.lblTituloUsuariosRecentes.Name = "lblTituloUsuariosRecentes";
      this.lblTituloUsuariosRecentes.Size = new System.Drawing.Size(232, 21);
      this.lblTituloUsuariosRecentes.TabIndex = 8;
      this.lblTituloUsuariosRecentes.Text = "Últimos Usuários Cadastrados";
      // 
      // dgvUltimosUsuarios
      // 
      this.dgvUltimosUsuarios.AllowUserToAddRows = false;
      this.dgvUltimosUsuarios.AllowUserToDeleteRows = false;
      dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
      this.dgvUltimosUsuarios.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
      this.dgvUltimosUsuarios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
      | System.Windows.Forms.AnchorStyles.Left) 
      | System.Windows.Forms.AnchorStyles.Right)));
      this.dgvUltimosUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvUltimosUsuarios.BackgroundColor = System.Drawing.Color.White;
      this.dgvUltimosUsuarios.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dgvUltimosUsuarios.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
      this.dgvUltimosUsuarios.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
      dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
      dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvUltimosUsuarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
      this.dgvUltimosUsuarios.ColumnHeadersHeight = 35;
      dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
      dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F);
      dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
      dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgvUltimosUsuarios.DefaultCellStyle = dataGridViewCellStyle6;
      this.dgvUltimosUsuarios.EnableHeadersVisualStyles = false;
      this.dgvUltimosUsuarios.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.dgvUltimosUsuarios.Location = new System.Drawing.Point(525, 255);
      this.dgvUltimosUsuarios.Name = "dgvUltimosUsuarios";
      this.dgvUltimosUsuarios.ReadOnly = true;
      this.dgvUltimosUsuarios.RowHeadersVisible = false;
      this.dgvUltimosUsuarios.RowTemplate.Height = 32;
      this.dgvUltimosUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgvUltimosUsuarios.Size = new System.Drawing.Size(490, 360);
      this.dgvUltimosUsuarios.TabIndex = 9;
      this.dgvUltimosUsuarios.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
      this.dgvUltimosUsuarios.ThemeStyle.BackColor = System.Drawing.Color.White;
      this.dgvUltimosUsuarios.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.dgvUltimosUsuarios.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.dgvUltimosUsuarios.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
      this.dgvUltimosUsuarios.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.dgvUltimosUsuarios.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
      this.dgvUltimosUsuarios.ThemeStyle.ReadOnly = true;
      this.dgvUltimosUsuarios.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
      this.dgvUltimosUsuarios.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
      this.dgvUltimosUsuarios.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.dgvUltimosUsuarios.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.dgvUltimosUsuarios.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
      this.dgvUltimosUsuarios.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      // 
      // FrmDashboard
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
      this.ClientSize = new System.Drawing.Size(1040, 640);
      this.Controls.Add(this.dgvUltimosUsuarios);
      this.Controls.Add(this.lblTituloUsuariosRecentes);
      this.Controls.Add(this.dgvProximosEventos);
      this.Controls.Add(this.lblTituloEventosRecentes);
      this.Controls.Add(this.btnRefresh);
      this.Controls.Add(this.cardConteudo);
      this.Controls.Add(this.cardComunidades);
      this.Controls.Add(this.cardEventos);
      this.Controls.Add(this.cardUsuarios);
      this.Controls.Add(this.lblSubtitulo);
      this.Controls.Add(this.lblTitulo);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "FrmDashboard";
      this.Text = "Dashboard";
      this.Load += new System.EventHandler(this.FrmDashboard_Load);
      this.cardUsuarios.ResumeLayout(false);
      this.cardUsuarios.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picIconUser)).EndInit();
      this.cardEventos.ResumeLayout(false);
      this.cardEventos.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picIconEventos)).EndInit();
      this.cardComunidades.ResumeLayout(false);
      this.cardComunidades.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picIconComunidades)).EndInit();
      this.cardConteudo.ResumeLayout(false);
      this.cardConteudo.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picIconConteudo)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvProximosEventos)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvUltimosUsuarios)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
