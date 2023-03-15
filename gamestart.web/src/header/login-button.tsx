import { NavLink } from "react-router-dom";
import "./style/header.scss";

export default function LoginButton() {
  return (
    <NavLink to="/login" className="login-button">
      Login
    </NavLink>
  );
}
