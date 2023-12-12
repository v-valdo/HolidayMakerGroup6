# SQL Queries

## Skapa bokning

Skapa en bokning för kund med ID 1, en vecka i juni, i rum med ID 1, 3 personer, för $500

Priset räknar vi ut i appen(!)

```
insert into bookings (customer_id, start_date, end_date, room_id, number_of_people, price)
values (1, '2020-06-20', '2020-06-27', 1, 3, 500);
```

### Lägg till extratjänst/er

Lägger till "private jet" som extratjänst i booking med ID 1

```
insert into extra_service_and_bookings(booking_id, extra_service_name)
values (1, 'private jet');
```

### Söka på boenden baserat på till centrum

Eftersom union inte kan användas, och vill inte ha duplicationer definerades alla kolumner för hand.

```
SELECT r.id, r.size, r.price, r.reviews, r.location_name, l.distance_to_city, l.distance_to_beach
FROM rooms r
RIGHT JOIN location l
ON r.location_name = l.name
ORDER BY distance_to_city ASC;
```

### Ordna sökträffar - omdömme (högt till lågt)

Grundprincipen på hur man sorterar högt till lågt

```
SELECT * 
FROM rooms
ORDER BY reviews DESC;
```

### Ordna sökträffar - recensioner (lågt till högt)

Grundprincipen på hur man sorterar lågt till högt
```
SELECT *
FROM rooms
ORDER BY price ASC;
```

### Ordna sökträffar - boendes avstånd till strand (lågt till högt)
```
select rooms.id room_id, location.distance_to_beach, rooms.size room_size
from rooms
join location on rooms.location_name = location.name
order by location.distance_to_beach ASC;
```

### Ordna sökträffar - boendes avstånd till stad (lågt till högt)
```
select rooms.id room_id, location.distance_to_city, rooms.size room_size
from rooms
join location on rooms.location_name = location.name
order by location.distance_to_city ASC;
```
