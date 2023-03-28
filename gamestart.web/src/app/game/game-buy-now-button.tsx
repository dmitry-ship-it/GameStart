import uuid from "react-uuid";
import { store } from "../../App";
import { CartItemWrapper, VideoGame } from "../util/types";
import { useNavigate } from "react-router-dom";

export default function GameBuyButton(ctx: { game: VideoGame }) {
  const [, setCartItems] = store.useState("cart");
  const navigate = useNavigate();

  const handleBuyRequest = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();

    const item: CartItemWrapper = {
      id: uuid(),
      game: ctx.game,
      isPhysicalCopy: false,
      count: 1,
    };

    setCartItems([item]);
    navigate("/account/cart/checkout");
  };

  return (
    <button className="game-buy-button" type="button" onClick={handleBuyRequest}>
      BUY NOW
    </button>
  );
}
