import { useNavigate } from "react-router-dom";
import "../style/account.scss";
import LoginToRegisterBox from "./login-to-register-box";
import LoginExternal from "./login-register-external";
import ApiRouter from "../util/ApiRouter";
import { LoginModel } from "../util/types";
import { useState } from "react";
import { store } from "../../App";

export default function Login() {
  const [error, setError] = useState<string>();
  const [, setIsLoggedIn] = store.useState<boolean>("isLoggedIn");
  const navigate = useNavigate();

  const handleLogin = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();

    const form = document.querySelector<HTMLFormElement>(".account-form")!;

    form.querySelectorAll<HTMLInputElement>("[required]").forEach((element) => {
      if (!element.reportValidity()) return;
    });

    const formData = new FormData(form);
    const response = await ApiRouter.account.post<LoginModel>("login", {
      username: formData.get("username") as string,
      password: formData.get("password") as string,
    });

    if (response.status !== 200) {
      setError("Login error");
    } else {
      setIsLoggedIn(true);
      navigate("/");
    }
  };

  return (
    <div className="account-page">
      <div className="account-page-group">
        <form className="account-form">
          <label className="account-label">
            <span className="account-label-text">Username</span>
            <input className="account-input" type="text" name="username" required={true} />
          </label>
          <label className="account-label">
            <span className="account-label-text">Password</span>
            <input className="account-input" type="password" name="password" required={true} />
          </label>
          <button className="account-form-button" type="submit" onClick={handleLogin}>
            Login
          </button>
          <span className="account-form-error">{error}</span>
        </form>
        <LoginToRegisterBox />
        <LoginExternal schemes={["Google"]} label="Login with:" />
      </div>
    </div>
  );
}
