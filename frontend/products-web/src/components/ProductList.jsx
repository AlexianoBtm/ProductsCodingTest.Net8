function ProductList({ products }) {
  return (
    <div className="card">
      <h2>Products</h2>

      {products.length === 0 ? (
        <p className="muted">No products found.</p>
      ) : (
        <div className="products-grid">
          {products.map((product) => (
            <div key={product.id} className="product-item">
              <h3>{product.name}</h3>
              <p>{product.description || "No description provided."}</p>
              <p>
                <strong>Colour:</strong> {product.colour}
              </p>
              <p>
                <strong>Price:</strong> ${Number(product.price).toFixed(2)}
              </p>
              <p className="muted">
                Created: {new Date(product.createdAtUtc).toLocaleString()}
              </p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default ProductList;