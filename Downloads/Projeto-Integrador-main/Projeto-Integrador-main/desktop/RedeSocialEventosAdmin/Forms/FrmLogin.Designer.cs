namespace RedeSocialEventosAdmin.Forms
{
  partial class FrmLogin
  {
    private System.ComponentModel.IContainer components = null;
    private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
    private Guna.UI2.WinForms.Guna2Panel pnlLateral;
    private System.Windows.Forms.Label lblBrandTitle;
    private System.Windows.Forms.Label lblBrandSub;
    private FontAwesome.Sharp.IconPictureBox picLogo;
    private Guna.UI2.WinForms.Guna2TextBox txtEmail;
    private Guna.UI2.WinForms.Guna2TextBox txtSenha;
    private Guna.UI2.WinForms.Guna2Button btnEntrar;
    private System.Windows.Forms.Label lblTitulo;
    private System.Windows.Forms.Label lblSubtitulo;
    private FontAwesome.Sharp.IconButton btnFechar;
    private Guna.UI2.WinForms.Guna2Chip chipSecurity;
    private FontAwesome.Sharp.IconButton btnVerSenha;

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
      this.components = new System.ComponentModel.Container();
      this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
      this.pnlLateral = new Guna.UI2.WinForms.Guna2Panel();
      this.chipSecurity = new Guna.UI2.WinForms.Guna2Chip();
      this.lblBrandSub = new System.Windows.Forms.Label();
      this.lblBrandTitle = new System.Windows.Forms.Label();
      this.picLogo = new FontAwesome.Sharp.IconPictureBox();
      this.txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtSenha = new Guna.UI2.WinForms.Guna2TextBox();
      this.btnEntrar = new Guna.UI2.WinForms.Guna2Button();
      this.lblTitulo = new System.Windows.Forms.Label();
      this.lblSubtitulo = new System.Windows.Forms.Label();
      this.btnFechar = new FontAwesome.Sharp.IconButton();
      this.btnVerSenha = new FontAwesome.Sharp.IconButton();
      this.pnlLateral.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // guna2Elipse1
      // 
      this.guna2Elipse1.BorderRadius = 16;
      this.guna2Elipse1.TargetControl = this;
      // 
      // pnlLateral
      // 
      this.pnlLateral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.pnlLateral.Controls.Add(this.chipSecurity);
      this.pnlLateral.Controls.Add(this.lblBrandSub);
      this.pnlLateral.Controls.Add(this.lblBrandTitle);
      this.pnlLateral.Controls.Add(this.picLogo);
      this.pnlLateral.Dock = System.Windows.Forms.DockStyle.Left;
      this.pnlLateral.Location = new System.Drawing.Point(0, 0);
      this.pnlLateral.Name = "pnlLateral";
      this.pnlLateral.Size = new System.Drawing.Size(320, 480);
      this.pnlLateral.TabIndex = 0;
      // 
      // chipSecurity
      // 
      this.chipSecurity.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.chipSecurity.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
      this.chipSecurity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chipSecurity.Location = new System.Drawing.Point(50, 400);
      this.chipSecurity.Name = "chipSecurity";
      this.chipSecurity.Size = new System.Drawing.Size(220, 32);
      this.chipSecurity.TabIndex = 3;
      this.chipSecurity.Text = " ROLE 'ADMIN' OBRIGATÓRIA";
      // 
      // lblBrandSub
      // 
      this.lblBrandSub.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.lblBrandSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.lblBrandSub.Location = new System.Drawing.Point(20, 260);
      this.lblBrandSub.Name = "lblBrandSub";
      this.lblBrandSub.Size = new System.Drawing.Size(280, 45);
      this.lblBrandSub.TabIndex = 2;
      this.lblBrandSub.Text = "Painel de Controle e Super Administração da Rede Social SocialJoin.";
      this.lblBrandSub.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // lblBrandTitle
      // 
      this.lblBrandTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
      this.lblBrandTitle.ForeColor = System.Drawing.Color.White;
      this.lblBrandTitle.Location = new System.Drawing.Point(20, 215);
      this.lblBrandTitle.Name = "lblBrandTitle";
      this.lblBrandTitle.Size = new System.Drawing.Size(280, 35);
      this.lblBrandTitle.TabIndex = 1;
      this.lblBrandTitle.Text = "SocialJoin Admin";
      this.lblBrandTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // picLogo
      // 
      this.picLogo.BackColor = System.Drawing.Color.Transparent;
      this.picLogo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.picLogo.IconChar = FontAwesome.Sharp.IconChar.ShieldAlt;
      this.picLogo.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.picLogo.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.picLogo.IconSize = 85;
      this.picLogo.Location = new System.Drawing.Point(117, 100);
      this.picLogo.Name = "picLogo";
      this.picLogo.Size = new System.Drawing.Size(85, 85);
      this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.picLogo.TabIndex = 0;
      this.picLogo.TabStop = false;
      // 
      // txtEmail
      // 
      this.txtEmail.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
      this.txtEmail.BorderRadius = 8;
      this.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtEmail.DefaultText = "";
      this.txtEmail.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
      this.txtEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtEmail.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.txtEmail.Location = new System.Drawing.Point(370, 170);
      this.txtEmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.PasswordChar = '\0';
      this.txtEmail.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.txtEmail.PlaceholderText = "E-mail ou Usuário de Acesso";
      this.txtEmail.SelectedText = "";
      this.txtEmail.Size = new System.Drawing.Size(430, 48);
      this.txtEmail.TabIndex = 1;
      this.txtEmail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Campos_KeyDown);
      // 
      // txtSenha
      // 
      this.txtSenha.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
      this.txtSenha.BorderRadius = 8;
      this.txtSenha.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtSenha.DefaultText = "";
      this.txtSenha.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtSenha.Font = new System.Drawing.Font("Segoe UI", 10F);
      this.txtSenha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtSenha.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.txtSenha.Location = new System.Drawing.Point(370, 240);
      this.txtSenha.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtSenha.Name = "txtSenha";
      this.txtSenha.PasswordChar = '●';
      this.txtSenha.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.txtSenha.PlaceholderText = "Senha de Acesso";
      this.txtSenha.SelectedText = "";
      this.txtSenha.Size = new System.Drawing.Size(380, 48);
      this.txtSenha.TabIndex = 2;
      this.txtSenha.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Campos_KeyDown);
      // 
      // btnVerSenha
      // 
      this.btnVerSenha.BackColor = System.Drawing.Color.White;
      this.btnVerSenha.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnVerSenha.FlatAppearance.BorderSize = 0;
      this.btnVerSenha.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnVerSenha.IconChar = FontAwesome.Sharp.IconChar.Eye;
      this.btnVerSenha.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnVerSenha.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.btnVerSenha.IconSize = 22;
      this.btnVerSenha.Location = new System.Drawing.Point(755, 240);
      this.btnVerSenha.Name = "btnVerSenha";
      this.btnVerSenha.Size = new System.Drawing.Size(45, 48);
      this.btnVerSenha.TabIndex = 7;
      this.btnVerSenha.UseVisualStyleBackColor = false;
      this.btnVerSenha.Click += new System.EventHandler(this.btnVerSenha_Click);
      // 
      // btnEntrar
      // 
      this.btnEntrar.BorderRadius = 8;
      this.btnEntrar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnEntrar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.btnEntrar.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
      this.btnEntrar.ForeColor = System.Drawing.Color.White;
      this.btnEntrar.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(56)))), ((int)(((byte)(202)))));
      this.btnEntrar.Location = new System.Drawing.Point(370, 320);
      this.btnEntrar.Name = "btnEntrar";
      this.btnEntrar.Size = new System.Drawing.Size(430, 48);
      this.btnEntrar.TabIndex = 3;
      this.btnEntrar.Text = "AUTENTICAR COMO ADMIN";
      this.btnEntrar.Click += new System.EventHandler(this.btnEntrar_Click);
      // 
      // lblTitulo
      // 
      this.lblTitulo.AutoSize = true;
      this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
      this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.lblTitulo.Location = new System.Drawing.Point(363, 60);
      this.lblTitulo.Name = "lblTitulo";
      this.lblTitulo.Size = new System.Drawing.Size(206, 37);
      this.lblTitulo.TabIndex = 4;
      this.lblTitulo.Text = "Acesso Restrito";
      // 
      // lblSubtitulo
      // 
      this.lblSubtitulo.AutoSize = true;
      this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 10F);
      this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
      this.lblSubtitulo.Location = new System.Drawing.Point(366, 105);
      this.lblSubtitulo.Name = "lblSubtitulo";
      this.lblSubtitulo.Size = new System.Drawing.Size(374, 19);
      this.lblSubtitulo.TabIndex = 5;
      this.lblSubtitulo.Text = "Credenciais autorizadas apenas com privilégios de Admin.";
      // 
      // btnFechar
      // 
      this.btnFechar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnFechar.FlatAppearance.BorderSize = 0;
      this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnFechar.IconChar = FontAwesome.Sharp.IconChar.Times;
      this.btnFechar.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnFechar.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.btnFechar.IconSize = 22;
      this.btnFechar.Location = new System.Drawing.Point(800, 12);
      this.btnFechar.Name = "btnFechar";
      this.btnFechar.Size = new System.Drawing.Size(35, 35);
      this.btnFechar.TabIndex = 6;
      this.btnFechar.UseVisualStyleBackColor = true;
      this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
      // 
      // FrmLogin
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
      this.ClientSize = new System.Drawing.Size(850, 480);
      this.Controls.Add(this.btnVerSenha);
      this.Controls.Add(this.btnFechar);
      this.Controls.Add(this.lblSubtitulo);
      this.Controls.Add(this.lblTitulo);
      this.Controls.Add(this.btnEntrar);
      this.Controls.Add(this.txtSenha);
      this.Controls.Add(this.txtEmail);
      this.Controls.Add(this.pnlLateral);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "FrmLogin";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "SocialJoin - Painel Administrativo Pro";
      this.pnlLateral.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
