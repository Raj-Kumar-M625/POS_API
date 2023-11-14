import React from 'react'
import Button from 'react-bootstrap/Button';
import Dropdown from 'react-bootstrap/Dropdown';
import Container from 'react-bootstrap/Container';
import Form from 'react-bootstrap/Form';
import Navbar from 'react-bootstrap/Navbar'
import { useNavigate } from "react-router-dom";
import DropdownButton from 'react-bootstrap/DropdownButton'
import {useLocation} from 'react-router-dom'

export const EditScreen=()=> {

  const [phoneNo, setPhoneNo] = React.useState();
  const location = useLocation();

  const User = JSON.parse(sessionStorage.getItem("LoginData"));
  if (User === "" || User == null) {
    sessionStorage.setItem("isLoggedIn", "Login");
    window.location.href = "/Login";
  }else{
    if(User.userRoles !== "Admin"){
     window.location.href = "/Common/ErrorPage"
    }
  }

  const handlePhoneChange = (value) =>{
    setPhoneNo(value);
  }

    const navigate = useNavigate();
  return (
    <div>
   <Navbar style={{background:"#6ba9bf"}} className="mb-2">
    <Container fluid>
      <h5 style={{marginLeft:'50px'}}>Add User</h5>
      <Form className="d-flex mt-1" 
>        <DropdownButton
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
    <div className='text'style={{
       marginLeft:'500px',
       marginBottom:'70px',
       marginTop:'30px'
    }}>
           
           
          <div className="row g-3">
            <div className="col-md-5">
            <label>First Name</label>
            <input type="text" placeholder='Radhika' className="form-control" />
            </div>
            </div>

            <div className="row g-3">
            <div className="col-md-5">
                <label>Last Name</label>
                     <input type="text"  placeholder='Iyer' className="form-control" />
             </div>
            </div>   

            <div className="row g-3">
            <div className="col-md-5">
                <label>Age</label>
                     <input type="text" placeholder='24' className="form-control" />
             </div>
            </div>         

            <div className="row g-3">
            <div className="col-md-5">
            <Form.Group controlId="dob">
                            <Form.Label>Select Date</Form.Label>
                            <Form.Control type="date" name="dob" placeholder="Date of Birth" />
                        </Form.Group>
             </div>
            </div> 

            <div className="row g-3">
            <div className="col-md-5">
                <label>Gender</label>
                <select className="form-select" >
                     <option value=""></option>
                      <option value="">M</option>
                      <option value="">F</option>
                     </select>
             </div>
            </div> 

            <div className="row g-3">
            <div className="col-md-5">
            <label>Mobile Number</label>
                <input type="text" class="form-control" onChange={(e)=> handlePhoneChange(e.target.value)}  maxLength={10}/>
             </div>
            </div> 


            <div className="row g-3">
            <div className="col-md-5">
                <label>CreatedDate</label>
                     <input type="text" placeholder='25/05/2022'  className="form-control" />
             </div>
            </div> 

            <div className="row g-3">
            <div className="col-md-5">
                <label>Status</label>
                <select className="form-select">
                     <option value=""></option>
                      <option value="">Enable</option>
                      <option value="">Disable</option>
                     </select>
             </div>
            </div> 
            <Button  onClick={()=> {
           navigate("/Admin/UserManagement")
          }}style={{float:"right" , marginRight:'150px',marginBottom:"80px",  width:'100px',height:'50px'}
      }>Save</Button>



            </div>
  </div>
     

  )
}

export default EditScreen