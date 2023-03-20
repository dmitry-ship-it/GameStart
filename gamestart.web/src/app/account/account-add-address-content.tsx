import axios from "axios";
import { Address, AddressRequest } from "../util/types";
import ApiRouter from "../util/ApiRouter";
import { useNavigate } from "react-router-dom";

export default function AccountAddAddressContent(ctx: {
  isOpenPopupFunc: React.Dispatch<React.SetStateAction<boolean>>;
  allAddresses: Address[];
  setAllAddresses: React.Dispatch<React.SetStateAction<Address[]>>;
}) {
  const navigate = useNavigate();

  const addAddress = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();

    const form = document.querySelector<HTMLFormElement>("#add-form")!;
    const formData = new FormData(form);

    const body = axios.formToJSON(formData) as AddressRequest;
    const response = await ApiRouter.address.post("", body, false);

    if (response.status === 200) {
      const newAddress = body as Address;

      // FIXME: this call does not causes updated object fetch which id can be used for address deletion
      ctx.setAllAddresses(ctx.allAddresses.concat(newAddress));
      ctx.isOpenPopupFunc(false);
      navigate("/account");
    }
  };

  return (
    <form className="account-address-form" id="add-form">
      <label className="account-address-form-label">
        <span className="account-address-form-title">Country</span>
        <input className="account-address-form-value" type="text" name="country" required />
      </label>
      <label className="account-address-form-label">
        <span className="account-address-form-title">State</span>
        <input className="account-address-form-value" type="text" name="state" />
      </label>
      <label className="account-address-form-label">
        <span className="account-address-form-title">City</span>
        <input className="account-address-form-value" type="text" name="city" required />
      </label>
      <label className="account-address-form-label">
        <span className="account-address-form-title">Street</span>
        <input className="account-address-form-value" type="text" name="street" required />
      </label>
      <label className="account-address-form-label">
        <span className="account-address-form-title">House</span>
        <input className="account-address-form-value" type="text" name="house" required />
      </label>
      <label className="account-address-form-label">
        <span className="account-address-form-title">Flat</span>
        <input className="account-address-form-value" type="text" name="flat" />
      </label>
      <label className="account-address-form-label">
        <span className="account-address-form-title">Postcode</span>
        <input className="account-address-form-value" type="text" name="postCode" required />
      </label>
      <button className="account-address-form-button" type="submit" onClick={addAddress}>
        Add
      </button>
    </form>
  );
}
