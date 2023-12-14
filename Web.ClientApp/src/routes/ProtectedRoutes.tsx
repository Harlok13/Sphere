import {Navigate, Outlet, useLocation} from "react-router-dom";
import UserService from "../services/user/user.service";
import {useGlobalInitData} from "../BL/hooks/init-data/use-global-init-data";

export const ProtectedRoutes = () => {
    const location = useLocation();
    useGlobalInitData()

    return UserService.getUser().isLoggedIn ? <Outlet/> : <Navigate to="/login" replace state={{from: location}}/>
}
