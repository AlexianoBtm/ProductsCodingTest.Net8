import { useState } from "react";
import { createProduct } from "../api/productsApi";

function CreateProductForm({ token, onProductCreated }) {
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    colour: "",
    price: "",
  });

  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  function handleChange(event) {
    const { name, value } = event.target;

    setFormData((current) => ({
      ...current,
      [name]: value,
    }));
  }

  async function handleSubmit(event) {
    event.preventDefault();
    setErrorMessage("");
    setSuccessMessage("");

    if (!token) {
      setErrorMessage("You must login before creating a product.");
      return;
    }

    setIsSubmitting(true);

    try {
      const payload = {
        name: formData.name,
        description: formData.description,
        colour: formData.colour,
        price: Number(formData.price),
      };

      await createProduct(token, payload);

      setSuccessMessage("Product created successfully.");
      setFormData({
        name: "",
        description: "",
        colour: "",
        price: "",
      });

      onProductCreated();
    } catch (error) {
      setErrorMessage(error.message || "Failed to create product.");
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <div className="card">
      <h2>Create Product</h2>

      <form onSubmit={handleSubmit}>
        <div className="form-row">
          <label htmlFor="name">Name</label>
          <input
            id="name"
            name="name"
            value={formData.name}
            onChange={handleChange}
            placeholder="Product name"
          />
        </div>

        <div className="form-row">
          <label htmlFor="description">Description</label>
          <input
            id="description"
            name="description"
            value={formData.description}
            onChange={handleChange}
            placeholder="Product description"
          />
        </div>

        <div className="form-row">
          <label htmlFor="colour">Colour</label>
          <input
            id="colour"
            name="colour"
            value={formData.colour}
            onChange={handleChange}
            placeholder="Product colour"
          />
        </div>

        <div className="form-row">
          <label htmlFor="price">Price</label>
          <input
            id="price"
            name="price"
            type="number"
            step="0.01"
            value={formData.price}
            onChange={handleChange}
            placeholder="0.00"
          />
        </div>

        <div className="button-row">
          <button type="submit" disabled={isSubmitting}>
            {isSubmitting ? "Creating..." : "Create Product"}
          </button>
        </div>

        {errorMessage && <div className="message error">{errorMessage}</div>}
        {successMessage && <div className="message success">{successMessage}</div>}
      </form>
    </div>
  );
}

export default CreateProductForm;