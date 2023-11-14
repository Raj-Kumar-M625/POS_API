import React from "react";
import { Container, Row, Col, Card } from "react-bootstrap";
import PersonIcon from "@mui/icons-material/Person";
import ListAltIcon from "@mui/icons-material/ListAlt";
import { Link } from "react-router-dom";
import "../../CSS/Views/survayordashboard.css"
import i18n from "../../i18n/i18n";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";


export const SurveyorDashboard = () => {
  
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
  return (
    <>
       <NavbarTitle data={{ Title: i18n.t('surveyDashboard') }}></NavbarTitle>
      
      <Container className="vertical-center">
        <Row
          className="justify-content-center"

        >
          <Col md="3">
            <Card className="card4">
              <Link className="link1" to="/Surveyor/SurveyDetails"
              state={{
              id: "",
              name: "",
              locale: "",
              origin: "AddSurvrey",
              
            }}
              >
                <PersonIcon className="icon1"

                />
                <Card.Body>
                  <Card.Title className="title1">
                    {i18n.t('Dashboard.Add')}
                  </Card.Title>
                </Card.Body>
              </Link>
            </Card>
          </Col>

          <Col md="3">
            <Card className="card5">
              <Link className="link2" to="/Surveyor/SurveyList2">
                <ListAltIcon
                  className="icon2"
                />
              </Link>
              <Card.Body>
                <Card.Title className="title2">
                {i18n.t('Dashboard.List')}
                </Card.Title>
              </Card.Body>
            </Card>
          </Col>

          {/* <Col md="3">
            <Card className="card6">
              <CloudUploadIcon
                className="icon3"
              />
              <Card.Body>
                <Card.Title className="title3">
                {i18n.t('Dashboard.Upload')}
                </Card.Title>
              </Card.Body>
            </Card>
          </Col> */}
        </Row>
      </Container>
    </>
  );
};
