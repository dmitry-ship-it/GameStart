import LoginButton from "./login-button";
import RegisterButton from "./register-button";
import "./style/header.scss";

export default function Header() {
  return (
    <div className="navbar">
      <header className="title">Game Start</header>
      <div className="account-buttons-box">
        <RegisterButton />
        <LoginButton />
      </div>
    </div>
  );
}
