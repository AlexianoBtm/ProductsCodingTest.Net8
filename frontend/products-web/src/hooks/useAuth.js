import { useMemo, useState } from "react";

const TOKEN_STORAGE_KEY = "products_demo_jwt";

function isTokenCurrent(token) {
  if (!token) {
    return false;
  }

  try {
    const [, encodedPayload] = token.split(".");
    const normalizedPayload = encodedPayload
      .replace(/-/g, "+")
      .replace(/_/g, "/")
      .padEnd(Math.ceil(encodedPayload.length / 4) * 4, "=");
    const payload = JSON.parse(atob(normalizedPayload));

    return typeof payload.exp === "number" && payload.exp * 1000 > Date.now();
  } catch {
    return false;
  }
}

export function useAuth() {
  const [token, setTokenState] = useState(() => {
    const storedToken = sessionStorage.getItem(TOKEN_STORAGE_KEY) || "";

    if (isTokenCurrent(storedToken)) {
      return storedToken;
    }

    sessionStorage.removeItem(TOKEN_STORAGE_KEY);
    return "";
  });

  function setToken(newToken) {
    if (!isTokenCurrent(newToken)) {
      clearToken();
      return;
    }

    setTokenState(newToken);
    sessionStorage.setItem(TOKEN_STORAGE_KEY, newToken);
  }

  function clearToken() {
    setTokenState("");
    sessionStorage.removeItem(TOKEN_STORAGE_KEY);
  }

  const isAuthenticated = useMemo(() => isTokenCurrent(token), [token]);

  return {
    token,
    setToken,
    clearToken,
    isAuthenticated,
  };
}
