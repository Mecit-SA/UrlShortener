export async function handle400ErrorResponse(response: any): Promise<string> {
  try {
    let errors: any;

    if (typeof response.error === 'string') {
      // If the error is a string, try parsing it as JSON
      errors = JSON.parse(response.error).errors;
    } else if (response.error instanceof Blob) {
      // If the error is a Blob, read and parse it as JSON
      const text = await response.error.text();
      errors = JSON.parse(text).errors;
    } else if (response.error && typeof response.error === 'object') {
      // If it's already an object, extract errors directly
      errors = response.error.errors;
    }

    if (!errors || typeof errors !== 'object') {
      return "An unknown error occurred.";
    }

    // Extract and format errors
    return Object.entries(errors)
      .map(([key, messages]) => {
        const messageList = Array.isArray(messages) ? messages : [messages];
        return `${key}: ${messageList.join(" | ")}`;
      })
      .join(" | ");
  } catch (e) {
    return "An error occurred while parsing the validation errors. " + e.message;
  }
}
