import React from "react";
import { Button } from "react-bootstrap";
import i18n from "../../i18n/i18n";

function SurveyInfo_BottomNavigation({ backNavigator }) {
  
  return (
    <>
      <div className="col-md-12 container  d-flex justify-content-end mt-3 ">
          <Button className="btn btn-primary mx-3" onClick={backNavigator}>
           {i18n.t("Back")}
          </Button>
      </div>
    </>
  );
}

export default SurveyInfo_BottomNavigation;
