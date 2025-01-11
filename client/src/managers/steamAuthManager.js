const _authUrl = "/api/SteamAuth";

export const steamLogin = () => {
  window.location.href = `${_authUrl}/login`;
};

export const steamLogout = () => {
  return fetch(`${_authUrl}/logout`, {
    method: "GET",
    credentials: "include",
  }).then((res) => {
    if (!res.ok) {
      throw new Error("Failed to logout.");
    }
  });
};

export const checkSteamAuth = () => {
  return fetch(`${_authUrl}/ping`)
    .then((res) => {
      if (!res.ok) {
        throw new Error("Failed to check Steam authentication.");
      }
      return res.text();
    })
    .catch((err) => {
      console.error("Error checking Steam authentication:", err);
    });
};
