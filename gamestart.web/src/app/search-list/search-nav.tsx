import { useEffect } from "react";

export default function SearchNav(ctx: { gameCount: number; pageSize: number; setPageFunc: React.Dispatch<React.SetStateAction<number>> }) {
  const pages: number[] = [];
  for (let page = 0; page < Math.round(ctx.gameCount / ctx.pageSize) + 1; page++) {
    pages.push(page + 1);
  }

  let currentPage = pages[0];

  const setPage = (page: number) => {
    if (!pages.includes(page)) return;

    for (const pageNumber of pages) {
      const button = document.querySelector<HTMLButtonElement>(`#page-${pageNumber}`)!;
      button.style.textDecoration = "none";
    }

    const selectedButton = document.querySelector<HTMLButtonElement>(`#page-${page}`)!;
    selectedButton.style.textDecoration = "underline";

    currentPage = page;
    ctx.setPageFunc(page);
  };

  useEffect(() => setPage(pages[0]));

  return (
    <span className="search-nav">
      <button className="search-nav-button" onClick={() => setPage(pages[0])}>
        first
      </button>
      <span className="search-nav-middle">
        <button className="search-nav-button" onClick={() => setPage(currentPage - 1)}>
          {"<"}
        </button>
        {pages.map((page) => (
          <button className="search-nav-button" id={`page-${page}`} key={page} onClick={() => setPage(page)}>
            {page}
          </button>
        ))}
        <button className="search-nav-button" onClick={() => setPage(currentPage + 1)}>
          {">"}
        </button>
      </span>
      <button className="search-nav-button" onClick={() => setPage(pages[pages.length - 1])}>
        last
      </button>
    </span>
  );
}
