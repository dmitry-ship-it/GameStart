import { useEffect, useState } from "react";
import "../style/app.scss";
import { VideoGame } from "../util/types";
import SearchItem from "./search-item";
import { store } from "../../App";
import ApiRouter from "../util/ApiRouter";
import SearchNav from "./search-nav";

export default function SearchList(ctx: { items: VideoGame[]; pageSize: number; setPageFunc: React.Dispatch<React.SetStateAction<number>> }) {
  const [videoGames, setVideoGames] = useState<VideoGame[]>([]);
  const [gameCount, setGameCount] = useState(0);

  useEffect(() => {
    setVideoGames(ctx.items);
    store.setState("games", videoGames);
  }, [ctx.items, videoGames]);

  useEffect(() => {
    const loadGameCount = async () => {
      const response = await ApiRouter.catalog.get<number>("count");
      setGameCount(response.data);
    };
    loadGameCount();
  }, []);

  return (
    <>
      <div className="search-list">
        {videoGames.map((item) => (
          <SearchItem item={item} key={item.id} />
        ))}
        <SearchNav gameCount={gameCount} pageSize={ctx.pageSize} setPageFunc={ctx.setPageFunc} />
      </div>
    </>
  );
}
