import { React, useState, useEffect } from "react";
import { Button } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import Accordian from "../../CommonComponent/Accordian";
import { config } from "../../constants";
import "../../CSS/Views/ViewerDashboard.css";
import DataTable from "react-data-table-component";
import i18n from "../../i18n/i18n";
import { useTranslation } from "react-i18next";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";

import axios from "axios";



function SurveyList2() {
  const navigate = useNavigate();
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Content-type": "application/json",
      "Authorization": `Bearer ${sessionStorage.getItem("token")}`
    },

  });

  debugger;
  const User = JSON.parse(sessionStorage.getItem("LoginData"));
  if (User === "" || User == null) {
    sessionStorage.setItem("isLoggedIn", "Login");
    window.location.href = "/Login";
  }else{
    if(User.userRoles !== "Surveyor"){
     window.location.href = "/Common/ErrorPage"
    }
  }

  const [matches,setMatches] = useState(window.matchMedia("(min-width: 1025px) and (max-width: 1580px)").matches);
  useEffect(() => {
    debugger
    window.matchMedia("(min-width: 1025px) and (max-width: 1580px)").addEventListener('change',e => setMatches(e.matches));
  },[]);
  const [data, setData] = useState([]);
  const [errorMessage, setErrorMessage] = useState(null);
  const { t } = useTranslation()

  useEffect(() => {
    if (User) {

      const urlLink = `${config.localUrl}BasicSurveyDetail/getBasicSurveyDetailsByUser?UserId=${User.userId}`;
      privateAxios.get(urlLink).then((response) => {
        debugger;
        setData(response.data);
      }).catch((error) => {
        console.log(error);
      });
    }
  }, []);

  var columns;
  matches ? columns = [
    {
      name: `${i18n.t("BasicPersonal.Name")}`,
      selector: (row) => <p className="text-wrap tableStyle">{row.name}</p>,
      width: "140px",
    },
    {
      name: `${i18n.t("BasicPersonal.Address")}`,
      selector: (row) => <p className="text-wrap tableStyle">{row.address}</p>,
      width: "260px",
    },
    {
      name: `${i18n.t("BasicPersonal.District")}`,
      selector: (row) => <p className="text-wrap tableStyle">{row.district}</p>,
      width: "180px",
    },
    {
      name: `${i18n.t("BasicPersonal.Taluk")}`,
      selector: (row) => <p className="text-wrap tableStyle">{row.taluk}</p>,
      width: "180px",
    },
    {
      name: `${i18n.t("BasicPersonal.Hobli")}`,
      selector: (row) => <p className="text-wrap tableStyle" >{row.hobli}</p>,
      width: "180px",
    },
    {
      name: `${i18n.t("SurveyID")}`,
      selector: (row) => <p className="text-wrap tableStyle" >{row.surveyId}</p>,
      width: "150px",
    },
    {
      name: `${i18n.t("status")}`,
      selector: (row) => (
        <p className="statusStyle">
          {row.status == null ? `${i18n.t("pending")}` : `${i18n.t("completed")}`}
        </p>
      ),
      width: "175px",
    },
    {
      name: `${i18n.t("details")}`,
      selector: (row) => (
        <Button
          className="btn btn-primary btn-sm"
          onClick={() =>
            {row.status === "Completed" ? navigate(`/Surveyor/Survey/SurveyDetails/${row.surveyId}/${row.name}`,{
              state: {status:(row.status === "Completed" ? "Completed" : "Pending"),
              locale:row.locale,
              origin:"Surveyor"
            }
            }) : navigate(`/Surveyor/SurveyDetails`, {
              state: {
                id: row.surveyId,
                name: row.name,
                locale: row.locale,
                origin: "SurveyList",
                status: row.status
              }
            },)}
            
          
          }
        >
          {i18n.t("view")}
        </Button>
      ),
      width: "100px",
    },
  ] : columns = [
    {
      name: `${i18n.t("BasicPersonal.Name")}`,
      selector: (row) => <p className="text-wrap mobileTableStyle">{row.name}</p>,
      width: "100px",
    },
    {
      name: `${i18n.t("BasicPersonal.District")}`,
      selector: (row) => <p className="text-wrap mobileTableStyle">{row.district}</p>,
      width: "100px",
    },
    {
      name: `${i18n.t("BasicPersonal.Taluk")}`,
      selector: (row) => <p className="text-wrap mobileTableStyle">{row.taluk}</p>,
      width: "100px",
    },
    {
      name: `${i18n.t("BasicPersonal.Hobli")}`,
      selector: (row) => <p className="text-wrap mobileTableStyle">{row.hobli}</p>,
      width: "100px",
    },
    {
      name: `${i18n.t("SurveyID")}`,
      selector: (row) => <p className="text-wrap mobileTableStyle">{row.surveyId}</p>,
      width: "100px",
    },
    {
      name: `${i18n.t("status")}`,
      selector: (row) => (
        <p className="text-wrap mobileStatusStyle"  >
          {row.status == null ? `${i18n.t("pending")}` : `${i18n.t("completed")}`}
        </p>
      ),
      width: "100px",
    },
    {
      name: `${i18n.t("details")}`,
      selector: (row) => (
        <Button
          className="btn btn-primary btn-sm"
          onClick={() =>  
            {row.status === "Completed" ? navigate(`/Surveyor/Survey/SurveyDetails/${row.surveyId}/${row.name}`,{
              state: {status:(row.status === "Completed" ? "Completed" : "Pending"),
              locale:row.locale,
              origin:"Surveyor"
            }
            }) :        
            navigate(`/Surveyor/SurveyDetails`, {
              state: {
                id: row.surveyId,
                name: row.name,
                locale: row.locale,
                origin: "SurveyList",
                status: row.status
              }
            },)
          }
          }
        >
          {i18n.t("view")}
        </Button>
      ),
      width: "100px",
    },
  ]
 

  return (<>
    <NavbarTitle data={{ Title: i18n.t('surveyList') }}></NavbarTitle>
    <nav aria-label="breadcrumb" className="mx-3">
      <ol class="breadcrumb">
        <li class="breadcrumb-item"><Link to="/Surveyor/SurveyDashboard">{i18n.t("Home")}</Link></li>
        <li class="breadcrumb-item active" aria-current="page">{i18n.t('surveyList')}</li>
      </ol>
    </nav>
    <div className="m-3">
      {matches && <Accordian title={i18n.t("surveyList")}>
        <div className="m-3 border accordianContainer">
          <DataTable
            columns={columns}
            data={data}           
            progressPending={data.length > 0 ? false : true}
            pagination
            highlightOnHover
            pointerOnHover={true}
            paginationRowsPerPageOptions={[5, 10, 15, 20, 25]}
          />
        </div>
      </Accordian>}
      {!matches && <DataTable
            columns={columns}
            data={data}
            progressPending={data.length > 0 ? false : true}
            pagination
            highlightOnHover
            pointerOnHover={true}
            paginationRowsPerPageOptions={[5, 10, 15, 20, 25]}
          />}
      {/* <Accordian title={i18n.t("surveyList")}>
        <div className="m-3 border accordianContainer">
          <DataTable
            columns={columns}
            data={data}

            progressPending={data.length > 0 ? false : true}
            pagination
            highlightOnHover
            pointerOnHover={true}
            paginationRowsPerPageOptions={[5, 10, 15, 20, 24]}
          />
        </div>
      </Accordian> */}
    </div>
  </>);
}

export default SurveyList2;
