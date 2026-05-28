using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RR_DoulaEFuroHumanizado
{
    public class NotificacaoService
    {
        private readonly EmailService emailService;

        public NotificacaoService(EmailService emailService)
        {
            this.emailService = emailService;
        }

        public void ProcessarNotificacoes24h()
        {
            using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
            {
                conn.Open();

                string sql = @"
SELECT 
    A.Id, 
    C.Email AS EmailCliente, 
    MIN(S.Data) AS Data, 
    GROUP_CONCAT(S.Horario) AS Horarios, 
    GROUP_CONCAT(S.Servico) AS Servicos, 
    SUM(S.Valor) AS ValorTotal, 
    A.Status
FROM agendamentos A
INNER JOIN clientes C ON A.ClienteId = C.Id
INNER JOIN agendamento_servicos S ON S.AgendamentoId = A.Id
WHERE S.Status = 'ATIVO'
AND C.Email IS NOT NULL
AND TRIM(C.Email) <> ''
AND IFNULL(A.Notificacao24hEnviada, 0) = 0
GROUP BY A.Id, C.Email, A.Status";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    DataTable tabela = new DataTable();
                    tabela.Load(dr);

                    foreach (DataRow row in tabela.Rows)
                    {
                        int id = Convert.ToInt32(row["Id"]);

                        string emailCliente =
                            row["EmailCliente"]?.ToString() ?? "";

                        DateTime data =
                            Convert.ToDateTime(row["Data"]);

                        string horarios =
                            row["Horarios"]?.ToString() ?? "";

                        string servicos =
                            row["Servicos"]?.ToString() ?? "";

                        decimal valorTotal =
                            row["ValorTotal"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(row["ValorTotal"]);

                        DateTime? primeiroHorario =
                            ObterPrimeiraDataHoraAgendamento(data, horarios);

                        if (!primeiroHorario.HasValue)
                            continue;

                        TimeSpan diferenca =
                            primeiroHorario.Value - DateTime.Now;

                        if (diferenca.TotalHours <= 24 &&
                            diferenca.TotalHours > 23)
                        {
                            string assunto =
                                "Lembrete: seu agendamento é amanhã";

                            string corpo = $@"Olá!

Este é um lembrete do seu agendamento.

Data: {data:dd/MM/yyyy}
Horário(s): {horarios}
Serviço(s): {servicos}
Valor total: R$ {valorTotal:N2}

Aguardamos você.
Sistema Doula";

                            try
                            {
                                // Envia para a cliente
                                emailService.EnviarEmail(emailCliente, assunto, corpo);

                                //  Envia para o Administrador 
                                string assuntoAdmin = $"📅 ATENDIMENTO AMANHÃ: {emailCliente}";
                                string corpoAdmin = $@"Atenção, equipe!
                                
Vocês têm um atendimento agendado para amanhã.

Cliente: {emailCliente}
Data: {data:dd/MM/yyyy}
Horários: {horarios}
Serviços: {servicos}

Preparem os materiais!";
                                emailService.EnviarEmail("projetodoulaefuro01@gmail.com", assuntoAdmin, corpoAdmin);
                               

                                MarcarNotificacao24hComoEnviada(id);
                            }
                            catch
                            {
                                //evita travar o sistema se falhar envio
                            }
                        }
                    }
                }
            }
        }

        public void ProcessarNotificacoes1h()
        {
            using (MySqlConnection conn = new MySqlConnection(Conexao.StringConexao))
            {
                conn.Open();

                string sql = @"
SELECT 
    A.Id, 
    C.Email AS EmailCliente, 
    MIN(S.Data) AS Data, 
    GROUP_CONCAT(S.Horario) AS Horarios, 
    GROUP_CONCAT(S.Servico) AS Servicos, 
    SUM(S.Valor) AS ValorTotal, 
    A.Status
FROM agendamentos A
INNER JOIN clientes C ON A.ClienteId = C.Id
INNER JOIN agendamento_servicos S ON S.AgendamentoId = A.Id
WHERE S.Status = 'ATIVO'
AND C.Email IS NOT NULL
AND TRIM(C.Email) <> ''
AND IFNULL(A.Notificacao1hEnviada, 0) = 0
GROUP BY A.Id, C.Email, A.Status";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    DataTable tabela = new DataTable();
                    tabela.Load(dr);

                    foreach (DataRow row in tabela.Rows)
                    {
                        int id = Convert.ToInt32(row["Id"]);

                        string emailCliente =
                            row["EmailCliente"]?.ToString() ?? "";

                        DateTime data =
                            Convert.ToDateTime(row["Data"]);

                        string horarios =
                            row["Horarios"]?.ToString() ?? "";

                        string servicos =
                            row["Servicos"]?.ToString() ?? "";

                        decimal valorTotal =
                            row["ValorTotal"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(row["ValorTotal"]);

                        DateTime? primeiroHorario =
                            ObterPrimeiraDataHoraAgendamento(data, horarios);

                        if (!primeiroHorario.HasValue)
                            continue;

                        TimeSpan diferenca =
                            primeiroHorario.Value - DateTime.Now;

                        if (diferenca.TotalMinutes <= 60 &&
                            diferenca.TotalMinutes > 0)
                        {
                            string assunto =
                                "Lembrete: seu agendamento começa em breve";

                            string corpo = $@"Olá!

Seu agendamento está próximo.

Data: {data:dd/MM/yyyy}
Horário(s): {horarios}
Serviço(s): {servicos}
Valor total: R$ {valorTotal:N2}

Nos vemos em breve!
Sistema Doula";

                            try
                            {
                                // Envia para a cliente
                                emailService.EnviarEmail(emailCliente, assunto, corpo);

                                //  NOVO: Envia para o Administrador
                                string assuntoAdmin = $"⏰ ATENDIMENTO EM 1 HORA: {emailCliente}";
                                string corpoAdmin = $@"Atenção, equipe!
                                
O atendimento está prestes a começar (em menos de 1 hora).

Cliente: {emailCliente}
Horários: {horarios}
Serviços: {servicos}

Ótimo atendimento!";
                                emailService.EnviarEmail("projetodoulaefuro01@gmail.com", assuntoAdmin, corpoAdmin);
                                
                                MarcarNotificacao1hComoEnviada(id);
                            }
                            catch
                            {
                                // evita travar o sistema se falhar envio
                            }
                        }
                    }
                }
            }
        }

        private DateTime? ObterPrimeiraDataHoraAgendamento(
            DateTime data,
            string horarios
        )
        {
            if (string.IsNullOrWhiteSpace(horarios))
                return null;

            List<TimeSpan> listaHorarios =
                new List<TimeSpan>();

            string[] partes =
                horarios.Split(',');

            foreach (string parte in partes)
            {
                string horarioLimpo =
                    parte.Trim();

                if (TimeSpan.TryParse(
                    horarioLimpo,
                    out TimeSpan hora))
                {
                    listaHorarios.Add(hora);
                }
            }

            if (listaHorarios.Count == 0)
                return null;

            TimeSpan menorHorario =
                listaHorarios.Min();

            return data.Date.Add(menorHorario);
        }

        private void MarcarNotificacao24hComoEnviada(int id)
        {
            using (MySqlConnection conn =
                   new MySqlConnection(Conexao.StringConexao))
            {
                conn.Open();

                string sql = @"
UPDATE Agendamentos
SET Notificacao24hEnviada = 1
WHERE Id = @Id";

                using (MySqlCommand cmd =
                       new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void MarcarNotificacao1hComoEnviada(int id)
        {
            using (MySqlConnection conn =
                   new MySqlConnection(Conexao.StringConexao))
            {
                conn.Open();

                string sql = @"
UPDATE Agendamentos
SET Notificacao1hEnviada = 1
WHERE Id = @Id";

                using (MySqlCommand cmd =
                       new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}