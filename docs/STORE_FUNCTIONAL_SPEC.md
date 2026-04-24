# Store Functional Specification

## User Types
- Guest customer
- Employee
- Admin
- CEO

## Guest Experience
- Browse all products
- Filter products by department/category/subcategory/price/search
- View product details
- Place order with supported payment methods

## Staff Experience
- Login via account page
- Role-based dashboard
- Orders view:
  - Employee: view only
  - Admin/CEO: update order and payment status
- Product management:
  - Admin/CEO: create/edit products
  - CEO: delete products

## Core Flows
1. Guest opens store and applies filters
2. Guest opens product detail
3. Guest places order and selects payment method
4. Order appears in staff orders panel
5. Admin/CEO updates status as payment/order progresses

## Validation Rules
- Product taxonomy must be valid against configured tree
- Manual payment methods require reference and screenshot URL
- Out-of-stock quantity blocks checkout
- Invalid order/payment status combinations are rejected

