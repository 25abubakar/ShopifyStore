# Payment Workflow

## Supported Payment Methods
1. `CashOnDelivery`
2. `BankTransfer`
3. `EasypaisaOrJazzCash`
4. `OnlineGateway` (placeholder integration)

## Checkout Behavior
- COD:
  - Initial `PaymentStatus = Unpaid`
  - Initial `OrderStatus = Pending`
- Bank Transfer / Easypaisa/JazzCash:
  - Require `PaymentReference` + `PaymentScreenshotUrl`
  - Initial `PaymentStatus = VerificationPending`
  - Initial `OrderStatus = PaymentUnderReview`
- Online Gateway:
  - Initial `PaymentStatus = GatewayPending`
  - Initial `OrderStatus = Pending`

## Staff Verification
- Admin/CEO can update payment and order status from Orders page.
- Invalid status combinations are blocked (server-side validation).

## Recommended Next Step for Live Gateway
- Replace placeholder with provider SDK/webhooks
- Mark payment confirmed from webhook callback only
- Add signed callback validation and replay protection

