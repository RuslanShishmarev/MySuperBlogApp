import React from 'react';

export const Counter = () => {

  const [currentCount, setCurrentCount] = React.useState(0);

  return (
    <div>
      <h1>Func Counter</h1>

      <p>This is a simple example of a React component.</p>

      <p aria-live="polite">Current count: <strong>{currentCount}</strong></p>

      <button className="btn btn-primary" onClick={() => setCurrentCount(currentCount + 1)}>Increment</button>
    </div>
  );
}