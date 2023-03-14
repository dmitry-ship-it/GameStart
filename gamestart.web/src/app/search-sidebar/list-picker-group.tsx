import { BasicModel } from "../util/types";
import ListPicker from "./list-picker";

export default function ListPickerGroup(ctx: { list: BasicModel[]; displayName: string }) {
  return (
    <>
      <span>{ctx.displayName}</span>
      <ListPicker list={ctx.list} name={ctx.displayName} />
    </>
  );
}
