import React from "react";
import { Form } from "react-bootstrap";
import { Multiselect } from "multiselect-react-dropdown";
import "../../CSS/Views/pseodu.css"
import Spinner2 from "../../Assets/Image/Spinner2.gif";

function SurveyInfo_Questiones({ api,register,data,setData }) {

  return (
    <>
      {api.length <= 0 ? 
      <div
          className="d-flex justify-content-center"
          
        >
          <img
            src={Spinner2}
            width="200px"
            height="200px"
            alt="spinner2"
          />
        </div> :
        <></>}
      <div className="row gy-3 mx-auto mt-3  justify-content-center fw-bold" >
        {api.map((qn) => {
            if (qn.QuestionTypeName === "Descriptive") {
              return (
                <>
                  <div className="col-md-6 row mt-2 mb-1" key={qn.Id}>
                    <Form.Label  key={qn.Id} className="star">{qn.QText}</Form.Label>
                  </div>
                  <div className="col-md-6 row mt-2 mb-1" >
                    <input className="form-control" 
                      {...register(`Qn${qn.Id}`)}
                      defaultValue={qn.AnswerText}
                      type="text"
                      disabled={true}
                    />
              
                  </div>
                </>
              );
            } else if (qn.QuestionTypeName === "Single Select") {
           
              return (
                <>
                  <div className="col-md-6 row mt-2 mb-1" key={qn.Id}>
                    <label className="star">{qn.QText}</label>
                  </div>
                  <div className="col-md-6 row mt-2 mb-1 cursor-na" >
                    <select  {...register(`Qn${qn.Id}`)} className="form-select" disabled={true}>
                      <option defaultValue={qn.AnswerText == null ? null : qn.AnswerText}>{qn.AnswerText == null ? null : qn.AnswerText}</option>
       

                      {qn.QuestionPaperAnswers.map((e) => {
                        return (
                          <>
                            {qn.AnswerText == e ? null : (<>
                              <option key={e.Id} defaultValue={e.AText}>
                                {e.AText}
                              </option>
                            </>)}
                          </>
               
                        );
                      })}
                    </select>
                  </div>
            
                </>
              );
            } else if (qn.QuestionTypeName === "Multi Select") {
              var value = []
              if (qn.AnswerText != null) {
                value = qn.AnswerText.split(" | ")
              }
              return (<>
                <div className="col-md-6 row mt-2 mb-1">
                  <Form.Label  className="star">{qn.QText}</Form.Label>
                </div>
                <div className="col-md-6 row mt-2 mb-1 p-0" key={qn.Id}>
                  <Multiselect
                    disable={true}
                    
                    isObject={false}
                    selectedValues={value.filter(e => e!=='')}
                    options={qn.QuestionPaperAnswers.map((e) => {
                      return e.AText
                    })}
                    placeholder=""
                    showCheckbox={true}
                    onRemove={(selectedList) => {
                      setData({
                        ...data, [`Qn${qn.Id}`]: selectedList.join(" | ")
                      })
                    }}
                    displayValue="key"
                    onSelect={(event) => {
        
                      setData({
                        ...data, [`Qn${qn.Id}`]: event.join(" | ")
                      })
                    }}
       
                  />
                </div>
              </>);
            }
        })}
      </div>
    </>
  );
}

export default SurveyInfo_Questiones;
