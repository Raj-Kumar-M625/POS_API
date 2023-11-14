import axios from "axios";
import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";

const LogIn = ({ setLoggedIn }) => {
  const [showResults, setShowResults] = React.useState(true);
  const [user, setUser] = React.useState("Surveyor");
  const [surveyorID, setSurveyorID] = React.useState("");
  const [phoneNo, setPhoneNo] = React.useState();
  const [otp, setOtp] = React.useState("")

  const handleIDChange = (value) =>{
      setSurveyorID(value)
  }

  const handlePhoneChange = (value) =>{
    setPhoneNo(value);
  }

  const handleOtpChange = (value) =>{
    setOtp(value);
  }

  const pushView = (e) =>{
      localStorage.setItem("isLoggedIn","Logout")
      history("/Admin/Dahsboard")

  }

  const surveyView = (e) =>{
    localStorage.setItem("isLoggedIn","Logout")
    history("/Surveyor/SurveyDashboard")

}

  // const handleSave = () =>{
  // //   const data ={
  // //     PhoneNumber : phoneNo,
  // //   };
  // //   const headers = { 
  // //     contentType:'application/json'
      
  // // };


  //   const url = 'https://localhost:7110/v1/Auth/phone';
  //   axios.get(url).then((result)=> {
  //     alert(result.data);

  //   }).catch((error)=>{
  //     if (error.response) {
        
  //   } else if (error.request) {
        
  //       console.log(error.request);
  //   } else {
  //       console.log('Error', error.message);
  //   }
  //   console.log(error.config);    })
  // }

  
  //    const [josn,setJson] = useState([])
  //   useEffect(() => {
  //     axios
  //       .get("https://localhost:7110/v1/Auth/phone")
  //       .then(function (response) {
  //         setJson(response.data)
          
  //       });
  //   }, []);
    
  //   console.log(josn)
  const history = useNavigate();

  const onChangeUser = (e) => {
    setShowResults(!showResults);
    setUser(e.target.value);
  };
  const HandleLogIn = () => {
    setUser("otpscreen");
  };
  const HandleLogIn2 = () => {
    setUser("otpscreen2");
  };
  console.log("isLoggedIn : " + localStorage.getItem("isLoggedIn"));
  return (
    <div class="row">
      <div
        style={{
          width: "40%",
          borderRadius: "15px",
          boxShadow: " 0 3px 10px rgb(0 0 0 / 0.2)",
          textAlign: "center",
          padding: "45px",
          margin: "60px auto",
        }}
      >
        <div class="row justify-content-center">
          <h5 className="row-md-3 mb-1">Sign In</h5>

          <h6 class="col-md-3">Select User</h6>
          <div class="col-md-6">
            <select id="s1" onChange={onChangeUser} class="form-control">
              <option value="Surveyor">Surveyor</option>

              <option value="LoginScreen">Viewer</option>
            </select>
          </div>
        </div>
        <br />
        {user === "Surveyor" ? (
          <div>
          <div class="form-group row justify-content-center mb-3">
            <label class="control-label col-sm-3">Mobile Number</label>
            <div class="col-sm-6">
              <input type="text" class="form-control" onChange={(e)=> handlePhoneChange(e.target.value)} maxLength={10}/>
            </div>
          </div>

          <div class="form-group row justify-content-center">
            <div class="col-sm-4">
              <button
                class="form-control"
                style={{ background: "#eeb919", fontWeight: "600" }}
                onClick={HandleLogIn2}
              >
                Send OTP
              </button>
            </div>
            <Link>Resend OTP</Link>
          </div>
        </div>
        ) : null}
        <div></div>

        {user === "LoginScreen" ? (
          <div>
            <div class="form-group row justify-content-center mb-3">
              <label class="control-label col-sm-3">Mobile Number</label>
              <div class="col-sm-6">
                <input type="text" class="form-control" onChange={(e)=> handlePhoneChange(e.target.value)}/>
              </div>
            </div>

            <div class="form-group row justify-content-center">
              <div class="col-sm-4">
                <button
                  class="form-control"
                  style={{ background: "#eeb919", fontWeight: "600" }}
                  onClick={HandleLogIn}
                >
                  Send OTP
                </button>
              </div>
              <Link>Resend OTP</Link>
            </div>
          </div>
        ) : null}

{user === "otpscreen2" ? (
          <div>
            <div class="form-group row justify-content-center ">
              <label class="control-label col-sm-3">Mobile Number</label>
              <div class="col-sm-6">
                <input type="text" class="form-control" onChange={(e)=> handlePhoneChange(e.target.value)}  maxLength={10}/>
              </div>
            </div>
            <br />
            <div class="form-group row justify-content-center">
              <label class="control-label col-sm-3">Enter OTP</label>
              <div class="col-sm-6 d-flex mb-4 justify-content-between">
                <input type="text" class="form-control row-sm-3" onChange={(e)=> handleOtpChange(e.target.value)}  maxLength={4}/>
              </div>
            </div>
            <div class="form-group row justify-content-center">
              <div class="col-sm-4">
                <button
                  class="form-control"
                  style={{ background: "#eeb919", fontWeight: "600" }}
                  onClick={() => {surveyView()}}
                > 
                  Login
                </button>
              </div>
            </div>
          </div>
        ) : null}

        {user === "otpscreen" ? (
          <div>
            <div class="form-group row justify-content-center ">
              <label class="control-label col-sm-3">Mobile Number</label>
              <div class="col-sm-6">
                <input type="text" class="form-control" onChange={(e)=> handlePhoneChange(e.target.value)}  maxLength={10}/>
              </div>
            </div>
            <br />
            <div class="form-group row justify-content-center">
              <label class="control-label col-sm-3">Enter OTP</label>
              <div class="col-sm-6 d-flex mb-4 justify-content-between">
                <input type="text" class="form-control row-sm-3" onChange={(e)=> handleOtpChange(e.target.value)}  maxLength={4}/>
              </div>
            </div>
            <div class="form-group row justify-content-center">
              <div class="col-sm-4">
                <button
                  class="form-control"
                  style={{ background: "#eeb919", fontWeight: "600" }}
                  onClick={() => {pushView()}}
                > 
                  Login
                </button>
              </div>
            </div>
          </div>
        ) : null}
      </div>
    </div>
  );
};



export default LogIn;
