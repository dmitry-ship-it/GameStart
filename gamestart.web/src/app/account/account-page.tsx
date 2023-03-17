import { NavLink } from "react-router-dom";
import { store } from "../../App";
import { decodeJwt } from "../util/helpers";
import AccountUnauthorized from "./account-unauthorized";

export default function AccountPage() {
  const isLoggedIn = store.getState("isLoggedIn").getValue<boolean>();
  const user = decodeJwt();

  return !isLoggedIn ? (
    <AccountUnauthorized />
  ) : (
    <div className="account-page">
      <div className="account-info-box">
        <table className="account-summary">
          <caption className="account-summary-title">Profile summary</caption>
          <tr className="account-summary-row">
            <td className="account-summary-item-title">Username</td>
            <td className="account-summary-item">{user.name}</td>
          </tr>
          <tr className="account-summary-row">
            <td className="account-summary-item-title">Email</td>
            <td className="account-summary-item">{user.email}</td>
          </tr>
          <tr className="account-summary-row">
            <td className="account-summary-item-title">Role</td>
            <td className="account-summary-item">{user.role}</td>
          </tr>
          <tr className="account-summary-row">
            <td className="account-summary-item-title">Email verified</td>
            <td className="account-summary-item">
              {user.email_verified ? (
                "Yes"
              ) : (
                <>
                  No <NavLink to="/account/verifyEmail">[verify email]</NavLink>
                </>
              )}
            </td>
          </tr>
        </table>
      </div>
    </div>
  );
}
