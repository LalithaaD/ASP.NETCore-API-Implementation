****ASP.NETCore API Implementation with SQLServer/EFCore:****

**Server Details:**
**Server:** ASP.NET Core
**Client:** Postman
**Database:** SQLServer/EFCore

**Task Description:**
The task involves creating an ASP.NET Core implementation of the server components to facilitate communication between the JavaScript client described in Assignment 1 and a SQLServer database instance using Entity Framework Core.
The API endpoints are designed to handle CRUD operations for various database entities, including:

**Product: **Contains fields for description, image, pricing, and shipping cost.
**User:** Includes fields for email, password, username, purchase history, and shipping address.
**Comments:** Relates to product reviews with fields for product, user, rating, image(s), and text.
**Cart:** Manages user's shopping cart with fields for products, quantities, and user information.
**Order:** Includes fields such as user_id, product_id, quantity, etc., to record a sale.

**Testing:**
A comprehensive validation of the API endpoints has been conducted using Postman and Swagger to ensure their robustness and reliability under various scenarios.

**Testing Endpoints:**
Utilized Postman to send requests to the implemented API endpoints.
Validated the functionality of CRUD operations for each entity in the database.
