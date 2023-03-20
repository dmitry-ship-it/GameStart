import uuid from "react-uuid";
import { store } from "../../App";
import { CartItemWrapper, VideoGame } from "../util/types";
import "../style/cart.scss";

export default function GameAddToCartPopup(ctx: { game: VideoGame }) {
  const [cartItems, setCartItems] = store.useState<CartItemWrapper[]>("cart");

  const handleAddToCart = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();

    const form = document.querySelector<HTMLFormElement>("#add-to-cart")!;
    const formData = new FormData(form);
    const isPhysicalCopy = (formData.get("isPhysicalCopy") as string) === "1";

    const item: CartItemWrapper = {
      id: uuid(),
      game: ctx.game,
      isPhysicalCopy: isPhysicalCopy,
      count: 1,
    };

    setCartItems(cartItems.concat(item));

    if (!isPhysicalCopy) {
      const physicalRadio = document.querySelector<HTMLInputElement>("#physical-copy-radio")!;
      physicalRadio.checked = true;
    }
  };

  return (
    <form className="add-to-cart-form" id="add-to-cart">
      <label className="add-to-cart-label">
        <span className="add-to-cart-title">Digital copy</span>
        <input
          className="add-to-cart-value"
          type="radio"
          name="isPhysicalCopy"
          value="0"
          disabled={cartItems.some((item) => item.game.id === ctx.game.id && !item.isPhysicalCopy)}
        />
      </label>
      <label className="add-to-cart-label">
        <span className="add-to-cart-title">Physical copy</span>
        <input className="add-to-cart-value" id="physical-copy-radio" type="radio" name="isPhysicalCopy" value="1" />
      </label>
      <button className="add-to-cart-button" onClick={handleAddToCart}>
        Add
      </button>
    </form>
  );
}
