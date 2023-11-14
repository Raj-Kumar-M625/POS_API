
import React from 'react'
import Button from 'react-bootstrap/Button';
import Dropdown from 'react-bootstrap/Dropdown';
import Container from 'react-bootstrap/Container';
import Form from 'react-bootstrap/Form';
import Navbar from 'react-bootstrap/Navbar'
import { useNavigate } from "react-router-dom";


function AddUser() {
  const navigate = useNavigate();
  return (

      <div>
    <Navbar style={{background:"#00ace6"}} className="mb-2">
    <Container fluid>
      <h5 style={{marginLeft:'50px'}}>Add User</h5>
        <Form className="d-flex" 
>       <Dropdown>
      <Dropdown.Toggle  style={{marginRight:'20px'}} variant="primary" id="dropdown-basic">
        Menu
      </Dropdown.Toggle>

      <Dropdown.Menu>
        <Dropdown.Item href="#/action-1">Feed Back</Dropdown.Item>
        <Dropdown.Item href="#/action-2">User Management</Dropdown.Item>
        <Dropdown.Item href="#/action-3">User Assignment</Dropdown.Item>
      </Dropdown.Menu>
    </Dropdown>
        </Form>
    </Container>
  </Navbar>

    <div className='text'style={{
       marginLeft:'500px',
       marginBottom:'70px',
       marginTop:'50px'
    }}>
           <div className='add' style={{
            marginRight:'100px'
           }}>
           <label>First Name</label>
           </div>
           
          <div className="row g-3">
            <div className="col-md-6 ">
            <input type="text"  className="form-control" />
            </div>
            </div>

            <div className="row g-3">
            <div className="col-md-6">
                <label>Last Name</label>
                     <input type="text"  className="form-control" />
             </div>
            </div>   

            <div className="row g-3">
            <div className="col-md-6">
                <label>Age</label>
                     <input type="text"  className="form-control" />
             </div>
            </div>         

            <div className="row g-3">
            <div className="col-md-6">
                <label>Date of Birth</label>
                     <input type="text"  className="form-control" />
             </div>
            </div> 

            <div className="row g-3">
            <div className="col-md-6">
                <label>Gender</label>
                <select className="form-select">
                     <option value=""></option>
                      <option value=""></option>
                      <option value=""></option>
                     </select>
             </div>
            </div> 

            <div className="row g-3">
            <div className="col-md-6">
                <label>Phone/Mobile Number</label>
                     <input type="text"  className="form-control" />
             </div>
            </div> 
            <Button  onClick={()=> {
           navigate("/user")
          }}style={{float:"right" , marginRight:'150px',  width:'100px',height:'50px'}
      }>Save</Button>

            </div>


           
  </div>
     



  )

}

export default AddUser