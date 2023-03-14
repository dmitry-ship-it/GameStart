import { useEffect, useState } from "react";
import "../style/app.scss";
import { VideoGame } from "../util/types";
import SearchItem from "./search-item";

export default function SearchList(ctx: { items: VideoGame[] }) {
  const [videoGames, setVideoGames] = useState<VideoGame[]>([]);

  useEffect(() => {
    setVideoGames(ctx.items);
  }, [ctx.items]);

  return (
    <div className="search-list">
      {videoGames.map((item) => (
        <SearchItem item={item} key={item.id} />
      ))}
    </div>
  );
}
