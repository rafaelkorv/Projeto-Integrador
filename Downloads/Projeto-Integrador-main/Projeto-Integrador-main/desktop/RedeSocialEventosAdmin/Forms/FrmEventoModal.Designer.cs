namespace RedeSocialEventosAdmin.Forms
{
  partial class FrmEventoModal
  {
    private System.ComponentModel.IContainer components = null;
    private Guna.UI2.WinForms.Guna2Elipse guna2ElipseForm;
    private System.Windows.Forms.Label lblOperacao;
    private System.Windows.Forms.Label lblSubInfo;
    private Guna.UI2.WinForms.Guna2TextBox txtTitulo;
    private Guna.UI2.WinForms.Guna2TextBox txtCategoria;
    private Guna.UI2.WinForms.Guna2DateTimePicker dtpData;
    private Guna.UI2.WinForms.Guna2TextBox txtHorarioInicio;
    private Guna.UI2.WinForms.Guna2TextBox txtHorarioFim;
    private Guna.UI2.WinForms.Guna2TextBox txtLocal;
    private Guna.UI2.WinForms.Guna2TextBox txtLimite;
    private Guna.UI2.WinForms.Guna2ComboBox cmbComunidade;
    private Guna.UI2.WinForms.Guna2ComboBox cmbStatus;
    private Guna.UI2.WinForms.Guna2CheckBox chkCheckin;
    private Guna.UI2.WinForms.Guna2TextBox txtDescricao;
    private Guna.UI2.WinForms.Guna2TextBox txtImagemCapa;
    private Guna.UI2.WinForms.Guna2Button btnSalvar;
    private Guna.UI2.WinForms.Guna2Button btnCancelar;
    private System.Windows.Forms.Label lblData;
    private System.Windows.Forms.Label lblComunidade;
    private System.Windows.Forms.Label lblStatus;

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
      this.txtTitulo = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtCategoria = new Guna.UI2.WinForms.Guna2TextBox();
      this.dtpData = new Guna.UI2.WinForms.Guna2DateTimePicker();
      this.txtHorarioInicio = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtHorarioFim = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtLocal = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtLimite = new Guna.UI2.WinForms.Guna2TextBox();
      this.cmbComunidade = new Guna.UI2.WinForms.Guna2ComboBox();
      this.cmbStatus = new Guna.UI2.WinForms.Guna2ComboBox();
      this.chkCheckin = new Guna.UI2.WinForms.Guna2CheckBox();
      this.txtDescricao = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtImagemCapa = new Guna.UI2.WinForms.Guna2TextBox();
      this.btnSalvar = new Guna.UI2.WinForms.Guna2Button();
      this.btnCancelar = new Guna.UI2.WinForms.Guna2Button();
      this.lblData = new System.Windows.Forms.Label();
      this.lblComunidade = new System.Windows.Forms.Label();
      this.lblStatus = new System.Windows.Forms.Label();
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
      this.lblOperacao.Size = new System.Drawing.Size(250, 30);
      this.lblOperacao.TabIndex = 0;
      this.lblOperacao.Text = "Manutenção de Evento";
      // 
      // lblSubInfo
      // 
      this.lblSubInfo.AutoSize = true;
      this.lblSubInfo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.lblSubInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
      this.lblSubInfo.Location = new System.Drawing.Point(32, 53);
      this.lblSubInfo.Name = "lblSubInfo";
      this.lblSubInfo.Size = new System.Drawing.Size(390, 17);
      this.lblSubInfo.TabIndex = 1;
      this.lblSubInfo.Text = "Preencha as informações detalhadas para publicação do evento.";
      // 
      // txtTitulo
      // 
      this.txtTitulo.BorderRadius = 8;
      this.txtTitulo.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtTitulo.DefaultText = "";
      this.txtTitulo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtTitulo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtTitulo.Location = new System.Drawing.Point(35, 90);
      this.txtTitulo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtTitulo.Name = "txtTitulo";
      this.txtTitulo.PlaceholderText = "Título do Evento *";
      this.txtTitulo.SelectedText = "";
      this.txtTitulo.Size = new System.Drawing.Size(380, 42);
      this.txtTitulo.TabIndex = 1;
      // 
      // txtCategoria
      // 
      this.txtCategoria.BorderRadius = 8;
      this.txtCategoria.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtCategoria.DefaultText = "";
      this.txtCategoria.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtCategoria.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtCategoria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtCategoria.Location = new System.Drawing.Point(435, 90);
      this.txtCategoria.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtCategoria.Name = "txtCategoria";
      this.txtCategoria.PlaceholderText = "Categoria (ex: Música, Tecnologia, Esportes)";
      this.txtCategoria.SelectedText = "";
      this.txtCategoria.Size = new System.Drawing.Size(240, 42);
      this.txtCategoria.TabIndex = 2;
      // 
      // lblData
      // 
      this.lblData.AutoSize = true;
      this.lblData.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
      this.lblData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.lblData.Location = new System.Drawing.Point(35, 142);
      this.lblData.Name = "lblData";
      this.lblData.Size = new System.Drawing.Size(94, 15);
      this.lblData.TabIndex = 15;
      this.lblData.Text = "Data do Evento:";
      // 
      // dtpData
      // 
      this.dtpData.BorderRadius = 8;
      this.dtpData.Checked = true;
      this.dtpData.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
      this.dtpData.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.dtpData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.dtpData.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtpData.Location = new System.Drawing.Point(35, 160);
      this.dtpData.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
      this.dtpData.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
      this.dtpData.Name = "dtpData";
      this.dtpData.Size = new System.Drawing.Size(180, 42);
      this.dtpData.TabIndex = 3;
      this.dtpData.Value = System.DateTime.Now;
      // 
      // txtHorarioInicio
      // 
      this.txtHorarioInicio.BorderRadius = 8;
      this.txtHorarioInicio.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtHorarioInicio.DefaultText = "";
      this.txtHorarioInicio.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtHorarioInicio.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtHorarioInicio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtHorarioInicio.Location = new System.Drawing.Point(235, 160);
      this.txtHorarioInicio.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtHorarioInicio.Name = "txtHorarioInicio";
      this.txtHorarioInicio.PlaceholderText = "Início (ex: 19:00)";
      this.txtHorarioInicio.SelectedText = "";
      this.txtHorarioInicio.Size = new System.Drawing.Size(130, 42);
      this.txtHorarioInicio.TabIndex = 4;
      // 
      // txtHorarioFim
      // 
      this.txtHorarioFim.BorderRadius = 8;
      this.txtHorarioFim.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtHorarioFim.DefaultText = "";
      this.txtHorarioFim.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtHorarioFim.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtHorarioFim.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtHorarioFim.Location = new System.Drawing.Point(385, 160);
      this.txtHorarioFim.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtHorarioFim.Name = "txtHorarioFim";
      this.txtHorarioFim.PlaceholderText = "Fim (ex: 22:00)";
      this.txtHorarioFim.SelectedText = "";
      this.txtHorarioFim.Size = new System.Drawing.Size(130, 42);
      this.txtHorarioFim.TabIndex = 5;
      // 
      // txtLimite
      // 
      this.txtLimite.BorderRadius = 8;
      this.txtLimite.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtLimite.DefaultText = "";
      this.txtLimite.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtLimite.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtLimite.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtLimite.Location = new System.Drawing.Point(535, 160);
      this.txtLimite.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtLimite.Name = "txtLimite";
      this.txtLimite.PlaceholderText = "Limite Vagas";
      this.txtLimite.SelectedText = "";
      this.txtLimite.Size = new System.Drawing.Size(140, 42);
      this.txtLimite.TabIndex = 6;
      // 
      // txtLocal
      // 
      this.txtLocal.BorderRadius = 8;
      this.txtLocal.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtLocal.DefaultText = "";
      this.txtLocal.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtLocal.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtLocal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtLocal.Location = new System.Drawing.Point(35, 215);
      this.txtLocal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtLocal.Name = "txtLocal";
      this.txtLocal.PlaceholderText = "Local do Evento (Endereço, Sala ou Link) *";
      this.txtLocal.SelectedText = "";
      this.txtLocal.Size = new System.Drawing.Size(640, 42);
      this.txtLocal.TabIndex = 7;
      // 
      // lblComunidade
      // 
      this.lblComunidade.AutoSize = true;
      this.lblComunidade.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
      this.lblComunidade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.lblComunidade.Location = new System.Drawing.Point(35, 268);
      this.lblComunidade.Name = "lblComunidade";
      this.lblComunidade.Size = new System.Drawing.Size(134, 15);
      this.lblComunidade.TabIndex = 16;
      this.lblComunidade.Text = "Comunidade Vinculada:";
      // 
      // cmbComunidade
      // 
      this.cmbComunidade.BackColor = System.Drawing.Color.Transparent;
      this.cmbComunidade.BorderRadius = 8;
      this.cmbComunidade.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
      this.cmbComunidade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbComunidade.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.cmbComunidade.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.cmbComunidade.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.cmbComunidade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.cmbComunidade.ItemHeight = 32;
      this.cmbComunidade.Location = new System.Drawing.Point(35, 287);
      this.cmbComunidade.Name = "cmbComunidade";
      this.cmbComunidade.Size = new System.Drawing.Size(300, 38);
      this.cmbComunidade.TabIndex = 8;
      // 
      // lblStatus
      // 
      this.lblStatus.AutoSize = true;
      this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
      this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.lblStatus.Location = new System.Drawing.Point(355, 268);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new System.Drawing.Size(100, 15);
      this.lblStatus.TabIndex = 17;
      this.lblStatus.Text = "Status do Evento:";
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
      this.cmbStatus.ItemHeight = 32;
      this.cmbStatus.Items.AddRange(new object[] {
      "AGENDADO",
      "ACONTECENDO_AGORA",
      "ENCERRADO",
      "CANCELADO"});
      this.cmbStatus.Location = new System.Drawing.Point(355, 287);
      this.cmbStatus.Name = "cmbStatus";
      this.cmbStatus.Size = new System.Drawing.Size(180, 38);
      this.cmbStatus.StartIndex = 0;
      this.cmbStatus.TabIndex = 9;
      // 
      // chkCheckin
      // 
      this.chkCheckin.AutoSize = true;
      this.chkCheckin.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkCheckin.CheckedState.BorderRadius = 4;
      this.chkCheckin.CheckedState.BorderThickness = 0;
      this.chkCheckin.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.chkCheckin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.chkCheckin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.chkCheckin.Location = new System.Drawing.Point(555, 298);
      this.chkCheckin.Name = "chkCheckin";
      this.chkCheckin.Size = new System.Drawing.Size(107, 19);
      this.chkCheckin.TabIndex = 10;
      this.chkCheckin.Text = "Exigir Check-in";
      this.chkCheckin.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
      this.chkCheckin.UncheckedState.BorderRadius = 4;
      this.chkCheckin.UncheckedState.BorderThickness = 1;
      this.chkCheckin.UncheckedState.FillColor = System.Drawing.Color.White;
      // 
      // txtDescricao
      // 
      this.txtDescricao.BorderRadius = 8;
      this.txtDescricao.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtDescricao.DefaultText = "";
      this.txtDescricao.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtDescricao.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtDescricao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtDescricao.Location = new System.Drawing.Point(35, 340);
      this.txtDescricao.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtDescricao.Multiline = true;
      this.txtDescricao.Name = "txtDescricao";
      this.txtDescricao.PlaceholderText = "Descrição Completa do Evento...";
      this.txtDescricao.SelectedText = "";
      this.txtDescricao.Size = new System.Drawing.Size(640, 80);
      this.txtDescricao.TabIndex = 11;
      // 
      // txtImagemCapa
      // 
      this.txtImagemCapa.BorderRadius = 8;
      this.txtImagemCapa.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtImagemCapa.DefaultText = "";
      this.txtImagemCapa.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtImagemCapa.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtImagemCapa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtImagemCapa.Location = new System.Drawing.Point(35, 430);
      this.txtImagemCapa.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtImagemCapa.Name = "txtImagemCapa";
      this.txtImagemCapa.PlaceholderText = "URL da Imagem de Capa (Opcional)";
      this.txtImagemCapa.SelectedText = "";
      this.txtImagemCapa.Size = new System.Drawing.Size(640, 42);
      this.txtImagemCapa.TabIndex = 12;
      // 
      // btnSalvar
      // 
      this.btnSalvar.BorderRadius = 8;
      this.btnSalvar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnSalvar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.btnSalvar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
      this.btnSalvar.ForeColor = System.Drawing.Color.White;
      this.btnSalvar.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(56)))), ((int)(((byte)(202)))));
      this.btnSalvar.Location = new System.Drawing.Point(405, 495);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(130, 45);
      this.btnSalvar.TabIndex = 13;
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
      this.btnCancelar.Location = new System.Drawing.Point(545, 495);
      this.btnCancelar.Name = "btnCancelar";
      this.btnCancelar.Size = new System.Drawing.Size(130, 45);
      this.btnCancelar.TabIndex = 14;
      this.btnCancelar.Text = "CANCELAR";
      this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
      // 
      // FrmEventoModal
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size(710, 560);
      this.Controls.Add(this.btnCancelar);
      this.Controls.Add(this.btnSalvar);
      this.Controls.Add(this.txtImagemCapa);
      this.Controls.Add(this.txtDescricao);
      this.Controls.Add(this.chkCheckin);
      this.Controls.Add(this.lblStatus);
      this.Controls.Add(this.cmbStatus);
      this.Controls.Add(this.lblComunidade);
      this.Controls.Add(this.cmbComunidade);
      this.Controls.Add(this.txtLocal);
      this.Controls.Add(this.txtLimite);
      this.Controls.Add(this.txtHorarioFim);
      this.Controls.Add(this.txtHorarioInicio);
      this.Controls.Add(this.dtpData);
      this.Controls.Add(this.lblData);
      this.Controls.Add(this.txtCategoria);
      this.Controls.Add(this.txtTitulo);
      this.Controls.Add(this.lblSubInfo);
      this.Controls.Add(this.lblOperacao);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "FrmEventoModal";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Manter Evento";
      this.Load += new System.EventHandler(this.FrmEventoModal_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
