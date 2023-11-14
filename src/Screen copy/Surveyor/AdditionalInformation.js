import React, { useState } from "react";
import "../../CSS/Views/Info.css";
import BottomNavigation from "../BottomNavigation";
import Title from "../../CommonComponent/Title";
import Questiones from "../../CommonComponent/Questiones";
import additionalInformation from "../../Questions/AdditionalInformation";
import { useNavigate } from "react-router-dom";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import { http } from "../../_services/http.service";

export const Additionalinformation = () => {
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
      <NavbarTitle data={{ Title: "Additional Details" }}></NavbarTitle>
      <div className="container">
        <div className="container d-flex mx-auto justify-content-center">
          <h5 className="m-3">Surveyor Id :{http.GetData().SurveyId}</h5>
          <h5 className="m-3">Surveyor Name : {http.GetData().Name}</h5>
        </div>

        <Questiones
          api={additionalInformation.Questions}
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

export default Additionalinformation;

// {/* <form>
// <meta
//   name="viewport"
//   content="width=device-width, initial-scale=1.0"
// ></meta>
// <div>
//   <h4>Additional Information</h4>
// </div>
// <div class="info">
//   {api.Questions.map((q) => {
//     if (q.QuestionTypeName === "Descriptive") {
//       return (
//         <div key={q.QText}>
//           <FormInput QText={q.QText} />
//         </div>
//       );
//     } else if (q.QuestionTypeName === "Single Select") {
//       return (
//         <div key={q.QText}>
//           {/* <label>{q.QText}</label>
//                   <select className='form-select' >
//                       {q.AnswerChoices.map((e) => {
//                           return <option>{e}</option>
//                       })}
//                   </select> */}

//           <FormInputSelect QText={q.QText} val={q.AnswerChoices} />
//         </div>
//       );
//     }
//   })}
// </div>
// {/* <input type="button" class="button1" value="Save" />
//       <input type="button" class="button2" value="Back" /> */}
// <div style={{ display: "flex", justifyContent: "flex-end" }}>
//   <button class="button1">Back</button>
//   <button class="button1">Save and Continue</button>
// </div>
// </form> */}
