import PricePicker from "./price-picker";
import "../style/app.scss";

export default function PricePickerGroup(ctx: { name: string; displayName: string }) {
  const from = ctx.name + "From";
  const to = ctx.name + "To";

  return (
    <>
      <span>{ctx.displayName}</span>
      <div className="search-picker">
        <PricePicker id={from} placeholder="from" />
        <span className="search-picker-delimiter"> - </span>
        <PricePicker id={to} placeholder="to" />
      </div>
    </>
  );
}
