import { VideoGame } from "../util/types";
import logo from "../../logo.svg";
import GameSideBoxTable from "./game-side-box-table";
import "../style/game.scss";
import GameBuyButton from "./game-buy-now-button";
import GameAddToCartButton from "./game-add-to-cart-button";

export default function GameSideBox(ctx: { game: VideoGame }) {
  return (
    <div className="game-side-box">
      <img className="game-side-box-image" src={logo} alt={ctx.game.title} />
      <span className="game-side-box-price">${ctx.game.price.toFixed(2)}</span>
      <GameBuyButton game={ctx.game} />
      <GameAddToCartButton game={ctx.game} />
      <GameSideBoxTable game={ctx.game} />
    </div>
  );
}
