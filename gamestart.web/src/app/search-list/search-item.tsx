import { useEffect, useState } from "react";
import { VideoGame } from "../util/types";
import logo from "../../logo.svg";

export default function SearchItem(ctx: { item: VideoGame }) {
  const [videoGame, setVideoGame] = useState<VideoGame>();

  useEffect(() => {
    setVideoGame(ctx.item);
  }, [ctx.item]);

  if (videoGame !== null)
    return (
      <div className="search-item">
        <div className="search-item-left-group">
          <img className="search-item-image" src={logo} />
          <div className="search-item-middle">
            <span className="search-item-title">{ctx.item.title}</span>
            <span className="search-item-platforms">
              {ctx.item.systemRequirements.map((item) => (
                <img className="search-item-platform" src={logo} key={item.id} />
              ))}
            </span>
          </div>
        </div>
        <span className="search-item-price">${ctx.item.price}</span>
      </div>
    );
  else return <></>;
}
