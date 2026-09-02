using System;
using System.Data;
using System.Windows.Forms;
using RedeSocialEventosAdmin.DAO;
using RedeSocialEventosAdmin.Models;

namespace RedeSocialEventosAdmin.Forms
{
  public partial class FrmEventoModal : Form
  {
    private readonly EventoDAO _eventoDAO;
    private readonly ComunidadeDAO _comunidadeDAO;
    private readonly Evento? _eventoExistente;
    private readonly bool _isEdicao;

    public FrmEventoModal()
    {
      InitializeComponent();
      _eventoDAO = new EventoDAO();
      _comunidadeDAO = new ComunidadeDAO();
      _isEdicao = false;
      _eventoExistente = null;
      lblOperacao.Text = "Criar Novo Evento";
    }

    public FrmEventoModal(Evento evento)
    {
      InitializeComponent();
      _eventoDAO = new EventoDAO();
      _comunidadeDAO = new ComunidadeDAO();
      _eventoExistente = evento;
      _isEdicao = true;
      lblOperacao.Text = $"Editar Evento #{evento.IdEvento}";
    }

    private void FrmEventoModal_Load(object sender, EventArgs e)
    {
      CarregarComunidades();
      if (_isEdicao && _eventoExistente != null)
      {
        PreencherCampos();
      }
      else
      {
        dtpData.Value = DateTime.Today.AddDays(7);
        txtHorarioInicio.Text = "19:00";
        txtHorarioFim.Text = "22:00";
        cmbStatus.SelectedItem = "AGENDADO";
      }
    }

    private void CarregarComunidades()
    {
      try
      {
        DataTable dt = _comunidadeDAO.ObterComunidadesCombo();
        DataRow dr = dt.NewRow();
        dr["id_comunidade"] = 0;
        dr["nome"] = "Nenhuma (Evento Geral da Rede)";
        dt.Rows.InsertAt(dr, 0);

        cmbComunidade.DataSource = dt;
        cmbComunidade.DisplayMember = "nome";
        cmbComunidade.ValueMember = "id_comunidade";
      }
      catch { }
    }

    private void PreencherCampos()
    {
      if (_eventoExistente == null) return;

      txtTitulo.Text = _eventoExistente.Titulo;
      txtCategoria.Text = _eventoExistente.Categoria ?? "";
      dtpData.Value = _eventoExistente.DataEvento;
      txtHorarioInicio.Text = _eventoExistente.HorarioInicio.ToString(@"hh\:mm");
      txtHorarioFim.Text = _eventoExistente.HorarioFim.HasValue ? _eventoExistente.HorarioFim.Value.ToString(@"hh\:mm") : "";
      txtLocal.Text = _eventoExistente.LocalEvento;
      txtLimite.Text = _eventoExistente.LimiteParticipantes.HasValue ? _eventoExistente.LimiteParticipantes.Value.ToString() : "";
      cmbStatus.SelectedItem = _eventoExistente.Status;
      chkCheckin.Checked = _eventoExistente.ExigeCheckin;
      txtDescricao.Text = _eventoExistente.Descricao ?? "";
      txtImagemCapa.Text = _eventoExistente.ImagemCapa ?? "";

      if (_eventoExistente.ComunidadeId.HasValue && _eventoExistente.ComunidadeId.Value > 0)
      {
        cmbComunidade.SelectedValue = _eventoExistente.ComunidadeId.Value;
      }
      else
      {
        cmbComunidade.SelectedIndex = 0;
      }
    }

    private void btnSalvar_Click(object sender, EventArgs e)
    {
      string titulo = txtTitulo.Text.Trim();
      string local = txtLocal.Text.Trim();
      string categoria = txtCategoria.Text.Trim();
      string desc = txtDescricao.Text.Trim();
      string capa = txtImagemCapa.Text.Trim();
      string status = cmbStatus.SelectedItem?.ToString() ?? "AGENDADO";

      if (string.IsNullOrWhiteSpace(titulo))
      {
        MessageBox.Show("O título do evento é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtTitulo.Focus();
        return;
      }

      if (string.IsNullOrWhiteSpace(local))
      {
        MessageBox.Show("O local do evento é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtLocal.Focus();
        return;
      }

      if (!TimeSpan.TryParse(txtHorarioInicio.Text.Trim(), out TimeSpan inicio))
      {
        MessageBox.Show("Informe um horário de início válido no formato HH:mm.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        txtHorarioInicio.Focus();
        return;
      }

      TimeSpan? fim = null;
      if (!string.IsNullOrWhiteSpace(txtHorarioFim.Text))
      {
        if (TimeSpan.TryParse(txtHorarioFim.Text.Trim(), out TimeSpan fimVal))
        {
          fim = fimVal;
        }
      }

      int? limite = null;
      if (!string.IsNullOrWhiteSpace(txtLimite.Text) && int.TryParse(txtLimite.Text.Trim(), out int limVal))
      {
        limite = limVal;
      }

      long? comunidadeId = null;
      if (cmbComunidade.SelectedValue != null && Convert.ToInt64(cmbComunidade.SelectedValue) > 0)
      {
        comunidadeId = Convert.ToInt64(cmbComunidade.SelectedValue);
      }

      try
      {
        if (_isEdicao && _eventoExistente != null)
        {
          _eventoExistente.Titulo = titulo;
          _eventoExistente.Categoria = categoria;
          _eventoExistente.DataEvento = dtpData.Value.Date;
          _eventoExistente.HorarioInicio = inicio;
          _eventoExistente.HorarioFim = fim;
          _eventoExistente.LocalEvento = local;
          _eventoExistente.LimiteParticipantes = limite;
          _eventoExistente.ComunidadeId = comunidadeId;
          _eventoExistente.Status = status;
          _eventoExistente.ExigeCheckin = chkCheckin.Checked;
          _eventoExistente.Descricao = desc;
          _eventoExistente.ImagemCapa = capa;

          if (_eventoDAO.Atualizar(_eventoExistente))
          {
            MessageBox.Show("Evento atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
          }
        }
        else
        {
          long adminId = Program.UsuarioLogado?.IdUsuario ?? 1;

          Evento novo = new Evento
          {
            Titulo = titulo,
            Categoria = categoria,
            DataEvento = dtpData.Value.Date,
            HorarioInicio = inicio,
            HorarioFim = fim,
            LocalEvento = local,
            LimiteParticipantes = limite,
            ComunidadeId = comunidadeId,
            CriadorId = adminId,
            Status = status,
            ExigeCheckin = chkCheckin.Checked,
            Descricao = desc,
            ImagemCapa = capa
          };

          if (_eventoDAO.Inserir(novo))
          {
            MessageBox.Show("Evento criado e publicado com sucesso no sistema!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Erro ao salvar evento: {ex.Message}", "Falha", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }
  }
}
