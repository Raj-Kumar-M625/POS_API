import React from "react";
import { Button, Form } from "react-bootstrap";
import { Multiselect } from "multiselect-react-dropdown";
import Select from "../../CommonComponent/Select";
import PersonalDetails from "../../Questions/PersonalDetails";

function AadharDetails() {
  return (
    <div>
      <div className="container row g-3">
        {PersonalDetails.QuestionsPage1.map((qn) => {
          if (qn.QuestionTypeName === "Descriptive") {
            return (
              <div key={qn.QText} className="col-md-6">
                <Form.Label>{qn.QText}</Form.Label>
                <Form.Control type="text" />
              </div>
            );
          } else if (qn.QuestionTypeName === "Single Select") {
            return (
              <div key={qn.QText} className="col-md-6">
                <Form.Label htmlFor="">{qn.QText}</Form.Label>
                <Select options={qn.AnswerChoices} />
                <br />
              </div>
            );
          } else if (qn.QuestionTypeName === "MultiSelect") {
            return (
              <div key={qn.QText} className="col-md-6">
                <Form.Label htmlFor="">{qn.QText}</Form.Label>
                <Multiselect
                  isObject={false}
                  options={qn.AnswerChoices}
                  showCheckbox={true}
                />
                <br />
              </div>
            );
          }
        })}
      </div>
    </div>
  );
}

export default AadharDetails;
