import { VideoGame } from "../util/types";
import GameSpecificationsBlock from "./game-specifications-block";

export default function GameMainBox(ctx: { game: VideoGame }) {
  return (
    <div className="game-main-box">
      <iframe className="game-video" src="https://youtube.com/embed/dQw4w9WgXcQ?autoplay=1&mute=1" title={ctx.game.title}></iframe>
      <span className="game-description">{ctx.game.description}</span>
      <GameSpecificationsBlock game={ctx.game} />
    </div>
  );
}
