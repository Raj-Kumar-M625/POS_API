import React, { useState, useEffect, useRef } from "react";
import { Stack, Row, Col, Button } from "react-bootstrap";
import Accordian from "../../CommonComponent/Accordian";
import Select from "react-select";
import "../../CSS/Views/ViewerDashboard.css";
import { Link, useNavigate } from "react-router-dom";
import DataTable from "react-data-table-component";
import "../../CSS/Views/UserAssignment.css";
import AutorenewIcon from "@mui/icons-material/Autorenew";
import i18n from "../../i18n/i18n";
import { useTranslation } from "react-i18next";
import { OfficeService } from "../../_services/officeService";
import { config } from "../../constants";
import { AdminNavbar } from "../../CommonComponent/AdminNavbar";
//import { privateAxios } from "../../context/Axios";

import axios from "axios";

export const UserAssignment = () => {
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Content-type": "application/json",
      "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
    },
   
  });

  const data = privateAxios.get(`${config.localUrl}AssignedUsers`);
  const [AssignedUser, setAssignedUser] = useState([]);

  const users = [];
  const selectInputRefDistrict1 = useRef();
  const selectInputRefTaluk1 = useRef();
  const selectInputRefHobli1 = useRef();
  const selectInputRefVillage1 = useRef();
  const gender = useRef();
  const userName = useRef();

  const onClear = () => {
    setAssignedUser(data);
    if (selectInputRefDistrict1.current) {
      selectInputRefDistrict1.current.clearValue();
      selectInputRefTaluk1.current.clearValue();
      selectInputRefHobli1.current.clearValue();
      selectInputRefVillage1.current.clearValue();
      gender.current.clearValue();
      userName.current.clearValue();
    }
  };

  const [districtList, setDistrictList] = useState([]);
  const [district, setDistrict] = useState(String);
  const [TalukList, setTalukList] = useState([]);
  const [Taluk, setTaluk] = useState(String);
  const [HobliList, setHobliList] = useState([]);
  const [Hobli, setHobli] = useState(String);
  const [VillageList, setVillageList] = useState([]);
  const [Village, setVillage] = useState(String);

  for (var user in data) {
    users.push({
      label: data[user].user.userName,
      value: data[user].user.userName,
    });
  }

  useEffect(() => {
    setAssignedUser(data);
    fetchDistrictList(0);
  }, [data]);

  const getAssignedUserByDistrict = async (district) => {
    const DistrictUsers = [];
    for (var user in data) {
      if (data[user].userAssignment.district === district) {
        DistrictUsers.push(data[user]);
      }
    }
    console.log(DistrictUsers);
    setAssignedUser(DistrictUsers);
  };

  async function getAssignedUserByTaluk(taluk) {
    const TalukUsers = [];
    for (var user in data) {
      if (data[user].userAssignment.taluk === taluk) {
        TalukUsers.push(data[user]);
      }
    }
    setAssignedUser(TalukUsers);
  }

  async function getAssignedUserByHobli(hobli) {
    const HobliUsers = [];
    for (var user in data) {
      if (data[user].userAssignment.hobli === hobli) {
        HobliUsers.push(data[user]);
      }
    }
    setAssignedUser(HobliUsers);
  }

  console.log(AssignedUser);

  const fetchDistrictList = async () => {
    const res = await OfficeService.getAllOfficeList(29);
    setDistrictList(res.data);
  };

  const getTaluk = async (distCode) => {
    const res = await OfficeService.getTalukList(distCode);
    setTalukList(res.data);
  };

  const getHobli = async (talukCode) => {
    const res = await OfficeService.getHobliList(talukCode);
    setHobliList(res.data);
  };

  const getVillage = async (hobliCode) => {
    const res = await OfficeService.getVillageList(hobliCode);
    setVillageList(res.data);
  };

  const navigate = useNavigate();
  const { t } = useTranslation();

  const style = {
    fontWeight: "500",
    fontSize: "18px",
  };
  const columns = [
    {
      name: i18n.t("userName"),
      selector: (row) => <p style={style}>{row.user.userName}</p>,
      width: "200px",
    },

    {
      name: i18n.t("age"),
      selector: (row) => <p style={style}>{row.user.age}</p>,
      width: "200px",
    },
    {
      name: i18n.t("BasicPersonal.District"),
      selector: (row) => <p style={style}>{row.userAssignment.district}</p>,
      width: "200px",
    },
    {
      name: i18n.t("contact"),
      selector: (row) => <p style={style}>{row.user.phoneNumber}</p>,
      width: "200px",
    },
    {
      name: i18n.t("taluk"),
      selector: (row) => <p style={style}>{row.userAssignment.taluk}</p>,
      width: "200px",
    },

    {
      name: i18n.t("hobli"),
      selector: (row) => <p style={style}>{row.userAssignment.hobli}</p>,
      width: "200px",
    },
  ];

  <div className="d-flex justify-content-end mb-0">
    <AutorenewIcon
      color="primary"
      onClick={() => {
        onClear();
      }}
    />
  </div>;
  return (
    <div className="jaya">
      <AdminNavbar data={{ Title: i18n.t("Dashboard.UserAssignment")}}></AdminNavbar>
     <nav aria-label="breadcrumb" className="mx-3 ">
  <ol class="breadcrumb">
          <li class="breadcrumb-item"><Link to="/Admin/Dashboard"  style={{textDecoration:"none"}}>Admin Dashboard</Link></li>
          <li class="breadcrumb-item"><Link to="/Admin/UserManagement"  style={{textDecoration:"none"}}>User Management</Link></li>
          <li class="breadcrumb-item active" aria-current="page">User Assignment</li>
        </ol>
      
</nav>
      {/* <Navbar className="mb-2">
        <Container fluid className="d-flex align-items-center">
          <h5 className="User">{i18n.t("Dashboard.UserAssignment")}</h5>
          <Form className="d-flex ">
            <DropdownButton
              className="dropdown mb-1"
              align="end"
              title={i18n.t("menu")}
              id="dropdown-menu-align-end"
            >
              <Dropdown.Item eventKey="2">
                {i18n.t("Dashboard.UserAssignment")}
              </Dropdown.Item>
              <Dropdown.Item
                eventKey="3"
                onClick={() => {
                  navigate("/Admin/UserManagement");
                }}
              >
                {i18n.t("Dashboard.UserManagement")}
              </Dropdown.Item>
            </DropdownButton>
          </Form>
        </Container>
      </Navbar> */}
      <Stack direction="vertical" className="m-3">
        <div className="d-flex justify-content-end" style={{ margin: "0px" }}>
          <b>{i18n.t("Refresh")}</b>
          <AutorenewIcon
            className="select-dropdowns"
            color="primary"
            onClick={() => {
              onClear();
            }}
          />
        </div>
        <Accordian title={i18n.t("search")}>
          <Row className="g-0">
            <Col md="2">
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                className="select-dropdowns"
                ref={selectInputRefDistrict1}
                name="distirct"
                options={districtList.map((opt) => ({
                  label: opt.districtName,
                  value: opt.districtCode,
                }))}
                onChange={(code, option) => {
                  if (selectInputRefTaluk1.current) {
                    selectInputRefTaluk1.current.clearValue();
                    setTalukList([]);
                  }

                  if (selectInputRefHobli1.current) {
                    selectInputRefHobli1.current.clearValue();
                    setHobliList([])
                  }

                  if (selectInputRefVillage1.current) {
                    selectInputRefVillage1.current.clearValue()
                    setVillageList([])
                   }
                  if (code) {
                    getAssignedUserByDistrict(code.label);
                    setDistrict(code.label);
                    getTaluk(code.value);
                  }
                }}
                placeholder="Select by Distict"
                isSearchable={true}
              ></Select>
            </Col>
            <Col md="2">
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                ref={selectInputRefTaluk1}
                name="taluk"
                className="select-dropdowns"
                options={TalukList.map((opt) => ({
                  label: opt.talukOrTownName,
                  value: opt.talukOrTownCode,
                }))}
                onChange={(code, option) => {
                   if (selectInputRefHobli1.current) {
                    selectInputRefHobli1.current.clearValue();
                    setHobliList([])
                  }

                  if (selectInputRefVillage1.current) {
                    selectInputRefVillage1.current.clearValue()
                    setVillageList([])
                   }
                  if (code) {
                    getAssignedUserByTaluk(code.label);
                    setTaluk(code.label);
                    getHobli(code.value);
                  }
                }}
                placeholder="Select by Taluk/Town"
                isSearchable={true}
              ></Select>
            </Col>
            <Col md="2">
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                ref={selectInputRefHobli1}
                name="hobli"
                className="select-dropdowns"
                options={HobliList.map((opt) => ({
                  label: opt.hobliOrZoneName,
                  value: opt.hobliOrZoneCode,
                }))}
                onChange={(code, option) => {
                    if (selectInputRefVillage1.current) {
                    selectInputRefVillage1.current.clearValue()
                    setVillageList([])
                   }
                  if (code) {
                    getAssignedUserByHobli(code.label);
                    setHobli(code.label);
                    getVillage(code.value);
                  }
                }}
                placeholder="Select by Hobli/Zone"
                isSearchable={true}
              ></Select>
            </Col>
            <Col md="2">
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                name="village"
                ref={selectInputRefVillage1}
                className="select-dropdowns"
                options={VillageList.map((opt) => ({
                  label: opt.villageOrWardName,
                  value: opt.villageOrWardCode,
                }))}
                onChange={(code, option) => {
                  if (code) {
                    setVillage(code.label);
                  }
                }}
                placeholder="Select by Village/Ward"
                isSearchable={true}
              ></Select>
            </Col>
            <Col md="2">
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                name="status"
                className="select-dropdowns"
                options={[
                  {
                    label: "Male",
                    value: "Male",
                  },
                  {
                    label: "Female",
                    value: "Female",
                  },
                ]}
                ref={gender}
                placeholder="Select by Gender"
                isSearchable={true}
              ></Select>
            </Col>
            <Col md="1">
              <Select
                className="select-dropdowns"
                aria-label="Floating label select example"
                isClearable={true}
                name="status"
                ref={userName}
                options={users}
                placeholder="Select by User name"
                isSearchable={true}
              ></Select>
            </Col>
{/* 
            <Col>
              <AutorenewIcon
                className="select-dropdowns"
                color="primary"
                onClick={() => {
                  onClear();
                }}
              />
            </Col> */}
          </Row>
        </Accordian>
      </Stack>

      <div className="m-3">
        <Accordian title={i18n.t("assignedList")}>
          {/* <div className="d-flex justify-content-between align-items-center">
            <div className="d-flex align-items-center m-4 invisible">
              <label>Show</label>
              <select className="form-control mx-2">
                <option>1</option>
                <option>2</option>
                <option>3</option>
                <option>4</option>
                <option>5</option>
                <option>6</option>
                <option>7</option>
                <option>8</option>
                <option>9</option>
                <option>10</option>
              </select>
              <label>Entities</label>
            </div>

            <Button
              className="m-3 text-light"
              onClick={() => {
                navigate("/add/userAssignment");
              }}
            >
              {i18n.t("add")}
            </Button>
          </div> */}

          <div className="m-3 border">
               <Button
              className="m-3 text-light float-end"
              onClick={() => {
                navigate("/add/userAssignment");
              }}
            >
              {i18n.t("add")}
            </Button>
            <DataTable
              pagination
              paginationRowsPerPageOptions={[5, 10, 15, 20, 25]}
              pointerOnHover={true}
              highlightOnHover
              defaultSortFieldId={5}
              defaultSortAsc={false}
              columns={columns}
              data={AssignedUser}
            />
          </div>
        </Accordian>
      </div>
    </div>
  );
};

export default UserAssignment;
