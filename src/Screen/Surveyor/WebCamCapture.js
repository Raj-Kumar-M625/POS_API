import React, { useEffect, useRef, useState } from "react";
import Camera from "./Camera";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { config } from "../../constants";
import i18n from "../../i18n/i18n";
import { useTranslation } from "react-i18next";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import StorageFactory from "../../_services/StorageFactory";
import axios from "axios";



function WebCamCapture() {
  const navigate = useNavigate();
  const User = JSON.parse(sessionStorage.getItem("LoginData"));
  if (User === "" || User == null) {
    sessionStorage.setItem("isLoggedIn", "Login");
    window.location.href = "/Login";
  }else{
    if(User.userRoles !== "Surveyor"){
     window.location.href = "/Common/ErrorPage"
    }
  }

  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Authorization": `Bearer ${sessionStorage.getItem("token")}`
    },

  });
  const location = useLocation();
  const [surveyee, setsurveyee] = useState();
  const [currentCapture, setcurrentCapture] = useState();
  const [imgArray, setimgArray] = useState([]);
  const [statusCode, setstatusCode] = useState(0)
  const imgRef1 = useRef();
  const imgRef2 = useRef();

  const [bool, setbool] = useState(false);
  const [imgSrc, setimgSrc] = useState("");
  const [data, setdata] = useState();
  const { t } = useTranslation();
  const show = (user) => {
    setcurrentCapture(user);
    setbool(true);
  };

  const onSubmitHandler = async () => {
    debugger;
    setstatusCode(1)
    const cameraInfo = [
      {
        BasicSurveyId: parseInt(location.state.id.match(/(\d+)/)),
        surveyId: location.state.id,
        Image: imgArray[0],
        userId: User == null ? "" : User.userId,
      },
      {
        BasicSurveyId: parseInt(location.state.id.match(/(\d+)/)),
        surveyId: location.state.id,
        Image: imgArray[1],
        userId: User == null ? "" : User.userId,
      },
    ];

    const surveyMapper = {
      surveyId: location.state.id,
      status: 1,
      CategoryId: 76,
    };

    StorageFactory.postMapperInfo(config, surveyMapper);

    await privateAxios.post(`${config.localUrl}SurveyImages/addImage`, cameraInfo).then((res) => {
      debugger;
      console.log(res);
      setimgArray([]);     
      setstatusCode(0)
      navigate("/Surveyor/SurveyDetails", {
        state: {
          id: location.state.id,
          name: location.state.name,
          locale: location.state.locale,
          origin: location.state.origin,
          status: location.state.status
        },
      });
    }).catch((error) => {
      debugger;
      console.log(error);
    });

  }
  useEffect(() => {
    fetchImage()
  }, []);
  

  const fetchImage = async() => {
    debugger;
    const url = `${config.localUrl}SurveyImages/GetSurveyImagesById?surveyId=${location.state.id}`;
    privateAxios.get(url)
      .then((res) => {
        debugger;
        return res.data;
      })
      .then((response) => {
        debugger;
        setdata(response);
      });
    if (imgRef1.current) {
      imgRef1.current.src = imgArray[0];
      imgRef2.current.src = imgArray[1];
    }
  }
  return (
    <>
      <NavbarTitle data={{ Title: `${i18n.t("CaptureImage")}` }}></NavbarTitle>
      {location.state.origin === "SurveyList" ? (
        <>
          <nav aria-label="breadcrumb" className="mx-3">
            <ol className="breadcrumb">
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
              <li class="breadcrumb-item">
                <Link
                  to="/Surveyor/SurveyDetails"
                  className="textDecoration"
                  state={{
                    id: location.state.id,
                    name: location.state.name,
                    locale: location.state.locale,
                    origin: location.state.origin,
                    status: location.state.status
                  }}
                >
                  {i18n.t("SurveyDetails")}
                </Link>
              </li>

              <li class="breadcrumb-item active" aria-current="page">
                {i18n.t("CaptureImage")}
              </li>
            </ol>
          </nav>
        </>
      ) : (
        <>
          <nav aria-label="breadcrumb" className="mx-3">
            <ol class="breadcrumb">
              <li class="breadcrumb-item">
                <a
                  className="text-primary textDecoration"
                  onClick={() => {
                    navigate("/Surveyor/SurveyDashboard");
                  }}
                  href="/Surveyor/SurveyDashboard"
                >
                  {i18n.t("Home")}
                </a>
              </li>
              <li class="breadcrumb-item">
                <a
                  className="text-primary textDecoration"
                  onClick={() => {
                    navigate("/Surveyor/SurveyDetails", {
                      state: {
                        id: location.state.id,
                        name: location.state.name,
                        locale: location.state.locale,
                        origin: location.state.origin,
                        status: location.state.status
                      },
                    });
                  }}
                  
                  href
                >
                  {i18n.t("SurveyDashboard")}
                </a>
              </li>

              <li class="breadcrumb-item active" aria-current="page">
                {i18n.t("CaptureImage")}
              </li>
            </ol>
          </nav>
        </>
      )}

      {location.state === null ? (
        <></>
      ) : (
        <>
          <div className="container d-flex mx-auto justify-content-center">
            <h5 className="m-2 header">
              {i18n.t("surveyee")}:{" "}
              <b className="text-primary">{location.state.id}</b>
            </h5>
            <h5 className="m-2 header">
              {i18n.t("surveyeeName")}:{" "}
              <b className="text-primary">{location.state.name}</b>
            </h5>
          </div>
        </>
      )}

      {/* web cam start */}

      <div className="d-flex flex-column justify-content-center align-items-center">
        {location.state.showCamera === false ? (
          <div >
            <div className="container">
              <Camera
                bool={setbool}
                setimgArray={setimgArray}
                imgArray={imgArray}
                setimgSrc={setimgSrc}
                currentCapture={currentCapture}
                setsurveyee={setsurveyee}
                onSubmitHandler={onSubmitHandler}
                statusCode={statusCode}
              />
            </div>

            <div className="row mt-3 d-flex justify-content-center showImage">
              <div className="col-md-6 d-flex flex-column justify-content-center align-items-center">
                <img
                  src={data == null || data === undefined || data.length <= 0
                    ? imgArray[0] : data[0].image }
                  ref={imgRef1}
                  width="200px"
                  height="140px"
                  className="mx-1"
                  alt="surveyee"
                />
                <h6 className="text-center">{i18n.t("Surveyee")}</h6>
              </div>
              <div className="col-md-6 d-flex flex-column justify-content-center align-items-center">
                <img
                  src={data == null || data === undefined || data.length <= 0
                    ? imgArray[1] : data[1].image}
                  ref={imgRef2}
                  width="200px"
                  height="140px"
                  className="mx-1"
                  alt="surveyorWithSurveyee"
                />
                <h6 className="text-center">{i18n.t("SurveyeeWithSurveyor")}</h6>
              </div>
            </div>
          </div>
        ) : (
          <>
            <div className="row mt-3 d-flex  justify-content-center align-items-center showImage">
              <div className="col-md-6">
                <img
                  src={
                    data == null || data == undefined || data.length <= 0
                      ? ""
                      : data[0].image
                  }
                  width="283px"
                  height="200px"
                  alt=""
                />
                <h6 className="text-center">{i18n.t("Surveyee")}</h6>
              </div>
              <div className="col-md-6">
                <img
                  src={
                    data == null || data == undefined || data.length <= 0
                      ? ""
                      : data[1].image
                  }
                  width="283px"
                  height="200px"
                  alt=""
                />
                <h6 className="text-center">{i18n.t("SurveyeeWithSurveyor")}</h6>
              </div>
            </div>
          </>
        )}

        {/* web cam end  */}
      </div>
    </>
  );
}

export default WebCamCapture;
