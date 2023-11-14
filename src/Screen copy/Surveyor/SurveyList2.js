import  {React,useState,useEffect} from 'react';
import { Button } from "react-bootstrap";
import DataTable from "react-data-table-component";
import { CommonAccordian } from "../../CommonComponent/CommonAccordian";
import { useNavigate } from "react-router-dom";
import { http } from "../../_services/http.service";
import axios from "axios";
import { config } from "../../constants";

function SurveyList2() {
  const navigate = useNavigate();
  const [data, setData] = useState([]);

  useEffect(() => {
    const urlLink = `${config.localUrl}BasicSurveyDetail`;
    axios
      .get(urlLink)
      .then(function (response) {
        setData(response.data);
      });
  }, []);

  const style = {
    fontWeight: "500",
    fontSize: "18px",
  };
  const columns = [
    {
      name: "Name",
      selector: (row) => <p style={style}>{row.name}</p>,
      width: "200px",
    },
    {
      name: "Application ID",
      selector: (row) => <p style={style}>{row.surveyId}</p>,
      width: "200px",
    },
    {
      name: "Choosen Gender",
      selector: (row) => <p style={style}>{row.genderByBirth}</p>,
      width: "200px",
    },
    {
      name: "Prefered Language",
      selector: (row) => <p style={style}>English</p>,
      width: "200px",
    },
    {
      name: "Completed",
      selector: (row) => (
        <p style={{ color: "orange", fontWeight: 500, fontSize: "18px" }}>
          pending...
        </p>
      ),
      width: "200px",
    },
    {
      name: "Details",
      selector: (row) => (
        <Button
          className="btn btn-primary btn-sm"
          onClick={() => navigate("/Surveyor/SurveyDetails")}
        >
          view
        </Button>
      ),
      width: "100px",
    },
  ];

  return (
    <div className="container  mt-5">
      <CommonAccordian data={{ title: "Survey List", eventKey: "0" }}>
        <DataTable columns={columns} data={data} paginationTotalRows="1" />
      </CommonAccordian>
    </div>
  );
}

export default SurveyList2;
