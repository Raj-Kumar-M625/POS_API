import { React, useState } from "react";
import { Stack, Row, Col } from "react-bootstrap";
import { CommonAccordian } from "../../CommonComponent/CommonAccordian";
import { CustomCard } from "../../CommonComponent/CustomCard.js";
import { AccordionTable } from "../../CommonComponent/AccordionTable";
import AutorenewIcon from "@mui/icons-material/Autorenew";
import Select from "react-select";
import { NavbarTitle } from "../../CommonComponent/NavbarTitle";


export const SurveyList = () =>{
    


    return(<>
   

            <Stack direction="vertical" gap={2}>
                <CommonAccordian data={{ title: "Search by Location", eventKey: "0" }}>
                    <Row>
                        <Col xs={3} lg={2}>
                            <Select aria-label="Floating label select example"
                                isClearable={true}
                                name="distirct"
                                className="select-dropdowns"
                                options={""}
                                placeholder="Select by Distict"
                                isSearchable={true}

                            ></Select>

                        </Col>
                        <Col xs={3} lg={2}>
                            <Select aria-label="Floating label select example"
                                isClearable={true}
                                name="taluk"
                                className="select-dropdowns"
                                options={""}
                                placeholder="Select by Taluk/Town"
                                isSearchable={true}

                            ></Select>

                        </Col>
                        <Col xs={3} lg={2}>
                            <Select aria-label="Floating label select example"
                                isClearable={true}
                                name="hobli"
                                className="select-dropdowns"
                                options={""}
                                placeholder="Select by Hobli/Zone"
                                isSearchable={true}

                            ></Select>

                        </Col>
                        <Col xs={3} lg={2}>
                            <Select aria-label="Floating label select example"
                                isClearable={true}
                                name="village"
                                className="select-dropdowns"
                                options={""}
                                placeholder="Select by Village/Ward"
                                isSearchable={true}

                            ></Select>

                        </Col>
                        <Col xs={3} lg={2}>
                            <Select aria-label="Floating label select example"
                                isClearable={true}
                                name="status"
                                className="select-dropdowns"
                                options={""}
                                placeholder="Select by Status"
                                isSearchable={true}

                            ></Select>

                        </Col>
                        <Col xs={3} lg={2}>
                            <AutorenewIcon color="primary" style={{ marginTop: "6px", marginLeft: "6px", cursor: "pointer" }} />
                        </Col>
                    </Row>
                </CommonAccordian>

            </Stack>

            <Stack direction="vertical" gap={2}>
                <CommonAccordian data={{ title: "Statistics", eventKey: "0" }}>
                    <div className="row g-2">
                        <Row className='justify-content-center' >
                            <Col xs={3} lg={2} >
                            <CustomCard
            cardTitle="Total"
            backgroundColor="#f3ca3e"
            cardValue="2"
          />                                </Col>
                            <Col xs={3} lg={2}>
                            <CustomCard
            cardTitle="Completed"
            backgroundColor="#62a76c"
            cardValue="1"
          />                                </Col>
                            <Col xs={3} lg={2}>
                            <CustomCard
            cardTitle="Pending"
            backgroundColor="#ff5f58"
            cardValue="1"
          />                            </Col>

<Col xs={3} lg={2}>
                            <CustomCard
            cardTitle="With Aadhar"
            backgroundColor="#5098a7"
            cardValue="1"
          />                            </Col>

<Col xs={3} lg={2}>
                            <CustomCard
            cardTitle="Without Aadhar"
            backgroundColor="#808bde"
            cardValue="1"
          />                            </Col>

                        </Row>
                    </div>
                </CommonAccordian>


            </Stack>

      <Stack direction="vertical" gap={2} className="m-3">
        <CommonAccordian data={{ title: "Search by Location", eventKey: "0" }}>
          <Row>
            <Col xs={3} lg={12}>
              <AccordionTable columns="Name" />
            </Col>
          </Row>
        </CommonAccordian>
      </Stack>
    </>
  );
};

export default SurveyList;
