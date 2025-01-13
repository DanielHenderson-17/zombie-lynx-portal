import axios from "axios";

const PROXY_URL = "http://localhost:8080/";
const API_BASE_URL = `${PROXY_URL}https://plugin.tebex.io`;
const PUBLIC_KEY = import.meta.env.VITE_TEBEX_PUBLIC_KEY;

const tebexService = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    "X-Tebex-Secret": PUBLIC_KEY,
    "X-Requested-With": "XMLHttpRequest"
  },
});

export const getPackages = async () => {
  try {
    const response = await tebexService.get("/packages");
    return response.data;
  } catch (error) {
    console.error("Error fetching packages:", error);
    throw error;
  }
};

export const createBasket = async (completeUrl, cancelUrl) => {
  try {
    const response = await tebexService.post("/baskets", {
      complete_url: completeUrl,
      cancel_url: cancelUrl,
    });
    return response.data;
  } catch (error) {
    console.error("Error creating basket:", error);
    throw error;
  }
};

export const addPackageToBasket = async (basketId, packageId, quantity = 1) => {
  try {
    const response = await tebexService.post(`/baskets/${basketId}/packages`, {
      package_id: packageId,
      quantity,
    });
    return response.data;
  } catch (error) {
    console.error("Error adding package to basket:", error);
    throw error;
  }
};
