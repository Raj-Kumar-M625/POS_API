import React, { useEffect } from "react";
import { CustomCard } from "../../CommonComponent/CustomCard";
import PersonIcon from "@mui/icons-material/Person";
import SchoolIcon from "@mui/icons-material/School";
import BadgeIcon from "@mui/icons-material/Badge";
import HouseIcon from "@mui/icons-material/House";
import LocalHospitalIcon from "@mui/icons-material/LocalHospital";
import AddAPhotoIcon from "@mui/icons-material/AddAPhoto";
import LockIcon from "@mui/icons-material/Lock";
import NoteAddIcon from "@mui/icons-material/NoteAdd";
import { Link, useLocation, useParams, useNavigate } from "react-router-dom";
import ListAltIcon from "@mui/icons-material/ListAlt";
import CheckCircleRoundedIcon from "@mui/icons-material/CheckCircleRounded";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import i18n from "../../i18n/i18n";
import { AdminNavbar } from "../../CommonComponent/AdminNavbar";
import { config } from "../../constants";
//import { privateAxios } from "../../context/Axios";
import "../../CSS/BreadCrumb.css";
import axios from "axios";
import "../../CSS/Views/SurveyDetail.css";

function SurveyInfo_SurveyDetails() {
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
  const params = useParams();
  const { t } = useTranslation();
  const location = useLocation();
  const [data, setdata] = useState([]);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    let uri = `${config.localUrl}SurveyMapper/GetMappedSurveyByID?Surveyid=${params.SurveyId}`;
    privateAxios.get(uri)
      .then((res) => {
        debugger;
        return res.data;
      })
      .then((response) => {
        debugger;
        setdata(response);
      });
  }

  function getStatus(categoryId) {
    for (var json in data) {
      if (data[json].categoryId == categoryId) {
        return data[json];
      }
    }
    return { status: 0 };
  }


  return (
    <>
      <AdminNavbar data={{ Title: i18n.t('SurveyDetails') }}></AdminNavbar>

      <nav aria-label="breadcrumb" className="mx-3">
        {location.state.origin === "Surveyor" ? <ol class="breadcrumb">
          <li class="breadcrumb-item"><Link className="textDecoration" to="/Surveyor/SurveyDashboard">{i18n.t("Home")}</Link></li>
          <li class="breadcrumb-item"><Link className="textDecoration" to="/Surveyor/SurveyList2">{i18n.t("surveyList")}</Link></li>
          <li class="breadcrumb-item active" aria-current="page">{i18n.t("SurveyDetails")}</li>
        </ol> :
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><Link className="textDecoration" to="/Admin/Dashboard">{i18n.t('admin')}</Link></li>
            <li class="breadcrumb-item"><Link className="textDecoration" to="/Surveyor/SurveyList">{i18n.t('Dashboard.SurveyManagement')}</Link></li>
            <li class="breadcrumb-item active" aria-current="page">{i18n.t('SurveyDetails')}</li>

          </ol>}
      </nav>
      <div className="container d-flex mx-auto justify-content-center">
        <h5 className="m-3 header">
          {i18n.t("surveyee")}:{" "}
          <b className="text-primary">{params.SurveyId}</b>
        </h5>
        <h5 className="m-3 header">
          {i18n.t("surveyeeName")}:
          <b className="text-primary"> {params.Name}</b>
        </h5>
        <h5 className="m-3 header">
          {i18n.t("status")}:
          <b className="text-primary"> {location.state.status}</b>
        </h5>
      </div>
      <div className="CardContainer"
      >
        <Link
          to={
            `/Surveyor/Survey/BasicDetails/${params.SurveyId}/${params.Name}`
          }
          state={location.state}
          className="textDecoration"
        >
          <CustomCard
            cardTitle={i18n.t("BasicSurvey")}
            backgroundColor="#636998"
            status={
              params.SurveyId !== null ? (
                <CheckCircleRoundedIcon
                  className="checked"
                ></CheckCircleRoundedIcon>
              ) : null
            }
            cardImg={

              <ListAltIcon
                className="icon"
              />


            }
          />
        </Link>
        <Link
          to={`/Surveyor/Survey/Image/${params.SurveyId}/${params.Name}`}
          className="textDecoration"
          state={location.state}
        >
          <CustomCard

            cardTitle={i18n.t("photo")}
            backgroundColor="rgba(255, 95, 88)"
            status={
              getStatus(76).status === 1 ? (
                <CheckCircleRoundedIcon
                  className="checked"

                ></CheckCircleRoundedIcon>

              ) : null
            }
            cardImg={
              <AddAPhotoIcon
                className="icon"
              ></AddAPhotoIcon>
            }
          />
        </Link>
        <Link
          to={`/Surveyor/Survey/PersonalDetails2/${params.SurveyId}/${params.Name}`}
          className="textDecoration"
          state={location.state}
        >
          <CustomCard
            cardTitle={i18n.t("Dashboard2.personal")}
            backgroundColor="#5098a7"
            status={
              getStatus(69).status === 1 ? (
                <CheckCircleRoundedIcon
                  className="checked"

                ></CheckCircleRoundedIcon>
              ) : null
            }
            cardImg={
              <PersonIcon
              className="icon"
              ></PersonIcon>
            }
          />
        </Link>
        <Link to={`/Surveyor/Survey/Education/${params.SurveyId}/${params.Name}`} state={location.state} className="textDecoration">
          <CustomCard
            cardTitle={i18n.t("Dashboard2.education")}
            backgroundColor="#636998"
            status={
              getStatus(70).status === 1 ? (
                <CheckCircleRoundedIcon
                  className="checked"

                ></CheckCircleRoundedIcon>
              ) : null
            }

            cardImg={
              <SchoolIcon
              className="icon"
              ></SchoolIcon>
            }
          />
        </Link>
        <Link to={`/Surveyor/Survey/Employment/${params.SurveyId}/${params.Name}`} state={location.state} className="textDecoration">
          <CustomCard
            cardTitle={i18n.t("Dashboard2.employment")}
            backgroundColor="#62a76c"
            status={
              getStatus(71).status === 1 ? (
                <CheckCircleRoundedIcon
                  className="checked"

                ></CheckCircleRoundedIcon>
              ) : null
            }
            cardImg={
              <BadgeIcon
              className="icon"
              ></BadgeIcon>
            }
          />
        </Link>
        <Link to={`/Surveyor/Survey/Housing/${params.SurveyId}/${params.Name}`} state={location.state} className="textDecoration">
          <CustomCard
            cardTitle={i18n.t("Dashboard2.housing")}
            backgroundColor="#33e1ff"
            status={
              getStatus(72).status === 1 ? (
                <CheckCircleRoundedIcon
                  className="checked"
                ></CheckCircleRoundedIcon>
              ) : null
            }
            cardImg={
              <HouseIcon
              className="icon"
              ></HouseIcon>
            }
          />
        </Link>
      </div>
      <div
       className="CardContainer2"
      >
        <Link to={`/Surveyor/Survey/Health/${params.SurveyId}/${params.Name}`} state={location.state} className="textDecoration">
          <CustomCard
            status={
              getStatus(73).status === 1 ? (
                <CheckCircleRoundedIcon
                  className="checked"
                ></CheckCircleRoundedIcon>
              ) : null
            }
            cardTitle={i18n.t("Dashboard2.health")}
            backgroundColor="#f3ca3e"
            cardImg={
              <LocalHospitalIcon
              className="icon"
              ></LocalHospitalIcon>
            }
          />
        </Link>
        <Link to={`/Surveyor/Survey/SocialSecurity/${params.SurveyId}/${params.Name}`} state={location.state} className="textDecoration">
          <CustomCard
            cardTitle={i18n.t("Dashboard2.social")}
            backgroundColor="#808bde"
            status={
              getStatus(74).status === 1 ? (
                <CheckCircleRoundedIcon
                  className="checked"
                ></CheckCircleRoundedIcon>
              ) : null
            }
            cardImg={
              <LockIcon
              className="icon"
              ></LockIcon>
            }
          />
        </Link>
        <Link
          to={`/Surveyor/Survey/AdditionalDetails/${params.SurveyId}/${params.Name}`} state={location.state}
          className="textDecoration"
        >
          <CustomCard
            status={
              getStatus(75).status === 1 ? (
                <CheckCircleRoundedIcon
                  className="checked"
                ></CheckCircleRoundedIcon>
              ) : null
            }
            cardTitle={i18n.t("Dashboard2.additional")}
            backgroundColor="#ff5f58"
            cardImg={
              <NoteAddIcon
              className="icon"
              ></NoteAddIcon>
            }
          />
        </Link>


      </div>
    </>
  );
}

export default SurveyInfo_SurveyDetails;
