import React, { useState, useMemo } from "react";
import { Stack, Navbar } from "react-bootstrap";
import { CommonAccordian } from "../../CommonComponent/CommonAccordian";
import { CustomCard } from "../../CommonComponent/CustomCard.js";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import { Container, Row, Col, Card } from "react-bootstrap";
import PersonIcon from "@mui/icons-material/Person";
import AssignmentIndIcon from "@mui/icons-material/AssignmentInd";
import ManageAccountsIcon from "@mui/icons-material/ManageAccounts";
import { Link } from "react-router-dom";

const AdminDashboardScreen = () => {
  const navigationHandler = () => {};

  const data1 = {
    // cardTitle: `${
    //   !data.length ? 0 : data.length
    // }`,
    cardValue: "User Management",
    width: "40%",
    backgroundColor: "#a6d1ff",
  };
  const data2 = {
    // cardTitle: `${
    //   !pendingCount ? 0 : pendingCount
    // }`,
    cardValue: "User Assignment",
    width: "40%",
    backgroundColor: "#b2ffb3",
  };
  const data3 = {
    // cardTitle: `${
    //   !venderSelected ? 0 : venderSelected
    // }`,
    cardValue: "Survey Management",
    width: "40%",
    backgroundColor: "#ffadca",
  };

  return (
    <>
      <NavbarTitle data={{ Title: "User Assignment" }}></NavbarTitle>

      <Container class="vertical-center">
        <Row
          className="justify-content-center"
          style={{ marginTop: "12%", marginBottom: "12%" }}
        >
          <Col md="3">
            <Card
              style={{
                background: "#5098a7",
                margin: "0 auto",
                height: "100%",
                width: "70%",
              }}
            >
              <Link
                to="/Admin/UserManagement"
                style={{ textDecoration: "none" }}
              >
                <PersonIcon
                  style={{ width: "100%", height: "200px", color: "white" }}
                  onClick={navigationHandler}
                />
                <Card.Body>
                  <Card.Title style={{ color: "white", textAlign: "center" }}>
                    User Management
                  </Card.Title>
                </Card.Body>
              </Link>
            </Card>
          </Col>

          <Col md="3">
            <Card
              style={{
                background: "#636998",
                margin: "0 auto",
                height: "100%",
                width: "70%",
              }}
            >
              <Link
                to="/Admin/UserAssignment"
                style={{ textDecoration: "none" }}
              >
                <AssignmentIndIcon
                  style={{ width: "100%", height: "200px", color: "white" }}
                />
                <Card.Body>
                  <Card.Title style={{ color: "white", textAlign: "center" }}>
                    User Assignment
                  </Card.Title>
                </Card.Body>
              </Link>
            </Card>
          </Col>
          <Col md="3">
            <Card
              style={{
                background: "#808bde",
                margin: "0 auto",
                height: "100%",
                width: "70%",
              }}
            >
              <Link
                to="/Surveyor/SurveyList"
                style={{ textDecoration: "none" }}
              >
                <ManageAccountsIcon
                  style={{ width: "100%", height: "200px", color: "white" }}
                />
                <Card.Body>
                  <Card.Title style={{ color: "white", textAlign: "center" }}>
                    Survey Management
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
