import "../style/app.scss";
import TextSearchField from "../search-sidebar/text-search-field";
import ReleaseDatePicker from "./release-date-picker-group";
import PricePickerGroup from "./price-picker-group";
// import ListPicker from "./list-picker-group";

// const arr = ["123", "abc"];

export default function SearchSidebar() {
  return (
    <form className="search-sidebar">
      <TextSearchField name="Title" />
      <TextSearchField name="Publisher" />
      <ReleaseDatePicker name="date" displayName="Release Date" />
      <PricePickerGroup name="price" displayName="Price" />
      {/* <ListPicker list={arr} displayName="Developers" /> */}
    </form>
  );
}