import { formToJSON } from "axios";
import ApiRouter from "../util/ApiRouter";
import { Address, AddressRequest } from "../util/types";

export default function AccountEditAddress(ctx: {
  address: Address;
  popupStateFunc: React.Dispatch<React.SetStateAction<boolean>>;
  setAddress: React.Dispatch<React.SetStateAction<Address>>;
}) {
  const handleAddressUpdate = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();

    const form = document.querySelector<HTMLFormElement>("#add-form")!;

    let body = formToJSON(form) as AddressRequest;
    body = Object.fromEntries(Object.entries(body).map(([key, value]) => [key, value === "" ? null : value])) as AddressRequest;

    const response = await ApiRouter.address.put(ctx.address.id, body);

    if (response.status === 200) {
      const address = body as Address;
      address.id = ctx.address.id;

      ctx.setAddress(address);
      ctx.popupStateFunc(false);
    }
  };

  return (
    <form className="account-address-form" id="add-form">
      {Object.entries(JSON.parse(JSON.stringify(ctx.address)) as Address)
        .filter(([key]) => !key.toLowerCase().includes("id"))
        .map(([key, value]) => (
          <label className="account-address-form-label" key={key}>
            <span className="account-address-form-title">{key.charAt(0).toUpperCase() + key.slice(1)}</span>
            <input className="account-address-form-value" type="text" name={key} defaultValue={value} key={value} />
          </label>
        ))}
      <button className="account-address-form-button" onClick={handleAddressUpdate}>
        Save
      </button>
    </form>
  );
}
