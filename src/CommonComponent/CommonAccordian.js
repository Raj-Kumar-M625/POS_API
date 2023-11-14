import { Accordion } from "react-bootstrap";
//import "../CSS/CommonComponentsCSS/Accordian.css";
import "../SCSS/CommonComponentsCSS/Accordian.css";

export const CommonAccordian = (props) => {
  const { data, children, clsBody } = props;
  return (
    <>
      <div className="Accordion-Common">
        <Accordion defaultActiveKey="0" flush={true}>
          <Accordion.Item eventKey={data.eventKey}>
            <Accordion.Header>
              <p >
                {data.title}
                <br />
                <span className="accordian-header-subtitle">
                  {data.subtitle}
                </span>
              </p>
            </Accordion.Header>
            <Accordion.Body className="accBody">{children}</Accordion.Body>
          </Accordion.Item>
        </Accordion>
      </div>
    </>
  )
}
