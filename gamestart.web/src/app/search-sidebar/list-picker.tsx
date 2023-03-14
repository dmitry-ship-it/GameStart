import { useEffect, useState } from "react";
import ListItem from "./list-item";
import "../style/app.scss";
import Popup from "reactjs-popup";
import PopupContent from "./popup-content";
import { BasicModel } from "../util/types";
import { selected } from "./search-sidebar";

export default function ListPicker(ctx: { list: BasicModel[]; name: string }) {
  const [list, setList] = useState<BasicModel[]>([]);
  const [notPicked, setNotPicked] = useState<BasicModel[]>([]);

  useEffect(() => {
    setNotPicked(ctx.list);
  }, [ctx.list]);

  const removeItem = (item: BasicModel) => {
    const newList = list.filter((i) => i.id !== item.id);
    setList(newList);
    setNotPicked(notPicked.concat(item));
    selected.set(ctx.name, newList);
  };

  const addItem = (item: BasicModel) => {
    setNotPicked(notPicked.filter((i) => i.id !== item.id));
    const newList = list.concat(item);
    setList(newList);
    selected.set(ctx.name, newList);
  };

  return (
    <div className="list-picker">
      <div className="list-picker-container">
        {list.map((item) => (
          <ListItem value={item} deleteFunc={removeItem} key={item.id} />
        ))}
      </div>
      <Popup
        arrow={false}
        trigger={
          <button className="list-picker-button" type="button">
            +
          </button>
        }
        position="right bottom">
        <PopupContent notPicked={notPicked} addItemFunc={addItem} />
      </Popup>
    </div>
  );
}
