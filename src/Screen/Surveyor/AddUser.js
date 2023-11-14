
import React from 'react'
import Button from 'react-bootstrap/Button';
import Dropdown from 'react-bootstrap/Dropdown';
import { DropdownButton } from 'react-bootstrap';
import Container from 'react-bootstrap/Container';
import Form from 'react-bootstrap/Form';
import Navbar from 'react-bootstrap/Navbar'
import { useNavigate } from "react-router-dom";
import { config } from "./../../constants";
import axios from 'axios';
import './../../CSS/Views/adduser.css'

export const AddUser =()=> {
  const navigate = useNavigate();
  const [firstName, setFirstName] = React.useState('')
  const [lastName, setLastName] = React.useState('')
  const [age, setAge] = React.useState('')
  const [dob, setDob] = React.useState(Date)
  const [gender, setGender] = React.useState('')
  const [phone, setPhone] = React.useState('')
  const [userName, setUserName] = React.useState('')

  const User = JSON.parse(sessionStorage.getItem("LoginData"));
  if (User === "" || User == null) {
    sessionStorage.setItem("isLoggedIn", "Login");
    window.location.href = "/Login";
  }else{
    if(User.userRoles !== "Admin"){
     window.location.href = "/Common/ErrorPage"
    }
  }
  
  const handleFirstnameChange = (value) => {
    setFirstName(value);
  }

  const handleLastnameChange = (value) => {
    setLastName(value);
  }

  const handleAge = (value) => {
    setAge(value)
  }

  const handleDob = (value) => {
    setDob(value)
  }

  const handlePhone = (value) => {
    setPhone(value)
  }
  const [show, setShow] = React.useState(false);

  const handleClose = () => setShow(false);
  const handleShow = () => setShow(true);
  const handleSubmit = async () => {
    debugger;
    const data = {
      userName: firstName + lastName,
      firstName: firstName,
      lastName: lastName,
      age: age,
      phoneNumber: phone,
      dob: dob
    };
    const url = `${config.localUrl}UserManagement`;
    await axios.post(url+"?userDto",data).then((result) => {
      if( data != null){
        setShow(true);
      }
        }).catch((error) =>{
      alert(error)
    })
  }


  return (

    <div>
      
      <Navbar className="NavbarUser">
        <Container fluid className='mb-3'>
          <h5>Add User</h5>
          <Form className="d-flex mt-1"
          >        <DropdownButton
            className="dropdown"
            align="end"
            title="Menu"
            id="dropdown-menu-align-end"
          >
              <Dropdown.Item eventKey="1">Feed Back</Dropdown.Item>
              <Dropdown.Item eventKey="2">UserManagement</Dropdown.Item>
              <Dropdown.Item eventKey="3">User Assignment</Dropdown.Item>

            </DropdownButton>
          </Form>
        </Container>
      </Navbar>

      <div className='text'
      >
        <div className="row g-3">
          <div className="col-md-6 ">
            <label>First Name</label>
            <input type="text" className="form-control" onChange={(e) => handleFirstnameChange(e.target.value)} />
          </div>
        </div>

        <div className="row g-3">
          <div className="col-md-6">
            <label>Last Name</label>
            <input type="text" className="form-control" onChange={(e) => handleLastnameChange(e.target.value)} />
          </div>
        </div>

        <div className="row g-3">
          <div className="col-md-6">
            <label>Age</label>
            <input type="text" className="form-control" onChange={(e) => handleAge(e.target.value)} />
          </div>
        </div>

        <div className="row g-3">
          <div className="col-md-6">
            <label>Date of Birth</label>
            <input type="date" className="form-control" onChange={(e) => handleDob(e.target.value)} />
          </div>
        </div>

        <div className="row g-3">
          <div className="col-md-6">
            <label>Gender</label>
            <select className="form-select">
              <option value=""></option>
              <option value="">M</option>
              <option value="">F</option>
            </select>
          </div>
        </div>

        <div className="row g-3">
          <div className="col-md-6">
            <label>Phone/Mobile Number</label>
            <input type="text" className="form-control" onChange={(e) => handlePhone(e.target.value)} />
          </div>
        </div>
        <Button className="button" onClick={() => {
          navigate("/user"); handleSubmit()
        }} >Save</Button>

      </div>



    </div>




  )

    }

export default AddUser