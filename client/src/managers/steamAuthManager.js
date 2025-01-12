const _authUrl = "/api/SteamAuth";

// ✅ Opens a popup for Steam account linking and logs the Steam data
export const linkSteamAccount = () => {
  console.log("🟢 User clicked the 'Link Steam' button.");

  const steamWindow = window.open(
    "/api/SteamAuth/login",
    "Steam Login",
    "width=600,height=800"
  );

  const checkWindowClosed = setInterval(() => {
    if (steamWindow.closed) {
      clearInterval(checkWindowClosed);
      console.log("🟡 User completed Steam login and popup closed.");

      // ✅ Fetch updated Steam data and log it
      getLinkedSteamAccount()
        .then((data) => {
          console.log("🟢 Steam account successfully linked:", data);  // LOG HERE
        })
        .catch((err) =>
          console.error("🔴 Failed to fetch Steam data after linking:", err)
        );
    }
  }, 500);
};

// ✅ Fetch linked Steam account data
export const getLinkedSteamAccount = () => {
  return fetch(`${_authUrl}/linked`, {
    method: "GET",
    credentials: "include",
  }).then((res) => {
    if (!res.ok) {
      throw new Error("Failed to fetch linked Steam account.");
    }
    return res.json();
  });
};

// Existing Steam login (unchanged)
export const steamLogin = () => {
  window.location.href = `${_authUrl}/login`;
};

// Existing Steam logout (unchanged)
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

// Existing Steam authentication check (unchanged)
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
