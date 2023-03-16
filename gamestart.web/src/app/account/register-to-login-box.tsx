import { NavLink } from "react-router-dom";

export default function RegisterToLoginBox() {
  return (
    <div className="account-register-box">
      <span className="account-register-text">Already have an account?</span>
      <NavLink className="account-register-link" to="/login">
        Login
      </NavLink>
    </div>
  );
}
