import { Card } from "react-bootstrap";
import React from "react";
import "../CSS/CommonComponentsCSS/card.css";

export const CustomCard = (props) => {
  const { cardTitle,cardValue, backgroundColor,cardImg,status,cardNum } = props ;
  
  return (
    <>
      <Card
       className={cardNum === "one" ? "card one" : cardNum === "two" ? "card bg-secondary" : cardNum === "three" ? "card three" : cardNum === "four" ? "card four" : 
       cardNum === "five" ? "card five" :cardNum === "six" ? "card six" :cardNum === "seven" ? "card seven" :cardNum === "eight" ? "card eight" :cardNum === "nine" ? "card nine" :"card bg-info" }
        bg={backgroundColor}
      >

        <Card.Body color={backgroundColor}>
          {status}
          {cardImg}
          <Card.Title className="h6 cardTitle"         
          >
            {cardTitle}
          </Card.Title>
          <Card.Text
           className="cardText"
          >
            {cardValue}
          </Card.Text>
        </Card.Body>
      </Card>
    </>
  );
};
