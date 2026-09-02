namespace RedeSocialEventosAdmin.Forms
{
  partial class FrmUsuarioMockCadastro
  {
    private System.ComponentModel.IContainer components = null;
    private Guna.UI2.WinForms.Guna2Elipse guna2ElipseForm;
    private System.Windows.Forms.Label lblOperacao;
    private System.Windows.Forms.Label lblSubInfo;
    private Guna.UI2.WinForms.Guna2TextBox txtUsername;
    private Guna.UI2.WinForms.Guna2TextBox txtNome;
    private Guna.UI2.WinForms.Guna2TextBox txtEmail;
    private Guna.UI2.WinForms.Guna2TextBox txtTelefone;
    private Guna.UI2.WinForms.Guna2TextBox txtSenha;
    private Guna.UI2.WinForms.Guna2TextBox txtBio;
    private Guna.UI2.WinForms.Guna2ComboBox cmbStatus;
    private Guna.UI2.WinForms.Guna2TextBox txtCustomRole;
    private Guna.UI2.WinForms.Guna2CheckBox chkRoleAdmin;
    private Guna.UI2.WinForms.Guna2CheckBox chkRoleModerator;
    private Guna.UI2.WinForms.Guna2CheckBox chkRolePremium;
    private Guna.UI2.WinForms.Guna2CheckBox chkRoleTester;
    private Guna.UI2.WinForms.Guna2CheckBox chkRoleBeta;
    private Guna.UI2.WinForms.Guna2CheckBox chkRoleUser;
    private Guna.UI2.WinForms.Guna2Button btnSalvar;
    private Guna.UI2.WinForms.Guna2Button btnCancelar;
    private Guna.UI2.WinForms.Guna2GroupBox grpRoles;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.Label lblAvisoSenha;
    private FontAwesome.Sharp.IconButton btnToggleSenha;

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
      this.guna2ElipseForm = new Guna.UI2.WinForms.Guna2Elipse(this.components);
      this.lblOperacao = new System.Windows.Forms.Label();
      this.lblSubInfo = new System.Windows.Forms.Label();
      this.txtUsername = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtNome = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtTelefone = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtSenha = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtBio = new Guna.UI2.WinForms.Guna2TextBox();
      this.cmbStatus = new Guna.UI2.WinForms.Guna2ComboBox();
      this.grpRoles = new Guna.UI2.WinForms.Guna2GroupBox();
      this.txtCustomRole = new Guna.UI2.WinForms.Guna2TextBox();
      this.chkRoleAdmin = new Guna.UI2.WinForms.Guna2CheckBox();
      this.chkRoleModerator = new Guna.UI2.WinForms.Guna2CheckBox();
      this.chkRolePremium = new Guna.UI2.WinForms.Guna2CheckBox();
      this.chkRoleTester = new Guna.UI2.WinForms.Guna2CheckBox();
      this.chkRoleBeta = new Guna.UI2.WinForms.Guna2CheckBox();
      this.chkRoleUser = new Guna.UI2.WinForms.Guna2CheckBox();
      this.btnSalvar = new Guna.UI2.WinForms.Guna2Button();
      this.btnCancelar = new Guna.UI2.WinForms.Guna2Button();
      this.lblStatus = new System.Windows.Forms.Label();
      this.lblAvisoSenha = new System.Windows.Forms.Label();
      this.btnToggleSenha = new FontAwesome.Sharp.IconButton();
      this.grpRoles.SuspendLayout();
      this.SuspendLayout();
      // 
      // guna2ElipseForm
      // 
      this.guna2ElipseForm.BorderRadius = 16;
      this.guna2ElipseForm.TargetControl = this;
      // 
      // lblOperacao
      // 
      this.lblOperacao.AutoSize = true;
      this.lblOperacao.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
      this.lblOperacao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
      this.lblOperacao.Location = new System.Drawing.Point(30, 20);
      this.lblOperacao.Name = "lblOperacao";
      this.lblOperacao.Size = new System.Drawing.Size(258, 30);
      this.lblOperacao.TabIndex = 0;
      this.lblOperacao.Text = "Gerenciamento de Conta";
      // 
      // lblSubInfo
      // 
      this.lblSubInfo.AutoSize = true;
      this.lblSubInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.lblSubInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
      this.lblSubInfo.Location = new System.Drawing.Point(32, 53);
      this.lblSubInfo.Name = "lblSubInfo";
      this.lblSubInfo.Size = new System.Drawing.Size(370, 17);
      this.lblSubInfo.TabIndex = 1;
      this.lblSubInfo.Text = "Edite credenciais, atribua roles e defina o status de permissão.";
      // 
      // txtUsername
      // 
      this.txtUsername.BorderRadius = 8;
      this.txtUsername.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtUsername.DefaultText = "";
      this.txtUsername.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtUsername.Location = new System.Drawing.Point(35, 90);
      this.txtUsername.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtUsername.Name = "txtUsername";
      this.txtUsername.PlaceholderText = "Nome de Usuário (Username)";
      this.txtUsername.SelectedText = "";
      this.txtUsername.Size = new System.Drawing.Size(280, 42);
      this.txtUsername.TabIndex = 1;
      // 
      // txtNome
      // 
      this.txtNome.BorderRadius = 8;
      this.txtNome.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtNome.DefaultText = "";
      this.txtNome.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtNome.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtNome.Location = new System.Drawing.Point(335, 90);
      this.txtNome.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtNome.Name = "txtNome";
      this.txtNome.PlaceholderText = "Nome Completo";
      this.txtNome.SelectedText = "";
      this.txtNome.Size = new System.Drawing.Size(280, 42);
      this.txtNome.TabIndex = 2;
      // 
      // txtEmail
      // 
      this.txtEmail.BorderRadius = 8;
      this.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtEmail.DefaultText = "";
      this.txtEmail.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtEmail.Location = new System.Drawing.Point(35, 145);
      this.txtEmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.PlaceholderText = "Endereço de E-mail";
      this.txtEmail.SelectedText = "";
      this.txtEmail.Size = new System.Drawing.Size(280, 42);
      this.txtEmail.TabIndex = 3;
      // 
      // txtTelefone
      // 
      this.txtTelefone.BorderRadius = 8;
      this.txtTelefone.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtTelefone.DefaultText = "";
      this.txtTelefone.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtTelefone.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtTelefone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtTelefone.Location = new System.Drawing.Point(335, 145);
      this.txtTelefone.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtTelefone.Name = "txtTelefone";
      this.txtTelefone.PlaceholderText = "Telefone (Opcional)";
      this.txtTelefone.SelectedText = "";
      this.txtTelefone.Size = new System.Drawing.Size(280, 42);
      this.txtTelefone.TabIndex = 4;
      // 
      // txtSenha
      // 
      this.txtSenha.BorderRadius = 8;
      this.txtSenha.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtSenha.DefaultText = "";
      this.txtSenha.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtSenha.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtSenha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtSenha.Location = new System.Drawing.Point(35, 200);
      this.txtSenha.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtSenha.Name = "txtSenha";
      this.txtSenha.PasswordChar = '\u25CF';
      this.txtSenha.PlaceholderText = "Nova Senha (Mín. 6 dígitos)";
      this.txtSenha.SelectedText = "";
      this.txtSenha.Size = new System.Drawing.Size(240, 42);
      this.txtSenha.TabIndex = 5;
      // 
      // btnToggleSenha
      // 
      this.btnToggleSenha.BackColor = System.Drawing.Color.White;
      this.btnToggleSenha.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnToggleSenha.FlatAppearance.BorderSize = 0;
      this.btnToggleSenha.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnToggleSenha.IconChar = FontAwesome.Sharp.IconChar.Eye;
      this.btnToggleSenha.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.btnToggleSenha.IconFont = FontAwesome.Sharp.IconFont.Auto;
      this.btnToggleSenha.IconSize = 20;
      this.btnToggleSenha.Location = new System.Drawing.Point(277, 200);
      this.btnToggleSenha.Name = "btnToggleSenha";
      this.btnToggleSenha.Size = new System.Drawing.Size(38, 42);
      this.btnToggleSenha.TabIndex = 13;
      this.btnToggleSenha.UseVisualStyleBackColor = false;
      // 
      // lblAvisoSenha
      // 
      this.lblAvisoSenha.AutoSize = true;
      this.lblAvisoSenha.Font = new System.Drawing.Font("Segoe UI", 8F);
      this.lblAvisoSenha.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
      this.lblAvisoSenha.Location = new System.Drawing.Point(35, 245);
      this.lblAvisoSenha.Name = "lblAvisoSenha";
      this.lblAvisoSenha.Size = new System.Drawing.Size(237, 13);
      this.lblAvisoSenha.TabIndex = 14;
      this.lblAvisoSenha.Text = "* Deixe em branco para manter a senha atual.";
      // 
      // lblStatus
      // 
      this.lblStatus.AutoSize = true;
      this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.lblStatus.Location = new System.Drawing.Point(335, 200);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new System.Drawing.Size(95, 15);
      this.lblStatus.TabIndex = 15;
      this.lblStatus.Text = "Status da Conta:";
      // 
      // cmbStatus
      // 
      this.cmbStatus.BackColor = System.Drawing.Color.Transparent;
      this.cmbStatus.BorderRadius = 8;
      this.cmbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbStatus.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.cmbStatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.cmbStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.cmbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.cmbStatus.ItemHeight = 30;
      this.cmbStatus.Items.AddRange(new object[] {
      "ativo",
      "suspenso",
      "inativo"});
      this.cmbStatus.Location = new System.Drawing.Point(335, 220);
      this.cmbStatus.Name = "cmbStatus";
      this.cmbStatus.Size = new System.Drawing.Size(280, 36);
      this.cmbStatus.StartIndex = 0;
      this.cmbStatus.TabIndex = 6;
      // 
      // txtBio
      // 
      this.txtBio.BorderRadius = 8;
      this.txtBio.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtBio.DefaultText = "";
      this.txtBio.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtBio.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtBio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtBio.Location = new System.Drawing.Point(35, 270);
      this.txtBio.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtBio.Multiline = true;
      this.txtBio.Name = "txtBio";
      this.txtBio.PlaceholderText = "Biografia / Descrição do Usuário (Opcional)";
      this.txtBio.SelectedText = "";
      this.txtBio.Size = new System.Drawing.Size(580, 60);
      this.txtBio.TabIndex = 7;
      // 
      // grpRoles
      // 
      this.grpRoles.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
      this.grpRoles.BorderRadius = 10;
      this.grpRoles.Controls.Add(this.txtCustomRole);
      this.grpRoles.Controls.Add(this.chkRoleAdmin);
      this.grpRoles.Controls.Add(this.chkRoleModerator);
      this.grpRoles.Controls.Add(this.chkRolePremium);
      this.grpRoles.Controls.Add(this.chkRoleTester);
      this.grpRoles.Controls.Add(this.chkRoleBeta);
      this.grpRoles.Controls.Add(this.chkRoleUser);
      this.grpRoles.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.grpRoles.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
      this.grpRoles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.grpRoles.Location = new System.Drawing.Point(35, 345);
      this.grpRoles.Name = "grpRoles";
      this.grpRoles.Size = new System.Drawing.Size(580, 140);
      this.grpRoles.TabIndex = 8;
      this.grpRoles.Text = "Permissões e Roles de Acesso (Controle Total)";
      // 
      // chkRoleAdmin
      // 
      this.chkRoleAdmin.AutoSize = true;
      this.chkRoleAdmin.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkRoleAdmin.CheckedState.BorderRadius = 4;
      this.chkRoleAdmin.CheckedState.BorderThickness = 0;
      this.chkRoleAdmin.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkRoleAdmin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.chkRoleAdmin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.chkRoleAdmin.Location = new System.Drawing.Point(15, 50);
      this.chkRoleAdmin.Name = "chkRoleAdmin";
      this.chkRoleAdmin.Size = new System.Drawing.Size(107, 19);
      this.chkRoleAdmin.TabIndex = 0;
      this.chkRoleAdmin.Text = " Admin (Total)";
      this.chkRoleAdmin.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
      this.chkRoleAdmin.UncheckedState.BorderRadius = 4;
      this.chkRoleAdmin.UncheckedState.BorderThickness = 1;
      this.chkRoleAdmin.UncheckedState.FillColor = System.Drawing.Color.White;
      // 
      // chkRoleModerator
      // 
      this.chkRoleModerator.AutoSize = true;
      this.chkRoleModerator.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkRoleModerator.CheckedState.BorderRadius = 4;
      this.chkRoleModerator.CheckedState.BorderThickness = 0;
      this.chkRoleModerator.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkRoleModerator.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.chkRoleModerator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.chkRoleModerator.Location = new System.Drawing.Point(135, 50);
      this.chkRoleModerator.Name = "chkRoleModerator";
      this.chkRoleModerator.Size = new System.Drawing.Size(81, 19);
      this.chkRoleModerator.TabIndex = 1;
      this.chkRoleModerator.Text = "Moderator";
      this.chkRoleModerator.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
      this.chkRoleModerator.UncheckedState.BorderRadius = 4;
      this.chkRoleModerator.UncheckedState.BorderThickness = 1;
      this.chkRoleModerator.UncheckedState.FillColor = System.Drawing.Color.White;
      // 
      // chkRolePremium
      // 
      this.chkRolePremium.AutoSize = true;
      this.chkRolePremium.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkRolePremium.CheckedState.BorderRadius = 4;
      this.chkRolePremium.CheckedState.BorderThickness = 0;
      this.chkRolePremium.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkRolePremium.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.chkRolePremium.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.chkRolePremium.Location = new System.Drawing.Point(235, 50);
      this.chkRolePremium.Name = "chkRolePremium";
      this.chkRolePremium.Size = new System.Drawing.Size(74, 19);
      this.chkRolePremium.TabIndex = 2;
      this.chkRolePremium.Text = "Premium";
      this.chkRolePremium.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
      this.chkRolePremium.UncheckedState.BorderRadius = 4;
      this.chkRolePremium.UncheckedState.BorderThickness = 1;
      this.chkRolePremium.UncheckedState.FillColor = System.Drawing.Color.White;
      // 
      // chkRoleTester
      // 
      this.chkRoleTester.AutoSize = true;
      this.chkRoleTester.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkRoleTester.CheckedState.BorderRadius = 4;
      this.chkRoleTester.CheckedState.BorderThickness = 0;
      this.chkRoleTester.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkRoleTester.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.chkRoleTester.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.chkRoleTester.Location = new System.Drawing.Point(330, 50);
      this.chkRoleTester.Name = "chkRoleTester";
      this.chkRoleTester.Size = new System.Drawing.Size(57, 19);
      this.chkRoleTester.TabIndex = 3;
      this.chkRoleTester.Text = "Tester";
      this.chkRoleTester.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
      this.chkRoleTester.UncheckedState.BorderRadius = 4;
      this.chkRoleTester.UncheckedState.BorderThickness = 1;
      this.chkRoleTester.UncheckedState.FillColor = System.Drawing.Color.White;
      // 
      // chkRoleBeta
      // 
      this.chkRoleBeta.AutoSize = true;
      this.chkRoleBeta.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkRoleBeta.CheckedState.BorderRadius = 4;
      this.chkRoleBeta.CheckedState.BorderThickness = 0;
      this.chkRoleBeta.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkRoleBeta.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.chkRoleBeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.chkRoleBeta.Location = new System.Drawing.Point(405, 50);
      this.chkRoleBeta.Name = "chkRoleBeta";
      this.chkRoleBeta.Size = new System.Drawing.Size(81, 19);
      this.chkRoleBeta.TabIndex = 4;
      this.chkRoleBeta.Text = "Betatester";
      this.chkRoleBeta.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
      this.chkRoleBeta.UncheckedState.BorderRadius = 4;
      this.chkRoleBeta.UncheckedState.BorderThickness = 1;
      this.chkRoleBeta.UncheckedState.FillColor = System.Drawing.Color.White;
      // 
      // chkRoleUser
      // 
      this.chkRoleUser.AutoSize = true;
      this.chkRoleUser.Checked = true;
      this.chkRoleUser.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkRoleUser.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkRoleUser.CheckedState.BorderRadius = 4;
      this.chkRoleUser.CheckedState.BorderThickness = 0;
      this.chkRoleUser.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkRoleUser.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.chkRoleUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.chkRoleUser.Location = new System.Drawing.Point(500, 50);
      this.chkRoleUser.Name = "chkRoleUser";
      this.chkRoleUser.Size = new System.Drawing.Size(49, 19);
      this.chkRoleUser.TabIndex = 5;
      this.chkRoleUser.Text = "User";
      this.chkRoleUser.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
      this.chkRoleUser.UncheckedState.BorderRadius = 4;
      this.chkRoleUser.UncheckedState.BorderThickness = 1;
      this.chkRoleUser.UncheckedState.FillColor = System.Drawing.Color.White;
      // 
      // txtCustomRole
      // 
      this.txtCustomRole.BorderRadius = 6;
      this.txtCustomRole.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtCustomRole.DefaultText = "";
      this.txtCustomRole.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtCustomRole.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.txtCustomRole.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtCustomRole.Location = new System.Drawing.Point(15, 85);
      this.txtCustomRole.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtCustomRole.Name = "txtCustomRole";
      this.txtCustomRole.PlaceholderText = "Ou digite roles personalizadas separadas por vírgula (ex: admin,moderator,tester)";
      this.txtCustomRole.SelectedText = "";
      this.txtCustomRole.Size = new System.Drawing.Size(550, 36);
      this.txtCustomRole.TabIndex = 6;
      // 
      // btnSalvar
      // 
      this.btnSalvar.BorderRadius = 8;
      this.btnSalvar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnSalvar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.btnSalvar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
      this.btnSalvar.ForeColor = System.Drawing.Color.White;
      this.btnSalvar.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(56)))), ((int)(((byte)(202)))));
      this.btnSalvar.Location = new System.Drawing.Point(345, 505);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(130, 45);
      this.btnSalvar.TabIndex = 9;
      this.btnSalvar.Text = "SALVAR";
      this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
      // 
      // btnCancelar
      // 
      this.btnCancelar.BorderRadius = 8;
      this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnCancelar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.btnCancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
      this.btnCancelar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
      this.btnCancelar.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(232)))), ((int)(((byte)(240)))));
      this.btnCancelar.Location = new System.Drawing.Point(485, 505);
      this.btnCancelar.Name = "btnCancelar";
      this.btnCancelar.Size = new System.Drawing.Size(130, 45);
      this.btnCancelar.TabIndex = 10;
      this.btnCancelar.Text = "CANCELAR";
      this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
      // 
      // FrmUsuarioMockCadastro
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size(650, 570);
      this.Controls.Add(this.lblAvisoSenha);
      this.Controls.Add(this.btnToggleSenha);
      this.Controls.Add(this.lblStatus);
      this.Controls.Add(this.cmbStatus);
      this.Controls.Add(this.grpRoles);
      this.Controls.Add(this.btnCancelar);
      this.Controls.Add(this.btnSalvar);
      this.Controls.Add(this.txtBio);
      this.Controls.Add(this.txtSenha);
      this.Controls.Add(this.txtTelefone);
      this.Controls.Add(this.txtEmail);
      this.Controls.Add(this.txtNome);
      this.Controls.Add(this.txtUsername);
      this.Controls.Add(this.lblSubInfo);
      this.Controls.Add(this.lblOperacao);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "FrmUsuarioMockCadastro";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Manter Usuário";
      this.grpRoles.ResumeLayout(false);
      this.grpRoles.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
