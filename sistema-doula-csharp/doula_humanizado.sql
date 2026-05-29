-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Tempo de geração: 29/05/2026 às 19:17
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

DROP TABLE IF EXISTS `agendamentos`;
CREATE TABLE `agendamentos` (
  `Id` int(11) NOT NULL,
  `ClienteId` int(11) NOT NULL,
  `ServicoId` int(11) DEFAULT NULL,
  `UsuarioResponsavelId` int(11) DEFAULT NULL,
  `DataAgendamento` date NOT NULL,
  `HoraAgendamento` time NOT NULL,
  `QuantidadePessoas` int(11) NOT NULL DEFAULT 1,
  `ValorTotal` decimal(10,2) NOT NULL DEFAULT 0.00,
  `StatusPagamento` enum('PENDENTE','PAGO','REEMBOLSADO') NOT NULL DEFAULT 'PENDENTE',
  `StatusServico` enum('AGENDADO','CONFIRMADO','REMARCADO','FINALIZADO','NAO_COMPARECEU','CANCELADO') NOT NULL DEFAULT 'AGENDADO',
  `Observacoes` text DEFAULT NULL,
  `CodigoConfirmacao` varchar(100) DEFAULT NULL,
  `DataCadastro` datetime NOT NULL DEFAULT current_timestamp(),
  `UltimaAtualizacao` datetime DEFAULT NULL,
  `Notificacao24hEnviada` tinyint(1) DEFAULT 0,
  `Notificacao1hEnviada` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `agendamentos`
--

INSERT INTO `agendamentos` (`Id`, `ClienteId`, `ServicoId`, `UsuarioResponsavelId`, `DataAgendamento`, `HoraAgendamento`, `QuantidadePessoas`, `ValorTotal`, `StatusPagamento`, `StatusServico`, `Observacoes`, `CodigoConfirmacao`, `DataCadastro`, `UltimaAtualizacao`, `Notificacao24hEnviada`, `Notificacao1hEnviada`) VALUES
(18, 13, NULL, NULL, '2026-05-29', '18:00:00', 1, 180.00, 'PAGO', 'REMARCADO', NULL, NULL, '2026-05-29 14:05:20', NULL, 0, 0);

-- --------------------------------------------------------

--
-- Estrutura para tabela `agendamento_servicos`
--

DROP TABLE IF EXISTS `agendamento_servicos`;
CREATE TABLE `agendamento_servicos` (
  `Id` int(11) NOT NULL,
  `AgendamentoId` int(11) NOT NULL,
  `Data` date NOT NULL,
  `Tipo` varchar(50) NOT NULL,
  `Horario` time NOT NULL,
  `Servico` varchar(100) NOT NULL,
  `Valor` decimal(10,2) NOT NULL,
  `Status` enum('ATIVO','CANCELADO') NOT NULL DEFAULT 'ATIVO',
  `Comparecimento` varchar(20) DEFAULT 'PENDENTE'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `agendamento_servicos`
--

INSERT INTO `agendamento_servicos` (`Id`, `AgendamentoId`, `Data`, `Tipo`, `Horario`, `Servico`, `Valor`, `Status`, `Comparecimento`) VALUES
(48, 18, '2026-05-29', 'Doula', '16:00:00', 'Consulta pré-natal', 100.00, 'CANCELADO', 'PENDENTE'),
(49, 18, '2026-05-29', 'Furo', '15:00:00', 'Titânio', 80.00, 'CANCELADO', 'PENDENTE');

-- --------------------------------------------------------

--
-- Estrutura para tabela `blacklist`
--

DROP TABLE IF EXISTS `blacklist`;
CREATE TABLE `blacklist` (
  `Id` int(11) NOT NULL,
  `nome` varchar(150) DEFAULT NULL,
  `CPF` varchar(20) NOT NULL,
  `Email` varchar(150) NOT NULL,
  `DataBloqueio` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Despejando dados para a tabela `blacklist`
--

INSERT INTO `blacklist` (`Id`, `nome`, `CPF`, `Email`, `DataBloqueio`) VALUES
(20, 'sudeste', '123,434,565-xx', 'kaique.smatos@senacsp.edu.br', '2026-05-29 14:13:14');

-- --------------------------------------------------------

--
-- Estrutura para tabela `clientes`
--

DROP TABLE IF EXISTS `clientes`;
CREATE TABLE `clientes` (
  `Id` int(11) NOT NULL,
  `Nome` varchar(150) NOT NULL,
  `Idade` int(11) DEFAULT NULL,
  `CPF` varchar(20) NOT NULL,
  `CEP` varchar(20) DEFAULT NULL,
  `Bairro` varchar(100) DEFAULT NULL,
  `Sexo` varchar(20) DEFAULT NULL,
  `EstadoCivil` varchar(50) DEFAULT NULL,
  `Nacionalidade` varchar(50) DEFAULT NULL,
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

--
-- Despejando dados para a tabela `clientes`
--

INSERT INTO `clientes` (`Id`, `Nome`, `Idade`, `CPF`, `CEP`, `Bairro`, `Sexo`, `EstadoCivil`, `Nacionalidade`, `Email`, `Telefone`, `Status`, `DataCadastro`, `Observacoes`, `NomeCompanheiro`, `NomeBebe`, `DPP`, `LocalParto`, `EquipeMedica`) VALUES
(13, 'sudeste', 37, '123,434,565-xx', '21234245', 'dsfsfgdggf', 'Feminino', 'Solteiro', 'Brasil', 'kaique.smatos@senacsp.edu.br', '1234567889', 'BLOQUEADO', '2026-05-29 14:04:10', NULL, 'sul', 'oeste', '2048-11-11', 'centro', 'centrro-oeste');

-- --------------------------------------------------------

--
-- Estrutura para tabela `logs`
--

DROP TABLE IF EXISTS `logs`;
CREATE TABLE `logs` (
  `Id` int(11) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  `Acao` varchar(255) NOT NULL,
  `Descricao` text DEFAULT NULL,
  `DataHora` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estrutura para tabela `servicos`
--

DROP TABLE IF EXISTS `servicos`;
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

DROP TABLE IF EXISTS `usuarios`;
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
(1, 'Administrador Principal', 30, '00000000000', '00000-000', 'Masculino', 'Nao Informado', '11999999999', 'admin@doula.com', 'admin123', 'ADM', 'ADM_MASTER_001', 'ATIVO', '2026-05-25 12:28:17', NULL, NULL, NULL),
(7, 'flop', 34, '23443234', '12323-123', 'Heterossexual', 'Solteiro', '(23)4 4234-2423', 'ka@gmail.com', '123456a', 'SUBADM', 'SUB-01', 'ATIVO', '2026-05-29 14:09:37', NULL, 'fgdsfgfg', 'Brasil'),
(8, 'kaka', 46, '213424', '23134-235', 'Heterossexual', 'Solteiro', '(43)2 1442-3121', 'kakaka@gmail.com', '123456a', 'FUNCIONARIO', 'FUN-01', 'ATIVO', '2026-05-29 14:10:49', NULL, 'sdfgsdgfdg', 'Brasil'),
(9, 'kai', 35, '232432', '23213-454', 'Heterossexual', 'Solteiro', '(32)3 4214-5235', 'fui@gmail.com', '12345a', 'FUNCIONARIO', 'FUN-02', 'ATIVO', '2026-05-29 14:12:08', NULL, 'dffdgsdfg', 'Brasil');

-- --------------------------------------------------------

--
-- Estrutura stand-in para view `view_financeiro`
-- (Veja abaixo para a visão atual)
--
DROP VIEW IF EXISTS `view_financeiro`;
CREATE TABLE `view_financeiro` (
`Data` date
,`LucroDia` decimal(32,2)
);

-- --------------------------------------------------------

--
-- Estrutura para view `view_financeiro`
--
DROP TABLE IF EXISTS `view_financeiro`;

DROP VIEW IF EXISTS `view_financeiro`;
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
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT de tabela `agendamento_servicos`
--
ALTER TABLE `agendamento_servicos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=50;

--
-- AUTO_INCREMENT de tabela `blacklist`
--
ALTER TABLE `blacklist`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT de tabela `clientes`
--
ALTER TABLE `clientes`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de tabela `logs`
--
ALTER TABLE `logs`
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
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

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
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
