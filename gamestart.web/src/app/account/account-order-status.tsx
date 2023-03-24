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
  }, []);

  if (!order) return <h3>Not found</h3>;

  return (
    <table key={order.id}>
      <tbody>
        <tr>
          <td>Order date</td>
          <td>{order.dateTime.toString()}</td>
        </tr>
        <tr>
          <td>Total price</td>
          <td>{order.totalPrice.toFixed(2)}</td>
        </tr>
        <tr>
          <td>State</td>
          <td>{order.state}</td>
        </tr>
        <tr>
          <td>Bought items</td>
          <td>{order.items.length}</td>
        </tr>
      </tbody>
    </table>
  );
}
