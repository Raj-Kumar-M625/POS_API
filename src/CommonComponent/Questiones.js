import React, { Children, useRef, useState } from "react";
import { Multiselect } from "multiselect-react-dropdown";
import i18n from "../i18n/i18n";
import { useTranslation } from "react-i18next";
import "../../src/CSS/Views/pseodu.css";
import { Form, FormControl } from "react-bootstrap";
import { useId } from "react";
import Spinner2 from "../Assets/Image/Spinner2.gif";

function Questiones({
  api,
  register,
  setData,
  data,
  reset,
  childQuestion,
  locale
}) {
  const { t } = useTranslation();
  const refs = useRef([]);
 
  
  return (
    <div
      className="row gy-3 mx-auto mt-3 justify-content-center font-weight-bold"
      
    >
      {api.length <= 0 ? (
        <div
          className="d-flex justify-content-center"
          // style={{ height: "50rem" }}
        >
          <img
            src={Spinner2}
            width="200px"
            height="200px"
            alt="spinner"
          />
        </div>
      ) : (
        <></>
      )}
      {api.map((qn) => {
        if (qn.QuestionTypeName === "Descriptive") {
          return (
            <>
              <div className="col-md-6 row mt-2 mb-1">
                <label
                  // style={{ overflowWrap: "break-word" }}
                  className={qn.IsMandatory ? "star overFlowWrap" : "overFlowWrap"}
                >
                  {qn.QText}
                </label>
              </div>
              <div className="col-md-6 row col-md-6  mt-2 mb-1">
                <input
                  defaultValue={`${qn.AnswerText ? qn.AnswerText : ""}`}
                  className="form-control"
                  {...register(`Qn${qn.Id}`)}
                  type="text"
                  disabled={
                    childQuestion.filter((item) => item.CId === qn.Id).length >
                    0
                      ? childQuestion.filter((item) => item.CId === qn.Id)[0]
                          .disable
                      : false
                  }
                  required={qn.IsMandatory}
                />
              </div>
            </>
          );
        } else if (qn.QuestionTypeName === "Single Select") {
          return (
            <>
              <div className="col-md-6 row col-md-6  mt-2 mb-1">
                <label
                  // style={{ overflowWrap: "break-word" }}
                  className={qn.IsMandatory ? "star overFlowWrap" : "overFlowWrap"}
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
                    
                      if (
                        childQuestion.filter((item) => item.PId === qn.Id)
                          .length > 0
                      ) {
                        if (
                          e.target.value.trim() === "Yes" ||
                          e.target.value.trim() === "ಹೌದು"
                        ) {
                          childQuestion
                            .filter(
                              (item) =>
                                item.PId === qn.Id && item.Type === "Yes"
                            )
                            .forEach((item) => {
                              item.disable = false;
                              reset({ [`Qn${item.CId}`]: "" });
                              //unregister(`Qn${item.CId}`);
                           

                              return item;
                            });

                          childQuestion
                            .filter(
                              (item) => item.PId === qn.Id && item.Type === "No"
                            )
                            .forEach((item) => {
                              item.disable = true;
                              reset({ [`Qn${item.CId}`]: "" });
                            
                              return item;
                            });

                          childQuestion
                            .filter(
                              (item) =>
                                item.PId === qn.Id && item.Type === "Others"
                            )
                            .forEach((item) => {
                              item.disable = true;
                              reset({ [`Qn${item.CId}`]: "" });
                              //unregister(`Qn${item.CId}`);

                              return item;
                            });
                          
                          return childQuestion;
                        } else if (
                          e.target.value.trim() === "No" ||
                          e.target.value.trim() === "ಇಲ್ಲ"
                        ) {
                          childQuestion
                          .filter(
                            (item) => item.PId === qn.Id && item.Type === "No"
                            )
                            .forEach(async (item) => {
                             
                              item.disable = false;
                              reset({ [`Qn${item.CId}`]: "" });
                              await refs.current[`Qn${item.CId}`].resetSelectedValues()
                              return item;
                            });
                          childQuestion
                            .filter(
                              (item) =>
                                item.PId === qn.Id && item.Type === "Yes"
                            )
                            .forEach((item) => {
                              item.disable = true;
                              reset({ [`Qn${item.CId}`]: "" });
                              //unregister(`Qn${item.CId}`);
                              return item;
                            });

                          childQuestion
                            .filter(
                              (item) =>
                                item.PId === qn.Id && item.Type === "Others"
                            )
                            .forEach((item) => {
                              item.disable = true;
                              reset({ [`Qn${item.CId}`]: "" });
                              //unregister(`Qn${item.CId}`);
                              return item;
                            });
                          
                          return childQuestion;
                        } else if (
                          e.target.value.trim() === "Others" ||
                          e.target.value.trim() === "Other" ||
                          e.target.value.trim() === "ಇತರೆ" ||
                          e.target.value.trim() === "ಇತರರು"
                        ) {
                          childQuestion
                            .filter(
                              (item) =>
                                item.PId === qn.Id && item.Type === "Others"
                            )
                            .forEach((item) => {
                              item.disable = false;

                              reset({ [`Qn${item.CId}`]: "" });
                              //unregister(`Qn${item.CId}`);
                              return item;
                            });

                          
                          return childQuestion;
                        } else if (
                          e.target.value.trim() === "Agriculture" ||
                          e.target.value.trim() === "ಕೃಷಿ"
                        ) {
                          childQuestion
                            .filter(
                              (item) =>
                                item.PId === qn.Id &&
                                item.Type === "Agriculture"
                            )
                            .forEach((item) => {
                              item.disable = false;

                              reset({ [`Qn${item.CId}`]: "" });
                              //unregister(`Qn${item.CId}`);
                              return item;
                            });
                          childQuestion
                            .filter(
                              (item) =>
                                item.PId === qn.Id && item.Type === "Commercial"
                            )
                            .forEach((item) => {
                              item.disable = true;

                              reset({ [`Qn${item.CId}`]: "" });
                              //unregister(`Qn${item.CId}`);
                              return item;
                            });
                          
                          return childQuestion;
                        } else if (
                          e.target.value.trim() === "Commercial" ||
                          e.target.value.trim() === "ವಾಣಿಜ್ಯ"
                        ) {
                          childQuestion
                            .filter(
                              (item) =>
                                item.PId === qn.Id && item.Type === "Commercial"
                            )
                            .forEach((item) => {
                              item.disable = false;

                              reset({ [`Qn${item.CId}`]: "" });
                              //unregister(`Qn${item.CId}`);
                              return item;
                            });
                          childQuestion
                            .filter(
                              (item) =>
                                item.PId === qn.Id &&
                                item.Type === "Agriculture"
                            )
                            .forEach((item) => {
                              item.disable = true;

                              reset({ [`Qn${item.CId}`]: "" });
                              //unregister(`Qn${item.CId}`);
                              return item;
                            });
                          
                          return childQuestion;
                        } else {
                          childQuestion
                            .filter(
                              (item) => item.PId === qn.Id && item.Type === "No"
                            )
                            .forEach((item) => {
                              item.disable = true;
                              reset({ [`Qn${item.CId}`]: "" });
                             
                              return item;
                            });
                          childQuestion
                            .filter(
                              (item) =>
                                item.PId === qn.Id && item.Type === "Yes"
                            )
                            .forEach((item) => {
                              item.disable = true;
                              reset({ [`Qn${item.CId}`]: "" });
                              //unregister(`Qn${item.CId}`);
                            
                              return item;
                            });

                          childQuestion
                            .filter(
                              (item) =>
                                item.PId === qn.Id && item.Type === "Others"
                            )
                            .forEach((item) => {
                              item.disable = true;
                              reset({ [`Qn${item.CId}`]: "" });
                              //unregister(`Qn${item.CId}`);

                              return item;
                            });
                        }
                      }
                    },
                  })}
                  disabled={
                    childQuestion.filter((item) => item.CId === qn.Id).length >
                    0
                      ? childQuestion.filter((item) => item.CId === qn.Id)[0]
                          .disable
                      : false
                  }
                >
                  <option
                   value={
                      qn.AnswerText
                    }
                  >
                    {qn.AnswerText === ""
                      ? `${locale==='en'?'Select...':'ಆಯ್ಕೆ ಮಾಡಿ...'}`
                      : qn.AnswerText}
                  </option>
                  {qn.QuestionPaperAnswers.filter(t => t.AText!==qn.AnswerText ).map((e) => {
                    return (
                      
                      <option key={e.Id} value={e.AText}>
                        {e.AText}
                      </option>
                    );
                  })}
                </select>
              </div>
            </>
          );
        } else if (qn.QuestionTypeName === "Multi Select") {
          var value = [];
          if (qn.AnswerText != null) {
            value = qn.AnswerText.split(" | ");
          }
          
          return (
            <>
              <div key={qn.QText} className="col-md-6 row  mt-2 mb-1">
                <label className={qn.IsMandatory ? "star" : ""}>
                  {qn.QText}
                </label>
              </div>
              <div
               
                className="col-md-6 row mt-2 mb-1 p-0"
              >
                <Multiselect
                  ref = {(ele)=>refs.current[`Qn${qn.Id}`]=ele}
                  placeholder={locale==='kn'?'ಆಯ್ಕೆ ಮಾಡಿ...':'Select...'}
                  isObject={false}
                  selectedValues={value.filter((e)=>e !== '')}
                  options={ qn.QuestionPaperAnswers.map((e) => e.AText)}
                  showCheckbox={true}
                  onRemove={(selectedList,selectedItem) => {

                     setData({
                        ...data,
                        [`Qn${qn.Id}`]: selectedList.join(" | "),
                     });
                    
                    if (
                      selectedItem.trim() === "Others" ||
                      selectedItem.trim() === "Other" ||
                      selectedItem.trim() === "ಇತರೆ" ||
                      selectedItem.trim() === "ಇತರರು"
                    ) {
                      childQuestion
                        .filter(
                          (item) => item.PId === qn.Id && item.Type === "Others"
                        )
                        .forEach((item) => {
                          item.disable = true;

                          reset({ [`Qn${item.CId}`]: "" });
                          //unregister(`Qn${item.CId}`);
                          return item;
                        });

                      return childQuestion;
                    }

                    if (
                      selectedItem.trim() === "Physical disability" ||
                      selectedItem.trim() === "ದೈಹಿಕ ಅಂಗವೈಕಲ್ಯ"
                    ) {
                      childQuestion
                        .filter(
                          (item) =>
                            item.PId === qn.Id && item.Type === "Physical"
                        )
                        .forEach((item) => {
                          item.disable = true;

                          reset({ [`Qn${item.CId}`]: "" });
                          //unregister(`Qn${item.CId}`);
                          return item;
                        });
                    }

                    if (
                      selectedItem.trim() === "Mental health diagnosis" ||
                      selectedItem.trim() === "ಮಾನಸಿಕ ಆರೋಗ್ಯ ರೋಗ ನಿರ್ಣಯ"
                    ) {
                      childQuestion
                        .filter(
                          (item) => item.PId === qn.Id && item.Type === "Mental"
                        )
                        .forEach((item) => {
                          item.disable = true;

                          reset({ [`Qn${item.CId}`]: "" });
                          //unregister(`Qn${item.CId}`);
                          return item;
                        });
                    }
                  }}
                  onSelect={(event, selectedItem) => {
                    setData({
                      ...data,
                      [`Qn${qn.Id}`]:event.join(" | "),
                    });
                    if (
                      selectedItem.trim() === "Others" ||
                      selectedItem.trim() === "Other" ||
                      selectedItem.trim() === "ಇತರೆ"
                    ) {
                      childQuestion
                        .filter(
                          (item) => item.PId === qn.Id && item.Type === "Others"
                        )
                        .forEach((item) => {
                          item.disable = false;

                          reset({ [`Qn${item.CId}`]: "" });
                          //unregister(`Qn${item.CId}`);
                          return item;
                        });

                      
                      return childQuestion;
                    } else if (
                      selectedItem.trim() === "Physical disability" ||
                      selectedItem.trim() === "ದೈಹಿಕ ಅಂಗವೈಕಲ್ಯ"
                    ) {
                      childQuestion
                        .filter(
                          (item) =>
                            item.PId === qn.Id && item.Type === "Physical"
                        )
                        .forEach((item) => {
                          item.disable = false;

                          reset({ [`Qn${item.CId}`]: "" });
                          //unregister(`Qn${item.CId}`);
                          return item;
                        });

                      
                      return childQuestion;
                    } else if (
                      selectedItem.trim() === "Mental health diagnosis" ||
                      selectedItem.trim() === "ಮಾನಸಿಕ ಆರೋಗ್ಯ ರೋಗ ನಿರ್ಣಯ"
                    ) {
                      childQuestion
                        .filter(
                          (item) => item.PId === qn.Id && item.Type === "Mental"
                        )
                        .forEach((item) => {
                          item.disable = false;

                          reset({ [`Qn${item.CId}`]: "" });
                          //unregister(`Qn${item.CId}`);
                          return item;
                        });

                      return childQuestion;
                    }
                  }}
                  
                  disable={
                    childQuestion.filter((item) => item.CId === qn.Id).length >
                      0
                      ? childQuestion.filter((item) => item.CId === qn.Id)[0]
                        .disable
                      : false
                    
                  }
                />
              </div>
            </>
          );
        }
      })}
    </div>
  );
}

export default Questiones;
