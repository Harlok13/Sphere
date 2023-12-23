import {Route, Routes} from "react-router-dom";
import WelcomePage from "pages/WelcomePage/WelcomePage";
import LoginPage from "pages/LoginPage/LoginPage";
import RegisterPage from "pages/RegisterPage/RegisterPage";
import ProfilePage from "pages/ProfilePage/ProfilePage";
import {RoomPage} from "pages/RoomPage/RoomPage";
import {SettingsPage} from "pages/SettingsPage/SettingsPage";
import {LeaderboardsPage} from "pages/LeaderboardsPage/LeaderboardsPage";
import {ProtectedRoutes} from "routes/ProtectedRoutes";
import {v4} from "uuid";
import {LobbyPage} from "pages/LobbyPage/LobbyPage";

export const Router = () => {
    return (
        <Routes>
            <Route key="/" path="/" element={<WelcomePage/>}/>
            <Route key="login" path="login" element={<LoginPage/>}/>
            <Route key="register" path="register" element={<RegisterPage/>}/>
            <Route element={<ProtectedRoutes/>}>
                <Route key={v4()} path="room/:id" element={<RoomPage/>}/>
                <Route key={v4()} path="lobby" element={<LobbyPage/>}/>
                <Route key="settings" path="settings" element={<SettingsPage/>}/>
                <Route key="leaderboards" path="leaderboards" element={<LeaderboardsPage/>}/>
                <Route key="profile" path="profile" element={<ProfilePage/>}/>
            </Route>
            <Route key="*" path="*" element={<div>Page Not Found</div>}/>
        </Routes>
    )
}