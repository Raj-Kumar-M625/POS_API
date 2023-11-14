import React, { useState, useEffect } from "react";
import { Navbar } from "react-bootstrap";
import Emblem from "../Assets/Image/emblem2.png";
//import { Typography, Button, Toolbar, IconButton } from '@mui/material';
import { Row } from "react-bootstrap";
import { Typography, Button, IconButton } from "@mui/material";
import "./../CSS/Views/Topbar.css";
import { useNavigate } from "react-router-dom";

export const Topbar = ({ loggedIn, setLoggedIn }) => {
  const navigate = useNavigate();
  // const HandleLogIn = (logIn) => {
  //   debugger;
  //   window.localStorage.setItem("isloggedIn", !isloggedIn);
  // };

  const isLoggedIn = localStorage.getItem("isLoggedIn");
  return (
    <div>
      <Navbar
        className="justify-content-center  nc-topbar"
        sticky="top"
        collapseOnSelect
        variant="dark"
      >
        <IconButton
          size="small"
          edge="start"
          color="inherit"
          aria-label="menu"
          sx={{ mr: 2 }}
        >
          <Row>
            <a>
              <img
                className=""
                style={{ width: "75px" }}
                src={Emblem}
                alt="this is emblem"
              ></img>
            </a>
          </Row>
        </IconButton>
        <h2 style={{ flexGrow: 1, color: "white" }}>Karmani</h2>
        <Typography
          component="div"
          style={{
            flexGrow: 1,
            color: "white",
            textAlign: "center",
            lineHeight: "2",
          }}
        >
          <h4
            style={{
              marginLeft: "-742px",
              marginTop: "-35px",
              marginBottom: "-35px",
              color: "white",
            }}
          >
            GOVT. OF KARNATAKA
          </h4>
          <br></br>
          <h4
            style={{
              marginLeft: "-742px",
              marginBottom: "-104px",
              color: "white",
            }}
          >
            Karnataka State Transgender Population Survey
          </h4>
        </Typography>
        {loggedIn ? (
          <Button
            style={{ color: "white" }}
            onClick={() => {
              if (localStorage.getItem("isLoggedIn") === "Logout")
                localStorage.setItem("isLoggedIn", "Login");

              navigate("/Login");
            }}
          >
            <h5>{localStorage.getItem("isLoggedIn")}</h5>
          </Button>
        ) : (
          ""
        )}
      </Navbar>
    </div>
  );
};

export default Topbar;
