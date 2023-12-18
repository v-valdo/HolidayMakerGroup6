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

### Uppdatera pris - bookings

Priset för rummet multiplicerat med antalet dagar rummet är bokat. 
Extract funktionen används för att omvandla intervall datatypen (från AGE() funktionen) till numeric.
```
UPDATE bookings b
SET price = (select r.price * (EXTRACT(days from AGE(b.end_date, b.start_date)) + 1)
			from rooms r
			WHERE b.room_id = r.id )
FROM rooms r
WHERE b.room_id = r.id 
```

### Uppdatera pris - extra_service_and_bookings

Priset för en extra service multiplicerat antalet dagar rummet är bokat.
```
UPDATE extra_service_and_bookings eb
SET price = (select e.price * EXTRACT((days from AGE(b.end_date, b.start_date)) +1)
					from rooms r, extra_service e, bookings b
					WHERE b.room_id = r.id AND eb.booking_id = b.id AND e.id = eb.extra_service_id)
FROM rooms r, extra_service e, bookings b
WHERE b.room_id = r.id AND eb.booking_id = b.id AND e.id = eb.extra_service_id;
```
