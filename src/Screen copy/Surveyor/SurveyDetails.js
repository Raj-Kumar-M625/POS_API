import React, { useEffect } from "react";
import { CustomCard } from "../../CommonComponent/CustomCard";
import PersonIcon from "@mui/icons-material/Person";
import SchoolIcon from "@mui/icons-material/School";
import BadgeIcon from "@mui/icons-material/Badge";
import HouseIcon from "@mui/icons-material/House";
import LocalHospitalIcon from "@mui/icons-material/LocalHospital";
import LockIcon from "@mui/icons-material/Lock";
import NoteAddIcon from "@mui/icons-material/NoteAdd";
import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import axios from "axios";
import { config } from "../../constants";

export const SurveyDetails = () => {
  const navigate = useNavigate();
  const [data, setdata] = useState({
    Name: "",
    SurveyId: "",
  });

  useEffect(() => {
    const urlLink = `${config.localUrl}BasicSurveyDetail`;
    axios
      .get(urlLink)
      .then(function (response) {
        setdata({
          Name: response.data[response.data.length - 1].name,
          SurveyId: response.data[response.data.length - 1].surveyId,
        });
      });
  }, []);

  return (
    <>
      <div className="container d-flex mx-auto justify-content-center">
        <h5 className="m-3">Surveyor Id : {data.SurveyId}</h5>
        <h5 className="m-3">Surveyor Name : {data.Name}</h5>
      </div>

      <div
        style={{
          width: "100%",
          margin: "0 auto",
          display: "flex",
          flexWrap: "wrap",
          justifyContent: "center",
        }}
      >
        <Link
          to="/Surveyor/PersonalDetails2"
          style={{ textDecoration: "none" }}
        >
          <CustomCard
            cardTitle="Personal Details"
            backgroundColor="#5098a7"
            cardImg={
              <PersonIcon
                style={{ width: "100%", height: "40px", color: "white" }}
              ></PersonIcon>
            }
          />
        </Link>
        <Link to="/Surveyor/Education" style={{ textDecoration: "none" }}>
          <CustomCard
            cardTitle="Education"
            backgroundColor="#636998"
            cardImg={
              <SchoolIcon
                style={{ width: "100%", height: "40px", color: "white" }}
              ></SchoolIcon>
            }
          />
        </Link>
        <Link to="/Surveyor/Employment" style={{ textDecoration: "none" }}>
          <CustomCard
            cardTitle="Employment"
            backgroundColor="#62a76c"
            cardImg={
              <BadgeIcon
                style={{ width: "100%", height: "40px", color: "white" }}
              ></BadgeIcon>
            }
          />
        </Link>
        <Link to="/Surveyor/Housing" style={{ textDecoration: "none" }}>
          <CustomCard
            cardTitle="Housing"
            backgroundColor="#33e1ff"
            cardImg={
              <HouseIcon
                style={{ width: "100%", height: "40px", color: "white" }}
              ></HouseIcon>
            }
          />
        </Link>
      </div>
      <div
        style={{
          width: "100%",
          margin: "0 auto 68px",
          display: "flex",
          flexWrap: "wrap",
          justifyContent: "center",
        }}
      >
        <Link to="/Surveyor/Health" style={{ textDecoration: "none" }}>
          <CustomCard
            cardTitle="Health"
            backgroundColor="#f3ca3e"
            cardImg={
              <LocalHospitalIcon
                style={{ width: "100%", height: "40px", color: "white" }}
              ></LocalHospitalIcon>
            }
          />
        </Link>
        <Link to="/Surveyor/SocialSecurity" style={{ textDecoration: "none" }}>
          <CustomCard
            cardTitle="Social Security"
            backgroundColor="#808bde"
            cardImg={
              <LockIcon
                style={{
                  width: "100%",
                  height: "40px",
                  color: "white",
                }}
              ></LockIcon>
            }
          />
        </Link>
        <Link
          to="/Surveyor/AdditionalDetails"
          style={{ textDecoration: "none" }}
        >
          <CustomCard
            cardTitle="Additional Information"
            backgroundColor="#ff5f58"
            cardImg={
              <NoteAddIcon
                style={{ width: "100%", height: "40px", color: "white" }}
              ></NoteAddIcon>
            }
          />
        </Link>
      </div>
    </>
  );
};
