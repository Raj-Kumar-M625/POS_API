import React, { useEffect, useState } from "react";
import { Form } from "react-bootstrap";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import { BasicSurveyService } from "../../_services/BasicSurveyService";
import { useForm } from "react-hook-form";
import { config } from "../../constants";
import { OfficeService } from "../../_services/officeService";
import Select from "react-select";
import i18n from "../../i18n/i18n";
import { useRef } from "react";
import Accordian from "../../CommonComponent/Accordian";
import AadharVerifyScreen from "./AadharVerifyScreen";
import spinner from "../../Assets/Image/spinner.gif";
import "../../CSS/Views/PersonalDetails.css";
import axios from "axios";


function PersonalDetailsScreen() {

  const navigate = useNavigate();
  
  const User = JSON.parse(sessionStorage.getItem("LoginData"));
  if (User === "" || User == null) {
    sessionStorage.setItem("isLoggedIn", "Login");
    window.location.href = "/Login";
  }else{
    if(User.userRoles !== "Surveyor"){
     window.location.href = "/Common/ErrorPage"
    }
  }

  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Content-type": "application/json",
      "Authorization": `Bearer ${sessionStorage.getItem("token")}`
    },

  });


  const { register, handleSubmit, control, setValue } = useForm();
  const checkBoxRef = useRef();
  const [districtList, setDistrictList] = useState([]);
  const [TalukList, setTalukList] = useState([]);
  const [HobliList, setHobliList] = useState([]);
  const [VillageList, setVillageList] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [loading, setLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState(null);
  const [district, setDistrict] = useState(String);
  const [Taluk, setTaluk] = useState(String);
  const [Hobli, setHobli] = useState(String);
  const _id = useLocation();
  const selectInputRefTaluk = useRef();
  const selectInputRefHobli = useRef();
  const selectInputRefVillage = useRef();
  const [Aadhar, setAadhar] = useState(false);
  const [statusCode, setStatusCode] = useState(0);
  
  const [location, setLocation] = useState({
    state: {
      id: "",
      name: "",
      locale: "",
      origin: _id.state.origin,
      status: ""
    },
  });

  const [BasicDetails, setBasicDetails] = useState({
    name: "",
    dob: "",
    genderByBirth: "",
    number: "",
    email: "",
    address: "",
    district: "",
    SurveyId: "",
    taluk: "",
    hobli: "",
    villageOrWard: "",
    pincode: "",
    PresentAddress: "",
    Presentdistrict: "",
    Presenttaluk: "",
    Presenthobli: "",
    PresentVillageOrWard: "",
    PresentPincode: "",
    DBTEKYCDataId: 0,
    AadharStatus: "",
    locale: `${i18n.language}`,
    UserId: User.userId ? User.userId.toString() : "",
  });

  useEffect(() => {
    fetchData()
  }, []);

  async function fetchData() {
    const url = `${config.localUrl}BasicSurveyDetail/getBasicSurveyById?id=${_id.state.id}`;
    if (_id.state.id != null && _id.state.id.length > 0) {
      await privateAxios.get(url)
        .then((res) => {
          
          return res.data;
        })
        .then((response) => {
          
          setLocation({
            state: {
              id: response.surveyId,
              name: response.name,
              locale: response.locale,
              origin: _id.state.origin,
              status: response.status
            },
          });
          if (response.presentAddress === response.address) {
            checkBoxRef.current.checked = true;
          }

          setBasicDetails({
            name: response.name,
            dob: `${response.dob.slice(0, 10)}`,
            age: response.age,
            genderByBirth: response.genderByBirth,
            number: response.number,
            email: response.email,
            address: response.address,
            district: response.district,
            taluk: response.taluk,
            hobli: response.hobli,
            villageOrWard: response.villageOrWard,
            pincode: response.pinCode,
            surveyId: response.surveyId,
            locale: response.locale,
            PresentAddress: response.presentAddress,
            Presentdistrict:
              response.presentDistrict === null ? "" : response.presentDistrict,
            Presenttaluk:
              response.presentTaluk === null ? "" : response.presentTaluk,
            Presenthobli:
              response.presentHobli === null ? "" : response.presentHobli,
            PresentVillageOrWard:
              response.presentVillageOrWard === null
                ? ""
                : response.presentVillageOrWard,
            PresentPincode: response.presentPinCode,
            
          });
        });
    }
  }

  useEffect(() => {
    fetchDistrictList();
  }, []);

  const fetchDistrictList = async () => {
    setLoading(true);
    const res = await OfficeService.getAllOfficeList(29);
    debugger;
    if (res?.errorCode) {
      setErrorMessage(res.messsage);
    } else {
      setDistrictList(res.data);
    }
    setIsLoading(false);
    setLoading(false);
  };

  const getTaluk = async (distCode) => {
    const res = await OfficeService.getTalukList(distCode);
    debugger;
    if (res?.errorCode) {
      setErrorMessage(res.message);
    } else {
      setTalukList(res.data);
    }
  };

  const getHobli = async (talukCode) => {
    const res = await OfficeService.getHobliList(talukCode);
    if (res?.errorCode) {
      setErrorMessage(res.message);
    } else {
      setHobliList(res.data);
    }
  };

  const getVillage = async (hobliCode) => {
    const res = await OfficeService.getVillageList(hobliCode);
    if (res?.errorCode) {
      setErrorMessage(res.message);
    } else {
      setVillageList(res.data);
    }
  };


  const onSubmitHandler = async () => {
    if (_id.state.name.length > 0) {
      debugger;

      await BasicSurveyService.SaveBasicSurveyDetail(BasicDetails).then((res) => {
        debugger
        if(res != null){
          navigate("/Surveyor/SurveyDetails", {
            state: {
              id: res.data.surveyId,
              name: res.data.name,
              locale: res.data.locale,
              origin: _id.state.origin
            },
          });
        }
       

        return;
      })

    }
    debugger;
    await BasicSurveyService.SaveBasicSurveyDetail(BasicDetails).then((res) => {
      setAadhar(true);
      debugger
      if(res != null){
        setLocation({
          state: {
            id: res.data.surveyId,
            name: res.data.name,
            locale: res.data.locale,
            origin: _id.state.origin
          },
        });
      }
      
      navigate("/Surveyor/SurveyDetails", {
        state: {
          id: res.data.surveyId,
          name: res.data.name,
          locale: res.data.locale,
          origin: _id.state.origin
        },
      });
    })



  };

  return (
    <>
      <NavbarTitle data={{ Title: i18n.t("BasicDetails") }}></NavbarTitle>
      {location.state.origin === "SurveyList" ? (
        <>
          <nav aria-label="breadcrumb" className="mx-3">
            <ol className="breadcrumb">
              <li class="breadcrumb-item">
                <Link
                  to="/Surveyor/SurveyDashboard"
                  className="textDecoration"
                >
                  {i18n.t("Home")}
                </Link>
              </li>
              <li class="breadcrumb-item">
                <Link
                  to="/Surveyor/SurveyList2"
                  className="textDecoration"
                >
                  {i18n.t("surveyList")}
                </Link>
              </li>
              <li class="breadcrumb-item">
                <Link
                  to="/Surveyor/SurveyDetails"
                  className="textDecoration"
                  state={{
                    id: location.state.id,
                    name: location.state.name,
                    locale: location.state.locale,
                    origin: _id.state.origin,
                    status: location.state.status
                  }}
                >
                  {i18n.t("SurveyDetails")}
                </Link>
              </li>

              <li class="breadcrumb-item active" aria-current="page">
                {i18n.t("BasicDetails")}
              </li>
            </ol>
          </nav>
        </>
      ) : (
        <>
          <nav aria-label="breadcrumb" className="mx-3">
            <ol className="breadcrumb">
              <li className="breadcrumb-item">
                <Link
                  to="/Surveyor/SurveyDashboard"
                  className="textDecoration"
                >
                  {i18n.t("Home")}
                </Link>
              </li>
              <li className="breadcrumb-item">
                <a
                  className="text-primary textDecoration"
                  onClick={() => {
                    navigate("/Surveyor/SurveyDetails", {
                      state: {
                        id: location.state.id,
                        name: location.state.name,
                        locale: location.state.locale,
                        origin: _id.state.origin,
                        status: location.state.status
                      },
                    });
                  }}
                  href
                >
                  {i18n.t("SurveyDashboard")}
                </a>
              </li>

              <li className="breadcrumb-item active" aria-current="page">
                {i18n.t("BasicDetails")}
              </li>
            </ol>
          </nav>
        </>
      )}

      <div className="container d-flex mx-auto justify-content-center">
        {location.state.name.length > 0 ? (
          <>
            <h5 className="m-3 header">
              {i18n.t("surveyee")}:{" "}
              <b className="text-primary">{location.state.id}</b>
            </h5>
            <h5 className="m-3 header">
              {i18n.t("surveyeeName")}:{" "}
              <b className="text-primary">{location.state.name}</b>
            </h5>
          </>
        ) : (
          <></>
        )}
      </div>
      {_id.state.name.length > 0 ? (
        <></>
      ) : (
        <AadharVerifyScreen
          location={location}
          Aadhar={Aadhar}
          setBasicDetails={setBasicDetails}
          BasicDetails={BasicDetails}
        />
      )}
      <Form onSubmit={handleSubmit(onSubmitHandler)}>
        <div className="container" >
          <Accordian title={i18n.t("BasicDetails")}>
            <div
              className="justify-content-center accordian"
            >
              <div className="row">
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.Name")}
                  </Form.Label>
                  <Form.Control
                    defaultValue={BasicDetails.name}
                    type="text"
                    {...register("name", {
                      onChange: (e) => {
                        setBasicDetails({
                          ...BasicDetails,
                          name: e.target.value.replace(/[<>]/gi, ''),
                        });
                      },
                    })}
                    required
                    autoComplete="off"
                  />
                </div>
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.DOB")}
                  </Form.Label>
                  <Form.Control
                    defaultValue={BasicDetails.dob}
                    type="date"
                    max={new Date().toISOString().split("T")[0]}
                    {...register("dob", {
                      onChange: (e) => {
                        setValue(
                          "age",
                          new Date().getFullYear() -
                          new Date(e.target.value).getFullYear()
                        );
                        setBasicDetails({
                          ...BasicDetails,
                          dob: e.target.value,
                          age:
                            new Date().getFullYear() -
                            new Date(e.target.value).getFullYear(),
                        });
                      },
                    })}
                    required

                  />
                </div>
              </div>

              <div className="row">
                <div className="col m-2">
                  <Form.Label>{i18n.t("age")}</Form.Label>
                  <Form.Control
                    defaultValue={BasicDetails.age}
                    onChange={(e) => (BasicDetails.age = e.target.value.replace(/[<>]/gi, ''))}
                    className="form-control"
                    type="text"
                    {...register("age")}
                    autoComplete="off"
                  />
                </div>

                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.Gender")}
                  </Form.Label>
                  <select
                    className="form-select"
                    {...register("genderByBirth", {
                      onChange: (e) => {
                        setBasicDetails({
                          ...BasicDetails,
                          genderByBirth: e.target.value.replace(/[<>]/gi, ''),
                        });
                      },
                    })}
                    required
                    onChange={(e) =>
                      (BasicDetails.genderByBirth = e.target.value.replace(/[<>]/gi, ''))
                    }

                  >
                    <option disabled selected defaultValue="">
                      {BasicDetails.genderByBirth}
                    </option>
                    <option defaultValue="male">{i18n.t("Male")}</option>
                    <option defaultValue="female">{i18n.t("Female")}</option>
                    <option defaultValue="intersex">{i18n.t("Intersex")}</option>
                  </select>
                </div>
              </div>

              <div className="row">
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("mobilenumber")}
                  </Form.Label>
                  <input
                    defaultValue={BasicDetails.number}
                    minLength={10}
                    maxLength={10}
                    pattern="[6-9]{1}[0-9]{9}"
                    className="form-control"
                    type="text"

                    {...register("number", {
                      onChange: (e) => {
                        setBasicDetails({
                          ...BasicDetails,
                          number: e.target.value.replace(/[<>]/gi, ''),
                        });
                      },
                    })}
                    required
                    autoComplete="off"
                  />
                </div>

                <div className="col m-2">
                  <Form.Label>{i18n.t("email")}</Form.Label>
                  <input
                    defaultValue={BasicDetails.email}
                    className="form-control"
                    type="text"
                    autoComplete="off"
                    {...register("email", {
                      onChange: (e) => {
                        setBasicDetails({
                          ...BasicDetails,
                          email: e.target.value.replace(/[<>]/gi, ''),
                        });
                      },
                    })}
                  />
                </div>
              </div>

              <div className="row">
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.Address")}
                  </Form.Label>
                  <input
                    defaultValue={BasicDetails.address}
                    className="form-control"
                    type="text"
                    autoComplete="off"
                    {...register("address", {
                      onChange: (e) => {
                        setBasicDetails({
                          ...BasicDetails,
                          address: e.target.value.replace(/[<>]/gi, ''),
                        });
                      },
                    })}
                    required
                  />
                </div>
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.District")}
                  </Form.Label>

                  <Select
                    // aria-label="Floating label select example"
                    isClearable={true}
                    name="district"
                    value={BasicDetails.district.length > 0 && {
                      value: `${BasicDetails.district}`,
                      label: `${BasicDetails.district}`,
                                      }}

                    required={true}
                    options={districtList?.map((opt) => ({
                      label:
                        i18n.language === "kn"
                          ? opt.districtName_KA
                          : opt.districtName,
                      value: opt.districtCode,
                    }))}
                    onChange={(code, option) => {
                      setTalukList([]);
                      setHobliList([]);
                      setVillageList([]);

                      if (code) {
                        setBasicDetails({
                          ...BasicDetails,
                          district: code.label,
                          taluk: "",
                          hobli: "",
                          villageOrWard: ""
                        });
                        setDistrict(code.value);
                        getTaluk(code.value);
                      }
                    }}

                    placeholder={i18n.t("SelectDistrict")}
                    isSearchable={true}
                    isLoading={isLoading}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.Taluk")}
                  </Form.Label>
                  <Select

                    // aria-label="Floating label select example"
                    isClearable={true}
                    name="taluk"
                    value={ BasicDetails.taluk.length > 0 && {
                      value: `${BasicDetails.taluk}`,
                      label: `${BasicDetails.taluk}`,
                    }}
                    required={true}
                    options={TalukList.map((opt) => ({
                      label:
                        i18n.language === "kn"
                          ? opt.talukOrTownName_KA
                          : opt.talukOrTownName,
                      value: opt.talukOrTownCode,
                    }))}
                    onChange={(code, option) => {
                      setHobliList([]);
                      setVillageList([]);

                      if (code) {
                        setBasicDetails({
                          ...BasicDetails,
                          taluk: code.label,
                          hobli: "",
                          villageOrWard: ""
                        });
                        setTaluk(code.label);
                        getHobli(code.value);
                      }
                    }}
                    placeholder={i18n.t("SelectTaluk")}
                    isSearchable={true}
                  ></Select>
                </div>
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.Hobli")}
                  </Form.Label>

                  <Select
                    // aria-label="Floating label select example"
                    isClearable={true}
                    name="hobli"

                    ref={selectInputRefHobli}
                    required
                    value={BasicDetails.hobli.length > 0 && {
                      value: `${BasicDetails.hobli}`,
                      label: `${BasicDetails.hobli}`,
                    }}
                    options={HobliList.map((opt) => ({
                      label:
                        i18n.language === "kn"
                          ? opt.hobliOrZoneName_KA
                          : opt.hobliOrZoneName,
                      value: opt.hobliOrZoneCode,
                    }))}
                    onChange={(code, option) => {
                      setVillageList([]);

                      if (code) {
                        setBasicDetails({
                          ...BasicDetails,
                          hobli: code.label,
                          villageOrWard: ""
                        });
                        setHobli(code.label);
                        getVillage(code.value);
                      }
                    }}
                    placeholder={i18n.t("SelectHobli")}
                    isSearchable={true}
                  ></Select>
                </div>
              </div>

              <div className="row">
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.Village")}
                  </Form.Label>

                  <Select
                    // aria-label="Floating label select example"
                    isClearable={true}
                    name="villageOrWard"
                    value={ BasicDetails.villageOrWard.length > 0 && {
                      value: `${BasicDetails.villageOrWard}`,
                      label: `${BasicDetails.villageOrWard}`,
                    }}

                    required
                    options={VillageList.map((opt) => ({
                      label:
                        i18n.language === "kn"
                          ? opt.villageOrWardName_KA
                          : opt.villageOrWardName,
                      value: opt.villageOrWardCode,
                    }))}
                    onChange={(code, option) => {
                      if (code) {
                        setBasicDetails({
                          ...BasicDetails,
                          villageOrWard: code.label,
                        });
                        register("villageOrWard", { value: code.label });
                      }
                    }}
                    placeholder={i18n.t("SelectVillage")}
                    isSearchable={true}
                  ></Select>
                </div>
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.PinCode")}
                  </Form.Label>
                  <input

                    onChange={(e) => (BasicDetails.pincode = e.target.value.replace(/[<>]/gi, ''))}
                    type="text"
                    defaultValue={BasicDetails.pincode}
                    className="form-control"
                    maxLength={6}
                    minLength={6}
                    autoComplete="off"
                    {...register("pincode", {
                      onChange: (e) => {
                        setBasicDetails({
                          ...BasicDetails,
                          pincode: e.target.value.replace(/[<>]/gi, ''),
                        });
                      },
                    })}
                    required
                  />
                </div>
              </div>
              <div className="row">
                <div>
                  <input
                    type="checkbox"
                    className="col m-2"
                    ref={checkBoxRef}
                    autoComplete="off"
                    onChange={(e) => {
                      if (e.target.checked) {
                        setBasicDetails({
                          ...BasicDetails,
                          PresentAddress: BasicDetails.address,
                          Presentdistrict: BasicDetails.district,
                          Presenttaluk: BasicDetails.taluk,
                          Presenthobli: BasicDetails.hobli,
                          PresentVillageOrWard: BasicDetails.villageOrWard,
                          PresentPincode: BasicDetails.pincode,
                        });
                      }

                      if (!e.target.checked) {
                        setBasicDetails({
                          ...BasicDetails,
                          PresentAddress: "",
                          Presentdistrict: "",
                          Presenttaluk: "",
                          Presenthobli: "",
                          PresentVillageOrWard: "",
                          PresentPincode: "",
                        });
                      }
                    }}
                  />
                  {i18n.t("sameAsPermanentAddress")}
                </div>
              </div>

              <div className="row">
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("PresentAddress")}
                  </Form.Label>
                  <input
                    defaultValue={BasicDetails.PresentAddress}
                    className="form-control"
                    type="text"
                    autoComplete="off"
                    {...register("PresentAddress", {
                      onChange: (e) => {
                        setBasicDetails({
                          ...BasicDetails,
                          PresentAddress: e.target.value.replace(/[<>]/gi, ''),
                        });
                      },
                    })}
                    required
                  />
                </div>
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.District")}
                  </Form.Label>

                  <Select
                    // aria-label="Floating label select example"
                    isClearable={true}
                    value={BasicDetails.Presentdistrict.length > 0 && {
                      value: `${BasicDetails.Presentdistrict}`,
                      label: `${BasicDetails.Presentdistrict}`,
                    }}
                    name="Presentdistrict"

                    // styles={{
                    //   control: (baseStyles, state) => ({
                    //     ...baseStyles,
                    //   }),
                    // }}
                    required={true}
                    options={districtList.map((opt) => ({
                      label:
                        i18n.language === "kn"
                          ? opt.districtName_KA
                          : opt.districtName,
                      value: opt.districtCode,
                    }))}
                    onChange={(code, option) => {
                      setTalukList([]);
                      setHobliList([]);
                      setVillageList([]);

                      if (code) {
                        setBasicDetails({
                          ...BasicDetails,
                          Presentdistrict: code.label,
                          Presenttaluk: "",
                          Presenthobli: "",
                          PresentVillageOrWard: ""
                        });
                        setDistrict(code.value);
                        getTaluk(code.value);
                      }
                    }}
                    placeholder={i18n.t("SelectDistrict")}
                    isSearchable={true}
                    isLoading={isLoading}
                  />
                </div>
              </div>
              <div className="row">
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.Taluk")}
                  </Form.Label>
                  <Select
                    ref={selectInputRefTaluk}
                    // aria-label="Floating label select example"
                    isClearable={true}
                    value={ BasicDetails.Presenttaluk.length > 0 && {
                      value: `${BasicDetails.Presenttaluk}`,
                      label: `${BasicDetails.Presenttaluk}`
                    }}
                    name="Presenttaluk"
                    required={true}
                    options={TalukList.map((opt) => ({
                      label:
                        i18n.language === "kn"
                          ? opt.talukOrTownName_KA
                          : opt.talukOrTownName,
                      value: opt.talukOrTownCode,
                    }))}
                    onChange={(code, option) => {
                      setHobliList([]);
                      setVillageList([]);

                      if (code) {
                        setBasicDetails({
                          ...BasicDetails,
                          Presenttaluk: code.label,
                          Presenthobli: "",
                          PresentVillageOrWard: ""

                        });
                        setTaluk(code.label);
                        getHobli(code.value);
                      }
                    }}
                    placeholder={i18n.t("SelectTaluk")}
                    isSearchable={true}
                  ></Select>
                </div>
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.Hobli")}
                  </Form.Label>

                  <Select
                    // aria-label="Floating label select example"
                    isClearable={true}
                    name="Presenthobli"
                    value={ BasicDetails.Presenthobli.length > 0 && {
                      value: `${BasicDetails.Presenthobli}`,
                      label: `${BasicDetails.Presenthobli}`,
                    }}

                    ref={selectInputRefHobli}
                    required={true}
                    options={HobliList.map((opt) => ({
                      label:
                        i18n.language === "kn"
                          ? opt.hobliOrZoneName_KA
                          : opt.hobliOrZoneName,
                      value: opt.hobliOrZoneCode,
                    }))}
                    onChange={(code, option) => {
                      setVillageList([]);
                      if (code) {
                        setBasicDetails({
                          ...BasicDetails,
                          Presenthobli: code.label,
                          PresentVillageOrWard: ""
                        });
                        setHobli(code.label);
                        getVillage(code.value);
                      }
                    }}
                    placeholder={i18n.t("SelectHobli")}
                    isSearchable={true}
                  ></Select>
                </div>
              </div>

              <div className="row">
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.Village")}
                  </Form.Label>

                  <Select
                    // aria-label="Floating label select example"
                    isClearable={true}
                    name="PresentVillageOrWard"

                    value={ BasicDetails.PresentVillageOrWard.length > 0 && {
                      value: `${BasicDetails.PresentVillageOrWard}`,
                      label: `${BasicDetails.PresentVillageOrWard}`,
                    }}
                    required
                    ref={selectInputRefVillage}
                    options={VillageList.map((opt) => ({
                      label:
                        i18n.language === "kn"
                          ? opt.villageOrWardName_KA
                          : opt.villageOrWardName,
                      value: opt.villageOrWardCode,
                    }))}
                    onChange={(code, option) => {
                      if (code) {
                        setBasicDetails({
                          ...BasicDetails,
                          PresentVillageOrWard: code.label,
                        });
                      }
                    }}
                    placeholder={i18n.t("SelectVillage")}
                    isSearchable={true}
                  ></Select>
                </div>
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.PinCode")}
                  </Form.Label>
                  <input

                    onChange={(e) =>
                      (BasicDetails.PresentPincode = e.target.value.replace(/[<>]/gi, ''))
                    }
                    type="text"
                    defaultValue={BasicDetails.PresentPincode}
                    className="form-control"
                    maxLength={6}
                    minLength={6}
                    autoComplete="off"
                    {...register("PresentPincode", {
                      onChange: (e) => {
                        setBasicDetails({
                          ...BasicDetails,
                          pincode: e.target.value.replace(/[<>]/gi, ''),
                        });
                      },
                    })}
                    required
                  />
                </div>
              </div>
            </div>
            <div className="col-md-12  d-flex justify-content-end ">
              
              <button
                className="btn btn-success m-1 float-end"
                type="submit"
              >
                {statusCode === 0 ? i18n.t("Submit") : <>
                  <img src={spinner} width="24px" height="24px" alt=""/>
                </>}

              </button>
            </div>
          </Accordian>
        </div>
      </Form>
    </>
  );
}

export default PersonalDetailsScreen;
