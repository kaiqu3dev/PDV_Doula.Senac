-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 26/05/2026 às 19:54
-- Versão do servidor: 10.4.32-MariaDB
-- Versão do PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Banco de dados: `doula_humanizado`
--

-- --------------------------------------------------------

--
-- Estrutura para tabela `agendamentos`
--

CREATE TABLE `agendamentos` (
  `Id` int(11) NOT NULL,
  `ClienteId` int(11) NOT NULL,
  `ServicoId` int(11) NOT NULL,
  `UsuarioResponsavelId` int(11) NOT NULL,
  `DataAgendamento` date NOT NULL,
  `HoraAgendamento` time NOT NULL,
  `QuantidadePessoas` int(11) NOT NULL DEFAULT 1,
  `ValorTotal` decimal(10,2) NOT NULL DEFAULT 0.00,
  `StatusPagamento` enum('PENDENTE','PAGO','REEMBOLSADO') NOT NULL DEFAULT 'PENDENTE',
  `StatusServico` enum('AGENDADO','CONFIRMADO','REMARCADO','FINALIZADO','NAO_COMPARECEU','CANCELADO') NOT NULL DEFAULT 'AGENDADO',
  `Observacoes` text DEFAULT NULL,
  `CodigoConfirmacao` varchar(100) DEFAULT NULL,
  `DataCadastro` datetime NOT NULL DEFAULT current_timestamp(),
  `UltimaAtualizacao` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `agendamento_servicos`
--

CREATE TABLE `agendamento_servicos` (
  `Id` int(11) NOT NULL,
  `AgendamentoId` int(11) NOT NULL,
  `Data` date NOT NULL,
  `Tipo` varchar(50) NOT NULL,
  `Horario` time NOT NULL,
  `Servico` varchar(100) NOT NULL,
  `Valor` decimal(10,2) NOT NULL,
  `Status` enum('ATIVO','CANCELADO') NOT NULL DEFAULT 'ATIVO'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `blacklist`
--

CREATE TABLE `blacklist` (
  `Id` int(11) NOT NULL,
  `nome` varchar(150) DEFAULT NULL,
  `CPF` varchar(20) NOT NULL,
  `Email` varchar(150) NOT NULL,
  `DataBloqueio` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `clientes`
--

CREATE TABLE `clientes` (
  `Id` int(11) NOT NULL,
  `Nome` varchar(150) NOT NULL,
  `Idade` int(11) DEFAULT NULL,
  `CPF` varchar(20) NOT NULL,
  `CEP` varchar(20) DEFAULT NULL,
  `Sexo` varchar(20) DEFAULT NULL,
  `Email` varchar(150) NOT NULL,
  `Telefone` varchar(25) DEFAULT NULL,
  `Status` enum('ATIVO','PERIGOSO','BLOQUEADO') NOT NULL DEFAULT 'ATIVO',
  `DataCadastro` datetime NOT NULL DEFAULT current_timestamp(),
  `Observacoes` text DEFAULT NULL,
  `NomeCompanheiro` varchar(150) DEFAULT NULL,
  `NomeBebe` varchar(150) DEFAULT NULL,
  `DPP` date DEFAULT NULL,
  `LocalParto` varchar(150) DEFAULT NULL,
  `EquipeMedica` varchar(200) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `logs`
--

CREATE TABLE `logs` (
  `Id` int(11) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  `Acao` varchar(255) NOT NULL,
  `Descricao` text DEFAULT NULL,
  `DataHora` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `notificacoes`
--

CREATE TABLE `notificacoes` (
  `Id` int(11) NOT NULL,
  `AgendamentoId` int(11) NOT NULL,
  `Tipo` enum('CONFIRMACAO','PAGAMENTO','LEMBRETE24H','LEMBRETE1H','CANCELAMENTO','REMARCACAO') NOT NULL,
  `EmailDestino` varchar(150) NOT NULL,
  `Status` enum('PENDENTE','ENVIADO','ERRO') NOT NULL DEFAULT 'PENDENTE',
  `DataEnvio` datetime DEFAULT NULL,
  `Mensagem` text DEFAULT NULL,
  `Tentativas` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `servicos`
--

CREATE TABLE `servicos` (
  `Id` int(11) NOT NULL,
  `NomeServico` varchar(150) NOT NULL,
  `Descricao` text DEFAULT NULL,
  `Valor` decimal(10,2) NOT NULL DEFAULT 0.00,
  `DuracaoMinutos` int(11) NOT NULL DEFAULT 60,
  `Categoria` varchar(100) DEFAULT NULL,
  `Status` enum('ATIVO','INATIVO') NOT NULL DEFAULT 'ATIVO',
  `DataCadastro` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `usuarios`
--

CREATE TABLE `usuarios` (
  `Id` int(11) NOT NULL,
  `Nome` varchar(150) NOT NULL,
  `Idade` int(11) DEFAULT NULL,
  `CPF` varchar(20) NOT NULL,
  `CEP` varchar(20) DEFAULT NULL,
  `Sexo` varchar(20) DEFAULT NULL,
  `EstadoCivil` varchar(50) DEFAULT NULL,
  `Telefone` varchar(25) DEFAULT NULL,
  `Email` varchar(150) NOT NULL,
  `Senha` varchar(255) NOT NULL,
  `TipoUsuario` enum('ADM','SUBADM','FUNCIONARIO') NOT NULL,
  `CodigoAcesso` varchar(100) NOT NULL,
  `Status` enum('ATIVO','INATIVO') NOT NULL DEFAULT 'ATIVO',
  `DataCadastro` datetime NOT NULL DEFAULT current_timestamp(),
  `UltimoLogin` datetime DEFAULT NULL,
  `Endereco` varchar(255) DEFAULT NULL,
  `Naturalidade` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `usuarios`
--

INSERT INTO `usuarios` (`Id`, `Nome`, `Idade`, `CPF`, `CEP`, `Sexo`, `EstadoCivil`, `Telefone`, `Email`, `Senha`, `TipoUsuario`, `CodigoAcesso`, `Status`, `DataCadastro`, `UltimoLogin`, `Endereco`, `Naturalidade`) VALUES
(1, 'Administrador Principal', 30, '00000000000', '00000-000', 'Masculino', 'Nao Informado', '11999999999', 'admin@doula.com', 'admin123', 'ADM', 'ADM_MASTER_001', 'ATIVO', '2026-05-25 12:28:17', NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Estrutura stand-in para view `view_financeiro`
-- (Veja abaixo para a visão atual)
--
CREATE TABLE `view_financeiro` (
`Data` date
,`LucroDia` decimal(32,2)
);

-- --------------------------------------------------------

--
-- Estrutura para view `view_financeiro`
--
DROP TABLE IF EXISTS `view_financeiro`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_financeiro`  AS SELECT cast(`agendamentos`.`DataAgendamento` as date) AS `Data`, sum(`agendamentos`.`ValorTotal`) AS `LucroDia` FROM `agendamentos` WHERE `agendamentos`.`StatusPagamento` = 'PAGO' GROUP BY cast(`agendamentos`.`DataAgendamento` as date) ;

--
-- Índices para tabelas despejadas
--

--
-- Índices de tabela `agendamentos`
--
ALTER TABLE `agendamentos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FK_Agendamentos_Clientes` (`ClienteId`),
  ADD KEY `FK_Agendamentos_Servicos` (`ServicoId`),
  ADD KEY `FK_Agendamentos_Usuarios` (`UsuarioResponsavelId`);

--
-- Índices de tabela `agendamento_servicos`
--
ALTER TABLE `agendamento_servicos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FK_Itens_Agendamento` (`AgendamentoId`);

--
-- Índices de tabela `blacklist`
--
ALTER TABLE `blacklist`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `CPF` (`CPF`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- Índices de tabela `clientes`
--
ALTER TABLE `clientes`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `CPF` (`CPF`);

--
-- Índices de tabela `logs`
--
ALTER TABLE `logs`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FK_Logs_Usuarios` (`UsuarioId`);

--
-- Índices de tabela `notificacoes`
--
ALTER TABLE `notificacoes`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `FK_Notificacoes_Agendamentos` (`AgendamentoId`);

--
-- Índices de tabela `servicos`
--
ALTER TABLE `servicos`
  ADD PRIMARY KEY (`Id`);

--
-- Índices de tabela `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `CPF` (`CPF`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- AUTO_INCREMENT para tabelas despejadas
--

--
-- AUTO_INCREMENT de tabela `agendamentos`
--
ALTER TABLE `agendamentos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `agendamento_servicos`
--
ALTER TABLE `agendamento_servicos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `blacklist`
--
ALTER TABLE `blacklist`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `clientes`
--
ALTER TABLE `clientes`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `logs`
--
ALTER TABLE `logs`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `notificacoes`
--
ALTER TABLE `notificacoes`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `servicos`
--
ALTER TABLE `servicos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de tabela `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Restrições para tabelas despejadas
--

--
-- Restrições para tabelas `agendamentos`
--
ALTER TABLE `agendamentos`
  ADD CONSTRAINT `FK_Agendamentos_Clientes` FOREIGN KEY (`ClienteId`) REFERENCES `clientes` (`Id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_Agendamentos_Servicos` FOREIGN KEY (`ServicoId`) REFERENCES `servicos` (`Id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `FK_Agendamentos_Usuarios` FOREIGN KEY (`UsuarioResponsavelId`) REFERENCES `usuarios` (`Id`) ON UPDATE CASCADE;

--
-- Restrições para tabelas `agendamento_servicos`
--
ALTER TABLE `agendamento_servicos`
  ADD CONSTRAINT `FK_Itens_Agendamento` FOREIGN KEY (`AgendamentoId`) REFERENCES `agendamentos` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Restrições para tabelas `logs`
--
ALTER TABLE `logs`
  ADD CONSTRAINT `FK_Logs_Usuarios` FOREIGN KEY (`UsuarioId`) REFERENCES `usuarios` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Restrições para tabelas `notificacoes`
--
ALTER TABLE `notificacoes`
  ADD CONSTRAINT `FK_Notificacoes_Agendamentos` FOREIGN KEY (`AgendamentoId`) REFERENCES `agendamentos` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
