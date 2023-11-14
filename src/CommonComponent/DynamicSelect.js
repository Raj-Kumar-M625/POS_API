import React from "react";
import { useState } from "react";
import i18n from "../i18n/i18n";

function DynamicSelect({ qn,register }) {
    const [model, setmodel] = useState({})
    console.log(model)
    return <>{qn.Id === 1992 ? <>
         <div className="col-md-6 row col-md-6  mt-2 mb-1">
                <label
                  style={{ overflowWrap: "break-word" }}
                  className={qn.IsMandatory ? "star" : ""}
                >
                  {qn.QText}
                </label>
              </div>
              <div className=" col-md-6 row col-md-6  mt-2 mb-1">
                <select
                  required={qn.IsMandatory}
                  className="form-select"
                {...register(`Qn${qn.Id}`, {
                    onChange: (e) => {
                        let Qid = `Qn${qn.Id}`
                        setmodel({
                            ...model,
                            "Qn1992":e.target.value
                        })
                      
                      }
                  })}
                >
                  <option value="">{i18n.t("Select")}</option>
                  {qn.QuestionPaperAnswers.map((e) => {
                    return (
                      <option key={e.Id} value={e.AText}>
                        {e.AText}
                      </option>
                    );
                  })}
                </select>
        </div>
        
        {model.Qn1992 === "Yes" ? <>
       <div className="col-md-6 row col-md-6  mt-2 mb-1">
                <label
                  style={{ overflowWrap: "break-word" }}
                  className={qn.IsMandatory ? "star" : ""}
                >
                If family accepted you do you live with parents
                </label>
              </div>
              <div className=" col-md-6 row col-md-6  mt-2 mb-1">
                <select
                  required={qn.IsMandatory}
                  className="form-select"
                {...register(`Qn${qn.Id}`)}
                >
                  <option value="">{i18n.t("Select")}</option>
                  {qn.QuestionPaperAnswers.map((e) => {
                    return (
                      <option key={e.Id} value={e.AText}>
                        {e.AText}
                      </option>
                    );
                  })}
                </select>
        </div>
        </> : <>
        <h1>No</h1>
        </>}

    </> : <>
    
            

    </>}
    
    </>;
}

export default DynamicSelect;
