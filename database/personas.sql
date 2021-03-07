-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema personas
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema personas
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `personas` DEFAULT CHARACTER SET utf8 ;
USE `personas` ;

-- -----------------------------------------------------
-- Table `personas`.`persona`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `personas`.`persona` (
  `curp` VARCHAR(20) NOT NULL,
  `nombres` VARCHAR(50) NOT NULL,
  `apellidos` VARCHAR(90) NOT NULL,
  PRIMARY KEY (`curp`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

LOCK TABLES `persona` WRITE;
/*!40000 ALTER TABLE `persona` DISABLE KEYS */;
INSERT INTO `persona` VALUES ('GUCS991001HVZDHM09','Sammy','Guadarrama');
/*!40000 ALTER TABLE `persona` ENABLE KEYS */;
UNLOCK TABLES;


-- -----------------------------------------------------
-- Table `personas`.`email`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `personas`.`email` (
  `idemail` INT(11) NOT NULL AUTO_INCREMENT,
  `email` VARCHAR(60) NOT NULL,
  `curp` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`idemail`),
  INDEX `fk_email_persona1_idx` (`curp` ASC) VISIBLE,
  CONSTRAINT `fk_email_persona1`
    FOREIGN KEY (`curp`)
    REFERENCES `personas`.`persona` (`curp`))
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8;

LOCK TABLES `email` WRITE;
/*!40000 ALTER TABLE `email` DISABLE KEYS */;
INSERT INTO `email` VALUES (1,'sammy@gmail.com','GUCS991001HVZDHM09');
/*!40000 ALTER TABLE `email` ENABLE KEYS */;
UNLOCK TABLES;

-- -----------------------------------------------------
-- Table `personas`.`telefono`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `personas`.`telefono` (
  `idtelefono` INT(11) NOT NULL AUTO_INCREMENT,
  `telefono` VARCHAR(10) NOT NULL,
  `curp` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`idtelefono`),
  INDEX `fk_telefono_persona_idx` (`curp` ASC) VISIBLE,
  CONSTRAINT `fk_telefono_persona`
    FOREIGN KEY (`curp`)
    REFERENCES `personas`.`persona` (`curp`))
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8;

LOCK TABLES `telefono` WRITE;
/*!40000 ALTER TABLE `telefono` DISABLE KEYS */;
INSERT INTO `telefono` VALUES (1,'2881001212','GUCS991001HVZDHM09');
/*!40000 ALTER TABLE `telefono` ENABLE KEYS */;
UNLOCK TABLES;

GRANT ALL PRIVILEGES ON `personas`.* TO 'adminPersonas'@'%' IDENTIFIED BY 'practica1';

-- CREATE USER 'adminPersonas' IDENTIFIED BY 'practica1';

-- GRANT ALL ON `personas`.* TO 'adminPersonas';
-- GRANT SELECT ON TABLE `personas`.* TO 'adminPersonas';
-- GRANT SELECT, INSERT, TRIGGER ON TABLE `personas`.* TO 'adminPersonas';
-- GRANT SELECT, INSERT, TRIGGER, UPDATE, DELETE ON TABLE `personas`.* TO 'adminPersonas';
/*GRANT EXECUTE ON ROUTINE `personas`.* TO 'adminPersonas';*/

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;