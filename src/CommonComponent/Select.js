import React from "react";
import { Form } from "react-bootstrap";

function Select({ options, setSelect,qn,register }) {
  return (
    <>
    <div className="col-md-6" >
    <label >{qn.QText}</label>
      </div>
      <div className="col-md-6" key={qn.QText}>
      <select
     className="form-select"
     onChange={(e) => {
      console.log(e.target.value)
      setSelect(e.target.value)
  }
}  
      >
        <option defaultValue="selected">--Select--</option>
        {options.map((e) => {
          return (
            <option key={e} value={e}>
              {e}
            </option>
          );
        })}
      </select>
    </div>
    <div className="col-md-6" >
    <label>{qn.AdditionalQuestion}</label>
    </div>
    <div className="col-md-6" key={qn.QText}>
    <input className="form-control"
                type="text"
                defaultValue=""
                {...register(`Qn${qn.QuestionId}`)} 
              
                disabled={false}
              />
              </div>
    </>
  );
}

export default Select;
