import React, { useState } from "react";
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
import spinner from "../../Assets/Image/spinner.gif";

const questions = require("../../Questions/Question.json");

const ChildQuestion = [
  {
    PId: 2355,
    CId: 2356,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2356,
    CId: 2290,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2357,
    CId: 2358,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2269,
    CId: 2292,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2270,
    CId: 2293,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2367,
    CId: 2294,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2360,
    CId: 2361,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2362,
    CId: 2363,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2364,
    CId: 2365,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2365,
    CId: 2366,
    Type: "Agriculture",
    disable: true,
  },
  {
    PId: 2365,
    CId: 2278,
    Type: "Agriculture",
    disable: true,
  },
  {
    PId: 2365,
    CId: 2298,
    Type: "Commercial",
    disable: true,
  },
  {
    PId: 2364,
    CId: 2297,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2279,
    CId: 2299,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2280,
    CId: 2300,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2284,
    CId: 2301,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2286,
    CId: 2287,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2287,
    CId: 2302,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2289,
    CId: 2303,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2368,
    CId: 2369,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2369,
    CId: 1973,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2370,
    CId: 2371,
    Type: "Others",
    disable: true,
  },
  {
    PId: 1952,
    CId: 1975,
    Type: "Others",
    disable: true,
  },
  {
    PId: 1953,
    CId: 1976,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2372,
    CId: 1977,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2373,
    CId: 2374,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2375,
    CId: 2376,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2377,
    CId: 2378,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2377,
    CId: 1980,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2378,
    CId: 2379,
    Type: "Agriculture",
    disable: true,
  },
  {
    PId: 2378,
    CId: 1961,
    Type: "Agriculture",
    disable: true,
  },
  {
    PId: 2378,
    CId: 1981,
    Type: "Commercial",
    disable: true,
  },
  {
    PId: 1962,
    CId: 1982,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 1963,
    CId: 1984,
    Type: "Others",
    disable: true,
  },
  {
    PId: 1967,
    CId: 1983,
    Type: "Others",
    disable: true,
  },
  {
    PId: 1969,
    CId: 1970,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 1970,
    CId: 1985,
    Type: "Others",
    disable: true,
  },
  {
    PId: 1972,
    CId: 1986,
    Type: "Others",
    disable: true,
  },
];

function Housing() {
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
  const { register, handleSubmit, reset, unregister, getValues } = useForm();
  const [Model, setModel] = useState([]);
  const { t } = useTranslation();
  const location = useLocation();
  const [statusCode, setStatusCode] = useState(0);
  debugger;

  var Questions;
  var Housing;
  if (Housing == null) {
    if (localStorage.getItem("Questions") != "") {
      Questions = localStorage.getItem("Questions");
      Questions = JSON.parse(Questions);
    } else {
      Questions = questions;
    }

    if (location.state.locale === "en-US" || location.state.locale === "en") {
      Housing = Questions.find((item) => item.Id === 71);
    } else {
      Housing = Questions.find((item) => item.Id === 73);
    }
  }

  function sortByDisplaySequence(x, y) {
    return x.DisplaySequence > y.DisplaySequence ? 1 : -1;
  }
  Housing.QuestionPaperQuestions.sort(sortByDisplaySequence);

  const model = [];
  function setModelState() {
    Housing.QuestionPaperQuestions.map((Qn) => {
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
      CategoryId: 72,
    };
    StorageFactory.postMapperInfo(config, surveyMapper);
    StorageFactory.StoreData(
      data,
      multiselect,
      location.state.id,
      config,
      Housing.Id,
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
      <NavbarTitle data={{ Title: i18n.t("housing") }}></NavbarTitle>
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
                {i18n.t("housing")}
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
                        status: location.state.status,
                        origin: location.state.origin,
                      },
                    });
                  }}
                  href
                >
                  {i18n.t("SurveyDashboard")}
                </a>
              </li>

              <li class="breadcrumb-item active" aria-current="page">
                {i18n.t("housing")}
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
        <Accordian title={i18n.t("housing")}>
          <Form onSubmit={handleSubmit(onSubmitHandler)}>
            <Questiones
              register={register}
              api={Model}
              reset={reset}
              setData={setData}
              locale={location.state.locale}
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

export default Housing;
