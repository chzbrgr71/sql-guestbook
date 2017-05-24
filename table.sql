CREATE DATABASE sql_guestbook;

USE sql_guestbook;

CREATE TABLE guestlog (entrydate DATETIME, name NVARCHAR(30), phone NVARCHAR(30), message TEXT, sentiment_score NVARCHAR(30));

# Seed values
INSERT INTO guestlog VALUES ('2017-5-2 23:59:59', 'Andy Dufresne', '215-837-9120', 'Get busy living, or get busy dying', '99.5');
INSERT INTO guestlog VALUES ('2017-5-1 23:59:59', 'John Lennon', '412-248-1101', 'I am the walrus', '84.1');
INSERT INTO guestlog VALUES ('2017-4-15 23:59:59', 'Jeff Lebowski', '919-231-0925', 'That rug really tied the room together', '66.2');
INSERT INTO guestlog VALUES ('2016-12-15 23:59:59', 'Tyler Durden', '814-867-5309', 'The first rule about Fight Club is we do not talk about Fight Club', '23.9');