//location populated
INSERT INTO location(name, distance_to_beach, distance_to_city) 
VALUES  ('tangalooma', 50, 250),
('greenside', 100, 200),
('rivergate', 150, 150),
('azurebliss', 200, 100), 
('crystalcove', 250, 50);

// Rooms populated
INSERT INTO rooms (size, location_name, price, reviews)
VALUES (2,'tangalooma',1550.99,4.4),
(1,'rivergate',1405.99,3.6),
(2,'tangalooma',1395.99,3.9),
(1,'tangalooma',1564.99,4.6),
(1,'azurebliss',1997.99,4.1),
(3,'rivergate',1676.99,3.2),
(1,'azurebliss',1134.99,4.7),
(1,'greenside',1657.99,4.9),
(1,'tangalooma',1295.99,4.8),
(3,'tangalooma',856.99,4.4),
(1,'rivergate',1170.99,4.8),
(1,'greenside',1038.99,4.1),
(2,'tangalooma',1922.99,4.6),
(3,'azurebliss',1342.99,4.5),
(1,'rivergate',1824.99,4.5),
(3,'tangalooma',153.99,2.3),
(1,'crystalcove',1531.99,2.0),
(2,'greenside',1970.99,2.5),
(1,'azurebliss',703.99,2.7),
(3,'rivergate',1217.99,3.4),
(2,'tangalooma',1981.99,4.4),
(3,'tangalooma',1149.99,3.6),
(2,'greenside',726.99,4.0),
(2,'azurebliss',809.99,2.7),
(1,'azurebliss',877.99,4.0),
(2,'azurebliss',1762.99,4.7),
(1,'rivergate',898.99,2.5),
(1,'greenside',1639.99,2.6),
(1,'tangalooma',890.99,2.5),
(3,'tangalooma',1350.99,1.7);

