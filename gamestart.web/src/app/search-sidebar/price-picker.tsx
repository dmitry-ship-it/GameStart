import "../style/app.scss";

export default function PricePicker(ctx: { id: string; placeholder: string }) {
  return (
    <input
      className="value-picker"
      type="text"
      id={ctx.id}
      name={ctx.id}
      placeholder={ctx.placeholder}
      onFocus={(e) => {
        e.target.value = "";
        e.target.type = "number";
      }}
      onBlur={(e) => {
        const value = "$" + e.currentTarget.valueAsNumber;
        e.target.type = "text";
        if (e.target.value) e.currentTarget.value = value;
      }}
    />
  );
}
