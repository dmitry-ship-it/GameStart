import { VideoGame } from "../util/types";

export default function GameAddToCartButton(ctx: { game: VideoGame }) {
  const handleAddToCart = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();

    // placeholder
    alert(`ADD TO CART (placeholder) ${ctx.game.id}`);
  };

  return (
    <button className="game-add-to-cart-button" type="button" onClick={handleAddToCart}>
      ADD TO CART
    </button>
  );
}
