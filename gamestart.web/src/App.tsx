import React from "react";
import logo from "./logo.svg";
import "./App.css";
import Header from "./header/header";
import Footer from "./footer/footer";
import Main from "./app/main";
import { Routes, Route } from "react-router-dom";
import Login from "./app/login-page/login";

export default function App() {
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
