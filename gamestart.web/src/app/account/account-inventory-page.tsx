import { useEffect, useState } from "react";
import { InventoryItem } from "../util/types";
import ApiRouter from "../util/ApiRouter";
import { NavLink } from "react-router-dom";

export default function AccountInventoryPage() {
  const [inventoryItems, setInventoryItems] = useState<InventoryItem[]>();

  useEffect(() => {
    const loadInventory = async () => {
      const response = await ApiRouter.inventory.get<InventoryItem[]>("", false);
      setInventoryItems(response.data);
    };
    if (inventoryItems === undefined) loadInventory();
  }, [inventoryItems]);

  if (inventoryItems === undefined) return <h3>Loading...</h3>;
  if (inventoryItems !== undefined && inventoryItems.length === 0) return <h3>Your inventory is empty</h3>;

  return (
    <div className="account-inventory-page">
      {inventoryItems.map((item) => (
        <table className="account-inventory-table" key={item.id}>
          <tbody>
            <tr className="account-inventory-table-row">
              <td className="account-inventory-table-row-title">Title</td>
              <td className="account-inventory-table-row-value">{item.title}</td>
            </tr>
            <tr className="account-inventory-table-row">
              <td className="account-inventory-table-row-title">Purchase date</td>
              <td className="account-inventory-table-row-value">{new Date(item.purchaseDateTime).toLocaleString()}</td>
            </tr>
            <tr className="account-inventory-table-row">
              <td className="account-inventory-table-row-title">Digital key</td>
              <td className="account-inventory-table-row-value">{item.gameKey}</td>
            </tr>
          </tbody>
          <NavLink className="account-inventory-table-link" to={`/game/${item.gameId}`}>
            Go to shop page
          </NavLink>
        </table>
      ))}
    </div>
  );
}
