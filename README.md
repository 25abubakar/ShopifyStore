# ShopifyStore

Professional ASP.NET Core MVC ecommerce store with:
- Guest storefront browsing and checkout
- Staff-only operations for `CEO`, `Admin`, `Employee`
- Department/category/subcategory catalog structure
- Advanced filters and sorting
- Multi-method payment workflow

## Tech Stack
- ASP.NET Core MVC
- Entity Framework Core + SQL Server
- Bootstrap + Razor views

## Run Locally
1. Update connection string in `ShopifyStore/appsettings.json` if needed.
2. Run the app.
3. App startup auto-applies compatibility column updates via `DbSchemaUpdater`.
4. Seed users are auto-created if not present:
   - `ceo / ceo123`
   - `admin / admin123`
   - `employee / emp123`

## Authentication and Roles
- Guests can open store pages and place orders.
- Staff login is available from navbar (`Staff Login`).
- Dashboard/management links are shown only for authenticated staff users.
- Logout is a secure `POST` action with antiforgery token.

## Catalog Structure
Products use three-level taxonomy:
- `Department`: Men, Women, Kids, Accessories
- `Category`: e.g., Clothing, Footwear, General
- `Subcategory`: e.g., Shirts, Pants, Casual Shoes, Bags

Taxonomy rules are enforced in admin product create/edit.

## Storefront Filters
Available filters:
- Department
- Category
- Subcategory
- Search by name/description
- Min/Max price
- In-stock only
- Sort (name, newest, price asc, price desc)

## Payment Methods
- Cash on Delivery
- Bank Transfer (manual verification)
- Easypaisa/JazzCash (manual verification)
- Online Gateway placeholder (gateway pending flow)

Manual verification methods require:
- Payment reference
- Payment proof URL

## Documentation
- `docs/STORE_FUNCTIONAL_SPEC.md`
- `docs/PAYMENT_WORKFLOW.md`
- `docs/ADMIN_GUIDE.md`
- `docs/DB_SCHEMA.md`

