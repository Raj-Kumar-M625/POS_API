import "./App.css";
import { BrowserRouter, Route } from "react-router-dom";
import { Container } from "react-bootstrap";
import { Router } from "./Routes/router";


function App() {

  // window.addEventListener('beforeunload', (event) => {
  //   debugger;
  //   localStorage.setItem("isLoggedIn", "Login");
  //   localStorage.removeItem("User", "");
  //   localStorage.removeItem("token", "");
  //   localStorage.removeItem("LoginData", "");
  // })
  return (
    <>
      <BrowserRouter>
        <Container bsPrefix>
          <Router />
        </Container>
      </BrowserRouter>
    </>
  );
}

export default App;
