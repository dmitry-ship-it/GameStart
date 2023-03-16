import { VideoGame } from "../util/types";
import logo from "../../logo.svg";
import "../style/game.scss";

export default function GameSideBoxTable(ctx: { game: VideoGame }) {
  return (
    <table className="game-side-box-table">
      <tr className="game-side-box-table-row">
        <td className="game-side-box-table-label">Developers</td>
        <td className="game-side-box-table-value">{ctx.game.developers.map(({ name }) => name).join(", ")}</td>
      </tr>
      <tr className="game-side-box-table-row">
        <td className="game-side-box-table-label">Publisher</td>
        <td className="game-side-box-table-value">{ctx.game.publisher.name}</td>
      </tr>
      <tr className="game-side-box-table-row">
        <td className="game-side-box-table-label">Genres</td>
        <td className="game-side-box-table-value">{ctx.game.genres.map(({ name }) => name).join(", ")}</td>
      </tr>
      <tr className="game-side-box-table-row">
        <td className="game-side-box-table-label">Release Date</td>
        <td className="game-side-box-table-value">{new Date(ctx.game.releaseDate).toLocaleDateString()}</td>
      </tr>
      <tr className="game-side-box-table-row">
        <td className="game-side-box-table-label">Platforms</td>
        <td className="game-side-box-table-value">
          {ctx.game.systemRequirements.map(({ platform }) => (
            <img className="game-side-box-table-platform" src={logo} alt={platform.name} key={platform.id} />
          ))}
        </td>
      </tr>
    </table>
  );
}
