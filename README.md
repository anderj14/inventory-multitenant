# **Multi-Tenant API for Inventory Management**

This API is a demo designed to showcase multi-tenant inventory management operations. It includes functionalities for tenant management, user authentication, and basic inventory operations such as creating, updating, and deleting products, brands, categories, and other related entities. The API is built using ASP.NET Core and Entity Framework Core, demonstrating a scalable and flexible architecture suitable for multi-tenant scenarios.

---

## **Features**

- **Multi-Tenant Support**: Each tenant can have its own isolated database or share the same database with data isolation at the application level.
- **Tenant Management**: Create, retrieve, and delete tenants with full database isolation options.
- **User Authentication**: Role-based access control (RBAC) using ASP.NET Identity with JWT-based authentication.
- **Inventory Management**: Create and manage products, categories, suppliers, and other inventory-related entities.

---

## **Technologies Used**

- **ASP.NET Core**: Framework for building RESTful APIs.
- **Entity Framework Core**: ORM for database interactions and migrations.
- **Docker Container - SQL Server**: A SQL Server instance running in a Docker container, used for storing tenant and inventory data.
- **ASP.NET Identity**: User authentication and role management.
- **JWT (JSON Web Tokens)**: Authentication mechanism for secure API access.

---

## **Getting Started**

### **Prerequisites**

Before setting up the project, make sure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Docker](https://www.docker.com/get-started) (Optional for containerization)

### **Clone the Repository**

Clone the repository to your local machine:

```bash
git clone https://github.com/anderj14/inventory-multitenant.git
cd inventory-multitenant
cd API
```

## **Configuration**

### **Set up Database**

- Configure your database connection strings in the appsettings.json or through environment variables.

### **Migrations**

- Run database migrations to set up the schema for your API. If you're using Entity Framework, you can use the following command:

```bash
dotnet ef migrations add InitialTenant --context TenantDbContext -o Migrations/TenantMigrationDb
dotnet ef migrations add InitialEntity --context ApplicationDbContext --output-dir Migrations/AppMigrationDb
dotnet ef database update --context ApplicationDbContext
```

### **JWT Secret**

- Set the JWT secret for token generation. You can configure this in appsettings.json or as an environment variable.

```json
"Token": {
"Key": "fb7c9f2b3e5d7894a1f3d2c4b9a7e6f8c9d2b4e7a6c8e1f3b7c9d4e6f1a2b3c7d9e4f6a8c2b4f1e3c5a7b9d1e2f3c4b7d8f9e0a1b2c3d4f5",
"Issuer": "https://localhost:5001",
"Audience": "https://localhost:5001"
},
```

## **Running the API**

- To run the API locally, use the following command:

```bash
dotnet run
```

```bash
dotnet watch
```

## **Endpoints**

### **Tenant**

- Create Tenant: POST /api/v1/tenants
- Get All Tenants: GET /api/v1/tenants
- Get Tenant by ID: GET /api/v1/tenants/{tenantId}
- Delete Tenant: DELETE /api/v1/tenants/{tenantId}

### **User**

- Email User Exists: POST /api/v1/users/emailexists
- Register User: POST /api/v1/users/register
- Login User: POST /api/v1/users/login
- Get User Details: GET /api/v1/users/{userId}

### **Brands**

- Create Brand: POST /api/v1/brands
- Get All Brands: GET /api/v1/brands
- Get Brand by ID: GET /api/v1/brands/{brandId}
- Update Brand: PUT /api/v1/brands/{brandId}
- Delete Brand: DELETE /api/v1/brands/{brandId}

### **Categories**

- Create Category: POST /api/v1/categories
- Get All Categories: GET /api/v1/categories
- Get Category by ID: GET /api/v1/categories/{categoryId}
- Update Category: PUT /api/v1/categories/{categoryId}
- Delete Category: DELETE /api/v1/categories/{categoryId}

### **Suppliers**

- Create Supplier: POST /api/v1/suppliers
- Get All Suppliers: GET /api/v1/suppliers
- Get Supplier by ID: GET /api/v1/suppliers/{supplierId}
- Update Supplier: PUT /api/v1/suppliers/{supplierId}
- Delete Supplier: DELETE /api/v1/suppliers/{supplierId}

### **Products**

- Create Product: POST /api/v1/products
- Get All Products: GET /api/v1/products
- Get Product by ID: GET /api/v1/products/{productId}
- Update Product: PUT /api/v1/products/{productId}
- Delete Product: DELETE /api/v1/products/{productId}

### **Warehouse**

- Create Warehouse: POST /api/v1/warehouse
- Get All Warehouses: GET /api/v1/warehouse
- Get Warehouse by ID: GET /api/v1/warehouse/{warehouseId}
- Update Warehouse: PUT /api/v1/warehouse/{warehouseId}
- Delete Warehouse: DELETE /api/v1/warehouse/{warehouseId}

### **Inventory Movements**

- Create Inventory Movement: POST /api/v1/inventorymovements
- Get All Inventory Movements: GET /api/v1/inventorymovements
- Get Inventory Movement by ID: GET /api/v1/inventorymovements/{inventorymovementId}
- Update Inventory Movement: PUT /api/v1/inventorymovements/{inventorymovementId}
- Delete Inventory Movement: DELETE /api/v1/inventorymovements/{inventorymovementId}

## **Authentication and Authorization**

- The API uses JWT for authentication.
- To obtain a JWT token, authenticate a user by sending a POST request to /api/users/authenticate with the user's credentials (email and password).
- The token should be included in the Authorization header of subsequent requests.

```bash
Authorization: Bearer {token}
```

## **Tenant**

-The API uses the tenant ID for all controllers except the tenant-created endpoint.
-Put the tenant ID in the header to send the request to any endpoint, for example to get all users, get all products, categories, etc.
To test the api use postman

```bash
Header: tenant {tenantId(company_1)}
```
