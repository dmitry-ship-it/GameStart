import "../style/app.scss";
import TextSearchField from "../search-sidebar/text-search-field";
import ReleaseDatePicker from "./release-date-picker-group";
import PricePickerGroup from "./price-picker-group";
import ListPicker from "./list-picker-group";
import ApiRouter from "../util/ApiRouter";
import { BasicModel, Developer, Genre, Language, Platform, VideoGame, VideoGameSearchParams } from "../util/types";
import { useEffect, useState } from "react";

const handleSearchRequest = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
  e.preventDefault();

  const form = document.querySelector<HTMLFormElement>(".search-sidebar")!;
  const formData = new FormData(form);

  const model: VideoGameSearchParams = {
    title: formData.get("title") as string,
    releasedFrom: new Date(formData.get("dateFrom") as string),
    releasedTo: new Date(formData.get("dateTo") as string),
    priceFrom: Number.parseFloat((formData.get("priceFrom") as string).replace("$", "")),
    priceTo: Number.parseFloat((formData.get("priceTo") as string).replace("$", "")),
    publisher: formData.get("publisher") as string,
    developers: selected.get("Developers")?.map(({ name }) => name) ?? null,
    genres: selected.get("Genres")?.map(({ name }) => name) ?? null,
    languages: selected.get("Languages")?.map(({ name }) => name) ?? null,
    platforms: selected.get("Platforms")?.map(({ name }) => name) ?? null,
  };

  const findValue = (model: Object, key: string) => Object.keys(model).find((k) => k === key);

  const queryString =
    "?" +
    Object.keys(model)
      .map((key) => {
        const value = findValue(model, key);
        return Array.isArray(value)
          ? Array.from(value)
              .map((v) => `${key}=${v}`)
              .join("&")
          : `${key}=${value}`;
      })
      .join("&");

  const { data } = await ApiRouter.catalog.get<VideoGame[]>("search" + queryString);

  setFoundItems(data);
};

async function getCategory<T extends BasicModel>(categoryName: string) {
  const { data } = await ApiRouter.catalog.get<T[]>(categoryName);
  return data;
}

export let selected = new Map<string, BasicModel[]>();
let setFoundItems: React.Dispatch<React.SetStateAction<VideoGame[]>>;

export default function SearchSidebar(ctx: { setFoundItems: React.Dispatch<React.SetStateAction<VideoGame[]>> }) {
  setFoundItems = ctx.setFoundItems;

  const [developers, setDevelopers] = useState<Developer[]>([]);
  const [genres, setGenres] = useState<Genre[]>([]);
  const [languages, setLanguages] = useState<Language[]>([]);
  const [platforms, setPlatforms] = useState<Platform[]>([]);

  useEffect(() => {
    const developersGetter = async () => {
      const data = await getCategory<Developer>("developers");
      setDevelopers(data);
    };
    const genresGetter = async () => {
      const data = await getCategory<Genre>("genres");
      setGenres(data);
    };
    const languagesGetter = async () => {
      const data = await getCategory<Language>("languages");
      setLanguages(data);
    };
    const platformsGetter = async () => {
      const data = await getCategory<Platform>("platforms");
      setPlatforms(data);
    };

    developersGetter();
    genresGetter();
    languagesGetter();
    platformsGetter();
  }, []);

  return (
    <form className="search-sidebar">
      <TextSearchField name="Title" />
      <TextSearchField name="Publisher" />
      <ReleaseDatePicker name="date" displayName="Release Date" />
      <PricePickerGroup name="price" displayName="Price" />
      <ListPicker list={developers} displayName="Developers" />
      <ListPicker list={genres} displayName="Genres" />
      <ListPicker list={languages} displayName="Languages" />
      <ListPicker list={platforms} displayName="Platforms" />
      <button className="search-sidebar-submit" type="submit" onClick={handleSearchRequest}>
        Search
      </button>
    </form>
  );
}
