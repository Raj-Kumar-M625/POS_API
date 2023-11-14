import React, { useState } from "react";
import { FormInputText } from "../../CommonComponent/FormInputText";
import { FormInputSelect } from "../../CommonComponent/FormInputSelect";
import "../../CSS/Views/SocialSecurity.css";
import { useNavigate } from "react-router-dom";
import Title from "../../CommonComponent/Title";
import BottomNavigation from "../BottomNavigation";
import Questiones from "../../CommonComponent/Questiones";
import socialSecurity from "../../Questions/SocialSecurity";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";

export const SocialSecurity = () => {
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
    <NavbarTitle data={{ Title: "Social Security Details" }}></NavbarTitle>
      <div className="container">
        
        <div className="container d-flex mx-auto justify-content-center">
          <h5 className="m-3">Surveyor Id :C202201103</h5>
          <h5 className="m-3">Surveyor Name : Pradeep</h5>
        </div>
        
        <Questiones
          api={socialSecurity.Questions}
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

{
  /* <div>
<div>
  {JSON.Questions.map((q) => {
    if (q.QuestionTypeName === "Descriptive") {
      return (
        <form id="contactform">
          <div class="formcolumn" key={q.QText}>
            <label>
              {q.DisplaySequence}. {q.QText}
            </label>
            <FormInputText />
          </div>
        </form>
      );
    } else if (q.QuestionTypeName === "Single Select") {
      return (
        <form id="contactform">
          <div class="formcolumn" key={q.QText}>
            <label>
              {q.DisplaySequence}. {q.QText}
            </label>
            <FormInputSelect>
              {q.AnswerChoices.map((e) => {
                return { e };
              })}
            </FormInputSelect>
          </div>
        </form>
      );
    }
  })}
</div>
</div>
<div
style={{ width: "100%", display: "flex", justifyContent: "flex-end" }}
>
<div>
  <button onClick={() => navigate(-1)} class="button">
    Back
  </button>
</div>
<div>
  <button class="button">Save and Continue</button>
</div>
</div> */
}
