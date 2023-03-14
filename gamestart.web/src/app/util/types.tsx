export interface BasicModel {
  id: string;
  name: string;
}

export interface VideoGame {
  id: string;
  title: string;
  description: string | null;
  copyright: string | null;
  releaseDate: Date;
  price: number;
  publisher: Publisher;
  developers: Developer[];
  genres: Genre[];
  languageAvailabilities: LanguageAvailability[];
  systemRequirements: SystemRequirement[];
}

export interface LanguageAvailability {
  id: string;
  availableForInterface: boolean;
  availableForAudio: boolean;
  availableForSubtitles: boolean;
  language: Language;
}

export interface SystemRequirement {
  id: string;
  os: string | null;
  processor: string | null;
  memory: string | null;
  graphics: string | null;
  network: string | null;
  storage: string | null;
  platform: Platform;
}

export interface Developer extends BasicModel {}
export interface Genre extends BasicModel {}
export interface Language extends BasicModel {}
export interface Platform extends BasicModel {}
export interface Publisher extends BasicModel {}

export interface VideoGameSearchParams {
  title: string | null;
  releasedFrom: Date;
  releasedTo: Date | null;
  priceFrom: number;
  priceTo: number | null;
  publisher: string | null;
  developers: string[] | null;
  genres: string[] | null;
  languages: string[] | null;
  platforms: string[] | null;
}
