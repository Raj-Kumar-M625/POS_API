import React from "react";
import { Container, Row, Col, Card } from "react-bootstrap";
import PersonIcon from "@mui/icons-material/Person";
import ManageAccountsIcon from "@mui/icons-material/ManageAccounts";
import { Link, useNavigate } from "react-router-dom";
import "../../CSS/Views/dashboard.css";
import i18n from "../../i18n/i18n";
import { useTranslation } from "react-i18next";
import { AdminNavbar } from "../../CommonComponent/AdminNavbar";

const AdminDashboardScreen = () => {
  const navigate = useNavigate();
  debugger;
  const User = JSON.parse(sessionStorage.getItem("LoginData"));
  if (User === "" || User == null) {
    sessionStorage.setItem("isLoggedIn", "Login");
    window.location.href = "/Login";
  }else{
    if(User.userRoles !== "Admin"){
     window.location.href = "/Common/ErrorPage"
    }
  }
  debugger;
  
  const navigationHandler = () => { };
  


  return (
    <>
      <AdminNavbar data={{ Title: i18n.t('admin') }}></AdminNavbar>

      <Container className="vertical-center horizontal-center">
        <Row
          className="justify-content-center"

        >
          <Col md="3">
            <Card className="card1"

            >
              <Link className="link1"
                to="/Admin/UserManagement"

              >
                <PersonIcon className="icon1"

                  onClick={navigationHandler}
                />
                <Card.Body>
                  <Card.Title className="title1" >
                    {i18n.t('Dashboard.UserManagement')}
                  </Card.Title>
                </Card.Body>
              </Link>
            </Card>
          </Col>


          <Col md="3">
            <Card className="card3"

            >
              <Link className="link3"
                to="/Surveyor/SurveyList"

              >
                <ManageAccountsIcon
                  className="icon3"
                />
                <Card.Body>
                  <Card.Title className="title3">
                    {i18n.t('Dashboard.SurveyManagement')}
                  </Card.Title>
                </Card.Body>
              </Link>
            </Card>
          </Col>
        </Row>
      </Container>
    </>
  );
};

export default AdminDashboardScreen;

{/* <Col md="3">
            <Card className="card2"

            >
              <Link className="link2"
                to="/Admin/UserAssignment"

              >
                <AssignmentIndIcon className="icon2"

                />
                <Card.Body>
                  <Card.Title className="title2">
                  {i18n.t('Dashboard.UserAssignment')}
                  </Card.Title>
                </Card.Body>
              </Link>
            </Card>
          </Col> */}
