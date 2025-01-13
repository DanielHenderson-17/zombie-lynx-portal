const _authUrl = "/api/SteamAuth";

export const getAllSteamUsers = () => {
  return fetch(`${_authUrl}/all-steam-users`)
    .then((res) => {
      if (!res.ok) {
        throw new Error("Failed to fetch Steam users.");
      }
      return res.json();
    })
    .catch((error) => {
      console.error("Error fetching all Steam users:", error);
      throw error;
    });
};

export const linkSteamAccount = (onWindowClose) => {
  const steamWindow = window.open(
    "/api/SteamAuth/login",
    "Steam Login",
    "width=600,height=800"
  );

  const handleMessage = (event) => {
    if (event.data === "steamLinked") {
      getLinkedSteamAccount().finally(() => {
        window.removeEventListener("message", handleMessage);
      });
    }
  };

  window.addEventListener("message", handleMessage);

  const checkWindowClosed = setInterval(() => {
    if (steamWindow.closed) {
      clearInterval(checkWindowClosed);
      window.removeEventListener("message", handleMessage);
      onWindowClose && onWindowClose();
    }
  }, 500);
};

export const unlinkSteamAccount = (identityUserId) => {
  return fetch(`${_authUrl}/unlink`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify({ identityUserId }),
  }).then((res) => {
    if (!res.ok) throw new Error("Failed to unlink Steam account.");
    return res.text();
  });
};

export const getLinkedSteamAccount = () => {
  return new Promise((resolve) => setTimeout(resolve, 500)).then(() =>
    fetch(`/api/SteamAuth/linked?cacheBust=${Date.now()}`, {
      method: "GET",
      credentials: "include",
    }).then((res) => {
      if (!res.ok) throw new Error(`Failed to fetch linked Steam account. Status: ${res.status}`);
      return res.json();
    })
  );
};

export const steamLogin = () => {
  window.location.href = `${_authUrl}/login`;
};

export const steamLogout = () => {
  return fetch(`${_authUrl}/logout`, {
    method: "GET",
    credentials: "include",
  }).then((res) => {
    if (!res.ok) throw new Error("Failed to logout.");
  });
};

export const checkSteamAuth = () => {
  return fetch(`${_authUrl}/ping`)
    .then((res) => {
      if (!res.ok) throw new Error("Failed to check Steam authentication.");
      return res.text();
    })
    .catch(() => {});
};
