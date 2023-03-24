import { store } from "../../App";
import AccountUnauthorized from "../account/account-unauthorized";
import "../style/cart.scss";
import { CartItemWrapper } from "../util/types";
import CartCheckoutButton from "./cart-checkout-button";
import CartItem from "./cart-item";

export default function CartPage() {
  const [cartItems, setCartItems] = store.useState<CartItemWrapper[]>("cart");
  const isLoggedIn = store.getState("isLoggedIn").getValue<boolean>() as boolean;
  const total = cartItems === null || cartItems === undefined ? 0 : cartItems.reduce((sum, current) => sum + current.game.price, 0);

  const removeItemFromCart = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>, item: CartItemWrapper) => {
    e.preventDefault();
    setCartItems(cartItems.filter(({ id }) => id !== item.id));
  };

  const isCartEmpty = cartItems === null || cartItems === undefined || cartItems.length === 0;

  if (!isLoggedIn) return <AccountUnauthorized />;

  return (
    <div className="cart-page">
      {isCartEmpty ? (
        <h3>Nothing to show</h3>
      ) : (
        cartItems.map((item) => (
          <div className="cart-item-group" key={item.id}>
            <CartItem item={item} />
            <button
              className="cart-item-delete"
              onClick={(e) => {
                removeItemFromCart(e, item);
              }}>
              X
            </button>
          </div>
        ))
      )}
      <span className="cart-total">Total: ${total.toFixed(2)}</span>
      {isCartEmpty ? <></> : <CartCheckoutButton />}
    </div>
  );
}
