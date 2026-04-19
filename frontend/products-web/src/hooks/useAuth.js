import { useMemo, useState } from "react";

const TOKEN_STORAGE_KEY = "products_jwt_token";

export function useAuth() {
  const [token, setTokenState] = useState(() => {
    return localStorage.getItem(TOKEN_STORAGE_KEY) || "";
  });

  function setToken(newToken) {
    setTokenState(newToken);
    localStorage.setItem(TOKEN_STORAGE_KEY, newToken);
  }

  function clearToken() {
    setTokenState("");
    localStorage.removeItem(TOKEN_STORAGE_KEY);
  }

  const isAuthenticated = useMemo(() => token.trim().length > 0, [token]);

  return {
    token,
    setToken,
    clearToken,
    isAuthenticated,
  };
}