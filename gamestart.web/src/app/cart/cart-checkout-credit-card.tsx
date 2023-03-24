export default function CartCheckoutCreditCard() {
  const formatCardNumber = (e: React.FormEvent<HTMLInputElement>) => {
    e.preventDefault();

    const value = e.currentTarget.value.substring(0, 19);
    let result = "";

    for (let i = 0; i < value.length; i++) {
      if (!Number.isNaN(Number.parseInt(value[i]))) {
        result += value[i];

        if (i === 3 || i === 8 || i === 13) {
          result += " ";
        }
      }
    }

    if (result[result.length - 1] === " ") result = result.substring(0, result.length - 1);

    e.currentTarget.value = result;
  };

  const formatCardDate = (e: React.FormEvent<HTMLInputElement>) => {
    e.preventDefault();

    let value = e.currentTarget.value.substring(0, 5);

    if (value.length >= 2 && !value.match("^(0[1-9]|1[0-2])")) value = "";
    if (value.length >= 2 && !value.includes("/")) value = `${value.slice(0, 2)}/${value.slice(2)}`;
    if (value.length > 0 && value[value.length - 1] === "/") value = value.substring(0, value.length - 1);
    if (value.length === 5 && !value.match("^(0[1-9]|1[0-2])/([0-9]{2})$")) value = value.substring(0, 3);

    e.currentTarget.value = value;
  };

  const formatCardHolder = (e: React.FormEvent<HTMLInputElement>) => {
    e.preventDefault();
    e.currentTarget.value = e.currentTarget.value.toLocaleUpperCase();
  };

  const formatCardCVV = (e: React.FormEvent<HTMLInputElement>) => {
    e.preventDefault();

    const value = e.currentTarget.value.substring(0, 3);
    let result = "";

    for (const c of value) {
      if (!Number.isNaN(Number.parseInt(c))) {
        result += c;
      }
    }

    e.currentTarget.value = result;
  };

  return (
    <form className="cart-checkout-credit-card">
      <div className="cart-checkout-credit-card-front">
        <label className="cart-checkout-credit-card-number">
          <span className="cart-checkout-credit-card-number-title">Card number</span>
          <input
            className="cart-checkout-credit-card-number-input"
            type="text"
            maxLength={19}
            placeholder="0000 0000 0000 0000"
            onInputCapture={formatCardNumber}
            required
          />
        </label>
        <label className="cart-checkout-credit-card-date">
          <span className="cart-checkout-credit-card-date-title">Valid until</span>
          <input className="cart-checkout-credit-card-date-input" type="text" placeholder="00/00" onInputCapture={formatCardDate} required />
        </label>
        <label className="cart-checkout-credit-card-holder">
          <span className="cart-checkout-credit-card-holder-title">Card holder</span>
          <input className="cart-checkout-credit-card-holder-input" type="text" placeholder="JON DOE" onInputCapture={formatCardHolder} required />
        </label>
      </div>
      <div className="cart-checkout-credit-card-back">
        <label className="cart-checkout-credit-card-cvv">
          <span className="cart-checkout-credit-card-cvv-title">CVC/CVV</span>
          <input className="cart-checkout-credit-card-cvv-input" type="text" placeholder="000" onInputCapture={formatCardCVV} required />
        </label>
      </div>
    </form>
  );
}
