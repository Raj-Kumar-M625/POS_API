import React, { useState } from "react";
import Questiones from "../../CommonComponent/Questiones";
import { useNavigate } from "react-router-dom";
import { http } from "../../_services/http.service";
import BottomNavigation from "../BottomNavigation";
import PersonalDetails from "../../Questions/PersonalDetails";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import { Form } from "react-bootstrap";
import { config } from "../../constants";

function PersonalDetailsScreen2() {
  const navigate = useNavigate();
  const [responseBody, setresponseBody] = useState({});

  function inputChangeHandler(event) {
    const { name, value } = event.target;
    setresponseBody({ ...responseBody, [name]: value });
  }

  function onSubmitHandler(e) {
    e.preventDefault();
    console.log("Submitted Successfully");
    console.log(responseBody);

    //const url = "https://localhost:7110/Survey";
    const url = `${config.localUrl}Survey`;
    http.PostData(url, responseBody);
    // navigate("/Surveyor/SurveyDetails");
  }

  function backNavigator() {
    navigate("/Surveyor/SurveyDetails");
  }

  return (
    <>
      <NavbarTitle data={{ Title: "Personal Details" }}></NavbarTitle>
      <div className="container">
        <div className="container d-flex mx-auto justify-content-center">
          <h5 className="m-3">Surveyor Id :{http.GetData().SurveyId}</h5>
          <h5 className="m-3">Surveyor Name : {http.GetData().Name}</h5>
        </div>
        <Form onSubmit={onSubmitHandler}>
          <Questiones
            api={PersonalDetails.QuestionsPage2}
            inputChangeHandler={inputChangeHandler}
          />

          <BottomNavigation
            onSubmitHandler={onSubmitHandler}
            backNavigator={backNavigator}
          />
        </Form>
      </div>
    </>
  );
}

export default PersonalDetailsScreen2;
