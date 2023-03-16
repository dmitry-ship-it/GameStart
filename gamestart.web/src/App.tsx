import React, { useEffect } from "react";
import logo from "./logo.svg";
import "./App.css";
import Header from "./header/header";
import Footer from "./footer/footer";
import Main from "./app/main";
import { Routes, Route } from "react-router-dom";
import Login from "./app/login-page/login";
import { createStore } from "state-pool";
import jwtDecode from "jwt-decode";
import { useCookies } from "react-cookie";
import reportWebVitals from "./reportWebVitals";

export const store = createStore();
store.setState("isLoggedIn", false);

export default function App() {
  // const [cookies, setCookie] = useCookies(["Authorization"]);
  // const [isLoggedIn, setIsLoggedIn] = store.useState<boolean>("isLoggedIn");

  // useEffect(() => {
  //   const checkAuthCookie = () => {
  //     const cookie = cookies.Authorization;
  //     if (cookie !== undefined && cookie !== null) {
  //       const jwt = jwtDecode<any>(cookie);
  //       return Date.now() < (jwt.exp as number) * 1000;
  //     }
  //     return false;
  //   };

  //   if (checkAuthCookie() !== isLoggedIn) setIsLoggedIn(!isLoggedIn);
  // });

  return (
    <div className="App">
      <Header />
      <Routes>
        <Route path="/" element={<Main />} />
        <Route path="/login" element={<Login />} />
        {/* <Route path="/register" element={<Register />} /> */}
      </Routes>
      <Footer />
    </div>
  );
}

reportWebVitals();
