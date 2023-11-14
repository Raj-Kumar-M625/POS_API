import React, { useRef, useState } from 'react'
import { Stack, Row, Col, Button } from "react-bootstrap";
import Accordian from "../../CommonComponent/Accordian";
import Select from "react-select";
import "../../CSS/Views/ViewerDashboard.css";
import { AdminNavbar } from "../../CommonComponent/AdminNavbar";
import AutorenewIcon from '@mui/icons-material/Autorenew';
import 'react-dual-listbox/lib/react-dual-listbox.css';
import DualListBox from 'react-dual-listbox';
import { UserService } from '../../_services/UserService';
import { useEffect } from 'react';
import { OfficeService } from "../../_services/officeService";
import { display } from '@mui/system';
import "../../CSS/Views/UserAssignment2.css"
import {config } from '../../constants';
import { Link } from 'react-router-dom';
//import { privateAxios } from '../../context/Axios';

import axios from 'axios';

export const AddAssignment = () =>{
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Content-type": "application/json",
      "Authorization" : `Bearer ${sessionStorage.getItem("token")}`
    },
   
  });
  const button = useRef();
  const [districtList, setDistrictList] = useState([]);
  const [district,setDistrict] = useState(String);
  const [TalukList,setTalukList] = useState([]);
  const [Taluk,setTaluk] = useState(String);
  const [HobliList,setHobliList] = useState([]);
  const [Hobli,setHobli] = useState(String);
  const [VillageList,setVillageList] = useState([]);
  const [Village,setVillage] = useState(String);
  const [selected, setselected] = useState([]);
  const [UserList, setUserList] = useState([])
  const [users, setusers] = useState([])
  
  
  async function GetAllUsers() { 
    const res = await UserService.GetAllUsers();
    setUserList(res);
  }
    


  const selectInputRefDistrictt3 = useRef();
  const selectInputRefTaluk3 = useRef();
  const selectInputRefHobli3 = useRef();
  const selectInputRefVillage3 = useRef();


  const onClear = () => {
    if(selectInputRefDistrictt3.current){
        selectInputRefDistrictt3.current.clearValue()
        selectInputRefTaluk3.current.clearValue()
        selectInputRefHobli3.current.clearValue()
        selectInputRefVillage3.current.clearValue()
    }
  };

  const options = [];
 
for(var user in UserList){
  options.push({
    value:`${UserList[user].id}`,
    label:`${UserList[user].userName} (${UserList[user].phoneNumber})`
  })
}

  const getAssignedUserByDistrict = async (district) => {
    var districtUsers =  await privateAxios.get(`${config.localUrl}AssignedUsers/district?district=${district}`)
    const DistrictUsers = []
    for(var user in districtUsers){
      DistrictUsers.push(`${districtUsers[user].userId}`)
    }
    setselected(DistrictUsers)  
  }
 

async function getAssignedUserByTaluk(taluk){
  var talukUsers = await privateAxios.get(`${config.localUrl}AssignedUsers/taluk?taluk=${taluk}`)
  const TalukUsers = []
  for(var user in talukUsers){
    TalukUsers.push(`${talukUsers[user].userId}`)
  }
  setselected(TalukUsers)
}


async function getAssignedUserByHobli(hobli){
  var hobliUsers = await privateAxios.get(`${config.localUrl}AssignedUsers/hobli?hobli=${hobli}`)
  const HobliUsers = []
  for(var user in hobliUsers){
    HobliUsers.push(`${hobliUsers[user].userId}`)
  }
  setselected(HobliUsers)
}

useEffect(() => {
  fetchDistrictList(0);
  GetAllUsers()
},[]);

const fetchDistrictList = async() => {
  const res = await OfficeService.getAllOfficeList(29);
  setDistrictList(res);;
}

const getTaluk= async(distCode)=>{
  const res = await OfficeService.getTalukList(distCode);
  setTalukList(res);     
}

const getHobli = async(talukCode) => {
  const res = await OfficeService.getHobliList(talukCode);
  setHobliList(res);
}

const getVillage = async(hobliCode) => {
  const res = await OfficeService.getVillageList(hobliCode);
  setVillageList(res);
}


