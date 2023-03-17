import { VideoGame } from "../util/types";

export default function GameBuyButton(ctx: { game: VideoGame }) {
  const handleBuyRequest = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();

    // placeholder
    alert(`BUY NOW (placeholder) ${ctx.game.id}`);
  };

  return (
    <button className="game-buy-button" type="button" onClick={handleBuyRequest}>
      BUY NOW
    </button>
  );
}
