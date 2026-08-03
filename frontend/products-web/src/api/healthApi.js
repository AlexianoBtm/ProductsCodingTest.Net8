import { API_BASE_URL } from "../config";
import { createApiError } from "./apiError";

export async function getHealthStatus() {
  const response = await fetch(`${API_BASE_URL}/health`);

  if (!response.ok) {
    throw await createApiError(response, "Failed to fetch health status.");
  }

  return await response.json();
}
