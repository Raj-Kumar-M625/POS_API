import React from "react";
import { Navbar } from "react-bootstrap";
import { Typography, Toolbar } from "@mui/material";
import "../CSS/Views/Footer.css";
import i18n from "../i18n/i18n";
import { useTranslation } from "react-i18next";

export const Footer = () => {
  const {t} = useTranslation();
  return (
    <div className="">
      <Navbar className="navbar fixed-bottom nc-footer" variant="dark"  >
        <Typography className="content"
        // component="div"
        // sx={{ flexGrow: 1, color: "white", textAlign: "center" }}
        >
          <h4 className="ho" >
            {" "}
           {i18n.t('Footer.1')}
          </h4>
        </Typography>
      </Navbar>
    </div >

  );
};
