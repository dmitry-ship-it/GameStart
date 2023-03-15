import { NavLink } from "react-router-dom";
import LoginButton from "./login-button";
import RegisterButton from "./register-button";
import "./style/header.scss";
import LogoutButton from "./logout-button";
import jwtDecode from "jwt-decode";
import { useCookies } from "react-cookie";

export default function Header() {
  const [cookies, setCookie] = useCookies(["Authorization"]);

  const isLoggedIn = () => {
    const cookie = cookies.Authorization;
    if (cookie !== undefined && cookie !== null) {
      const jwt = jwtDecode<any>(cookie);
      return Date.now() < (jwt.exp as number) * 1000;
    }
    return false;
  };

  return (
    <nav className="navbar">
      <NavLink className="title-link" to="/">
        <header className="title">Game Start</header>
      </NavLink>
      <div className="account-buttons-box">
        {isLoggedIn() ? <LogoutButton /> : <RegisterButton />}
        {isLoggedIn() ? <></> : <LoginButton />}
      </div>
    </nav>
  );
}
