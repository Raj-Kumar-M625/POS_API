import React, { useEffect, useState } from "react";
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
import "../../CSS/BreadCrumb.css";
import axios from "axios";
import spinner from "../../Assets/Image/spinner.gif";

const questions = require("../../Questions/Question.json");

const ChildQuestion = [
  {
    PId: 2392,
    CId: 2393,
    Type: "Others",
    disable: true,
  },
  {
    PId: 1869,
    CId: 2394,
    Type: "Others",
    disable: true,
  },
  {
    PId: 1870,
    CId: 1881,
    Type: "Others",
    disable: true,
  },
  {
    PId: 1872,
    CId: 2397,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2398,
    CId: 2399,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2400,
    CId: 2401,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2403,
    CId: 1888,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 1877,
    CId: 1889,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2440,
    CId: 2441,
    Type: "Others",
    disable: true,
  },
  {
    PId: 1929,
    CId: 2442,
    Type: "Others",
    disable: true,
  },
  {
    PId: 1930,
    CId: 1941,
    Type: "Others",
    disable: true,
  },
  {
    PId: 1932,
    CId: 2445,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2446,
    CId: 2447,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2448,
    CId: 2449,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2451,
    CId: 1948,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 1937,
    CId: 1949,
    Type: "Others",
    disable: true,
  },
];

function Employment() {
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
  const [Model, setModel] = useState([]);
  const location = useLocation();
  const [useId, setuseId] = useState({});
  const { t } = useTranslation();
  const [statusCode, setStatusCode] = useState(0);

  var Questions;
  var Employment;

  if (Employment == null) {
    if (localStorage.getItem("Questions") != "") {
      Questions = localStorage.getItem("Questions");
      Questions = JSON.parse(Questions);
    } else {
      Questions = questions;
    }
    if (location.state.locale == "en-US" || location.state.locale == "en") {
      Employment = Questions.find((item) => item.Id == 70);
    } else {
      Employment = Questions.find((item) => item.Id == 72);
    }
  }

  function sortByDisplaySequence(x, y) {
    return x.DisplaySequence > y.DisplaySequence ? 1 : -1;
  }
  Employment.QuestionPaperQuestions.sort(sortByDisplaySequence);

  const model = [];
  function setModelState() {
    Employment.QuestionPaperQuestions.map((Qn) => {
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
    setStatusCode(1);
    const surveyMapper = {
      surveyId: location.state.id,
      status: 1,
      CategoryId: 71,
    };

    StorageFactory.postMapperInfo(config, surveyMapper);
    StorageFactory.StoreData(
      data,
      multiselect,
      location.state.id,
      config,
      Employment.Id,
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
      <NavbarTitle data={{ Title: i18n.t("employment") }}></NavbarTitle>
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
                {i18n.t("employment")}
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
                {i18n.t("employment")}
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
        <Accordian title={i18n.t("employment")}>
          <Form onSubmit={handleSubmit(onSubmitHandler)}>
            <Questiones
              register={register}
              api={Model}
              reset={reset}
              useId={useId}
              setuseId={setuseId}
              locale={location.state.locale}
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

export default Employment;
