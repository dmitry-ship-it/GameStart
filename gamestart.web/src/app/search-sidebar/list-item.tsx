import "../style/app.scss";
import { BasicModel } from "../util/types";

export default function ListItem(ctx: { value: BasicModel; deleteFunc: (item: BasicModel) => void }) {
  return (
    <span className="list-item">
      {ctx.value.name}
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
