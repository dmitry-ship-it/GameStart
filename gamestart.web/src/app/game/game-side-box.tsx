import { VideoGame } from "../util/types";
import logo from "../../logo.svg";
import GameSideBoxTable from "./game-side-box-table";
import "../style/game.scss";

export default function GameSideBox(ctx: { game: VideoGame }) {
  return (
    <div className="game-side-box">
      <img className="game-side-box-image" src={logo} alt={ctx.game.title} />
      <span className="game-side-box-price">${ctx.game.price}</span>
      <button type="button">BUY NOW</button>
      <button type="button">ADD TO CART</button>
      <GameSideBoxTable game={ctx.game} />
    </div>
  );
}
