import {config} from "../constants";
//import { privateAxios } from "../context/Axios";
import { http } from "./http.service";
// const Axios = require("../context/Axios");
// const privateAxios = Axios.privateAxios;
import axios from "axios";
export const OfficeService ={
    getAllOfficeList,
    getTalukList,
    getHobliList,
    getVillageList
}

async function getAllOfficeList(StateCode){
    
    const privateAxios = axios.create({
        baseURL: config.localUrl,
        headers: {
          "Content-type": "application/json",
          "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
        },
       
      });
    const url = `${config.localUrl}Office/GetAllOfficeData?stateCOde=${StateCode}`;
    return privateAxios.get(url).then((res) => {
        debugger;
        return res;
    }).catch((error) => {
        debugger;
        console.log(error);
    })

}

async function getTalukList(DistrictCode){
      
    const privateAxios = axios.create({
        baseURL: config.localUrl,
        headers: {
          "Content-type": "application/json",
          "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
        },
       
      });

    const url = `${config.localUrl}Office/GetTalukList?districtCode=${DistrictCode}`;
   
    return privateAxios.get(url).then((res) => {
        return res;
    }).catch((error) => {
        console.log(error);
    })

  }

  async function getHobliList(talukCode){
      
    const privateAxios = axios.create({
        baseURL: config.localUrl,
        headers: {
          "Content-type": "application/json",
          "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
        },
       
      });

    const url = `${config.localUrl}Office/GetHobliList?talukCode=${talukCode}`;
    return privateAxios.get(url).then((res) => {
        return res;
    }).catch((error) => {
        console.log(error);
    })

  }

  async function getVillageList(hobliCode){
      
    const privateAxios = axios.create({
        baseURL: config.localUrl,
        headers: {
          "Content-type": "application/json",
          "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
        },
       
      });
      
    const url = `${config.localUrl}Office/GetVillageList?hobliCode=${hobliCode}`;
    return privateAxios.get(url).then((res) => {
        return res;
    }).catch((error) => {
        console.log(error);
    })
  }