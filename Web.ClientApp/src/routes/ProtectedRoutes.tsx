import {Navigate, Outlet, useLocation} from "react-router-dom";
import {useGlobalInitData} from "hooks/init-data/use-global-init-data";
import UserService from "services/user/user.service";
import {SignalrProvider} from "providers/SignalrProvider";

export const ProtectedRoutes = () => {
    const location = useLocation();
    useGlobalInitData()
    return (
        <SignalrProvider>
            {UserService.getUser().isLoggedIn
                ? <Outlet/>
                : <Navigate to="/login" replace state={{from: location}}/>}
        </SignalrProvider>
    )
}
