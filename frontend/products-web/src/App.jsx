import { useState } from "react";
import LoginForm from "./components/LoginForm";
import HealthStatus from "./components/HealthStatus";
import ProductList from "./components/ProductList";
import ProductFilter from "./components/ProductFilter";
import CreateProductForm from "./components/CreateProductForm";
import { useAuth } from "./hooks/useAuth";
import { getProducts, getProductsByColour } from "./api/productsApi";

function App() {
  const { token, setToken, clearToken, isAuthenticated } = useAuth();
  const [products, setProducts] = useState([]);
  const [productsMessage, setProductsMessage] = useState("");
  const [isLoadingProducts, setIsLoadingProducts] = useState(false);

  async function loadProducts() {
    if (!token) {
      setProductsMessage("You must login first.");
      return;
    }

    setIsLoadingProducts(true);
    setProductsMessage("");

    try {
      const result = await getProducts(token);
      setProducts(result);
      setProductsMessage(`Loaded ${result.length} product(s).`);
    } catch (error) {
      setProductsMessage(error.message || "Failed to load products.");
    } finally {
      setIsLoadingProducts(false);
    }
  }

  async function filterProducts(colour) {
    if (!token) {
      setProductsMessage("You must login first.");
      return;
    }

    if (!colour.trim()) {
      await loadProducts();
      return;
    }

    setIsLoadingProducts(true);
    setProductsMessage("");

    try {
      const result = await getProductsByColour(token, colour);
      setProducts(result);
      setProductsMessage(`Found ${result.length} product(s) for colour "${colour}".`);
    } catch (error) {
      setProductsMessage(error.message || "Failed to filter products.");
    } finally {
      setIsLoadingProducts(false);
    }
  }

  async function handleProductCreated() {
    await loadProducts();
  }

  function handleLogout() {
    clearToken();
    setProducts([]);
    setProductsMessage("Logged out.");
  }

  return (
    <div className="app-container">
      <h1 className="app-title">Products Coding Test</h1>
      <p className="app-subtitle">
        React frontend connected to the .NET Products API.
      </p>

      <HealthStatus />

      <LoginForm onLoginSuccess={setToken} />

      <div className="card">
        <div className="top-bar">
          <div>
            <h2>Session</h2>
            <p className="muted">
              {isAuthenticated ? "Authenticated" : "Not authenticated"}
            </p>
          </div>

          <div className="button-row">
            <button onClick={loadProducts} disabled={isLoadingProducts}>
              {isLoadingProducts ? "Loading..." : "Load Products"}
            </button>
            <button onClick={handleLogout}>Logout</button>
          </div>
        </div>

        {productsMessage && <div className="message">{productsMessage}</div>}
      </div>

      <ProductFilter
        onFilter={filterProducts}
        onClear={loadProducts}
        isLoading={isLoadingProducts}
      />

      <CreateProductForm
        token={token}
        onProductCreated={handleProductCreated}
      />

      <ProductList products={products} />
    </div>
  );
}

export default App;