import { NavLink } from "react-router-dom";
import LoginButton from "./login-button";
import RegisterButton from "./register-button";
import "./style/header.scss";
import LogoutButton from "./logout-button";
import { store } from "../App";
import { decodeJwt } from "../app/util/helpers";
import AccountButton from "./account-button";

export default function Header() {
  const [isLoggedIn, setIsLoggedIn] = store.useState<boolean>("isLoggedIn");

  return (
    <nav className="navbar">
      <NavLink className="title-link" to="/">
        <header className="title">Game Start</header>
      </NavLink>
      <div className="account-buttons-box">
        {isLoggedIn ? (
          <>
            <span className="accounts-logged-text">Hello, {decodeJwt().name}</span>
            <AccountButton />
            <LogoutButton />
          </>
        ) : (
          <>
            <RegisterButton />
            <LoginButton />
          </>
        )}
      </div>
    </nav>
  );
}
