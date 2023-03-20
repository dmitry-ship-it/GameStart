import { NavLink } from "react-router-dom";
import { CartItemWrapper, VideoGame } from "../util/types";
import logo from "../../logo.svg";

export default function CartItem(ctx: { item: CartItemWrapper }) {
  return (
    <NavLink className="cart-item" to={`/game/${ctx.item.game.id}`}>
      <div className="cart-item-left-group">
        <img className="cart-item-image" src={logo} alt="logo" />
        <div className="cart-item-middle">
          <span className="cart-item-title">{ctx.item.game.title + (ctx.item.isPhysicalCopy ? " (Physical copy)" : " (Digital copy)")}</span>
          <span className="cart-item-platforms">
            {ctx.item.game.systemRequirements.map((item) => (
              <img className="cart-item-platform" src={logo} key={item.id} alt={item.platform.name} />
            ))}
          </span>
        </div>
      </div>
      <span className="cart-item-price">${ctx.item.game.price.toFixed(2)}</span>
    </NavLink>
  );
}
