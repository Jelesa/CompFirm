-- MySQL dump 10.13  Distrib 8.0.30, for Win64 (x86_64)
--
-- Host: localhost    Database: bdcomp
-- ------------------------------------------------------
-- Server version	8.0.30

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `characteristic_datatypes`
--

DROP TABLE IF EXISTS `characteristic_datatypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `characteristic_datatypes` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `characteristic_datatypes`
--

LOCK TABLES `characteristic_datatypes` WRITE;
/*!40000 ALTER TABLE `characteristic_datatypes` DISABLE KEYS */;
INSERT INTO `characteristic_datatypes` VALUES (1,'string');
/*!40000 ALTER TABLE `characteristic_datatypes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `characteristics`
--

DROP TABLE IF EXISTS `characteristics`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `characteristics` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `unit` varchar(100) NOT NULL,
  `id_ch_datatype` bigint unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_ch_datatype` (`id_ch_datatype`),
  CONSTRAINT `characteristics_ibfk_1` FOREIGN KEY (`id_ch_datatype`) REFERENCES `characteristic_datatypes` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `characteristics`
--

LOCK TABLES `characteristics` WRITE;
/*!40000 ALTER TABLE `characteristics` DISABLE KEYS */;
INSERT INTO `characteristics` VALUES (5,'Вид товара','',1),(6,'Форм-фактор','',1),(7,'Цвет','',1);
/*!40000 ALTER TABLE `characteristics` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `groups`
--

DROP TABLE IF EXISTS `groups`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `groups` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `parent_group_id` bigint unsigned DEFAULT NULL,
  `name` varchar(200) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groups`
--

