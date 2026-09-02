namespace RedeSocialEventosAdmin.Forms
{
  partial class FrmPrincipal
  {
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Panel pnlMenuLateral;
    private FontAwesome.Sharp.IconButton btnPerfil;
    private FontAwesome.Sharp.IconButton btnRelatorios;
    private FontAwesome.Sharp.IconButton btnModeracao;
    private FontAwesome.Sharp.IconButton btnComunidades;
    private FontAwesome.Sharp.IconButton btnEventos;
    private FontAwesome.Sharp.IconButton btnUsuarios;
    private FontAwesome.Sharp.IconButton btnDashboard;
    private System.Windows.Forms.Panel pnlLogoContainer;
    private System.Windows.Forms.Label lblNomePainel;
    private System.Windows.Forms.Label lblSubBrand;
    private FontAwesome.Sharp.IconPictureBox picLogoPrincipal;
    private System.Windows.Forms.Panel pnlTopo;
    private System.Windows.Forms.Label lblTituloJanela;
    private System.Windows.Forms.Panel pnlConteudo;
    private FontAwesome.Sharp.IconButton btnSair;
    private Guna.UI2.WinForms.Guna2Panel pnlAdminHeaderInfo;
    private System.Windows.Forms.Label lblAdminHeaderNome;
    private Guna.UI2.WinForms.Guna2Chip chipHeaderRole;
    private FontAwesome.Sharp.IconPictureBox picHeaderAvatar;

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
      this.pnlMenuLateral = new System.Windows.Forms.Panel();
      this.btnSair = new FontAwesome.Sharp.IconButton();
      this.btnPerfil = new FontAwesome.Sharp.IconButton();
      this.btnRelatorios = new FontAwesome.Sharp.IconButton();
      this.btnModeracao = new FontAwesome.Sharp.IconButton();
      this.btnComunidades = new FontAwesome.Sharp.IconButton();
      this.btnEventos = new FontAwesome.Sharp.IconButton();
      this.btnUsuarios = new FontAwesome.Sharp.IconButton();
      this.btnDashboard = new FontAwesome.Sharp.IconButton();
      this.pnlLogoContainer = new System.Windows.Forms.Panel();
      this.lblSubBrand = new System.Windows.Forms.Label();
      this.lblNomePainel = new System.Windows.Forms.Label();
      this.picLogoPrincipal = new FontAwesome.Sharp.IconPictureBox();
      this.pnlTopo = new System.Windows.Forms.Panel();
      this.pnlAdminHeaderInfo = new Guna.UI2.WinForms.Guna2Panel();
      this.chipHeaderRole = new Guna.UI2.WinForms.Guna2Chip();
      this.lblAdminHeaderNome = new System.Windows.Forms.Label();
      this.picHeaderAvatar = new FontAwesome.Sharp.IconPictureBox();
      this.lblTituloJanela = new System.Windows.Forms.Label();
      this.pnlConteudo = new System.Windows.Forms.Panel();
      this.pnlMenuLateral.SuspendLayout();
      this.pnlLogoContainer.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picLogoPrincipal)).BeginInit();
      this.pnlTopo.SuspendLayout();
      this.pnlAdminHeaderInfo.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picHeaderAvatar)).BeginInit();
      this.SuspendLayout();
      // 
      // pnlMenuLateral
      // 
      this.pnlMenuLateral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.pnlMenuLateral.Controls.Add(this.btnSair);
      this.pnlMenuLateral.Controls.Add(this.btnPerfil);
      this.pnlMenuLateral.Controls.Add(this.btnRelatorios);
      this.pnlMenuLateral.Controls.Add(this.btnModeracao);
      this.pnlMenuLateral.Controls.Add(this.btnComunidades);
      this.pnlMenuLateral.Controls.Add(this.btnEventos);
      this.pnlMenuLateral.Controls.Add(this.btnUsuarios);
      this.pnlMenuLateral.Controls.Add(this.btnDashboard);
      this.pnlMenuLateral.Controls.Add(this.pnlLogoContainer);
      this.pnlMenuLateral.Dock = System.Windows.Forms.DockStyle.Left;
      this.pnlMenuLateral.Location = new System.Drawing.Point(0, 0);
      this.pnlMenuLateral.Name = "pnlMenuLateral";
      this.pnlMenuLateral.Size = new System.Drawing.Size(250, 720);
      this.pnlMenuLateral.TabIndex = 0;
      // 
      // btnSair
      // 
      this.btnSair.BackColor = System.Drawing.Color.Transparent;
      this.btnSair.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnSair.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.btnSair.FlatAppearance.BorderSize = 0;
      this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnSair.Font = new System.Drawing.Font("Segoe UI", 10.5F);
      this.btnSair.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
      this.btnSair.IconChar = FontAwesome.Sharp.IconChar.SignOutAlt;
      this.btnSair.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
      this.btnSair.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.btnSair.IconSize = 24;
      this.btnSair.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnSair.Location = new System.Drawing.Point(0, 665);
      this.btnSair.Name = "btnSair";
      this.btnSair.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
      this.btnSair.Size = new System.Drawing.Size(250, 55);
      this.btnSair.TabIndex = 8;
      this.btnSair.Text = " Sair do Sistema";
      this.btnSair.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnSair.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnSair.UseVisualStyleBackColor = false;
      this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
      // 
      // btnPerfil
      // 
      this.btnPerfil.BackColor = System.Drawing.Color.Transparent;
      this.btnPerfil.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnPerfil.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnPerfil.FlatAppearance.BorderSize = 0;
      this.btnPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnPerfil.Font = new System.Drawing.Font("Segoe UI", 10.5F);
      this.btnPerfil.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnPerfil.IconChar = FontAwesome.Sharp.IconChar.UserShield;
      this.btnPerfil.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnPerfil.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.btnPerfil.IconSize = 24;
      this.btnPerfil.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnPerfil.Location = new System.Drawing.Point(0, 420);
      this.btnPerfil.Name = "btnPerfil";
      this.btnPerfil.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
      this.btnPerfil.Size = new System.Drawing.Size(250, 55);
      this.btnPerfil.TabIndex = 7;
      this.btnPerfil.Text = " Meu Perfil Admin";
      this.btnPerfil.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnPerfil.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnPerfil.UseVisualStyleBackColor = false;
      this.btnPerfil.Click += new System.EventHandler(this.btnPerfil_Click);
      // 
      // btnRelatorios
      // 
      this.btnRelatorios.BackColor = System.Drawing.Color.Transparent;
      this.btnRelatorios.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnRelatorios.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnRelatorios.FlatAppearance.BorderSize = 0;
      this.btnRelatorios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnRelatorios.Font = new System.Drawing.Font("Segoe UI", 10.5F);
      this.btnRelatorios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnRelatorios.IconChar = FontAwesome.Sharp.IconChar.ChartBar;
      this.btnRelatorios.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnRelatorios.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.btnRelatorios.IconSize = 24;
      this.btnRelatorios.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnRelatorios.Location = new System.Drawing.Point(0, 365);
      this.btnRelatorios.Name = "btnRelatorios";
      this.btnRelatorios.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
      this.btnRelatorios.Size = new System.Drawing.Size(250, 55);
      this.btnRelatorios.TabIndex = 6;
      this.btnRelatorios.Text = " Relatórios";
      this.btnRelatorios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnRelatorios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnRelatorios.UseVisualStyleBackColor = false;
      this.btnRelatorios.Click += new System.EventHandler(this.btnRelatorios_Click);
      // 
      // btnModeracao
      // 
      this.btnModeracao.BackColor = System.Drawing.Color.Transparent;
      this.btnModeracao.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnModeracao.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnModeracao.FlatAppearance.BorderSize = 0;
      this.btnModeracao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnModeracao.Font = new System.Drawing.Font("Segoe UI", 10.5F);
      this.btnModeracao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnModeracao.IconChar = FontAwesome.Sharp.IconChar.ShieldAlt;
      this.btnModeracao.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnModeracao.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.btnModeracao.IconSize = 24;
      this.btnModeracao.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnModeracao.Location = new System.Drawing.Point(0, 310);
      this.btnModeracao.Name = "btnModeracao";
      this.btnModeracao.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
      this.btnModeracao.Size = new System.Drawing.Size(250, 55);
      this.btnModeracao.TabIndex = 5;
      this.btnModeracao.Text = " Moderação";
      this.btnModeracao.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnModeracao.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnModeracao.UseVisualStyleBackColor = false;
      this.btnModeracao.Click += new System.EventHandler(this.btnModeracao_Click);
      // 
      // btnComunidades
      // 
      this.btnComunidades.BackColor = System.Drawing.Color.Transparent;
      this.btnComunidades.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnComunidades.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnComunidades.FlatAppearance.BorderSize = 0;
      this.btnComunidades.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnComunidades.Font = new System.Drawing.Font("Segoe UI", 10.5F);
      this.btnComunidades.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnComunidades.IconChar = FontAwesome.Sharp.IconChar.Comments;
      this.btnComunidades.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnComunidades.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.btnComunidades.IconSize = 24;
      this.btnComunidades.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnComunidades.Location = new System.Drawing.Point(0, 255);
      this.btnComunidades.Name = "btnComunidades";
      this.btnComunidades.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
      this.btnComunidades.Size = new System.Drawing.Size(250, 55);
      this.btnComunidades.TabIndex = 4;
      this.btnComunidades.Text = " Comunidades";
      this.btnComunidades.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnComunidades.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnComunidades.UseVisualStyleBackColor = false;
      this.btnComunidades.Click += new System.EventHandler(this.btnComunidades_Click);
      // 
      // btnEventos
      // 
      this.btnEventos.BackColor = System.Drawing.Color.Transparent;
      this.btnEventos.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnEventos.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnEventos.FlatAppearance.BorderSize = 0;
      this.btnEventos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnEventos.Font = new System.Drawing.Font("Segoe UI", 10.5F);
      this.btnEventos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnEventos.IconChar = FontAwesome.Sharp.IconChar.CalendarAlt;
      this.btnEventos.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnEventos.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.btnEventos.IconSize = 24;
      this.btnEventos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnEventos.Location = new System.Drawing.Point(0, 200);
      this.btnEventos.Name = "btnEventos";
      this.btnEventos.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
      this.btnEventos.Size = new System.Drawing.Size(250, 55);
      this.btnEventos.TabIndex = 3;
      this.btnEventos.Text = " Eventos";
      this.btnEventos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnEventos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnEventos.UseVisualStyleBackColor = false;
      this.btnEventos.Click += new System.EventHandler(this.btnEventos_Click);
      // 
      // btnUsuarios
      // 
      this.btnUsuarios.BackColor = System.Drawing.Color.Transparent;
      this.btnUsuarios.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnUsuarios.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnUsuarios.FlatAppearance.BorderSize = 0;
      this.btnUsuarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnUsuarios.Font = new System.Drawing.Font("Segoe UI", 10.5F);
      this.btnUsuarios.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnUsuarios.IconChar = FontAwesome.Sharp.IconChar.Users;
      this.btnUsuarios.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnUsuarios.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.btnUsuarios.IconSize = 24;
      this.btnUsuarios.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnUsuarios.Location = new System.Drawing.Point(0, 145);
      this.btnUsuarios.Name = "btnUsuarios";
      this.btnUsuarios.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
      this.btnUsuarios.Size = new System.Drawing.Size(250, 55);
      this.btnUsuarios.TabIndex = 2;
      this.btnUsuarios.Text = " Usuários & Roles";
      this.btnUsuarios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnUsuarios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnUsuarios.UseVisualStyleBackColor = false;
      this.btnUsuarios.Click += new System.EventHandler(this.btnUsuarios_Click);
      // 
      // btnDashboard
      // 
      this.btnDashboard.BackColor = System.Drawing.Color.Transparent;
      this.btnDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnDashboard.Dock = System.Windows.Forms.DockStyle.Top;
      this.btnDashboard.FlatAppearance.BorderSize = 0;
      this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnDashboard.Font = new System.Drawing.Font("Segoe UI", 10.5F);
      this.btnDashboard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnDashboard.IconChar = FontAwesome.Sharp.IconChar.ChartPie;
      this.btnDashboard.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnDashboard.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.btnDashboard.IconSize = 24;
      this.btnDashboard.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnDashboard.Location = new System.Drawing.Point(0, 90);
      this.btnDashboard.Name = "btnDashboard";
      this.btnDashboard.Padding = new System.Windows.Forms.Padding(18, 0, 0, 0);
      this.btnDashboard.Size = new System.Drawing.Size(250, 55);
      this.btnDashboard.TabIndex = 1;
      this.btnDashboard.Text = " Dashboard";
      this.btnDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.btnDashboard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
      this.btnDashboard.UseVisualStyleBackColor = false;
      this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
      // 
      // pnlLogoContainer
      // 
      this.pnlLogoContainer.BackColor = System.Drawing.Color.Transparent;
      this.pnlLogoContainer.Controls.Add(this.lblSubBrand);
      this.pnlLogoContainer.Controls.Add(this.lblNomePainel);
      this.pnlLogoContainer.Controls.Add(this.picLogoPrincipal);
      this.pnlLogoContainer.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlLogoContainer.Location = new System.Drawing.Point(0, 0);
      this.pnlLogoContainer.Name = "pnlLogoContainer";
      this.pnlLogoContainer.Size = new System.Drawing.Size(250, 90);
      this.pnlLogoContainer.TabIndex = 0;
      // 
      // lblSubBrand
      // 
      this.lblSubBrand.AutoSize = true;
      this.lblSubBrand.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
      this.lblSubBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.lblSubBrand.Location = new System.Drawing.Point(70, 48);
      this.lblSubBrand.Name = "lblSubBrand";
      this.lblSubBrand.Size = new System.Drawing.Size(126, 13);
      this.lblSubBrand.TabIndex = 2;
      this.lblSubBrand.Text = "SUPER ADMIN CONSOLE";
      // 
      // lblNomePainel
      // 
      this.lblNomePainel.AutoSize = true;
      this.lblNomePainel.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
      this.lblNomePainel.ForeColor = System.Drawing.Color.White;
      this.lblNomePainel.Location = new System.Drawing.Point(70, 24);
      this.lblNomePainel.Name = "lblNomePainel";
      this.lblNomePainel.Size = new System.Drawing.Size(100, 25);
      this.lblNomePainel.TabIndex = 1;
      this.lblNomePainel.Text = "SocialJoin";
      // 
      // picLogoPrincipal
      // 
      this.picLogoPrincipal.BackColor = System.Drawing.Color.Transparent;
      this.picLogoPrincipal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.picLogoPrincipal.IconChar = FontAwesome.Sharp.IconChar.ShieldAlt;
      this.picLogoPrincipal.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.picLogoPrincipal.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.picLogoPrincipal.IconSize = 40;
      this.picLogoPrincipal.Location = new System.Drawing.Point(18, 24);
      this.picLogoPrincipal.Name = "picLogoPrincipal";
      this.picLogoPrincipal.Size = new System.Drawing.Size(40, 40);
      this.picLogoPrincipal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.picLogoPrincipal.TabIndex = 0;
      this.picLogoPrincipal.TabStop = false;
      // 
      // pnlTopo
      // 
      this.pnlTopo.BackColor = System.Drawing.Color.White;
      this.pnlTopo.Controls.Add(this.pnlAdminHeaderInfo);
      this.pnlTopo.Controls.Add(this.lblTituloJanela);
      this.pnlTopo.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlTopo.Location = new System.Drawing.Point(250, 0);
      this.pnlTopo.Name = "pnlTopo";
      this.pnlTopo.Size = new System.Drawing.Size(1030, 75);
      this.pnlTopo.TabIndex = 1;
      // 
      // pnlAdminHeaderInfo
      // 
      this.pnlAdminHeaderInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlAdminHeaderInfo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
      this.pnlAdminHeaderInfo.BorderRadius = 10;
      this.pnlAdminHeaderInfo.BorderThickness = 1;
      this.pnlAdminHeaderInfo.Controls.Add(this.chipHeaderRole);
      this.pnlAdminHeaderInfo.Controls.Add(this.lblAdminHeaderNome);
      this.pnlAdminHeaderInfo.Controls.Add(this.picHeaderAvatar);
      this.pnlAdminHeaderInfo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
      this.pnlAdminHeaderInfo.Location = new System.Drawing.Point(740, 12);
      this.pnlAdminHeaderInfo.Name = "pnlAdminHeaderInfo";
      this.pnlAdminHeaderInfo.Size = new System.Drawing.Size(265, 50);
      this.pnlAdminHeaderInfo.TabIndex = 1;
      // 
      // chipHeaderRole
      // 
      this.chipHeaderRole.BackColor = System.Drawing.Color.Transparent;
      this.chipHeaderRole.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
      this.chipHeaderRole.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
      this.chipHeaderRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.chipHeaderRole.Location = new System.Drawing.Point(52, 26);
      this.chipHeaderRole.Name = "chipHeaderRole";
      this.chipHeaderRole.Size = new System.Drawing.Size(95, 20);
      this.chipHeaderRole.TabIndex = 2;
      this.chipHeaderRole.Text = "SUPER ADMIN";
      // 
      // lblAdminHeaderNome
      // 
      this.lblAdminHeaderNome.AutoSize = true;
      this.lblAdminHeaderNome.BackColor = System.Drawing.Color.Transparent;
      this.lblAdminHeaderNome.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.lblAdminHeaderNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.lblAdminHeaderNome.Location = new System.Drawing.Point(52, 8);
      this.lblAdminHeaderNome.Name = "lblAdminHeaderNome";
      this.lblAdminHeaderNome.Size = new System.Drawing.Size(86, 15);
      this.lblAdminHeaderNome.TabIndex = 1;
      this.lblAdminHeaderNome.Text = "Administrador";
      // 
      // picHeaderAvatar
      // 
      this.picHeaderAvatar.BackColor = System.Drawing.Color.Transparent;
      this.picHeaderAvatar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.picHeaderAvatar.IconChar = FontAwesome.Sharp.IconChar.UserShield;
      this.picHeaderAvatar.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.picHeaderAvatar.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.picHeaderAvatar.IconSize = 30;
      this.picHeaderAvatar.Location = new System.Drawing.Point(10, 10);
      this.picHeaderAvatar.Name = "picHeaderAvatar";
      this.picHeaderAvatar.Size = new System.Drawing.Size(30, 30);
      this.picHeaderAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.picHeaderAvatar.TabIndex = 0;
      this.picHeaderAvatar.TabStop = false;
      // 
      // lblTituloJanela
      // 
      this.lblTituloJanela.AutoSize = true;
      this.lblTituloJanela.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
      this.lblTituloJanela.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.lblTituloJanela.Location = new System.Drawing.Point(25, 22);
      this.lblTituloJanela.Name = "lblTituloJanela";
      this.lblTituloJanela.Size = new System.Drawing.Size(126, 30);
      this.lblTituloJanela.TabIndex = 0;
      this.lblTituloJanela.Text = "Dashboard";
      // 
      // pnlConteudo
      // 
      this.pnlConteudo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
      this.pnlConteudo.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlConteudo.Location = new System.Drawing.Point(250, 75);
      this.pnlConteudo.Name = "pnlConteudo";
      this.pnlConteudo.Size = new System.Drawing.Size(1030, 645);
      this.pnlConteudo.TabIndex = 2;
      // 
      // FrmPrincipal
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1280, 720);
      this.Controls.Add(this.pnlConteudo);
      this.Controls.Add(this.pnlTopo);
      this.Controls.Add(this.pnlMenuLateral);
      this.MinimumSize = new System.Drawing.Size(1100, 650);
      this.Name = "FrmPrincipal";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "SocialJoin - Painel de Controle e Super Administração";
      this.Load += new System.EventHandler(this.FrmPrincipal_Load);
      this.pnlMenuLateral.ResumeLayout(false);
      this.pnlLogoContainer.ResumeLayout(false);
      this.pnlLogoContainer.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picLogoPrincipal)).EndInit();
      this.pnlTopo.ResumeLayout(false);
      this.pnlTopo.PerformLayout();
      this.pnlAdminHeaderInfo.ResumeLayout(false);
      this.pnlAdminHeaderInfo.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picHeaderAvatar)).EndInit();
      this.ResumeLayout(false);
    }
  }
}
