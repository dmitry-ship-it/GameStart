import "../style/app.scss";
import DatePicker from "./date-picker";

export default function ReleaseDatePicker(ctx: { name: string; displayName: string }) {
  const from = ctx.name + "From";
  const to = ctx.name + "To";

  return (
    <>
      <span>{ctx.displayName}</span>
      <div className="search-picker">
        <DatePicker id={from} placeholder="from" />
        <span className="search-picker-delimiter"> - </span>
        <DatePicker id={to} placeholder="to" />
      </div>
    </>
  );
}
