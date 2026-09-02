namespace RedeSocialEventosAdmin.Forms
{
  partial class FrmEventos
  {
    private System.ComponentModel.IContainer components = null;
    private Guna.UI2.WinForms.Guna2TextBox txtPesquisa;
    private Guna.UI2.WinForms.Guna2Button btnNovo;
    private Guna.UI2.WinForms.Guna2Button btnEditar;
    private Guna.UI2.WinForms.Guna2Button btnStatusToggle;
    private Guna.UI2.WinForms.Guna2Button btnParticipantes;
    private Guna.UI2.WinForms.Guna2Button btnExcluir;
    private Guna.UI2.WinForms.Guna2Button btnRefresh;
    private Guna.UI2.WinForms.Guna2ComboBox cmbFiltroStatus;
    private Guna.UI2.WinForms.Guna2DataGridView dgvEventos;
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
            btnParticipantes = new Guna.UI2.WinForms.Guna2Button();
            btnExcluir = new Guna.UI2.WinForms.Guna2Button();
            btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            cmbFiltroStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            dgvEventos = new Guna.UI2.WinForms.Guna2DataGridView();
            lblHeaderTitle = new Label();
            lblTotalRegistros = new Label();
            pnlFiltros = new Guna.UI2.WinForms.Guna2Panel();
            ((System.ComponentModel.ISupportInitialize)dgvEventos).BeginInit();
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
            txtPesquisa.PlaceholderText = " Buscar por título, local ou categoria...";
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
            btnNovo.Location = new Point(513, 6);
            btnNovo.Margin = new Padding(4, 3, 4, 3);
            btnNovo.Name = "btnNovo";
            btnNovo.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnNovo.Size = new Size(122, 44);
            btnNovo.TabIndex = 2;
            btnNovo.Text = "+ Criar Evento";
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
            btnEditar.Location = new Point(642, 6);
            btnEditar.Margin = new Padding(4, 3, 4, 3);
            btnEditar.Name = "btnEditar";
            btnEditar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnEditar.Size = new Size(99, 44);
            btnEditar.TabIndex = 3;
            btnEditar.Text = "Editar";
            btnEditar.Click += btnEditar_Click;
            // 
            // btnStatusToggle
            // 
            btnStatusToggle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStatusToggle.BorderRadius = 8;
            btnStatusToggle.Cursor = Cursors.Hand;
            btnStatusToggle.CustomizableEdges = customizableEdges7;
            btnStatusToggle.FillColor = Color.FromArgb(238, 242, 255);
            btnStatusToggle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnStatusToggle.ForeColor = Color.FromArgb(79, 70, 229);
            btnStatusToggle.HoverState.FillColor = Color.FromArgb(224, 231, 255);
            btnStatusToggle.Location = new Point(747, 6);
            btnStatusToggle.Margin = new Padding(4, 3, 4, 3);
            btnStatusToggle.Name = "btnStatusToggle";
            btnStatusToggle.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnStatusToggle.Size = new Size(134, 44);
            btnStatusToggle.TabIndex = 4;
            btnStatusToggle.Text = "Mudar Status";
            btnStatusToggle.Click += btnStatusToggle_Click;
            // 
            // btnParticipantes
            // 
            btnParticipantes.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnParticipantes.BorderRadius = 8;
            btnParticipantes.Cursor = Cursors.Hand;
            btnParticipantes.CustomizableEdges = customizableEdges9;
            btnParticipantes.FillColor = Color.FromArgb(241, 245, 249);
            btnParticipantes.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnParticipantes.ForeColor = Color.FromArgb(51, 65, 85);
            btnParticipantes.HoverState.FillColor = Color.FromArgb(226, 232, 240);
            btnParticipantes.Location = new Point(887, 6);
            btnParticipantes.Margin = new Padding(4, 3, 4, 3);
            btnParticipantes.Name = "btnParticipantes";
            btnParticipantes.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnParticipantes.Size = new Size(134, 44);
            btnParticipantes.TabIndex = 5;
            btnParticipantes.Text = " Ver Inscritos";
            btnParticipantes.Click += btnParticipantes_Click;
            // 
            // btnExcluir
            // 
            btnExcluir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExcluir.BorderRadius = 8;
            btnExcluir.Cursor = Cursors.Hand;
            btnExcluir.CustomizableEdges = customizableEdges11;
            btnExcluir.FillColor = Color.FromArgb(254, 226, 226);
            btnExcluir.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExcluir.ForeColor = Color.FromArgb(220, 38, 38);
            btnExcluir.HoverState.FillColor = Color.FromArgb(254, 202, 202);
            btnExcluir.Location = new Point(1027, 6);
            btnExcluir.Margin = new Padding(4, 3, 4, 3);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.ShadowDecoration.CustomizableEdges = customizableEdges12;
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
            btnRefresh.CustomizableEdges = customizableEdges13;
            btnRefresh.FillColor = Color.FromArgb(241, 245, 249);
            btnRefresh.Font = new Font("Segoe UI", 10F);
            btnRefresh.ForeColor = Color.FromArgb(51, 65, 85);
            btnRefresh.HoverState.FillColor = Color.FromArgb(226, 232, 240);
            btnRefresh.Location = new Point(1114, 6);
            btnRefresh.Margin = new Padding(4, 3, 4, 3);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.ShadowDecoration.CustomizableEdges = customizableEdges14;
            btnRefresh.Size = new Size(41, 44);
            btnRefresh.TabIndex = 7;
            btnRefresh.Text = "🔄";
            btnRefresh.Click += btnRefresh_Click;
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
            cmbFiltroStatus.Items.AddRange(new object[] { "Status: TODOS", "AGENDADO", "ACONTECENDO_AGORA", "ENCERRADO", "CANCELADO" });
            cmbFiltroStatus.Location = new Point(315, 6);
            cmbFiltroStatus.Margin = new Padding(4, 3, 4, 3);
            cmbFiltroStatus.Name = "cmbFiltroStatus";
            cmbFiltroStatus.ShadowDecoration.CustomizableEdges = customizableEdges16;
            cmbFiltroStatus.Size = new Size(186, 38);
            cmbFiltroStatus.StartIndex = 0;
            cmbFiltroStatus.TabIndex = 1;
            cmbFiltroStatus.SelectedIndexChanged += Filtros_Changed;
            // 
            // dgvEventos
            // 
            dgvEventos.AllowUserToAddRows = false;
            dgvEventos.AllowUserToDeleteRows = false;
            dgvEventos.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dgvEventos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvEventos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvEventos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvEventos.ColumnHeadersHeight = 42;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(79, 70, 229);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvEventos.DefaultCellStyle = dataGridViewCellStyle3;
            dgvEventos.GridColor = Color.FromArgb(241, 245, 249);
            dgvEventos.Location = new Point(29, 162);
            dgvEventos.Margin = new Padding(4, 3, 4, 3);
            dgvEventos.MultiSelect = false;
            dgvEventos.Name = "dgvEventos";
            dgvEventos.ReadOnly = true;
            dgvEventos.RowHeadersVisible = false;
            dgvEventos.RowTemplate.Height = 36;
            dgvEventos.Size = new Size(1155, 542);
            dgvEventos.TabIndex = 3;
            dgvEventos.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvEventos.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvEventos.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvEventos.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvEventos.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvEventos.ThemeStyle.BackColor = Color.White;
            dgvEventos.ThemeStyle.GridColor = Color.FromArgb(241, 245, 249);
            dgvEventos.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvEventos.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvEventos.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvEventos.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvEventos.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvEventos.ThemeStyle.HeaderStyle.Height = 42;
            dgvEventos.ThemeStyle.ReadOnly = true;
            dgvEventos.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvEventos.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvEventos.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9.5F);
            dgvEventos.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(30, 41, 59);
            dgvEventos.ThemeStyle.RowsStyle.Height = 36;
            dgvEventos.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dgvEventos.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(79, 70, 229);
            dgvEventos.DoubleClick += btnEditar_Click;
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblHeaderTitle.Location = new Point(29, 23);
            lblHeaderTitle.Margin = new Padding(4, 0, 4, 0);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(290, 30);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "Gerenciamento de Eventos";
            // 
            // lblTotalRegistros
            // 
            lblTotalRegistros.AutoSize = true;
            lblTotalRegistros.Font = new Font("Segoe UI", 9.5F);
            lblTotalRegistros.ForeColor = Color.FromArgb(100, 116, 139);
            lblTotalRegistros.Location = new Point(31, 60);
            lblTotalRegistros.Margin = new Padding(4, 0, 4, 0);
            lblTotalRegistros.Name = "lblTotalRegistros";
            lblTotalRegistros.Size = new Size(185, 17);
            lblTotalRegistros.TabIndex = 1;
            lblTotalRegistros.Text = "Carregando total de eventos...";
            // 
            // pnlFiltros
            // 
            pnlFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlFiltros.BackColor = Color.Transparent;
            pnlFiltros.Controls.Add(btnRefresh);
            pnlFiltros.Controls.Add(btnExcluir);
            pnlFiltros.Controls.Add(btnParticipantes);
            pnlFiltros.Controls.Add(btnStatusToggle);
            pnlFiltros.Controls.Add(btnEditar);
            pnlFiltros.Controls.Add(btnNovo);
            pnlFiltros.Controls.Add(cmbFiltroStatus);
            pnlFiltros.Controls.Add(txtPesquisa);
            pnlFiltros.CustomizableEdges = customizableEdges17;
            pnlFiltros.Location = new Point(29, 92);
            pnlFiltros.Margin = new Padding(4, 3, 4, 3);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.ShadowDecoration.CustomizableEdges = customizableEdges18;
            pnlFiltros.Size = new Size(1155, 58);
            pnlFiltros.TabIndex = 2;
            // 
            // FrmEventos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            ClientSize = new Size(1213, 738);
            Controls.Add(dgvEventos);
            Controls.Add(pnlFiltros);
            Controls.Add(lblTotalRegistros);
            Controls.Add(lblHeaderTitle);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "FrmEventos";
            Text = "Gerenciamento de Eventos";
            Load += FrmEventos_Load;
            ((System.ComponentModel.ISupportInitialize)dgvEventos).EndInit();
            pnlFiltros.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
