import { useEffect } from "react";
import "./App.scss";
import Header from "./header/header";
import Footer from "./footer/footer";
import Main from "./app/main";
import { Routes, Route } from "react-router-dom";
import Login from "./app/account/login";
import { createStore } from "state-pool";
import jwtDecode from "jwt-decode";
import { useCookies } from "react-cookie";
import reportWebVitals from "./reportWebVitals";
import Register from "./app/account/register";
import AccountPage from "./app/account/account-page";
import GamePage from "./app/game/game-page";
import { CartItemWrapper, VideoGame } from "./app/util/types";
import AccountVerificationPage from "./app/account/account-verification-page";
import CartPage from "./app/cart/cart-page";
import CartCheckoutPage from "./app/cart/cart-checkout-page";
import AccountOrderStatus from "./app/account/account-order-status";
import AccountInventoryPage from "./app/account/account-inventory-page";
import Cookies from "js-cookie";

export const store = createStore();
store.persist({
  saveState: (key, state, isInitialState) => {
    localStorage.setItem(key, JSON.stringify(state));
  },
  loadState: (key, noState) => {
    const data = localStorage.getItem(key);
    if (data === null) return noState;

    return JSON.parse(data);
  },
});

store.setState("isLoggedIn", false);

const initialGames: VideoGame[] = [];
store.setState("games", initialGames);

const initialCart: CartItemWrapper[] = [];
store.setState("cart", initialCart, { persist: true });

export default function App() {
  const [isLoggedIn, setIsLoggedIn] = store.useState<boolean>("isLoggedIn");

  useEffect(() => {
    const cookie = Cookies.get("Authorization");
    let loggedIn = false;
    if (cookie !== undefined) {
      const jwt = jwtDecode<any>(cookie);
      loggedIn = Date.now() < (jwt.exp as number) * 1000;
      if (loggedIn !== isLoggedIn) setIsLoggedIn(loggedIn);
    }
  }, [isLoggedIn, setIsLoggedIn]);

  return (
    <div className="App">
      <Header />
      <Routes>
        <Route path="/" element={<Main />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/account" element={<AccountPage />} />
        <Route path="/account/verifyEmail" element={<AccountVerificationPage />} />
        <Route path="/account/inventory" element={<AccountInventoryPage />} />
        <Route path="/account/cart" element={<CartPage />} />
        <Route path="/account/cart/checkout" element={<CartCheckoutPage />} />
        <Route path="/account/order/:orderId" element={<AccountOrderStatus />} />
        <Route path="/game/:gameId" element={<GamePage />} />
      </Routes>
      <Footer />
    </div>
  );
}

reportWebVitals();
