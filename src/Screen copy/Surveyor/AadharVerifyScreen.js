import React, { useState, useRef, useEffect } from "react";
import { Button, Form } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import BottomNavigation from "../BottomNavigation";
import AadharDetails from "./AadharDetails";

function AadharVerifyScreen() {
  const [value, setvalue] = useState("");
  const navigate = useNavigate();

  function backNavigator() {
    navigate("/Surveyor/PersonalDetails");
  }

  function navigationtHandler() {
    navigate("/Surveyor/SurveyDetails");
  }

  const AadharGiven = useRef();
  const AadharNotGiven = useRef();
  const personalDetails = useRef();
  const disable = useRef();

  function handleChange(event) {
    event.preventDefault();
    setvalue(event.target.value);
  }
  useEffect(() => {
    if (value === "Yes") {
      AadharGiven.current.style.display = "block";
      AadharNotGiven.current.style.display = "none";
    } else if (value === "No") {
      AadharNotGiven.current.style.display = "block";
      AadharGiven.current.style.display = "none";
      personalDetails.current.style.display = "none";
    }
  }, [value]);

  function showPersonalDetails() {
    disable.current.disabled = true;
    personalDetails.current.style.display = "block";
  }

  return (
    <div className="container">
      <Form onSubmit={navigationtHandler}>
        <div className="container mt-3 mx-auto w-75">
          <Form.Label htmlFor="">
            Would You Like to provide your Aadhar?
          </Form.Label>
          <Form.Select onChange={handleChange} ref={disable} required>
            <option defaultValue="selected">--Select--</option>
            <option>Yes</option>
            <option>No</option>
          </Form.Select>
          <div
            style={{
              display: "none",
              marginTop: "26px",
            }}
            ref={AadharGiven}
          >
            <Form.Control type="text" placeholder="Enter your aadhar" />
            <Button className="mt-2" onClick={showPersonalDetails}>
              Verify
            </Button>
          </div>

          <div
            style={{
              display: "none",
              marginTop: "26px",
            }}
            ref={AadharNotGiven}
          >
            <Form.Label htmlFor="">Reason for not giving Aadhar.</Form.Label>

            <Form.Select>
              <option defaultValue="selected">--Select--</option>
              <option>I don't have my aadhar</option>
              <option>I lost my aadhar</option>
              <option>Refuses to give</option>
            </Form.Select>
          </div>

          <div style={{ display: "none" }} ref={personalDetails}>
            <AadharDetails />
          </div>
        </div>
        <BottomNavigation
          backNavigator={backNavigator}
          onSubmitHandler={navigationtHandler}
        />
      </Form>
      {/* <button onClick={navigationtHandler} className="btn btn-primary">
        Navigate
      </button> */}
    </div>
  );
}

export default AadharVerifyScreen;
