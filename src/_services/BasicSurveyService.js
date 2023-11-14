import { config } from "../constants";
import { http } from "./http.service";
import { useState } from "react";
//import { privateAxios } from "../context/Axios.js";
// const Axios = require("../context/Axios");
// const privateAxios = Axios.privateAxios;
import axios from "axios";


export const BasicSurveyService = {
    SaveBasicSurveyDetail,
    getSurveyDetails,
    getSurveyByDistrict
}

async function SaveBasicSurveyDetail(data) {

    const privateAxios = axios.create({
        baseURL: config.localUrl,
        headers: {
          "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
        },
       
      });
       debugger;
        const url = `${config.localUrl}BasicSurveyDetail/AddBasicSurvey`;
        return await privateAxios.post(url, data).then((res) => {
            console.log(res);
            debugger;
            return res;
            
        }).catch((error) => {
          debugger;
            console.log(error);
        })
    

    

}

async function getSurveyDetails() {
    debugger;
    const privateAxios = axios.create({
        baseURL: config.localUrl,
        headers: {
          "Content-type": "application/json",
          "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
        },
       
      });
    const url = `${config.localUrl}BasicSurveyDetail/GetAllSurveyList`;
   
    return await privateAxios.get(url).then((response) => {
        debugger;
        return response;
    }).catch((error) => {
        console.log(error);
    })
}

async function getSurveyByDistrict(District) {

    const privateAxios = axios.create({
        baseURL: config.localUrl,
        headers: {
          "Content-type": "application/json",
          "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
        },
       
      });
    const url = `${config.localUrl}BasicSurveyDetail/GetSurveyByDistrict?district=` + District;
    
    return await privateAxios.get(url).then((response) => {
        return response;
    }).catch((error) => {
        console.log(error);
    })
}

export default BasicSurveyService;