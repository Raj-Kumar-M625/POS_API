import React, { useEffect, useState } from 'react'
import { Link, useLocation, useNavigate, useParams } from 'react-router-dom'
import { AdminNavbar } from '../../CommonComponent/AdminNavbar';
import { config } from '../../constants';
import i18n from '../../i18n/i18n';

import axios from 'axios';

function SurveyInfo_Image() {
  const navigate = useNavigate();
  const User = JSON.parse(sessionStorage.getItem("LoginData"));
  if (User === "" || User == null) {
    sessionStorage.setItem("isLoggedIn", "Login");
    window.location.href = "/Login";
  }

  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Content-type": "application/json",
      "Authorization": `Bearer ${sessionStorage.getItem("token")}`
    },

  });
  const location = useLocation();
  const params = useParams();
  const info = privateAxios.get(`${config.localUrl}BasicSurveyDetail/getBasicSurveyById?id=${params.SurveyId}`)
  const [data, setdata] = useState();

  useEffect(() => {
    const url = `${config
      .localUrl}SurveyImages/GetSurveyImagesById?surveyId=${params.SurveyId}`;
    privateAxios.get(url)
      .then((res) => {
        return res.data;
      })
      .then((response) => {
        setdata(response);
      });
  }, []);

  return (
    <>
      <AdminNavbar data={{ Title: "Images" }}></AdminNavbar>
      <nav aria-label="breadcrumb" className="mx-3">
        {location.state.origin === "Surveyor" ? <ol className="breadcrumb">
          <li class="breadcrumb-item textDecoration"><Link to="/Surveyor/SurveyDashboard" >{i18n.t("Home")}</Link></li>
          <li class="breadcrumb-item textDecoration"><Link to="/Surveyor/SurveyList2" >{i18n.t("surveyList")}</Link></li>
          <li class="breadcrumb-item textDecoration"><Link to={`/Surveyor/Survey/SurveyDetails/${params.SurveyId}/${params.Name}`} state={location.state} >Survey Details</Link></li>
          <li class="breadcrumb-item active" aria-current="page">Images</li>
        </ol> :
          <ol class="breadcrumb">
            <li class="breadcrumb-item textDecoration"><Link to="/Admin/Dashboard" >Admin Dashboard</Link></li>
            <li class="breadcrumb-item textDecoration"><Link to="/Surveyor/SurveyList" >Survey Management</Link></li>
            <li class="breadcrumb-item textDecoration"><Link to={`/Surveyor/Survey/SurveyDetails/${params.SurveyId}/${params.Name}`} state={location.state} >Survey Details</Link></li>
            <li class="breadcrumb-item active" aria-current="page">Images</li>

          </ol>
        }
      </nav>
      <div className="container d-flex mx-auto justify-content-center">
        <h5 className="m-3 header">
          {i18n.t("surveyee")}: <b className="text-primary">{params.SurveyId}</b>
        </h5>
        <h5 className="m-3 header">
          {i18n.t("surveyeeName")}: <b className="text-primary">{params.Name}</b>
        </h5>
        <h5 className="m-3 header">{i18n.t("status")}: <b className="text-primary">{location.state.status}</b></h5>
      </div>

      <div className='container d-flex w-75 justify-content-center'>
        <img src={
          data === null || data === undefined || data.length <= 0
            ? ""
            : data[1].image
        } width="283px" height="200px" className="mx-1 mt-3" alt="Image1"/>
        <img src={
          data === null || data === undefined || data.length <= 0
            ? ""
            : data[1].image
        } width="283px" height="200px" className="mx-1 mt-3" alt="LoadingImage"/>

      </div>
    </>
  )
}

export default SurveyInfo_Image;