import React, { useState } from "react";
import "../../CSS/Views/Info.css";
import BottomNavigation from "../BottomNavigation";
import Questiones from "../../CommonComponent/Questiones";
import { Link, useNavigate } from "react-router-dom";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import { Form } from "react-bootstrap";
import { config } from "../../constants";
import { useForm } from "react-hook-form";
import StorageFactory from "../../_services/StorageFactory";
import { useLocation } from "react-router-dom";
import i18n from "../../i18n/i18n";
import { useTranslation } from "react-i18next";
import Accordian from "../../CommonComponent/Accordian";
import { useEffect } from "react";
import "../../CSS/BreadCrumb.css";
import axios from "axios";


const questions = require("../../Questions/Question.json");

const ChildQuestion = [
  {
    PId: 2350,
    CId: 2351,
    Type: "No",
    disable: true,
  },
  {
    PId: 2258,
    CId: 2352,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2353,
    CId: 2265,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2354,
    CId: 2266,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2464,
    CId: 2465,
    Type: "No",
    disable: true,
  },
  {
    PId: 2164,
    CId: 2466,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2467,
    CId: 2171,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2470,
    CId: 2172,
    Type: "Others",
    disable: true,
  },
];

export const Additionalinformation = () => {
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
  const { register, handleSubmit, reset } = useForm();
  const location = useLocation();
  const [Model, setModel] = useState([]);
  const [useId, setuseId] = useState({});
  const { t } = useTranslation();
  const [statusCode, setStatusCode] = useState(0);

  var Questions;
  var AdditionalInformation;

  if (AdditionalInformation == null) {
    if (localStorage.getItem("Questions") != "") {
      Questions = localStorage.getItem("Questions");
      Questions = JSON.parse(Questions);
    } else {
      Questions = questions;
    }

    if (location.state.locale == "en-US" || location.state.locale == "en") {
      AdditionalInformation = Questions.find((item) => item.Id == 80);
    } else {
      AdditionalInformation = Questions.find((item) => item.Id == 82);
    }
  }
  function sortByDisplaySequence(x, y) {
    return x.DisplaySequence > y.DisplaySequence ? 1 : -1;
  }

  AdditionalInformation.QuestionPaperQuestions.sort(sortByDisplaySequence);

  const model = [];
  async function setModelState() {
    AdditionalInformation.QuestionPaperQuestions.map((Qn) => {
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
    setModelState();
    fetchData();
  }, []);

  const fetchData = async () => {
    const url = `${config.localUrl}Survey/GetSurveyDetailByid?id=${location.state.id}`;
    privateAxios
      .get(url)
      .then(function (res) {
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

  const onSubmitHandler = (data) => {
    setStatusCode(1)
    const surveyMapper = {
      surveyId: location.state.id,
      status: 1,
      CategoryId: 75,
    };

    StorageFactory.postMapperInfo(config, surveyMapper);
    StorageFactory.StoreData(
      data,
      multiselect,
      location.state.id,
      config,
      AdditionalInformation.Id,
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
    navigate("/Surveyor/SurveyDetails", {
      state: { id: location.state.id, name: location.state.name },
    });
  }

  return (
    <>
      <NavbarTitle data={{ Title: i18n.t("additional") }}></NavbarTitle>
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
                {i18n.t("additional")}
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
                      },
                    });
                  }}
                  href
                >
                  {i18n.t("SurveyDashboard")}
                </a>
              </li>

              <li class="breadcrumb-item active" aria-current="page">
                {i18n.t("additional")}
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
        <Accordian title={i18n.t("additional")}>
          <Form onSubmit={handleSubmit(onSubmitHandler)}>
            <Questiones
              api={Model}
              register={register}
              reset={reset}
              setData={setData}
              setuseId={setuseId}
              locale={location.state.locale}
              useId={useId}
              data={multiselect}
              childQuestion={ChildQuestion}
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
};

export default Additionalinformation;
