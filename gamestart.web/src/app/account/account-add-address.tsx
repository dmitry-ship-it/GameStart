import Popup from "reactjs-popup";
import AccountAddAddressContent from "./account-add-address-content";
import { useState } from "react";
import { Address } from "../util/types";

export default function AccountAddAddress(ctx: { allAddresses: Address[]; setAllAddresses: React.Dispatch<React.SetStateAction<Address[]>> }) {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <Popup
      open={isOpen}
      onOpen={() => setIsOpen(true)}
      onClose={() => setIsOpen(false)}
      arrow={false}
      trigger={<button className="account-address-form-button">Add new address</button>}
      position="center center">
      <AccountAddAddressContent isOpenPopupFunc={setIsOpen} allAddresses={ctx.allAddresses} setAllAddresses={ctx.setAllAddresses} />
    </Popup>
  );
}
