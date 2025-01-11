const _apiUrl = "/api/auth";

export const login = (email, password) => {
  return fetch(_apiUrl + "/login", {
    method: "POST",
    credentials: "same-origin",
    headers: {
      Authorization: `Basic ${btoa(`${email}:${password}`)}`,
    },
  }).then((res) => {
    if (res.status !== 200) {
      return Promise.resolve(null);
    } else {
      return tryGetLoggedInUser();
    }
  });
};

export const logout = () => {
  return fetch(_apiUrl + "/logout");
};

export const tryGetLoggedInUser = () => {
  return fetch("/api/auth/me", {
    method: "GET",
    credentials: "include", // ✅ This sends the auth cookie
  })
    .then((res) => {
      if (res.status === 401) {
        return Promise.resolve(null);
      }
      if (!res.ok) {
        return res.text().then((text) => {
          console.error("Error fetching user:", text);
          return Promise.resolve(null);
        });
      }
      return res.json();
    })
    .catch((err) => {
      console.error("Error fetching logged-in user:", err);
      return Promise.resolve(null);
    });
};

export const register = (userProfile) => {
  userProfile.password = btoa(userProfile.password);
  return fetch(_apiUrl + "/register", {
    credentials: "same-origin",
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(userProfile),
  }).then(() => tryGetLoggedInUser());
};