function assignUsers(){
  privateAxios.post(`${config.localUrl}assign`,users)
}


 function onChange(select,selection,controlKey) {

  if(controlKey==="available"){
    setusers([...users,{
      "userId":selection[0],
      "hobli": Hobli,
      "district": district,
      "taluk": Taluk
    }])

 
}else if(controlKey==="selected"){
    privateAxios.delete(`${config.localUrl}AssignedUsers?id=${selection[0]}`)
}
  setselected(select);
}


    return(
        <>
        <div>
          <AdminNavbar data={{ Title: "Add User" }}></AdminNavbar>
             <nav aria-label="breadcrumb" className="mx-3 ">
  <ol class="breadcrumb">
          <li class="breadcrumb-item"><Link to="/Admin/Dahsboard"  style={{textDecoration:"none"}}>Admin Dashboard</Link></li>
          <li class="breadcrumb-item"><Link to="/Admin/UserManagement"  style={{textDecoration:"none"}}>User Management</Link></li>
          <li class="breadcrumb-item"><Link to="/Admin/UserAssignment"  style={{textDecoration:"none"}}>User Assignment</Link></li>
          <li class="breadcrumb-item active" aria-current="page">Add User</li>
        </ol>
      
</nav>
      <Stack direction="vertical" gap={2} className="m-3">
        <Accordian title="Search">
          <Row className="seven-cols">
            <Col md="2">
            <Select aria-label="Floating label select example"
                isClearable={true}
                ref={selectInputRefDistrictt3}
                name="distirct"
                className="select-dropdowns"
                options={districtList.map((opt) => ({
                  label: opt.districtName,
                  value: opt.districtCode,
                }))}
                onChange={(code, option) => {
                  if(code){
                  setDistrict(code.label);
                  getTaluk(code.value);
                  getAssignedUserByDistrict(code.label)
                  }
                  button.current.style.display = "block"
                }}
                placeholder="Select by Distict"
                isSearchable={true}
             
              ></Select>
            </Col>
            <Col md="2">
            <Select aria-label="Floating label select example"
           ref =  {selectInputRefTaluk3}
                isClearable={true}
                name="taluk"
                className="select-dropdowns"
                options={TalukList.map((opt) => ({
                  label: opt.talukOrTownName,
                  value: opt.talukOrTownCode,
                }))}
                onChange={(code,option)=>{
                  if(code){
                  setTaluk(code.label);
                  getHobli(code.value);
                  getAssignedUserByTaluk(code.label)
                  }
                }}
                placeholder="Select by Taluk/Town"
                isSearchable={true}

              ></Select>
            </Col>
            <Col md="2">
            <Select aria-label="Floating label select example"
                isClearable={true}
                name="hobli"
                ref={selectInputRefHobli3}
                className="select-dropdowns"
                options={HobliList.map((opt) => ({
                  label: opt.hobliOrZoneName,
                  value: opt.hobliOrZoneCode,
                }))}
                onChange={(code,option)=>{
                  if(code){
                  setHobli(code.label);
                  getVillage(code.value)
                  getAssignedUserByHobli(code.label)
                  }
                }}
                placeholder="Select by Hobli/Zone"
                isSearchable={true}

              ></Select>
            </Col>
            <Col md="2">
              <Select
                aria-label="Floating label select example"
                isClearable={true}
                name="village"
                ref = {selectInputRefVillage3}
                className="select-dropdowns"
                options={VillageList.map((opt) => ({
                  label: opt.villageOrWardName,
                  value: opt.villageOrWardCode,
                }))}
                onChange={(code,option)=>{
                  if(code){
                  setVillage(code.label);
                  }
                }}
                placeholder="Select by Village/Ward"
                isSearchable={true}
              ></Select>
            </Col>
            <Col md="2">
              <AutorenewIcon color="primary"  onClick={()=>{
                onClear()
              }}/>
            </Col>
          </Row>
        </Accordian>

        <div class="mt-5">
        <DualListBox
                options={options}
                selected={selected}
                
              onChange={onChange}
              icons={{
                moveLeft: '<',
                moveAllLeft: '<<',
                moveRight: '>',
                moveAllRight: '>>'
              }}
            />
        </div>
        <button 
         onClick={() => assignUsers()}
           ref = {button}
          className='btn btn-success' style={{
          width:"5rem",
          alignSelf:"end",
          display:"none"
        }}>Save</button>
      </Stack>



      </div>
    </>
  )
}