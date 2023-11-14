import axios from "axios";
import { config } from "../constants";
import * as FileSaver from "file-saver";
import * as XLSX from "xlsx";
//import { privateAxios } from "../context/Axios";
// const Axios = require("../context/Axios");
// const privateAxios = Axios.privateAxios;

export const SurveyService = {
  GetSurveyById,
  updateSurvey,
  getSurveyByCategory,
  getAadharInfo,
  ExportToExcel,
  ExportToExcelAllSurvey
};

async function getAadharInfo(data) {
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
    },
   
  });

  const url = `${config.localUrl}Kutumba/GetBeneficiaryData`;
  
  return await privateAxios.post(url,data).then((res) => {
    debugger;
    return res;
  }).catch((error) => {
    console.log(error);
  })
}

function GetSurveyById(id) {
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Content-type": "application/json",
      "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
    },
   
  });
  const url = `${config.localUrl}Survey/GetSurveyDetailByid?id=${id}`;
  
  privateAxios.get(url).then((res) => {
    return res;
  }).catch((error) => {
    console.log(error);
  })
}

async function updateSurvey(json, surveyId){
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
    },
   
  });
  const url = `${config.localUrl}Survey/UpdateSurveyDetail?SurveyId=${surveyId}`;
  
  await privateAxios.put(url,json).then((res) => {
    return res;
  }).catch((error) => {
    console.log(error);
  })
}

async function getSurveyByCategory(id, CId)  {
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Content-type": "application/json",
      "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
    },
   
  });
  const url = `${config.localUrl}Survey/surveyByCategory?id=${id}&QuestionPaperId=${CId}`;
  
  await privateAxios.get(url).then((res) => {
    return res;
  }).catch((error) => {
    console.log(error);
  })
}

async function ExportToExcel(fileName, id) {
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
    },
   
  });
  const url = `${config.localUrl}Download/DownloadIndividualSurvey?SurveyId=${id}`;
  
  const fileType =
    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8";
  const fileExtension = ".xlsx";

  await privateAxios
    .get(url)
    .then(function (res) {
      const ws = XLSX.utils.json_to_sheet(res.data);
      const wb = { Sheets: { data: ws }, SheetNames: ["data"] };
      const excelBuffer = XLSX.write(wb, { bookType: "xlsx", type: "array" });
      const data = new Blob([excelBuffer], { type: fileType });
      FileSaver.saveAs(data, fileName + fileExtension);
      return res;
    })
    .catch((error) => {
      console.log(error);
    });
}

async function ExportToExcelAllSurvey(fileName,filterModel) {
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
    },
   
  });
  const url = `${config.localUrl}Download/DownloadFilteredSurvey`;
  
  const fileType =
    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8";
  const fileExtension = ".xlsx";

  await privateAxios
    .post(url,filterModel)
    .then(function (res) {
      debugger;
      const ws = XLSX.utils.json_to_sheet(res.data);
      const wb = { Sheets: { data: ws }, SheetNames: ["data"] };
      const excelBuffer = XLSX.write(wb, { bookType: "xlsx", type: "array" });
      const data = new Blob([excelBuffer], { type: fileType });
      FileSaver.saveAs(data, fileName + fileExtension);
      return res;
    })
    .catch((error) => {
      console.log(error);
    });
}

export default SurveyService;
