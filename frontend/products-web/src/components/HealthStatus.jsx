import { useEffect, useState } from "react";
import { getHealthStatus } from "../api/healthApi";

function HealthStatus() {
  const [status, setStatus] = useState("Checking...");
  const [errorMessage, setErrorMessage] = useState("");

  async function loadHealth() {
    setErrorMessage("");

    try {
      const result = await getHealthStatus();
      setStatus(result.status || "Unknown");
    } catch (error) {
      setStatus("Unavailable");
      setErrorMessage(error.message || "Health check failed.");
    }
  }

  useEffect(() => {
    loadHealth();
  }, []);

  return (
    <div className="card">
      <div className="top-bar">
        <div>
          <h2>Health Status</h2>
          <p className="muted">Backend availability check.</p>
        </div>

        <button onClick={loadHealth}>Refresh</button>
      </div>

      <p>
        <strong>Status:</strong> {status}
      </p>

      {errorMessage && <div className="message error">{errorMessage}</div>}
    </div>
  );
}

export default HealthStatus;