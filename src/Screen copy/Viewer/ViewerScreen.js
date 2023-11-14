import React,{useState} from "react";
import { Stack, Row, Col } from "react-bootstrap";
import { CommonAccordian } from "../../CommonComponent/CommonAccordian";
import Select  from 'react-select';
import AutorenewIcon from '@mui/icons-material/Autorenew';
import "../../CSS/Views/ViewerDashboard.css";
import AssignedList from "../AdminScreen/AssignedList";
//import Navbar from "../../surveyor/Navbar";
import { Footer } from "../Footer";
import { Typography } from "@mui/material";


export const ViewScreen = () => {
    const [data, setData] = useState([]);

    return (
<div className="jaya">
        <Stack direction="vertical" gap={2}>
            <CommonAccordian data={{ title: "Search", eventKey: "0" }}>
                <Row>
                    <Col xs={3} lg={2} style={{marginLeft:'-30px'}}>
                        <Select aria-label="Floating label select example"
                            isClearable={true}
                            name="distirct"
                            className="select-dropdowns"
                            options={""}
                            placeholder="Select by Distict"
                            isSearchable={true}

                        ></Select>

                    </Col>
                    <Col xs={2} lg={2} style={{marginLeft:'-30px'}}>
                        <Select aria-label="Floating label select example"
                            isClearable={true}
                            name="taluk"
                            className="select-dropdowns"
                            options={""}
                            placeholder="Select by Taluk/Town"
                            isSearchable={true}

                        ></Select>

                    </Col>
                    <Col xs={3} lg={2}  style={{marginLeft:'-30px'}}>
                        <Select aria-label="Floating label select example"
                            isClearable={true}
                            name="hobli"
                            className="select-dropdowns"
                            options={""}
                            placeholder="Select by Hobli/Zone"
                            isSearchable={true}

                        ></Select>

                    </Col>
                    <Col xs={3} lg={2}  style={{marginLeft:'-30px'}}>
                        <Select aria-label="Floating label select example"
                            isClearable={true}
                            name="village"
                            className="select-dropdowns"
                            options={""}
                            placeholder="Select by Village/Ward"
                            isSearchable={true}

                        ></Select>

                    </Col>
                    <Col xs={3} lg={2}  style={{marginLeft:'-30px'}}>
                        <Select aria-label="Floating label select example"
                            isClearable={true}
                            name="status"
                            className="select-dropdowns"
                            options={""}
                            placeholder="Select by Gender"
                            isSearchable={true}

                        ></Select>


                    </Col>
                    <Col xs={3} lg={2}  style={{marginLeft:'-30px'}}>
                        <Select aria-label="Floating label select example"
                            isClearable={true}
                            name="status"
                            className="select-dropdowns"
                            options={""}
                            placeholder="Select by User name"
                            isSearchable={true}

                        ></Select>


                    </Col>
                     <Col xs={3} lg={2}  style={{marginLeft:'-100px'}}>
                    <AutorenewIcon color="secondary"style={{marginTop:"50px",marginLeft:"6px",cursor:"pointer"}} />
                </Col>
                </Row>
            </CommonAccordian>

        </Stack>
        {/* <Navbar /> */}
        <AssignedList />
        
        
        
    </div>
    
    )
}
