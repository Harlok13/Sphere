import {createContext, useState} from "react";

// @ts-ignore
export const UserContext = createContext();

const UserProvider = ({children}) => {
    const [user, setUser] = useState({
        id: 0,
        userName: "default",
        // isLoggedIn: false,
        userStatistic: {
            wins: 0,
            loses: 0,
            draws: 0,
            matches: 0,
            money: 0,
            likes: 0,
            has21: 0,
            level: 0,
            exp: 0
        }
    })
    return (
        <UserContext.Provider value={{user, setUser}}>
            {children}
        </UserContext.Provider>
    )
}

export default UserProvider;