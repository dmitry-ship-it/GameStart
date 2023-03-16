import { NavLink } from "react-router-dom";

export default function AccountButton() {
  return (
    <NavLink to="/account" className="account-button">
      Account
    </NavLink>
  );
}
