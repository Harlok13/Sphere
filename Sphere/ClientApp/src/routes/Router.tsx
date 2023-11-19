import {Route, Routes} from "react-router-dom";
import Welcome from "../components/pages/Welcome/Welcome";
import LoginPage from "../components/pages/Login/LoginPage";
import RegisterPage from "../components/pages/Register/RegisterPage";
import HomePage from "../components/pages/Home/HomePage";
import GamePage from "../components/pages/Game/GamePage";
import ProfilePage from "../components/pages/Profile/ProfilePage";
import ProtectedRoutes from "./ProtectedRoutes";
import {Room} from "../components/pages/Room/Room";
export const Router = () => {
    return (
        <Routes>
            <Route key="/" path="/" element={<Welcome/>}/>
            <Route key="login" path="login" element={<LoginPage/>}/>
            <Route key="register" path="register" element={<RegisterPage/>}/>
            <Route key="room" path="room" element={<Room/>}/>
            <Route key="home" path="home" element={<HomePage/>}/>
            <Route element={<ProtectedRoutes/>}>
                <Route key="game" path="game" element={<GamePage/>}/>
                <Route key="profile" path="profile" element={<ProfilePage/>}/>
            </Route>
            <Route key="*" path="*" element={<div>Page Not Found</div>}/>
        </Routes>
    )
}