export default function PopupContent(ctx: { notPicked: string[]; addItemFunc: (item: string) => void }) {
  return (
    <div className="popup-content">
      {ctx.notPicked.map((item) => (
        <button className="popup-content-item" onClick={() => ctx.addItemFunc(item)} key={item}>
          {item}
        </button>
      ))}
    </div>
  );
}
