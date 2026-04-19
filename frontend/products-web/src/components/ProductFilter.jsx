import { useState } from "react";

function ProductFilter({ onFilter, onClear, isLoading }) {
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
    <div className="card">
      <h2>Filter by Colour</h2>

      <form onSubmit={handleSubmit} className="inline-form">
        <input
          type="text"
          value={colour}
          onChange={(event) => setColour(event.target.value)}
          placeholder="Enter a colour, e.g. Black"
        />

        <button type="submit" disabled={isLoading}>
          {isLoading ? "Filtering..." : "Filter"}
        </button>

        <button type="button" onClick={handleClear} disabled={isLoading}>
          Clear
        </button>
      </form>
    </div>
  );
}

export default ProductFilter;