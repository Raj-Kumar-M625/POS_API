import React, { useEffect, useState } from "react";
import { Form } from "react-bootstrap";
import { Link, useLocation, useNavigate, useParams } from "react-router-dom";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";
import { BasicSurveyService } from "../../_services/BasicSurveyService";
import { Controller, useForm } from "react-hook-form";
import { config } from "../../constants";
import { OfficeService } from "../../_services/officeService";
import Select from "react-select";

import i18n from "../../i18n/i18n";
import { useTranslation } from "react-i18next";
import { useRef } from "react";
import Accordian from "../../CommonComponent/Accordian";
import { AdminNavbar } from "../../CommonComponent/AdminNavbar";
import "../../CSS/BreadCrumb.css";
import axios from "axios";

function SurveyInfo_BasicDetails() {
  const navigate = useNavigate();
  const User = JSON.parse(sessionStorage.getItem("LoginData"));
  if (User === "" || User == null) {
    sessionStorage.setItem("isLoggedIn", "Login");
    window.location.href = "/Login";
  }

  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Content-type": "application/json",
      "Authorization": `Bearer ${sessionStorage.getItem("token")}`
    },

  });

  const { register, handleSubmit, control } = useForm();
  const { t } = useTranslation();
  const params = useParams();
  const location = useLocation();
  const checkBoxRef = useRef();
  const [data, setData] = useState([]);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    const url = `${config.localUrl}BasicSurveyDetail/getBasicSurveyById?id=${params.SurveyId}`;
    privateAxios.get(url)
      .then((res) => {
        return res.data;
      })
      .then((response) => {
        debugger;
        if (response.address === response.presentAddress) {
          checkBoxRef.current.checked = true;
        }
        debugger;
        setData(response);
      });
  }

  return (
    <>
      <AdminNavbar data={{ Title: i18n.t("BasicDetails") }}></AdminNavbar>
      <nav aria-label="breadcrumb" className="mx-3">
        {location.state.origin === "Surveyor" ? <ol className="breadcrumb">
          <li class="breadcrumb-item"><Link to="/Surveyor/SurveyDashboard" className="textDecoration" >{i18n.t("Home")}</Link></li>
          <li class="breadcrumb-item"><Link to="/Surveyor/SurveyList2" className="textDecoration" >{i18n.t("surveyList")}</Link></li>
          <li class="breadcrumb-item"><Link className="textDecoration" to={`/Surveyor/Survey/SurveyDetails/${params.SurveyId}/${params.Name}`} state={location.state} >{i18n.t('SurveyDetails')}</Link></li>
          <li class="breadcrumb-item active" aria-current="page">{i18n.t('BasicSurvey')}</li>
        </ol> :
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><Link className="textDecoration" to="/Admin/Dashboard" >{i18n.t('admin')}</Link></li>
            <li class="breadcrumb-item"><Link className="textDecoration" to="/Surveyor/SurveyList" >{i18n.t('Dashboard.SurveyManagement')}</Link></li>
            <li class="breadcrumb-item"><Link className="textDecoration" to={`/Surveyor/Survey/SurveyDetails/${params.SurveyId}/${params.Name}`} state={location.state} >{i18n.t('SurveyDetails')}</Link></li>
            <li class="breadcrumb-item active" aria-current="page">{i18n.t('BasicSurvey')}</li>

          </ol>
        }

      </nav>
      <div className="container d-flex mx-auto justify-content-center">
        <h5 className="m-3 header">
          {i18n.t("surveyee")}:{" "}
          <b className="text-primary">{params.SurveyId}</b>
        </h5>
        <h5 className="m-3 header">
          {i18n.t("surveyeeName")}:{" "}
          <b className="text-primary">{params.Name}</b>
        </h5>
        <h5 className="m-3 header">
          {i18n.t("status")}: <b className="text-primary">{location.state.status}</b>
        </h5>
      </div>
      <Form>
        <div className="container ">
          <Accordian title={i18n.t("BasicDetails")}>
            <div
              className="justify-content-center"
              
            >
              <div className="row">
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.Name")}
                  </Form.Label>
                  <input
                    className="form-control"
                    type="text"
                    defaultValue={data.name}
                    disabled={true}
                  />
                </div>
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.DOB")}
                  </Form.Label>
                  <input
                    className="form-control"
                    type="text"
                    defaultValue={
                      data.dob === undefined
                        ? data.dob
                        : data.dob.substring(0, 10)
                    }
                    disabled={true}
                  />
                </div>
              </div>

              <div className="row">
                <div className="col m-2">
                  <Form.Label>{i18n.t("age")}</Form.Label>
                  <input
                    className="form-control"
                    type="text"
                    defaultValue={data.age}
                    disabled={true}
                  />
                </div>
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.Gender")}
                  </Form.Label>
                  <select className="form-select"
                    disabled={true}
                  >
                    <option disabled selected value={data.genderByBirth}>
                      {data.genderByBirth}
                    </option>
                  </select>
                </div>
              </div>

              <div className="row">
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("mobilenumber")}
                  </Form.Label>
                  <input
                    className="form-control"
                    type="text"
                    disabled={true}
                    defaultValue={data.number}
                  />
                </div>
                <div className="col m-2">
                  <Form.Label>{i18n.t("email")}</Form.Label>
                  <input
                    className="form-control"
                    type="text"
                    disabled={true}
                    defaultValue={data.email}
                  />
                </div>
              </div>

              <div className="row">
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.Address")}
                  </Form.Label>
                  <input
                    className="form-control"
                    type="text"
                    disabled={true}
                    defaultValue={data.address}
                  />
                </div>
                <div className="col m-2">
                  <Form.Label className="star">
                    {i18n.t("BasicPersonal.District")}
                  </Form.Label>

                  <select className="form-select"
                    name="district"
                    disabled={true}
                    value={data.district}
                  >
                    <option selected>{data.district}</option>
                  </select>
                </div>

                <div className="row">
                  <div className="col m-2">
                    <Form.Label className="star">
                      {i18n.t("BasicPersonal.Taluk")}
                    </Form.Label>
                    <select className="form-select"
                      disabled={true}
                      name="taluk"
                    >
                      <option selected>{data.taluk}</option>
                    </select>
                  </div>
                  <div className="col m-2">
                    <Form.Label className="star">
                      {i18n.t("BasicPersonal.Hobli")}
                    </Form.Label>
                    <select className="form-select"
                      name="hobli"
                      disabled={true}
                    >
                      <option selected>{data.hobli}</option>
                    </select>
                  </div>
                </div>

                <div className="row">
                  <div className="col m-2">
                    <Form.Label className="star">
                      {i18n.t("BasicPersonal.Village")}
                    </Form.Label>

                    <select className="form-control"

                      name="village"
                      disabled={true}
                    >
                      <option selected>{data.villageOrWard}</option>
                    </select>
                  </div>
                  <div className="col m-2">
                    <Form.Label className="star">
                      {i18n.t("BasicPersonal.PinCode")}
                    </Form.Label>
                    <input
                      disabled={true}
                      type="text"
                      className="form-control"
                      defaultValue={data.pinCode}
                    />
                  </div>
                </div>
              </div>
            </div>

            <div className="row">
              <div>
                <input
                  type="checkbox"
                  disabled
                  className="col m-2"
                  ref={checkBoxRef}
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
                  defaultValue={data.presentAddress}
                  className="form-control"
                  type="text"
                  disabled={true}
                />
              </div>
              <div className="col m-2">
                <Form.Label className="star">
                  {i18n.t("BasicPersonal.District")}
                </Form.Label>

                <select className="form-select"
                  disabled={true}
                > <option selected>{data.presentDistrict}</option></select>
              </div>
            </div>
            <div className="row">
              <div className="col m-2">
                <Form.Label className="star">
                  {i18n.t("BasicPersonal.Taluk")}
                </Form.Label>
                <select className="form-select" disabled>
                  <option selected>{data.presentTaluk}</option>
                </select>
              </div>
              <div className="col m-2">
                <Form.Label className="star">
                  {i18n.t("BasicPersonal.Hobli")}
                </Form.Label>

                <select className="form-select"
                  disabled>
                  <option selected>{data.presentHobli}</option>
                </select>
              </div>
            </div>

            <div className="row">
              <div className="col m-2">
                <Form.Label className="star">
                  {i18n.t("BasicPersonal.Village")}
                </Form.Label>

                <select className="form-select" disabled>
                  <option selected>{data.presentVillageOrWard}</option>
                </select>
              </div>
              <div className="col m-2">
                <Form.Label className="star">
                  {i18n.t("BasicPersonal.PinCode")}
                </Form.Label>
                <input
                  disabled={true}
                  defaultValue={data.presentPinCode}
                  className="form-control"
                />
              </div>
            </div>
          </Accordian>
        </div>
      </Form>
    </>
  );
}

export default SurveyInfo_BasicDetails;
