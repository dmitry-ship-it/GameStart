import "../style/app.scss";

export default function DatePicker(ctx: { id: string; placeholder: string }) {
  return (
    <input
      className="value-picker"
      type="text"
      id={ctx.id}
      name={ctx.id}
      placeholder={ctx.placeholder}
      onFocus={(e) => (e.target.type = "date")}
      onInputCapture={(e) => {
        const date = e.currentTarget.valueAsDate;
        if (date !== null && date.getFullYear() >= 1950) {
          e.currentTarget.type = "text";
          e.currentTarget.value = date.toLocaleDateString();
          e.currentTarget.blur();
        }
      }}
      min="1950-01-01"
    />
  );
}
