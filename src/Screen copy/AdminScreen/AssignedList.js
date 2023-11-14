import React, { useState, useEffect } from "react";
import { Button } from "react-bootstrap";
import DataTable from "react-data-table-component";
import Dropdown from 'react-bootstrap/Dropdown';
import { Navbar } from "react-bootstrap";
import { Typography, Toolbar, IconButton } from '@mui/material';
import { Row } from 'react-bootstrap';
import { useNavigate } from "react-router-dom";
import { CommonAccordian } from "../../CommonComponent/CommonAccordian";
import Emblem from "../../Assets/Image/emblem2.png";


function AssignedList() {
  const navigate = useNavigate();
  const style = {
    fontWeight: "400",
    fontSize: "20px",
    
    
  };
  const columns = [
    {
      name: "User ID",
      selector: (row) => <p style={style}>{row.UserID}</p>,
      width: "150px",
    },
    {
      name: "User name",
      selector: (row) => <p style={style}>{row.UserName}</p>,
      width: "150px",
    },
   
    {
      name: "Age",
      selector: (row) => <p style={style}>{row.Age}</p>,
      width: "80px",
    },

    {
      name: "Gender",
      selector: (row) => <p style={style}>{row.Gender}</p>,
      width: "140px",
    },
   
    {
      name: "District",
      selector: (row) => <p style={style}>{row.District}</p>,
      width: "180px",
    },
    {
      name: "Contact number",
      selector: (row) => <p style={style}>{row.Contactnumber}</p>,
      width: "180px",
    },
    {
      name: "Taluk",
      selector: (row) => <p style={style}>{row.Taluk}</p>,
      width: "180px",
    },

    {
      name: "Hobli",
      selector: (row) => <p style={style}>{row.Hobli}</p>,
      width: "180px",
    },
  ];

  const items = [
    {
      UserID: "HNS001",
      UserName: "Radhika",
      Age: "24",
      Gender: "F",
      Contactnumber: "9985533720",
      District: "Bengalur Rural",
      Taluk: "Aneka",
      Hobli:"Sarjapur",
    },
    {
      UserID: "HNS024",
      UserName: "Shubham",
      Age: "28",
      Gender: "M",
      Contactnumber: "9985533721",
      District: "Shivamogga",
      Taluk: "Sagara",
      Hobli:"Achapura",  
    },
    {
      UserID: "HNS136",
      UserName: "Rahul",
      Age: "36",
      Gender: "M",
      Contactnumber: "9985533722",
      District: "Kalburgi",
      Taluk: "Sedam",
      Hobli:"Adki",
    },
    {
      UserID: "HNS205",
      UserName: "Madhu P",
      Age: "32",
      Gender: "F",
      Contactnumber: "9985533723",
      District: "Bengalur Urban",
      Taluk: "Bengalur South",
      Hobli:"Tavarekere",
    },
    {
      UserID: "HNS136",
      UserName: "Ajay",
      Age: "36",
      Gender: "M",
      Contactnumber: "9985533762",
      District: "Kalburgi",
      Taluk: "Sedam",
      Hobli:"Adki",
    },
  ];

  return<>
  {/* <div>
         
            <nav class="navbar bg-light">
                <div class="container-fluid">
                    <a class="navbar-brand" href="#">User Assignment</a>
                    <Dropdown>
      <Dropdown.Toggle variant="primary" id="dropdown-basic">
        Menu 
      </Dropdown.Toggle>

      <Dropdown.Menu>
        <Dropdown.Item href="#/action-1">Feedback</Dropdown.Item>
        <Dropdown.Item href="#/action-2">User Managment</Dropdown.Item>
        <Dropdown.Item href="#/action-3">User Assignment</Dropdown.Item>
      </Dropdown.Menu>
    </Dropdown>

                </div>
            </nav>
            
</div> */}




    <div className="Assignedlist"> 
      <CommonAccordian data={{ title: "AssignedList", eventKey: "" }}>
  <div class="col-md-8">

  <div class="search d-flex" style={{
    marginLeft:'1400px'
  }}>
    <button class="btn btn-primary ">Search</button>
      <input type="text" placeholder="search" class="form-control"/>
    </div>
    
  </div>
      <Button style={{
        float:"right",
        marginTop:"15px"
      }}>Add</Button>
        <DataTable columns={columns} data={items} paginationTotalRows="1" />
      </CommonAccordian>
    </div>
  

  </>

}

export default AssignedList;