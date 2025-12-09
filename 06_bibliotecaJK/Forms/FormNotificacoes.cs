using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BibliotecaJK.DAL;
using BibliotecaJK.Model;
using BibliotecaJK;

namespace BibliotecaJK.Forms
{
    /// <summary>
    /// Centro de Notificacoes - Interface moderna para visualizar e gerenciar notificacoes
    /// Layout responsivo com Dock e Anchor
    /// </summary>
    public partial class FormNotificacoes : Form
    {
        private readonly NotificacaoDAL _notificacaoDAL;
        private DataGridView dgvNotificacoes = new DataGridView();
        private ComboBox cboFiltroTipo = new ComboBox();
        private ComboBox cboFiltroPrioridade = new ComboBox();
        private ComboBox cboFiltroStatus = new ComboBox();
        private Button btnMarcarLida = new Button();
        private Button btnMarcarTodasLidas = new Button();
        private Button btnExcluir = new Button();
        private Button btnAtualizar = new Button();
        private Button btnFechar = new Button();
        private Label lblTotal = new Label();
        private Label lblNaoLidas = new Label();
        private System.Windows.Forms.Timer timerAtualizacao = new System.Windows.Forms.Timer();

        public FormNotificacoes()
        {
            _notificacaoDAL = new NotificacaoDAL();
            InitializeComponent();
            CarregarNotificacoes();

            // Auto-refresh a cada 30 segundos
            timerAtualizacao.Interval = 30000;
            timerAtualizacao.Tick += (s, e) => CarregarNotificacoes();
            timerAtualizacao.Start();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // FormNotificacoes - Responsivo
            this.ClientSize = new Size(1100, 700);
            this.MinimumSize = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Central de Notificacoes - BibliotecaJK";
            this.BackColor = Color.FromArgb(245, 245, 250);

            // ==================== HEADER (Dock.Top) ====================
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(63, 81, 181)
            };

            var lblTitulo = new Label
            {
                Text = "CENTRAL DE NOTIFICACOES",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            pnlHeader.Controls.Add(lblTitulo);

            lblNaoLidas = new Label
            {
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 50),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            pnlHeader.Controls.Add(lblNaoLidas);

            lblTotal = new Label
            {
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(200, 200, 255),
                Location = new Point(330, 50),
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            pnlHeader.Controls.Add(lblTotal);

            // ==================== ACOES (Dock.Bottom) ====================
            var pnlAcoes = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };

            // FlowLayoutPanel para botoes de acao (alinhados a esquerda)
            var flowBotoesAcao = new FlowLayoutPanel
            {
                Dock = DockStyle.Left,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 2, 0, 0)
            };

            btnMarcarLida = new Button
            {
                Text = "Marcar como Lida",
                Size = new Size(160, 35),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false,
                Margin = new Padding(0, 0, 10, 0)
            };
            btnMarcarLida.FlatAppearance.BorderSize = 0;
            btnMarcarLida.Click += BtnMarcarLida_Click;
            flowBotoesAcao.Controls.Add(btnMarcarLida);

            btnMarcarTodasLidas = new Button
            {
                Text = "Marcar Todas como Lidas",
                Size = new Size(180, 35),
                BackColor = Color.FromArgb(139, 195, 74),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 10, 0)
            };
            btnMarcarTodasLidas.FlatAppearance.BorderSize = 0;
            btnMarcarTodasLidas.Click += BtnMarcarTodasLidas_Click;
            flowBotoesAcao.Controls.Add(btnMarcarTodasLidas);

            btnExcluir = new Button
            {
                Text = "Excluir",
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Enabled = false,
                Margin = new Padding(0, 0, 10, 0)
            };
            btnExcluir.FlatAppearance.BorderSize = 0;
            btnExcluir.Click += BtnExcluir_Click;
            flowBotoesAcao.Controls.Add(btnExcluir);

            pnlAcoes.Controls.Add(flowBotoesAcao);

            // Botao Fechar (ancorado a direita)
            btnFechar = new Button
            {
                Text = "Fechar",
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnFechar.FlatAppearance.BorderSize = 0;
            btnFechar.Click += (s, e) => this.Close();
            btnFechar.Location = new Point(pnlAcoes.Width - btnFechar.Width - 15, 12);
            pnlAcoes.Controls.Add(btnFechar);

            // ==================== FILTROS (Dock.Top) ====================
            var pnlFiltros = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };

