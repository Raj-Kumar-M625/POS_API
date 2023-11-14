import React from 'react'
import axios, { AxiosInstance } from "axios";
import { config } from "../constants";
import { createContext, useContext } from "react";

import { PropsWithChildren } from 'react';

// const AxiosContextData = {
//   privateAxios:AxiosInstance
// }
const AxiosContext = createContext(null);


const AxiosProvider = (props) => {
  
  let privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Content-type": "application/json",
      "Authorization": `Bearer ${sessionStorage.getItem("token")}`
    },
  
  });

  
  return(
    <AxiosContext.Provider value={{privateAxios}}>
      {props.children}
    </AxiosContext.Provider>
  );
}


function useAxios(privateAxios){
  const context = useContext(privateAxios);
  return context;
}


export { AxiosContext,AxiosProvider,useAxios }