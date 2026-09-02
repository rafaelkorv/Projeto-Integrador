namespace RedeSocialEventosAdmin.Forms
{
  partial class FrmUsuarios
  {
    private System.ComponentModel.IContainer components = null;
    private Guna.UI2.WinForms.Guna2TextBox txtPesquisa;
    private Guna.UI2.WinForms.Guna2Button btnNovo;
    private Guna.UI2.WinForms.Guna2Button btnEditar;
    private Guna.UI2.WinForms.Guna2Button btnStatusToggle;
    private Guna.UI2.WinForms.Guna2Button btnExcluir;
    private Guna.UI2.WinForms.Guna2Button btnRefresh;
    private Guna.UI2.WinForms.Guna2ComboBox cmbFiltroRole;
    private Guna.UI2.WinForms.Guna2ComboBox cmbFiltroStatus;
    private Guna.UI2.WinForms.Guna2DataGridView dgvUsuarios;
    private System.Windows.Forms.Label lblHeaderTitle;
    private System.Windows.Forms.Label lblTotalRegistros;
    private Guna.UI2.WinForms.Guna2Panel pnlFiltros;

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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            txtPesquisa = new Guna.UI2.WinForms.Guna2TextBox();
            btnNovo = new Guna.UI2.WinForms.Guna2Button();
            btnEditar = new Guna.UI2.WinForms.Guna2Button();
            btnStatusToggle = new Guna.UI2.WinForms.Guna2Button();
            btnExcluir = new Guna.UI2.WinForms.Guna2Button();
            btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            cmbFiltroRole = new Guna.UI2.WinForms.Guna2ComboBox();
            cmbFiltroStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            dgvUsuarios = new Guna.UI2.WinForms.Guna2DataGridView();
            lblHeaderTitle = new Label();
            lblTotalRegistros = new Label();
            pnlFiltros = new Guna.UI2.WinForms.Guna2Panel();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
            pnlFiltros.SuspendLayout();
            SuspendLayout();
            // 
            // txtPesquisa
            // 
            txtPesquisa.BorderColor = Color.FromArgb(226, 232, 240);
            txtPesquisa.BorderRadius = 8;
            txtPesquisa.Cursor = Cursors.IBeam;
            txtPesquisa.CustomizableEdges = customizableEdges1;
            txtPesquisa.DefaultText = "";
            txtPesquisa.FocusedState.BorderColor = Color.FromArgb(99, 102, 241);
            txtPesquisa.Font = new Font("Segoe UI", 9.5F);
            txtPesquisa.ForeColor = Color.FromArgb(30, 41, 59);
            txtPesquisa.HoverState.BorderColor = Color.FromArgb(79, 70, 229);
            txtPesquisa.Location = new Point(0, 6);
            txtPesquisa.Margin = new Padding(4, 5, 4, 5);
            txtPesquisa.Name = "txtPesquisa";
            txtPesquisa.PasswordChar = '\0';
            txtPesquisa.PlaceholderText = " Buscar por nome, email ou username...";
            txtPesquisa.SelectedText = "";
            txtPesquisa.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtPesquisa.Size = new Size(303, 44);
            txtPesquisa.TabIndex = 0;
            txtPesquisa.TextChanged += txtPesquisa_TextChanged;
            // 
            // btnNovo
            // 
            btnNovo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNovo.BorderRadius = 8;
            btnNovo.Cursor = Cursors.Hand;
            btnNovo.CustomizableEdges = customizableEdges3;
            btnNovo.FillColor = Color.FromArgb(79, 70, 229);
            btnNovo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnNovo.ForeColor = Color.White;
            btnNovo.HoverState.FillColor = Color.FromArgb(67, 56, 202);
            btnNovo.Location = new Point(648, 6);
            btnNovo.Margin = new Padding(4, 3, 4, 3);
            btnNovo.Name = "btnNovo";
            btnNovo.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnNovo.Size = new Size(117, 44);
            btnNovo.TabIndex = 3;
            btnNovo.Text = "+ Novo";
            btnNovo.Click += btnNovo_Click;
            // 
            // btnEditar
            // 
            btnEditar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditar.BorderRadius = 8;
            btnEditar.Cursor = Cursors.Hand;
            btnEditar.CustomizableEdges = customizableEdges5;
            btnEditar.FillColor = Color.FromArgb(241, 245, 249);
            btnEditar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnEditar.ForeColor = Color.FromArgb(51, 65, 85);
            btnEditar.HoverState.FillColor = Color.FromArgb(226, 232, 240);
            btnEditar.Location = new Point(776, 6);
            btnEditar.Margin = new Padding(4, 3, 4, 3);
            btnEditar.Name = "btnEditar";
            btnEditar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnEditar.Size = new Size(111, 44);
            btnEditar.TabIndex = 4;
            btnEditar.Text = "Editar";
            btnEditar.Click += btnEditar_Click;
            // 
            // btnStatusToggle
            // 
            btnStatusToggle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStatusToggle.BorderRadius = 8;
            btnStatusToggle.Cursor = Cursors.Hand;
            btnStatusToggle.CustomizableEdges = customizableEdges7;
            btnStatusToggle.FillColor = Color.FromArgb(254, 243, 199);
            btnStatusToggle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnStatusToggle.ForeColor = Color.FromArgb(180, 83, 9);
            btnStatusToggle.HoverState.FillColor = Color.FromArgb(253, 230, 138);
            btnStatusToggle.Location = new Point(896, 6);
            btnStatusToggle.Margin = new Padding(4, 3, 4, 3);
            btnStatusToggle.Name = "btnStatusToggle";
            btnStatusToggle.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnStatusToggle.Size = new Size(122, 44);
            btnStatusToggle.TabIndex = 5;
            btnStatusToggle.Text = " Suspender";
            btnStatusToggle.Click += btnStatusToggle_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExcluir.BorderRadius = 8;
            btnExcluir.Cursor = Cursors.Hand;
            btnExcluir.CustomizableEdges = customizableEdges9;
            btnExcluir.FillColor = Color.FromArgb(254, 226, 226);
            btnExcluir.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExcluir.ForeColor = Color.FromArgb(220, 38, 38);
            btnExcluir.HoverState.FillColor = Color.FromArgb(254, 202, 202);
            btnExcluir.Location = new Point(1027, 6);
            btnExcluir.Margin = new Padding(4, 3, 4, 3);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnExcluir.Size = new Size(82, 44);
            btnExcluir.TabIndex = 6;
            btnExcluir.Text = "Excluir";
            btnExcluir.Click += btnExcluir_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BorderRadius = 8;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.CustomizableEdges = customizableEdges11;
            btnRefresh.FillColor = Color.FromArgb(241, 245, 249);
            btnRefresh.Font = new Font("Segoe UI", 10F);
            btnRefresh.ForeColor = Color.FromArgb(51, 65, 85);
            btnRefresh.HoverState.FillColor = Color.FromArgb(226, 232, 240);
            btnRefresh.Location = new Point(1114, 6);
            btnRefresh.Margin = new Padding(4, 3, 4, 3);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btnRefresh.Size = new Size(41, 44);
            btnRefresh.TabIndex = 7;
            btnRefresh.Text = "🔄";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // cmbFiltroRole
            // 
            cmbFiltroRole.BackColor = Color.Transparent;
            cmbFiltroRole.BorderColor = Color.FromArgb(226, 232, 240);
            cmbFiltroRole.BorderRadius = 8;
            cmbFiltroRole.CustomizableEdges = customizableEdges13;
            cmbFiltroRole.DrawMode = DrawMode.OwnerDrawFixed;
            cmbFiltroRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroRole.FocusedColor = Color.FromArgb(99, 102, 241);
            cmbFiltroRole.FocusedState.BorderColor = Color.FromArgb(99, 102, 241);
            cmbFiltroRole.Font = new Font("Segoe UI", 9F);
            cmbFiltroRole.ForeColor = Color.FromArgb(30, 41, 59);
            cmbFiltroRole.ItemHeight = 32;
            cmbFiltroRole.Items.AddRange(new object[] { "Role: TODAS", "admin", "moderator", "premium", "tester", "betatester", "user" });
            cmbFiltroRole.Location = new Point(315, 6);
            cmbFiltroRole.Margin = new Padding(4, 3, 4, 3);
            cmbFiltroRole.Name = "cmbFiltroRole";
            cmbFiltroRole.ShadowDecoration.CustomizableEdges = customizableEdges14;
            cmbFiltroRole.Size = new Size(151, 38);
            cmbFiltroRole.StartIndex = 0;
            cmbFiltroRole.TabIndex = 1;
            cmbFiltroRole.SelectedIndexChanged += Filtros_Changed;
            // 
            // cmbFiltroStatus
            // 
            cmbFiltroStatus.BackColor = Color.Transparent;
            cmbFiltroStatus.BorderColor = Color.FromArgb(226, 232, 240);
            cmbFiltroStatus.BorderRadius = 8;
            cmbFiltroStatus.CustomizableEdges = customizableEdges15;
            cmbFiltroStatus.DrawMode = DrawMode.OwnerDrawFixed;
            cmbFiltroStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFiltroStatus.FocusedColor = Color.FromArgb(99, 102, 241);
            cmbFiltroStatus.FocusedState.BorderColor = Color.FromArgb(99, 102, 241);
            cmbFiltroStatus.Font = new Font("Segoe UI", 9F);
            cmbFiltroStatus.ForeColor = Color.FromArgb(30, 41, 59);
            cmbFiltroStatus.ItemHeight = 32;
            cmbFiltroStatus.Items.AddRange(new object[] { "Status: TODOS", "ativo", "suspenso", "inativo" });
            cmbFiltroStatus.Location = new Point(478, 6);
            cmbFiltroStatus.Margin = new Padding(4, 3, 4, 3);
            cmbFiltroStatus.Name = "cmbFiltroStatus";
            cmbFiltroStatus.ShadowDecoration.CustomizableEdges = customizableEdges16;
            cmbFiltroStatus.Size = new Size(151, 38);
            cmbFiltroStatus.StartIndex = 0;
            cmbFiltroStatus.TabIndex = 2;
            cmbFiltroStatus.SelectedIndexChanged += Filtros_Changed;
            // 
            // dgvUsuarios
            // 
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dgvUsuarios.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvUsuarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvUsuarios.ColumnHeadersHeight = 42;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(79, 70, 229);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvUsuarios.DefaultCellStyle = dataGridViewCellStyle3;
            dgvUsuarios.GridColor = Color.FromArgb(241, 245, 249);
            dgvUsuarios.Location = new Point(29, 162);
            dgvUsuarios.Margin = new Padding(4, 3, 4, 3);
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.RowTemplate.Height = 36;
            dgvUsuarios.Size = new Size(1155, 542);
            dgvUsuarios.TabIndex = 3;
            dgvUsuarios.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvUsuarios.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvUsuarios.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvUsuarios.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvUsuarios.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvUsuarios.ThemeStyle.BackColor = Color.White;
            dgvUsuarios.ThemeStyle.GridColor = Color.FromArgb(241, 245, 249);
            dgvUsuarios.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvUsuarios.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvUsuarios.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvUsuarios.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvUsuarios.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUsuarios.ThemeStyle.HeaderStyle.Height = 42;
            dgvUsuarios.ThemeStyle.ReadOnly = true;
            dgvUsuarios.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvUsuarios.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUsuarios.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9.5F);
            dgvUsuarios.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(30, 41, 59);
            dgvUsuarios.ThemeStyle.RowsStyle.Height = 36;
            dgvUsuarios.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dgvUsuarios.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(79, 70, 229);
            dgvUsuarios.DoubleClick += btnEditar_Click;
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblHeaderTitle.Location = new Point(29, 23);
            lblHeaderTitle.Margin = new Padding(4, 0, 4, 0);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(241, 30);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "Usuários e Permissões";
            // 
            // lblTotalRegistros
            // 
            lblTotalRegistros.AutoSize = true;
            lblTotalRegistros.Font = new Font("Segoe UI", 9.5F);
            lblTotalRegistros.ForeColor = Color.FromArgb(100, 116, 139);
            lblTotalRegistros.Location = new Point(31, 60);
            lblTotalRegistros.Margin = new Padding(4, 0, 4, 0);
            lblTotalRegistros.Name = "lblTotalRegistros";
            lblTotalRegistros.Size = new Size(178, 17);
            lblTotalRegistros.TabIndex = 1;
            lblTotalRegistros.Text = "Carregando total de contas...";
            // 
            // pnlFiltros
            // 
            pnlFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlFiltros.BackColor = Color.Transparent;
            pnlFiltros.Controls.Add(btnRefresh);
            pnlFiltros.Controls.Add(btnExcluir);
            pnlFiltros.Controls.Add(btnStatusToggle);
            pnlFiltros.Controls.Add(btnEditar);
            pnlFiltros.Controls.Add(btnNovo);
            pnlFiltros.Controls.Add(cmbFiltroStatus);
            pnlFiltros.Controls.Add(cmbFiltroRole);
            pnlFiltros.Controls.Add(txtPesquisa);
            pnlFiltros.CustomizableEdges = customizableEdges17;
            pnlFiltros.Location = new Point(29, 92);
            pnlFiltros.Margin = new Padding(4, 3, 4, 3);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.ShadowDecoration.CustomizableEdges = customizableEdges18;
            pnlFiltros.Size = new Size(1155, 58);
            pnlFiltros.TabIndex = 2;
            // 
            // FrmUsuarios
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            ClientSize = new Size(1213, 738);
            Controls.Add(dgvUsuarios);
            Controls.Add(pnlFiltros);
            Controls.Add(lblTotalRegistros);
            Controls.Add(lblHeaderTitle);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "FrmUsuarios";
            Text = "Gerenciamento de Usuários";
            Load += FrmUsuarios_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
            pnlFiltros.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
