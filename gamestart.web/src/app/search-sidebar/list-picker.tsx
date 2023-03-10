import { useState } from "react";
import ListItem from "./list-item";
import "../style/app.scss";
import Popup from "reactjs-popup";
import PopupContent from "./popup-content";

export default function ListPicker(ctx: { list: string[] }) {
  const [list, setList] = useState(ctx.list);
  const [notPicked, setNotPicked] = useState<string[]>([]);

  const removeItem = (item: string) => {
    setList(list.filter((i) => i !== item));
    setNotPicked(notPicked.concat(item));
  };

  const addItem = (item: string) => {
    setNotPicked(notPicked.filter((i) => i !== item));
    setList(list.concat(item));
  };

  return (
    <div className="list-picker">
      <div className="list-picker-container">
        {list.map((item) => (
          <ListItem value={item} deleteFunc={removeItem} key={item} />
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
