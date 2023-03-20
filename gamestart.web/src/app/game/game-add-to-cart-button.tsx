import uuid from "react-uuid";
import { store } from "../../App";
import { CartItemWrapper, VideoGame } from "../util/types";
import Popup from "reactjs-popup";
import { useState } from "react";
import GameAddToCartPopup from "./game-add-to-cart-popup";

export default function GameAddToCartButton(ctx: { game: VideoGame }) {
  const [isPopupOpen, setIsPopupOpen] = useState(false);

  return (
    <Popup
      arrow={false}
      open={isPopupOpen}
      onOpen={() => setIsPopupOpen(true)}
      onClose={() => setIsPopupOpen(false)}
      trigger={
        <button className="game-add-to-cart-button" type="button">
          ADD TO CART
        </button>
      }>
      <GameAddToCartPopup game={ctx.game} />
    </Popup>
  );
}
