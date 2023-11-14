import React, { useState, useEffect, useRef } from "react";
import { Stack, Row, Col, Modal, } from "react-bootstrap";
import Accordian from "../../CommonComponent/Accordian";
import Select from "react-select";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import DataTable from "react-data-table-component";
import { Link, useNavigate } from "react-router-dom";
import AutorenewIcon from "@mui/icons-material/Autorenew";
import { OfficeService } from "../../_services/officeService";
import { UserService } from "../../_services/UserService";
import { useMemo } from "react";
import DataTableExtensions from "react-data-table-component-extensions";
import "../../CSS/Views/Usermanagement.css";
import i18n from "../../i18n/i18n";
import { AdminNavbar } from "../../CommonComponent/AdminNavbar";

function UserManagement(props) {
  const navigate = useNavigate();
  const User = JSON.parse(sessionStorage.getItem("LoginData"));
  if (User === "" || User == null) {
    sessionStorage.setItem("isLoggedIn", "Login");
    window.location.href = "/Login";
  }else{
    if(User.userRoles !== "Admin"){
     window.location.href = "/Common/ErrorPage"
    }
  }

  const selectInputRefDistrictt2 = useRef();
  const selectInputRefTaluk2 = useRef();
  const selectInputRefHobli2 = useRef();
  const [data, setData] = useState([]);
  const [filterData, setFilterData] = useState([]);
  const [show, setShow] = useState(false);
  const onClear = async () => {
    const res = await UserService.GetAllUsers();
    debugger
    setData(res.data)
    if (selectInputRefDistrictt2.current) {
      selectInputRefDistrictt2.current.clearValue();
      // selectInputRefTaluk2.current.clearValue();
      // selectInputRefHobli2.current.clearValue();
    }
    if (selectInputRefTaluk2.current) {
      selectInputRefTaluk2.current.clearValue();
    }
    if (selectInputRefHobli2.current) {
      selectInputRefHobli2.current.clearValue();
    }

  };

  
 
  const [view, setView] = useState(false);
  const [districtList, setDistrictList] = useState([]);
  const [errorMessage, setErrorMessage] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [loading, setLoading] = useState(false);

  const [district, setDistrict] = useState(String);
  const [TalukList, setTalukList] = useState([]);
  const [Taluk, setTaluk] = useState(String);
  const [HobliList, setHobliList] = useState([]);
  const [Hobli, setHobli] = useState(String);
  const [viewData, setViewData] = useState([]);
  const [dob, setDob] = React.useState(Date | "yyyy-MM-dd");
  var userList;
  const [formData, setFormData] = React.useState({
    firstName: "",
    lastName: "",
    age: "",
    dob: "",
    phoneNumber: "",
    district: "",
    taluk: "",
    hobli: "",
    district_Code: "",
    taluk_Code: "",
    hobli_Code: "",
    Role: "Surveyor"
  });

  const handleChange = (e) => {

    setFormData({ ...formData, [e.target.name]: e.target.value.replace(/[<>]/gi, '') });
  };

  const handleOfficeChange = (name, value) => {

    setFormData({ ...formData, [name]: value });
  }

  const handleDate = (e) => {
    setDob(e.target.value);
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  useEffect(() => {
    fetchDistrictList(1);
  }, []);

  useEffect(() => {
    setTimeout(() => {
      fetchUserList()
    }, 2000)
  }, [show, view])

  const fetchUserList = async () => {
    setLoading(true);
    const res = await UserService.GetAllUsers();
    if (res?.errorCode) {
      setErrorMessage(res.messsage);
    } else {
      debugger;
      setData(res.data);
      setFilterData(res.data);
    }

    setIsLoading(false);
    setLoading(false);
  };

  async function districtUsers(district) {
    debugger
    //const res = await UserService.GetAllUsers();
    if (filterData.length > 0) {
      setData(filterData.filter((e) => e.district_Code === district))
    }

  }

  async function talukUsers(taluk) {
    debugger
    //const res = await UserService.GetAllUsers();
    if (filterData.length > 0) {
      setData(filterData.filter((e) => e.taluk_Code === taluk))
    }

  }

  async function hobliUsers(hobli) {
    debugger
    //const res = await UserService.GetAllUsers();
    if (filterData.length > 0) {
      setData(filterData.filter((e) => e.hobli_Code === hobli))
    }

  }


  const fetchDistrictList = async () => {
    setLoading(true);
    const res = await OfficeService.getAllOfficeList(29);
    if (res?.errorCode) {
      setErrorMessage(res.messsage);
    } else {
      setDistrictList(res.data);
    }
    setIsLoading(false);
    setLoading(false);
  };

  const getTaluk = async (distCode) => {
    debugger
    const res = await OfficeService.getTalukList(distCode);
    if (res?.errorCode) {
      setErrorMessage(res.message);
    } else {
      setTalukList(res.data);
    }
  };

  const getHobli = async (talukCode) => {
    debugger
    const res = await OfficeService.getHobliList(talukCode);
    if (res?.errorCode) {
      setErrorMessage(res.message);
    } else {
      setHobliList(res.data);
    }
  };



  const handleClick = async (e) => {
    debugger;
    const res = await data.find((data) => data.id === e.target.id);
    if (res.status === 0) {
      ButtonRef[res.id].current.value = i18n.t("enable");
      ButtonRef[res.id].current.className = "btn btn-success";
      res.status = 1;
    } else {
      ButtonRef[res.id].current.value = i18n.t("disable");
      ButtonRef[res.id].current.className = "btn btn-danger";
      res.status = 0;
    }

    const updateuser = await UserService.UpdateUser(res);
    console.log(updateuser);

  };

  const handleViewClick = async (id) => {
    const res = await data.find((data) => data.id === id);

    if (res.dob) {
      res.dob = res.dob.substring(0, 10);
    }
    setViewData(res);
    setFormData(res);
  };

  const handelEditClick = async (id) => {
    const res = await data.find((data) => data.id === id);
    setViewData(res);
    setFormData(res);
    setView(true);
  };

  const onSubmitHandler = async (e) => {
    debugger
    const res = await UserService.UpdateUser(formData);
    const res2 = data.find((data) => data.id === formData.id);
    setFormData(res2);
    setView(false)
  };

  const onSubmitAddUser = async (e) => {
    setShow(false)
    const res = await UserService.AddUser(formData);

  };

  


  const ButtonRef = useMemo(() => {
    const refs = {};
    data.forEach((item) => {
      refs[item.id] = React.createRef(null);
    });
    return refs;
  }, [data]);

  const columns = useMemo(() => [
    {
      name: i18n.t("SurveyorID"),
      selector: "SurveyorID",
      cell: (row) => <p className="tableStyle">{row.surveyorId}</p>,
      width: "150px",
    },
    {
      name: i18n.t("userName"),
      selector: "userName",
      cell: (row) => <p className="tableStyle">{row.firstName + " " + row.lastName}</p>,
      width: "200px",
    },
    {
      name: i18n.t('BasicPersonal.District'),
      selector: "District",
      cell: (row) => <p className="tableStyle">{row.district}</p>,
      width: "200px",
    },
    {
      name: i18n.t('taluk'),
      selector: "Taluk",
      cell: (row) => <p className="tableStyle">{row.taluk}</p>,
      width: "200px",
    },
    {
      name: i18n.t('hobli'),
      selector: "Hobli",
      cell: (row) => <p className="tableStyle">{row.hobli}</p>,
      width: "200px",
    },
    {
      name: i18n.t("view"),
      selector: "view",
      cell: (row) => (
        <div>
          <Button
            className="TableButton"
            variant="secondary"
            data-toggle="modal"
            data-target="#exampleModalCenter"
            onClick={() => {
              handleViewClick(row.id);
            }}
          >
            {i18n.t("view")}
          </Button>

          <div
            class="modal fade"
            id="exampleModalCenter"
            role="dialog"
            aria-labelledby="exampleModalCenterTitle"
            aria-hidden="true"
          >
            <div class="modal-dialog modal-dialog-centered" role="document">
              <div class="modal-content">
                <div class="modal-header">
                  <h5 class="modal-title" id="exampleModalLongTitle">
                    {i18n.t("view")}
                  </h5>
                </div>
                <div class="modal-body">
                  <div className="row m-1">
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("BasicPersonal.FirstName")}</label>
                      <input
                        type="text"
                        value={viewData.firstName}
                        className="form-control"
                        disabled
                      />
                    </div>
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("BasicPersonal.LastName")}</label>
                      <input
                        type="text"
                        value={viewData.lastName}
                        className="form-control"
                        disabled
                      />
                    </div>
                  </div>

                  <div className="row m-1">
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("age")}</label>
                      <input
                        type="text"
                        value={viewData.age}
                        className="form-control"
                        disabled
                      />
                    </div>
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("BasicPersonal.DOB")}</label>

                      <input
                        type="text"
                        value={viewData.dob}
                        className="form-control"
                        disabled
                      />
                    </div>
                  </div>

                  <div className="row m-1">
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("BasicPersonal.PhoneNumber")}</label>
                      <input
                        type="text"
                        value={viewData.phoneNumber}
                        className="form-control"
                        disabled
                      />
                    </div>
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("BasicPersonal.District")}</label>
                      <input type="text" className="form-control" disabled value={viewData.district} />
                    </div>
                  </div>


                  <div className="row m-1">
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("BasicPersonal.Taluk")}</label>
                      <input type="text" className="form-control" disabled value={viewData.taluk} />
                    </div>
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("BasicPersonal.Hobli")}</label>
                      <input type="text" className="form-control" disabled value={viewData.hobli} />
                    </div>
                  </div>

                </div>
                <div class="modal-footer">
                  <button
                    type="button"
                    class="btn btn-secondary"
                    data-dismiss="modal"
                  >
                    {i18n.t("UserManagement.Cancel")}
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      ),
    },
    {
      name: i18n.t("action"),
      selector: "id",
      cell: (row) => (
        <div>
          <Button
            id={row.id}
            onClick={() => {
              handelEditClick(row.id);
            }}
          >
            {i18n.t("edit")}
          </Button>

        </div>
      ),
    },
    {
      name: `${i18n.t("status")}`,
      sortable: false,
      selector: "id",
      cell: (row) => {
        return (
          <>
            <input
              className={
                row.status === 0 ? "btn btn-danger" : "btn btn-success"
              }
              id={row.id}
              ref={ButtonRef[row.id]}
              onClick={(e) => {
                handleClick(e);
              }}
              type="button"
              defaultValue={row.status === 1 ? `${i18n.t("enable")}` : `${i18n.t("disable")}`}
            />
          </>
        );
      },
    },
  ]);

  const tableData = {
    columns,
    data,
  };

  return (
    <div>
      <AdminNavbar data={{ Title: i18n.t('Dashboard.UserManagement') }}></AdminNavbar>
      <div className="d-flex justify-content-between">
        <nav aria-label="breadcrumb" className="mx-3 ">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><Link className="text-decoration-none" to="/Admin/Dashboard">{i18n.t('admin')}</Link></li>
            <li class="breadcrumb-item active" aria-current="page">{i18n.t('Dashboard.UserManagement')}</li>

          </ol>
        </nav>
      </div>

      <Stack direction="vertical">
        <Accordian title={i18n.t("search")}>
          <div class="flex-container row1">
            <Row className="row">
              <Col className="col-sm-3">
                <Select
                  aria-label="Floating label select example"
                  isClearable={true}
                  ref={selectInputRefDistrictt2}
                  name="distirct"
                  className="select-dropdowns1"
                  options={districtList.map((opt) => ({
                    label:
                      i18n.language === "kn"
                        ? opt.districtName_KA
                        : opt.districtName,
                    value: opt.districtCode,
                  }))}
                  onChange={(code, option) => {
                    if (selectInputRefTaluk2.current) {
                      selectInputRefTaluk2.current.clearValue();
                      setTalukList([])
                    }

                    if (selectInputRefHobli2.current) {
                      selectInputRefHobli2.current.clearValue();
                      setHobliList([])
                    }
                    if (code) {
                      districtUsers(code.value)
                      setDistrict(code.value);
                      getTaluk(code.value);
                    }
                  }}
                  placeholder={i18n.t("SelectDistrict")}
                  isSearchable={true}
                  isLoading={isLoading}
                ></Select>
              </Col>
              <Col className="col-sm-3">
                <Select
                  aria-label="Floating label select example"
                  isClearable={true}
                  ref={selectInputRefTaluk2}
                  name="taluk"
                  className="select-dropdowns2"
                  options={TalukList.map((opt) => ({
                    label:
                      i18n.language === "kn"
                        ? opt.talukOrTownName_KA
                        : opt.talukOrTownName,
                    value: opt.talukOrTownCode,
                  }))}
                  onChange={(code, option) => {
                    if (selectInputRefHobli2.current) {
                      selectInputRefHobli2.current.clearValue();
                      setHobliList([])
                    }
                    if (code) {
                      talukUsers(code.value)
                      setTaluk(code.label);
                      getHobli(code.value);
                    }
                  }}
                  placeholder={i18n.t("SelectTaluk")}
                  isSearchable={true}
                ></Select>
              </Col>
              <Col className="col-sm-3">
                <Select
                  aria-label="Floating label select example"
                  isClearable={true}
                  name="hobli"
                  className="select-dropdowns3"
                  ref={selectInputRefHobli2}
                  options={HobliList.map((opt) => ({
                    label:
                      i18n.language === "kn"
                        ? opt.hobliOrZoneName_KA
                        : opt.hobliOrZoneName,
                    value: opt.hobliOrZoneCode,
                  }))}
                  onChange={(code, option) => {
                    if (code) {
                      hobliUsers(code.value)
                      setHobli(code.label);
                    }
                  }}
                  placeholder={i18n.t("SelectHobli")}
                  isSearchable={true}
                ></Select>
              </Col>

              <Col className="col4">
                <AutorenewIcon
                  color="primary"
                  onClick={() => {
                    onClear();
                  }}
                />
              </Col>
            </Row>
          </div>
        </Accordian>
      </Stack>

      <div className="">
        <Accordian title={i18n.t("userDetails")}>
          <div className="m-3 border">
            <Button
              className="m-3 text-light float-end"
              onClick={() => { setShow(true) }}
              data-toggle="modal"
              data-target="#AddUser"
            >
              {i18n.t("add")}
            </Button>
            {data.length ? (
              <DataTableExtensions
                export={false}
                print={false}
                {...tableData}
              >
                <DataTable
                  pagination
                  columns={columns}
                  data={data}

                  progressPending={data.length > 0 ? false : true}
                  highlightOnHover
                  // defaultSortFieldId={1}
                  // defaultSortAsc={false}
                />
              </DataTableExtensions>
            ) : (
              <DataTable
                pagination
                columns={columns}
                data={data}
                progressPending={data.length > 0 ? false : true}
                highlightOnHover
                // defaultSortFieldId={1}
                // defaultSortAsc={false}
              />
            )}
          </div>
        </Accordian>
      </div>
      <Form
      >
        <Modal show={show}>
          <div>
            <div class="modal-dialog modal-dialog-centered" role="document">
              <div class="modal-content">
                <div class="modal-header">
                  <h5 class="modal-title" id="exampleModalLongTitle">
                    {i18n.t("add")}
                  </h5>
                </div>
                <div class="modal-body">
                  <div className="row m-1">
                    <div className="col-md-5  m-1">
                      <Form.Label>{i18n.t("BasicPersonal.FirstName")}</Form.Label>
                      <Form.Control
                        type="text"
                        name="firstName"
                        onChange={handleChange}
                      />
                    </div>
                    <div className="col-md-5 m-1">
                      <Form.Label>{i18n.t("BasicPersonal.LastName")}</Form.Label>
                      <Form.Control
                        type="text"
                        name="lastName"
                        onChange={handleChange}
                      />
                    </div>
                  </div>


                  <div className="row m-1">
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("age")}</label>
                      <input
                        type="number"
                        className="form-control"
                        name="age"
                        onChange={handleChange}
                      />
                    </div>
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("BasicPersonal.DOB")}</label>
                      <input
                        type="Date"
                        className="form-control"
                        name="dob"
                        onChange={handleChange}
                      />
                    </div>
                  </div>

                  <div className="row m-1">
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("BasicPersonal.PhoneNumber")}</label>
                      <input
                        type="text"
                        className="form-control"
                        name="phoneNumber"
                        onChange={handleChange}
                      />
                    </div>
                    <div className="col-md-5 m-1">
                      <Form.Label className="star">
                        {i18n.t("BasicPersonal.District")}
                      </Form.Label>

                      <Select
                        aria-label="Floating label select example"
                        isClearable={true}
                        ref={selectInputRefDistrictt2}
                        name="distirct"
                        className="select-dropdowns1"
                        options={districtList.map((opt) => ({
                          label: i18n.language === "kn"
                            ? opt.districtName_KA : opt.districtName,
                          value: opt.districtCode,
                        }))}
                        onChange={(code, option) => {
                          if (selectInputRefTaluk2.current) {
                            selectInputRefTaluk2.current.clearValue();
                            setTalukList([])
                          }

                          if (selectInputRefHobli2.current) {
                            selectInputRefHobli2.current.clearValue();
                            setHobliList([])
                          }
                          if (code) {
                            setFormData({ ...formData, district: code.label.toString(), district_Code: code.value });
                            setDistrict(code.value);
                            getTaluk(code.value);
                          }
                        }}
                        placeholder={i18n.t("SelectDistrict")}
                        isSearchable={true}
                        isLoading={isLoading}
                        defaultValue={viewData.district}
                      ></Select>

                    </div>
                  </div>

                  <div className="row m-1">

                    <div className="col-md-5 m-1">
                      <Form.Label className="star">
                        {i18n.t("BasicPersonal.Taluk")}
                      </Form.Label>
                      <Select
                        aria-label="Floating label select example"
                        isClearable={true}
                        ref={selectInputRefTaluk2}
                        name="taluk"
                        className="select-dropdowns2"
                        options={TalukList.map((opt) => ({
                          label: i18n.language === "kn" ? opt.talukOrTownName_KA : opt.talukOrTownName,
                          value: opt.talukOrTownCode,
                        }))}
                        onChange={(code, option) => {
                          if (selectInputRefHobli2.current) {
                            selectInputRefHobli2.current.clearValue();
                            setHobliList([])
                          }
                          if (code) {
                            setFormData({ ...formData, taluk: code.label.toString(), taluk_Code: code.value });
                            setTaluk(code.label);
                            getHobli(code.value);
                          }
                        }}
                        placeholder={i18n.t("SelectTaluk")}
                        isSearchable={true}
                        defaultValue={viewData.taluk}
                      ></Select>

                    </div>
                    <div className="col-md-5 m-1">
                      <Form.Label className="star">
                        {i18n.t("BasicPersonal.Hobli")}
                      </Form.Label>

                      <Select
                        aria-label="Floating label select example"
                        isClearable={true}
                        name="hobli"
                        className="select-dropdowns3"
                        ref={selectInputRefHobli2}
                        options={HobliList.map((opt) => ({
                          label: i18n.language === "kn" ? opt.hobliOrZoneName_KA : opt.hobliOrZoneName,
                          value: opt.hobliOrZoneCode,
                        }))}
                        onChange={(code, option) => {
                          if (code) {
                            setHobli(code.label);
                            setFormData({ ...formData, hobli: code.label.toString(), hobli_Code: code.value });
                          }
                        }}
                        placeholder={i18n.t("SelectHobli")}
                        isSearchable={true}
                        defaultValue={viewData.hobli}
                      ></Select>

                    </div>
                  </div>


                  <div className="row m-1">

                  </div>
                </div>
                <div class="modal-footer">
                  <button
                    type="button"
                    class="btn btn-secondary"
                    onClick={() => {
                      setShow(false)
                    }}
                  >
                    {i18n.t("UserManagement.Cancel")}
                  </button>

                  <button className="btn btn-success m-1" onClick={() => {
                    onSubmitAddUser()
                  }}>
                    {i18n.t("Save")}
                  </button>
                </div>
              </div>
            </div>
          </div>
        </Modal>
      </Form>

      <Form >
        <Modal show={view}>
          <div>
            <div class="modal-dialog modal-dialog-centered" role="document">
              <div class="modal-content">
                <div class="modal-header">
                  <h5 class="modal-title" id="exampleModalLongTitle">
                    {i18n.t("edit")}
                  </h5>
                </div>
                <div class="modal-body">
                  <div className="row m-1">
                    <div className="col-md-5  m-1">
                      <label>{i18n.t("BasicPersonal.FirstName")}</label>
                      <input
                        type="text"
                        name="firstName"
                        defaultValue={viewData.firstName}
                        className="form-control"
                        onChange={(e) => { handleChange(e) }}
                      />
                    </div>
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("BasicPersonal.LastName")}</label>
                      <input
                        type="text"
                        name="lastName"
                        defaultValue={viewData.lastName}
                        className="form-control"
                        onChange={(e) => { handleChange(e) }}
                      />
                    </div>
                  </div>



                  <div className="row m-1">
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("age")}</label>
                      <input
                        type="number"
                        className="form-control"
                        name="age"
                        defaultValue={viewData.age}
                        onChange={(e) => { handleChange(e) }}
                      />
                    </div>
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("BasicPersonal.PhoneNumber")}</label>
                      <input
                        type="text"
                        defaultValue={viewData.phoneNumber}
                        className="form-control"
                        name="phoneNumber"
                        minLength={10}
                        maxLength={10}
                        onChange={(e) => { handleChange(e) }}
                      />
                    </div>
                  </div>


                  <div className="row m-1">
                    <div className="col-md-5 m-1">
                      <label>{i18n.t("BasicPersonal.DOB")}</label>
                      <input type="date" className="form-control" name="dob" onChange={handleChange} defaultValue={viewData.dob?.slice(0, 10)} />

                    </div>
                    <div className="col-md-5 m-1">
                      <Form.Label className="star">
                        {i18n.t("BasicPersonal.District")}
                      </Form.Label>
                      <Select
                        isClearable={true}
                        name="distirct"
                        defaultValue={{
                          label: `${viewData.district}`,
                          value: `${viewData.district}`,
                        }}

                        placeholder={i18n.t("SelectDistrict")}
                        isSearchable={true}
                        isLoading={isLoading}
                        options={districtList.map((opt) => ({
                          label:
                            i18n.language == "en" || i18n.language == "en-US"
                              ? opt.districtName
                              : opt.districtName_KA,
                          value: opt.districtCode,
                        }))}
                        onChange={(code, option) => {
                          if (selectInputRefTaluk2.current) {
                            selectInputRefTaluk2.current.clearValue();
                            setTalukList([]);
                          }
                          if (selectInputRefHobli2.current) {
                            selectInputRefHobli2.current.clearValue();
                            setHobliList([]);
                          }



                          if (code) {
                            setFormData({ ...formData, district: code.label.toString(), district_Code: code.value });
                            setDistrict(code.label);
                            getTaluk(code.value);
                          }


                        }}
                      />

                    </div>

                  </div>
                  <div className="row m-1">
                    <div className="col-md-5 m-1">
                      <Form.Label className="star">
                        {i18n.t("BasicPersonal.Taluk")}
                      </Form.Label>
                      <Select
                        aria-label="Floating label select example"
                        isClearable={true}
                        ref={selectInputRefTaluk2}
                        name="taluk"
                        defaultValue={{
                          value: `${viewData.taluk}`,
                          label: `${viewData.taluk}`,
                        }}
                        className="select-dropdowns2"
                        options={TalukList.map((opt) => ({
                          label: i18n.language === "en" || i18n.language === "en-US" ? opt.talukOrTownName : opt.talukOrTownName_KA,
                          value: opt.talukOrTownCode,
                        }))}
                        onChange={(code, option) => {
                          if (selectInputRefHobli2.current) {
                            selectInputRefHobli2.current.clearValue();
                            setHobliList([])

                          }
                          if (code) {
                            setFormData({ ...formData, taluk: code.label.toString(), taluk_Code: code.value });
                            setTaluk(code.label);
                            getHobli(code.value);
                          }
                        }}
                        placeholder={i18n.t("SelectTaluk")}
                        isSearchable={true}

                      ></Select>

                    </div>
                    <div className="col-md-5 m-1">
                      <Form.Label className="star">
                        {i18n.t("BasicPersonal.Hobli")}
                      </Form.Label>

                      <Select
                        aria-label="Floating label select example"
                        isClearable={true}
                        name="hobli"
                        defaultValue={{
                          value: `${viewData.hobli}`,
                          label: `${viewData.hobli}`,
                        }}
                        className="select-dropdowns3"
                        ref={selectInputRefHobli2}
                        options={HobliList.map((opt) => ({
                          label: i18n.language === "en" || i18n.language === "en-US" ? opt.hobliOrZoneName : opt.hobliOrZoneName_KA,
                          value: opt.hobliOrZoneCode,
                        }))}
                        onChange={(code, option) => {
                          if (code) {
                            setHobli(code.label);

                            setFormData({ ...formData, hobli: code.label.toString(), hobli_Code: code.value });
                          }

                        }}
                        placeholder={i18n.t("SelectHobli")}
                        isSearchable={true}

                      />
                    </div>

                  </div>
                </div>
                <div class="modal-footer">
                  <button
                    type="button"
                    class="btn btn-secondary"
                    onClick={() => { setView(false) }}
                  >
                    {i18n.t("UserManagement.Cancel")}
                  </button>

                  <button className="btn btn-success m-1" onClick={() => {
                    onSubmitHandler()
                  }}>
                    {i18n.t("Save")}
                  </button>
                </div>
              </div>
            </div>
          </div>
        </Modal>
      </Form>
    </div>
  );
}

export default UserManagement;







