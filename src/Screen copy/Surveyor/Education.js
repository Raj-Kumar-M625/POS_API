import React, { useState } from "react";
import { Form } from "react-bootstrap";
import { FormInputSelect } from "../../CommonComponent/InputSelect";
import { FormInput } from "../../CommonComponent/Input";
import "../../CSS/Views/education.css";
import BottomNavigation from "../BottomNavigation";
import Title from "../../CommonComponent/Title";
import Questiones from "../../CommonComponent/Questiones";
import education from "../../Questions/Education";
import { useNavigate } from "react-router-dom";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import { http } from "../../_services/http.service";

export const Education = () => {
  const [responseBody, setresponseBody] = useState({});
  const navigate = useNavigate();

  function inputChangeHandler(event) {
    const { name, value } = event.target;
    setresponseBody({ ...responseBody, [name]: value });
  }

  function onSubmitHandler() {
    console.log("Submitted Successfully");
    console.log(responseBody);
    navigate("/Surveyor/SurveyDetails");
  }

  function backNavigator() {
    navigate("/Surveyor/SurveyDetails");
  }

  return (
    <>
    <NavbarTitle data={{ Title: "Education Details" }}></NavbarTitle>
      <div className="container">
      
        <div className="container d-flex mx-auto justify-content-center">
          <h5 className="m-3">Surveyor Id :{http.GetData().SurveyId}</h5>
          <h5 className="m-3">Surveyor Name : {http.GetData().Name}</h5>
        </div>
        
        <Questiones
          api={education.Questions}
          inputChangeHandler={inputChangeHandler}
        />
        <BottomNavigation
          onSubmitHandler={onSubmitHandler}
          backNavigator={backNavigator}
        />
      </div>
    </>
  );
};

export default Education;

{
  /* <form>
<meta
  name="viewport"
  content="width=device-width, initial-scale=1.0"
></meta>
<div>
  <h4>Education</h4>
</div>
<div class="edu">
  {api.Questions.map((q) => {
    if (q.QuestionTypeName === "Descriptive") {
      return (
        <Form>
          <div key={q.QText} class="dev">
            <FormInput QText={q.QText} />
          </div>
        </Form>
      );
    } else if (q.QuestionTypeName === "Single Select") {
      return (
        <div key={q.QText}>
          <FormInputSelect QText={q.QText} val={q.AnswerChoices} />
        </div>
      );
    }
  })}
</div>
{/* <input type="button" class="button1" value="Save" />
      <input type="button" class="button2" value="Back" /> */
}
{
  /* <div style={{ display: "flex", justifyContent: "flex-end" }}>
  <button class="button1">Back</button>
  <button class="button1">Save and Continue</button>
</div> */
}
// </form> */}
