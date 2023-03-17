import { NavLink } from "react-router-dom";

export default function AccountEmailVerified() {
  return (
    <span className="account-message-page">
      Your email is verified <NavLink to="/account">Go back</NavLink>
    </span>
  );
}
