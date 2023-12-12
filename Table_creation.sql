CREATE TABLE customers(
    id SERIAL NOT NULL PRIMARY KEY,
    first_name TEXT NOT NULL,
    last_name TEXT NOT NULL,
    email VARCHAR(255) NOT NULL,
    telnumber INTEGER NOT NULL,
    date_of_birth DATE NOT NULL
);
CREATE TABLE criteria_rooms(
    id SERIAL NOT NULL PRIMARY KEY,
    criteria_id SERIAL NOT NULL,
    room_id SERIAL NOT NULL
);
CREATE TABLE rooms(
    id SERIAL NOT NULL PRIMARY KEY,
    size INTEGER NOT NULL,
    location_id SERIAL NOT NULL,
    price DECIMAL(8, 2) NOT NULL,
    reviews DECIMAL(2, 1) NULL
);
CREATE TABLE search_criteria(
    id SERIAL NOT NULL PRIMARY KEY,
    name TEXT NOT NULL
);
CREATE TABLE extra_service(
    id SERIAL NOT NULL PRIMARY KEY,
    name TEXT NOT NULL,
    price DECIMAL(8, 2) NOT NULL
);
CREATE TABLE extra_service_and_bookings(
    id SERIAL NOT NULL PRIMARY KEY,
    booking_id SERIAL NOT NULL,
    extra_service_id SERIAL NOT NULL
);
CREATE TABLE bookings(
    id SERIAL NOT NULL PRIMARY KEY,
    customer_id SERIAL NOT NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    room_id SERIAL NOT NULL,
    number_of_people INTEGER NOT NULL,
    price INTEGER NOT NULL
);
CREATE TABLE location(
    id SERIAL NOT NULL PRIMARY KEY,
    name TEXT NOT NULL,
    distance_to_beach INTEGER NOT NULL,
    distance_to_city INTEGER NOT NULL
);
ALTER TABLE
    bookings ADD CONSTRAINT bookings_room_id_foreign FOREIGN KEY(room_id) REFERENCES rooms(id);
ALTER TABLE
    criteria_rooms ADD CONSTRAINT criteria_rooms_room_id_foreign FOREIGN KEY(room_id) REFERENCES rooms(id);
ALTER TABLE
    bookings ADD CONSTRAINT bookings_id_foreign FOREIGN KEY(id) REFERENCES extra_service_and_bookings(id);
ALTER TABLE
    rooms ADD CONSTRAINT rooms_location_id_foreign FOREIGN KEY(location_id) REFERENCES location(id);
ALTER TABLE
    bookings ADD CONSTRAINT bookings_customer_id_foreign FOREIGN KEY(customer_id) REFERENCES customers(id);
ALTER TABLE
    extra_service_and_bookings ADD CONSTRAINT extra_service_and_bookings_extra_service_id_foreign FOREIGN KEY(extra_service_id) REFERENCES extra_service(id);
ALTER TABLE
    criteria_rooms ADD CONSTRAINT criteria_rooms_criteria_id_foreign FOREIGN KEY(criteria_id) REFERENCES search_criteria(id);

ALTER SEQUENCE 
    customers_id_seq RESTART WITH 1;
ALTER SEQUENCE 
    criteria_rooms_id_seq RESTART WITH 1;
ALTER SEQUENCE 
    search_criteria_id_seq RESTART WITH 1;
ALTER SEQUENCE 
    rooms_id_seq RESTART WITH 1;
ALTER SEQUENCE 
    extra_service_id_seq RESTART WITH 1;
ALTER SEQUENCE 
    extra_service_and_bookings_id_seq RESTART WITH 1;
ALTER SEQUENCE 
    bookings_id_seq RESTART WITH 1;