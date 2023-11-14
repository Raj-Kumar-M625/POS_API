import { authenticationService } from '../_services/authentication.service';

export function authHeader() {
    // return authorization header with jwt token
    let token = "";
    if (authenticationService.currentUserToken ) {
        token = authenticationService.currentUserToken;
    }  else if (sessionStorage.getItem('userToken')) {
        token = JSON.parse(sessionStorage.getItem('userToken'));
    };

    if (token) {
        console.log(`Bearer ${token}`)
        return { Authorization: token };
    } else {
        return {};
    }
}
