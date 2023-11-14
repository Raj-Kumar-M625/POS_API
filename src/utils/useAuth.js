import { useContext } from "react";
import AuthContext from '../context/auth-provider.js';

const useAuth = () => {
    return useContext(AuthContext);
}

export default useAuth;