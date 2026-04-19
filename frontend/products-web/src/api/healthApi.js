import { API_BASE_URL } from "../config";

export async function getHealthStatus() {
  const response = await fetch(`${API_BASE_URL}/health`);

  if (!response.ok) {
    throw new Error("Failed to fetch health status.");
  }

  return await response.json();
}