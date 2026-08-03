import { useState } from "react";
import { login } from "../api/authApi";

function LoginForm({ onLoginSuccess }) {
  const [username, setUsername] = useState("demo");
  const [password, setPassword] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  async function handleSubmit(event) {
    event.preventDefault();
    setErrorMessage("");
    setSuccessMessage("");
    setIsLoading(true);

    try {
      const result = await login(username, password);
      onLoginSuccess(result.token);
      setSuccessMessage("Login successful.");
      setPassword("");
    } catch (error) {
      setErrorMessage(error.message || "Login failed.");
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <div className="card status-card">
      <h2>Demo Access</h2>
      <p className="muted">
        Credentials are configured locally and are not stored in this repository.
      </p>
      <form onSubmit={handleSubmit}>
        <div className="form-row">
          <label htmlFor="username">Username</label>
          <input
            id="username"
            value={username}
            onChange={(event) => setUsername(event.target.value)}
            placeholder="Enter username"
            autoComplete="username"
            required
          />
        </div>

        <div className="form-row">
          <label htmlFor="password">Password</label>
          <input
            id="password"
            type="password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            placeholder="Enter password"
            autoComplete="current-password"
            required
          />
        </div>

        <div className="button-row">
          <button className="button button-primary" type="submit" disabled={isLoading}>
            {isLoading ? "Signing in..." : "Login"}
          </button>
        </div>

        {errorMessage && <div className="message error">{errorMessage}</div>}
        {successMessage && <div className="message success">{successMessage}</div>}
      </form>
    </div>
  );
}

export default LoginForm;
