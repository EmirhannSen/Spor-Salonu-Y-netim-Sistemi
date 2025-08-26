-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: localhost    Database: ssyonetim
-- ------------------------------------------------------
-- Server version	8.0.42

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `programlar`
--

DROP TABLE IF EXISTS `programlar`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `programlar` (
  `id` int NOT NULL AUTO_INCREMENT,
  `uye_id` int DEFAULT NULL,
  `egzersiz_adi` varchar(100) DEFAULT NULL,
  `set_sayisi` varchar(10) DEFAULT NULL,
  `tekrar_sayisi` varchar(10) DEFAULT NULL,
  `sure` varchar(10) DEFAULT NULL,
  `gun` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `uye_id` (`uye_id`),
  CONSTRAINT `programlar_ibfk_1` FOREIGN KEY (`uye_id`) REFERENCES `uyeler` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `programlar`
--

LOCK TABLES `programlar` WRITE;
/*!40000 ALTER TABLE `programlar` DISABLE KEYS */;
INSERT INTO `programlar` VALUES (1,35,'kanatlar','3','12',NULL,'Çarşamba'),(2,35,'güğüs başlangıç','4','10','','Cuma'),(4,35,'güğüs başlangıç','12','2','2','Salı'),(5,35,'güğüs başlangıç','12','12','0','Salı'),(6,35,'kanatlar','12','12','12','Pazartesi');
/*!40000 ALTER TABLE `programlar` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-05-30 11:46:20
