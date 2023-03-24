import { Address } from "../util/types";

export default function CartCheckoutPageAddress(ctx: { address: Address }) {
  return (
    <table className="cart-address-table">
      <tbody>
        {Object.entries(ctx.address)
          .filter(([key]) => !key.toLowerCase().includes("id"))
          .map(([key, value]) => (
            <tr className="cart-address-card-row" key={ctx.address.id + "row" + key}>
              <td className="cart-address-card-label" key={key + ctx.address.id}>
                {key.charAt(0).toUpperCase() + key.slice(1)}
              </td>
              <td className="cart-address-card-value" key={value + ctx.address.id}>
                {value}
              </td>
            </tr>
          ))}
      </tbody>
    </table>
  );
}
