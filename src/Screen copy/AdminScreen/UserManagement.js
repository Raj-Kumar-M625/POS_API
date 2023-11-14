import React, { useState } from 'react';
import { CommonAccordian } from "../../CommonComponent/CommonAccordian";
import { Stack, Row, Col } from "react-bootstrap";
import Select from "react-select";
import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';
import Form from 'react-bootstrap/Form';
import Navbar from 'react-bootstrap/Navbar';
import Table from 'react-bootstrap/Table';
import Dropdown from 'react-bootstrap/Dropdown';
import DataTable from "react-data-table-component";
import { useNavigate } from "react-router-dom";
import Modal from 'react-bootstrap/Modal';
import AutorenewIcon from '@mui/icons-material/Autorenew';
import { display } from '@mui/system';
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";



function UserManagement() {




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
      name: "User Name",
      selector: (row) => <p style={style}>{row.UserName}</p>,
      width: "200px",
    },
    {
      name: "District",
      selector: (row) => <p style={style}>{row.District}</p>,
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
    {

      selector: (row) => (
        <div>
          < button style={{width:"100%",height:"100%"}} type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModalCenter">
            View
          </button>


          <div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
              <div class="modal-content">
                <div class="modal-header">
                  <h5 class="modal-title" id="exampleModalLongTitle">View</h5>

                </div>
                <div class="modal-body">
                  <div className="row g-3">
                    <div className="col-md-6 ">
                      <label>First Name</label>
                      <input type="text" placeholder='Radhika' className="form-control" disabled />
                    </div>
                  </div>

                  <div className="row g-3">
                    <div className="col-md-6">
                      <label>Last Name</label>
                      <input type="text" placeholder='Iyer' className="form-control" disabled />
                    </div>
                  </div>

                  <div className="row g-3">
                    <div className="col-md-6">
                      <label>Age</label>
                      <input type="text" placeholder='24' className="form-control" disabled />
                    </div>
                  </div>

                  <div className="row g-3">
                    <div className="col-md-6">
                      <label>Date of Birth</label>
                      <input type="text" placeholder='22/5/1998' className="form-control" disabled />
                    </div>
                  </div>

                  <div className="row g-3">
                    <div className="col-md-6">
                      <label>Gender</label>
                      <select className="form-select" disabled>
                        {/* <option value=""></option> */}
                        <option value="">M</option>
                        <option value="">F</option>
                      </select>
                    </div>
                  </div>

                  <div className="row g-3">
                    <div className="col-md-6">
                      <label>Phone/Mobile Number</label>
                      <input type="text" placeholder='9985533720' className="form-control" disabled />
                    </div>
                  </div>


                  <div className="row g-3">
                    <div className="col-md-6">
                      <label>CreatedDate</label>
                      <input type="text" placeholder='25/05/2022' className="form-control" disabled />
                    </div>
                  </div>
                </div>
                <div class="modal-footer">
                  <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>

                </div>
              </div>
            </div>
          </div>

        </div>
      ),

      width: "100px",
    },
    {
      name: "Action",
      selector: (row) => (
        <Button
          className="btn btn-success btn-sm"
          onClick={() => navigate("/edit")}
        >
          Edit
        </Button>
      ),

      width: "100px",
    },
    {

      selector: (row) => (
        <Button
          className="btn btn-danger btn-sm"
          onClick={() => navigate("/")}
        >
          Disable
        </Button>
      ),

      width: "100px",
    },
  ];

  const items = [
    {
      UserID: "HNS001",
      UserName: "Radhika iyer",
      District: "Bengaluru Rural",
      Taluk: "Anekal",
      w: "with",
      Hobli: "Sarjapura",
      v: "view",
    },
    {
      UserID: "HNS024",
      UserName: "Shubam",
      District: "Shivamogga",
      Taluk: "Sagara",
      w: "with",
      Hobli: "Achapura",
      v: "view",
    },
    {
      UserID: "HNS136",
      UserName: "Rahul",
      District: "kalaburagi",
      Taluk: "Sedam",
      w: "with",
      Hobli: "Adki",
      v: "view",
    },
    {
      UserID: "HNS205",
      UserName: "Madhu",
      District: "Bengaluru Urban",
      Taluk: "Bengaluru South",
      w: "with",
      Hobli: "Tavarekere",
      v: "view",
    },
    {
      UserID: "HNS136",
      UserName: "Ajay",
      District: "kalaburagi",
      Taluk: "Sedam",
      w: "with",
      Hobli: "Adki",
      v: "view",
    },
  ];

  return (

    <div>
      <NavbarTitle data={{ Title: "User Management" }}>
      <Form className="d-flex mt-1"
          >       <Dropdown>
              <Dropdown.Toggle style={{ marginRight: '20px' }} variant="primary" id="dropdown-basic">
                Menu
              </Dropdown.Toggle>

              <Dropdown.Menu>
                <Dropdown.Item href="#/action-1">Feed Back</Dropdown.Item>
                <Dropdown.Item href="#/action-2">User Management</Dropdown.Item>
                <Dropdown.Item href="#/action-3">User Assignment</Dropdown.Item>
              </Dropdown.Menu>
            </Dropdown>

          </Form>
      </NavbarTitle>

      {/* <Navbar style={{ background: "#00ace6" }} className="mb-2">
        <Container fluid>
          <h5 style={{ marginLeft: '50px' }}>User Management</h5>
          <Form className="d-flex mt-1"
          >       <Dropdown>
              <Dropdown.Toggle style={{ marginRight: '20px' }} variant="primary" id="dropdown-basic">
                Menu
              </Dropdown.Toggle>

              <Dropdown.Menu>
                <Dropdown.Item href="#/action-1">Feed Back</Dropdown.Item>
                <Dropdown.Item href="#/action-2">User Management</Dropdown.Item>
                <Dropdown.Item href="#/action-3">User Assignment</Dropdown.Item>
              </Dropdown.Menu>
            </Dropdown>

          </Form>
        </Container>
      </Navbar> */}

      <Stack direction="vertical" gap={2}>
        <CommonAccordian data={{ title: "Search", eventKey: "0" }} >
          <Row>
            <Col xs={3} lg={2}>
              <Select aria-label="Floating label select example"
                isClearable={true}
                name="distirct"
                className="select-dropdowns"
                options={""}
                placeholder="Select by Distict"
                isSearchable={true}

              ></Select>

            </Col>
            <Col xs={3} lg={2}>
              <Select aria-label="Floating label select example"
                isClearable={true}
                name="taluk"
                className="select-dropdowns"
                options={""}
                placeholder="Select by Taluk/Town"
                isSearchable={true}

              ></Select>

            </Col>
            <Col xs={3} lg={2}>
              <Select aria-label="Floating label select example"
                isClearable={true}
                name="hobli"
                className="select-dropdowns"
                options={""}
                placeholder="Select by Hobli/Zone"
                isSearchable={true}

              ></Select>
            </Col>

            <Col xs={3} lg={2}>
              <AutorenewIcon color="primary" style={{ marginTop: "6px", marginLeft: "600px", cursor: "pointer" }} />
            </Col>

          </Row>
        </CommonAccordian>
      </Stack>

      <div className="mt-5 mb-5">
        <CommonAccordian data={{ title: "User Details", eventKey: "0" }}>
          <div className="d-flex justify-content-between align-items-center">
            <div>
              <label>Show</label>
              <select>
                <option>1</option>
                <option>1</option>
              </select>
              <label>Entities</label>
            </div>
            <Button onClick={() => {
              navigate("/add")
            }} style={{ float: "right" }
            }>Add User</Button>

          </div>
          <div class="container" style={{
            marginLeft: '1075px',
            marginTop: '10px',
            display: 'flex'
          }}>

            {/* <div >

              <div class="col-md-8">

                <div class="search d-flex">
                  <button class="btn btn-primary ">Search</button>
                  <input type="text" placeholder="search" class="form-control" />

                </div>

              </div>

            </div> */}
          </div>


          <DataTable columns={columns} data={items} paginationTotalRows="1" />
        </CommonAccordian>
      </div>


    </div>

  )
}

export default UserManagement