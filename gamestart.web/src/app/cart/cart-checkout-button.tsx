import { NavLink } from "react-router-dom";

export default function CartCheckoutButton() {
  return (
    <NavLink className="cart-checkout-button" to="/account/cart/checkout">
      Checkout
    </NavLink>
  );
}
