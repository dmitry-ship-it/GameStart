import { useNavigate } from "react-router-dom";
import { store } from "../../App";
import { useState } from "react";
import ApiRouter from "../util/ApiRouter";
import { RegisterModel } from "../util/types";
import LoginExternal from "./login-register-external";
import RegisterToLoginBox from "./register-to-login-box";

export default function Register() {
  const [error, setError] = useState<string>();
  const [isLoggedIn, setIsLoggedIn] = store.useState<boolean>("isLoggedIn");
  const navigate = useNavigate();

  const handleLogin = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();

    const form = document.querySelector<HTMLFormElement>(".account-form")!;
    const formData = new FormData(form);

    const password = formData.get("password") as string;
    const confirmPassword = formData.get("confirm-password") as string;
    if (password !== confirmPassword) {
      setError("Passwords are not matching");
      return;
    }

    const response = await ApiRouter.account.post<RegisterModel>("register", {
      username: formData.get("username") as string,
      email: formData.get("email") as string,
      password: password,
    });

    if (response.status !== 200) {
      setError("Register error");
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
            <input className="account-input" type="text" name="username" />
          </label>
          <label className="account-label">
            <span className="account-label-text">Email</span>
            <input className="account-input" type="email" name="email" />
          </label>
          <label className="account-label">
            <span className="account-label-text">Password</span>
            <input className="account-input" type="password" name="password" />
          </label>
          <label className="account-label">
            <span className="account-label-text">Confirm password</span>
            <input className="account-input" type="password" name="confirm-password" />
          </label>
          <button className="account-form-button" type="submit" onClick={handleLogin}>
            Register
          </button>
          <span className="account-form-error">{error}</span>
        </form>
        <RegisterToLoginBox />
        <LoginExternal schemes={["Google"]} label="Register with:" />
      </div>
    </div>
  );
}
