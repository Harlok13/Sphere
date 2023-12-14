import {createSlice} from "@reduxjs/toolkit";
import UserService from "../../services/user/user.service";


type Room = {
    guid: number;
    img: string;
    roomSize: number;
    startBid: number;
    minBid: number;
    maxBid: number;
    status: string;
    playersInRoom: number;
}

const userName = UserService.getUser().userName;
const userAvatar = UserService.getUser().userAvatar;
console.log(userAvatar)
export const lobbySlice = createSlice({
    name: "lobby",
    initialState: {
        roomSettings: {
            // guid: "",  // the guid automatically getting by response
            roomName: `${userName}'s Room`,
            imgUrl: userAvatar,
            roomSize: 2,
            startBid: 100,
            minBid: 10,
            maxBid: 20,
            status: "waiting",
            playersInRoom: 0,
            // players: []
        },
        rooms: [
            {
                guid: "",
                imgUrl: userAvatar,
                roomName: "Harlok",
                roomSize: 2,
                startBid: 100,
                minBid: 5,
                maxBid: 20,
                status: "waiting",
                playersInRoom: 1,
                players: []
            },
            {
                guid: "",
                imgUrl: "/img/avatars/ava.jpg",
                roomName: "Lara",
                roomSize: 3,
                startBid: 200,
                minBid: 10,
                maxBid: 30,
                status: "playing",
                playersInRoom: 3,
                players: []
            }
        ],

    },
    reducers: {
        addNewRoom: (state, action) => {
            state.rooms = [{...action.payload}, ...state.rooms];  // TODO: [action.payload, ...state.rooms]
        },
        removeRoom: (state, action) => {
            // @ts-ignore
            state.rooms = [...state.rooms.filter(l => l.guid !== action.payload)];
        },
        updateRoom: (state, action) => {
            state.rooms = [...state.rooms.filter(r => r.guid !== action.payload.guid), action.payload]
        },

        // setRoomGuid: (state, action) => {
        //     state.roomSettings.guid = action.payload;
        // },
        setRoomName: (state, action) => {
            state.roomSettings.roomName = action.payload;
        },
        setRoomSize: (state, action) => {
            state.roomSettings.roomSize = action.payload;
        },
        setStartBid: (state, action) => {
            state.roomSettings.startBid = action.payload;
        },
        setMinBid: (state, action) => {
            state.roomSettings.minBid = action.payload;
            if (state.roomSettings.minBid >= state.roomSettings.maxBid){
                state.roomSettings.maxBid = action.payload;  // TODO: fix
            }
        },
        setMaxBid: (state, action) => {
            state.roomSettings.maxBid = action.payload;
            if (state.roomSettings.minBid >= state.roomSettings.maxBid){
                state.roomSettings.minBid = action.payload;  // TODO: fix
            }
        },
        setImgUrl: (state, action) => {
            state.roomSettings.imgUrl = action.payload;
        },
        // setPlayersInRoom: (state, action) => {
        //     state.
        // }
        // setPlayers: (state, action) => {
        //     debugger
        //     console.log("invoke")
        //     console.log(action.payload, "payload")
        //     state.roomSettings.players = [...state.roomSettings.players, {...action.payload}];
        // },

        setLowBid: (state) => {
            state.roomSettings.startBid = 50;
            state.roomSettings.minBid = 5;
            state.roomSettings.maxBid = 10;
        },
        setMediumBid: (state) => {
            state.roomSettings.startBid = 200;
            state.roomSettings.minBid = 30;
            state.roomSettings.maxBid = 80;
        },
        setHighBid: (state) => {
            state.roomSettings.startBid = 500;
            state.roomSettings.minBid = 100;
            state.roomSettings.maxBid = 300;
        },

        initRooms: (state, action) => {
            state.rooms = [...action.payload].reverse();
        },
    }
});

export const {
    addNewRoom,
    removeRoom,
    setRoomName,
    setRoomSize,
    setMaxBid,
    setStartBid,
    setMinBid,
    setLowBid,
    setMediumBid,
    setHighBid,
    setImgUrl,
    // setPlayers,
    initRooms,
    updateRoom,
    // setRoomGuid,
} = lobbySlice.actions;

export default lobbySlice.reducer;