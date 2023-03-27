import { useEffect, useState } from "react";
import { Address, AddressRequest, CartItemWrapper, OrderItem, OrderRequest } from "../util/types";
import ApiRouter from "../util/ApiRouter";
import "../style/cart.scss";
import CartCheckoutPageAddress from "./cart-checkout-page-address";
import CartCheckoutCreditCard from "./cart-checkout-credit-card";
import { store } from "../../App";
import { NavLink, useNavigate } from "react-router-dom";

export default function CartCheckoutPage() {
  const navigate = useNavigate();
  const [addresses, setAddresses] = useState<Address[]>([]);
  const [cartItems, setCartItems] = store.useState<CartItemWrapper[]>("cart");

  useEffect(() => {
    const loadAddresses = async () => {
      const response = await ApiRouter.address.get<Address[]>("", false);
      if (response.status === 200) {
        setAddresses(response.data);
      }
    };
    loadAddresses();
  }, []);

  const handleOrder = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();

    const form = document.querySelector<HTMLFormElement>(".cart-address-card-group")!;
    const formData = new FormData(form);

    const body: OrderRequest = {
      address: (addresses.find(({ id }) => id === (formData.get("address") as string | null)) as AddressRequest | undefined) ?? null,
      items: cartItems.map((item) => {
        return {
          gameId: item.game.id,
          isPhysicalCopy: item.isPhysicalCopy,
        } as OrderItem;
      }),
    };

    const response = await ApiRouter.order.post("", body, false);

    if (response.status === 202) {
      setCartItems([]);
      navigate(`/account/order/${response.data as any}`);
    }
  };

  const isValidOrder = addresses.length !== 0 || cartItems.every((item) => !item.isPhysicalCopy);

  return (
    <div className="cart-checkout-page">
      <h2>Enter your credentials</h2>
      <form className="cart-address-card-group" key={addresses.length}>
        {!isValidOrder ? (
          <h3>
            You've got no shipping addresses. Add one in <NavLink to="/account">account</NavLink> page
          </h3>
        ) : (
          <></>
        )}
        {addresses.map((address) => (
          <label className="cart-address-card" key={address.id}>
            <input className="cart-address-radio" type="radio" value={address.id} name="address" />
            <CartCheckoutPageAddress address={address} />
          </label>
        ))}
      </form>
      <CartCheckoutCreditCard />
      <button className="cart-checkout-place-order" onClick={handleOrder} disabled={!isValidOrder}>
        PLACE ORDER
      </button>
    </div>
  );
}
