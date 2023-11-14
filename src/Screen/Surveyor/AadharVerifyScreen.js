import React, { useState, useRef, useEffect } from "react";
import { Button, Form } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import i18n from "../../i18n/i18n";
import loading from "../../Assets/Image/spinner.gif"
import { useTranslation } from "react-i18next";
import Accordian from "../../CommonComponent/Accordian";
import SurveyService from "../../_services/SurveyService";
import {  Modal } from "react-bootstrap";
import "../../CSS/Views/VerifyAadhar.css";

function AadharVerifyScreen({
  location,
  BasicDetails,
  setBasicDetails,
}) {
  const [value, setvalue] = useState("Yes");
  const [show, setShow] = useState(false);
  const [spinner, setspinner] = useState(false);
  const navigate = useNavigate();
  const [aadharNumber, setaadharNumber] = useState("");
  const { t } = useTranslation();
  // const loc = useLocation();
  const [others, setothers] = useState();

  function navigationtHandler() {
    navigate("/Surveyor/SurveyDetails", {
      state: {
        id: location.state.id,
        name: location.state.name,
      },
    });
  }

  const AadharGiven = useRef();
  const AadharNotGiven = useRef();
  const personalDetails = useRef();
  const disable = useRef();
  const selectRef = useRef();
  const other = useRef();
  function handleChange(event) {
    event.preventDefault();
    console.log(others);

    setvalue(event.target.value);
  }
  useEffect(() => {
    if (value === "Yes") {
      AadharGiven.current.style.display = "block";
      AadharNotGiven.current.style.display = "none";
      other.current.style.display = "none";
    } else if (value === "No") {
      setShow(false)
      AadharNotGiven.current.style.display = "block";
      AadharGiven.current.style.display = "none";
      // personalDetails.current.style.display = "none";
    }
  }, [value]);

  useEffect(() => {
    if (
      others === "Others" ||
      selectRef.current.value === "Others" ||
      selectRef.current.value === "ಇತರರು"
    ) {
      other.current.style.display = "block";
    } else {
      other.current.style.display = "none";
    }
  }, [others]);

  function showPersonalDetails() {
    disable.current.disabled = true;
  }

  const fetchPersonalDetails = async (AadharNumber) => {
    setspinner(true)
    try {
      
      const KutumbaRequest = {
        depId: "",
        benId: "",
        rc_Number: "",
        aadhar_No: AadharNumber,
        clientCode: "",
        hashedMac: "",
        apiVersion: "1.0",
        isPhotoRequired: "0",
        memberId: "",
        mobile_No: "",
        request_Id: "0123456789",
        uidType: "1",
      };
        setShow(false)
      const res = await SurveyService.getAadharInfo(KutumbaRequest);
      debugger;
      if (res.data.length > 0) {
        setspinner(false)
        setBasicDetails({
          ...BasicDetails,
          DBTEKYCDataId: 1,
          AadharStatus: "Verified",
          age: res.data[0].mbr_DOB ?
            new Date().getFullYear() -
            new Date(
              res.data[0].mbr_DOB.replace(/(\d\d)\/(\d\d)\/(\d{4})/, "$3-$2-$1")
            ).getFullYear() : "",
          name: res.data[0].member_Name_Eng ? res.data[0].member_Name_Eng : "",
          address: res.data[0].mbR_ADDRESS ? res.data[0].mbR_ADDRESS : "",
          dob: res.data[0].mbr_DOB
            ? res.data[0].mbr_DOB.replace(/(\d\d)\/(\d\d)\/(\d{4})/, "$3-$2-$1")
            : "",
          genderByBirth: res.data[0].mbr_Gender
            ? res.data[0].mbr_Gender === "M"
              ? i18n.t("Male")
              : i18n.t("Female")
            : "",
        });
      } else {
        setspinner(false)
        setShow(true)
      }
    } catch (ex) {
      throw ex;
    }
  };
  const handleClose = () => setShow(false);
  return (
    <>
 
      <div className="container">
     
        <Accordian title={i18n.t("VerifyAadhar")}>
          <Form
            onSubmit={navigationtHandler}
            className="col g-3 mt-3 "
          >
            <div className="container row-md-4 align-self-center w-75">
              <Form.Label htmlFor="">{i18n.t("Aadhaar")}</Form.Label>
              <div className="d-flex">
                <Form.Select
                  onChange={handleChange}
                  ref={disable}
                  // disabled={!Aadhar?true:false}
                  required={true}
                >
                  <option selected value="Yes">
                    {i18n.t("Yes")}
                  </option>
                  <option value="No">{i18n.t("No")}</option>
                </Form.Select>
                <Button
                  onClick={showPersonalDetails}
                  className="mx-3 hiddenButton"
                  
                >
                  { i18n.t("verify")}
                </Button>
              </div>
            </div>
            <div
              className="row-md-12 container w-75 mt-3"
              ref={AadharGiven}
            >
              <Form.Label htmlFor="">{i18n.t("EnterAadhar")}</Form.Label>

              <div className="d-flex">
              

                <Form.Control
                  // disabled={!Aadhar?true:false}
                  onChange={(e) => setaadharNumber(e.target.value)}
                  type="text"
                  pattern="[2-9]{1}[0-9]{11}"
                  maxLength={12}
                  minLength={12}
                  required={value === "Yes" ? true : false}
                />
              
              
                <Button
                  onClick={() => fetchPersonalDetails(aadharNumber)}
                  className="mx-3"
                  disabled={spinner}
                >
                   {spinner ? <>
                  <img src={loading} width="25px" height="25px" alt=""/>
                  </> : i18n.t("verify")}
                </Button>
              </div>
              {show && <div className="text-danger mt-2">Invalid Aadhar Number!</div>}
            </div>

            <div
              className="row-md-6"
              ref={AadharNotGiven}
            >
              <div className="container w-75 mt-3">
                <Form.Label htmlFor="">{i18n.t("Reason")}</Form.Label>
                <div className="d-flex">
                  <Form.Select
                    required={value == "No" ? true : false}
                    ref={selectRef}
                    onChange={(e) => {
                      if(e.target.value !== "Others")
                      setBasicDetails({
                        ...BasicDetails,
                        DBTEKYCDataId: 0,
                        AadharStatus: e.target.value
                      })
                      setothers(e.target.value);
                    }}
                  >
                    <option defaultValue="" disabled selected>
                      {i18n.t("Select")}
                    </option>
                    <option defaultValue={i18n.t("Reason1")}>
                      {i18n.t("Reason1")}
                    </option>
                    <option defaultValue={i18n.t("Reason2")}>
                      {i18n.t("Reason2")}
                    </option>
                    <option defaultValue={i18n.t("Reason3")}>
                      {i18n.t("Reason3")}
                    </option>
                    <option defaultValue={i18n.t("Reason4")}>
                      {i18n.t("Reason4")}
                    </option>
                  </Form.Select>
                  <Button
                    onClick={showPersonalDetails}
                    className="mx-3 hiddenButton"
                    disabled={true}
                  >
                      { i18n.t("verify")}
                  </Button>
                </div>
              </div>
            </div>
            <div
              className="container w-75 mt-3"
              ref={other}
            >
              <Form.Label htmlFor="">If Others Specify</Form.Label>
              <div className="d-flex">
                <Form.Control
                  type="text"
                  required={others === "Others" ? true : false}
                  onChange={(e) => {
                    setBasicDetails({
                      ...BasicDetails,
                      DBTEKYCDataId: 0,
                      AadharStatus: e.target.value
                    })
                  }
                  }
                />
              </div>
            </div>
           
            <div className="row-md-12 d-flex justify-content-end">
              {/* <Button className="btn btn-success" type="submit" disabled={!Aadhar?true:false}>
                {i18n.t("Submit")}
              </Button> */}
            </div>
          </Form>
        </Accordian>
      </div>
    </>
  );
}

export default AadharVerifyScreen;
