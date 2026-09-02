namespace RedeSocialEventosAdmin.Forms
{
  partial class FrmComunidadeModal
  {
    private System.ComponentModel.IContainer components = null;
    private Guna.UI2.WinForms.Guna2Elipse guna2ElipseForm;
    private System.Windows.Forms.Label lblOperacao;
    private System.Windows.Forms.Label lblSubInfo;
    private Guna.UI2.WinForms.Guna2TextBox txtNome;
    private Guna.UI2.WinForms.Guna2TextBox txtCategoria;
    private Guna.UI2.WinForms.Guna2TextBox txtCor;
    private Guna.UI2.WinForms.Guna2TextBox txtDescricao;
    private Guna.UI2.WinForms.Guna2TextBox txtImagem;
    private Guna.UI2.WinForms.Guna2Button btnSalvar;
    private Guna.UI2.WinForms.Guna2Button btnCancelar;
    private System.Windows.Forms.Panel pnlCorPreview;
    private System.Windows.Forms.Label lblCor;

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
      this.txtNome = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtCategoria = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtCor = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtDescricao = new Guna.UI2.WinForms.Guna2TextBox();
      this.txtImagem = new Guna.UI2.WinForms.Guna2TextBox();
      this.btnSalvar = new Guna.UI2.WinForms.Guna2Button();
      this.btnCancelar = new Guna.UI2.WinForms.Guna2Button();
      this.pnlCorPreview = new System.Windows.Forms.Panel();
      this.lblCor = new System.Windows.Forms.Label();
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
      this.lblOperacao.Size = new System.Drawing.Size(288, 30);
      this.lblOperacao.TabIndex = 0;
      this.lblOperacao.Text = "Manutenção de Comunidade";
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
      this.lblSubInfo.Text = "Defina os dados estruturais e identidade visual da comunidade.";
      // 
      // txtNome
      // 
      this.txtNome.BorderRadius = 8;
      this.txtNome.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtNome.DefaultText = "";
      this.txtNome.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtNome.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtNome.Location = new System.Drawing.Point(35, 90);
      this.txtNome.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtNome.Name = "txtNome";
      this.txtNome.PlaceholderText = "Nome da Comunidade *";
      this.txtNome.SelectedText = "";
      this.txtNome.Size = new System.Drawing.Size(350, 42);
      this.txtNome.TabIndex = 1;
      // 
      // txtCategoria
      // 
      this.txtCategoria.BorderRadius = 8;
      this.txtCategoria.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtCategoria.DefaultText = "";
      this.txtCategoria.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtCategoria.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtCategoria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtCategoria.Location = new System.Drawing.Point(400, 90);
      this.txtCategoria.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtCategoria.Name = "txtCategoria";
      this.txtCategoria.PlaceholderText = "Categoria (ex: Games, Música)";
      this.txtCategoria.SelectedText = "";
      this.txtCategoria.Size = new System.Drawing.Size(220, 42);
      this.txtCategoria.TabIndex = 2;
      // 
      // lblCor
      // 
      this.lblCor.AutoSize = true;
      this.lblCor.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
      this.lblCor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
      this.lblCor.Location = new System.Drawing.Point(35, 142);
      this.lblCor.Name = "lblCor";
      this.lblCor.Size = new System.Drawing.Size(107, 15);
      this.lblCor.TabIndex = 15;
      this.lblCor.Text = "Cor Hexadecimal:";
      // 
      // txtCor
      // 
      this.txtCor.BorderRadius = 8;
      this.txtCor.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtCor.DefaultText = "#EA3F74";
      this.txtCor.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtCor.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtCor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtCor.Location = new System.Drawing.Point(35, 160);
      this.txtCor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtCor.Name = "txtCor";
      this.txtCor.PlaceholderText = "#EA3F74";
      this.txtCor.SelectedText = "";
      this.txtCor.Size = new System.Drawing.Size(150, 42);
      this.txtCor.TabIndex = 3;
      this.txtCor.TextChanged += new System.EventHandler(this.txtCor_TextChanged);
      // 
      // pnlCorPreview
      // 
      this.pnlCorPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(63)))), ((int)(((byte)(116)))));
      this.pnlCorPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pnlCorPreview.Location = new System.Drawing.Point(195, 160);
      this.pnlCorPreview.Name = "pnlCorPreview";
      this.pnlCorPreview.Size = new System.Drawing.Size(42, 42);
      this.pnlCorPreview.TabIndex = 16;
      // 
      // txtImagem
      // 
      this.txtImagem.BorderRadius = 8;
      this.txtImagem.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtImagem.DefaultText = "";
      this.txtImagem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtImagem.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtImagem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtImagem.Location = new System.Drawing.Point(250, 160);
      this.txtImagem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtImagem.Name = "txtImagem";
      this.txtImagem.PlaceholderText = "URL do Banner / Logo (Opcional)";
      this.txtImagem.SelectedText = "";
      this.txtImagem.Size = new System.Drawing.Size(370, 42);
      this.txtImagem.TabIndex = 4;
      // 
      // txtDescricao
      // 
      this.txtDescricao.BorderRadius = 8;
      this.txtDescricao.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.txtDescricao.DefaultText = "";
      this.txtDescricao.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(102)))), ((int)(((byte)(241)))));
      this.txtDescricao.Font = new System.Drawing.Font("Segoe UI", 9.5F);
      this.txtDescricao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
      this.txtDescricao.Location = new System.Drawing.Point(35, 215);
      this.txtDescricao.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtDescricao.Multiline = true;
      this.txtDescricao.Name = "txtDescricao";
      this.txtDescricao.PlaceholderText = "Descrição e Regras da Comunidade...";
      this.txtDescricao.SelectedText = "";
      this.txtDescricao.Size = new System.Drawing.Size(585, 100);
      this.txtDescricao.TabIndex = 5;
      // 
      // btnSalvar
      // 
      this.btnSalvar.BorderRadius = 8;
      this.btnSalvar.Cursor = System.Windows.Forms.Cursors.Hand;
      this.btnSalvar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(70)))), ((int)(((byte)(229)))));
      this.btnSalvar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
      this.btnSalvar.ForeColor = System.Drawing.Color.White;
      this.btnSalvar.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(56)))), ((int)(((byte)(202)))));
      this.btnSalvar.Location = new System.Drawing.Point(350, 335);
      this.btnSalvar.Name = "btnSalvar";
      this.btnSalvar.Size = new System.Drawing.Size(130, 45);
      this.btnSalvar.TabIndex = 6;
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
      this.btnCancelar.Location = new System.Drawing.Point(490, 335);
      this.btnCancelar.Name = "btnCancelar";
      this.btnCancelar.Size = new System.Drawing.Size(130, 45);
      this.btnCancelar.TabIndex = 7;
      this.btnCancelar.Text = "CANCELAR";
      this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
      // 
      // FrmComunidadeModal
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size(655, 400);
      this.Controls.Add(this.btnCancelar);
      this.Controls.Add(this.btnSalvar);
      this.Controls.Add(this.txtDescricao);
      this.Controls.Add(this.txtImagem);
      this.Controls.Add(this.pnlCorPreview);
      this.Controls.Add(this.txtCor);
      this.Controls.Add(this.lblCor);
      this.Controls.Add(this.txtCategoria);
      this.Controls.Add(this.txtNome);
      this.Controls.Add(this.lblSubInfo);
      this.Controls.Add(this.lblOperacao);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "FrmComunidadeModal";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Manter Comunidade";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
