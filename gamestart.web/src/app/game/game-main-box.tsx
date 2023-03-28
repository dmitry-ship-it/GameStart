import ApiRouter from "../util/ApiRouter";
import { VideoGame } from "../util/types";
import GameSpecificationsBlock from "./game-specifications-block";

export default function GameMainBox(ctx: { game: VideoGame }) {
  return (
    <div className="game-main-box">
      <video className="game-video" autoPlay controls muted src={ApiRouter.getMediaSource(`${ctx.game.id}.mp4`)} title={ctx.game.title}></video>
      <span className="game-description">{ctx.game.description}</span>
      <GameSpecificationsBlock game={ctx.game} />
    </div>
  );
}
