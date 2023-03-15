import ApiRouter from "../app/util/ApiRouter";

export default function LogoutButton() {
  const handleLogout = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();
    await ApiRouter.account.get("logout");
  };

  return (
    <button className="logout-button" type="button" onClick={handleLogout}>
      Logout
    </button>
  );
}
