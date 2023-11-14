import React from "react";
import { Navbar } from "react-bootstrap";
import { Typography, Toolbar } from "@mui/material";
import "./../CSS/Views/Footer.css";
export const Footer = () => {
  return (
    <div style={{marginTop:"73px"}}>
<Navbar className="justify-content-bottom  nc-footer" variant="dark" style={{position:"fixed",bottom:0,width:"100%",}}>
        <Typography
          component="div"
          sx={{ flexGrow: 1, color: "white", textAlign: "center"}}
        >
          <h5 style={{ marginLeft: "3%" }}>
            {" "}
            Government of Karnataka 2022, Center for e-Governanace
          </h5>
        </Typography>
    </Navbar>
    </div>
    
  );
};