//customer populated
INSERT INTO customers (first_name, last_name, email, telnumber, date_of_birth) VALUES
('Emlen', 'Desborough', 'edesborough0@sogou.com', '0714142806', '1990-03-23'),
('Nestor', 'Mugglestone', 'nmugglestone1@myspace.com', '0787378765', '1982-08-24'),
('Becka', 'Giamelli', 'bgiamelli2@xinhuanet.com', '0781442357', '1953-08-26'),
('Tadio', 'Lakenton', 'tlakenton3@desdev.cn', '0705905342', '1984-11-16'),
('Hynda', 'Matyugin', 'hmatyugin4@google.es', '0782263907', '1962-08-08'),
('Glad', 'Darbon', 'gdarbon5@fema.gov', '0738191067', '1967-08-05'),
('Maurizio', 'Corrington', 'mcorrington6@typepad.com', '0701410517', '1995-12-06'),
('Lissy', 'Tett', 'ltett7@jiathis.com', '0712843656', '1957-09-11'),
('Heloise', 'Ellams', 'hellams8@ted.com', '0759200035', '2001-01-02'),
('Darnell', 'Batters', 'dbatters9@uol.com.br', '0767458303', '1971-09-30'),
('Dre', 'Whiston', 'dwhistona@phoca.cz', '0787268917', '1960-04-19'),
('Fawne', 'O Roan', 'foroanb@hubpages.com', '0702030179', '1950-08-31'),
('Charity', 'Mowbury', 'cmowburyc@arstechnica.com', '0712035175', '1966-09-10'),
('Ailina', 'Gostick', 'agostickd@storify.com', '0718538751', '1989-06-12'),
('Leonid', 'Keaveney', 'lkeaveneye@qq.com', '0767766424', '1979-05-22'),
('Cordi', 'Osipov', 'cosipovf@hibu.com', '0723132011', '1973-04-24'),
('Merill', 'Toll', 'mtollg@tiny.cc', '0707753223', '1947-08-26'),
('Yehudi', 'Mowen', 'ymowenh@hubpages.com', '0762386408', '1992-08-12'),
('Cody', 'Champley', 'cchampleyi@ft.com', '0738666472', '1993-04-29'),
('Prentiss', 'Bunnell', 'pbunnellj@mozilla.org', '0751696819', '1953-11-28'),
('Tobit', 'Crickett', 'tcrickettk@edublogs.org', '0714637026', '1999-11-22'),
('Lucilia', 'Pottes', 'lpottesl@ftc.gov', '0736887023', '1964-07-18'),
('Aundrea', 'Darnborough', 'adarnboroughm@cam.ac.uk', '0783655269', '1968-04-20'),
('Kahaleel', 'Vargas', 'kvargasn@unc.edu', '0788863097', '1973-11-06'),
('Avigdor', 'Ambrogiotti', 'aambrogiottio@t.co', '0754877965', '1982-02-15'),
('Rodney', 'Tait', 'rtaitp@mapquest.com', '0779158546', '1930-04-03'),
('Mimi', 'Biaggelli', 'mbiaggelliq@illinois.edu', '0778292832', '1989-05-31'),
('Verile', 'Prys', 'vprysr@ifeng.com', '0715437318', '1981-08-25'),
('Mabel', 'Villaron', 'mvillarons@fastcompany.com', '0794207289', '1959-12-31'),
('Joseito', 'Tollemache', 'jtollemachet@go.com', '0765546514', '1984-02-17'),
('Lockwood', 'Klemps', 'lklempsu@phpbb.com', '0761285384', '1975-04-16'),
('Skipper', 'Swindells', 'sswindellsv@cargocollective.com', '0753353600', '1949-03-31'),
('Dilly', 'Scolts', 'dscoltsw@moonfruit.com', '0774990835', '1977-08-31'),
('Kellsie', 'Del Dello', 'kdelx@zimbio.com', '0704879834', '1990-05-31'),
('Hestia', 'Hovy', 'hhovyy@hud.gov', '0712660745', '2000-09-25'),
('Michaeline', 'Frowen', 'mfrowenz@constantcontact.com', '0753965827', '1994-09-28'),
('Darleen', 'Blondin', 'dblondin10@ameblo.jp', '0755843234', '1993-08-28'),
('Cammie', 'Ert', 'cert11@storify.com', '0775480214', '1955-08-17'),
('Sandy', 'Guillem', 'sguillem12@baidu.com', '0753494808', '1981-08-16'),
('Caitlin', 'Steinson', 'csteinson13@nymag.com', '0725490054', '1969-06-24'),
('Hewet', 'Hurich', 'hhurich14@artisteer.com', '0743838821', '1965-04-30'),
('Chiarra', 'Burger', 'cburger15@google.fr', '0742251785', '1942-12-18'),
('Herbert', 'Isaq', 'hisaq16@ucsd.edu', '0707869444', '1930-07-17'),
('Lissa', 'O Concannon', 'lo17@webnode.com', '0736201110', '1999-10-19'),
('Winona', 'Courtier', 'wcourtier18@cbc.ca', '0793016817', '1964-10-06'),
('Orin', 'Camies', 'ocamies19@blogtalkradio.com', '0701834175', '1992-02-14'),
('Abbe', 'Semkins', 'asemkins1a@nydailynews.com', '0795506805', '1982-04-27'),
('Dalli', 'Trevena', 'dtrevena1b@blogger.com', '0703899595', '2001-04-01'),
('Marcelle', 'Riveles', 'mriveles1c@wufoo.com', '0746716341', '2000-01-17'),
('Winfred', 'Schule', 'wschule1d@usnews.com', '0733819625', '1976-01-17');

// extras
INSERT INTO extra_service (name, price)
VALUES ('extrabed', 750.99),
('halfboard', 250.50),
('fullboard', 500.99),
('breakfast', 200.0),
('exclusive spa', 2000.0),
('gym access', 750.0),
('private jet', 12000.0);

// criterias
INSERT INTO search_criteria (name)
VALUES 
('pool'),
('restaurant'),
('evening entertainment'),
('childrens club');

// Reset SERIAL
delete from table;

ALTER sequence table_id_seq restart with 1;

--> insert
