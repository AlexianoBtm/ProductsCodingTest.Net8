function ProductList({ products }) {
  return (
    <section className="card products-section">
      <div className="section-heading">
        <div>
          <p className="eyebrow">LOCAL DATA</p>
          <h2>Products</h2>
        </div>
        <span className="count-badge">{products.length}</span>
      </div>

      {products.length === 0 ? (
        <p className="muted">No products found.</p>
      ) : (
        <div className="products-grid">
          {products.map((product) => (
            <article key={product.id} className="product-item">
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
            </article>
          ))}
        </div>
      )}
    </section>
  );
}

export default ProductList;
