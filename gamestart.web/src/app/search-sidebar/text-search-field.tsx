import "../style/app.scss";

export default function TextSearchField(ctx: { name: string }) {
  return <input className="search-sidebar-item" type="text" id={ctx.name} name={ctx.name} placeholder={ctx.name} autoComplete="off" />;
}
