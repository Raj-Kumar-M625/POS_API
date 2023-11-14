import React, { useState } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import LogIn from "../Screen/Login";
import Topbar from "../Screen/Topbar";
import PersonalDetailsScreen from "../Screen/Surveyor/PersonalDetailsScreen";
import PersonalDetailsScreen2 from "../Screen/Surveyor/PersonalDetailsScreen2";
import AadharVerifyScreen from "../Screen/Surveyor/AadharVerifyScreen";
import { Footer } from "../Screen/Footer";
import { SocialSecurity } from "../Screen/Surveyor/SocialSecurity";
import { Education } from "../Screen/Surveyor/Education";
import { Additionalinformation } from "../Screen/Surveyor/AdditionalInformation";
import { SurveyorDashboard } from "../Screen/Surveyor/SurveyorDashboard";
import { SurveyDetails } from "../Screen/Surveyor/SurveyDetails";
import Health from "../Screen/Surveyor/Health";
import Housing from "../Screen/Surveyor/Housing";
import Employment from "../Screen/Surveyor/Employment";
import AdminDashboardScreen from "../Screen/AdminScreen/AdminDashboardScreen";
import SurveyList2 from "../Screen/Surveyor/SurveyList2";
import { SurveyList } from "../Screen/Surveyor/SurveyList";
import UserManagement from "../Screen/AdminScreen/UserManagement";
import UserAssignment from "../Screen/AdminScreen/UserAssignment";
import { EditScreen } from "../Screen/Surveyor/EditScreen";
import { AddUser } from "../Screen/Surveyor/AddUser";
import { AddAssignment } from "../Screen/Surveyor/AddAssignment";
import SurveyInfo_SurveyDetails from "../Screen/SurveyInfoScreen/SurveyInfo_SurveyDetails";
import SurveyInfo_Additionalinformation from "../Screen/SurveyInfoScreen/SurveyInfo_AdditionalInformation";
import SurveyInfo_Education from "../Screen/SurveyInfoScreen/SurveyInfo_Education";
import SurveyInfo_Employment from "../Screen/SurveyInfoScreen/SurveyInfo_Employment";
import SurveyInfo_Health from "../Screen/SurveyInfoScreen/SurveyInfo_Health";
import SurveyInfo_Housing from "../Screen/SurveyInfoScreen/SurveyInfo_Housing";
import SurveyInfo_PersonalDetailsScreen2 from "../Screen/SurveyInfoScreen/SurveyInfo_PersonalDetailsScreen2";
import SurveyInfo_SocialSecurity from "../Screen/SurveyInfoScreen/SurveyInfo_SocialSecurity";
import "../CSS/Views/route.css";
import WebcamCapture from "../Screen/Surveyor/WebCamCapture";
import App from "../App";
import SurveyInfo_BasicDetails from "../Screen/SurveyInfoScreen/SurveyInfo_BasicDetails";
import SurveyInfo_Image from "../Screen/SurveyInfoScreen/SurveyInfo_Image";
import { ErrorPage } from "../Screen/Common/ErrorPage";

export const Router = () => {
  const [loggedIn, setLoggedIn] = useState("Login");
 

  sessionStorage.setItem("isLoggedIn", "Login");
  const getStatus = sessionStorage.getItem("isLoggedIn");

  return (
    <div>
      <Topbar
        loggedIn={loggedIn}
        setLoggedIn={setLoggedIn}
        getStatus={getStatus}
      ></Topbar>
      <Routes>
        <Route path="/" element={<Navigate replace to="/Login" />}></Route>
        <Route
          exact
          path="/Login"
          element={<LogIn setLoggedIn={setLoggedIn} />}
        ></Route>
        <Route path="/Common/ErrorPage" element={<ErrorPage />}></Route>
        
        <Route
          path="/Surveyor/PersonalDetails"
          element={<PersonalDetailsScreen />}
        ></Route>
        <Route
          path="/Surveyor/PersonalDetails2"
          element={<PersonalDetailsScreen2 />}
        ></Route>
        <Route
          path="/Surveyor/VerifyAadhar"
          element={<AadharVerifyScreen />}
        ></Route>
        <Route path="/Surveyor/Health" element={<Health />}></Route>
        <Route
          path="/Surveyor/SocialSecurity"
          element={<SocialSecurity />}
        ></Route>
        <Route path="/Surveyor/Education" element={<Education />}></Route>
        <Route
          path="/Surveyor/AdditionalDetails"
          element={<Additionalinformation />}
        />
        <Route
          path="/Surveyor/SurveyDashboard"
          element={<SurveyorDashboard />}
        />
        <Route path="/Admin/UserManagement" element={<UserManagement />} />
        <Route path="/Admin/UserAssignment" element={<UserAssignment />} />
        <Route path="/Surveyor/Employment" element={<Employment />} />
        <Route path="/Surveyor/Housing" element={<Housing />} />

        <Route path="/Surveyor/SurveyDetails" element={<SurveyDetails />} />

        <Route path="/Surveyor/SurveyList" element={<SurveyList />} />
        <Route path="/Surveyor/SurveyList2" element={<SurveyList2 />} />
       
        <Route
          path="/Surveyor/Survey/PersonalDetails2/:SurveyId/:Name"
          element={<SurveyInfo_PersonalDetailsScreen2 />}
        />
        <Route
          path="/Surveyor/Survey/SurveyDetails/:SurveyId/:Name"
          element={<SurveyInfo_SurveyDetails />}
        />
        <Route
          path="/Surveyor/Survey/Health/:SurveyId/:Name"
          element={<SurveyInfo_Health />}
        ></Route>
        <Route
          path="/Surveyor/Survey/SocialSecurity/:SurveyId/:Name"
          element={<SurveyInfo_SocialSecurity />}
        ></Route>
        <Route
          path="/Surveyor/Survey/Education/:SurveyId/:Name"
          element={<SurveyInfo_Education />}
        ></Route>
        <Route
          path="/Surveyor/Survey/AdditionalDetails/:SurveyId/:Name"
          element={<SurveyInfo_Additionalinformation />}
        ></Route>
        <Route
          path="/Surveyor/Survey/Employment/:SurveyId/:Name"
          element={<SurveyInfo_Employment />}
        />
        <Route
          path="/Surveyor/Survey/BasicDetails/:SurveyId/:Name"
          element={<SurveyInfo_BasicDetails />}
        />
        <Route
          path="/Surveyor/Survey/Image/:SurveyId/:Name"
          element={<SurveyInfo_Image />}
        />
        <Route
          path="/Surveyor/Survey/Housing/:SurveyId/:Name"
          element={<SurveyInfo_Housing />}
        />
        <Route
          path="/Admin/Dashboard"
          element={<AdminDashboardScreen />}
        ></Route>
        <Route path="/edit" element={<EditScreen />} />
        <Route path="/add" element={<AddUser />} />
        <Route path="/add/userAssignment" element={<AddAssignment />} />
        <Route
          path="/Surveyor/CaptureImage"
          element={<WebcamCapture />}
        ></Route>
      </Routes>
      <Footer />
    </div>
  );
};
