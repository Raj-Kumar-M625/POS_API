import React from "react";
import Form from "react-bootstrap/Form";

function Descriptive({ qn, inputChangeHandler }) {
  return (
    <div className="col-md-6" key={qn.QText}>
      <Form.Label>{qn.QText}</Form.Label>
      {qn.CodeName === "DOB" ? (
        <Form.Control
          name={qn.CodeName}
          type="date"
          onChange={(e) => inputChangeHandler(e)}
        />
      ) : (
        <Form.Control
          name={qn.CodeName}
          type="text"
          onChange={(e) => inputChangeHandler(e)}
        />
      )}
    </div>
  );
}

export default Descriptive;
