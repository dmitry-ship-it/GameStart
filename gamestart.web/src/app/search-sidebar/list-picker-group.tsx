import ListPicker from "./list-picker";

export default function ListPickerGroup(ctx: { list: string[]; displayName: string }) {
  return (
    <>
      <span>{ctx.displayName}</span>
      <ListPicker list={ctx.list} />
    </>
  );
}
