import logo from "../../logo.svg";
import ApiRouter from "../util/ApiRouter";

export default function LoginRegisterExternal(ctx: { schemes: string[]; label: string }) {
  const handleExternalLogin = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>, scheme: string) => {
    e.preventDefault();
    const returnUrl = "https://localhost:3000";
    await ApiRouter.account.get(`challenge?scheme=${scheme}&returnUrl=${returnUrl}`);
  };

  return (
    <div className="account-external">
      <span className="account-external-text">{ctx.label} </span>
      {ctx.schemes.map((scheme) => (
        <button className="account-external-button" type="button" key={scheme} onClick={(e) => handleExternalLogin(e, scheme)}>
          <img className="account-external-picture" src={logo} alt={scheme} title={scheme} />
        </button>
      ))}
    </div>
  );
}
