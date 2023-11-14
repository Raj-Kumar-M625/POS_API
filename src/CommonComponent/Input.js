import React from "react";
import { Form } from "react-bootstrap";

export const FormInput = ({ QText }) => {
  return (
    <>
      <label>{QText}</label>
      <Form.Control type="text" />
    </>
  );
};
