import { BasicModel } from "../util/types";

export default function PopupContent(ctx: { notPicked: BasicModel[]; addItemFunc: (item: BasicModel) => void }) {
  return (
    <div className="popup-content">
      {ctx.notPicked.map((item) => (
        <button className="popup-content-item" onClick={() => ctx.addItemFunc(item)} key={item.id}>
          {item.name}
        </button>
      ))}
    </div>
  );
}
