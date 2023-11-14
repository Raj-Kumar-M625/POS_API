import React from "react";
import "../CSS/Views/Footer.css";
import i18n from "../i18n/i18n";
import { useTranslation } from "react-i18next";
import spinner from "../Assets/Image/spinner.gif"

function BottomNavigation({ onSubmitHandler,statusCode }) {
  debugger;
 
  const { t } = useTranslation();
  return (
    <>
      <div className="container col-md-12 d-flex justify-content-end mt-3">


        <button className="btn btn-success mx-3" type="submit" onClick={onSubmitHandler}>
          {statusCode === 0 ? i18n.t('Save') : <>
            <img src={spinner} width="30px" height="24px" alt="spin" />
          </>}
        </button>
        {/* <button className="btn btn-success mx-3" type="submit" onClick={onSubmitHandler}>
          {i18n.t('Save')}
        </button> */}
      </div>
    </>
  );
}

export default BottomNavigation;


