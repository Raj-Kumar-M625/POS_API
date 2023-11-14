import React, { useState } from "react";
import "../../CSS/Views/SocialSecurity.css";
import { Form } from "react-bootstrap";
import SurevyInfo_BottomNavigation from "./SurveyInfo_BottomNavigation";
import SurveyInfo_Questiones from "./SurveyInfo_Questiones";
import { Link, useLocation, useNavigate, useParams } from "react-router-dom";
import { useForm } from "react-hook-form";
import StorageFactory from "../../_services/StorageFactory"
import i18n from "../../i18n/i18n";
import { useTranslation } from "react-i18next";
import { AdminNavbar } from "../../CommonComponent/AdminNavbar";
import Accordian from "../../CommonComponent/Accordian";
import { useEffect } from "react";
import { config } from "../../constants";
//import { privateAxios } from "../../context/Axios";
import "../../CSS/BreadCrumb.css";
import axios from "axios";

const questions = require("../../Questions/Question.json");


function SurveyInfo_SocialSecurity() {
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
  const { register, handleSubmit } = useForm();
  const location = useLocation();
  const params = useParams();
  const [Model, setModel] = useState([]);
  const [multiselect, SetData] = useState({});
  const { t } = useTranslation();

  function onSubmitHandler(data) {
    StorageFactory.UpdateData(data, multiselect, params.SurveyId)
    navigate(`/Surveyor/SurveyDetails/${params.SurveyId}/${params.Name}`);
  }

  var Questions;
  var SocialSecurity;

  if (SocialSecurity == null) {
    if (localStorage.getItem("Questions") != "") {
      Questions = localStorage.getItem("Questions");
      Questions = JSON.parse(Questions);
    } else {
      Questions = questions;
    }

    if (location.state.locale == "en-US" || location.state.locale == "en") {
      SocialSecurity = Questions.find((item) => item.Id == 79);
    } else {
      SocialSecurity = Questions.find((item) => item.Id == 81);
    }
  }

  function sortByDisplaySequence(x, y) {
    return x.DisplaySequence > y.DisplaySequence ? 1 : -1;
  }
  SocialSecurity.QuestionPaperQuestions.sort(sortByDisplaySequence);

  const model = [];
  async function setModelState() {
    SocialSecurity.QuestionPaperQuestions.map((Qn) => {
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
    const url = `${config.localUrl}Survey/GetSurveyDetailByid?id=${params.SurveyId}`;
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

  function backNavigator() {
    navigate(`/Surveyor/Survey/SurveyDetails/${params.SurveyId}/${params.Name}`, { state: location.state });

  }

  return (
    <>
      <AdminNavbar data={{ Title: `${i18n.t("social")}` }}></AdminNavbar>
      <nav aria-label="breadcrumb" className="mx-3">
        {location.state.origin === "Surveyor" ? <ol className="breadcrumb">
          <li class="breadcrumb-item"><Link to="/Surveyor/SurveyDashboard" className="textDecoration">{i18n.t("Home")}</Link></li>
          <li class="breadcrumb-item"><Link to="/Surveyor/SurveyList2" className="textDecoration">{i18n.t("surveyList")}</Link></li>
          <li class="breadcrumb-item"><Link to={`/Surveyor/Survey/SurveyDetails/${params.SurveyId}/${params.Name}`} state={location.state} className="textDecoration">{i18n.t('SurveyDetails')}</Link></li>
          <li class="breadcrumb-item active" aria-current="page">{i18n.t('Dashboard2.social')}</li>
        </ol> :
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><Link to="/Admin/Dashboard" className="textDecoration">{i18n.t('admin')}</Link></li>
            <li class="breadcrumb-item"><Link to="/Surveyor/SurveyList" className="textDecoration">{i18n.t('Dashboard.SurveyManagement')}</Link></li>
            <li class="breadcrumb-item"><Link to={`/Surveyor/Survey/SurveyDetails/${params.SurveyId}/${params.Name}`} state={location.state} className="textDecoration">{i18n.t('SurveyDetails')}</Link></li>
            <li class="breadcrumb-item active" aria-current="page">{i18n.t('Dashboard2.social')}</li>

          </ol>
        }
      </nav>
      <div className="container">

        <div className="container d-flex mx-auto justify-content-center">
          <h5 className="m-3 header">{i18n.t('surveyee')}: <b className="text-primary">{params.SurveyId}</b></h5>
          <h5 className="m-3 header">{i18n.t('surveyeeName')}:<b className="text-primary"> {params.Name}</b></h5>
          <h5 className="m-3 header">{i18n.t("status")}: <b className="text-primary">{location.state.status}</b></h5>
        </div>
        <Accordian title={i18n.t("social")}>
          <Form onSubmit={handleSubmit(onSubmitHandler)}>
            <SurveyInfo_Questiones
              api={Model}
              register={register}
              data={multiselect}
              setData={SetData}
            />
            <SurevyInfo_BottomNavigation backNavigator={backNavigator} />
          </Form>
        </Accordian>
      </div>
    </>
  );
};


export default SurveyInfo_SocialSecurity;