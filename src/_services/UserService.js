import { HtmlSharp } from "@mui/icons-material";
import axios from "axios";
import { useEffect, useState } from "react";
import {config} from "../constants";
//import { privateAxios } from "../context/Axios";
import { http } from "./http.service";

const Axios = require("../context/Axios");
const privateAxios = Axios.privateAxios;

export const UserService = {
    GetAllUsers,
    UpdateUser,
    AddUser,
}


// async function GetAllUsers(){
//   const url = `${config.localUrl}UserManagement/users`;
//     return http.getData(url).then((res) =>{
//         return res;
//     });
  //   const url = `${config.localUrl}users`;
  //   const [data, setdata] = useState([])
  // useEffect(() => {
  //   axios
  //     .get(url)
  //     .then(function (response) {
  //       setdata(response.data);
  //     });
  // }, []);
  // return data;
//}
async function GetAllUsers(){
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Content-type": "application/json",
      "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
    },
   
  });
  const url = `${config.localUrl}users/GetAllUsers`;
  return privateAxios.get(url).then((res)=>{
    return res;
  })
}

async function UpdateUser(data){
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
    },
   
  });

  const url =  `${config.localUrl}users/UpdateUser`;
  return privateAxios.put(url,data).then((res) =>{
    return res;
  });
  
}

async function AddUser(data){
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
    },
   
  });
  
  const url = `${config.localUrl}users/AddUser`;
  
  return privateAxios.post(url, data).then((res) => {
    debugger;
    return res;
  })
}