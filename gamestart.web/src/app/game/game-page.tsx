import { useParams } from "react-router-dom";
import { VideoGame } from "../util/types";
import { store } from "../../App";
import ApiRouter from "../util/ApiRouter";
import { useEffect, useState } from "react";
import GameSideBox from "./game-side-box";
import "../style/game.scss";
import GameMainBox from "./game-main-box";
import GameNotFound from "./game-not-found";

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

  if (game === undefined) return <GameNotFound />;

  return (
    <div className="game-page">
      <div className="game-title">{game.title}</div>
      <div className="game-video-and-info">
        <GameMainBox game={game} />
        <GameSideBox game={game} />
      </div>
    </div>
  );
}
