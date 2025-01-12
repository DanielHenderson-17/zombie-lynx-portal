import { useEffect, useState } from "react";
import {
  getPackages,
  createBasket,
  addPackageToBasket,
} from "../../tebexService";

export default function Shop() {
  const [packages, setPackages] = useState([]);
  const [basketId, setBasketId] = useState(null);

  useEffect(() => {
    const fetchPackages = async () => {
      try {
        const data = await getPackages();
        setPackages(data);
      } catch (error) {
        console.error("Failed to fetch packages:", error);
      }
    };

    fetchPackages();
  }, []);

  const handleAddToBasket = async (packageId) => {
    try {
      if (!basketId) {
        const basket = await createBasket(
          window.location.href,
          window.location.href
        );
        setBasketId(basket.id);
      }
      await addPackageToBasket(basketId, packageId);
    } catch (error) {
      console.error("Failed to add package to basket:", error);
    }
  };

  const handleCheckout = () => {
    if (basketId) {
      window.Tebex.checkout.init({
        ident: basketId,
      });
      window.Tebex.checkout.launch();
    } else {
      console.error("No basket created for checkout.");
    }
  };

  return (
    <div>
      <h1>Shop</h1>
      <ul>
        {packages.map((pkg) => (
          <li key={pkg.id}>
            <h2>{pkg.name}</h2>
            <p>{pkg.description}</p>
            <button onClick={() => handleAddToBasket(pkg.id)}>
              Add to Basket
            </button>
          </li>
        ))}
      </ul>
      <button onClick={handleCheckout} disabled={!basketId}>
        Proceed to Checkout
      </button>
    </div>
  );
}
