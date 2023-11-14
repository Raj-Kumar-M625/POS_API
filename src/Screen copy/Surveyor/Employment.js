import React, { useState } from "react";
import BottomNavigation from "../BottomNavigation";
import Title from "../../CommonComponent/Title";
import Questiones from "../../CommonComponent/Questiones";
import employment from "../../Questions/Employment";
import { useNavigate } from "react-router-dom";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import { http } from "../../_services/http.service";

function Employment() {
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
    <NavbarTitle data={{ Title: "Employment Details" }}></NavbarTitle>
      <div className="container">
      
        <div className="container d-flex mx-auto justify-content-center">
          <h5 className="m-3">Surveyor Id :{http.GetData().SurveyId}</h5>
          <h5 className="m-3">Surveyor Name : {http.GetData().Name}</h5>
        </div>
        
        <Questiones
          api={employment.Questions}
          inputChangeHandler={inputChangeHandler}
        />
        <BottomNavigation
          onSubmitHandler={onSubmitHandler}
          backNavigator={backNavigator}
        />
      </div>
    </>
  );
}

export default Employment;
