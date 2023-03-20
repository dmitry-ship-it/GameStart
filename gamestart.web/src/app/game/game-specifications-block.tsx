import { useState } from "react";
import { LanguageAvailability, VideoGame } from "../util/types";

export default function GameSpecificationsBlock(ctx: { game: VideoGame }) {
  const [systemRequirements, setSystemRequirements] = useState(ctx.game.systemRequirements[0]);
  const [selected, setSelected] = useState(systemRequirements.id);

  const changeSelection = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();

    const id = e.currentTarget.id;
    if (systemRequirements.id !== id) {
      const found = ctx.game.systemRequirements.find((sr) => sr.id === id)!;
      setSystemRequirements(found);
      setSelected(found.id);
    }
  };

  const compareByLanguageName = (a: LanguageAvailability, b: LanguageAvailability) => a.language.name.localeCompare(b.language.name);

  return (
    <>
      <div className="game-specifications-block">
        <h3 className="game-specifications-title">Specifications</h3>
        <div className="game-specifications-platforms-buttons">
          {ctx.game.systemRequirements.map((r) => (
            <button
              className={"game-specifications-platform-button" + (selected === r.id ? " game-specifications-platform-button-selected" : "")}
              type="button"
              onClick={changeSelection}
              id={r.id}
              key={r.id}>
              {r.platform.name}
            </button>
          ))}
        </div>
        <table className="game-specifications-table" key={systemRequirements.id}>
          {Object.entries(systemRequirements)
            .filter(([key, value]) => value !== null && key !== "platform" && key !== "id")
            .map(([key, value]) => (
              <tr className="game-specifications-row" key={systemRequirements.id}>
                <td className="game-specifications-label" key={key}>
                  {key === "os" ? "OS" : key.charAt(0).toUpperCase() + key.slice(1)}
                </td>
                <td className="game-specifications-value" key={value}>
                  {value}
                </td>
              </tr>
            ))}
        </table>
      </div>
      <div className="game-specifications-block">
        <table className="game-languages-table">
          <tr className="game-languages-title-row">
            <td className="game-languages-col-title-main">Language</td>
            <td className="game-languages-col-title">Interface</td>
            <td className="game-languages-col-title">Audio</td>
            <td className="game-languages-col-title">Subtitles</td>
          </tr>
          {[...ctx.game.languageAvailabilities].sort(compareByLanguageName).map((la) => (
            <tr className="game-languages-row" key={la.id}>
              <td className="game-languages-label" key={la.language.id}>
                {la.language.name}
              </td>
              <td className="game-languages-value" key={la.language.id + "interface"}>
                {la.availableForInterface ? "✓" : ""}
              </td>
              <td className="game-languages-value" key={la.language.id + "audio"}>
                {la.availableForAudio ? "✓" : ""}
              </td>
              <td className="game-languages-value" key={la.language.id + "subtitles"}>
                {la.availableForSubtitles ? "✓" : ""}
              </td>
            </tr>
          ))}
        </table>
      </div>
      <span className="game-specifications-copyright">
        <span className="game-specifications-copyright-title">Copyright: </span>
        {ctx.game.copyright}
      </span>
    </>
  );
}