            var lblFiltros = new Label
            {
                Text = "Filtros:",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Location = new Point(10, 18),
                AutoSize = true
            };
            pnlFiltros.Controls.Add(lblFiltros);

            // Filtro Status
            var lblStatus = new Label
            {
                Text = "Status:",
                Location = new Point(70, 18),
                AutoSize = true
            };
            pnlFiltros.Controls.Add(lblStatus);

            cboFiltroStatus = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(125, 15),
                Size = new Size(130, 25)
            };
            cboFiltroStatus.Items.AddRange(new object[] { "Todas", "Nao Lidas", "Lidas" });
            cboFiltroStatus.SelectedIndex = 0;
            cboFiltroStatus.SelectedIndexChanged += (s, e) => CarregarNotificacoes();
            pnlFiltros.Controls.Add(cboFiltroStatus);

            // Filtro Tipo
            var lblTipo = new Label
            {
                Text = "Tipo:",
                Location = new Point(270, 18),
                AutoSize = true
            };
            pnlFiltros.Controls.Add(lblTipo);

            cboFiltroTipo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(310, 15),
                Size = new Size(180, 25)
            };
            cboFiltroTipo.Items.AddRange(new object[] {
                "Todos os Tipos",
                "Emprestimo Atrasado",
                "Reserva Expirada",
                "Livro Disponivel"
            });
            cboFiltroTipo.SelectedIndex = 0;
            cboFiltroTipo.SelectedIndexChanged += (s, e) => CarregarNotificacoes();
            pnlFiltros.Controls.Add(cboFiltroTipo);

            // Filtro Prioridade
            var lblPrioridade = new Label
            {
                Text = "Prioridade:",
                Location = new Point(510, 18),
                AutoSize = true
            };
            pnlFiltros.Controls.Add(lblPrioridade);

            cboFiltroPrioridade = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(580, 15),
                Size = new Size(110, 25)
            };
            cboFiltroPrioridade.Items.AddRange(new object[] {
                "Todas",
                "Urgente",
                "Alta",
                "Normal",
                "Baixa"
            });
            cboFiltroPrioridade.SelectedIndex = 0;
            cboFiltroPrioridade.SelectedIndexChanged += (s, e) => CarregarNotificacoes();
            pnlFiltros.Controls.Add(cboFiltroPrioridade);

            // Botao Atualizar (ancorado a direita)
            btnAtualizar = new Button
            {
                Text = "Atualizar",
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnAtualizar.FlatAppearance.BorderSize = 0;
            btnAtualizar.Click += (s, e) => CarregarNotificacoes();
            btnAtualizar.Location = new Point(pnlFiltros.Width - btnAtualizar.Width - 15, 12);
            pnlFiltros.Controls.Add(btnAtualizar);

            // ==================== DATAGRIDVIEW (Dock.Fill) ====================
            dgvNotificacoes = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9F),
                ColumnHeadersHeight = 40
            };

            dgvNotificacoes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(63, 81, 181);
            dgvNotificacoes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvNotificacoes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvNotificacoes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvNotificacoes.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);

            dgvNotificacoes.EnableHeadersVisualStyles = false;
            dgvNotificacoes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvNotificacoes.RowTemplate.Height = 50;

            dgvNotificacoes.CellFormatting += DgvNotificacoes_CellFormatting;
            dgvNotificacoes.SelectionChanged += DgvNotificacoes_SelectionChanged;

            // ==================== ADICIONAR CONTROLES (ordem inversa para Dock) ====================
            // Importante: Para Dock funcionar corretamente, adicionar na ordem inversa
            // 1. Bottom primeiro
            // 2. Fill segundo (ocupa o resto)
            // 3. Top por ultimo (empilha no topo)
            this.Controls.Add(dgvNotificacoes);  // Fill - fica no meio
            this.Controls.Add(pnlAcoes);          // Bottom
            this.Controls.Add(pnlFiltros);        // Top (depois do header)
            this.Controls.Add(pnlHeader);         // Top (primeiro)

            // Ajustar posicao do botao Fechar apos layout
            this.Load += (s, e) =>
            {
                btnFechar.Location = new Point(pnlAcoes.ClientSize.Width - btnFechar.Width - 15, 12);
                btnAtualizar.Location = new Point(pnlFiltros.ClientSize.Width - btnAtualizar.Width - 15, 12);
            };

            this.Resize += (s, e) =>
            {
                btnFechar.Location = new Point(pnlAcoes.ClientSize.Width - btnFechar.Width - 15, 12);
                btnAtualizar.Location = new Point(pnlFiltros.ClientSize.Width - btnAtualizar.Width - 15, 12);
            };

            this.ResumeLayout(false);
        }

        private void CarregarNotificacoes()
        {
            try
            {
                var todasNotificacoes = _notificacaoDAL.Listar();

                // Aplicar filtros
                var notificacoesFiltradas = todasNotificacoes.AsEnumerable();

                // Filtro de Status
                if (cboFiltroStatus.SelectedIndex == 1) // Nao Lidas
                    notificacoesFiltradas = notificacoesFiltradas.Where(n => !n.Lida);
                else if (cboFiltroStatus.SelectedIndex == 2) // Lidas
                    notificacoesFiltradas = notificacoesFiltradas.Where(n => n.Lida);

                // Filtro de Tipo
                if (cboFiltroTipo.SelectedIndex == 1)
                    notificacoesFiltradas = notificacoesFiltradas.Where(n => n.Tipo == TipoNotificacao.EMPRESTIMO_ATRASADO);
                else if (cboFiltroTipo.SelectedIndex == 2)
                    notificacoesFiltradas = notificacoesFiltradas.Where(n => n.Tipo == TipoNotificacao.RESERVA_EXPIRADA);
                else if (cboFiltroTipo.SelectedIndex == 3)
                    notificacoesFiltradas = notificacoesFiltradas.Where(n => n.Tipo == TipoNotificacao.LIVRO_DISPONIVEL);

                // Filtro de Prioridade
                if (cboFiltroPrioridade.SelectedIndex == 1)
                    notificacoesFiltradas = notificacoesFiltradas.Where(n => n.Prioridade == PrioridadeNotificacao.URGENTE);
                else if (cboFiltroPrioridade.SelectedIndex == 2)
                    notificacoesFiltradas = notificacoesFiltradas.Where(n => n.Prioridade == PrioridadeNotificacao.ALTA);
                else if (cboFiltroPrioridade.SelectedIndex == 3)
                    notificacoesFiltradas = notificacoesFiltradas.Where(n => n.Prioridade == PrioridadeNotificacao.NORMAL);
                else if (cboFiltroPrioridade.SelectedIndex == 4)
                    notificacoesFiltradas = notificacoesFiltradas.Where(n => n.Prioridade == PrioridadeNotificacao.BAIXA);

                var notificacoes = notificacoesFiltradas.ToList();

                // Configurar colunas (apenas na primeira vez)
                if (dgvNotificacoes.Columns.Count == 0)
                {
                    dgvNotificacoes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", HeaderText = "ID", Width = 50, Visible = false });
                    dgvNotificacoes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Prioridade", HeaderText = "Prioridade", Width = 100 });
                    dgvNotificacoes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Tipo", HeaderText = "Tipo", Width = 150 });
                    dgvNotificacoes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Titulo", HeaderText = "Titulo", Width = 250 });
                    dgvNotificacoes.Columns.Add(new DataGridViewTextBoxColumn { Name = "Mensagem", HeaderText = "Mensagem", Width = 350 });
                    dgvNotificacoes.Columns.Add(new DataGridViewTextBoxColumn { Name = "DataCriacao", HeaderText = "Data", Width = 150 });
                    dgvNotificacoes.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Lida", HeaderText = "Lida", Width = 70 });
                }

                dgvNotificacoes.Rows.Clear();

                foreach (var n in notificacoes)
                {
                    string tipoDisplay = n.Tipo switch
                    {
                        TipoNotificacao.EMPRESTIMO_ATRASADO => "Emprestimo Atrasado",
                        TipoNotificacao.RESERVA_EXPIRADA => "Reserva Expirada",
                        TipoNotificacao.LIVRO_DISPONIVEL => "Livro Disponivel",
                        _ => n.Tipo
                    };

                    string prioridadeDisplay = n.Prioridade switch
                    {
                        PrioridadeNotificacao.URGENTE => "URGENTE",
                        PrioridadeNotificacao.ALTA => "Alta",
                        PrioridadeNotificacao.NORMAL => "Normal",
                        PrioridadeNotificacao.BAIXA => "Baixa",
                        _ => n.Prioridade
                    };

                    dgvNotificacoes.Rows.Add(
                        n.Id,
                        prioridadeDisplay,
                        tipoDisplay,
                        n.Titulo,
                        n.Mensagem,
                        n.DataCriacao.ToString("dd/MM/yyyy HH:mm"),
                        n.Lida
                    );
                }

                // Atualizar contadores
                int totalNaoLidas = todasNotificacoes.Count(n => !n.Lida);
                lblNaoLidas.Text = $"{totalNaoLidas} Notificacao(oes) Nao Lida(s)";
                lblTotal.Text = $"Total: {todasNotificacoes.Count} notificacao(oes)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar notificacoes: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvNotificacoes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvNotificacoes.Rows[e.RowIndex];
            string? prioridade = row.Cells["Prioridade"].Value?.ToString();
            bool lida = row.Cells["Lida"].Value != null && (bool)row.Cells["Lida"].Value;

            // Cor de fundo baseada na prioridade
            if (prioridade != null)
            {
                if (prioridade.Contains("URGENTE"))
                    row.DefaultCellStyle.BackColor = lida ? Color.FromArgb(255, 235, 235) : Color.FromArgb(255, 205, 210);
                else if (prioridade.Contains("Alta"))
                    row.DefaultCellStyle.BackColor = lida ? Color.FromArgb(255, 245, 230) : Color.FromArgb(255, 224, 178);
                else if (prioridade.Contains("Normal"))
                    row.DefaultCellStyle.BackColor = lida ? Color.FromArgb(250, 250, 250) : Color.White;
                else if (prioridade.Contains("Baixa"))
                    row.DefaultCellStyle.BackColor = lida ? Color.FromArgb(240, 248, 240) : Color.FromArgb(232, 245, 233);
            }

            // Texto em cinza se ja foi lida
            if (lida)
            {
                row.DefaultCellStyle.ForeColor = Color.Gray;
                row.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            }
            else
            {
                row.DefaultCellStyle.ForeColor = Color.Black;
                row.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }
        }

        private void DgvNotificacoes_SelectionChanged(object? sender, EventArgs e)
        {
            bool temSelecao = dgvNotificacoes.SelectedRows.Count > 0;
            btnMarcarLida.Enabled = temSelecao;
            btnExcluir.Enabled = temSelecao;
        }

        private void BtnMarcarLida_Click(object? sender, EventArgs e)
        {
            if (dgvNotificacoes.SelectedRows.Count == 0) return;

            try
            {
                int idNotificacao = Convert.ToInt32(dgvNotificacoes.SelectedRows[0].Cells["Id"].Value);
                _notificacaoDAL.MarcarComoLida(idNotificacao);
                CarregarNotificacoes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao marcar notificacao como lida: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnMarcarTodasLidas_Click(object? sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show(
                    "Deseja marcar todas as notificacoes como lidas?",
                    "Confirmacao",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _notificacaoDAL.MarcarTodasComoLidas();
                    CarregarNotificacoes();
                    MessageBox.Show("Todas as notificacoes foram marcadas como lidas.", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao marcar notificacoes como lidas: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExcluir_Click(object? sender, EventArgs e)
        {
            if (dgvNotificacoes.SelectedRows.Count == 0) return;

            try
            {
                var result = MessageBox.Show(
                    "Deseja realmente excluir esta notificacao?",
                    "Confirmacao",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    int idNotificacao = Convert.ToInt32(dgvNotificacoes.SelectedRows[0].Cells["Id"].Value);
                    _notificacaoDAL.Excluir(idNotificacao);
                    CarregarNotificacoes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir notificacao: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                timerAtualizacao?.Stop();
                timerAtualizacao?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
