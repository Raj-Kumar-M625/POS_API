import React from "react";

function TextError(props) {
  return (
    <div
      style={{
        color: "#dc3545",
        fontSize: "0.875em",
      }}
      className="error"
    >
      {props.children}
    </div>
  );
}

export default TextError;
