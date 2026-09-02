namespace RedeSocialEventosAdmin.Forms
{
  partial class FrmRelatorios
  {
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblHeaderTitle;
    private System.Windows.Forms.Label lblSubInfo;
    private Guna.UI2.WinForms.Guna2Button btnTabRelatoriosComuns;
    private Guna.UI2.WinForms.Guna2Button btnTabAnalyticsApp;
    private Guna.UI2.WinForms.Guna2Panel pnlHeaderTabs;

    // Painel 1: Relatórios Comuns
    private Guna.UI2.WinForms.Guna2Panel pnlRelatoriosComuns;
    private Guna.UI2.WinForms.Guna2Button btnSubUsuarios;
    private Guna.UI2.WinForms.Guna2Button btnSubEventos;
    private Guna.UI2.WinForms.Guna2Button btnSubComunidades;
    private Guna.UI2.WinForms.Guna2ComboBox cmbSelecaoItem;
    private Guna.UI2.WinForms.Guna2Button btnVerFichaIndividual;
    private Guna.UI2.WinForms.Guna2Button btnExportarIndividual;
    private Guna.UI2.WinForms.Guna2Button btnExportarGeral;
    private Guna.UI2.WinForms.Guna2DataGridView dgvRelatorioComum;
    private Guna.UI2.WinForms.Guna2Panel pnlFichaIndividual;
    private System.Windows.Forms.Label lblFichaNome;
    private System.Windows.Forms.Label lblFichaInfo1;
    private System.Windows.Forms.Label lblFichaInfo2;
    private System.Windows.Forms.Label lblFichaInfo3;
    private System.Windows.Forms.Label lblFichaInfo4;
    private Guna.UI2.WinForms.Guna2Chip chipFichaScore;
    private Guna.UI2.WinForms.Guna2Chip chipFichaRole;
    private Guna.UI2.WinForms.Guna2Chip chipFichaStatus;
    private FontAwesome.Sharp.IconPictureBox picFichaAvatar;

    // Painel 2: Analytics & Engajamento Global
    private Guna.UI2.WinForms.Guna2Panel pnlAnalyticsApp;
    private RedeSocialEventosAdmin.Controls.ModernDonutChart chartDonutRoles;
    private RedeSocialEventosAdmin.Controls.ModernBarChart chartBarEventos;
    private RedeSocialEventosAdmin.Controls.ModernBarChart chartBarComunidades;
    private Guna.UI2.WinForms.Guna2Panel pnlKpisEngajamento;
    private System.Windows.Forms.Label lblKpiEngajamentoTitulo;
    private System.Windows.Forms.Label lblKpiEngajamentoSub;
    private Guna.UI2.WinForms.Guna2Button btnExportarAnalyticsCsv;

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
      this.lblHeaderTitle = new System.Windows.Forms.Label();
      this.lblSubInfo = new System.Windows.Forms.Label();
      this.pnlHeaderTabs = new Guna.UI2.WinForms.Guna2Panel();
      this.btnTabAnalyticsApp = new Guna.UI2.WinForms.Guna2Button();
      this.btnTabRelatoriosComuns = new Guna.UI2.WinForms.Guna2Button();
      this.pnlRelatoriosComuns = new Guna.UI2.WinForms.Guna2Panel();
      this.btnExportarGeral = new Guna.UI2.WinForms.Guna2Button();
      this.pnlFichaIndividual = new Guna.UI2.WinForms.Guna2Panel();
      this.chipFichaStatus = new Guna.UI2.WinForms.Guna2Chip();
      this.chipFichaRole = new Guna.UI2.WinForms.Guna2Chip();
      this.chipFichaScore = new Guna.UI2.WinForms.Guna2Chip();
      this.lblFichaInfo4 = new System.Windows.Forms.Label();
      this.lblFichaInfo3 = new System.Windows.Forms.Label();
      this.lblFichaInfo2 = new System.Windows.Forms.Label();
      this.lblFichaInfo1 = new System.Windows.Forms.Label();
      this.lblFichaNome = new System.Windows.Forms.Label();
      this.picFichaAvatar = new FontAwesome.Sharp.IconPictureBox();
      this.dgvRelatorioComum = new Guna.UI2.WinForms.Guna2DataGridView();
      this.btnExportarIndividual = new Guna.UI2.WinForms.Guna2Button();
      this.btnVerFichaIndividual = new Guna.UI2.WinForms.Guna2Button();
      this.cmbSelecaoItem = new Guna.UI2.WinForms.Guna2ComboBox();
      this.btnSubComunidades = new Guna.UI2.WinForms.Guna2Button();
      this.btnSubEventos = new Guna.UI2.WinForms.Guna2Button();
      this.btnSubUsuarios = new Guna.UI2.WinForms.Guna2Button();
      this.pnlAnalyticsApp = new Guna.UI2.WinForms.Guna2Panel();
      this.btnExportarAnalyticsCsv = new Guna.UI2.WinForms.Guna2Button();
      this.pnlKpisEngajamento = new Guna.UI2.WinForms.Guna2Panel();
      this.lblKpiEngajamentoSub = new System.Windows.Forms.Label();
      this.lblKpiEngajamentoTitulo = new System.Windows.Forms.Label();
      this.chartBarComunidades = new RedeSocialEventosAdmin.Controls.ModernBarChart();
      this.chartBarEventos = new RedeSocialEventosAdmin.Controls.ModernBarChart();
      this.chartDonutRoles = new RedeSocialEventosAdmin.Controls.ModernDonutChart();
      this.pnlHeaderTabs.SuspendLayout();
      this.pnlRelatoriosComuns.SuspendLayout();
      this.pnlFichaIndividual.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picFichaAvatar)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvRelatorioComum)).BeginInit();
      this.pnlAnalyticsApp.SuspendLayout();
      this.pnlKpisEngajamento.SuspendLayout();
      this.SuspendLayout();
      // 
      // lblHeaderTitle
      // 
      this.lblHeaderTitle.AutoSize = true;
      this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
      this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.lblHeaderTitle.Location = new System.Drawing.Point(25, 15);
      this.lblHeaderTitle.Name = "lblHeaderTitle";
      this.lblHeaderTitle.Size = new System.Drawing.Size(248, 30);
      this.lblHeaderTitle.TabIndex = 0;
      this.lblHeaderTitle.Text = "Relatórios e Analytics";
      // 
      // lblSubInfo
      // 
      this.lblSubInfo.AutoSize = true;
      this.lblSubInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.lblSubInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
      this.lblSubInfo.Location = new System.Drawing.Point(27, 45);
      this.lblSubInfo.Name = "lblSubInfo";
      this.lblSubInfo.Size = new System.Drawing.Size(430, 17);
      this.lblSubInfo.TabIndex = 1;
      this.lblSubInfo.Text = "Relatórios operacionais individuais e métricas de engajamento do SocialJoin.";
      // 
      // pnlHeaderTabs
      // 
      this.pnlHeaderTabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlHeaderTabs.Controls.Add(this.btnTabAnalyticsApp);
      this.pnlHeaderTabs.Controls.Add(this.btnTabRelatoriosComuns);
      this.pnlHeaderTabs.Location = new System.Drawing.Point(25, 70);
      this.pnlHeaderTabs.Name = "pnlHeaderTabs";
      this.pnlHeaderTabs.Size = new System.Drawing.Size(990, 42);
      this.pnlHeaderTabs.TabIndex = 2;
      // 
      // btnTabAnalyticsApp
      // 
      this.btnTabAnalyticsApp.BorderRadius = 8;
      this.btnTabAnalyticsApp.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnTabAnalyticsApp.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.btnTabAnalyticsApp.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
      this.btnTabAnalyticsApp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.btnTabAnalyticsApp.Location = new System.Drawing.Point(295, 2);
      this.btnTabAnalyticsApp.Name = "btnTabAnalyticsApp";
      this.btnTabAnalyticsApp.Size = new System.Drawing.Size(305, 38);
      this.btnTabAnalyticsApp.TabIndex = 1;
      this.btnTabAnalyticsApp.Text = " Analytics & Engajamento da Rede";
      this.btnTabAnalyticsApp.Click += new System.EventHandler(this.btnTabAnalyticsApp_Click);
      // 
      // btnTabRelatoriosComuns
      // 
      this.btnTabRelatoriosComuns.BorderRadius = 8;
      this.btnTabRelatoriosComuns.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnTabRelatoriosComuns.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.btnTabRelatoriosComuns.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
      this.btnTabRelatoriosComuns.ForeColor = System.Drawing.Color.White;
      this.btnTabRelatoriosComuns.Location = new System.Drawing.Point(0, 2);
      this.btnTabRelatoriosComuns.Name = "btnTabRelatoriosComuns";
      this.btnTabRelatoriosComuns.Size = new System.Drawing.Size(285, 38);
      this.btnTabRelatoriosComuns.TabIndex = 0;
      this.btnTabRelatoriosComuns.Text = " Relatórios Cadastrais & Fichas";
      this.btnTabRelatoriosComuns.Click += new System.EventHandler(this.btnTabRelatoriosComuns_Click);
      // 
      // pnlRelatoriosComuns
      // 
      this.pnlRelatoriosComuns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
      | System.Windows.Forms.AnchorStyles.Left) 
      | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlRelatoriosComuns.Controls.Add(this.btnExportarGeral);
      this.pnlRelatoriosComuns.Controls.Add(this.pnlFichaIndividual);
      this.pnlRelatoriosComuns.Controls.Add(this.dgvRelatorioComum);
      this.pnlRelatoriosComuns.Controls.Add(this.btnExportarIndividual);
      this.pnlRelatoriosComuns.Controls.Add(this.btnVerFichaIndividual);
      this.pnlRelatoriosComuns.Controls.Add(this.cmbSelecaoItem);
      this.pnlRelatoriosComuns.Controls.Add(this.btnSubComunidades);
      this.pnlRelatoriosComuns.Controls.Add(this.btnSubEventos);
      this.pnlRelatoriosComuns.Controls.Add(this.btnSubUsuarios);
      this.pnlRelatoriosComuns.Location = new System.Drawing.Point(25, 118);
      this.pnlRelatoriosComuns.Name = "pnlRelatoriosComuns";
      this.pnlRelatoriosComuns.Size = new System.Drawing.Size(990, 515);
      this.pnlRelatoriosComuns.TabIndex = 3;
      // 
      // btnExportarGeral
      // 
      this.btnExportarGeral.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportarGeral.BorderRadius = 8;
      this.btnExportarGeral.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnExportarGeral.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.btnExportarGeral.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.btnExportarGeral.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.btnExportarGeral.Location = new System.Drawing.Point(860, 50);
      this.btnExportarGeral.Name = "btnExportarGeral";
      this.btnExportarGeral.Size = new System.Drawing.Size(130, 38);
      this.btnExportarGeral.TabIndex = 8;
      this.btnExportarGeral.Text = " Exportar Tabela";
      this.btnExportarGeral.Click += new System.EventHandler(this.btnExportarGeral_Click);
      // 
      // pnlFichaIndividual
      // 
      this.pnlFichaIndividual.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlFichaIndividual.BackColor = System.Drawing.Color.White;
      this.pnlFichaIndividual.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
      this.pnlFichaIndividual.BorderRadius = 12;
      this.pnlFichaIndividual.BorderThickness = 1;
      this.pnlFichaIndividual.Controls.Add(this.chipFichaStatus);
      this.pnlFichaIndividual.Controls.Add(this.chipFichaRole);
      this.pnlFichaIndividual.Controls.Add(this.chipFichaScore);
      this.pnlFichaIndividual.Controls.Add(this.lblFichaInfo4);
      this.pnlFichaIndividual.Controls.Add(this.lblFichaInfo3);
      this.pnlFichaIndividual.Controls.Add(this.lblFichaInfo2);
      this.pnlFichaIndividual.Controls.Add(this.lblFichaInfo1);
      this.pnlFichaIndividual.Controls.Add(this.lblFichaNome);
      this.pnlFichaIndividual.Controls.Add(this.picFichaAvatar);
      this.pnlFichaIndividual.Location = new System.Drawing.Point(0, 95);
      this.pnlFichaIndividual.Name = "pnlFichaIndividual";
      this.pnlFichaIndividual.Size = new System.Drawing.Size(990, 150);
      this.pnlFichaIndividual.TabIndex = 7;
      this.pnlFichaIndividual.Visible = false;
      // 
      // chipFichaStatus
      // 
      this.chipFichaStatus.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(250)))), ((int)(((byte)(229)))));
      this.chipFichaStatus.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
      this.chipFichaStatus.ForeColor = System.Drawing.Color.FromArgb(5, 150, 105);
      this.chipFichaStatus.Location = new System.Drawing.Point(340, 15);
      this.chipFichaStatus.Name = "chipFichaStatus";
      this.chipFichaStatus.Size = new System.Drawing.Size(85, 24);
      this.chipFichaStatus.TabIndex = 8;
      this.chipFichaStatus.Text = "ATIVO";
      // 
      // chipFichaRole
      // 
      this.chipFichaRole.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
      this.chipFichaRole.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
      this.chipFichaRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.chipFichaRole.Location = new System.Drawing.Point(235, 15);
      this.chipFichaRole.Name = "chipFichaRole";
      this.chipFichaRole.Size = new System.Drawing.Size(95, 24);
      this.chipFichaRole.TabIndex = 7;
      this.chipFichaRole.Text = "ROLE";
      // 
      // chipFichaScore
      // 
      this.chipFichaScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.chipFichaScore.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(243)))), ((int)(((byte)(199)))));
      this.chipFichaScore.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.chipFichaScore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(83)))), ((int)(((byte)(9)))));
      this.chipFichaScore.Location = new System.Drawing.Point(790, 15);
      this.chipFichaScore.Name = "chipFichaScore";
      this.chipFichaScore.Size = new System.Drawing.Size(185, 30);
      this.chipFichaScore.TabIndex = 6;
      this.chipFichaScore.Text = "â­ Score: 0 pts";
      // 
      // lblFichaInfo4
      // 
      this.lblFichaInfo4.AutoSize = true;
      this.lblFichaInfo4.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.lblFichaInfo4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.lblFichaInfo4.Location = new System.Drawing.Point(520, 95);
      this.lblFichaInfo4.Name = "lblFichaInfo4";
      this.lblFichaInfo4.Size = new System.Drawing.Size(130, 17);
      this.lblFichaInfo4.TabIndex = 5;
      this.lblFichaInfo4.Text = "Comunidades: Nenhuma";
      // 
      // lblFichaInfo3
      // 
      this.lblFichaInfo3.AutoSize = true;
      this.lblFichaInfo3.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.lblFichaInfo3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.lblFichaInfo3.Location = new System.Drawing.Point(520, 60);
      this.lblFichaInfo3.Name = "lblFichaInfo3";
      this.lblFichaInfo3.Size = new System.Drawing.Size(180, 17);
      this.lblFichaInfo3.TabIndex = 4;
      this.lblFichaInfo3.Text = "Engajamento: 0 Posts | 0 Comentários";
      // 
      // lblFichaInfo2
      // 
      this.lblFichaInfo2.AutoSize = true;
      this.lblFichaInfo2.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.lblFichaInfo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.lblFichaInfo2.Location = new System.Drawing.Point(100, 95);
      this.lblFichaInfo2.Name = "lblFichaInfo2";
      this.lblFichaInfo2.Size = new System.Drawing.Size(255, 17);
      this.lblFichaInfo2.TabIndex = 3;
      this.lblFichaInfo2.Text = "Eventos: 0 Criados | 0 Inscritos | 0 Check-ins (0%)";
      // 
      // lblFichaInfo1
      // 
      this.lblFichaInfo1.AutoSize = true;
      this.lblFichaInfo1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.lblFichaInfo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
      this.lblFichaInfo1.Location = new System.Drawing.Point(100, 60);
      this.lblFichaInfo1.Name = "lblFichaInfo1";
      this.lblFichaInfo1.Size = new System.Drawing.Size(215, 17);
      this.lblFichaInfo1.TabIndex = 2;
      this.lblFichaInfo1.Text = "E-mail: user@socialjoin.com | Cadastro: --";
      // 
      // lblFichaNome
      // 
      this.lblFichaNome.AutoSize = true;
      this.lblFichaNome.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
      this.lblFichaNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.lblFichaNome.Location = new System.Drawing.Point(100, 15);
      this.lblFichaNome.Name = "lblFichaNome";
      this.lblFichaNome.Size = new System.Drawing.Size(124, 21);
      this.lblFichaNome.TabIndex = 1;
      this.lblFichaNome.Text = "Nome / Usuário";
      // 
      // picFichaAvatar
      // 
      this.picFichaAvatar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
      this.picFichaAvatar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.picFichaAvatar.IconChar = FontAwesome.Sharp.IconChar.UserCircle;
      this.picFichaAvatar.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.picFichaAvatar.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.picFichaAvatar.IconSize = 55;
      this.picFichaAvatar.Location = new System.Drawing.Point(20, 20);
      this.picFichaAvatar.Name = "picFichaAvatar";
      this.picFichaAvatar.Size = new System.Drawing.Size(65, 65);
      this.picFichaAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.picFichaAvatar.TabIndex = 0;
      this.picFichaAvatar.TabStop = false;
      // 
      // dgvRelatorioComum
      // 
      this.dgvRelatorioComum.AllowUserToAddRows = false;
      this.dgvRelatorioComum.AllowUserToDeleteRows = false;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
      this.dgvRelatorioComum.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      this.dgvRelatorioComum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
      | System.Windows.Forms.AnchorStyles.Left) 
      | System.Windows.Forms.AnchorStyles.Right)));
      this.dgvRelatorioComum.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      this.dgvRelatorioComum.BackgroundColor = System.Drawing.Color.White;
      this.dgvRelatorioComum.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.dgvRelatorioComum.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
      this.dgvRelatorioComum.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dgvRelatorioComum.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.dgvRelatorioComum.ColumnHeadersHeight = 36;
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
      dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dgvRelatorioComum.DefaultCellStyle = dataGridViewCellStyle3;
      this.dgvRelatorioComum.EnableHeadersVisualStyles = false;
      this.dgvRelatorioComum.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.dgvRelatorioComum.Location = new System.Drawing.Point(0, 100);
      this.dgvRelatorioComum.Name = "dgvRelatorioComum";
      this.dgvRelatorioComum.ReadOnly = true;
      this.dgvRelatorioComum.RowHeadersVisible = false;
      this.dgvRelatorioComum.RowTemplate.Height = 32;
      this.dgvRelatorioComum.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
      this.dgvRelatorioComum.Size = new System.Drawing.Size(990, 410);
      this.dgvRelatorioComum.TabIndex = 6;
      this.dgvRelatorioComum.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
      this.dgvRelatorioComum.ThemeStyle.BackColor = System.Drawing.Color.White;
      this.dgvRelatorioComum.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.dgvRelatorioComum.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.dgvRelatorioComum.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
      this.dgvRelatorioComum.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.dgvRelatorioComum.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
      this.dgvRelatorioComum.ThemeStyle.ReadOnly = true;
      this.dgvRelatorioComum.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
      this.dgvRelatorioComum.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
      this.dgvRelatorioComum.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.dgvRelatorioComum.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.dgvRelatorioComum.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
      this.dgvRelatorioComum.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      // 
      // btnExportarIndividual
      // 
      this.btnExportarIndividual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportarIndividual.BorderRadius = 8;
      this.btnExportarIndividual.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnExportarIndividual.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
      this.btnExportarIndividual.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.btnExportarIndividual.ForeColor = System.Drawing.Color.White;
      this.btnExportarIndividual.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(150)))), ((int)(((byte)(105)))));
      this.btnExportarIndividual.Location = new System.Drawing.Point(680, 50);
      this.btnExportarIndividual.Name = "btnExportarIndividual";
      this.btnExportarIndividual.Size = new System.Drawing.Size(170, 38);
      this.btnExportarIndividual.TabIndex = 5;
      this.btnExportarIndividual.Text = " Exportar Individual";
      this.btnExportarIndividual.Click += new System.EventHandler(this.btnExportarIndividual_Click);
      // 
      // btnVerFichaIndividual
      // 
      this.btnVerFichaIndividual.BorderRadius = 8;
      this.btnVerFichaIndividual.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnVerFichaIndividual.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.btnVerFichaIndividual.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.btnVerFichaIndividual.ForeColor = System.Drawing.Color.White;
      this.btnVerFichaIndividual.Location = new System.Drawing.Point(375, 50);
      this.btnVerFichaIndividual.Name = "btnVerFichaIndividual";
      this.btnVerFichaIndividual.Size = new System.Drawing.Size(160, 38);
      this.btnVerFichaIndividual.TabIndex = 4;
      this.btnVerFichaIndividual.Text = " Ver Ficha Individual";
      this.btnVerFichaIndividual.Click += new System.EventHandler(this.btnVerFichaIndividual_Click);
      // 
      // cmbSelecaoItem
      // 
      this.cmbSelecaoItem.BackColor = System.Drawing.Color.Transparent;
      this.cmbSelecaoItem.BorderRadius = 8;
      this.cmbSelecaoItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.cmbSelecaoItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbSelecaoItem.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.cmbSelecaoItem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.cmbSelecaoItem.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.cmbSelecaoItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.cmbSelecaoItem.ItemHeight = 30;
      this.cmbSelecaoItem.Location = new System.Drawing.Point(0, 50);
      this.cmbSelecaoItem.Name = "cmbSelecaoItem";
      this.cmbSelecaoItem.Size = new System.Drawing.Size(360, 36);
      this.cmbSelecaoItem.TabIndex = 3;
      this.cmbSelecaoItem.SelectedIndexChanged += new System.EventHandler(this.cmbSelecaoItem_SelectedIndexChanged);
      // 
      // btnSubComunidades
      // 
      this.btnSubComunidades.BorderRadius = 6;
      this.btnSubComunidades.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnSubComunidades.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.btnSubComunidades.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.btnSubComunidades.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.btnSubComunidades.Location = new System.Drawing.Point(260, 5);
      this.btnSubComunidades.Name = "btnSubComunidades";
      this.btnSubComunidades.Size = new System.Drawing.Size(130, 35);
      this.btnSubComunidades.TabIndex = 2;
      this.btnSubComunidades.Text = " Comunidades";
      this.btnSubComunidades.Click += new System.EventHandler(this.btnSubComunidades_Click);
      // 
      // btnSubEventos
      // 
      this.btnSubEventos.BorderRadius = 6;
      this.btnSubEventos.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnSubEventos.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.btnSubEventos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.btnSubEventos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.btnSubEventos.Location = new System.Drawing.Point(135, 5);
      this.btnSubEventos.Name = "btnSubEventos";
      this.btnSubEventos.Size = new System.Drawing.Size(115, 35);
      this.btnSubEventos.TabIndex = 1;
      this.btnSubEventos.Text = " Eventos";
      this.btnSubEventos.Click += new System.EventHandler(this.btnSubEventos_Click);
      // 
      // btnSubUsuarios
      // 
      this.btnSubUsuarios.BorderRadius = 6;
      this.btnSubUsuarios.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnSubUsuarios.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.btnSubUsuarios.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.btnSubUsuarios.ForeColor = System.Drawing.Color.White;
      this.btnSubUsuarios.Location = new System.Drawing.Point(0, 5);
      this.btnSubUsuarios.Name = "btnSubUsuarios";
      this.btnSubUsuarios.Size = new System.Drawing.Size(125, 35);
      this.btnSubUsuarios.TabIndex = 0;
      this.btnSubUsuarios.Text = " Usuários";
      this.btnSubUsuarios.Click += new System.EventHandler(this.btnSubUsuarios_Click);
      // 
      // pnlAnalyticsApp
      // 
      this.pnlAnalyticsApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
      | System.Windows.Forms.AnchorStyles.Left) 
      | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlAnalyticsApp.Controls.Add(this.btnExportarAnalyticsCsv);
      this.pnlAnalyticsApp.Controls.Add(this.pnlKpisEngajamento);
      this.pnlAnalyticsApp.Controls.Add(this.chartBarComunidades);
      this.pnlAnalyticsApp.Controls.Add(this.chartBarEventos);
      this.pnlAnalyticsApp.Controls.Add(this.chartDonutRoles);
      this.pnlAnalyticsApp.Location = new System.Drawing.Point(25, 118);
      this.pnlAnalyticsApp.Name = "pnlAnalyticsApp";
      this.pnlAnalyticsApp.Size = new System.Drawing.Size(990, 515);
      this.pnlAnalyticsApp.TabIndex = 4;
      this.pnlAnalyticsApp.Visible = false;
      // 
      // btnExportarAnalyticsCsv
      // 
      this.btnExportarAnalyticsCsv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportarAnalyticsCsv.BorderRadius = 8;
      this.btnExportarAnalyticsCsv.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnExportarAnalyticsCsv.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
      this.btnExportarAnalyticsCsv.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.btnExportarAnalyticsCsv.ForeColor = System.Drawing.Color.White;
      this.btnExportarAnalyticsCsv.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(150)))), ((int)(((byte)(105)))));
      this.btnExportarAnalyticsCsv.Location = new System.Drawing.Point(770, 5);
      this.btnExportarAnalyticsCsv.Name = "btnExportarAnalyticsCsv";
      this.btnExportarAnalyticsCsv.Size = new System.Drawing.Size(220, 38);
      this.btnExportarAnalyticsCsv.TabIndex = 4;
      this.btnExportarAnalyticsCsv.Text = " Exportar Analytics (CSV)";
      this.btnExportarAnalyticsCsv.Click += new System.EventHandler(this.btnExportarAnalyticsCsv_Click);
      // 
      // pnlKpisEngajamento
      // 
      this.pnlKpisEngajamento.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlKpisEngajamento.BackColor = System.Drawing.Color.White;
      this.pnlKpisEngajamento.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
      this.pnlKpisEngajamento.BorderRadius = 12;
      this.pnlKpisEngajamento.BorderThickness = 1;
      this.pnlKpisEngajamento.Controls.Add(this.lblKpiEngajamentoSub);
      this.pnlKpisEngajamento.Controls.Add(this.lblKpiEngajamentoTitulo);
      this.pnlKpisEngajamento.Location = new System.Drawing.Point(0, 0);
      this.pnlKpisEngajamento.Name = "pnlKpisEngajamento";
      this.pnlKpisEngajamento.Size = new System.Drawing.Size(750, 50);
      this.pnlKpisEngajamento.TabIndex = 3;
      // 
      // lblKpiEngajamentoSub
      // 
      this.lblKpiEngajamentoSub.AutoSize = true;
      this.lblKpiEngajamentoSub.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.lblKpiEngajamentoSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.lblKpiEngajamentoSub.Location = new System.Drawing.Point(15, 27);
      this.lblKpiEngajamentoSub.Name = "lblKpiEngajamentoSub";
      this.lblKpiEngajamentoSub.Size = new System.Drawing.Size(465, 15);
      this.lblKpiEngajamentoSub.TabIndex = 1;
      this.lblKpiEngajamentoSub.Text = "Indicadores gerais: Engajamento por usuário, taxa de presença em eventos e interações.";
      // 
      // lblKpiEngajamentoTitulo
      // 
      this.lblKpiEngajamentoTitulo.AutoSize = true;
      this.lblKpiEngajamentoTitulo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
      this.lblKpiEngajamentoTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.lblKpiEngajamentoTitulo.Location = new System.Drawing.Point(15, 7);
      this.lblKpiEngajamentoTitulo.Name = "lblKpiEngajamentoTitulo";
      this.lblKpiEngajamentoTitulo.Size = new System.Drawing.Size(325, 19);
      this.lblKpiEngajamentoTitulo.TabIndex = 0;
      this.lblKpiEngajamentoTitulo.Text = " Painel Visual de Performance da Plataforma";
      // 
      // chartBarComunidades
      // 
      this.chartBarComunidades.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
      | System.Windows.Forms.AnchorStyles.Right)));
      this.chartBarComunidades.BackColor = System.Drawing.Color.White;
      this.chartBarComunidades.ColorValue1 = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(63)))), ((int)(((byte)(116)))));
      this.chartBarComunidades.ColorValue2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(158)))), ((int)(((byte)(11)))));
      this.chartBarComunidades.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.chartBarComunidades.LabelValue1 = "Membros";
      this.chartBarComunidades.LabelValue2 = "Posts";
      this.chartBarComunidades.Location = new System.Drawing.Point(505, 290);
      this.chartBarComunidades.Name = "chartBarComunidades";
      this.chartBarComunidades.Size = new System.Drawing.Size(485, 220);
      this.chartBarComunidades.TabIndex = 2;
      // 
      // chartBarEventos
      // 
      this.chartBarEventos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
      | System.Windows.Forms.AnchorStyles.Right)));
      this.chartBarEventos.BackColor = System.Drawing.Color.White;
      this.chartBarEventos.ColorValue1 = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.chartBarEventos.ColorValue2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
      this.chartBarEventos.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.chartBarEventos.LabelValue1 = "Inscritos";
      this.chartBarEventos.LabelValue2 = "Capacidade";
      this.chartBarEventos.Location = new System.Drawing.Point(505, 60);
      this.chartBarEventos.Name = "chartBarEventos";
      this.chartBarEventos.Size = new System.Drawing.Size(485, 215);
      this.chartBarEventos.TabIndex = 1;
      // 
      // chartDonutRoles
      // 
      this.chartDonutRoles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
      | System.Windows.Forms.AnchorStyles.Left)));
      this.chartDonutRoles.BackColor = System.Drawing.Color.White;
      this.chartDonutRoles.CenterSubtitle = "Roles";
      this.chartDonutRoles.CenterTitle = "Total";
      this.chartDonutRoles.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.chartDonutRoles.Location = new System.Drawing.Point(0, 60);
      this.chartDonutRoles.Name = "chartDonutRoles";
      this.chartDonutRoles.Size = new System.Drawing.Size(490, 450);
      this.chartDonutRoles.TabIndex = 0;
      // 
      // FrmRelatorios
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
      this.ClientSize = new System.Drawing.Size(1040, 645);
      this.Controls.Add(this.pnlRelatoriosComuns);
      this.Controls.Add(this.pnlAnalyticsApp);
      this.Controls.Add(this.pnlHeaderTabs);
      this.Controls.Add(this.lblSubInfo);
      this.Controls.Add(this.lblHeaderTitle);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "FrmRelatorios";
      this.Text = "Relatórios e Analytics";
      this.Load += new System.EventHandler(this.FrmRelatorios_Load);
      this.pnlHeaderTabs.ResumeLayout(false);
      this.pnlRelatoriosComuns.ResumeLayout(false);
      this.pnlFichaIndividual.ResumeLayout(false);
      this.pnlFichaIndividual.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picFichaAvatar)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvRelatorioComum)).EndInit();
      this.pnlAnalyticsApp.ResumeLayout(false);
      this.pnlKpisEngajamento.ResumeLayout(false);
      this.pnlKpisEngajamento.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
