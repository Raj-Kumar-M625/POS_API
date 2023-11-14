import React, { useState } from "react";
import { Stack, Row, Col, Button } from "react-bootstrap";
import { CommonAccordian } from "../../CommonComponent/CommonAccordian";
import Select from "react-select";
import "../../CSS/Views/ViewerDashboard.css";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import { useNavigate } from "react-router-dom";
import DataTable from "react-data-table-component";

export const UserAssignment = () => {
  const [data, setData] = useState([]);
  const navigate = useNavigate();
  const style = {
    fontWeight: "500",
    fontSize: "18px",
  };
  const columns = [
    {
      name: "User ID",
      selector: (row) => <p style={style}>{row.UserID}</p>,
      width: "200px",
    },
    {
      name: "User name",
      selector: (row) => <p style={style}>{row.UserName}</p>,
      width: "200px",
    },

    {
      name: "Age",
      selector: (row) => <p style={style}>{row.Age}</p>,
      width: "200px",
    },

    {
      name: "Gender",
      selector: (row) => <p style={style}>{row.Gender}</p>,
      width: "200px",
    },

    {
      name: "District",
      selector: (row) => <p style={style}>{row.District}</p>,
      width: "200px",
    },
    {
      name: "Contact number",
      selector: (row) => <p style={style}>{row.Contactnumber}</p>,
      width: "200px",
    },
    {
      name: "Taluk",
      selector: (row) => <p style={style}>{row.Taluk}</p>,
      width: "200px",
    },

    {
      name: "Hobli",
      selector: (row) => <p style={style}>{row.Hobli}</p>,
      width: "200px",
    },
  ];

  const items = [
    {
      UserID: "HNS001",
      UserName: "Radhika",
      Age: "24",
      Gender: "F",
      Contactnumber: "9985533720",
      District: "Bengalur Rural",
      Taluk: "Aneka",
      Hobli: "Sarjapur",
    },
    {
      UserID: "HNS024",
      UserName: "Shubham",
      Age: "28",
      Gender: "M",
      Contactnumber: "9985533721",
      District: "Shivamogga",
      Taluk: "Sagara",
      Hobli: "Achapura",
    },
    {
      UserID: "HNS136",
      UserName: "Rahul",
      Age: "36",
      Gender: "M",
      Contactnumber: "9985533722",
      District: "Kalburgi",
      Taluk: "Sedam",
      Hobli: "Adki",
    },
    {
      UserID: "HNS205",
      UserName: "Madhu P",
      Age: "32",
      Gender: "F",
      Contactnumber: "9985533723",
      District: "Bengalur Urban",
      Taluk: "Bengalur South",
      Hobli: "Tavarekere",
    },
    {
      UserID: "HNS136",
      UserName: "Ajay",
      Age: "36",
      Gender: "M",
      Contactnumber: "9985533762",
      District: "Kalburgi",
      Taluk: "Sedam",
      Hobli: "Adki",
    },
  ];
  return (
    <div className="jaya">
      <NavbarTitle data={{ Title: "User Assignment" }}></NavbarTitle>
      <Stack direction="vertical" gap={2} className="m-3">
        <CommonAccordian data={{ title: "Search", eventKey: "0" }}>
          <Row>
            <Col xs={3} lg={2}>
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                name="distirct"
                className="select-dropdowns"
                options={""}
                placeholder="Select by Distict"
                isSearchable={true}
              ></Select>
            </Col>
            <Col xs={3} lg={2}>
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                name="taluk"
                className="select-dropdowns"
                options={""}
                placeholder="Select by Taluk/Town"
                isSearchable={true}
              ></Select>
            </Col>
            <Col xs={3} lg={2}>
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                name="hobli"
                className="select-dropdowns"
                options={""}
                placeholder="Select by Hobli/Zone"
                isSearchable={true}
              ></Select>
            </Col>
            <Col xs={3} lg={2}>
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                name="village"
                className="select-dropdowns"
                options={""}
                placeholder="Select by Village/Ward"
                isSearchable={true}
              ></Select>
            </Col>
            <Col xs={3} lg={2}>
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                name="status"
                className="select-dropdowns"
                options={""}
                placeholder="Select by Gender"
                isSearchable={true}
              ></Select>
            </Col>
            <Col xs={3} lg={2}>
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                name="status"
                className="select-dropdowns"
                options={""}
                placeholder="Select by User name"
                isSearchable={true}
              ></Select>
            </Col>
          </Row>
        </CommonAccordian>
      </Stack>

      <div className="m-3">
        <CommonAccordian data={{ title: "Assigned List", eventKey: "0" }}>
          <div className="d-flex justify-content-between align-items-center">
            <div className="d-flex align-items-center">
              <label>Show</label>
              <select className="form-control mx-2">
                <option>1</option>
                <option>1</option>
              </select>
              <label>Entities</label>
            </div>

            <Button
              style={{ background: "#eeb919", color: "#000", border: "none" }}
            >
              Add
            </Button>
          </div>
          <DataTable columns={columns} data={items} paginationTotalRows="1" />
        </CommonAccordian>
      </div>
    </div>
  );
};

export default UserAssignment;
