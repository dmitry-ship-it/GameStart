import { useEffect, useState } from "react";
import { store } from "../App";
import { CartItemWrapper } from "../app/util/types";
import { NavLink } from "react-router-dom";

export default function CartButton() {
  const [cartItems] = store.useState<CartItemWrapper[]>("cart");
  const [count, setCount] = useState(0);

  useEffect(() => {
    setCount(cartItems.length);
  }, [cartItems.length]);

  return (
    <NavLink to="/account/cart" className="account-button">
      Cart {count === 0 ? "" : `(${count})`}
    </NavLink>
  );
}
