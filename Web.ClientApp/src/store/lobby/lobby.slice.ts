import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import UserService from "services/user/user.service";
import {ICreatedRoomResponse, IRoomInLobbyDto, Room} from "shared/contracts/room-in-lobby-dto";
import {IRemovedRoomResponse} from "shared/contracts/responses/removed-room-response";
import {IChangedRoomAvatarUrlResponse} from "shared/contracts/responses/changed-room-avatar-response";
import {IChangedRoomStatusResponse} from "shared/contracts/responses/changed-room-status-response";
import {IChangedRoomPlayersInRoomResponse} from "shared/contracts/responses/changed-room-players-in-room-response";
import {IChangedRoomRoomNameResponse} from "shared/contracts/responses/changed-room-room-name-response";


const userName = UserService.getUser().userName;
const userAvatar = UserService.getUser().userAvatar;

export type NewRoomConfig = {
    roomName: string;
    avatarUrl: string;
    roomSize: number;
    startBid: number;
    minBid: number;
    maxBid: number;
    status: number | string;  // TODO: fix type, must be only string
    playersInRoom: number;

    lowerBound: number,
    upperBound: number
}

export interface LobbyState {
    newRoomConfig: NewRoomConfig;
    rooms: Array<Room>;
}

const initialState: LobbyState = {
    newRoomConfig: {
        roomName: `${userName}'s Room`,
        avatarUrl: userAvatar,
        roomSize: 2,
        startBid: 100,
        minBid: 10,
        maxBid: 20,
        status: "waiting",
        playersInRoom: 0,

        lowerBound: 240,
        upperBound: 540,
    },
    rooms: [],
}

// const computeStartMoney = (startBid: number, minBid: number, maxBid: number) => {
//     const lowerBound = Math.round(startBid * 1.5 + minBid * 3 + maxBid * 3);
//     const upperBound = Math.round(startBid * 3 + minBid * 8 + maxBid * 8);
//
//     return {lowerBound: lowerBound, upperBound: upperBound};
// }

const computeStartMoney = (state: LobbyState) => {
    const lowerBound = Math.round(state.newRoomConfig.startBid * 1.5 + state.newRoomConfig.minBid * 3 + state.newRoomConfig.maxBid * 3);
    const upperBound = Math.round(state.newRoomConfig.startBid * 3 + state.newRoomConfig.minBid * 8 + state.newRoomConfig.maxBid * 8);

    return {lowerBound: lowerBound, upperBound: upperBound};
}

// TODO: add resetNewRoomConfig | use in room useEffect
export const lobbySlice = createSlice({
    name: "lobby",
    initialState,
    reducers: {
        addNewRoom: (state, action: PayloadAction<ICreatedRoomResponse>) => {
            state.rooms = [action.payload.roomInLobbyDto, ...state.rooms];
        },
        removeRoom: (state, action: PayloadAction<IRemovedRoomResponse>) => {
            state.rooms = [...state.rooms.filter(r => r.id !== action.payload.roomId)];
        },
        updateRoom: (state, action: PayloadAction<IRoomInLobbyDto>) => {
            state.rooms = [...state.rooms.filter(r => r.id !== action.payload.id), action.payload]
        },

        setRoomName: (state, action: PayloadAction<string>) => {
            state.newRoomConfig.roomName = action.payload;
        },
        setRoomSize: (state, action: PayloadAction<number>) => {
            state.newRoomConfig.roomSize = action.payload;
        },
        setStartBid: (state, action: PayloadAction<number>) => {
            state.newRoomConfig.startBid = action.payload;
            const {lowerBound, upperBound} = computeStartMoney(state);
            state.newRoomConfig.lowerBound = lowerBound;
            state.newRoomConfig.upperBound = upperBound;
        },
        setMinBid: (state, action: PayloadAction<number>) => {
            state.newRoomConfig.minBid = action.payload;
            if (state.newRoomConfig.minBid >= state.newRoomConfig.maxBid){
                state.newRoomConfig.maxBid = action.payload;  // TODO: fix
            }

            const {lowerBound, upperBound} = computeStartMoney(state);
            state.newRoomConfig.lowerBound = lowerBound;
            state.newRoomConfig.upperBound = upperBound;
        },
        setMaxBid: (state, action: PayloadAction<number>) => {
            state.newRoomConfig.maxBid = action.payload;
            if (state.newRoomConfig.minBid >= state.newRoomConfig.maxBid){
                state.newRoomConfig.minBid = action.payload;  // TODO: fix
            }

            const {lowerBound, upperBound} = computeStartMoney(state);
            state.newRoomConfig.lowerBound = lowerBound;
            state.newRoomConfig.upperBound = upperBound;
        },
        setRoomAvatarUrl: (state, action: PayloadAction<IChangedRoomAvatarUrlResponse>) => {
            state.newRoomConfig.avatarUrl = action.payload.avatarUrl;
        },

        setLowBid: (state) => {  // TODO: reloc to constants
            state.newRoomConfig.startBid = 50;
            state.newRoomConfig.minBid = 5;
            state.newRoomConfig.maxBid = 10;

            const {lowerBound, upperBound} = computeStartMoney(state);
            state.newRoomConfig.lowerBound = lowerBound;
            state.newRoomConfig.upperBound = upperBound;
        },
        setMediumBid: (state) => {
            state.newRoomConfig.startBid = 200;
            state.newRoomConfig.minBid = 30;
            state.newRoomConfig.maxBid = 80;

            const {lowerBound, upperBound} = computeStartMoney(state);
            state.newRoomConfig.lowerBound = lowerBound;
            state.newRoomConfig.upperBound = upperBound;
        },
        setHighBid: (state) => {
            state.newRoomConfig.startBid = 500;
            state.newRoomConfig.minBid = 100;
            state.newRoomConfig.maxBid = 300;

            const {lowerBound, upperBound} = computeStartMoney(state);
            state.newRoomConfig.lowerBound = lowerBound;
            state.newRoomConfig.upperBound = upperBound;
        },

        initRooms: (state, action: PayloadAction<Array<IRoomInLobbyDto>>) => {
            state.rooms = [...action.payload].reverse();
        },

        updateRoomStatus: (state, action: PayloadAction<IChangedRoomStatusResponse>) => {
            const index = state.rooms.findIndex(r => r.id === action.payload.roomId)
            if (index > -1){
                state.rooms[index].status = action.payload.status;
            }
        },
        updatePlayersInRoom: (state, action: PayloadAction<IChangedRoomPlayersInRoomResponse>) => {
            const index = state.rooms.findIndex(r => r.id === action.payload.roomId);
            if(index > -1){
                state.rooms[index].playersInRoom = action.payload.playersInRoom;
            }
        },
        updateRoomNameInRooms: (state, action: PayloadAction<IChangedRoomRoomNameResponse>) => {
            const index = state.rooms.findIndex(r => r.id === action.payload.roomId);
            if (index > -1){
                state.rooms[index].roomName = action.payload.roomName;
            }
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
    setRoomAvatarUrl,
    initRooms,
    updateRoom,
    updateRoomStatus,
    updatePlayersInRoom,
    updateRoomNameInRooms,
} = lobbySlice.actions;

export default lobbySlice.reducer;
