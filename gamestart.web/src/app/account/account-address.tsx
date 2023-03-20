import Popup from "reactjs-popup";
import { Address } from "../util/types";
import AccountEditAddress from "./account-edit-address";
import { useState } from "react";
import ApiRouter from "../util/ApiRouter";

export default function AccountAddress(ctx: {
  address: Address;
  allAddresses: Address[];
  setAddresses: React.Dispatch<React.SetStateAction<Address[]>>;
}) {
  const [isEditPopupOpen, setIsEditPopupOpen] = useState(false);
  const [localAddress, setLocalAddress] = useState(ctx.address);

  const handleAddressDelete = async (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();

    const response = await ApiRouter.address.delete(localAddress.id);

    if (response.status === 204) {
      ctx.setAddresses(ctx.allAddresses.filter(({ id }) => id !== localAddress.id));
      setIsEditPopupOpen(false);
    }
  };

  return (
    <table className="account-address-card">
      <tbody>
        {Object.entries(localAddress)
          .filter(([key]) => key !== "id" && key !== "userId")
          .map(([key, value]) => (
            <tr className="account-address-card-row" key={localAddress.id + "row" + key}>
              <td className="account-address-card-label" key={key + localAddress.id}>
                {key.charAt(0).toUpperCase() + key.slice(1)}
              </td>
              <td className="account-address-card-value" key={value + localAddress.id}>
                {value}
              </td>
            </tr>
          ))}
      </tbody>
      <div className="account-address-card-buttons">
        <Popup
          arrow={false}
          open={isEditPopupOpen}
          onOpen={() => setIsEditPopupOpen(true)}
          onClose={() => setIsEditPopupOpen(false)}
          position="center center"
          trigger={<button className="account-address-card-edit">Edit</button>}>
          <AccountEditAddress address={localAddress} popupStateFunc={setIsEditPopupOpen} setAddress={setLocalAddress} />
        </Popup>
        <button className="account-address-card-delete" onClick={handleAddressDelete}>
          Delete
        </button>
      </div>
    </table>
  );
}
