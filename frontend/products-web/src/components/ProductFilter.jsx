import { useState } from "react";

function ProductFilter({ onFilter, onClear, isLoading, isAuthenticated }) {
  const [colour, setColour] = useState("");

  function handleSubmit(event) {
    event.preventDefault();
    onFilter(colour);
  }

  function handleClear() {
    setColour("");
    onClear();
  }

  return (
    <div className="card workspace-card compact-card">
      <h2>Filter by Colour</h2>

      <form onSubmit={handleSubmit} className="inline-form">
        <input
          type="text"
          value={colour}
          onChange={(event) => setColour(event.target.value)}
          placeholder="Enter a colour, e.g. Black"
        />

        <button
          className="button button-primary"
          type="submit"
          disabled={isLoading || !isAuthenticated}
        >
          {isLoading ? "Filtering..." : "Filter"}
        </button>

        <button
          className="button button-secondary"
          type="button"
          onClick={handleClear}
          disabled={isLoading || !isAuthenticated}
        >
          Clear
        </button>
      </form>
    </div>
  );
}

export default ProductFilter;
