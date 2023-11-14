import { React, useRef, useState } from "react";
import { Stack, Row, Col, FormControl, FormLabel } from "react-bootstrap";
import { CustomCard } from "../../CommonComponent/CustomCard.js";
import { AccordionTable } from "../../CommonComponent/AccordionTable";
import AutorenewIcon from "@mui/icons-material/Autorenew";
import Select from "react-select";
import "../../CSS/Views/ViewerDashboard.css";
import Accordian from "../../CommonComponent/Accordian";
import { useEffect } from "react";
import { BasicSurveyService } from "../../_services/BasicSurveyService";
import "../../CSS/Views/Surveymanagement.css";
import { OfficeService } from "../../_services/officeService";
import i18n from "../../i18n/i18n.js";
import { useTranslation } from "react-i18next";
import { AdminNavbar } from "../../CommonComponent/AdminNavbar.js";
import { Link,useNavigate } from "react-router-dom";
import axios from "axios";
import { config } from "../../constants";

export const SurveyList = () => {
  const navigate = useNavigate();
  const User = JSON.parse(sessionStorage.getItem("LoginData"));
  if (User === "" || User == null) {
    sessionStorage.setItem("isLoggedIn", "Login");
    window.location.href = "/Login";
  } else {
    if (User.userRoles !== "Admin") {
      window.location.href = "/Common/ErrorPage";
    }
  }

  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Content-type": "application/json",
      "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
    },
   
  });
  const { t } = useTranslation();
  const [districtList, setDistrictList] = useState([]);
  const [SurveyList, setSurveyList] = useState([]);
  const [data, setData] = useState([]);
  const [memo, setMemo] = useState([]);
  const selectInputRefDistrictt = useRef();
  const selectInputRefTaluk = useRef();
  const selectInputRefHobli = useRef();
  const selectInputRefVillage = useRef();
  const status = useRef();
  const idRef = useRef();
  const surveyeeNameRef = useRef();
  const surveyorNameRef = useRef();
  const startDateRef = useRef();
  const endDateRef = useRef();
  const [filterList, setfilterList] = useState([]);
  const [filterModel, setfilterModel] = useState({
    District: null,
    Taluk: null,
    Hobli: null,
    Village: null,
    SurveyId: null,
    SurveyeeName: null,
    SurveyorName: null,
    Status: null,
    StartDate: null,
    EndDate: null,
  });
  const [DateFilter, setDateFilter] = useState([]);

  const basicSurveyDetails = async () => {
    const BasicSurveyDetails = await BasicSurveyService.getSurveyDetails();
    if (BasicSurveyDetails != null) {
      setData(BasicSurveyDetails.data);
      setSurveyList(BasicSurveyDetails.data);
      setfilterList(BasicSurveyDetails.data);
      setMemo(BasicSurveyDetails.data);
    }
  };
  const onClear = () => {
    selectInputRefDistrictt.current.clearValue();
    selectInputRefTaluk.current.clearValue();
    selectInputRefHobli.current.clearValue();
    selectInputRefVillage.current.clearValue();
    idRef.current.value = "";
    surveyeeNameRef.current.value = "";
    surveyorNameRef.current.value = "";
    startDateRef.current.value = "";
    endDateRef.current.value = "";
    status.current.clearValue();
    Statistics.Total = 0;
    Statistics.completed = 0;
    Statistics.pending = 0;
    Statistics.withAadhar = 0;
    Statistics.withoutAadhar = 0;

    setfilterModel({
      District: null,
      Taluk: null,
      Hobli: null,
      Village: null,
      SurveyId: null,
      SurveyeeName: null,
      SurveyorName: null,
      Status: null,
      StartDate: null,
      EndDate: null
    });
    setSurveyList(data);
    setfilterList(data);
  };

  useEffect(() => {
    Statistics.Total = 0;
    Statistics.completed = 0;
    Statistics.pending = 0;
    Statistics.withAadhar = 0;
    Statistics.withoutAadhar = 0;
  }, [data]);

  useEffect(() => {
    basicSurveyDetails();
  }, []);

 
  const [district, setDistrict] = useState(String);
  const [TalukList, setTalukList] = useState([]);
  const [Taluk, setTaluk] = useState(String);
  const [HobliList, setHobliList] = useState([]);
  const [Hobli, setHobli] = useState(String);
  const [VillageList, setVillageList] = useState([]);
  const [Village, setVillage] = useState(String);

  useEffect(() => {
    fetchDistrictList(0);
  }, []);

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

  var Statistics = {
    Total: 0,
    completed: 0,
    pending: 0,
    withAadhar: 0,
    withoutAadhar: 0,
  };

  for (var json in SurveyList) {
    Statistics.Total++;
    if (SurveyList[json].dbtekycDataId) {
      Statistics.withAadhar++;
    } else {
      Statistics.withoutAadhar++;
    }

    if (SurveyList[json].status === "Completed") {
      Statistics.completed++;
    }

    if (SurveyList[json].status === null) {
      Statistics.pending++;
    }
  }

  function ApplyFilter() {
    //Filter Survey By District
    debugger
    if (filterModel.District != null) {
      const DistrictSurvey = [];
      var localeDistrict = districtList.find((e) => {
        return filterModel.District===e.districtName || filterModel.District===e.districtName_KA
      })
      for (var survey in data) {
        if (data[survey].district === localeDistrict.districtName || data[survey].district===localeDistrict.districtName_KA) {
          DistrictSurvey.push(data[survey]);
        }
      }
      setSurveyList(DistrictSurvey);
      setfilterList(DistrictSurvey);
    }

    //Filter Survey By Taluk
    if (filterModel.Taluk!= null) {
      const TalukSurvey = [];
      var localeTaluk = TalukList.find((e) => {
        return filterModel.Taluk===e.talukOrTownName || filterModel.Taluk===e.talukOrTownName_KA
      })
      for (var survey in data) {
        if (data[survey].taluk === localeTaluk.talukOrTownName || data[survey].taluk===localeTaluk.talukOrTownName_KA) {
          TalukSurvey.push(data[survey]);
        }
      }
      setSurveyList(TalukSurvey);
      setfilterList(TalukSurvey);
    }

    //Filter Survey By Hobli
    if (filterModel.Hobli!= null) {
      const HobliSurvey = [];
       var localeHobli = HobliList.find((e) => {
        return filterModel.Hobli===e.hobliOrZoneName || filterModel.Hobli===e.hobliOrZoneName_KA
      })
      for (var survey in data) {
        if (data[survey].hobli === localeHobli.hobliOrZoneName || data[survey].hobli===localeHobli.hobliOrZoneName_KA) {
          HobliSurvey.push(data[survey]);
        }
      }
      setSurveyList(HobliSurvey);
      setfilterList(HobliSurvey);
    }

    //Filter Survey By Village
    if (filterModel.Village!= null) {
      const villageSurvey = [];
      var localeVillage = VillageList.find((e) => {
        return filterModel.Village===e.villageOrWardName || filterModel.Village===e.villageOrWardName_KA
      })
      for (var survey in data) {
        if (data[survey].villageOrWard === localeVillage.villageOrWardName || data[survey].villageOrWard===localeVillage.villageOrWardName_KA) {
          villageSurvey.push(data[survey]);
        }
      }
      setSurveyList(villageSurvey);
      setfilterList(villageSurvey);
    }

    //Filter Survey By Surveyee Name
    if (filterModel.SurveyeeName!= null) {
      setSurveyList(
        filterList.filter(
          (e) =>
            e.name
              .toLowerCase()
              .search(filterModel.SurveyeeName.toLowerCase()) >= 0
        )
      );
    }

    //Filter Survey By Surveyor Name
    if (filterModel.SurveyorName != null) {
      setSurveyList(
        filterList.filter((e) => {
          if (e.user !== null) {
            return (
              e.user.userName
                .toLowerCase()
                .search(filterModel.SurveyorName.toLowerCase()) >= 0
            );
          }
        })
      );
    }

    //Filter Survey By Surveyor Id
    if (filterModel.SurveyId!= null) {
      setSurveyList(
        filterList.filter((e) => e.surveyId.toLowerCase() === filterModel.SurveyId.toLowerCase())
      );
    }
    //Filter Survey By Date
    debugger
    if (filterModel.StartDate!= null) {
      if (filterModel.EndDate!= null) {
        var startDate = filterModel.StartDate;
        var endDate = filterModel.EndDate;
        var dateFilter = [];
        for (let i = 0; i < filterList.length; i++) {
          if (
            filterList[i].created_Date.slice(0, 10) >= startDate &&
            filterList[i].created_Date.slice(0, 10) <= endDate
          ) {
            dateFilter.push(filterList[i]);
          }
        }
        setSurveyList(dateFilter);
        setDateFilter(dateFilter);
      } else {
      }
    }

    //Filter Survey By Status
    if (filterModel.Status!= null) {
      var SurveyStatus = [];
      setSurveyList([]);
      var filter = filterModel.EndDate != null ? DateFilter : filterList;
      if (
        filterModel.Status === "Completed" ||
        filterModel.Status === "ಪೂರ್ಣಗೊಂಡಿದೆ"
      ) {
        for (var survey in filter) {
          if (filter[survey].status === "Completed") {
            SurveyStatus.push(filter[survey]);
          }
        }
        setSurveyList(SurveyStatus);
        return;
      }

      if (
        filterModel.Status === "Pending" ||
        filterModel.Status === "ಬಾಕಿಯಿದೆ"
      ) {
        for (var survey in filter) {
          if (
            filter[survey].status !== filterModel.Status &&
            filter[survey].status !== "Completed"
          ) {
            SurveyStatus.push(filter[survey]);
          }
        }
        setSurveyList(SurveyStatus);
        return;
      }
    }
  }

  return (
    <>
      <AdminNavbar
        data={{ Title: i18n.t("Dashboard.SurveyManagement") }}
      ></AdminNavbar>
      <nav aria-label="breadcrumb" className="mx-3">
        <ol class="breadcrumb">
          <li class="breadcrumb-item">
            <Link to="/Admin/Dashboard" className="text-decoration-none">
              {i18n.t("admin")}
            </Link>
          </li>

          <li class="breadcrumb-item active" aria-current="page">
            {i18n.t("Dashboard.SurveyManagement")}
          </li>
        </ol>
      </nav>
      <Stack direction="vertical" className="m-3">
        <Accordian title={i18n.t("SurveySearch")}>
          <Row>
            <Col md="2">
              <Select
                ref={selectInputRefDistrictt}
                isClearable={true}
                name="distirct"
                className="select-dropdowns"
                placeholder={i18n.t("SelectDistrict")}
                isSearchable={true}
                options={districtList.map((opt) => ({
                  label:
                    i18n.language === "en"
                      ? opt.districtName
                      : opt.districtName_KA,
                  value: opt.districtCode,
                }))}
                onChange={(code, option) => {
                  if (selectInputRefTaluk.current) {
                    selectInputRefTaluk.current.clearValue();
                    setTalukList([]);
                  }

                  if (selectInputRefHobli.current) {
                    selectInputRefHobli.current.clearValue();
                    setHobliList([]);
                  }

                  if (selectInputRefVillage.current) {
                    selectInputRefVillage.current.clearValue();
                    setVillageList([]);
                  }
                  if (code) {
                    setDistrict(code.label);
                    getTaluk(code.value);
                    setfilterModel({
                      ...filterModel,
                      District: code.label,
                      Taluk: null,
                      Hobli: null,
                      Village: null,
                    });
                    // getSurveyByDistrict(code.label);
                  }
                }}
              ></Select>
            </Col>
            <Col md="2">
              <Select
                aria-label="Floating label select example"
                ref={selectInputRefTaluk}
                isClearable={true}
                name="taluk"
                className="select-dropdowns"
                options={TalukList.map((opt) => ({
                  label:
                    i18n.language === "en"
                      ? opt.talukOrTownName
                      : opt.talukOrTownName_KA,
                  value: opt.talukOrTownCode,
                }))}
                onChange={(code, option) => {
                  if (selectInputRefHobli.current) {
                    selectInputRefHobli.current.clearValue();
                    setHobliList([]);
                  }

                  if (selectInputRefVillage.current) {
                    selectInputRefVillage.current.clearValue();
                    setVillageList([]);
                  }

                  if (code) {
                    setTaluk(code.label);
                    getHobli(code.value);
                    // getSurveyByTaluk(code.label);
                    setfilterModel({
                      ...filterModel,
                      Taluk: code.label,
                      Hobli: null,
                      Village: null,
                    });
                  }
                }}
                pla
                placeholder={i18n.t("SelectTaluk")}
                isSearchable={true}
              ></Select>
            </Col>
            <Col md="2">
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                name="hobli"
                ref={selectInputRefHobli}
                className="select-dropdowns"
                options={HobliList.map((opt) => ({
                  label:
                    i18n.language === "en"
                      ? opt.hobliOrZoneName
                      : opt.hobliOrZoneName_KA,
                  value: opt.hobliOrZoneCode,
                }))}
                onChange={(code, option) => {
                  if (selectInputRefVillage.current) {
                    selectInputRefVillage.current.clearValue();
                    setVillageList([]);
                  }
                  if (code) {
                    setHobli(code.label);
                    getVillage(code.value);
                    // getSurveyByHobli(code.label);
                    setfilterModel({
                      ...filterModel,
                      Hobli: code.label,
                      Village: null,
                    });
                  }
                }}
                placeholder={i18n.t("SelectHobli")}
                isSearchable={true}
              ></Select>
            </Col>
            <Col md="2">
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                name="village"
                ref={selectInputRefVillage}
                className="select-dropdowns"
                options={VillageList.map((opt) => ({
                  label:
                    i18n.language === "en"
                      ? opt.villageOrWardName
                      : opt.villageOrWardName_KA,
                  value: opt.villageOrWardCode,
                }))}
                onChange={(code, option) => {
                  if (code) {
                    setVillage(code.label);
                    // getSurveyByVillageOrWard(code.label);
                    setfilterModel({ ...filterModel, Village: code.label });
                  }
                }}
                placeholder={i18n.t("SelectVillage")}
                isSearchable={true}
              ></Select>
            </Col>
            <Col md="2">
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                name="status"
                ref={status}
                className="select-dropdowns"
                onChange={(code, option) => {
                  if (code) {
                    // getSurveyByStatus(code.label);
                    setfilterModel({ ...filterModel, Status: code.label });
                  }
                }}
                options={[
                  {
                    label: i18n.t("completed"),
                    value: "Completed",
                  },
                  {
                    label: i18n.t("pending"),
                    value: "Pending",
                  },
                ]}
                placeholder={i18n.t("SelectByStatus")}
                isSearchable={true}
              ></Select>
            </Col>
            <Col md="2">
              <FormControl
                name="Survey Id"
                ref={idRef}
                onChange={(e) => {
                  setfilterModel({
                    ...filterModel,
                    SurveyId: e.target.value.replace(/[<>]/gi, ""),
                  });
                }}
                className="select-dropdowns"
                placeholder={i18n.t("SearchBySurveyId")}
              />
            </Col>

            <Col md="2">
              <FormControl
                isClearable={true}
                onChange={(e) => {
                  setfilterModel({
                    ...filterModel,
                    SurveyeeName: e.target.value.replace(/[<>]/gi, ""),
                  });
                }}
                ref={surveyeeNameRef}
                className="select-dropdowns"
                placeholder={i18n.t("SearchBySurveyeeName")}
                isSearchable={true}
              />
            </Col>

            <Col md="2">
              <FormControl
                ref={surveyorNameRef}
                onChange={(e) => {
                  setfilterModel({
                    ...filterModel,
                    SurveyorName: e.target.value.replace(/[<>]/gi, ""),
                  });
                }}
                className="select-dropdowns"
                placeholder={i18n.t("SearchBySurveyorName")}
              />
            </Col>

            <Col md="2">
              <div className="d-flex align-items-center">
                <FormLabel className="mt-2 mx-3">
                  {i18n.t("From")}:&nbsp;
                </FormLabel>
                <FormControl
                  md="2"
                  ref={startDateRef}
                  onChange={(e) => {
                    setfilterModel({
                      ...filterModel,
                      StartDate: e.target.value,
                    });
                  }}
                  type="date"
                  className="mt-2 fromDate"
                  placeholder={i18n.t("Search By Date")}
                />
              </div>
            </Col>
            <Col md="2">
              <div className="d-flex align-items-center">
                <FormLabel className="mt-2 mx-3">
                  {i18n.t("To")}:&nbsp;
                </FormLabel>
                <FormControl
                  ref={endDateRef}
                  md="2"
                  onChange={(e) => {
                    setfilterModel({ ...filterModel, EndDate: e.target.value });
                  }}
                  type="date"
                  className="mt-2 toDate"
                  placeholder={i18n.t("Search By Date")}
                />
              </div>
            </Col>

            <Col className="mt-2">
              <button
                className="btn btn-success mx-1"
                onClick={() => {
                  ApplyFilter();
                }}
              >
                Apply Filter
              </button>
              <AutorenewIcon
                color="primary"
                onClick={() => {
                  onClear();
                }}
              />
            </Col>
          </Row>
        </Accordian>
      </Stack>

      <Stack direction="vertical" gap={2} className="m-3">
        <Accordian title={i18n.t("statistics")}>
          <div className="row g-2">
            <Row className="justify-content-center">
              <Col xs={3} lg={2}>
                <CustomCard
                  cardTitle={i18n.t("total")}
                  backgroundColor="#f3ca3e"
                  cardValue={"" + Statistics.Total}
                  cardNum="one"
                />
              </Col>
              <Col xs={3} lg={2}>
                <CustomCard
                  cardTitle={i18n.t("completed")}
                  backgroundColor="#62a76c"
                  cardValue={"" + Statistics.completed}
                  cardNum="two"
                />
              </Col>
              <Col xs={3} lg={2}>
                <CustomCard
                  cardTitle={i18n.t("pending")}
                  backgroundColor="#ff5f58"
                  cardValue={"" + Statistics.pending}
                  cardNum="three"
                />
              </Col>

              <Col xs={3} lg={2}>
                <CustomCard
                  cardTitle={i18n.t("with")}
                  backgroundColor="#5098a7"
                  cardValue={"" + Statistics.withAadhar}
                  cardNum="four"
                />
              </Col>

              <Col xs={3} lg={2}>
                <CustomCard
                  cardTitle={i18n.t("without")}
                  backgroundColor="#808bde"
                  cardValue={"" + Statistics.withoutAadhar}
                  cardNum="five"
                />
              </Col>
            </Row>
          </div>
        </Accordian>
      </Stack>

      <Stack className="m-3">
        <Accordian title={i18n.t("Dashboard.List")}>
          <Row className="m-3">
            <Col>
              <AccordionTable columns="Name" data={SurveyList} filterModel={filterModel} total={Statistics.Total} />
            </Col>
          </Row>
        </Accordian>
      </Stack>
    </>
  );
};

export default SurveyList;
