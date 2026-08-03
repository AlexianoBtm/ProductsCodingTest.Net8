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

  function handleLogout(message = "Logged out.") {
    clearToken();
    setProducts([]);
    setProductsMessage(message);
  }

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
      if (error.status === 401) {
        handleLogout("Your demo session expired. Sign in again.");
        return;
      }

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
      if (error.status === 401) {
        handleLogout("Your demo session expired. Sign in again.");
        return;
      }

      setProductsMessage(error.message || "Failed to filter products.");
    } finally {
      setIsLoadingProducts(false);
    }
  }

  async function handleProductCreated() {
    await loadProducts();
  }

  return (
    <main className="app-container">
      <header className="hero">
        <div>
          <p className="eyebrow">PUBLIC TECHNICAL SAMPLE</p>
          <h1 className="app-title">Layered Products API</h1>
          <p className="app-subtitle">
            A focused .NET 8 and React sample for authenticated product workflows.
          </p>
        </div>
        <div className="tech-stack" aria-label="Technology stack">
          <span>.NET 8</span>
          <span>React</span>
          <span>SQLite</span>
          <span>JWT</span>
        </div>
      </header>

      <section className="status-grid" aria-label="System status and demo access">
        <HealthStatus />
        <LoginForm onLoginSuccess={setToken} />
      </section>

      <section className="card session-card">
        <div className="top-bar">
          <div>
            <h2>Session</h2>
            <p className="muted">
              {isAuthenticated
                ? "Demo session authenticated"
                : "Sign in with credentials configured on the local API"}
            </p>
          </div>

          <div className="button-row">
            <button
              className="button button-primary"
              onClick={loadProducts}
              disabled={isLoadingProducts || !isAuthenticated}
            >
              {isLoadingProducts ? "Loading..." : "Load Products"}
            </button>
            <button
              className="button button-secondary"
              onClick={() => handleLogout()}
              disabled={!isAuthenticated}
            >
              Logout
            </button>
          </div>
        </div>

        {productsMessage && <div className="message">{productsMessage}</div>}
      </section>

      <section className="workspace-grid" aria-label="Product workspace">
        <ProductFilter
          onFilter={filterProducts}
          onClear={loadProducts}
          isLoading={isLoadingProducts}
          isAuthenticated={isAuthenticated}
        />

        <CreateProductForm
          token={token}
          onProductCreated={handleProductCreated}
          onUnauthorized={() =>
            handleLogout("Your demo session expired. Sign in again.")
          }
        />
      </section>

      <ProductList products={products} />
    </main>
  );
}

export default App;
