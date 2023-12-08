CREATE TABLE "customers"(
    "id" SERIAL NOT NULL,
    "name" VARCHAR(255) NOT NULL,
    "email" VARCHAR(255) NOT NULL,
    "telnumber" INTEGER NOT NULL,
    "date_of_birth" DATE NOT NULL
);
ALTER TABLE
    "customers" ADD PRIMARY KEY("id");
CREATE TABLE "rooms"(
    "id" SERIAL NOT NULL,
    "size" INTEGER NOT NULL,
    "location_name" VARCHAR(255) NOT NULL,
    "price" DECIMAL(8, 2) NOT NULL
);
ALTER TABLE
    "rooms" ADD PRIMARY KEY("id");
CREATE TABLE "search_criteria"(
    "night_entertainment" VARCHAR(255) NOT NULL,
    "pool" VARCHAR(255) NOT NULL,
    "kidsclub" VARCHAR(255) NOT NULL,
    "restaurant" VARCHAR(255) NOT NULL,
    "location" VARCHAR(255) NOT NULL
);
CREATE TABLE "extra_service"(
    "id" SERIAL NOT NULL,
    "all_inclusive" VARCHAR(255) NOT NULL,
    "half_inclusive" VARCHAR(255) NOT NULL,
    "extra_room" VARCHAR(255) NOT NULL
);
ALTER TABLE
    "extra_service" ADD PRIMARY KEY("id");
CREATE TABLE "extra_service_and_bookings"(
    "booking_id" INTEGER NOT NULL,
    "extra_service_id" INTEGER NOT NULL
);
CREATE TABLE "bookings"(
    "id" SERIAL NOT NULL,
    "customer_id" INTEGER NOT NULL,
    "date" DATE NOT NULL,
    "room_id" INTEGER NOT NULL,
    "number_of_people" INTEGER NOT NULL,
    "price" INTEGER NOT NULL,
    "extras" INTEGER NOT NULL
);
ALTER TABLE
    "bookings" ADD PRIMARY KEY("id");
CREATE TABLE "location"(
    "name" VARCHAR(255) NOT NULL,
    "distance_to_beach" INTEGER NOT NULL,
    "distance_to_city" INTEGER NOT NULL,
    "price" INTEGER NOT NULL,
    "reviews" DOUBLE PRECISION NOT NULL
);
ALTER TABLE
    "location" ADD PRIMARY KEY("name");
ALTER TABLE
    "bookings" ADD CONSTRAINT "bookings_room_id_foreign" FOREIGN KEY("room_id") REFERENCES "rooms"("id");
ALTER TABLE
    "extra_service_and_bookings" ADD CONSTRAINT "extra_service_and_bookings_extra_service_id_foreign" FOREIGN KEY("extra_service_id") REFERENCES "extra_service"("id");
ALTER TABLE
    "extra_service_and_bookings" ADD CONSTRAINT "extra_service_and_bookings_booking_id_foreign" FOREIGN KEY("booking_id") REFERENCES "bookings"("id");
ALTER TABLE
    "bookings" ADD CONSTRAINT "bookings_customer_id_foreign" FOREIGN KEY("customer_id") REFERENCES "customers"("id");
ALTER TABLE
    "search_criteria" ADD CONSTRAINT "search_criteria_location_foreign" FOREIGN KEY("location") REFERENCES "location"("name");
ALTER TABLE
    "rooms" ADD CONSTRAINT "rooms_location_name_foreign" FOREIGN KEY("location_name") REFERENCES "location"("name");