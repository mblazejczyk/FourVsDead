-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Feb 13, 2025 at 04:07 PM
-- Wersja serwera: 10.11.10-MariaDB
-- Wersja PHP: 8.3.13

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `srv38973_fourvsdead`
--

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `accounts`
--

CREATE TABLE `accounts` (
  `id` int(11) NOT NULL,
  `login` text NOT NULL,
  `password` text NOT NULL,
  `totalXp` int(11) NOT NULL,
  `UpgradePoints` int(11) NOT NULL,
  `UpgradesSave` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

--
-- Dumping data for table `accounts`
--

INSERT INTO `accounts` (`id`, `login`, `password`, `totalXp`, `UpgradePoints`, `UpgradesSave`) VALUES
(1, 'admin', '$2y$10$IrFS0ERs9pSokHQIZpfD5emv6PoP1xbBAPoSxnfuC5XmHHq9vyHXC', 27138, 2, '1111111111111111111111111'),
(2, 'kolopiko', '$2y$10$JAKKQ/hzSFgwsMVwwytxtORtlqpgqhGscbdQjGyFTRdvGk0kE146y', 11101, 1, '1000000001111111100000000');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `bans`
--

CREATE TABLE `bans` (
  `id` int(11) NOT NULL,
  `bannedUserId` int(11) NOT NULL,
  `reason` text NOT NULL,
  `unbanDate` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `connectionCheck`
--

CREATE TABLE `connectionCheck` (
  `id` int(11) NOT NULL,
  `why` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

--
-- Dumping data for table `connectionCheck`
--

INSERT INTO `connectionCheck` (`id`, `why`) VALUES
(1, 'open');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `inviteCodes`
--

CREATE TABLE `inviteCodes` (
  `id` int(11) NOT NULL,
  `inviteCode` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `itemsOwned`
--

CREATE TABLE `itemsOwned` (
  `id` int(11) NOT NULL,
  `playerId` int(11) NOT NULL,
  `itemId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

--
-- Dumping data for table `itemsOwned`
--

INSERT INTO `itemsOwned` (`id`, `playerId`, `itemId`) VALUES
(1, 1, 1),
(2, 1, 2),
(9, 1, 3);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `lastLogin`
--

CREATE TABLE `lastLogin` (
  `id` int(11) NOT NULL,
  `userId` int(11) NOT NULL,
  `lastLogin` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

--
-- Dumping data for table `lastLogin`
--

INSERT INTO `lastLogin` (`id`, `userId`, `lastLogin`) VALUES
(1, 1, '2023-07-17'),
(30, 2, '2023-10-18');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `moderators`
--

CREATE TABLE `moderators` (
  `id` int(11) NOT NULL,
  `userId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

--
-- Dumping data for table `moderators`
--

INSERT INTO `moderators` (`id`, `userId`) VALUES
(1, 1),
(2, 2);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `reports`
--

CREATE TABLE `reports` (
  `id` int(11) NOT NULL,
  `reportedUserId` int(11) NOT NULL,
  `reason` text NOT NULL,
  `chatLog` text NOT NULL,
  `reportDate` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `saves`
--

CREATE TABLE `saves` (
  `saveId` int(11) NOT NULL,
  `playerId` int(11) NOT NULL,
  `zombieKilled` int(11) NOT NULL,
  `coinsCollected` int(11) NOT NULL,
  `dmgTaken` int(11) NOT NULL,
  `dmgGiven` int(11) NOT NULL,
  `deaths` int(11) NOT NULL,
  `knockouts` int(11) NOT NULL,
  `buys` int(11) NOT NULL,
  `shots` int(11) NOT NULL,
  `firstGame` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

--
-- Dumping data for table `saves`
--

INSERT INTO `saves` (`saveId`, `playerId`, `zombieKilled`, `coinsCollected`, `dmgTaken`, `dmgGiven`, `deaths`, `knockouts`, `buys`, `shots`, `firstGame`) VALUES
(2, 1, 829, 1228867, 4785, 13296, 3, 18, 74, 4927, '2023-03-14'),
(28, 2, 154, 62639, 1274, 3781, 1, 12, 32, 4136, '2023-03-21');

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `bans`
--
ALTER TABLE `bans`
  ADD PRIMARY KEY (`id`),
  ADD KEY `bannedUserId` (`bannedUserId`);

--
-- Indeksy dla tabeli `connectionCheck`
--
ALTER TABLE `connectionCheck`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `inviteCodes`
--
ALTER TABLE `inviteCodes`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `itemsOwned`
--
ALTER TABLE `itemsOwned`
  ADD PRIMARY KEY (`id`),
  ADD KEY `playerId` (`playerId`);

--
-- Indeksy dla tabeli `lastLogin`
--
ALTER TABLE `lastLogin`
  ADD PRIMARY KEY (`id`),
  ADD KEY `userId` (`userId`);

--
-- Indeksy dla tabeli `moderators`
--
ALTER TABLE `moderators`
  ADD PRIMARY KEY (`id`),
  ADD KEY `userId` (`userId`);

--
-- Indeksy dla tabeli `reports`
--
ALTER TABLE `reports`
  ADD PRIMARY KEY (`id`),
  ADD KEY `reportedUserId` (`reportedUserId`);

--
-- Indeksy dla tabeli `saves`
--
ALTER TABLE `saves`
  ADD PRIMARY KEY (`saveId`),
  ADD KEY `playerId` (`playerId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `accounts`
--
ALTER TABLE `accounts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32;

--
-- AUTO_INCREMENT for table `bans`
--
ALTER TABLE `bans`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT for table `connectionCheck`
--
ALTER TABLE `connectionCheck`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `inviteCodes`
--
ALTER TABLE `inviteCodes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- AUTO_INCREMENT for table `itemsOwned`
--
ALTER TABLE `itemsOwned`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `lastLogin`
--
ALTER TABLE `lastLogin`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT for table `moderators`
--
ALTER TABLE `moderators`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `reports`
--
ALTER TABLE `reports`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT for table `saves`
--
ALTER TABLE `saves`
  MODIFY `saveId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `bans`
--
ALTER TABLE `bans`
  ADD CONSTRAINT `bans_ibfk_1` FOREIGN KEY (`bannedUserId`) REFERENCES `accounts` (`id`);

--
-- Constraints for table `itemsOwned`
--
ALTER TABLE `itemsOwned`
  ADD CONSTRAINT `itemsOwned_ibfk_1` FOREIGN KEY (`playerId`) REFERENCES `accounts` (`id`);

--
-- Constraints for table `lastLogin`
--
ALTER TABLE `lastLogin`
  ADD CONSTRAINT `lastLogin_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `accounts` (`id`);

--
-- Constraints for table `moderators`
--
ALTER TABLE `moderators`
  ADD CONSTRAINT `moderators_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `accounts` (`id`);

--
-- Constraints for table `reports`
--
ALTER TABLE `reports`
  ADD CONSTRAINT `reports_ibfk_1` FOREIGN KEY (`reportedUserId`) REFERENCES `accounts` (`id`);

--
-- Constraints for table `saves`
--
ALTER TABLE `saves`
  ADD CONSTRAINT `saves_ibfk_1` FOREIGN KEY (`playerId`) REFERENCES `accounts` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
