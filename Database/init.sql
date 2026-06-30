-- MySQL dump 10.13  Distrib 8.4.8, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: veterinaria_antioquia
-- ------------------------------------------------------
-- Server version	8.4.8

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
-- Table structure for table `citas`
--

DROP TABLE IF EXISTS `citas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `citas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int NOT NULL,
  `id_mascota` int DEFAULT NULL,
  `id_veterinario` int NOT NULL,
  `id_servicio` int NOT NULL,
  `fecha` datetime NOT NULL,
  `estado` enum('programada','confirmada','completada','cancelada','no_asistio') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'programada',
  `motivo` text COLLATE utf8mb4_unicode_ci,
  `duracion_minutos` int DEFAULT '30',
  PRIMARY KEY (`id`),
  KEY `fk_cita_mascota` (`id_mascota`),
  KEY `fk_citas_servicios` (`id_servicio`),
  KEY `fk_cita_veterinario` (`id_veterinario`),
  KEY `fk_citas_usuario` (`id_usuario`),
  CONSTRAINT `fk_cita_mascota` FOREIGN KEY (`id_mascota`) REFERENCES `mascotas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_citas_servicios` FOREIGN KEY (`id_servicio`) REFERENCES `servicios` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `fk_citas_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `fk_citas_veterinario` FOREIGN KEY (`id_veterinario`) REFERENCES `veterinarios` (`id_usuario`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `citas`
--

LOCK TABLES `citas` WRITE;
/*!40000 ALTER TABLE `citas` DISABLE KEYS */;
INSERT INTO `citas` VALUES (1,17,NULL,2,1,'2026-06-14 14:30:00','cancelada',NULL,30),(2,17,NULL,2,1,'2026-06-17 12:00:00','cancelada','POtente',30),(3,17,NULL,2,1,'2026-06-17 11:00:00','cancelada',NULL,30),(4,17,NULL,2,2,'2026-06-17 12:00:00','cancelada',NULL,45),(5,17,NULL,2,2,'2026-06-18 12:00:00','cancelada',NULL,45),(6,17,NULL,2,1,'2026-06-16 12:00:00','cancelada',NULL,30),(7,17,NULL,2,1,'2026-06-26 17:30:00','programada',NULL,30),(8,17,1,2,1,'2026-06-27 07:00:00','programada',NULL,30);
/*!40000 ALTER TABLE `citas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `diagnosticos`
--

DROP TABLE IF EXISTS `diagnosticos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `diagnosticos` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_cita` int NOT NULL,
  `id_veterinario` int NOT NULL,
  `sintomas` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `diagnostico` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `observaciones` text COLLATE utf8mb4_unicode_ci,
  `fecha` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_cita` (`id_cita`),
  KEY `fk_diag_vet` (`id_veterinario`),
  CONSTRAINT `fk_diag_cita` FOREIGN KEY (`id_cita`) REFERENCES `citas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_diagnosticos_veterinario` FOREIGN KEY (`id_veterinario`) REFERENCES `veterinarios` (`id_usuario`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `diagnosticos`
--

LOCK TABLES `diagnosticos` WRITE;
/*!40000 ALTER TABLE `diagnosticos` DISABLE KEYS */;
/*!40000 ALTER TABLE `diagnosticos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `especies`
--

DROP TABLE IF EXISTS `especies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `especies` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre_comun` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `nombre_cientifico` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `caracteristicas` text COLLATE utf8mb4_unicode_ci,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `especies`
--

