SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

CREATE SCHEMA IF NOT EXISTS `filmdbtesting` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
USE `filmdbtesting` ;

-- -----------------------------------------------------
-- Table `filmdbtesting`.`films`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `filmdbtesting`.`films` (
  `FilmID` INT NOT NULL AUTO_INCREMENT,
  `FilmName` NVARCHAR(45) NOT NULL,
  `ImdbRating` DECIMAL(3,1) NOT NULL,
  `FilmYear` CHAR(4) NOT NULL,
  `FilmImdbID` CHAR(7) NOT NULL UNIQUE,
  PRIMARY KEY (`FilmID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `filmdbtesting`.`actors`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `filmdbtesting`.`actors` (
  `ActorID` INT NOT NULL AUTO_INCREMENT,
  `ActorName` NVARCHAR(45) NOT NULL,
  `ActorImdbID` CHAR(7) NOT NULL UNIQUE,
  PRIMARY KEY (`ActorID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `filmdbtesting`.`film_actor`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `filmdbtesting`.`film_actor` (
  `id_film_actor` INT NOT NULL AUTO_INCREMENT,
  `FilmID` INT NOT NULL,
  `ActorID` INT NOT NULL,
  PRIMARY KEY (`id_film_actor`),
  INDEX `Film ID_idx` (`FilmID` ASC),
  INDEX `Actor ID_idx` (`ActorID` ASC),
    FOREIGN KEY (`FilmID`)
    REFERENCES `filmdbtesting`.`films` (`FilmID`)
	ON DELETE CASCADE
    ON UPDATE CASCADE,
    FOREIGN KEY (`ActorID`)
    REFERENCES `filmdbtesting`.`actors` (`ActorID`)
	ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `filmdbtesting`.`directors`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `filmdbtesting`.`directors` (
  `DirectorID` INT NOT NULL AUTO_INCREMENT,
  `DirectorName` VARCHAR(45) NOT NULL,
  `DirectorImdbID` CHAR(7) NOT NULL UNIQUE,
  PRIMARY KEY (`DirectorID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `filmdbtesting`.`film_director`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `filmdbtesting`.`film_director` (
  `id_film_director` INT NOT NULL AUTO_INCREMENT,
  `FilmID` INT NULL,
  `DirectorID` INT NULL,
  PRIMARY KEY (`id_film_director`),
  INDEX `Film ID_idx` (`FilmID` ASC),
  INDEX `Director ID_idx` (`DirectorID` ASC),
  CONSTRAINT `FilmIDinD`
    FOREIGN KEY (`FilmID`)
    REFERENCES `filmdbtesting`.`films` (`FilmID`)
	ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `DirectorIDinD`
    FOREIGN KEY (`DirectorID`)
    REFERENCES `filmdbtesting`.`directors` (`DirectorID`)
	ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
