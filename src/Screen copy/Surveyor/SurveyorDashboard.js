import React from "react";
import { Container, Row, Col, Card } from "react-bootstrap";
import PersonIcon from "@mui/icons-material/Person";
import ListAltIcon from "@mui/icons-material/ListAlt";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import FileUploadIcon from "@mui/icons-material/FileUpload";

export const SurveyorDashboard = () => {
  const navigate = useNavigate();

  return (
    <>
      <Container class="vertical-center">
        <Row
          className="justify-content-center"
          style={{ marginTop: "12%", marginBottom: "12%" }}
        >
          <Col md="3">
            <Card style={{ background: "#5098a7", margin: "0 auto" }}>
              <Link to="/Surveyor/PersonalDetails" style={{textDecoration:'none'}}>
                <PersonIcon
                  style={{ width: "100%", height: "200px", color: "white" }}
                />
                <Card.Body>
                  <Card.Title style={{ color: "white", textAlign: "center" }}>
                    Add Survey
                  </Card.Title>
                </Card.Body>
              </Link>
            </Card>
          </Col>

          <Col md="3">
            <Card style={{ background: "#636998", margin: "0 auto" }}>
              <Link to="/Surveyor/SurveyList2">
                <ListAltIcon
                  style={{ width: "100%", height: "200px", color: "white" }}
                />
              </Link>
              <Card.Body>
                <Card.Title style={{ color: "white", textAlign: "center" }}>
                  Survey List
                </Card.Title>
              </Card.Body>
            </Card>
          </Col>

          <Col md="3">
            <Card style={{ background: "#808bde", margin: "0 auto" }}>
              <CloudUploadIcon
                style={{ width: "100%", height: "200px", color: "white" }}
              />
              <Card.Body>
                <Card.Title style={{ color: "white", textAlign: "center" }}>
                  Upload all survey
                </Card.Title>
              </Card.Body>
            </Card>
          </Col>
        </Row>
      </Container>
    </>
  );
};
