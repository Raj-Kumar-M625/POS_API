import React, { useState } from "react";
import BottomNavigation from "../BottomNavigation";
import Questiones from "../../CommonComponent/Questiones";
import { Link, useNavigate } from "react-router-dom";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import { Form } from "react-bootstrap";
import { useForm } from "react-hook-form";
import StorageFactory from "../../_services/StorageFactory";
import { config } from "../../constants";
import { useLocation } from "react-router-dom";
import i18n from "../../i18n/i18n";
import { useTranslation } from "react-i18next";
import Accordian from "../../CommonComponent/Accordian";
import { useEffect } from "react";
//import { privateAxios } from "../../context/Axios";
import "../../CSS/BreadCrumb.css";
import axios from "axios";
import spinner from "../../Assets/Image/spinner.gif";

const questions = require("../../Questions/Question.json");

const ChildQuestion = [
  {
    PId: 2404,
    CId: 2405,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2309,
    CId: 2406,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2312,
    CId: 2343,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2312,
    CId: 2329,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2312,
    CId: 2408,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2312,
    CId: 2331,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2312,
    CId: 2313,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2313,
    CId: 2409,
    Type: "No",
    disable: true,
  },
  {
    PId: 2313,
    CId: 2410,
    Type: "No",
    disable: true,
  },
  {
    PId: 2410,
    CId: 2411,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2412,
    CId: 2413,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2412,
    CId: 2414,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2414,
    CId: 2415,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2319,
    CId: 2320,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2320,
    CId: 2336,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2320,
    CId: 2337,
    Type: "Physical",
    disable: true,
  },
  {
    PId: 2320,
    CId: 2338,
    Type: "Mental",
    disable: true,
  },
  {
    PId: 2322,
    CId: 2339,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2323,
    CId: 2324,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2323,
    CId: 2340,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2325,
    CId: 2341,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2326,
    CId: 2342,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2452,
    CId: 2453,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2220,
    CId: 2454,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2223,
    CId: 2344,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2223,
    CId: 2240,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2223,
    CId: 2456,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2223,
    CId: 2242,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2223,
    CId: 2224,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2224,
    CId: 2457,
    Type: "No",
    disable: true,
  },
  {
    PId: 2224,
    CId: 2458,
    Type: "No",
    disable: true,
  },
  {
    PId: 2458,
    CId: 2459,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2460,
    CId: 2461,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2460,
    CId: 2462,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2462,
    CId: 2463,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2230,
    CId: 2231,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2231,
    CId: 2247,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2231,
    CId: 2248,
    Type: "Physical",
    disable: true,
  },
  {
    PId: 2231,
    CId: 2249,
    Type: "Mental",
    disable: true,
  },
  {
    PId: 2233,
    CId: 2250,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2234,
    CId: 2235,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2234,
    CId: 2251,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2236,
    CId: 2252,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2237,
    CId: 2253,
    Type: "Yes",
    disable: true,
  },
];

function Health() {
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

  var Questions;
  var Health;

  if (Health == null) {
    if (localStorage.getItem("Questions") != "") {
      Questions = localStorage.getItem("Questions");
      Questions = JSON.parse(Questions);
    } else {
      Questions = questions;
    }

    if (location.state.locale == "en-US" || location.state.locale == "en") {
      Health = Questions.find((item) => item.Id == 78);
    } else {
      Health = Questions.find((item) => item.Id == 83);
    }
  }

  function sortByDisplaySequence(x, y) {
    return x.DisplaySequence > y.DisplaySequence ? 1 : -1;
  }
  Health.QuestionPaperQuestions.sort(sortByDisplaySequence);

  const model = [];
  function setModelState() {
    Health.QuestionPaperQuestions.map((Qn) => {
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
      CategoryId: 73,
    };

    StorageFactory.postMapperInfo(config, surveyMapper);
    StorageFactory.StoreData(
      data,
      multiselect,
      location.state.id,
      config,
      Health.Id,
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
      <NavbarTitle data={{ Title: i18n.t("health") }}></NavbarTitle>
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
                {i18n.t("health")}
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
                {i18n.t("health")}
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
        <Accordian title={i18n.t("health")}>
          <Form onSubmit={handleSubmit(onSubmitHandler)}>
            <Questiones
              api={Model}
              register={register}
              setData={setData}
              locale={location.state.locale}
              reset={reset}
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

export default Health;
