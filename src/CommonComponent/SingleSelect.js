import React from "react";
import { Form } from "react-bootstrap";
import Select from "./Select";

function SingleSelect({ qn, inputChangeHandler }) {
  return (
    <div className="col-md-6" key={qn.QText}>
      <Form.Label htmlFor="">{qn.QText}</Form.Label>
      <Select
        options={qn.AnswerChoices}
        className="col-md-6"
        name={qn.QuestionId}
        inputChangeHandler={inputChangeHandler}
      />
    </div>
  );
}

export default SingleSelect;
