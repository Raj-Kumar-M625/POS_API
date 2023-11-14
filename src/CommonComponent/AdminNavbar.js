import { CenterFocusStrong } from "@mui/icons-material";
import React from "react";
import { Navbar } from "react-bootstrap";
// import DropdownItem from "react-bootstrap/esm/DropdownItem";
import { Navigate, useNavigate } from "react-router-dom";
import i18n from "../i18n/i18n";
import Dropdown from "rsuite/Dropdown";
import "rsuite/dist/rsuite.min.css";
import "../CSS/Views/TitleNavbar.css"

export const AdminNavbar = (props) => {
  const { Title } = props.data;
  const navigate = useNavigate();
  return (
    <>
      <Navbar
        className="d-flex justify-content-between align-items-center navbarContainer"
      >

        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="28"
          height="28"
          fill="currentColor"
          className="bi bi-house-door-fill mx-3"
          viewBox="0 0 16 16"
          onClick={() => {
            debugger;
            navigate("/Admin/Dashboard");
          }}
        >
          <path d="M6.5 14.5v-3.505c0-.245.25-.495.5-.495h2c.25 0 .5.25.5.5v3.5a.5.5 0 0 0 .5.5h4a.5.5 0 0 0 .5-.5v-7a.5.5 0 0 0-.146-.354L13 5.793V2.5a.5.5 0 0 0-.5-.5h-1a.5.5 0 0 0-.5.5v1.293L8.354 1.146a.5.5 0 0 0-.708 0l-6 6A.5.5 0 0 0 1.5 7.5v7a.5.5 0 0 0 .5.5h4a.5.5 0 0 0 .5-.5Z" />
        </svg>
        <h5 className="title">{Title}</h5>
        <div>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="30"
            height="30"
            fill="currentColor"
            className="bi bi-box-arrow-right mx-3"
            viewBox="0 0 16 16"
            onClick={() => {
              debugger;
              if (sessionStorage.getItem("isLoggedIn") === "Logout")
                sessionStorage.setItem("isLoggedIn", "Login");
              sessionStorage.removeItem("User", "");
              sessionStorage.removeItem("token", "");
              sessionStorage.removeItem("LoginData", "");
              navigate("/Login");
            }}
          >
            <path
              d="M10 12.5a.5.5 0 0 1-.5.5h-8a.5.5 0 0 1-.5-.5v-9a.5.5 0 0 1 .5-.5h8a.5.5 0 0 1 .5.5v2a.5.5 0 0 0 1 0v-2A1.5 1.5 0 0 0 9.5 2h-8A1.5 1.5 0 0 0 0 3.5v9A1.5 1.5 0 0 0 1.5 14h8a1.5 1.5 0 0 0 1.5-1.5v-2a.5.5 0 0 0-1 0v2z"
            />
            <path
              d="M15.854 8.354a.5.5 0 0 0 0-.708l-3-3a.5.5 0 0 0-.708.708L14.293 7.5H5.5a.5.5 0 0 0 0 1h8.793l-2.147 2.146a.5.5 0 0 0 .708.708l3-3z"
            />
          </svg>
        </div>

      </Navbar>
    </>
  );
};
