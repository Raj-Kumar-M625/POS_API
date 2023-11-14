import { authHeader } from "../_helpers/auth_header";
import { handleResponse } from "../_helpers/handle-response";
import { config } from "../constants";
import axios from "axios";
import { useState, useEffect } from "react";
//import { http } from "./http.service";
//import {privateAxios} from "../context/Axios";
const Axios = require("../context/Axios");
const privateAxios = Axios.privateAxios;

export const http = {
  postData,
  getData,
  putData,
  deleteData,
  fileData,
  GetData,
};


 




function GetData() {

  const [data, setdata] = useState({
    Name: "",
    SurveyId: "",
  });
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Content-type": "application/json",
      "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
    },
   
  });
  
  const urlLink = `${config.localUrl}BasicSurveyDetail/getBasicSurveyDetails`;
  
  useEffect(() => {
    privateAxios
      .get(urlLink)
      .then(function (response) {
        setdata({
          Name: response.data[response.data.length - 1].name,
          SurveyId: response.data[response.data.length - 1].surveyId,
        });
      });
  }, []);
  return data;
}



async function postData(url, data, headers) {
 
  const auth = authHeader();
  const requestOptions = {
    method: "POST",
    headers: {
      "Target-URL": `${config.targetUrl}`,
      //Authorization:`Bearer ${auth.Authorization}`,
      ...headers,
    },
    body: JSON.stringify(data),
  };
  return fetch(url, requestOptions)
    .then((response) => response.json())
    .then((response) => {

      return response
    });
}


async function getData(url) {
  const auth = authHeader();
  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      // "Target-URL": `${config.targetUrl}`,
      //Authorization:`Bearer ${auth.Authorization.token}`,
    },
  };
  return fetch(url, requestOptions)
    .then((response) => {
      return response.json();
    })
    .then((res) => {
      return res;
    }).catch(e=>console.log(e))
}


async function putData(url, data, headers) {

  const auth = authHeader();
  const requestOptions = {
    method: "PUT",
    headers: {
      "Target-URL": `${config.targetUrl}`,
      Authorization: `Bearer ${auth.Authorization}`,
      ...headers,
    },
    body: JSON.stringify(data),
  };
 fetch(url, requestOptions)
    .then((response)=>{
      return response.json()
    }).then((result)=>{
       return result
    })
}


async function deleteData(url) {
  const auth = authHeader();

  const requestOptions = {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      "Target-URL": `${config.targetUrl}`,
      //Authorization: auth.Authorization,
    },
    // body: JSON.stringify(data),
  };
  return fetch(url, requestOptions)
    .then(handleResponse)
    .then((response) => {
      return Promise.resolve(response);
    });
}

async function fileData(url, data, headers) {
  const auth = authHeader();
  const requestOptions = {
    method: "POST",
    headers: {
      Authorization: auth.Authorization,
      ...headers,
    },
    body: data,
  };
  return fetch(url, requestOptions)
    .then(handleResponse)
    .then((response) => {
      return Promise.resolve(response);
    });
}
