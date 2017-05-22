SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

CREATE SCHEMA IF NOT EXISTS `filmdb` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
USE `filmdb` ;

-- -----------------------------------------------------
-- Table `filmdb`.`films`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `filmdb`.`films` (
  `FilmID` INT NOT NULL AUTO_INCREMENT,
  `FilmName` VARCHAR(45) NOT NULL,
  `ImdbRating` DECIMAL(2,1) NOT NULL,
  `FilmYear` CHAR(4) NOT NULL,
  `FilmImdbID` CHAR(7) NOT NULL,
  PRIMARY KEY (`FilmID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `filmdb`.`actors`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `filmdb`.`actors` (
  `ActorID` INT NOT NULL AUTO_INCREMENT,
  `ActorName` VARCHAR(45) NOT NULL,
  `ActorImdbID` CHAR(7) NOT NULL,
  PRIMARY KEY (`ActorID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `filmdb`.`film_actor`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `filmdb`.`film_actor` (
  `id_film_actor` INT NOT NULL AUTO_INCREMENT,
  `FilmID` INT NOT NULL,
  `ActorID` INT NOT NULL,
  PRIMARY KEY (`id_film_actor`),
  INDEX `Film ID_idx` (`FilmID` ASC),
  INDEX `Actor ID_idx` (`ActorID` ASC),
  CONSTRAINT `FilmIDinA`
    FOREIGN KEY (`FilmID`)
    REFERENCES `filmdb`.`films` (`FilmID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `ActorIDinA`
    FOREIGN KEY (`ActorID`)
    REFERENCES `filmdb`.`actors` (`ActorID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `filmdb`.`directors`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `filmdb`.`directors` (
  `DirectorID` INT NOT NULL AUTO_INCREMENT,
  `DirectorName` VARCHAR(45) NOT NULL,
  `DirectorImdbID` CHAR(7) NOT NULL,
  PRIMARY KEY (`DirectorID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `filmdb`.`film_director`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `filmdb`.`film_director` (
  `id_film_director` INT NOT NULL AUTO_INCREMENT,
  `FilmID` INT NULL,
  `DirectorID` INT NULL,
  PRIMARY KEY (`id_film_director`),
  INDEX `Film ID_idx` (`FilmID` ASC),
  INDEX `Director ID_idx` (`DirectorID` ASC),
  CONSTRAINT `FilmIDinD`
    FOREIGN KEY (`FilmID`)
    REFERENCES `filmdb`.`films` (`FilmID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `DirectorIDinD`
    FOREIGN KEY (`DirectorID`)
    REFERENCES `filmdb`.`directors` (`DirectorID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
