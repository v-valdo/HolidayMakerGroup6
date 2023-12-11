CREATE TABLE "customers"(
    "id" SERIAL NOT NULL,
    "first_name" VARCHAR(255) NOT NULL,
    "last_name" VARCHAR(255) NOT NULL,
    "email" VARCHAR(255) NOT NULL,
    "telnumber" INTEGER NOT NULL,
    "date_of_birth" DATE NOT NULL
);
ALTER TABLE
    "customers" ADD PRIMARY KEY("id");
CREATE TABLE "criteria_rooms"(
    "criteria_id" INTEGER NOT NULL,
    "room_id" INTEGER NOT NULL
);
CREATE TABLE "rooms"(
    "id" SERIAL NOT NULL,
    "size" INTEGER NOT NULL,
    "location_name" VARCHAR(255) NOT NULL,
    "price" DECIMAL(8, 2) NOT NULL,
    "reviews" DECIMAL(8, 2) NOT NULL
);
ALTER TABLE
    "rooms" ADD PRIMARY KEY("id");
CREATE TABLE "search_criteria"(
    "id" SERIAL NOT NULL,
    "name" VARCHAR(255) NOT NULL
);
ALTER TABLE
    "search_criteria" ADD PRIMARY KEY("id");
CREATE TABLE "extra_service"(
    "name" VARCHAR(255) NOT NULL,
    "price" DECIMAL(8, 2) NOT NULL
);
ALTER TABLE
    "extra_service" ADD PRIMARY KEY("name");
CREATE TABLE "extra_service_and_bookings"(
    "booking_id" INTEGER NOT NULL,
    "extra_service_name" VARCHAR(255) NOT NULL
);
CREATE TABLE "bookings"(
    "id" SERIAL NOT NULL,
    "customer_id" INTEGER NOT NULL,
    "start_date" DATE NOT NULL,
    "end_date" DATE NOT NULL,
    "room_id" INTEGER NOT NULL,
    "number_of_people" INTEGER NOT NULL,
    "price" INTEGER NOT NULL
);
ALTER TABLE
    "bookings" ADD PRIMARY KEY("id");
CREATE TABLE "location"(
    "name" VARCHAR(255) NOT NULL,
    "distance_to_beach" INTEGER NOT NULL,
    "distance_to_city" INTEGER NOT NULL
);
ALTER TABLE
    "location" ADD PRIMARY KEY("name");
ALTER TABLE
    "bookings" ADD CONSTRAINT "bookings_room_id_foreign" FOREIGN KEY("room_id") REFERENCES "rooms"("id");
ALTER TABLE
    "criteria_rooms" ADD CONSTRAINT "criteria_rooms_room_id_foreign" FOREIGN KEY("room_id") REFERENCES "rooms"("id");
ALTER TABLE
    "extra_service_and_bookings" ADD CONSTRAINT "extra_service_and_bookings_booking_id_foreign" FOREIGN KEY("booking_id") REFERENCES "bookings"("id");
ALTER TABLE
    "bookings" ADD CONSTRAINT "bookings_customer_id_foreign" FOREIGN KEY("customer_id") REFERENCES "customers"("id");
ALTER TABLE
    "criteria_rooms" ADD CONSTRAINT "criteria_rooms_criteria_id_foreign" FOREIGN KEY("criteria_id") REFERENCES "search_criteria"("id");
ALTER TABLE
    "rooms" ADD CONSTRAINT "rooms_location_name_foreign" FOREIGN KEY("location_name") REFERENCES "location"("name");
ALTER TABLE
    "extra_service_and_bookings" ADD CONSTRAINT "extra_service_and_bookings_extra_service_name_foreign" FOREIGN KEY("extra_service_name") REFERENCES "extra_service"("name");

ALTER SEQUENCE 
    customers_id_seq RESTART WITH 1;
ALTER SEQUENCE 
    rooms_id_seq RESTART WITH 1;
ALTER SEQUENCE 
    search_criteria_id_seq RESTART WITH 1;
ALTER SEQUENCE 
    bookings_id_seq RESTART WITH 1;