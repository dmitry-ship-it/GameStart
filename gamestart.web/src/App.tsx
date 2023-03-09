import React from "react";
import logo from "./logo.svg";
import "./App.css";
import Header from "./header/header";
import Footer from "./footer/footer";
import Main from "./app/main";

export default function App() {
  return (
    <div className="App">
      <Header />
      <Main />
      <Footer />
    </div>
  );
}