LOCK TABLES `especies` WRITE;
/*!40000 ALTER TABLE `especies` DISABLE KEYS */;
INSERT INTO `especies` VALUES (1,'Perro','Canis Lupus','De prueba nnonma');
/*!40000 ALTER TABLE `especies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `factura_detalles`
--

DROP TABLE IF EXISTS `factura_detalles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `factura_detalles` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_factura` int NOT NULL,
  `id_medicamento` int DEFAULT NULL,
  `id_servicio` int DEFAULT NULL,
  `descripcion` varchar(200) COLLATE utf8mb4_unicode_ci NOT NULL,
  `cantidad` int NOT NULL DEFAULT '1',
  `precio_unitario` decimal(10,2) NOT NULL,
  `subtotal` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_det_factura` (`id_factura`),
  KEY `fk_det_med` (`id_medicamento`),
  KEY `fk_detalles_servicio` (`id_servicio`),
  CONSTRAINT `fk_det_factura` FOREIGN KEY (`id_factura`) REFERENCES `facturas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_det_med` FOREIGN KEY (`id_medicamento`) REFERENCES `medicamentos` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_detalles_servicio` FOREIGN KEY (`id_servicio`) REFERENCES `servicios` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `factura_detalles`
--

LOCK TABLES `factura_detalles` WRITE;
/*!40000 ALTER TABLE `factura_detalles` DISABLE KEYS */;
/*!40000 ALTER TABLE `factura_detalles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `facturas`
--

DROP TABLE IF EXISTS `facturas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `facturas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int NOT NULL,
  `id_cita` int DEFAULT NULL,
  `total` decimal(10,2) NOT NULL DEFAULT '0.00',
  `estado` enum('pendiente','pagada','cancelada') COLLATE utf8mb4_unicode_ci DEFAULT 'pendiente',
  `fecha_emision` datetime DEFAULT CURRENT_TIMESTAMP,
  `fecha_pago` datetime DEFAULT NULL,
  `metodo_pago` enum('efectivo','tarjeta','transferencia') COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_factura_cliente` (`id_usuario`),
  KEY `fk_factura_cita` (`id_cita`),
  CONSTRAINT `fk_factura_cita` FOREIGN KEY (`id_cita`) REFERENCES `citas` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `fk_facturas_usuarios` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `facturas`
--

LOCK TABLES `facturas` WRITE;
/*!40000 ALTER TABLE `facturas` DISABLE KEYS */;
/*!40000 ALTER TABLE `facturas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventarios`
--

DROP TABLE IF EXISTS `inventarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inventarios` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_medicamento` int NOT NULL,
  `cantidad` int NOT NULL DEFAULT '0',
  `cantidad_minima` int DEFAULT '5',
  `fecha_vencimiento` date DEFAULT NULL,
  `lote` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `fecha_actualizacion` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_medicamento` (`id_medicamento`),
  CONSTRAINT `fk_inv_med` FOREIGN KEY (`id_medicamento`) REFERENCES `medicamentos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventarios`
--

LOCK TABLES `inventarios` WRITE;
/*!40000 ALTER TABLE `inventarios` DISABLE KEYS */;
/*!40000 ALTER TABLE `inventarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mascotas`
--

DROP TABLE IF EXISTS `mascotas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mascotas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `fecha_nacimiento` date DEFAULT NULL,
  `sexo` enum('macho','hembra') COLLATE utf8mb4_unicode_ci NOT NULL,
  `esterilizado` tinyint(1) DEFAULT '0',
  `id_especie` int NOT NULL,
  `id_usuario` int NOT NULL,
  `foto_url` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `fecha_registro` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `fk_mascota_especie` (`id_especie`),
  KEY `fk_mascota_cliente` (`id_usuario`),
  CONSTRAINT `fk_mascota_especie` FOREIGN KEY (`id_especie`) REFERENCES `especies` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `fk_mascotas_usuarios` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id`) ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mascotas`
--

LOCK TABLES `mascotas` WRITE;
/*!40000 ALTER TABLE `mascotas` DISABLE KEYS */;
INSERT INTO `mascotas` VALUES (1,'ALejo','2026-06-03','hembra',1,1,17,NULL,'2026-06-27 09:31:32');
/*!40000 ALTER TABLE `mascotas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `medicamentos`
--

DROP TABLE IF EXISTS `medicamentos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `medicamentos` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `tipo` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `descripcion` text COLLATE utf8mb4_unicode_ci,
  `contraindicaciones` text COLLATE utf8mb4_unicode_ci,
  `fabricante` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `precio` decimal(10,2) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `medicamentos`
--

LOCK TABLES `medicamentos` WRITE;
/*!40000 ALTER TABLE `medicamentos` DISABLE KEYS */;
/*!40000 ALTER TABLE `medicamentos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notificaciones`
--

DROP TABLE IF EXISTS `notificaciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notificaciones` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int NOT NULL,
  `tipo` enum('cita','vacuna','stock','general') COLLATE utf8mb4_unicode_ci NOT NULL,
  `titulo` varchar(200) COLLATE utf8mb4_unicode_ci NOT NULL,
  `mensaje` text COLLATE utf8mb4_unicode_ci NOT NULL,
  `leida` tinyint(1) DEFAULT '0',
  `fecha_envio` datetime DEFAULT CURRENT_TIMESTAMP,
  `fecha_lectura` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_notif_usuario` (`id_usuario`),
  CONSTRAINT `fk_notif_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notificaciones`
--

LOCK TABLES `notificaciones` WRITE;
/*!40000 ALTER TABLE `notificaciones` DISABLE KEYS */;
/*!40000 ALTER TABLE `notificaciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `servicios`
--

DROP TABLE IF EXISTS `servicios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `servicios` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `descripcion` text COLLATE utf8mb4_unicode_ci,
  `categoria` enum('clinica','estetica','cirugia','vacunacion','laboratorio','otro') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'otro',
  `precio_base` decimal(10,2) NOT NULL,
  `duracion_estimada_min` int NOT NULL DEFAULT '30',
  `activo` tinyint(1) NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `servicios`
--

LOCK TABLES `servicios` WRITE;
/*!40000 ALTER TABLE `servicios` DISABLE KEYS */;
INSERT INTO `servicios` VALUES (1,'Consulta General','Revisión física de rutina, chequeo de peso y signos vitales.','clinica',50000.00,30,1),(2,'Consulta Especialista','Revisión con especialista (ortopedia, dermatología, etc.).','clinica',80000.00,45,1),(3,'Control','Cita de seguimiento para evaluar la evolución de un tratamiento.','clinica',25000.00,15,1),(4,'Baño y Peluquería Pequeños','Baño, secado, corte de pelo y limpieza de oídos para mascotas < 10kg.','estetica',45000.00,60,1),(5,'Baño y Peluquería Grandes','Baño, secado, corte de pelo y limpieza de oídos para mascotas > 20kg.','estetica',70000.00,90,1),(6,'Corte de Uñas','Corte o limado de uñas preventivo.','estetica',15000.00,15,1),(7,'Vacuna Hexavalente','Protección contra moquillo, parvovirus, hepatitis, leptospirosis, etc.','vacunacion',45000.00,15,1),(8,'Vacuna Antirrábica','Dosis anual obligatoria contra la rabia.','vacunacion',25000.00,15,1),(9,'Cuadro Hemático','Toma de muestra de sangre para análisis de laboratorio básico.','laboratorio',40000.00,15,1),(10,'Ecografía Abdominal','Examen por ultrasonido de la cavidad abdominal.','laboratorio',90000.00,30,1),(11,'Esterilización Gato/a','Cirugía ambulatoria de castración o esterilización felina.','cirugia',120000.00,120,1),(12,'Esterilización Perro/a','Cirugía ambulatoria de castración o esterilización canina.','cirugia',180000.00,150,1),(13,'Profilaxis Dental','Limpieza dental profunda bajo sedación.','cirugia',110000.00,90,1);
/*!40000 ALTER TABLE `servicios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sesiones`
--

DROP TABLE IF EXISTS `sesiones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sesiones` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_usuario` int NOT NULL,
  `token` varchar(255) NOT NULL,
  `fecha_creacion` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `id_usuario` (`id_usuario`),
  CONSTRAINT `sesiones_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sesiones`
--

LOCK TABLES `sesiones` WRITE;
/*!40000 ALTER TABLE `sesiones` DISABLE KEYS */;
INSERT INTO `sesiones` VALUES (1,12,'30b5ef2e0525be9b482832246079a5ec70a95dc65842b49751c6b4749cbcc795','2026-06-08 01:15:56'),(2,12,'3531419056f71004c35d1db9386690554fb32fc7ff71be954c521c23fab12674','2026-06-08 01:20:42');
/*!40000 ALTER TABLE `sesiones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tratamientos`
--

DROP TABLE IF EXISTS `tratamientos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tratamientos` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_diagnostico` int NOT NULL,
  `id_medicamento` int NOT NULL,
  `dosis` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `frecuencia` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `duracion_dias` int NOT NULL,
  `instrucciones` text COLLATE utf8mb4_unicode_ci,
  `estado` enum('activo','completado','cancelado') COLLATE utf8mb4_unicode_ci DEFAULT 'activo',
  `fecha_inicio` date NOT NULL,
  `fecha_fin` date DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_trata_diag` (`id_diagnostico`),
  KEY `fk_trata_med` (`id_medicamento`),
  CONSTRAINT `fk_trata_diag` FOREIGN KEY (`id_diagnostico`) REFERENCES `diagnosticos` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_trata_med` FOREIGN KEY (`id_medicamento`) REFERENCES `medicamentos` (`id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tratamientos`
--

LOCK TABLES `tratamientos` WRITE;
/*!40000 ALTER TABLE `tratamientos` DISABLE KEYS */;
/*!40000 ALTER TABLE `tratamientos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `email` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `telefono` varchar(15) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `direccion` varchar(200) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `preferencias` text COLLATE utf8mb4_unicode_ci,
  `rol` enum('administrador','veterinario','cliente') COLLATE utf8mb4_unicode_ci NOT NULL,
  `password_hash` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `fecha_registro` datetime DEFAULT CURRENT_TIMESTAMP,
  `activo` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`id`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (1,'Alejandro Gg','a.alejo12.a@gmail.com','3105022470','LLano BBolívar 456',NULL,'cliente','ee9a8dd4b6ef13326a77657bf08af2114aa2e951cb8a1a16341681f469b7b330','2026-06-06 23:29:32',1),(2,'Amar2','a.alejo102.a@gmail.com','3105606196','CL 20 # 5 - 131',NULL,'veterinario','$2b$09$uTE8h7bUA5wxkPXKpSf/lOtLvhNmumB3Q3rQAwPgy53zzmk38TggO','2026-06-07 13:54:32',1),(3,'Luis','ae@gmail.com','31050221','Lanno2',NULL,'cliente','5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5','2026-06-07 13:55:42',0),(4,'Test3','a@gg.com','321','asdk',NULL,'administrador','d7aa5d490431210c38d28cb06b6abe9ced534443d5ef46ca4129f89a53585f6a','2026-06-07 14:05:49',0),(5,'Alejandro','ale@example.com','555-0199','Calle Luna 123',NULL,'administrador','bd94dcda26fccb4e68d6a31f9b5aac0b571ae266d822620e901ef7ebe3a11d4f','2026-06-07 21:05:19',1),(6,'Alejandro','ale@gmail.com','310539487','CR 123 #20-10',NULL,'administrador','bd94dcda26fccb4e68d6a31f9b5aac0b571ae266d822620e901ef7ebe3a11d4f','2026-06-07 21:14:59',1),(8,'Alejandro G21','aleejo2@gmail.com','310539487','CR 123 #20-10',NULL,'administrador','bd94dcda26fccb4e68d6a31f9b5aac0b571ae266d822620e901ef7ebe3a11d4f','2026-06-07 21:29:51',1),(9,'Alejandro','testeos12@gmail.com','3105','CL 20 # 5 - 131',NULL,'cliente','$2y$10$Ss8mNhP32Fn29tgqLlAia.uUtAEaYvgEYTwtT7UEZvgsRxRcG/kx.','2026-06-07 17:52:27',1),(10,'hola','g@g.com','1234567','CL 20 # 5 - 131',NULL,'cliente','$2y$10$eBlUjyyCuC04BA8f8LJMl.qZ3rQgpoY4OXZY65ODkbRpYuQRYwQMG','2026-06-07 20:06:02',1),(11,'asdfdadssada','asdfdadssada@gmail.com','3213912','skdalas',NULL,'cliente','$2y$10$B92ythhmyZ5qZdRGQGKXM.ySdkJVJBu5rP3G22isHHk8FyWw3ddhi','2026-06-07 20:06:21',1),(12,'probando2','probando2@gmail.com','231','',NULL,'cliente','$2y$10$0F4LUh2H8SfsuOFC2HsmmuHr9QRnpYe8b423SE9LkFioS26VUrMPy','2026-06-07 20:12:18',1),(13,'Zuñiga Veterinario','zuniga@veterinaria.com','3001234567','Bello, Antioquia',NULL,'veterinario','$2b$10$RhMuuHs.6xZQp7sUR6FThuW2rp.GmdfkLFv2RGIaIQe9DnBmmCp.S','2026-06-08 23:44:12',1),(14,'Zuñiga Veterinario','zuniga1@veterinaria.com','3001234567','Bello, Antioquia',NULL,'veterinario','$2b$10$w4B6AFlm5tOqGdBEBRJReuXywPpviDr0issHxHuKpV5dHXi/GzXSO','2026-06-08 23:45:45',1),(15,'Alejandro Guzman ','1@g.com',NULL,NULL,NULL,'cliente','$2b$09$uqesYrqhz6tJJrFQk3DiMebv5jiYRDV2sxroAhzvB1kKo8zo7NGKm','2026-06-11 20:17:18',1),(16,'Alejandro Guzman','2@gmail.com',NULL,NULL,NULL,'cliente','$2b$09$MhWrOC2EHSKWH54X8cnt6erJU1/Ys8uy5.jeDeljkiNya8OhUISLu','2026-06-11 20:20:39',1),(17,'Alejandro Guzman','1@gmail.com',NULL,NULL,NULL,'administrador','$2b$09$uTE8h7bUA5wxkPXKpSf/lOtLvhNmumB3Q3rQAwPgy53zzmk38TggO','2026-06-12 16:53:03',1);
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vacunaciones`
--

DROP TABLE IF EXISTS `vacunaciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vacunaciones` (
  `id` int NOT NULL AUTO_INCREMENT,
  `id_mascota` int NOT NULL,
  `id_vacuna` int NOT NULL,
  `id_veterinario` int NOT NULL,
  `fecha_aplicacion` date NOT NULL,
  `fecha_proxima` date NOT NULL,
  `lote` varchar(50) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `observaciones` text COLLATE utf8mb4_unicode_ci,
  PRIMARY KEY (`id`),
  KEY `fk_vacunacion_mascota` (`id_mascota`),
  KEY `fk_vacunacion_vacuna` (`id_vacuna`),
  KEY `fk_vacunacion_vet` (`id_veterinario`),
  CONSTRAINT `fk_vacunacion_mascota` FOREIGN KEY (`id_mascota`) REFERENCES `mascotas` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_vacunacion_vacuna` FOREIGN KEY (`id_vacuna`) REFERENCES `vacunas` (`id`) ON UPDATE CASCADE,
  CONSTRAINT `fk_vacunaciones_veterinario` FOREIGN KEY (`id_veterinario`) REFERENCES `veterinarios` (`id_usuario`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vacunaciones`
--

LOCK TABLES `vacunaciones` WRITE;
/*!40000 ALTER TABLE `vacunaciones` DISABLE KEYS */;
/*!40000 ALTER TABLE `vacunaciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vacunas`
--

DROP TABLE IF EXISTS `vacunas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vacunas` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `descripcion` text COLLATE utf8mb4_unicode_ci,
  `contraindicaciones` text COLLATE utf8mb4_unicode_ci,
  `duracion_proteccion_meses` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vacunas`
--

LOCK TABLES `vacunas` WRITE;
/*!40000 ALTER TABLE `vacunas` DISABLE KEYS */;
/*!40000 ALTER TABLE `vacunas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `veterinarios`
--

DROP TABLE IF EXISTS `veterinarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `veterinarios` (
  `id_usuario` int NOT NULL,
  `especialidad` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `licencia_profesional` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`id_usuario`),
  CONSTRAINT `fk_veterinario_usuario` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `veterinarios`
--

LOCK TABLES `veterinarios` WRITE;
/*!40000 ALTER TABLE `veterinarios` DISABLE KEYS */;
INSERT INTO `veterinarios` VALUES (2,'TEsteo','Testeo');
/*!40000 ALTER TABLE `veterinarios` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-06-29 19:00:51
