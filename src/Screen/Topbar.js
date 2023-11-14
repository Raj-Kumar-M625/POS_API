import React, { useEffect, useState } from "react";
import { Navbar } from "react-bootstrap";
import Emblem from "../Assets/Image/emblem2.png";
import karmani from "../Assets/Image/karmani.jpg";
import { useLocation, useNavigate } from "react-router-dom";
import LangSelector from "../CommonComponent/langSelector";
import i18n from "../i18n/i18n";
import { useTranslation } from "react-i18next";
import "../CSS/Views/Topbar.css";
import i18next from "i18next";

export function Topbar() {
  const { t } = useTranslation();
  const location = useLocation();

  const [matches, setMatches] = useState(window.matchMedia("(min-width: 1025px) and (max-width: 1580px)").matches);
  useEffect(() => {
    debugger
    window.matchMedia("(min-width: 1025px) and (max-width: 1580px)").addEventListener('change', e => setMatches(e.matches));
  }, []);

  return (
    <div>
      <Navbar
        className="navbar fixed-top nc-topbar"
        sticky="top"
      >



        {!matches ?
          <>
            <div>
              <img className="" src={Emblem} alt="emblem"></img><br />
              <h3 className="text-light text-sm">{i18n.t('Header.3')}</h3>
            </div>

            <div className="section1">
              <h6 className="text-light text-sm">{i18n.t('Header.1')}</h6>
              <h6>{i18n.t("Header.2")}</h6>
              <h6>{i18n.t("Header.4")}</h6>
            </div>

            <div className="section2">
              {location.pathname === "/Login" ? <LangSelector /> : ""}
              <img
                src={karmani}
                className="logo"
                alt="logo"
              ></img>
            </div>

          </>


          :

          <>
            <div className="row align-items-center mx-2 ">
              <div className="image">
                <a>
                  <img className="emblem" src={Emblem} alt="emblem"></img>
                </a>
              </div>
              <div className="col-4 m-1">
                <div className="header3">
                  <h3 className="text-light text-sm">{i18n.t('Header.3')}</h3>
                </div>

              </div>
            </div>

            <div className="row justify-content-center mx-2 headerContainer">

              {i18next.language === "kn" ? <div className="header1_Kn"> <h4 className="text-light text-sm headerText_Kn">{i18n.t('Header.1')}</h4></div> :
                <div className="header1"> <h4 className="text-light text-sm headerText">{i18n.t('Header.1')}</h4> </div>}
              <div className="row">
                <div className="col md-10 ">
                  {i18next.language === "kn" ? <div className="header4_Kn">
                    <h6>{i18n.t("Header.4")}</h6>
                  </div> : <div className="header4">
                    <h6>{i18n.t("Header.4")}</h6>
                  </div>}
                </div>
                <div className="col md-10">
                  {i18next.language === "kn" ?
                    <div className="header2_kn">
                      <h6>{i18n.t("Header.2")}</h6>
                    </div>
                    : <div className="header2">
                      <h6>{i18n.t("Header.2")}</h6>
                    </div>}
                </div>
              </div>
            </div>

            <div className="row align-items-center justify-content-end">
              <div className="col-md-2 mx-4 langSelector">
                {location.pathname === "/Login" ? <LangSelector /> : ""}
              </div>
              <div className="col-md-4 mx-2 logoContainer">
                <img
                  src={karmani}
                  className="logo"
                  alt="logo"
                ></img>
              </div>
            </div></>

        }
      </Navbar>
    </div>
  );
}

export default Topbar;