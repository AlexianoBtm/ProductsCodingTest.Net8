import { API_BASE_URL } from "../config";
import { createApiError } from "./apiError";

function buildAuthHeaders(token) {
  return {
    "Content-Type": "application/json",
    Authorization: `Bearer ${token}`,
  };
}

export async function getProducts(token) {
  const response = await fetch(`${API_BASE_URL}/api/products`, {
    method: "GET",
    headers: buildAuthHeaders(token),
  });

  if (!response.ok) {
    throw await createApiError(response, "Failed to load products.");
  }

  return await response.json();
}

export async function getProductsByColour(token, colour) {
  const query = new URLSearchParams({ colour }).toString();

  const response = await fetch(`${API_BASE_URL}/api/products?${query}`, {
    method: "GET",
    headers: buildAuthHeaders(token),
  });

  if (!response.ok) {
    throw await createApiError(response, "Failed to filter products.");
  }

  return await response.json();
}

export async function createProduct(token, product) {
  const response = await fetch(`${API_BASE_URL}/api/products`, {
    method: "POST",
    headers: buildAuthHeaders(token),
    body: JSON.stringify(product),
  });

  if (!response.ok) {
    throw await createApiError(response, "Failed to create product.");
  }

  return await response.json();
}
