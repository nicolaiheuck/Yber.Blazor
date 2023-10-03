-- MariaDB dump 10.19  Distrib 10.9.8-MariaDB, for osx10.17 (x86_64)
--
-- Host: localhost    Database: UBER
-- ------------------------------------------------------
-- Server version	10.9.8-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `UBER`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `UBER` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;

USE `UBER`;

--
-- Table structure for table `Uber_Cities`
--

DROP TABLE IF EXISTS `Uber_Cities`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `Uber_Cities` (
  `Zipcode` int(11) NOT NULL,
  `City` varchar(75) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  PRIMARY KEY (`Zipcode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Uber_Cities`
--

LOCK TABLES `Uber_Cities` WRITE;
/*!40000 ALTER TABLE `Uber_Cities` DISABLE KEYS */;
INSERT INTO `Uber_Cities` VALUES
(6400,'Sønderborg'),
(6470,'Sydals');
/*!40000 ALTER TABLE `Uber_Cities` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `uber_requests`
--

DROP TABLE IF EXISTS `uber_requests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `uber_requests` (
  `RequesterID` int(11) NOT NULL,
  `RequesteeID` int(11) NOT NULL,
  `requestapproved` tinyint(4) DEFAULT NULL,
  KEY `RequesterID` (`RequesterID`),
  KEY `RequesteeID` (`RequesteeID`),
  CONSTRAINT `uber_requests_ibfk_1` FOREIGN KEY (`RequesterID`) REFERENCES `Uber_Students` (`Id`),
  CONSTRAINT `uber_requests_ibfk_2` FOREIGN KEY (`RequesteeID`) REFERENCES `Uber_Students` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `uber_requests`
--

LOCK TABLES `uber_requests` WRITE;
/*!40000 ALTER TABLE `uber_requests` DISABLE KEYS */;
INSERT INTO `uber_requests` VALUES
(2,1,1);
/*!40000 ALTER TABLE `uber_requests` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `uber_students`
--

DROP TABLE IF EXISTS `uber_students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `uber_students` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name_First` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `Name_Last` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `Street_Name` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `Street_Number` varchar(10) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `Longitude` varchar(15) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `Lattitude` varchar(15) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `Zipcode` int(11) DEFAULT NULL,
  `lift_take` tinyint(1) DEFAULT NULL,
  `lift_give` tinyint(1) DEFAULT NULL,
  `Username` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `Zipcode` (`Zipcode`),
  CONSTRAINT `uber_students_ibfk_1` FOREIGN KEY (`Zipcode`) REFERENCES `Uber_Cities` (`Zipcode`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `uber_students`
--

LOCK TABLES `uber_students` WRITE;
/*!40000 ALTER TABLE `uber_students` DISABLE KEYS */;
INSERT INTO `uber_students` VALUES
(1,'Peter','Hymøller','Smallegade','1.1','9.798410','54.913640',6400,0,1,'peter.hymoeller@hotmail.dk'),
(2,'Dummy','User','Fakestreet','10','9.7974','54.90567',6400,1,0,'dummy'),
(3,'Elev','Nummer 3','Rendsborgvej','22','9.71594','54.91654',6400,1,0,'elvn3'),
(4,'Elev','Nummer 4','Sønderled','7','9.76445','54.90912',6400,1,0,'elvn4'),
(5,'Elev','Nummer 5','Præstegårdsparken','34','9.83052','54.92874',6400,0,1,'elvn5'),
(6,'Lærer','Nummer 1','Damgade','4A','9.79757','54.91530',6400,0,1,'lrrn1'),
(7,'Lærer','Nummer 2','Østerbakken','6','9.89428','54.91182',6470,1,0,'lrrn2'),
(8,'Nicolai','Heuck','Løndals Stræde','1','9.80660','54.90700',6400,0,1,'nicheu'),
(9,'Tobias','Nielsen','Lavbrinkevej','2','9.80734','54.91864',6400,0,0,'tobiniel'),
(10,'Jan','Andreasen','Sønderled','7','9.76445','54.90912',6400,0,0,'Jan@tved.me');
/*!40000 ALTER TABLE `uber_students` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-10-02 13:40:18
