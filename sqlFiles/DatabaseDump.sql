-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: cs-dblab01.uwg.westga.edu    Database: cs3230f24a
-- ------------------------------------------------------
-- Server version	8.4.2

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
-- Table structure for table `administrator`
--

DROP TABLE IF EXISTS `administrator`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `administrator` (
  `admin_id` int NOT NULL AUTO_INCREMENT,
  `emp_id` int NOT NULL,
  PRIMARY KEY (`admin_id`),
  KEY `emp_id` (`emp_id`),
  CONSTRAINT `administrator_ibfk_1` FOREIGN KEY (`emp_id`) REFERENCES `employee` (`employee_id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `administrator`
--

LOCK TABLES `administrator` WRITE;
/*!40000 ALTER TABLE `administrator` DISABLE KEYS */;
INSERT INTO `administrator` VALUES (1,1),(2,2),(3,3),(4,4),(5,5);
/*!40000 ALTER TABLE `administrator` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `appointment`
--

DROP TABLE IF EXISTS `appointment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appointment` (
  `appointment_id` int NOT NULL AUTO_INCREMENT,
  `patient_id` int NOT NULL,
  `doctor_id` int NOT NULL,
  `datetime` datetime NOT NULL,
  `reason` varchar(255) NOT NULL,
  `status` varchar(50) NOT NULL,
  PRIMARY KEY (`appointment_id`),
  UNIQUE KEY `patient_id` (`patient_id`,`datetime`),
  KEY `doctor_id` (`doctor_id`),
  CONSTRAINT `appointment_ibfk_1` FOREIGN KEY (`patient_id`) REFERENCES `patient` (`patient_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `appointment_ibfk_2` FOREIGN KEY (`doctor_id`) REFERENCES `doctor` (`doctor_id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appointment`
--

LOCK TABLES `appointment` WRITE;
/*!40000 ALTER TABLE `appointment` DISABLE KEYS */;
INSERT INTO `appointment` VALUES (1,1,1,'2024-10-01 10:00:00','Annual Checkup','Scheduled'),(2,2,1,'2024-10-02 14:30:00','Follow-up','Scheduled'),(3,3,2,'2024-10-03 09:00:00','Routine Blood Test','Completed'),(4,4,3,'2024-10-05 11:15:00','Consultation','Cancelled'),(5,5,2,'2024-10-07 15:45:00','Vaccination','Scheduled'),(6,6,3,'2024-10-08 13:00:00','Skin Rash Examination','Scheduled'),(7,7,3,'2024-10-09 10:30:00','Chest Pain Follow-up','Completed');
/*!40000 ALTER TABLE `appointment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `doctor`
--

DROP TABLE IF EXISTS `doctor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `doctor` (
  `doctor_id` int NOT NULL AUTO_INCREMENT,
  `emp_id` int NOT NULL,
  PRIMARY KEY (`doctor_id`),
  KEY `emp_id` (`emp_id`),
  CONSTRAINT `doctor_ibfk_1` FOREIGN KEY (`emp_id`) REFERENCES `employee` (`employee_id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `doctor`
--

LOCK TABLES `doctor` WRITE;
/*!40000 ALTER TABLE `doctor` DISABLE KEYS */;
INSERT INTO `doctor` VALUES (1,6),(2,7),(3,8),(4,9),(5,10);
/*!40000 ALTER TABLE `doctor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `doctorspecialty`
--

DROP TABLE IF EXISTS `doctorspecialty`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `doctorspecialty` (
  `doctor_id` int NOT NULL,
  `specialty_id` int NOT NULL,
  PRIMARY KEY (`doctor_id`,`specialty_id`),
  KEY `specialty_id` (`specialty_id`),
  CONSTRAINT `doctorspecialty_ibfk_1` FOREIGN KEY (`doctor_id`) REFERENCES `doctor` (`doctor_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `doctorspecialty_ibfk_2` FOREIGN KEY (`specialty_id`) REFERENCES `specialty` (`specialty_id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `doctorspecialty`
--

LOCK TABLES `doctorspecialty` WRITE;
/*!40000 ALTER TABLE `doctorspecialty` DISABLE KEYS */;
INSERT INTO `doctorspecialty` VALUES (1,1),(2,1),(5,1),(1,2),(3,2),(2,3),(3,3),(5,3),(4,4),(5,4),(4,5);
/*!40000 ALTER TABLE `doctorspecialty` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee`
--

DROP TABLE IF EXISTS `employee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employee` (
  `employee_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int NOT NULL,
  `f_name` varchar(50) NOT NULL,
  `l_name` varchar(50) NOT NULL,
  `dob` date NOT NULL,
  `gender` char(1) NOT NULL,
  `address_1` varchar(100) NOT NULL,
  `address_2` varchar(100) DEFAULT NULL,
  `city` varchar(50) NOT NULL,
  `state` varchar(50) NOT NULL,
  `zipcode` varchar(20) NOT NULL,
  `personal_phone` varchar(20) NOT NULL,
  PRIMARY KEY (`employee_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `employee_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee`
--

LOCK TABLES `employee` WRITE;
/*!40000 ALTER TABLE `employee` DISABLE KEYS */;
INSERT INTO `employee` VALUES (1,1,'John','Doe','1980-01-15','M','123 Main St',NULL,'New York','NY','10001','6785551234'),(2,2,'Jane','Smith','1985-02-20','F','456 Oak St',NULL,'Los Angeles','CA','90001','6785555678'),(3,3,'Jason','Nunez','1982-06-25','M','321 Elm St',NULL,'San Francisco','CA','94101','6785553333'),(4,4,'Alice','Johnson','1990-03-30','F','789 Pine St',NULL,'Chicago','IL','60601','6785559012'),(5,5,'Emma','Williams','1995-07-10','F','951 Maple St',NULL,'Houston','TX','77001','6785554321'),(6,6,'Ethan','Brown','1978-11-12','M','624 Walnut St',NULL,'Phoenix','AZ','85001','6785558765'),(7,7,'Sophia','Miller','1993-05-15','F','847 Cedar St',NULL,'Philadelphia','PA','19101','6785559087'),(8,8,'Liam','Wilson','1989-12-01','M','456 Birch St',NULL,'Seattle','WA','98101','6785556543'),(9,9,'Olivia','Garcia','1997-09-20','F','982 Aspen St',NULL,'Miami','FL','33101','6785557890'),(10,10,'Noah','Martinez','1981-04-18','M','365 Hickory St',NULL,'Denver','CO','80201','6785553210'),(11,11,'Lucas','White','1987-03-12','M','789 Oak St',NULL,'Boston','MA','02101','6785559988'),(12,12,'Isabella','Taylor','1990-06-25','F','567 Pine St',NULL,'Austin','TX','73301','6785557766'),(13,13,'Mia','Harris','1983-12-10','F','234 Cedar Dr',NULL,'Chicago','IL','60601','6785555544'),(14,14,'Elijah','Moore','1979-08-14','M','102 Walnut Ln',NULL,'San Diego','CA','92101','6785553322'),(15,15,'Charlotte','Clark','1992-04-18','F','456 Maple Blvd',NULL,'San Francisco','CA','94101','6785551100');
/*!40000 ALTER TABLE `employee` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nurse`
--

DROP TABLE IF EXISTS `nurse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nurse` (
  `nurse_id` int NOT NULL AUTO_INCREMENT,
  `emp_id` int NOT NULL,
  PRIMARY KEY (`nurse_id`),
  KEY `emp_id` (`emp_id`),
  CONSTRAINT `nurse_ibfk_1` FOREIGN KEY (`emp_id`) REFERENCES `employee` (`employee_id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nurse`
--

LOCK TABLES `nurse` WRITE;
/*!40000 ALTER TABLE `nurse` DISABLE KEYS */;
INSERT INTO `nurse` VALUES (1,11),(2,12),(3,13),(4,14),(5,15);
/*!40000 ALTER TABLE `nurse` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `patient`
--

DROP TABLE IF EXISTS `patient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `patient` (
  `patient_id` int NOT NULL AUTO_INCREMENT,
  `f_name` varchar(50) NOT NULL,
  `l_name` varchar(50) NOT NULL,
  `dob` date NOT NULL,
  `gender` char(1) NOT NULL,
  `address_1` varchar(100) NOT NULL,
  `address_2` varchar(100) DEFAULT NULL,
  `city` varchar(50) NOT NULL,
  `state` varchar(50) NOT NULL,
  `zipcode` varchar(20) NOT NULL,
  `phone` varchar(20) NOT NULL,
  `active` tinyint(1) NOT NULL,
  PRIMARY KEY (`patient_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `patient`
--

LOCK TABLES `patient` WRITE;
/*!40000 ALTER TABLE `patient` DISABLE KEYS */;
INSERT INTO `patient` VALUES (1,'Emily','Brown','1995-04-10','F','234 Maple St',NULL,'Boston','MA','02101','6785551111',1),(2,'Michael','Green','2000-05-15','M','567 Birch St',NULL,'Seattle','WA','98101','6785552222',1),(3,'Sophia','Clark','1987-02-25','F','789 Cedar Ln',NULL,'Austin','TX','73301','6785553333',1),(4,'James','Adams','1979-11-20','M','123 Oak Ave',NULL,'Denver','CO','80201','6785554444',1),(5,'Olivia','White','1992-08-30','F','456 Pine St',NULL,'Miami','FL','33101','6785555555',1),(6,'David','Taylor','1984-03-14','M','321 Elm Blvd',NULL,'Chicago','IL','60601','6785556666',1),(7,'Isabella','Davis','1990-12-05','F','654 Walnut Dr',NULL,'Los Angeles','CA','90001','6785557777',1),(8,'William','Miller','1998-01-22','M','876 Maple St',NULL,'San Francisco','CA','94101','6785558888',1),(9,'Ava','Johnson','2002-07-18','F','345 Birch Blvd',NULL,'Boston','MA','02101','6785559999',1),(10,'Noah','Moore','1993-06-10','M','789 Cedar Ave',NULL,'New York','NY','10001','6785551010',1);
/*!40000 ALTER TABLE `patient` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `specialty`
--

DROP TABLE IF EXISTS `specialty`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `specialty` (
  `specialty_id` int NOT NULL AUTO_INCREMENT,
  `specialty_name` varchar(100) NOT NULL,
  PRIMARY KEY (`specialty_id`),
  UNIQUE KEY `specialty_name` (`specialty_name`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `specialty`
--

LOCK TABLES `specialty` WRITE;
/*!40000 ALTER TABLE `specialty` DISABLE KEYS */;
INSERT INTO `specialty` VALUES (1,'Cardiology'),(3,'Dermatology'),(4,'Neurology'),(5,'Orthopedics'),(2,'Pediatrics');
/*!40000 ALTER TABLE `specialty` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `test`
--

DROP TABLE IF EXISTS `test`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `test` (
  `test_code` int NOT NULL AUTO_INCREMENT,
  `test_name` varchar(100) NOT NULL,
  `low_value` decimal(10,2) DEFAULT NULL,
  `high_value` decimal(10,2) DEFAULT NULL,
  `unit_of_measurement` varchar(30) NOT NULL,
  PRIMARY KEY (`test_code`),
  UNIQUE KEY `test_name` (`test_name`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `test`
--

LOCK TABLES `test` WRITE;
/*!40000 ALTER TABLE `test` DISABLE KEYS */;
INSERT INTO `test` VALUES (1,'Blood Pressure',60.00,120.00,'MillimetersOfMercury'),(2,'Cholesterol',100.00,200.00,'MilligramsPerDeciliter'),(3,'Weight',NULL,NULL,'Kilograms'),(4,'Blood Sugar',70.00,140.00,'MilligramsPerDeciliter'),(5,'Heart Rate',60.00,100.00,'BeatsPerMinute'),(6,'Body Temperature',97.00,99.00,'Fahrenheit'),(7,'Respiratory Rate',12.00,20.00,'BreathsPerMinute'),(8,'Hemoglobin',12.00,18.00,'GramsPerDeciliter');
/*!40000 ALTER TABLE `test` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `testorder`
--

DROP TABLE IF EXISTS `testorder`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `testorder` (
  `test_order_id` int NOT NULL AUTO_INCREMENT,
  `visit_id` int NOT NULL,
  `test_code` int NOT NULL,
  `ordered_datetime` datetime NOT NULL,
  `performed_datetime` datetime DEFAULT NULL,
  `result` decimal(10,2) DEFAULT NULL,
  `abnormal` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`test_order_id`),
  UNIQUE KEY `visit_id` (`visit_id`,`test_code`,`ordered_datetime`),
  KEY `test_code` (`test_code`),
  CONSTRAINT `testorder_ibfk_1` FOREIGN KEY (`visit_id`) REFERENCES `visit` (`visit_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `testorder_ibfk_2` FOREIGN KEY (`test_code`) REFERENCES `test` (`test_code`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `testorder`
--

LOCK TABLES `testorder` WRITE;
/*!40000 ALTER TABLE `testorder` DISABLE KEYS */;
INSERT INTO `testorder` VALUES (1,1,1,'2024-10-01 10:35:00','2024-10-01 10:45:00',110.00,0),(2,1,3,'2024-10-01 10:35:00','2024-10-01 10:50:00',70.50,0),(3,1,6,'2024-10-01 10:35:00','2024-10-01 10:55:00',98.60,0),(4,2,1,'2024-10-02 15:05:00','2024-10-02 15:15:00',118.00,0),(5,2,4,'2024-10-02 15:05:00','2024-10-02 15:20:00',130.00,1),(6,2,5,'2024-10-02 15:05:00','2024-10-02 15:25:00',72.00,0),(7,3,2,'2024-10-03 09:50:00','2024-10-03 10:00:00',190.00,0),(8,3,7,'2024-10-03 09:50:00','2024-10-03 10:05:00',18.00,0),(9,3,8,'2024-10-03 09:50:00','2024-10-03 10:10:00',15.50,0);
/*!40000 ALTER TABLE `testorder` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL,
  `role` varchar(50) NOT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (1,'admin1','75K3eLr+dx6JJFuJ7LwIpEpOFmwGZZkRiB84PURz6U8=','Administrator'),(2,'admin2','bPYV1byqx3g1Ko8fM2DSPwLzTsGC4lmJf9bOSF14cNQ=','Administrator'),(3,'admin3','WQasNhoTfi0oZGXNZYjrtaw/WulVABEAvEFXfD11F2Q=','Administrator'),(4,'admin4','uXhzpA9zq+3Y1oWnzV5fheSpz7g+rCaIZkCggThQEis=','Administrator'),(5,'admin5','iyyG6pzy6k61F/0eBrdPOZ5/7A/vkuO0gqbPLisJICM=','Administrator'),(6,'doc1','75K3eLr+dx6JJFuJ7LwIpEpOFmwGZZkRiB84PURz6U8=','Doctor'),(7,'doc2','bPYV1byqx3g1Ko8fM2DSPwLzTsGC4lmJf9bOSF14cNQ=','Doctor'),(8,'doc3','WQasNhoTfi0oZGXNZYjrtaw/WulVABEAvEFXfD11F2Q=','Doctor'),(9,'doc4','uXhzpA9zq+3Y1oWnzV5fheSpz7g+rCaIZkCggThQEis=','Doctor'),(10,'doc5','iyyG6pzy6k61F/0eBrdPOZ5/7A/vkuO0gqbPLisJICM=','Doctor'),(11,'nurse1','75K3eLr+dx6JJFuJ7LwIpEpOFmwGZZkRiB84PURz6U8=','Nurse'),(12,'nurse2','bPYV1byqx3g1Ko8fM2DSPwLzTsGC4lmJf9bOSF14cNQ=','Nurse'),(13,'nurse3','WQasNhoTfi0oZGXNZYjrtaw/WulVABEAvEFXfD11F2Q=','Nurse'),(14,'nurse4','uXhzpA9zq+3Y1oWnzV5fheSpz7g+rCaIZkCggThQEis=','Nurse'),(15,'nurse5','iyyG6pzy6k61F/0eBrdPOZ5/7A/vkuO0gqbPLisJICM=','Nurse');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `visit`
--

DROP TABLE IF EXISTS `visit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `visit` (
  `visit_id` int NOT NULL AUTO_INCREMENT,
  `appointment_id` int NOT NULL,
  `patient_id` int NOT NULL,
  `doctor_id` int NOT NULL,
  `nurse_id` int NOT NULL,
  `visit_datetime` datetime NOT NULL,
  `weight` decimal(5,2) DEFAULT NULL,
  `height` decimal(5,2) DEFAULT NULL,
  `blood_pressure_systolic` int DEFAULT NULL,
  `blood_pressure_diastolic` int DEFAULT NULL,
  `temperature` decimal(4,2) DEFAULT NULL,
  `pulse` int DEFAULT NULL,
  `symptoms` text,
  `initial_diagnosis` varchar(255) DEFAULT NULL,
  `final_diagnosis` varchar(255) DEFAULT NULL,
  `visit_status` varchar(50) NOT NULL,
  PRIMARY KEY (`visit_id`),
  KEY `appointment_id` (`appointment_id`),
  KEY `patient_id` (`patient_id`),
  KEY `doctor_id` (`doctor_id`),
  KEY `nurse_id` (`nurse_id`),
  CONSTRAINT `visit_ibfk_1` FOREIGN KEY (`appointment_id`) REFERENCES `appointment` (`appointment_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `visit_ibfk_2` FOREIGN KEY (`patient_id`) REFERENCES `patient` (`patient_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `visit_ibfk_3` FOREIGN KEY (`doctor_id`) REFERENCES `doctor` (`doctor_id`) ON DELETE RESTRICT ON UPDATE CASCADE,
  CONSTRAINT `visit_ibfk_4` FOREIGN KEY (`nurse_id`) REFERENCES `nurse` (`nurse_id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `visit`
--

LOCK TABLES `visit` WRITE;
/*!40000 ALTER TABLE `visit` DISABLE KEYS */;
INSERT INTO `visit` VALUES (1,1,1,1,1,'2024-10-01 10:30:00',70.50,170.00,120,80,36.50,70,'Fatigue, headache','Common Cold','Recovering','Completed'),(2,2,2,1,2,'2024-10-02 15:00:00',65.00,165.00,118,78,36.60,72,'Fever, sore throat','Flu','Resolved','Completed'),(3,3,3,2,3,'2024-10-03 09:45:00',85.00,180.00,130,85,37.00,80,'Shortness of breath','Asthma','Under control','Completed'),(4,4,4,3,4,'2024-10-05 11:30:00',55.00,160.00,115,75,36.80,68,'Skin irritation','Eczema','Improving','Completed'),(5,5,5,2,1,'2024-10-07 16:00:00',77.00,175.00,125,82,36.70,75,'Fever, cough','Bronchitis','Under treatment','Ongoing'),(6,6,6,2,2,'2024-10-08 13:30:00',68.00,172.00,118,79,36.50,70,'Rash, itching','Allergic Reaction','Resolved','Completed'),(7,7,7,3,3,'2024-10-09 10:45:00',90.00,185.00,135,90,37.50,85,'Chest pain','Heartburn','Resolved','Completed');
/*!40000 ALTER TABLE `visit` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-12-01 12:45:29
