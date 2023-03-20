import { useNavigate } from "react-router-dom";
import { store } from "../App";
import ApiRouter from "../app/util/ApiRouter";

export default function LogoutButton() {
  const [, setIsLoggedIn] = store.useState<boolean>("isLoggedIn");
  const navigate = useNavigate();

  const handleLogout = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();
    const response = await ApiRouter.account.get("logout");
    if (response.status === 200) {
      setIsLoggedIn(false);
      navigate("/");
    }
  };

  return (
    <button className="logout-button" type="button" onClick={handleLogout}>
      Logout
    </button>
  );
}
