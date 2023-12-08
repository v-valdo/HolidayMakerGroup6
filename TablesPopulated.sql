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

//location populated
INSERT INTO location(name, distance_to_beach, distance_to_city) 
VALUES  ('tangalooma', 50, 250),
('greenside', 100, 200),
('rivergate', 150, 150),
('azurebliss', 200, 100), 
('crystalcove', 250, 50);