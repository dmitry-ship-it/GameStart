import { useEffect, useState } from "react";
import { VideoGame } from "../util/types";
import { NavLink } from "react-router-dom";
import ApiRouter from "../util/ApiRouter";

export default function SearchItem(ctx: { item: VideoGame }) {
  const [videoGame, setVideoGame] = useState<VideoGame>();

  useEffect(() => {
    setVideoGame(ctx.item);
  }, [ctx.item]);

  if (videoGame !== undefined)
    return (
      <NavLink className="search-item" to={`/game/${videoGame.id}`}>
        <div className="search-item-left-group">
          <img className="search-item-image" src={ApiRouter.getMediaSource(`${videoGame.id}.jpg`)} alt="logo" />
          <div className="search-item-middle">
            <span className="search-item-title">{videoGame.title}</span>
            <span className="search-item-platforms">
              {ctx.item.systemRequirements.map((item) => (
                <img
                  className="search-item-platform"
                  src={ApiRouter.getMediaSource(`${item.platform.name}.png`)}
                  key={item.id}
                  alt={item.platform.name}
                />
              ))}
            </span>
          </div>
        </div>
        <span className="search-item-price">${ctx.item.price.toFixed(2)}</span>
      </NavLink>
    );
  else return <></>;
}
