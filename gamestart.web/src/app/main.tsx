import { useEffect, useState } from "react";
import SearchList from "./search-list/search-list";
import SearchSidebar from "./search-sidebar/search-sidebar";
import "./style/app.scss";
import { VideoGame } from "./util/types";
import ApiRouter from "./util/ApiRouter";

export default function Main() {
  const [videoGames, setVideoGames] = useState<VideoGame[]>([]);

  useEffect(() => {
    const getVideoGames = async () => {
      const { data } = await ApiRouter.catalog.get<VideoGame[]>("?page=1", false);
      setVideoGames(data);
    };
    getVideoGames();
  }, []);

  return (
    <div className="main">
      <SearchSidebar setFoundItems={setVideoGames} />
      <SearchList items={videoGames} />
    </div>
  );
}
