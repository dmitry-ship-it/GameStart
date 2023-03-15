import { NavLink } from "react-router-dom";

export default function LoginRegisterBox() {
  return (
    <div className="account-register-box">
      <span className="account-register-text">Don't have an account yet?</span>
      <NavLink className="account-register-link" to="/register">
        Register
      </NavLink>
    </div>
  );
}