LOCK TABLES `groups` WRITE;
/*!40000 ALTER TABLE `groups` DISABLE KEYS */;
INSERT INTO `groups` VALUES (1,NULL,'Товары'),(2,NULL,'Услуги'),(3,1,'Мониторы'),(4,1,'Акустика'),(5,1,'Клавиатуры'),(6,1,'Компьютерные мыши'),(7,1,'Системные блоки');
/*!40000 ALTER TABLE `groups` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `manufacturers`
--

DROP TABLE IF EXISTS `manufacturers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `manufacturers` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(200) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `manufacturers`
--

LOCK TABLES `manufacturers` WRITE;
/*!40000 ALTER TABLE `manufacturers` DISABLE KEYS */;
INSERT INTO `manufacturers` VALUES (1,'ООО Контора'),(2,'Xiaomi');
/*!40000 ALTER TABLE `manufacturers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product_characteristics`
--

DROP TABLE IF EXISTS `product_characteristics`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `product_characteristics` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `value` varchar(255) NOT NULL,
  `id_product` bigint unsigned NOT NULL,
  `id_characteristic` bigint unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_product` (`id_product`),
  KEY `id_characteristic` (`id_characteristic`),
  CONSTRAINT `product_characteristics_ibfk_1` FOREIGN KEY (`id_product`) REFERENCES `products` (`id`),
  CONSTRAINT `product_characteristics_ibfk_2` FOREIGN KEY (`id_characteristic`) REFERENCES `characteristics` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product_characteristics`
--

LOCK TABLES `product_characteristics` WRITE;
/*!40000 ALTER TABLE `product_characteristics` DISABLE KEYS */;
INSERT INTO `product_characteristics` VALUES (1,'Наушники',1,5),(2,'Охватывающие',1,6),(3,'Черный',1,7),(6,'Мониторы',3,5),(7,'Черный',3,7),(8,'Мониторы',2,5),(9,'Черный',2,7);
/*!40000 ALTER TABLE `product_characteristics` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `price` int NOT NULL,
  `image_url` varchar(1000) DEFAULT NULL,
  `id_group` bigint unsigned NOT NULL,
  `id_manufacturer` bigint unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_group` (`id_group`),
  KEY `id_manufacturer` (`id_manufacturer`),
  CONSTRAINT `products_ibfk_1` FOREIGN KEY (`id_group`) REFERENCES `groups` (`id`),
  CONSTRAINT `products_ibfk_2` FOREIGN KEY (`id_manufacturer`) REFERENCES `manufacturers` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,'Тестовый товар',100500,'https://mobimg.b-cdn.net/v3/fetch/b7/b76a766ef450b12d3f47b8d5dcd3b0bb.jpeg',4,1),(2,'Xiaomi Mi Curved Gaming Monitor 34',29999,'https://c.dns-shop.ru/thumb/st4/fit/500/500/a2bbde8bb5c85f041e61df441e58ce10/c34451554b2f14a6dad4f5fffde6a46a5656e049958a2cd624392f32cfa5cfd9.jpg',3,2),(3,'Xiaomi Mi 2K Gaming Monitor',29999,'https://c.dns-shop.ru/thumb/st1/fit/500/500/ab6f5bc4f9741d18abd83e73c94a1819/883ed6ff3b68cd93a88abdc3fafc52ee13dbf0ec1418e5e1af72770b891a0298.jpg',3,2);
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products_moving`
--

DROP TABLE IF EXISTS `products_moving`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products_moving` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `id_product` bigint unsigned NOT NULL,
  `id_moving_type` bigint unsigned NOT NULL,
  `action_date` datetime NOT NULL,
  `count` bigint DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id_product` (`id_product`),
  KEY `id_moving_type` (`id_moving_type`),
  CONSTRAINT `products_moving_ibfk_1` FOREIGN KEY (`id_product`) REFERENCES `products` (`id`),
  CONSTRAINT `products_moving_ibfk_2` FOREIGN KEY (`id_moving_type`) REFERENCES `products_moving_type` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products_moving`
--

LOCK TABLES `products_moving` WRITE;
/*!40000 ALTER TABLE `products_moving` DISABLE KEYS */;
INSERT INTO `products_moving` VALUES (1,3,2,'2022-12-18 12:33:13',2),(2,2,3,'2022-12-18 12:33:13',1),(4,2,2,'2022-12-26 21:20:00',10),(5,3,1,'2022-12-27 10:11:33',-1);
/*!40000 ALTER TABLE `products_moving` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products_moving_type`
--

DROP TABLE IF EXISTS `products_moving_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `products_moving_type` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products_moving_type`
--

LOCK TABLES `products_moving_type` WRITE;
/*!40000 ALTER TABLE `products_moving_type` DISABLE KEYS */;
INSERT INTO `products_moving_type` VALUES (1,'Выдача товара'),(2,'Поступление товара'),(3,'Возврат товара');
/*!40000 ALTER TABLE `products_moving_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `request_items`
--

DROP TABLE IF EXISTS `request_items`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `request_items` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `count` bigint NOT NULL DEFAULT '1',
  `id_request` bigint unsigned NOT NULL,
  `id_product` bigint unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_request` (`id_request`),
  KEY `id_product` (`id_product`),
  CONSTRAINT `request_items_ibfk_1` FOREIGN KEY (`id_request`) REFERENCES `requests` (`id`),
  CONSTRAINT `request_items_ibfk_2` FOREIGN KEY (`id_product`) REFERENCES `products` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `request_items`
--

LOCK TABLES `request_items` WRITE;
/*!40000 ALTER TABLE `request_items` DISABLE KEYS */;
INSERT INTO `request_items` VALUES (1,3,1,1),(8,2,4,2),(9,1,4,1),(10,3,5,2),(11,1,6,1),(12,1,7,2),(13,1,7,3),(14,1,8,3);
/*!40000 ALTER TABLE `request_items` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `request_status`
--

DROP TABLE IF EXISTS `request_status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `request_status` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `request_status`
--

LOCK TABLES `request_status` WRITE;
/*!40000 ALTER TABLE `request_status` DISABLE KEYS */;
INSERT INTO `request_status` VALUES (1,'Черновик'),(2,'Создан'),(3,'Выдан'),(4,'Готов к выдаче'),(5,'В обработке'),(6,'Отменен'),(7,'Аннулирован');
/*!40000 ALTER TABLE `request_status` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `request_status_journal`
--

DROP TABLE IF EXISTS `request_status_journal`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `request_status_journal` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `id_request` bigint unsigned DEFAULT NULL,
  `id_request_status` bigint unsigned DEFAULT NULL,
  `status_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `id_request` (`id_request`),
  KEY `id_request_status` (`id_request_status`),
  CONSTRAINT `request_status_journal_ibfk_1` FOREIGN KEY (`id_request`) REFERENCES `requests` (`id`),
  CONSTRAINT `request_status_journal_ibfk_2` FOREIGN KEY (`id_request_status`) REFERENCES `request_status` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `request_status_journal`
--

LOCK TABLES `request_status_journal` WRITE;
/*!40000 ALTER TABLE `request_status_journal` DISABLE KEYS */;
INSERT INTO `request_status_journal` VALUES (1,1,1,'2022-12-11 15:09:52'),(2,1,2,'2022-12-11 15:10:29'),(3,2,1,'2022-12-11 15:09:52'),(4,3,1,'2022-12-11 16:44:39'),(5,4,1,'2022-12-11 18:57:45'),(6,3,1,'2022-12-12 19:54:40'),(7,4,2,'2022-12-12 20:02:35'),(8,5,1,'2022-12-12 20:05:56'),(9,5,2,'2022-12-12 20:06:06'),(10,6,1,'2022-12-12 20:15:19'),(11,6,2,'2022-12-12 20:15:22'),(12,4,6,'2022-12-17 20:03:36'),(13,5,6,'2022-12-17 20:52:56'),(14,6,5,'2022-12-18 11:51:13'),(15,6,7,'2022-12-18 11:51:28'),(16,1,5,'2022-12-18 11:51:37'),(17,7,1,'2022-12-23 11:31:54'),(18,7,2,'2022-12-23 13:14:29'),(19,8,1,'2022-12-27 10:11:12'),(20,8,2,'2022-12-27 10:11:18'),(21,8,5,'2022-12-27 10:11:31'),(22,8,4,'2022-12-27 10:11:32'),(23,8,3,'2022-12-27 10:11:33');
/*!40000 ALTER TABLE `request_status_journal` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `requests`
--

DROP TABLE IF EXISTS `requests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `requests` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `creation_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `period_excution` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `id_user` bigint unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_user` (`id_user`),
  CONSTRAINT `requests_ibfk_1` FOREIGN KEY (`id_user`) REFERENCES `users` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `requests`
--

LOCK TABLES `requests` WRITE;
/*!40000 ALTER TABLE `requests` DISABLE KEYS */;
INSERT INTO `requests` VALUES (1,'2022-12-11 00:00:00','2022-12-11 00:00:00',1),(2,'2022-12-11 00:00:00','2022-12-11 00:00:00',2),(3,'2022-12-12 19:52:22','2022-12-22 19:52:22',1),(4,'2022-12-12 20:02:32','2022-12-22 20:02:32',5),(5,'2022-12-12 20:06:06','2022-12-22 20:06:06',5),(6,'2022-12-12 20:15:22','2022-12-22 20:15:22',5),(7,'2022-12-23 13:14:29','2023-01-02 13:14:29',5),(8,'2022-12-27 10:11:18','2023-01-06 10:11:18',5);
/*!40000 ALTER TABLE `requests` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (1,'User'),(2,'Admin');
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_roles`
--

DROP TABLE IF EXISTS `user_roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_roles` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `id_role` bigint unsigned NOT NULL,
  `id_user` bigint unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_role` (`id_role`),
  KEY `id_user` (`id_user`),
  CONSTRAINT `user_roles_ibfk_1` FOREIGN KEY (`id_role`) REFERENCES `roles` (`id`),
  CONSTRAINT `user_roles_ibfk_2` FOREIGN KEY (`id_user`) REFERENCES `users` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_roles`
--

LOCK TABLES `user_roles` WRITE;
/*!40000 ALTER TABLE `user_roles` DISABLE KEYS */;
INSERT INTO `user_roles` VALUES (1,1,1),(2,1,1),(3,2,5);
/*!40000 ALTER TABLE `user_roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `surname` varchar(50) NOT NULL,
  `name` varchar(50) NOT NULL,
  `patronymic` varchar(50) DEFAULT NULL,
  `phone` varchar(50) NOT NULL,
  `login` varchar(100) NOT NULL,
  `password` varchar(100) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `phone` (`phone`),
  UNIQUE KEY `login` (`login`),
  CONSTRAINT `users_chk_1` CHECK (regexp_like(`phone`,_utf8mb4'^\\+7[0-9]{10}$'))
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'test','test','test','+78885554443','asdf','test'),(2,'asdv','asdv','asdv','+75442212121','asdv','asdv'),(4,'asdv','asdv','asdv','+79999999999','asdvpЫЫЫЫ','asdv'),(5,'Кривчикова','Анастасия','Сергеевна','+78005553535','ezirog','123456');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-12-28 19:14:12
