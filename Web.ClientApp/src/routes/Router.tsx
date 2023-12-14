import {Route, Routes} from "react-router-dom";
import Welcome from "../components/pages/Welcome/Welcome";
import LoginPage from "../components/pages/Login/LoginPage";
import RegisterPage from "../components/pages/Register/RegisterPage";
import GamePage from "../components/pages/Game/GamePage";
import ProfilePage from "../components/pages/ProfilePage/ProfilePage";
import {RoomPage} from "../components/pages/RoomPage/RoomPage";
import {SettingsPage} from "../components/pages/SettingsPage/SettingsPage";
import {LeaderboardsPage} from "../components/pages/LeaderboardsPage/LeaderboardsPage";
import {ProtectedRoutes} from "./ProtectedRoutes";
import {v4} from "uuid";
import {LobbyPage} from "../components/pages/LobbyPage/LobbyPage";
import {NavigateEnum} from "../constants/navigate.enum";

export const Router = () => {
    return (
        <Routes>
            <Route key="/" path="/" element={<Welcome/>}/>
            <Route key="login" path="login" element={<LoginPage/>}/>
            <Route key="register" path="register" element={<RegisterPage/>}/>
            <Route element={<ProtectedRoutes/>}>
                <Route key={v4()} path="room/:id" element={<RoomPage/>}/>
                <Route key={v4()} path="lobby" element={<LobbyPage/>}/>
                <Route key="settings" path="settings" element={<SettingsPage/>}/>
                <Route key="leaderboards" path="leaderboards" element={<LeaderboardsPage/>}/>
                <Route key="game" path="game" element={<GamePage/>}/>
                <Route key="profile" path="profile" element={<ProfilePage/>}/>
            </Route>
            <Route key="*" path="*" element={<div>Page Not Found</div>}/>
        </Routes>
    )
}