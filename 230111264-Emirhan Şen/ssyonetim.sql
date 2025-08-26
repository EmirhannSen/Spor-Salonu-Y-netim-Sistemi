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
-- Table structure for table `diyetprogramlari`
--

DROP TABLE IF EXISTS `diyetprogramlari`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `diyetprogramlari` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `OgunAdi` varchar(100) NOT NULL,
  `Kalori` int NOT NULL,
  `OgunZamani` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `diyetprogramlari`
--

LOCK TABLES `diyetprogramlari` WRITE;
/*!40000 ALTER TABLE `diyetprogramlari` DISABLE KEYS */;
INSERT INTO `diyetprogramlari` VALUES (17,'peynir',12,'Ara Öğün'),(18,'yumurta',30,'Kahvaltı'),(19,'muz',13,'Ara Öğün');
/*!40000 ALTER TABLE `diyetprogramlari` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `egitmenler`
--

DROP TABLE IF EXISTS `egitmenler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `egitmenler` (
  `id` int NOT NULL AUTO_INCREMENT,
  `ad` varchar(100) NOT NULL,
  `soyad` varchar(100) NOT NULL,
  `telefon` varchar(15) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `kayit_tarihi` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `egitmenler`
--

LOCK TABLES `egitmenler` WRITE;
/*!40000 ALTER TABLE `egitmenler` DISABLE KEYS */;
INSERT INTO `egitmenler` VALUES (31,'Ömer','Barlık','553 533','barlikomer1@gmail.com','2025-05-19 21:00:00');
/*!40000 ALTER TABLE `egitmenler` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `egzersizadi`
--

DROP TABLE IF EXISTS `egzersizadi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `egzersizadi` (
  `id` int NOT NULL AUTO_INCREMENT,
  `egzersiz_adi` varchar(100) NOT NULL,
  `kas_grubu` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `egzersizadi`
--

LOCK TABLES `egzersizadi` WRITE;
/*!40000 ALTER TABLE `egzersizadi` DISABLE KEYS */;
INSERT INTO `egzersizadi` VALUES (11,'güğüs başlangıç','göğüs'),(12,'ön kol','kol'),(13,'kanatlar','sırt');
/*!40000 ALTER TABLE `egzersizadi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fiyatlar`
--

DROP TABLE IF EXISTS `fiyatlar`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fiyatlar` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Kategori` varchar(100) DEFAULT NULL,
  `SureAy` int DEFAULT NULL,
  `Fiyat` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=41 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fiyatlar`
--

LOCK TABLES `fiyatlar` WRITE;
/*!40000 ALTER TABLE `fiyatlar` DISABLE KEYS */;
INSERT INTO `fiyatlar` VALUES (26,'kategori a',1,1111.00),(27,'kategori b',2,2222.00);
/*!40000 ALTER TABLE `fiyatlar` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kas_gruplari`
--

DROP TABLE IF EXISTS `kas_gruplari`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kas_gruplari` (
  `id` int NOT NULL AUTO_INCREMENT,
  `ad` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kas_gruplari`
--

LOCK TABLES `kas_gruplari` WRITE;
/*!40000 ALTER TABLE `kas_gruplari` DISABLE KEYS */;
INSERT INTO `kas_gruplari` VALUES (8,'göğüs'),(9,'kol'),(10,'sırt');
/*!40000 ALTER TABLE `kas_gruplari` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kategoriler`
--

DROP TABLE IF EXISTS `kategoriler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kategoriler` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Ad` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kategoriler`
--

LOCK TABLES `kategoriler` WRITE;
/*!40000 ALTER TABLE `kategoriler` DISABLE KEYS */;
INSERT INTO `kategoriler` VALUES (21,'kategori a'),(28,'kategori b'),(29,'kategori c'),(30,'kategori d');
/*!40000 ALTER TABLE `kategoriler` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `kullanici`
--

DROP TABLE IF EXISTS `kullanici`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `kullanici` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AdSoyad` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Sifre` varchar(100) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `kullanici`
--

LOCK TABLES `kullanici` WRITE;
/*!40000 ALTER TABLE `kullanici` DISABLE KEYS */;
INSERT INTO `kullanici` VALUES (1,'Ömer Barlık','deneme@gmail.com','123456');
/*!40000 ALTER TABLE `kullanici` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `odemeler`
--

DROP TABLE IF EXISTS `odemeler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `odemeler` (
  `OdemeID` int NOT NULL AUTO_INCREMENT,
  `UyeID` int DEFAULT NULL,
  `OdemeTarihi` date DEFAULT NULL,
  `Tutar` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`OdemeID`),
  KEY `odemeler_ibfk_1` (`UyeID`),
  CONSTRAINT `odemeler_ibfk_1` FOREIGN KEY (`UyeID`) REFERENCES `uyeler` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `odemeler`
--

LOCK TABLES `odemeler` WRITE;
/*!40000 ALTER TABLE `odemeler` DISABLE KEYS */;
INSERT INTO `odemeler` VALUES (30,35,'2025-05-25',2222.00);
/*!40000 ALTER TABLE `odemeler` ENABLE KEYS */;
UNLOCK TABLES;

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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `programlar`
--

LOCK TABLES `programlar` WRITE;
/*!40000 ALTER TABLE `programlar` DISABLE KEYS */;
INSERT INTO `programlar` VALUES (1,35,'kanatlar','3','12',NULL,'Çarşamba'),(2,35,'güğüs başlangıç','4','10','','Cuma'),(4,35,'güğüs başlangıç','12','2','2','Salı'),(5,35,'güğüs başlangıç','12','12','0','Salı'),(6,35,'kanatlar','12','12','12','Pazartesi');
/*!40000 ALTER TABLE `programlar` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `uye_diyet_programi`
--

DROP TABLE IF EXISTS `uye_diyet_programi`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `uye_diyet_programi` (
  `id` int NOT NULL AUTO_INCREMENT,
  `uye_id` int DEFAULT NULL,
  `ogun_id` int DEFAULT NULL,
  `gun` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `uye_diyet_programi`
--

LOCK TABLES `uye_diyet_programi` WRITE;
/*!40000 ALTER TABLE `uye_diyet_programi` DISABLE KEYS */;
INSERT INTO `uye_diyet_programi` VALUES (12,35,18,'Perşembe');
/*!40000 ALTER TABLE `uye_diyet_programi` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `uyeler`
--

DROP TABLE IF EXISTS `uyeler`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `uyeler` (
  `id` int NOT NULL AUTO_INCREMENT,
  `ad` varchar(50) DEFAULT NULL,
  `soyad` varchar(50) DEFAULT NULL,
  `telefon` varchar(20) DEFAULT NULL,
  `yas` int DEFAULT NULL,
  `boy` float DEFAULT NULL,
  `kilo` float DEFAULT NULL,
  `cinsiyet` varchar(10) DEFAULT NULL,
  `kayit_suresi` int DEFAULT NULL,
  `kategori` varchar(50) DEFAULT NULL,
  `dogumtarihi` date DEFAULT NULL,
  `kayit_tarihi` date DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=38 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `uyeler`
--

LOCK TABLES `uyeler` WRITE;
/*!40000 ALTER TABLE `uyeler` DISABLE KEYS */;
INSERT INTO `uyeler` VALUES (5,'Ahmet','mehmet','555 555 55 55',23,170,85,'Erkek',3,'kickboks','2025-05-14','2025-03-01'),(35,'Ömer','barlık','553',21,1.6,70,'Erkek',2,'kategori b','2004-01-11','2025-05-25');
/*!40000 ALTER TABLE `uyeler` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-15 15:04:01
