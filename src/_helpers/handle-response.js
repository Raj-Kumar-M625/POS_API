import { authenticationService } from '../_services/authentication.service';

export function handleResponse(response) {
    return response.text().then(text => {
        let data;
        try {
            data = text && JSON.parse(text);
        } catch (err) {
            if (typeof text === "string") {
                data = text;
            }
        }
        if (!response.ok) {
            if ([401, 403].indexOf(response.status) !== -1) {
                // auto logout if 401 Unauthorized or 403 Forbidden response returned from api
                authenticationService.logout();
                // window.location.reload();
            }

            const error = (data && data.message) || (data && data.response) || response.statusText;
            return Promise.reject(error);
        }

        return data;
    });
}