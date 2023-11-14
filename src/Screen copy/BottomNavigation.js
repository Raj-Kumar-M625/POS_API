import React from "react";
import { Button } from "react-bootstrap";
import "../CSS/Views/Footer.css";

function BottomNavigation({ onSubmitHandler, backNavigator }) {
  return (
    <div className="col-md-12 d-flex justify-content-end m-4">
      <Button className="btn btn-primary m-1" onClick={backNavigator}>
        Back
      </Button>
      <Button className="btn btn-success m-1" type="submit">
        Submit
      </Button>
    </div>
  );
}

export default BottomNavigation;
