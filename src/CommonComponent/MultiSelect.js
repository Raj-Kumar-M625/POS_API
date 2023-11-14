import React from "react";
import Form from "react-bootstrap/Form";
import { Multiselect } from "multiselect-react-dropdown";

function MultiSelect({ qn,inputChangeHandler }) {
  return (
    <div key={qn.QText} className="col-md-6">
      <Form.Label htmlFor="">{qn.QText}</Form.Label>
      <Form.Control
        as= "Multiselect"
        onChange={inputChangeHandler}
        name={qn.QText}
        >
      <Multiselect
        isObject={false}
        options={qn.AnswerChoices}
        showCheckbox={true}
        onChange={(e) =>inputChangeHandler(e)}
     
       
      />
      </Form.Control>
      
    </div>
  );
}

export default MultiSelect;
