import { useParams } from "react-router-dom";
import { VideoGame } from "../util/types";
import { store } from "../../App";
import ApiRouter from "../util/ApiRouter";
import { useEffect, useState } from "react";
import GameSideBox from "./game-side-box";
import "../style/game.scss";

export default function GamePage() {
  const { gameId } = useParams();
  const [game, setGame] = useState((store.getState("games").getValue() as VideoGame[]).find((g) => g.id === gameId));

  useEffect(() => {
    const loadGame = async () => {
      const { data } = await ApiRouter.catalog.get<VideoGame>(gameId!);
      setGame(data);
    };

    if (game === undefined) {
      loadGame();
    }
  }, [game, gameId]);

  return game === undefined ? (
    <>Not Found</>
  ) : (
    <div className="game-page">
      <div className="game-title">{game.title}</div>
      <div className="game-video-and-info">
        <iframe className="game-video" src="https://youtube.com/embed/dQw4w9WgXcQ?autoplay=1&mute=1" title={game.title}></iframe>
        <GameSideBox game={game} />
      </div>
    </div>
  );
}
