import { React, useState, useMemo } from "react";
import DataTable from "react-data-table-component";
import DataTableExtensions from "react-data-table-component-extensions";
import { Link, useNavigate } from "react-router-dom";
import i18n from "../i18n/i18n";
import SurveyService from "../_services/SurveyService";
import spinner from "../Assets/Image/spinner.gif";
import "../CSS/CommonComponentsCSS/AdminSurveyAccordian.css";

export const AccordionTable = ({ data,filterModel,total }) => {
  const [status,setStatus] = useState(0);
  const [individualStatus,setIndividualStatus] = useState(0)
  const navigate = useNavigate();
 
  const columns = [
    {
      name: i18n.t("BasicPersonal.Name"),
      sortable: true,
      selector: (row) => <p className="tableStyle">{row.name}</p>,
       width:"140px"
    },
    {
      name: i18n.t("ApplicationID"),
      selector: (row) => <p className="tableStyle">{row.surveyId}</p>,
      sortable: true,
       width:"160px"
    },

    {
      name: i18n.t("gender"),
      selector: (row) => <p className="tableStyle">{row.genderByBirth}</p>,
      sortable: true,
      width:"130px"
    },

    {
      name: i18n.t("SurveyID"),
      sortable: true,
      selector: (row) => <p className="tableStyle">{row.surveyId}</p>,
    },
    {
      name: i18n.t("Surveyor"),
      sortable: true,
      selector: (row) => <p className="tableStyle">{row.user?row.user.userName:""}</p>,
    },
    {
      name: i18n.t("Date"),
      sortable: true,
      selector: (row) => (
        <p className="tableStyle">
          {row.created_Date !== null
            ? row.created_Date.slice(0,10)
            : ""}
        </p>
      ),
    },
    {
      name: i18n.t("status"),
      sortable: true,
      selector: "status",
      width:"168px",
      cell: (row) => {
        return (
          <>
            <button
              type="button"
              className="submitted-btn fix-width statusStyle"
            >
              {row.status === "Completed"
                ? `${i18n.t("completed")}`
                : `${i18n.t("pending")}`}
            </button>
          </>
        );
      },
    },
    {
      name: i18n.t("action"),
      sortable: true,
      selector: "action",
      width:"130px",
      cell: (row) => {
        return (
          <>
            <Link
              to={`/Surveyor/Survey/SurveyDetails/${row.surveyId}/${row.name}`}
              state={{status:(row.status === "Completed" ? "Completed" : "Pending"),locale:row.locale}}
            >
              <button className="btn btn-warning text-light">
                {i18n.t("view")}
              </button>
            </Link>
          </>
        );
      },
    },
    {
      name: i18n.t("download"),
      sortable: true,
      selector: "action",
      width:"155px",
      cell: (row) => {
        return (
          <>
            <button
              className="btn btn-primary"
              onClick={async() => {
                
                const res = await SurveyService.ExportToExcel(
                  `${row.name}(${row.surveyId})`,
                  row.surveyId
                );
                
              }}
            >
              {i18n.t("download")}
            </button>
          </>
        );
      },
    },
  ];

  return (
    <>
      <div>
        <button className="btn text-light float-end downloadAllButton" onClick={async () => {
          debugger
          if (total > 1000) {
            alert("Maximum Downloadable Survey is 1000!")
            return
          }
          setStatus(1)
          const res = await SurveyService.ExportToExcelAllSurvey("Karmani",filterModel)
          setStatus(0)
        }}>{status === 0 ? i18n.t("DownloadAllSurvey") : <>
        <img src={spinner} width="50px" height="24px" alt="spinner" style={{objectFit:"contain"}}/>
      </>}</button>
        <DataTable
          data={data}
          columns={columns}
          pagination
          paginationRowsPerPageOptions={[5, 10, 15, 20, 25]}
          pointerOnHover={true}
          highlightOnHover

          // progressPending={data.length > 0 ? false : true}
        />
      </div>
    </>
  );
};
