import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Order } from "../util/types";
import ApiRouter from "../util/ApiRouter";

export default function AccountOrderStatus() {
  const { orderId } = useParams();
  const [order, setOrder] = useState<Order>();

  useEffect(() => {
    const loadOrder = async () => {
      const response = await ApiRouter.order.get<Order>(orderId!);
      setOrder(response.data);
    };
    loadOrder();
  }, [orderId]);

  const handleOrderStatusUpdated = (order: Order) => {
    setOrder(order);
  };

  useEffect(() => {
    const connection = ApiRouter.createSignalRConnection("order/hub");
    connection
      .start()
      .then(() => {
        connection.invoke("SubscribeToOrder", orderId);
        connection.on("OrderStatusUpdated", handleOrderStatusUpdated);
      })
      .catch((error) => {
        console.log(error);
      });

    return () => {
      connection.invoke("UnsubscribeFromOrder", orderId);
      connection.off("OrderStatusUpdated", handleOrderStatusUpdated);
      connection.stop();
    };
  }, [orderId]);

  if (!order) return <h3>Not found</h3>;

  return (
    <table className="account-order-table" key={order.id}>
      <h3>Order status</h3>
      <tbody>
        <tr className="account-order-table-row">
          <td className="account-order-table-row-title">Order date</td>
          <td className="account-order-table-row-value">{new Date(order.dateTime).toLocaleString()}</td>
        </tr>
        <tr className="account-order-table-row">
          <td className="account-order-table-row-title">Total price</td>
          <td className="account-order-table-row-value">{order.totalPrice.toFixed(2)}</td>
        </tr>
        <tr className="account-order-table-row">
          <td className="account-order-table-row-title">State</td>
          <td className="account-order-table-row-value">{order.state}</td>
        </tr>
        <tr className="account-order-table-row">
          <td className="account-order-table-row-title">Items to buy</td>
          <td className="account-order-table-row-value">{order.items.length}</td>
        </tr>
      </tbody>
    </table>
  );
}
