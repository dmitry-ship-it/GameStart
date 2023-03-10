import "../style/app.scss";

export default function ListItem(ctx: { value: string; deleteFunc: (item: string) => void }) {
  return (
    <span className="list-item">
      {ctx.value}
      <button
        className="list-item-button"
        type="button"
        onClick={(e) => {
          e.preventDefault();
          ctx.deleteFunc(ctx.value);
        }}>
        ✖
      </button>
    </span>
  );
}
