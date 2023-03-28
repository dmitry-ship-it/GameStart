import { useEffect, useState } from "react";
import SearchList from "./search-list/search-list";
import SearchSidebar from "./search-sidebar/search-sidebar";
import "./style/app.scss";
import { VideoGame } from "./util/types";
import ApiRouter from "./util/ApiRouter";
import SearchNav from "./search-list/search-nav";

export default function Main() {
  const [videoGames, setVideoGames] = useState<VideoGame[]>([]);
  const [page, setPage] = useState(1);
  const pageSize = 10;

  useEffect(() => {
    const getVideoGames = async () => {
      const { data } = await ApiRouter.catalog.get<VideoGame[]>(`?page=${page}&pageSize=${pageSize}`, false);
      setVideoGames(data);
    };
    getVideoGames();
  }, [page]);

  return (
    <div className="main">
      <SearchSidebar setFoundItems={setVideoGames} />
      <SearchList items={videoGames} pageSize={pageSize} setPageFunc={setPage} />
    </div>
  );
}
