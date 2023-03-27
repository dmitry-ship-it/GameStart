import { Order } from "../util/types";
import { NavLink } from "react-router-dom";

export default function AccountOrdersGroup(ctx: { orders: Order[] }) {
  return (
    <div className="account-orders-box">
      <span className="account-orders-box-title">Orders</span>
      {[...ctx.orders]
        .sort((a, b) => +new Date(b.dateTime) - +new Date(a.dateTime))
        .map((order) => (
          <NavLink className="account-orders-box-item" to={`/account/order/${order.id}`} key={order.id}>
            No. {order.id.substring(0, order.id.indexOf("-"))}
          </NavLink>
        ))}
    </div>
  );
}
