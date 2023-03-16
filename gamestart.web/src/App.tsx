import React, { useEffect } from "react";
import logo from "./logo.svg";
import "./App.css";
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
import { VideoGame } from "./app/util/types";

export const store = createStore();
store.setState("isLoggedIn", false);

const intialGames: VideoGame[] = [];
store.setState("games", intialGames);

export default function App() {
  const [cookies, setCookie] = useCookies(["Authorization"]);
  const [isLoggedIn, setIsLoggedIn] = store.useState<boolean>("isLoggedIn");

  useEffect(() => {
    const interval = setInterval(() => {
      const cookie = cookies.Authorization;
      let loggedIn = false;
      if (cookie !== undefined && cookie !== null) {
        const jwt = jwtDecode<any>(cookie);
        loggedIn = Date.now() < (jwt.exp as number) * 1000;
        if (loggedIn !== isLoggedIn) setIsLoggedIn(loggedIn);
      }
    }, 200);

    return () => clearInterval(interval);
  }, [cookies.Authorization, isLoggedIn, setIsLoggedIn]);

  return (
    <div className="App">
      <Header />
      <Routes>
        <Route path="/" element={<Main />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/account" element={<AccountPage />} />
        <Route path="/game/:gameId" element={<GamePage />} />
      </Routes>
      <Footer />
    </div>
  );
}

reportWebVitals();
