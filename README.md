# ChubbCarRental
ChubbCarRentalScreenshots.pdf consists of Postman screenshots

## Features

- **Authentication & Authorization**:
  - JWT-based authentication.
  - Role-based access for admin and users.

- **Car Management**:
  - Add, update, and delete cars (Admin only).
  - View available cars (all users).

- **Car Rental**:
  - Rent cars (Authenticated users only).
  - Automatically marks cars as unavailable when rented.

- **Email Notifications**:
  - Sends booking confirmation emails using **Mailgun**.

- **Database Integration**:
  - Uses **SQL Server** via **Entity Framework Core**.

---



## API Endpoints

### User Endpoints
1. **Register User**: `POST /api/users/register`
   ```json
   {
     "name": "John Doe",
     "email": "john@example.com",
     "password": "Password123",
     "role": "User"
   }
   ```

2. **Login**: `POST /api/users/login`
   ```json
   {
     "email": "john@example.com",
     "password": "Password123"
   }
   ```

---

### Car Endpoints
1. **Get All Cars**: `GET /api/cars`
2. **Add Car (Admin Only)**: `POST /api/cars`
3. **Update Car (Admin Only)**: `PUT /api/cars/{id}`
4. **Delete Car (Admin Only)**: `DELETE /api/cars/{id}`
5. **Rent Car (Authenticated Users Only)**: `POST /api/cars/rent?id={carId}`

---

## Mock JSON Objects

### UserModel
```json
{
  "id": 1,
  "name": "Nishant",
  "email": "nishant@gmail.com",
  "password": "hello@123",
  "role": "User"
}
```

### CarModel
```json
{
  "id": 1,
  "make": "BMW",
  "model": "M3",
  "year": 2022,
  "pricePerDay": 5000,
  "isAvailable": true
}
```

---
