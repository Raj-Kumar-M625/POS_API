import React, { useEffect, useState } from "react";
import Questiones from "../../CommonComponent/Questiones";
import BottomNavigation from "../BottomNavigation";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import { useForm } from "react-hook-form";
import { config } from "../../constants";
import StorageFactory from "../../_services/StorageFactory";
import { Link, useLocation, useNavigate } from "react-router-dom";
import i18n from "../../i18n/i18n";
import { useTranslation } from "react-i18next";
import Accordian from "../../CommonComponent/Accordian";
import { Form } from "react-bootstrap";
import axios from "axios";
import spinner from "../../Assets/Image/spinner.gif";
import "../../CSS/BreadCrumb.css";

const questions = require("../../Questions/Question.json");
const ChildQuestion = [
  {
    PId: 1992,
    CId: 1993,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 1992,
    CId: 1994,
    Type: "No",
    disable: true,
  }, {
    PId: 2002,
    CId: 3452,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2003,
    CId: 2021,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2054,
    CId: 2055,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2054,
    CId: 2056,
    Type: "No",
    disable: true,
  },
  {
    PId: 2064,
    CId: 2086,
    Type: "Yes",
    disable: true,
  }, {
    PId: 2063,
    CId: 3453,
    Type: "Others",
    disable: true,
  }
];

function sortByDisplaySequence(x, y) {
  return x.DisplaySequence > y.DisplaySequence ? 1 : -1;
}

function PersonalDetailsScreen2() {
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
      "Content-type": "application/json",
      "Authorization": `Bearer ${sessionStorage.getItem("token")}`
    },

  });
  const [multiselect, setData] = useState({});
  const { register, handleSubmit, reset, unregister } = useForm();
  const { t } = useTranslation();
  const location = useLocation();
  const [useId, setuseId] = useState({});
  const [Model, setModel] = useState([]);
  const [statusCode, setStatusCode] = useState(0)
  var Questions;
  var PersonalDetails;

  if (PersonalDetails == null) {
    if (localStorage.getItem("Questions") !== "") {
      Questions = localStorage.getItem("Questions");
      Questions = JSON.parse(Questions);
    } else {
      PersonalDetails = questions;
    }
    if (location.state.locale == "en-US" || location.state.locale == "en") {
      PersonalDetails = Questions.find((item) => item.Id === 74);
    } else {
      PersonalDetails = Questions.find((item) => item.Id === 76);
    }
    PersonalDetails.QuestionPaperQuestions.sort(sortByDisplaySequence);
  }

  const model = [];
  function setModelState() {
    PersonalDetails.QuestionPaperQuestions.map((Qn) => {
      model.push({
        QuestionPaperId: Qn.QuestionPaperId,
        Id: Qn.Id,
        AnswerText: "",
        QText: Qn.QText,
        QuestionTypeName: Qn.QuestionTypeName,
        QuestionPaperAnswers: Qn.QuestionPaperAnswers,
        IsMandatory: Qn.IsMandatory,
      });
    });
  }

  useEffect(() => {
    setModelState()
    fetchData()
  }, []);

  const fetchData = async () => {
    const url = `${config.localUrl}Survey/GetSurveyDetailByid?id=${location.state.id}`;
    await privateAxios
      .get(url)
      .then(function (res) {
        debugger
        for (var json in res.data) {
          for (let i = 0; i < model.length; i++) {
            if (model[i].Id === res.data[json].questionId) {
              model[i].AnswerText = res.data[json].answer;
            }
          }
        }
        setModel([...model]);
      })
      .catch((error) => {
        console.log(error);
      });
  }

  function onSubmitHandler(data) {
    debugger
    setStatusCode(1)
    const surveyMapper = {
      surveyId: location.state.id,
      status: 1,
      CategoryId: 69,
    };

    StorageFactory.postMapperInfo(config, surveyMapper);

    const res = StorageFactory.StoreData(
      data,
      multiselect,
      location.state.id,
      config,
      PersonalDetails.Id,
      User
    ).then(() => {
      debugger;
      setStatusCode(0)
      navigate("/Surveyor/SurveyDetails", {
        state: {
          id: location.state.id,
          name: location.state.name,
          locale: location.state.locale,
          origin: location.state.origin,
          status: location.state.status,
        },
      });
    })




  }

  function backNavigator() {
    debugger;
    navigate("/Surveyor/SurveyDetails", {
      state: { id: location.state.id, name: location.state.name },
    });
  }
  return (
    <>
      <NavbarTitle data={{ Title: i18n.t("personal") }}></NavbarTitle>

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
                    status: location.state.status,
                  }}
                >
                  {i18n.t("SurveyDetails")}
                </Link>
              </li>

              <li class="breadcrumb-item active" aria-current="page">
                {i18n.t("personal")}
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
                  href
                  
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
                        status: location.state.status,
                      },
                    });
                  }}
                  href
                >
                  {i18n.t("SurveyDashboard")}
                </a>
              </li>

              <li class="breadcrumb-item active" aria-current="page">
                {i18n.t("personal")}
              </li>
            </ol>
          </nav>
        </>
      )}
      <div className="container">
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

        <Accordian title={i18n.t("personal")}>
          <Form onSubmit={handleSubmit(onSubmitHandler)} className="">
            <Questiones
              // api={PersonalDetails.QuestionPaperQuestions}
              api={Model}
              register={register}
              Model={Model}
              locale={location.state.locale}
              reset={reset}
              setData={setData}
              data={multiselect}
              childQuestion={ChildQuestion}
              unregister={unregister}
            />
            <BottomNavigation
              statusCode={statusCode}
            />
            {/* <div className="container col-md-12 d-flex justify-content-end mt-3">
              <button className="btn btn-success mx-3" type="submit">
                {statusCode === 0 ? i18n.t('Save') : <>
                  <img src={spinner} width="30px" height="24px" alt="spin" />
                </>}
              </button>

            </div> */}
          </Form>
        </Accordian>
      </div>
    </>
  );
}

export default PersonalDetailsScreen2;
