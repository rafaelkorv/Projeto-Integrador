namespace RedeSocialEventosAdmin.Forms
{
  partial class FrmComunidades
  {
    private System.ComponentModel.IContainer components = null;
    private Guna.UI2.WinForms.Guna2TextBox txtPesquisa;
    private Guna.UI2.WinForms.Guna2Button btnNovo;
    private Guna.UI2.WinForms.Guna2Button btnEditar;
    private Guna.UI2.WinForms.Guna2Button btnMembros;
    private Guna.UI2.WinForms.Guna2Button btnExcluir;
    private Guna.UI2.WinForms.Guna2Button btnRefresh;
    private Guna.UI2.WinForms.Guna2DataGridView dgvComunidades;
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            txtPesquisa = new Guna.UI2.WinForms.Guna2TextBox();
            btnNovo = new Guna.UI2.WinForms.Guna2Button();
            btnEditar = new Guna.UI2.WinForms.Guna2Button();
            btnMembros = new Guna.UI2.WinForms.Guna2Button();
            btnExcluir = new Guna.UI2.WinForms.Guna2Button();
            btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            dgvComunidades = new Guna.UI2.WinForms.Guna2DataGridView();
            lblHeaderTitle = new Label();
            lblTotalRegistros = new Label();
            pnlFiltros = new Guna.UI2.WinForms.Guna2Panel();
            ((System.ComponentModel.ISupportInitialize)dgvComunidades).BeginInit();
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
            txtPesquisa.PlaceholderText = " Buscar por nome, categoria ou criador...";
            txtPesquisa.SelectedText = "";
            txtPesquisa.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtPesquisa.Size = new Size(408, 44);
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
            btnNovo.Location = new Point(572, 6);
            btnNovo.Margin = new Padding(4, 3, 4, 3);
            btnNovo.Name = "btnNovo";
            btnNovo.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnNovo.Size = new Size(163, 44);
            btnNovo.TabIndex = 1;
            btnNovo.Text = "+ Nova Comunidade";
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
            btnEditar.Location = new Point(747, 6);
            btnEditar.Margin = new Padding(4, 3, 4, 3);
            btnEditar.Name = "btnEditar";
            btnEditar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnEditar.Size = new Size(111, 44);
            btnEditar.TabIndex = 2;
            btnEditar.Text = "Editar";
            btnEditar.Click += btnEditar_Click;
            // 
            // btnMembros
            // 
            btnMembros.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMembros.BorderRadius = 8;
            btnMembros.Cursor = Cursors.Hand;
            btnMembros.CustomizableEdges = customizableEdges7;
            btnMembros.FillColor = Color.FromArgb(241, 245, 249);
            btnMembros.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnMembros.ForeColor = Color.FromArgb(51, 65, 85);
            btnMembros.HoverState.FillColor = Color.FromArgb(226, 232, 240);
            btnMembros.Location = new Point(869, 6);
            btnMembros.Margin = new Padding(4, 3, 4, 3);
            btnMembros.Name = "btnMembros";
            btnMembros.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnMembros.Size = new Size(146, 44);
            btnMembros.TabIndex = 3;
            btnMembros.Text = " Ver Membros";
            btnMembros.Click += btnMembros_Click;
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
            btnExcluir.TabIndex = 4;
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
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "🔄";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // dgvComunidades
            // 
            dgvComunidades.AllowUserToAddRows = false;
            dgvComunidades.AllowUserToDeleteRows = false;
            dgvComunidades.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dgvComunidades.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvComunidades.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvComunidades.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvComunidades.ColumnHeadersHeight = 42;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(79, 70, 229);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvComunidades.DefaultCellStyle = dataGridViewCellStyle3;
            dgvComunidades.GridColor = Color.FromArgb(241, 245, 249);
            dgvComunidades.Location = new Point(29, 162);
            dgvComunidades.Margin = new Padding(4, 3, 4, 3);
            dgvComunidades.MultiSelect = false;
            dgvComunidades.Name = "dgvComunidades";
            dgvComunidades.ReadOnly = true;
            dgvComunidades.RowHeadersVisible = false;
            dgvComunidades.RowTemplate.Height = 36;
            dgvComunidades.Size = new Size(1155, 542);
            dgvComunidades.TabIndex = 3;
            dgvComunidades.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvComunidades.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvComunidades.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvComunidades.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvComunidades.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvComunidades.ThemeStyle.BackColor = Color.White;
            dgvComunidades.ThemeStyle.GridColor = Color.FromArgb(241, 245, 249);
            dgvComunidades.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvComunidades.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvComunidades.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvComunidades.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvComunidades.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvComunidades.ThemeStyle.HeaderStyle.Height = 42;
            dgvComunidades.ThemeStyle.ReadOnly = true;
            dgvComunidades.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvComunidades.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvComunidades.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9.5F);
            dgvComunidades.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(30, 41, 59);
            dgvComunidades.ThemeStyle.RowsStyle.Height = 36;
            dgvComunidades.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dgvComunidades.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(79, 70, 229);
            dgvComunidades.DoubleClick += btnEditar_Click;
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblHeaderTitle.Location = new Point(29, 23);
            lblHeaderTitle.Margin = new Padding(4, 0, 4, 0);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(350, 30);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "Gerenciamento de Comunidades";
            // 
            // lblTotalRegistros
            // 
            lblTotalRegistros.AutoSize = true;
            lblTotalRegistros.Font = new Font("Segoe UI", 9.5F);
            lblTotalRegistros.ForeColor = Color.FromArgb(100, 116, 139);
            lblTotalRegistros.Location = new Point(31, 60);
            lblTotalRegistros.Margin = new Padding(4, 0, 4, 0);
            lblTotalRegistros.Name = "lblTotalRegistros";
            lblTotalRegistros.Size = new Size(218, 17);
            lblTotalRegistros.TabIndex = 1;
            lblTotalRegistros.Text = "Carregando total de comunidades...";
            // 
            // pnlFiltros
            // 
            pnlFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlFiltros.BackColor = Color.Transparent;
            pnlFiltros.Controls.Add(btnRefresh);
            pnlFiltros.Controls.Add(btnExcluir);
            pnlFiltros.Controls.Add(btnMembros);
            pnlFiltros.Controls.Add(btnEditar);
            pnlFiltros.Controls.Add(btnNovo);
            pnlFiltros.Controls.Add(txtPesquisa);
            pnlFiltros.CustomizableEdges = customizableEdges13;
            pnlFiltros.Location = new Point(29, 92);
            pnlFiltros.Margin = new Padding(4, 3, 4, 3);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.ShadowDecoration.CustomizableEdges = customizableEdges14;
            pnlFiltros.Size = new Size(1155, 58);
            pnlFiltros.TabIndex = 2;
            // 
            // FrmComunidades
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            ClientSize = new Size(1213, 738);
            Controls.Add(dgvComunidades);
            Controls.Add(pnlFiltros);
            Controls.Add(lblTotalRegistros);
            Controls.Add(lblHeaderTitle);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "FrmComunidades";
            Text = "Gerenciamento de Comunidades";
            Load += FrmComunidades_Load;
            ((System.ComponentModel.ISupportInitialize)dgvComunidades).EndInit();
            pnlFiltros.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
