import axios from "axios";
import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { config } from "../../src/constants";
import { useTranslation } from "react-i18next";
import i18n from "../i18n/i18n";
import spinner from "../Assets/Image/spinner.gif"
import { http } from "../_services/http.service";
import { Navbar } from "react-bootstrap";
import { authenticationService } from "../_services/authentication.service";
import { privateAxios } from "../context/Axios";
import "../CSS/Views/login.css";

const LOGIN = ({ setLoggedIn, props }) => {

  const [showResults, setShowResults] = React.useState(true);
  const [user, setUser] = React.useState("Surveyor");
  const [surveyorID, setSurveyorID] = React.useState("");
  const [phoneNo, setPhoneNo] = React.useState();
  const [otpError, setotpError] = useState(false);
  const [otp, setOtp] = React.useState("");
  const [isShown, setIsShown] = useState(false);
  const [buttonText, setButtonText] = useState(true);
  const [loginButton, setLoginButton] = useState(false);
  const [showPhoneError, setShowPhoneError] = useState(false);
  const [failed,setFailed] = useState(false);
  const [DBdata, setDBdata] = useState([]);
  const [statusCode, setstatusCode] = useState(0)
  const [loginCode,setLoginCode] = useState(0)
  const history = useNavigate();
  const { t } = useTranslation();
  const [data, setData] = useState([]);
  const [enteredOtp, setenteredOtp] = useState();
  const [invalidUser, setInvalidUser] = useState(false);
  const [invalidUserError, setInvalidUserError] = useState("");
  const handleIDChange = (value) => {
    setSurveyorID(value);
  };

  useEffect(() => {
    get();
  }, []);

  async function get() {
    const x = await axios.get(
      `${config.localUrl}Download/DownloadSurveys`
    );
    debugger;
    setDBdata(x.data);
    localStorage.setItem("Questions", JSON.stringify(x.data));

  }



  const handlePhoneChange = (event) => {
    const result = event.target.value.replace(/\D/g, "");
    setPhoneNo(result);
  };

  const handleOtpChange = (value) => {
    setOtp(value);
    setenteredOtp(value);
  };

  //Admin login
  const handleSave = async () => {
    setLoginCode(1)
    let url = `${config.localUrl}Auth/phone/verify`;
    let authData = {
      phoneNumber: phoneNo,
      otp: enteredOtp,
    };


    const loginData = await axios.post(url, authData).then((response) => {
      debugger
      if (response.data.userRoles === user) {
        sessionStorage.setItem("isLoggedIn", "Logout");

        sessionStorage.setItem("LoginData", JSON.stringify(
          response.data
        ))
        sessionStorage.setItem("User", response.data.userId);
        sessionStorage.setItem("token", response.data.token);
        setotpError(false);
        sessionStorage.setItem("isLoggedIn", "Logout");
        setInvalidUser(false);
        setLoginCode(0)
        history("/Admin/Dashboard");
      } else {
        setLoginCode(0)
        setInvalidUser(true);
        setInvalidUserError(i18n.t("error.InvalidUser"))
      }




    }).catch((error) => {
      setLoginCode(0)
      setotpError(true);
      return;
    })
  }


  //Surveyor login
  const handleSurveySave = async () => {
    setLoginCode(1)
    debugger;
    let url = `${config.localUrl}Auth/phone/verify`;
    let authData = {
      phoneNumber: phoneNo,
      otp: enteredOtp,
    };


    const loginData = await axios.post(url, authData).then((response) => {
      debugger
      if (response.data.userRoles === user) {
        sessionStorage.setItem("isLoggedIn", "Logout");

        sessionStorage.setItem("LoginData", JSON.stringify(
          response.data
        ))
        sessionStorage.setItem("User", response.data.userId);
        sessionStorage.setItem("token", response.data.token);
        setotpError(false);
        setInvalidUser(false);
        setLoginCode(0)
        if(user === "Surveyor"){
          history("/Surveyor/SurveyDashboard");
        }else if(user === "Admin"){
          history("/Admin/Dashboard");
        }
       

      } else {
        setLoginCode(0)
        setInvalidUser(true);
        setInvalidUserError(i18n.t("error.InvalidUser"))
      }



    }).catch((error) => {
      setLoginCode(0)
      setotpError(true);
      setInvalidUserError("");
      return;
    })
    console.log(loginData)
    console.log(otpError);

  };
  //Surveyor Otp generation
  const handleClick = async (event) => {
    debugger;
    setstatusCode(1)
    let url = `${config.localUrl}Auth/phone`;
    var response;
    try {
      response = await axios.get(url + "?phone=" + phoneNo);
      debugger;
      if (response.data === "OTP generated successfully!") {
        setShowPhoneError(false);
        setIsShown(true);
        setButtonText(false);
        setLoginButton(true);
        setInvalidUser(false);
        setstatusCode(0);
      }

      return;
    } catch (e) {
      debugger
      if(e.response.status === 404){
        setShowPhoneError(true);
      }else if(e.response.status === 304){

      }
      if (response != null) {
        setstatusCode(0)
        if (response.data !== "OTP generated successfully!") {
          setShowPhoneError(true);
          return;
        }
      } else {
        setstatusCode(0)
        return;
      }


    }

  };

  //admin user otp generation
  const handleClickviewer = async (event) => {
    setstatusCode(1)
    let url = `${config.localUrl}Auth/phone`;
    var response;
    try {
      response = await axios.get(url + "?phone=" + phoneNo);
      if (response.data === "OTP generated successfully!") {
        setShowPhoneError(false);
        setIsShown(true);
        setButtonText(false);
        setLoginButton(true);
        setInvalidUser(false);
      }
      return;
    } catch (e) {
      if (response != null) {
        setstatusCode(0)
        if (response.data !== "OTP generated successfully!") {
          setShowPhoneError(true);
          return;
        }
      } else {
        setstatusCode(0)
        return;
      }

    }
  };

  const resendOtp = () => {
    let OTP = Math.floor(1000 + Math.random() * 9000);
    setOtp(OTP);
    setTimeout(() => {
      alert("Your OTP is " + OTP);
    }, 2000);
  };

  const onChangeUser = (e) => {
    if (isShown) {
      setIsShown((current) => !current);
    }
    setButtonText(true);
    setLoginButton(false);
    setotpError(false);
    setShowPhoneError(false);
    setShowResults(!showResults);
    setPhoneNo("");
    setUser(e.target.value);
    console.log(user);
  };

  const areAllFieldsFilled = phoneNo != "";

  return (
    <>
      <div
        className="loginContainer"

      >
        <div

          className="logContainer"
        >
          <h3
           className="loginHeader"
          >
            {i18n.t("SignIn")}
          </h3>

          <label for="Select-User">{i18n.t("SelectUser")}</label>

          <select
            name=""
            id="Select-User"
            onChange={onChangeUser}
            className="form-select"
            required
          >
            <option value="Surveyor">{i18n.t("Surveyor")}</option>
            <option value="Admin">{i18n.t("Admin")}</option>
          </select>
          
            <>
              <label for="phoneNo">{i18n.t("MobileNumber")}</label>
              <input
                type="text"
                className="form-control"
                required
                value={phoneNo}
                onChange={handlePhoneChange}
                maxLength={10}
                id="phoneNo"
                name="phoneNo"
                autoComplete="off"
              />

              {showPhoneError && (
                <p
                  className="text-danger text-sm text-center errorStyle"
                  
                >
                  {i18n.t('error.InvalidPhone')}
                </p>
              )}
              {failed && (
                <p
                className="text-danger text-sm text-center errorStyle"
                
              >
                {i18n.t('error.OtpSendFailed')}
              </p>
              )}

              {isShown ? (
                <>
                  <label for="otp">{i18n.t("EnterOtp")}</label>
                  <input
                    type="text"
                    id="otp"
                    className="form-control"
                    required
                    onChange={(e) => handleOtpChange(e.target.value)}
                    maxLength={6}
                    autoComplete="off"
                  />
                  {otpError ? (
                    <p
                      className="text-danger text-sm text-center errorStyle"
                    >
                      {i18n.t('error.InvalidOtp')}
                    </p>
                  ) : ""}
                  {invalidUser ? (<p
                    className="text-danger text-sm text-center errorStyle"
                  >
                    {invalidUserError}
                  </p>) : ""}
                </>
              ) : ""}

              {buttonText ? (
                <>
                  <button
                    className="btn btn-warning otpButton"
                    type="submit"
                    onClick={() => {
                      handleClick();
                    }}
                    disabled={!phoneNo || statusCode}
                    
                  >
                    {statusCode === 0 ? i18n.t("SendOtp") : <>
                      <img src={spinner} width="24px" height="24px" alt="spinner"/>
                    </>}
                  </button>
                </>
              ):""}

              {loginButton ? (
                <>
                  <button
                    className="btn btn-warning loginButton"
                    type="submit"
                    
                    onClick={() => {
                      handleSurveySave();
                    }}
                  >
                    {loginCode === 0 ? i18n.t("LogIn") : <>
                      <img src={spinner} width="24px" height="24px" alt="spinner"/>
                    </>}
                  </button>
                  <div
                    className="text-center mb-6"
                    
                  >
                    <Link
                      className="text-muted resendOtp"
                      onClick={() => {
                        handleClick();
                      }}
                    >
                      {i18n.t("resend")}
                    </Link>
                  </div>
                </>
               ): ""}
            </>
          

          {/* {user === "Viewer" ? (
            <>
              <label for="phoneNo">{i18n.t("MobileNumber")}</label>
              <input
                type="text"
                className="form-control"
                required
                defaultValue={phoneNo}
                onChange={handlePhoneChange}
                maxLength={10}
                id="phoneNo"
                name="phoneNo"
                autoComplete="off"
              />
              {showPhoneError && (
                <p
                  className="text-danger text-sm text-center errorStyle"
                >
                  {i18n.t('error.InvalidPhone')}
                </p>
              )}

              {isShown ? (
                <>
                  <label for="otp">{i18n.t("EnterOtp")}</label>

                  <input
                    type="text"
                    id="otp"
                    className="form-control"
                    required
                    onChange={(e) => handleOtpChange(e.target.value)}
                    maxLength={6}
                    autoComplete="off"
                  />
                  {otpError ? (
                    <p
                      className="text-danger text-sm text-center errorStyle"
                    >
                      {i18n.t('error.InvalidOtp')}
                    </p>
                  ) : ""}
                  {invalidUser ? (<p
                    className="text-danger text-sm text-center errorStyle"
                  >
                    {invalidUserError}
                  </p>) : ""}
                </>
              ): ""}

              {buttonText ? (
                <>
                  <button
                    className="btn btn-warning otpButton"
                    type="submit"
                    onClick={() => {
                      handleClickviewer();
                    }}
                    disabled={!phoneNo}
                    
                  >
                    {statusCode === 0 ? i18n.t("SendOtp") : <>
                      <img src={spinner} width="24px" height="24px" alt="spinner"/>
                    </>}
                  </button>
                </>
              ):""}

              {loginButton ? (
                <>
                  <button
                    className="btn btn-warning loginButton"
                    type="submit"
                    onClick={() => {
                      handleSave();
                    }}
                  >
                    {loginCode === 0 ? i18n.t("LogIn") : <>
                      <img src={spinner} width="24px" height="24px" alt="spinner" />
                    </>}
                    
                  </button>
                  <div
                    className="w-100 text-center mb-3"
                    
                  >
                    <Link
                      
                      className="text-muted"
                      onClick={() => {
                        handleClick();
                      }}
                    >
                      {i18n.t("resend")}
                    </Link>
                  </div>
                </>
              ):""}
            </>
          ) : null} */}
        </div>
      </div>
    </>
  );
};

export default LOGIN;

