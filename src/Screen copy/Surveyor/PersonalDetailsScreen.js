import BottomNavigation from "../BottomNavigation";
import Questiones from "../../CommonComponent/Questiones";
import Title from "../../CommonComponent/Title";
import React, { useState } from "react";
import { Form } from "react-bootstrap";
import { http } from "../../_services/http.service";
import { useNavigate } from "react-router-dom";
import PersonalDetails from "../../Questions/PersonalDetails";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import axios from "axios";
import { config } from "../../constants";

function PersonalDetailsScreen() {
  const [responseBody, setresponseBody] = useState({});
  const navigate = useNavigate();

  function inputChangeHandler(event) {
    const { name, value } = event.target;
    setresponseBody({ ...responseBody, [name]: value });
  }

  function backNavigator() {
    navigate("/Surveyor/SurveyDashboard");
  }

  function onSubmitHandler(e) {
    e.preventDefault();

    //const url = "https://localhost:7110/BasicSurveyDetail";
    const url = `${config.localUrl}BasicSurveyDetail`;

    http.PostData(url, JSON.stringify(responseBody));
    navigate("/Surveyor/VerifyAadhar");
  }

  return (
    <>
    <NavbarTitle data={{ Title: "Basic Personal Details" }}></NavbarTitle>
    <div className="container" style={{ marginBottom: "115px" }}>
      
      <Form onSubmit={onSubmitHandler}>
        <Questiones
          api={PersonalDetails.QuestionsPage1}
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

export default PersonalDetailsScreen;
