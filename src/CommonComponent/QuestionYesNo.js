import React from "react";
import i18n from "../i18n/i18n";

function QuestionYesNo({ qn, register, useId, reset, setuseId, unregister,getValues }) {
  return (
    <>
      <div key={qn.QText} className="col-md-6 row  mt-2 mb-1">
        <label className={qn.IsMandatory ? "star" : ""}>{qn.QText}</label>
      </div>
      <div className=" col-md-6 row col-md-6  mt-2 mb-1">
        <select
          required={qn.IsMandatory}
          className="form-select"
          {...register(`Qn${qn.Id}`, {
            onChange: (e) => {
              if (e.target.value === "Yes") {
                reset({ [`Qn${qn.childQuestions[1].Id}`]: "" });
                  // reset({ ...getValues(),[`Qn${qn.childQuestions[1].Id}`]:undefined });
                unregister(`Qn${qn.childQuestions[1].Id}`);
              } else {
                reset({ [`Qn${qn.childQuestions[0].Id}`]: "" });
                //  reset({ ...getValues(),[`Qn${qn.childQuestions[0].Id}`]:undefined });
                unregister(`Qn${qn.childQuestions[0].Id}`);
              }
              setuseId({
                ...useId,
                [`Qn${qn.Id}`]: e.target.value,
              });
            },
          })}
        >
          <option value="">{i18n.t("Select")}</option>
          {qn.QuestionPaperAnswer.map((e) => {
            return (
              <option key={e.Id} value={e.AText}>
                {e.AText}
              </option>
            );
          })}
        </select>
      </div>

      <div
        className="col-md-6 row  mt-2 mb-1"
        style={{
          display:
            useId[`Qn${qn.childQuestions[1].pId}`] === "Yes" ? "block" : "none",
        }}
      >
        <label className={qn.childQuestions[1].IsMandatory ? "star" : ""}>
          {qn.childQuestions[0].QText}
        </label>
      </div>
      <div
        className=" col-md-6 row col-md-6  mt-2 mb-1"
        style={{
          display:
            useId[`Qn${qn.childQuestions[1].pId}`] === "Yes" ? "block" : "none",
        }}
      >
        <input
          required={qn.childQuestions[1].IsMandatory}
          className="form-control"
          {...register(`Qn${qn.childQuestions[0].Id}`)}
        />
      </div>

      <div
        className="col-md-6 row  mt-2 mb-1"
        style={{
          display:
            useId[`Qn${qn.childQuestions[1].pId}`] === "No" ? "block" : "none",
        }}
      >
        <label className={qn.childQuestions[1].IsMandatory ? "star" : ""}>
          {qn.childQuestions[1].QText}
        </label>
      </div>
      <div
        className=" col-md-6 row col-md-6  mt-2 mb-1"
        style={{
          display:
            useId[`Qn${qn.childQuestions[1].pId}`] === "No" ? "block" : "none",
        }}
      >
        <input
          required={qn.childQuestions[1].IsMandatory}
          className="form-control"
          {...register(`Qn${qn.childQuestions[1].Id}`)}
        />
      </div>
    </>
  );
}

export default QuestionYesNo;
