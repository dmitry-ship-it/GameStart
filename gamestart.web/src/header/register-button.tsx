import { NavLink } from "react-router-dom";
import "./style/header.scss";

export default function RegisterButton() {
  return (
    <NavLink to="/register" className="register-button">
      Register
    </NavLink>
  );
}
