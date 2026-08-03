export class ApiRequestError extends Error {
  constructor(message, status) {
    super(message);
    this.name = "ApiRequestError";
    this.status = status;
  }
}

export async function createApiError(response, fallbackMessage) {
  let message = fallbackMessage;

  try {
    const contentType = response.headers.get("content-type") || "";

    if (contentType.includes("application/json")) {
      const body = await response.json();
      message = body.detail || body.title || body.error || fallbackMessage;
    }
  } catch (error) {
    console.warn("The API error response could not be parsed.", error);
  }

  return new ApiRequestError(message, response.status);
}
