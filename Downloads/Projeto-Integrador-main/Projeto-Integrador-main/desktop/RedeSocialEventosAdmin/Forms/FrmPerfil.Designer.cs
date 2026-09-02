namespace RedeSocialEventosAdmin.Forms
{
  partial class FrmPerfil
  {
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.Label lblHeaderTitle;
    private System.Windows.Forms.Label lblSubInfo;
    private Guna.UI2.WinForms.Guna2Panel pnlCardPerfil;
    private FontAwesome.Sharp.IconPictureBox picAvatar;
    private System.Windows.Forms.Label lblNomeAdmin;
    private System.Windows.Forms.Label lblEmailAdmin;
    private System.Windows.Forms.Label lblUsernameAdmin;
    private Guna.UI2.WinForms.Guna2Chip chipRoleBadge;
    private Guna.UI2.WinForms.Guna2Chip chipStatusBadge;
    private System.Windows.Forms.Label lblDataCriacaoAdmin;
    private Guna.UI2.WinForms.Guna2GroupBox grpPermissoes;
    private System.Windows.Forms.Label lblPerm1;
    private System.Windows.Forms.Label lblPerm2;
    private System.Windows.Forms.Label lblPerm3;
    private System.Windows.Forms.Label lblPerm4;
    private System.Windows.Forms.Label lblPerm5;
    private Guna.UI2.WinForms.Guna2Panel pnlInfoSistema;
    private System.Windows.Forms.Label lblInfoSysTitulo;
    private System.Windows.Forms.Label lblSysHost;
    private System.Windows.Forms.Label lblSysDb;
    private System.Windows.Forms.Label lblSysAuth;

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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblHeaderTitle = new Label();
            lblSubInfo = new Label();
            pnlCardPerfil = new Guna.UI2.WinForms.Guna2Panel();
            lblDataCriacaoAdmin = new Label();
            chipStatusBadge = new Guna.UI2.WinForms.Guna2Chip();
            chipRoleBadge = new Guna.UI2.WinForms.Guna2Chip();
            lblUsernameAdmin = new Label();
            lblEmailAdmin = new Label();
            lblNomeAdmin = new Label();
            picAvatar = new FontAwesome.Sharp.IconPictureBox();
            grpPermissoes = new Guna.UI2.WinForms.Guna2GroupBox();
            lblPerm5 = new Label();
            lblPerm4 = new Label();
            lblPerm3 = new Label();
            lblPerm2 = new Label();
            lblPerm1 = new Label();
            pnlInfoSistema = new Guna.UI2.WinForms.Guna2Panel();
            lblSysAuth = new Label();
            lblSysDb = new Label();
            lblSysHost = new Label();
            lblInfoSysTitulo = new Label();
            pnlCardPerfil.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
            grpPermissoes.SuspendLayout();
            pnlInfoSistema.SuspendLayout();
            SuspendLayout();
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblHeaderTitle.Location = new Point(29, 23);
            lblHeaderTitle.Margin = new Padding(4, 0, 4, 0);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(257, 30);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "Perfil do Administrador";
            // 
            // lblSubInfo
            // 
            lblSubInfo.AutoSize = true;
            lblSubInfo.Font = new Font("Segoe UI", 9.5F);
            lblSubInfo.ForeColor = Color.FromArgb(100, 116, 139);
            lblSubInfo.Location = new Point(31, 60);
            lblSubInfo.Margin = new Padding(4, 0, 4, 0);
            lblSubInfo.Name = "lblSubInfo";
            lblSubInfo.Size = new Size(396, 17);
            lblSubInfo.TabIndex = 1;
            lblSubInfo.Text = "Credenciais autenticadas e privilégios absolutos de administração.";
            // 
            // pnlCardPerfil
            // 
            pnlCardPerfil.BackColor = Color.White;
            pnlCardPerfil.BorderColor = Color.FromArgb(226, 232, 240);
            pnlCardPerfil.BorderRadius = 14;
            pnlCardPerfil.BorderThickness = 1;
            pnlCardPerfil.Controls.Add(lblDataCriacaoAdmin);
            pnlCardPerfil.Controls.Add(chipStatusBadge);
            pnlCardPerfil.Controls.Add(chipRoleBadge);
            pnlCardPerfil.Controls.Add(lblUsernameAdmin);
            pnlCardPerfil.Controls.Add(lblEmailAdmin);
            pnlCardPerfil.Controls.Add(lblNomeAdmin);
            pnlCardPerfil.Controls.Add(picAvatar);
            pnlCardPerfil.CustomizableEdges = customizableEdges5;
            pnlCardPerfil.Location = new Point(29, 104);
            pnlCardPerfil.Margin = new Padding(4, 3, 4, 3);
            pnlCardPerfil.Name = "pnlCardPerfil";
            pnlCardPerfil.ShadowDecoration.CustomizableEdges = customizableEdges6;
            pnlCardPerfil.Size = new Size(537, 300);
            pnlCardPerfil.TabIndex = 2;
            // 
            // lblDataCriacaoAdmin
            // 
            lblDataCriacaoAdmin.AutoSize = true;
            lblDataCriacaoAdmin.Font = new Font("Segoe UI", 9F);
            lblDataCriacaoAdmin.ForeColor = Color.FromArgb(148, 163, 184);
            lblDataCriacaoAdmin.Location = new Point(146, 237);
            lblDataCriacaoAdmin.Margin = new Padding(4, 0, 4, 0);
            lblDataCriacaoAdmin.Name = "lblDataCriacaoAdmin";
            lblDataCriacaoAdmin.Size = new Size(151, 15);
            lblDataCriacaoAdmin.TabIndex = 6;
            lblDataCriacaoAdmin.Text = "Membro desde: 01/01/2026";
            // 
            // chipStatusBadge
            // 
            chipStatusBadge.AutoRoundedCorners = true;
            chipStatusBadge.BorderRadius = 15;
            chipStatusBadge.CustomizableEdges = customizableEdges1;
            chipStatusBadge.FillColor = Color.FromArgb(209, 250, 229);
            chipStatusBadge.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            chipStatusBadge.ForeColor = Color.FromArgb(5, 150, 105);
            chipStatusBadge.Location = new Point(303, 185);
            chipStatusBadge.Margin = new Padding(4, 3, 4, 3);
            chipStatusBadge.Name = "chipStatusBadge";
            chipStatusBadge.ShadowDecoration.CustomizableEdges = customizableEdges2;
            chipStatusBadge.Size = new Size(117, 32);
            chipStatusBadge.TabIndex = 5;
            chipStatusBadge.Text = "ATIVO";
            // 
            // chipRoleBadge
            // 
            chipRoleBadge.AutoRoundedCorners = true;
            chipRoleBadge.BorderRadius = 15;
            chipRoleBadge.CustomizableEdges = customizableEdges3;
            chipRoleBadge.FillColor = Color.FromArgb(238, 242, 255);
            chipRoleBadge.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            chipRoleBadge.ForeColor = Color.FromArgb(79, 70, 229);
            chipRoleBadge.Location = new Point(146, 185);
            chipRoleBadge.Margin = new Padding(4, 3, 4, 3);
            chipRoleBadge.Name = "chipRoleBadge";
            chipRoleBadge.ShadowDecoration.CustomizableEdges = customizableEdges4;
            chipRoleBadge.Size = new Size(146, 32);
            chipRoleBadge.TabIndex = 4;
            chipRoleBadge.Text = "SUPER ADMIN";
            // 
            // lblUsernameAdmin
            // 
            lblUsernameAdmin.AutoSize = true;
            lblUsernameAdmin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblUsernameAdmin.ForeColor = Color.FromArgb(99, 102, 241);
            lblUsernameAdmin.Location = new Point(146, 75);
            lblUsernameAdmin.Margin = new Padding(4, 0, 4, 0);
            lblUsernameAdmin.Name = "lblUsernameAdmin";
            lblUsernameAdmin.Size = new Size(64, 19);
            lblUsernameAdmin.TabIndex = 3;
            lblUsernameAdmin.Text = "@admin";
            // 
            // lblEmailAdmin
            // 
            lblEmailAdmin.AutoSize = true;
            lblEmailAdmin.Font = new Font("Segoe UI", 9.5F);
            lblEmailAdmin.ForeColor = Color.FromArgb(100, 116, 139);
            lblEmailAdmin.Location = new Point(146, 110);
            lblEmailAdmin.Margin = new Padding(4, 0, 4, 0);
            lblEmailAdmin.Name = "lblEmailAdmin";
            lblEmailAdmin.Size = new Size(138, 17);
            lblEmailAdmin.TabIndex = 2;
            lblEmailAdmin.Text = "admin@socialjoin.com";
            // 
            // lblNomeAdmin
            // 
            lblNomeAdmin.AutoSize = true;
            lblNomeAdmin.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblNomeAdmin.ForeColor = Color.FromArgb(15, 23, 42);
            lblNomeAdmin.Location = new Point(146, 35);
            lblNomeAdmin.Margin = new Padding(4, 0, 4, 0);
            lblNomeAdmin.Name = "lblNomeAdmin";
            lblNomeAdmin.Size = new Size(207, 25);
            lblNomeAdmin.TabIndex = 1;
            lblNomeAdmin.Text = "Administrador Master";
            // 
            // picAvatar
            // 
            picAvatar.BackColor = Color.FromArgb(238, 242, 255);
            picAvatar.ForeColor = Color.FromArgb(79, 70, 229);
            picAvatar.IconChar = FontAwesome.Sharp.IconChar.UserShield;
            picAvatar.IconColor = Color.FromArgb(79, 70, 229);
            picAvatar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            picAvatar.IconSize = 92;
            picAvatar.Location = new Point(29, 35);
            picAvatar.Margin = new Padding(4, 3, 4, 3);
            picAvatar.Name = "picAvatar";
            picAvatar.Size = new Size(93, 92);
            picAvatar.SizeMode = PictureBoxSizeMode.CenterImage;
            picAvatar.TabIndex = 0;
            picAvatar.TabStop = false;
            // 
            // grpPermissoes
            // 
            grpPermissoes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpPermissoes.BorderColor = Color.FromArgb(226, 232, 240);
            grpPermissoes.BorderRadius = 14;
            grpPermissoes.Controls.Add(lblPerm5);
            grpPermissoes.Controls.Add(lblPerm4);
            grpPermissoes.Controls.Add(lblPerm3);
            grpPermissoes.Controls.Add(lblPerm2);
            grpPermissoes.Controls.Add(lblPerm1);
            grpPermissoes.CustomBorderColor = Color.FromArgb(241, 245, 249);
            grpPermissoes.CustomizableEdges = customizableEdges7;
            grpPermissoes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grpPermissoes.ForeColor = Color.FromArgb(15, 23, 42);
            grpPermissoes.Location = new Point(595, 104);
            grpPermissoes.Margin = new Padding(4, 3, 4, 3);
            grpPermissoes.Name = "grpPermissoes";
            grpPermissoes.ShadowDecoration.CustomizableEdges = customizableEdges8;
            grpPermissoes.Size = new Size(589, 300);
            grpPermissoes.TabIndex = 3;
            grpPermissoes.Text = "Privilégios Administrativos Absolutos";
            // 
            // lblPerm5
            // 
            lblPerm5.AutoSize = true;
            lblPerm5.Font = new Font("Segoe UI", 9.5F);
            lblPerm5.ForeColor = Color.FromArgb(51, 65, 85);
            lblPerm5.Location = new Point(23, 237);
            lblPerm5.Margin = new Padding(4, 0, 4, 0);
            lblPerm5.Name = "lblPerm5";
            lblPerm5.Size = new Size(365, 17);
            lblPerm5.TabIndex = 4;
            lblPerm5.Text = "Exportação de relatórios executivos e métricas consolidadas.";
            // 
            // lblPerm4
            // 
            lblPerm4.AutoSize = true;
            lblPerm4.Font = new Font("Segoe UI", 9.5F);
            lblPerm4.ForeColor = Color.FromArgb(51, 65, 85);
            lblPerm4.Location = new Point(23, 194);
            lblPerm4.Margin = new Padding(4, 0, 4, 0);
            lblPerm4.Name = "lblPerm4";
            lblPerm4.Size = new Size(413, 17);
            lblPerm4.TabIndex = 3;
            lblPerm4.Text = "Moderação e exclusão direta de qualquer publicação ou comentário.";
            // 
            // lblPerm3
            // 
            lblPerm3.AutoSize = true;
            lblPerm3.Font = new Font("Segoe UI", 9.5F);
            lblPerm3.ForeColor = Color.FromArgb(51, 65, 85);
            lblPerm3.Location = new Point(23, 151);
            lblPerm3.Margin = new Padding(4, 0, 4, 0);
            lblPerm3.Name = "lblPerm3";
            lblPerm3.Size = new Size(448, 17);
            lblPerm3.TabIndex = 2;
            lblPerm3.Text = "Criação, edição e exclusão de comunidades e gerenciamento de membros.";
            // 
            // lblPerm2
            // 
            lblPerm2.AutoSize = true;
            lblPerm2.Font = new Font("Segoe UI", 9.5F);
            lblPerm2.ForeColor = Color.FromArgb(51, 65, 85);
            lblPerm2.Location = new Point(23, 108);
            lblPerm2.Margin = new Padding(4, 0, 4, 0);
            lblPerm2.Name = "lblPerm2";
            lblPerm2.Size = new Size(421, 17);
            lblPerm2.TabIndex = 1;
            lblPerm2.Text = "Criação, alteração de status e cancelamento/exclusão total de eventos.";
            // 
            // lblPerm1
            // 
            lblPerm1.AutoSize = true;
            lblPerm1.Font = new Font("Segoe UI", 9.5F);
            lblPerm1.ForeColor = Color.FromArgb(51, 65, 85);
            lblPerm1.Location = new Point(23, 66);
            lblPerm1.Margin = new Padding(4, 0, 4, 0);
            lblPerm1.Name = "lblPerm1";
            lblPerm1.Size = new Size(438, 17);
            lblPerm1.TabIndex = 0;
            lblPerm1.Text = "Atribuição e revogação de qualquer Role e Status em contas de usuários.";
            // 
            // pnlInfoSistema
            // 
            pnlInfoSistema.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlInfoSistema.BackColor = Color.White;
            pnlInfoSistema.BorderColor = Color.FromArgb(226, 232, 240);
            pnlInfoSistema.BorderRadius = 14;
            pnlInfoSistema.BorderThickness = 1;
            pnlInfoSistema.Controls.Add(lblSysAuth);
            pnlInfoSistema.Controls.Add(lblSysDb);
            pnlInfoSistema.Controls.Add(lblSysHost);
            pnlInfoSistema.Controls.Add(lblInfoSysTitulo);
            pnlInfoSistema.CustomizableEdges = customizableEdges9;
            pnlInfoSistema.Location = new Point(29, 427);
            pnlInfoSistema.Margin = new Padding(4, 3, 4, 3);
            pnlInfoSistema.Name = "pnlInfoSistema";
            pnlInfoSistema.ShadowDecoration.CustomizableEdges = customizableEdges10;
            pnlInfoSistema.Size = new Size(1155, 162);
            pnlInfoSistema.TabIndex = 4;
            // 
            // lblSysAuth
            // 
            lblSysAuth.AutoSize = true;
            lblSysAuth.Font = new Font("Segoe UI", 9.5F);
            lblSysAuth.ForeColor = Color.FromArgb(100, 116, 139);
            lblSysAuth.Location = new Point(29, 110);
            lblSysAuth.Margin = new Padding(4, 0, 4, 0);
            lblSysAuth.Name = "lblSysAuth";
            lblSysAuth.Size = new Size(331, 17);
            lblSysAuth.TabIndex = 3;
            lblSysAuth.Text = "Nível de Segurança: Restrição Obrigatória 'Role Admin'";
            // 
            // lblSysDb
            // 
            lblSysDb.AutoSize = true;
            lblSysDb.Font = new Font("Segoe UI", 9.5F);
            lblSysDb.ForeColor = Color.FromArgb(100, 116, 139);
            lblSysDb.Location = new Point(29, 78);
            lblSysDb.Margin = new Padding(4, 0, 4, 0);
            lblSysDb.Name = "lblSysDb";
            lblSysDb.Size = new Size(256, 17);
            lblSysDb.TabIndex = 2;
            lblSysDb.Text = "Banco de Dados: cl203108 (MySQL Server)";
            // 
            // lblSysHost
            // 
            lblSysHost.AutoSize = true;
            lblSysHost.Font = new Font("Segoe UI", 9.5F);
            lblSysHost.ForeColor = Color.FromArgb(100, 116, 139);
            lblSysHost.Location = new Point(29, 48);
            lblSysHost.Margin = new Padding(4, 0, 4, 0);
            lblSysHost.Name = "lblSysHost";
            lblSysHost.Size = new Size(292, 17);
            lblSysHost.TabIndex = 1;
            lblSysHost.Text = "Servidor Corporativo: 143.106.241.3 (Conectado)";
            // 
            // lblInfoSysTitulo
            // 
            lblInfoSysTitulo.AutoSize = true;
            lblInfoSysTitulo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblInfoSysTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblInfoSysTitulo.Location = new Point(29, 17);
            lblInfoSysTitulo.Margin = new Padding(4, 0, 4, 0);
            lblInfoSysTitulo.Name = "lblInfoSysTitulo";
            lblInfoSysTitulo.Size = new Size(189, 20);
            lblInfoSysTitulo.TabIndex = 0;
            lblInfoSysTitulo.Text = "Infraestrutura do Sistema";
            // 
            // FrmPerfil
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 252);
            ClientSize = new Size(1213, 738);
            Controls.Add(pnlInfoSistema);
            Controls.Add(grpPermissoes);
            Controls.Add(pnlCardPerfil);
            Controls.Add(lblSubInfo);
            Controls.Add(lblHeaderTitle);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "FrmPerfil";
            Text = "Meu Perfil";
            Load += FrmPerfil_Load;
            pnlCardPerfil.ResumeLayout(false);
            pnlCardPerfil.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
            grpPermissoes.ResumeLayout(false);
            grpPermissoes.PerformLayout();
            pnlInfoSistema.ResumeLayout(false);
            pnlInfoSistema.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
