import React from "react";
import WelcomePage from "components/pages/WelcomePage/WelcomePage";
import LoginPage from "components/pages/LoginPage/LoginPage";
import RegisterPage from "components/pages/RegisterPage/RegisterPage";
import {Route} from "react-router";
import {ProtectedRoutes} from "./ProtectedRoutes";
import {RoomPage} from "components/pages/RoomPage/RoomPage";
import {LobbyPage} from "components/pages/LobbyPage/LobbyPage";
import {Routes} from "react-router-dom";
import {SettingsPage} from "components/pages/SettingsPage/SettingsPage";
import {LeaderboardsPage} from "components/pages/LeaderboardsPage/LeaderboardsPage";
import ProfilePage from "components/pages/ProfilePage/ProfilePage";
import {v4} from "uuid";
import {SignalrProvider} from "providers/SignalrProvider";

export const Router = () => {
    return (
        <Routes>
            <Route key="/" path="/" element={<WelcomePage/>}/>
            <Route key="login" path="login" element={<LoginPage/>}/>
            <Route key="register" path="register" element={<RegisterPage/>}/>
            <Route element={<ProtectedRoutes/>}>
                {/*<Route element={<SignalrProvider/>}>*/}
                    <Route key={v4()} path="room/:id" element={<RoomPage/>}/>
                    <Route key={v4()} path="lobby" element={<LobbyPage/>}/>
                    <Route key="settings" path="settings" element={<SettingsPage/>}/>
                    <Route key="leaderboards" path="leaderboards" element={<LeaderboardsPage/>}/>
                    <Route key="profile" path="profile" element={<ProfilePage/>}/>
                {/*</Route>*/}
            </Route>
            <Route key="*" path="*" element={<div>Page Not Found</div>}/>
        </Routes>
    )
}