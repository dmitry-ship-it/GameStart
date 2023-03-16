import Cookies from "js-cookie";
import jwtDecode from "jwt-decode";

export function decodeJwt() {
  const cookie = Cookies.get("Authorization");
  if (cookie !== undefined && cookie !== null) {
    return jwtDecode<any>(cookie);
  }

  return null;
}
