import React, { useRef } from "react";
import { Form } from "react-bootstrap";

function Select1({ options, setselect, qn, register }) {

  const select = useRef();
  console.log(select.current)
  return (
    <>
    <div className="col-md-6">
      <label >{qn.QText}</label>
      </div>
      <div className="col-md-6" >
        <select
          className="form-select"
          ref = {select}
          
        onChange={(e) => {
            console.log(e.target.value)
            setselect(e.target.value)
        }
        }
        // {...register(`Qn${qn.QuestionId}`)} 
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
    <div className="col-md-6" >
    <input className="form-control"
                type="text"
                defaultValue=""
                value=""
          disabled={true}
              />
              </div>
    </>
  );
}

export default Select1;
