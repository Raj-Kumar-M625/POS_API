import { BehaviorSubject } from "rxjs";
import { config } from "../constants";

import { authHeader } from "../_helpers/auth_header";
import { http } from "./http.service";

const currentUserSubject = new BehaviorSubject(
  JSON.parse(sessionStorage.getItem("currentUser"))
);
const currentTokenSubject = new BehaviorSubject(
  JSON.parse(sessionStorage.getItem("userToken"))
);

export const authenticationService = {
  generateOtp,
  validateAndLogin,
  logout,
  registor,
  sessionService,
  userOTPForgotPassword,
  updateUserData,
  updateJwtToken,
  resendOTP,
  currentUser: currentUserSubject.asObservable(),
  userToken: currentTokenSubject.asObservable(),
  get currentUserValue() {
    return currentUserSubject.value;
  },
  get currentUserToken() {
    return currentTokenSubject.value;
  },
};

function updateUserData(User) {
  currentUserSubject.next(User);
}

function updateJwtToken(jwt) {
  currentTokenSubject.next(jwt);
}

async function generateOtp(params) {
  const url = `${config.localUrl}/Auth/phone?phone=${params}`;
  const loginParams = {
    PhoneNumber: params.PhoneNumber,
  };
  return http.getData(url).then((data) => {
    sessionStorage.setItem("userData", JSON.stringify(data));
    return data;
  });

  // return testUserRegData().then((data) => {
  //   localStorage.setItem("userData", JSON.stringify(data));
  //   return data;
  // });
}

//Function used to validate otp and login
async function validateAndLogin(params) {
    
  const url = `${config.localUrl}/Application/VerifyApplication`;
  const requestData = {
    ApplicationNumber: params.ApplicationNumber,
    PhoneNumber: params.PhoneNumber,
  };
  return http.postData(url, requestData).then((token) => {
    sessionStorage.setItem("userData", JSON.stringify(token));
    updateJwtToken(token);
    return token;
  });

  // return testJwtPostData().then((jwt) => {
  //   localStorage.setItem("userToken", JSON.stringify(jwt));
  //   updateJwtToken(jwt); //set the user session post login
  //   // sessionService();
  //   return jwt;
  // });
}

async function registor(params) {
  const url = `${config.apiUrl}/create-account`;
  const data = {
    username: params.name,
    emailId: params.email,
    userType: params.type,
  };
  return http.postData(url, data).then((user) => {
    sessionStorage.setItem("currentUser", JSON.stringify(user));
    updateUserData(user);
    return user;
  });
}



async function setPassword(password) {
  const auth = authHeader();
  const data = {
    emailId: this.currentUserValue.emailId,
    password: password,
  };
  const url = `${config.apiUrl}/reset-password`;
  return http.postData(url, data, auth);
}

async function activateAccount(otp) {
  const data = {
    emailId: this.currentUserValue.emailId,
    otp: otp,
  };
  const url = `${config.apiUrl}/activate-account`;
  return http.postData(url, data).then((jwt) => {
    sessionStorage.setItem("userToken", JSON.stringify(jwt));
    updateJwtToken(jwt);
    return jwt;
  });
}

async function sessionService() {
  const url = `${config.apiUrl}/user-session`;
  return http.getData(url).then((user) => {
    sessionStorage.setItem("currentUser", JSON.stringify(user));
    updateUserData(user);
    return user;
  });

  // return testSessionPostData().then((user) => {
  //   localStorage.setItem("currentUser", JSON.stringify(user));
  //   updateUserData(user);
  //   return user;
  // });
}

async function forgotPassword(email) {
  const req = {
    emailId: email,
  };
  const url = `${config.apiUrl}/forgot-password`;
  return http.postData(url, req).then((user) => {
    sessionStorage.setItem("currentUser", JSON.stringify(user));
    updateUserData(user);
    return user;
  });
}

async function userOTPForgotPassword(otp) {
  const req = {
    emailId: this.currentUserValue.emailId,
    otp: otp,
  };
  const url = `${config.apiUrl}/user-otp`;
  return http.postData(url, req).then((jwt) => {
    sessionStorage.setItem("userToken", JSON.stringify(jwt));
    updateJwtToken(jwt);
    return jwt;
  });
}

async function resendOTP(email) {
  const req = {
    emailId: email,
  };
  const url = `${config.apiUrl}/resend-otp`;
  return http.postData(url, req).then((user) => {
    sessionStorage.setItem("currentUser", JSON.stringify(user));
    updateUserData(user);
    return user;
  });
}

async function logout() {
  // remove user from local storage to log user out

  console.log("Logging Out");
  sessionStorage.removeItem("currentUser");
  sessionStorage.removeItem("userToken");
  updateJwtToken(null);
  updateUserData(null);
}
