import React, { useEffect, useState } from "react";
import { CustomCard } from "../../CommonComponent/CustomCard";
import PersonIcon from "@mui/icons-material/Person";
import SchoolIcon from "@mui/icons-material/School";
import BadgeIcon from "@mui/icons-material/Badge";
import HouseIcon from "@mui/icons-material/House";
import LocalHospitalIcon from "@mui/icons-material/LocalHospital";
import AddAPhotoIcon from "@mui/icons-material/AddAPhoto";
import LockIcon from "@mui/icons-material/Lock";
import NoteAddIcon from "@mui/icons-material/NoteAdd";
import ListAltIcon from "@mui/icons-material/ListAlt";
import CheckCircleRoundedIcon from "@mui/icons-material/CheckCircleRounded";
import {
  Link,
  useNavigate,
  useLocation,
  Route,
  Routes,
} from "react-router-dom";
import { config } from "../../constants";
import { http } from "../../_services/http.service";
import i18n from "../../i18n/i18n";
import { useTranslation } from "react-i18next";
import { Button, Container, Modal } from "react-bootstrap";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import { Checkmark } from "react-checkmark";
import "../../CSS/Views/SurveyDetail.css"
import axios from "axios";

export const SurveyDetails = () => {
  const navigate = useNavigate();
  const User = JSON.parse(sessionStorage.getItem("LoginData"));
  if (User === "" || User == null) {
    sessionStorage.setItem("isLoggedIn", "Login");
    window.location.href = "/Login";
  } else {
    if (User.userRoles !== "Surveyor") {
      window.location.href = "/Common/ErrorPage"
    }
  }

  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Authorization": `Bearer ${sessionStorage.getItem("token")}`
    },

  });

  const [count, setcount] = useState(0);
  const { t } = useTranslation();
  const loc = useLocation()

  const location =
    loc.state !== null
      ? {
        state: {
          id: loc.state.id,
          name: loc.state.name,
          locale: loc.state.locale,
          origin: loc.state.origin,
          status: loc.state.status,
        },
      }
      : {
        state: {
          id: "",
          name: "",
          locale: "",
          origin: "",
          status: "",
        },
      };

  const mapperData = {
    surveyId: "",
    status: "",
    categoryId: "",
    id: "",
    created_Date: "",
    created_By: "",
    updated_Date: "",
    updated_By: ""
  }

  const [data, setdata] = useState([]);

  const [btn, setBtn] = useState(null);
  const [length, setLength] = useState(data.length);
  const [captureImage, setCaptureImage] = useState(0);
  const [personalDetails, setPersonalDetails] = useState(0);
  const [education, setEducation] = useState(0);
  const [employment, setEmployment] = useState(0);
  const [housing, setHousing] = useState(0);
  const [health, setHealth] = useState(0);
  const [socialsecurity, setSocialsecurity] = useState(0);
  const [additionalInfo, setAdditionalInfo] = useState(0);
  const [surveyStatus, setSurveyStatus] = useState(false);


  const fetchData = async () => {
   
    if (location.state.id && location.state.id.length > 0) {

      let uri = `${config.localUrl}SurveyMapper/GetMappedSurveyByID?Surveyid=${location.state.id}`;
      const mapperdata = await privateAxios.get(uri);
      // .then((res) => {
      //   debugger;

      //     setdata(res.data);
      //     setLength(res.data.length);      
      // }).catch((error) => {
      //   console.log(error);
      // });
    
      if (mapperdata.data.length > 0) {
        setLength(mapperdata.data.length);
        if (mapperdata.data.findIndex(item => item.categoryId === 76) >= 0) {
          setCaptureImage(1);
        }
        mapperdata.data.findIndex(item => item.categoryId === 69)
        if (mapperdata.data.findIndex(item => item.categoryId === 69) >= 0) {
          setPersonalDetails(1);
        }

        if (mapperdata.data.findIndex(item => item.categoryId === 70) >= 0) {
          setEducation(1);
        }

        if (mapperdata.data.findIndex(item => item.categoryId === 71) >= 0) {
          setEmployment(1);
        }

        if (mapperdata.data.findIndex(item => item.categoryId === 72) >= 0) {
          setHousing(1);
        }

        if (mapperdata.data.findIndex(item => item.categoryId === 73) >= 0) {
          setHealth(1);
        }

        if (mapperdata.data.findIndex(item => item.categoryId === 74) >= 0) {
          setSocialsecurity(1);
        }

        if (mapperdata.data.findIndex(item => item.categoryId === 75) >= 0) {
          setAdditionalInfo(1);
        }
      }

      let url = `${config.localUrl}BasicSurveyDetail/getBasicSurveyById?id=${location.state.id}`;
      const surveyData = await privateAxios.get(url);
      
      if (surveyData.data !== null) {
        if (surveyData.data.status === "Completed") {
          setSurveyStatus(true);
        }
      }

    }
  }

  if (location.state.id && location.state.id.length > 0) {

    fetchData();
  }

  // useEffect(() => {

  //   fetchData()

  // },[]);


  function getStatus(categoryId) {

    for (var json in data) {

      if (data[json].categoryId === categoryId) {
        return data[json];
      }
    }
    return { status: 0 };


  }

  function onSubmitHandler() {
    handleShow();
    let json = {};
    json.Name = location.state.name;
    json.SurveyId = location.state.id;
    json.Status = "Completed";
    const url = `${config.localUrl}BasicSurveyDetail/updateBasicSurvey`;
    let x = privateAxios.put(
      `${config.localUrl}BasicSurveyDetail/updateBasicSurvey`,
      json
    );
  }

  function navigator() {
    navigate("/Surveyor/SurveyList2");
  }

  const [show, setShow] = useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);

  return (
    <>
      <NavbarTitle data={{ Title: `${i18n.t("SurveyDetails")}` }}></NavbarTitle>

      <Modal show={show} onHide={navigator} centered>
        <Modal.Header closeButton>
          <Modal.Title>
            {i18n.t("SurveyCompleted")}
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Checkmark size="xxLarge" color="#42f55d" />
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={navigator}>
            {i18n.t("BackToDashboard")}
          </Button>
        </Modal.Footer>
      </Modal>
      {loc.state.origin === "SurveyList" ? (
        <>
          <nav aria-label="breadcrumb" className="mx-3">
            <ol class="breadcrumb">
              <li class="breadcrumb-item">
                <Link
                  to="/Surveyor/SurveyDashboard"
                  className="textDecoration"
                >
                  {i18n.t("Home")}
                </Link>
              </li>
              <li class="breadcrumb-item">
                <Link
                  to="/Surveyor/SurveyList2"
                  className="textDecoration"
                >
                  {i18n.t("surveyList")}
                </Link>
              </li>
              <li class="breadcrumb-item active" aria-current="page">
                {i18n.t("SurveyDetails")}
              </li>
            </ol>
          </nav>
        </>
      ) : (
        <>
          <nav aria-label="breadcrumb" className="mx-3">
            <ol class="breadcrumb">
              <li class="breadcrumb-item">
                <Link
                  to="/Surveyor/SurveyDashboard"
                  className="textDecoration"
                >
                  {i18n.t("Home")}
                </Link>
              </li>
              <li class="breadcrumb-item active" aria-current="page">
                {i18n.t("SurveyDashboard")}
              </li>
            </ol>
          </nav>
        </>
      )}

      <Container>
        {loc.state === null || loc.state.name.length <= 0 ? (
          <></>
        ) : (
          <>
            <div className="container d-flex mx-auto justify-content-center">
              <h5 className="m-3 header">
                {i18n.t("surveyee")}:{" "}
                <b className="text-primary">{location.state.id}</b>
              </h5>
              <h5 className="m-3 header">
                {i18n.t("surveyeeName")}:{" "}
                <b className="text-primary">{location.state.name}</b>
              </h5>
            </div>
          </>
        )}

        <div
          className="CardContainer"
        >
          <Link
            to={"/Surveyor/PersonalDetails"}
            className="textDecoration"
            state={{
              id: location.state.id,
              name: location.state.name,
              locale: location.state.locale,
              origin: location.state.origin,
              status: loc.state.status,
            }}
          >
            <CustomCard
              cardTitle={i18n.t("BasicSurvey")}
              backgroundColor="#636998"
              status={
                loc.state !== null && loc.state.name.length > 0 ? (
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
              cardNum = "one"
            />
          </Link>

          <Link
            to={
              loc.state === null || loc.state.name.length <= 0
                ? null
                : "/Surveyor/CaptureImage"
            }
            className={(loc.state === null || loc.state.name.length <= 0) ? "cursorAllow" : "textDecoration"}
            state={
              loc.state === null
                ? null
                : {
                  id: location.state.id,
                  name: location.state.name,
                  locale: location.state.locale,
                  origin: location.state.origin,
                  status: location.state.status,
                  showCamera: captureImage === 1 ? true : false,
                }
            }
          >
            <CustomCard
              cardNum = "two"
              cardTitle={i18n.t("CaptureImage")}
              // cardTitle={i18n.t("Dashboard2.additional")}
              backgroundColor={
                loc.state === null || loc.state.name.length <= 0
                  ? "rgba(255, 95, 88,0.6)"
                  : "rgba(255, 95, 88)"
              }
              status={
                captureImage === 1 ? (
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
            to={
              captureImage !== 1 ? null : "/Surveyor/PersonalDetails2"
            }
            className={captureImage !== 1 ? "cursorAllow" : "textDecoration"}
            state={
              loc.state === null
                ? null
                : {
                  id: location.state.id,
                  name: location.state.name,
                  locale: location.state.locale,
                  origin: location.state.origin,
                  status: loc.state.status,
                }
            }
          >
            <CustomCard
            cardNum = "three"
              cardTitle={i18n.t("Dashboard2.personal")}
              backgroundColor={
                captureImage !== 1
                  ? "rgba(80, 152, 167,0.6)"
                  : "rgba(80, 152, 167)"
              }
              status={
                personalDetails === 1 ? (
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
          <Link
            to={personalDetails !== 1 ? null : "/Surveyor/Education"}
            className={personalDetails !== 1 ? "cursorAllow" : "textDecoration"}

            state={
              loc.state === null
                ? null
                : {
                  id: location.state.id,
                  name: location.state.name,
                  locale: location.state.locale,
                  origin: location.state.origin,
                  status: loc.state.status,
                }
            }
          >
            <CustomCard
              cardNum = "four"
              cardTitle={i18n.t("Dashboard2.education")}
              backgroundColor={
                personalDetails !== 1
                  ? "rgba(99, 105, 152,0.6)"
                  : "rgba(99, 105, 152)"
              }
              status={
                education === 1 ? (
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
          <Link
            to={education !== 1 ? null : "/Surveyor/Employment"}
            className={education !== 1 ? "cursorAllow" : "textDecoration"}

            state={
              loc.state === null
                ? null
                : {
                  id: location.state.id,
                  name: location.state.name,
                  locale: location.state.locale,
                  origin: location.state.origin,
                  status: loc.state.status,
                }
            }
          >
            <CustomCard
              cardNum = "five"
              cardTitle={i18n.t("Dashboard2.employment")}
              backgroundColor={
                education !== 1
                  ? "rgba(98, 167, 108,0.6)"
                  : "rgba(98, 167, 108)"
              }
              status={
                employment === 1 ? (
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
        </div>
        <div
          className="CardContainer2"
        >
          <Link
            to={employment !== 1 ? null : "/Surveyor/Housing"}
            className={employment !== 1 ? "cursorAllow" : "textDecoration"}

            state={
              loc.state === null
                ? null
                : {
                  id: location.state.id,
                  name: location.state.name,
                  locale: location.state.locale,
                  origin: location.state.origin,
                  status: loc.state.status,
                }
            }
          >
            <CustomCard
              cardNum = "six"
              cardTitle={i18n.t("Dashboard2.housing")}
              backgroundColor={
                employment !== 1
                  ? "rgba(51,225,255,0.6)"
                  : "rgba(51,225,255)"
              }
              status={
                housing === 1 ? (
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
          <Link
            to={housing !== 1 ? null : "/Surveyor/Health"}
            className={housing !== 1 ? "cursorAllow" : "textDecoration"}

            state={
              loc.state === null
                ? null
                : {
                  id: location.state.id,
                  name: location.state.name,
                  locale: location.state.locale,
                  status: loc.state.status,
                  origin: location.state.origin,
                }
            }
          >
            <CustomCard
              cardNum="seven"
              cardTitle={i18n.t("Dashboard2.health")}
              backgroundColor={
                housing !== 1
                  ? "rgba(243, 202, 62,0.6)"
                  : "rgba(243, 202, 62)"
              }
              status={
                health === 1 ? (
                  <CheckCircleRoundedIcon
                    className="checked"
                  ></CheckCircleRoundedIcon>
                ) : null
              }
              cardImg={
                <LocalHospitalIcon
                  className="icon"
                ></LocalHospitalIcon>
              }
            />
          </Link>
          <Link
            to={health !== 1 ? null : "/Surveyor/SocialSecurity"}
            className={health !== 1 ? "cursorAllow" : "textDecoration"}

            state={
              loc.state === null
                ? null
                : {
                  id: location.state.id,
                  name: location.state.name,
                  locale: location.state.locale,
                  origin: location.state.origin,
                  status: loc.state.status,
                }
            }
          >
            <CustomCard
              cardNum = "eight"
              cardTitle={i18n.t("Dashboard2.social")}
              backgroundColor={
                health !== 1
                  ? "rgba(128, 139, 222,0.6)"
                  : "rgba(128, 139, 222)"
              }
              status={
                socialsecurity === 1 ? (
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
            to={
              socialsecurity !== 1 ? null : "/Surveyor/AdditionalDetails"
            }
            className={socialsecurity !== 1 ? "cursorAllow" : "textDecoration"}

            state={
              loc.state === null
                ? null
                : {
                  id: location.state.id,
                  name: location.state.name,
                  locale: location.state.locale,
                  origin: location.state.origin,
                  status: loc.state.status,
                }
            }
          >
            <CustomCard
              cardNum = "nine"
              cardTitle={i18n.t("Dashboard2.additional")}
              backgroundColor={
                socialsecurity !== 1
                  ? "rgba(255, 95, 88,0.6)"
                  : "rgba(255, 95, 88)"
              }
              status={
                additionalInfo === 1 ? (
                  <CheckCircleRoundedIcon
                    className="checked"
                  ></CheckCircleRoundedIcon>
                ) : null
              }
              cardImg={
                <NoteAddIcon
                  className="icon"
                ></NoteAddIcon>
              }
            />
          </Link>
        </div>
        {<>
          <div className="col-md-12 d-flex justify-content-center m-4">

            <button
             
              className= { length !== 8 ? "text-light notAllowedSubmit" : "text-light submitButton"}
              type="submit"
              disabled={length === 8 ? false : true}
              onClick={onSubmitHandler}
            >

              {i18n.t("Submit")}
            </button>
          </div>
        </>}





      </Container>
    </>
  );
};
