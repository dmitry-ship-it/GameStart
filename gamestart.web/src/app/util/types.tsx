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

export interface LoginModel {
  username: string;
  password: string;
}

export interface RegisterModel extends LoginModel {
  email: string;
}

export interface AddressRequest {
  country: string;
  state: string | null;
  city: string;
  street: string;
  house: string;
  flat: string | null;
  postCode: string;
}

export interface OrderItem {
  gameId: string;
  isPhysicalCopy: boolean;
}

export interface OrderRequest {
  items: OrderItem[];
  address: AddressRequest | null;
}

export interface Address extends AddressRequest {
  id: string;
  userId: string;
}

export interface CartItemWrapper {
  id: string;
  isPhysicalCopy: boolean;
  count: number;
  game: VideoGame;
}

export interface Item extends OrderItem {
  id: string;
}

export interface Order {
  id: string;
  userId: string;
  dateTime: Date;
  totalPrice: number;
  state: string;
  items: Item[];
  address: Address;
}

export interface InventoryItem {
  id: string;
  gameId: string;
  title: string;
  gameKey: string;
  purchaseDateTime: Date;
}
