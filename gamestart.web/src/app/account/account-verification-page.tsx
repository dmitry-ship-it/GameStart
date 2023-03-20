import { store } from "../../App";
import ApiRouter from "../util/ApiRouter";
import { decodeJwt } from "../util/helpers";
import AccountUnauthorized from "./account-unauthorized";
import "../style/account.scss";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import AccountEmailVerified from "./account-email-verified";

export default function AccountVerificationPage() {
  const [message, setMessage] = useState("");
  const navigate = useNavigate();
  const isLoggedIn = store.getState("isLoggedIn").getValue<boolean>();
  const user = decodeJwt();

  const sendVerificationEmail = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();
    await ApiRouter.account.get("send-verification-email");
    setMessage(`Message was sent to ${user.email}`);
  };

  const submitCode = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();

    const form = document.querySelector<HTMLFormElement>(".account-form")!;
    const formData = new FormData(form);

    const response = await ApiRouter.account.get(`verifyEmail?token=${formData.get("token")}`);

    if (response.status === 200) {
      setMessage("Email verified!");
      setTimeout(() => {
        navigate("/account");
      }, 4000);
    } else {
      setMessage("Invalid token");
    }
  };

  if (!isLoggedIn) return <AccountUnauthorized />;
  if (user.email_verified) return <AccountEmailVerified />;

  return (
    <div className="account-page">
      <form className="account-form">
        <label className="account-label">
          <span className="account-label-text">Verification code</span>
          <input className="account-input" type="text" name="token" />
        </label>
        <div className="account-verification-button-group">
          <button className="account-verification-submit-button" type="submit" onClick={submitCode}>
            Submit code
          </button>
          <button className="account-verification-email-button" type="button" onClick={sendVerificationEmail}>
            Send code to email
          </button>
        </div>
      </form>
      <span className="account-info-message" id="message">
        {message}
      </span>
    </div>
  );
}
