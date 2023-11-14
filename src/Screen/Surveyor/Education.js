import React, { useState } from "react";
import { Form } from "react-bootstrap";
import BottomNavigation from "../BottomNavigation";
import Questiones from "../../CommonComponent/Questiones";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import { config } from "../../constants";
import { useForm } from "react-hook-form";
import StorageFactory from "../../_services/StorageFactory";
import { Link, useLocation, useNavigate } from "react-router-dom";
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
    PId: 2380,
    CId: 2381,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2024,
    CId: 2382,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2383,
    CId: 2026,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2383,
    CId: 2027,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2383,
    CId: 2044,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2384,
    CId: 2030,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2030,
    CId: 2045,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2385,
    CId: 2386,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2388,
    CId: 2389,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2391,
    CId: 2048,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2036,
    CId: 2049,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2037,
    CId: 2050,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2038,
    CId: 2051,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2471,
    CId: 2472,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2088,
    CId: 2473,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2474,
    CId: 2090,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2474,
    CId: 2091,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2474,
    CId: 2108,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2475,
    CId: 2094,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2094,
    CId: 2109,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2476,
    CId: 2477,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2479,
    CId: 2480,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2482,
    CId: 2112,
    Type: "Yes",
    disable: true,
  },
  {
    PId: 2100,
    CId: 2113,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2101,
    CId: 2114,
    Type: "Others",
    disable: true,
  },
  {
    PId: 2102,
    CId: 2115,
    Type: "Others",
    disable: true,
  },
];

export const Education = () => {
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
  var Education;

  if (Education == null) {
    if (localStorage.getItem("Questions") !== "") {
      Questions = localStorage.getItem("Questions");
      Questions = JSON.parse(Questions);
    } else {
      // Questions = questions;
    }

    if (location.state.locale == "en-US" || location.state.locale == "en") {
      Education = Questions.find((item) => item.Id == 75);
    } else {
      Education = Questions.find((item) => item.Id == 77);
    }
  }

  function sortByDisplaySequence(x, y) {
    return x.DisplaySequence > y.DisplaySequence ? 1 : -1;
  }
  Education.QuestionPaperQuestions.sort(sortByDisplaySequence);

  const model = [];
  function setModelState() {
    Education.QuestionPaperQuestions.map((Qn) => {
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
    fetchData()
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
      CategoryId: 70,
    };

    StorageFactory.postMapperInfo(config, surveyMapper);
    StorageFactory.StoreData(
      data,
      multiselect,
      location.state.id,
      config,
      Education.Id,
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
      <NavbarTitle data={{ Title: i18n.t("education") }}></NavbarTitle>

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
                {i18n.t("education")}
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
                {i18n.t("education")}
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
        <Accordian title={i18n.t("education")}>
          <Form onSubmit={handleSubmit(onSubmitHandler)}>
            <Questiones
              register={register}
              reset={reset}
              setuseId={setuseId}
              useId={useId}
              api={Model}
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
};

export default Education;
