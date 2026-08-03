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
    let isCurrent = true;

    getHealthStatus()
      .then((result) => {
        if (isCurrent) {
          setStatus(result.status || "Unknown");
        }
      })
      .catch((error) => {
        if (isCurrent) {
          setStatus("Unavailable");
          setErrorMessage(error.message || "Health check failed.");
        }
      });

    return () => {
      isCurrent = false;
    };
  }, []);

  return (
    <div className="card status-card">
      <div className="top-bar">
        <div>
          <h2>Health Status</h2>
          <p className="muted">Backend availability check.</p>
        </div>

        <button className="button button-secondary" onClick={loadHealth}>Refresh</button>
      </div>

      <p className="health-value">
        <span className={`status-dot status-${status.toLowerCase()}`} aria-hidden="true" />
        <strong>{status}</strong>
      </p>

      {errorMessage && <div className="message error">{errorMessage}</div>}
    </div>
  );
}

export default HealthStatus;
