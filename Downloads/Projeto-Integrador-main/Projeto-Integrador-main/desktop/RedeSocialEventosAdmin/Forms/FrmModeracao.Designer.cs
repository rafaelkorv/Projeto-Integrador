namespace RedeSocialEventosAdmin.Forms
{
  partial class FrmModeracao
  {
    private System.ComponentModel.IContainer components = null;
    private Guna.UI2.WinForms.Guna2TextBox txtPesquisa;
    private Guna.UI2.WinForms.Guna2Button btnExcluir;
    private Guna.UI2.WinForms.Guna2Button btnRefresh;
    private Guna.UI2.WinForms.Guna2Button btnTabPosts;
    private Guna.UI2.WinForms.Guna2Button btnTabComentarios;
    private Guna.UI2.WinForms.Guna2DataGridView dgvConteudo;
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            txtPesquisa = new Guna.UI2.WinForms.Guna2TextBox();
            btnExcluir = new Guna.UI2.WinForms.Guna2Button();
            btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            btnTabPosts = new Guna.UI2.WinForms.Guna2Button();
            btnTabComentarios = new Guna.UI2.WinForms.Guna2Button();
            dgvConteudo = new Guna.UI2.WinForms.Guna2DataGridView();
            lblHeaderTitle = new Label();
            lblTotalRegistros = new Label();
            pnlFiltros = new Guna.UI2.WinForms.Guna2Panel();
            ((System.ComponentModel.ISupportInitialize)dgvConteudo).BeginInit();
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
            txtPesquisa.Location = new Point(362, 6);
            txtPesquisa.Margin = new Padding(4, 5, 4, 5);
            txtPesquisa.Name = "txtPesquisa";
            txtPesquisa.PasswordChar = '\0';
            txtPesquisa.PlaceholderText = " Buscar por conteúdo, autor ou título...";
            txtPesquisa.SelectedText = "";
            txtPesquisa.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtPesquisa.Size = new Size(443, 44);
            txtPesquisa.TabIndex = 2;
            txtPesquisa.TextChanged += txtPesquisa_TextChanged;
            // 
            // btnExcluir
            // 
            btnExcluir.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExcluir.BorderRadius = 8;
            btnExcluir.Cursor = Cursors.Hand;
            btnExcluir.CustomizableEdges = customizableEdges3;
            btnExcluir.FillColor = Color.FromArgb(254, 226, 226);
            btnExcluir.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExcluir.ForeColor = Color.FromArgb(220, 38, 38);
            btnExcluir.HoverState.FillColor = Color.FromArgb(254, 202, 202);
            btnExcluir.Location = new Point(922, 6);
            btnExcluir.Margin = new Padding(4, 3, 4, 3);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnExcluir.Size = new Size(175, 44);
            btnExcluir.TabIndex = 3;
            btnExcluir.Text = " Remover Item";
            btnExcluir.Click += btnExcluir_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BorderRadius = 8;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.CustomizableEdges = customizableEdges5;
            btnRefresh.FillColor = Color.FromArgb(241, 245, 249);
            btnRefresh.Font = new Font("Segoe UI", 10F);
            btnRefresh.ForeColor = Color.FromArgb(51, 65, 85);
            btnRefresh.HoverState.FillColor = Color.FromArgb(226, 232, 240);
            btnRefresh.Location = new Point(1108, 6);
            btnRefresh.Margin = new Padding(4, 3, 4, 3);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnRefresh.Size = new Size(47, 44);
            btnRefresh.TabIndex = 4;
            btnRefresh.Text = "🔄";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnTabPosts
            // 
            btnTabPosts.BorderRadius = 8;
            btnTabPosts.Cursor = Cursors.Hand;
            btnTabPosts.CustomizableEdges = customizableEdges7;
            btnTabPosts.FillColor = Color.FromArgb(79, 70, 229);
            btnTabPosts.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTabPosts.ForeColor = Color.White;
            btnTabPosts.Location = new Point(0, 6);
            btnTabPosts.Margin = new Padding(4, 3, 4, 3);
            btnTabPosts.Name = "btnTabPosts";
            btnTabPosts.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnTabPosts.Size = new Size(163, 44);
            btnTabPosts.TabIndex = 0;
            btnTabPosts.Text = " Publicações";
            btnTabPosts.Click += btnTabPosts_Click;
            // 
            // btnTabComentarios
            // 
            btnTabComentarios.BorderRadius = 8;
            btnTabComentarios.Cursor = Cursors.Hand;
            btnTabComentarios.CustomizableEdges = customizableEdges9;
            btnTabComentarios.FillColor = Color.FromArgb(241, 245, 249);
            btnTabComentarios.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTabComentarios.ForeColor = Color.FromArgb(51, 65, 85);
            btnTabComentarios.Location = new Point(175, 6);
            btnTabComentarios.Margin = new Padding(4, 3, 4, 3);
            btnTabComentarios.Name = "btnTabComentarios";
            btnTabComentarios.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnTabComentarios.Size = new Size(163, 44);
            btnTabComentarios.TabIndex = 1;
            btnTabComentarios.Text = " Comentários";
            btnTabComentarios.Click += btnTabComentarios_Click;
            // 
            // dgvConteudo
            // 
            dgvConteudo.AllowUserToAddRows = false;
            dgvConteudo.AllowUserToDeleteRows = false;
            dgvConteudo.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dgvConteudo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvConteudo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(15, 23, 42);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvConteudo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvConteudo.ColumnHeadersHeight = 42;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9.5F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(79, 70, 229);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvConteudo.DefaultCellStyle = dataGridViewCellStyle3;
            dgvConteudo.GridColor = Color.FromArgb(241, 245, 249);
            dgvConteudo.Location = new Point(29, 162);
            dgvConteudo.Margin = new Padding(4, 3, 4, 3);
            dgvConteudo.MultiSelect = false;
            dgvConteudo.Name = "dgvConteudo";
            dgvConteudo.ReadOnly = true;
            dgvConteudo.RowHeadersVisible = false;
            dgvConteudo.RowTemplate.Height = 36;
            dgvConteudo.Size = new Size(1155, 542);
            dgvConteudo.TabIndex = 3;
            dgvConteudo.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgvConteudo.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvConteudo.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvConteudo.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvConteudo.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvConteudo.ThemeStyle.BackColor = Color.White;
            dgvConteudo.ThemeStyle.GridColor = Color.FromArgb(241, 245, 249);
            dgvConteudo.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvConteudo.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvConteudo.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvConteudo.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvConteudo.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvConteudo.ThemeStyle.HeaderStyle.Height = 42;
            dgvConteudo.ThemeStyle.ReadOnly = true;
            dgvConteudo.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvConteudo.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvConteudo.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9.5F);
            dgvConteudo.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(30, 41, 59);
            dgvConteudo.ThemeStyle.RowsStyle.Height = 36;
            dgvConteudo.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(238, 242, 255);
            dgvConteudo.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(79, 70, 229);
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblHeaderTitle.Location = new Point(29, 23);
            lblHeaderTitle.Margin = new Padding(4, 0, 4, 0);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(390, 30);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "Moderação e Auditoria de Conteúdo";
            // 
            // lblTotalRegistros
            // 
            lblTotalRegistros.AutoSize = true;
            lblTotalRegistros.Font = new Font("Segoe UI", 9.5F);
            lblTotalRegistros.ForeColor = Color.FromArgb(100, 116, 139);
            lblTotalRegistros.Location = new Point(31, 60);
            lblTotalRegistros.Margin = new Padding(4, 0, 4, 0);
            lblTotalRegistros.Name = "lblTotalRegistros";
            lblTotalRegistros.Size = new Size(255, 17);
            lblTotalRegistros.TabIndex = 1;
            lblTotalRegistros.Text = "Monitore e modere posts e comentários...";
            // 
            // pnlFiltros
            // 
            pnlFiltros.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlFiltros.BackColor = Color.Transparent;
            pnlFiltros.Controls.Add(btnTabPosts);
            pnlFiltros.Controls.Add(btnTabComentarios);
            pnlFiltros.Controls.Add(btnRefresh);
            pnlFiltros.Controls.Add(btnExcluir);
            pnlFiltros.Controls.Add(txtPesquisa);
            pnlFiltros.CustomizableEdges = customizableEdges11;
            pnlFiltros.Location = new Point(29, 92);
            pnlFiltros.Margin = new Padding(4, 3, 4, 3);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.ShadowDecoration.CustomizableEdges = customizableEdges12;
            pnlFiltros.Size = new Size(1155, 58);
            pnlFiltros.TabIndex = 2;
            // 
            // FrmModeracao
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            ClientSize = new Size(1213, 738);
            Controls.Add(dgvConteudo);
            Controls.Add(pnlFiltros);
            Controls.Add(lblTotalRegistros);
            Controls.Add(lblHeaderTitle);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "FrmModeracao";
            Text = "Moderação de Conteúdo";
            Load += FrmModeracao_Load;
            ((System.ComponentModel.ISupportInitialize)dgvConteudo).EndInit();
            pnlFiltros.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
