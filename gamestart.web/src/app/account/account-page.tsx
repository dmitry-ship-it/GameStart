import { NavLink } from "react-router-dom";
import { store } from "../../App";
import { decodeJwt } from "../util/helpers";
import AccountUnauthorized from "./account-unauthorized";
import AccountAddress from "./account-address";
import ApiRouter from "../util/ApiRouter";
import { Address } from "../util/types";
import { useEffect, useState } from "react";
import AccountAddAddress from "./account-add-address";

export default function AccountPage() {
  const isLoggedIn = store.getState("isLoggedIn").getValue<boolean>();
  const user = decodeJwt();

  const [addresses, setAddresses] = useState<Address[]>([]);

  useEffect(() => {
    const loadAddresses = async () => {
      const response = await ApiRouter.address.get<Address[]>("", false);
      if (response.status === 200) {
        setAddresses(response.data);
      }
    };
    loadAddresses();
  }, []);

  return !isLoggedIn ? (
    <AccountUnauthorized />
  ) : (
    <div className="account-page">
      <div className="account-info-group">
        <div className="account-info-box">
          <table className="account-summary">
            <caption className="account-summary-title">Profile summary</caption>
            <tbody>
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
            </tbody>
          </table>
        </div>
        <div className="account-add-address-group">
          <span className="account-add-address-title">Shipping addresses</span>
          {addresses.map((address) => (
            <AccountAddress address={address} allAddresses={addresses} setAddresses={setAddresses} key={address.id} />
          ))}
          <AccountAddAddress allAddresses={addresses} setAllAddresses={setAddresses} />
        </div>
      </div>
    </div>
  );
}
