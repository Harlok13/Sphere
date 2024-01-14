import {createSlice, current, PayloadAction} from "@reduxjs/toolkit";
import {IInitRoomDataResponse} from "shared/contracts/init-room-data-dto";
import {IPlayerDto, Player} from "shared/contracts/player-dto";
import {IAddedPlayerResponse} from "shared/contracts/responses/added-player-response";
import {produce} from "immer";
import {IRemovedPlayerResponse} from "shared/contracts/removed-player-response";
import {IChangedRoomBankResponse} from "shared/contracts/responses/changed-room-bank-response";
import {IChangedPlayerIsLeader} from "shared/contracts/responses/changed-player-is-leader-response";
import {IChangedRoomRoomNameResponse} from "shared/contracts/responses/changed-room-room-name-response";
import {IChangedPlayerReadinessResponse} from "shared/contracts/responses/changed-player-readiness-response";
import {IChangedPlayerMoneyResponse} from "shared/contracts/responses/changed-player-money-response";
import {IChangedPlayerInGameResponse} from "shared/contracts/responses/changed-player-in-game-response";
import {IAddedCardResponse} from "shared/contracts/responses/added-card-response";
import {IChangedPlayerOnlineResponse} from "shared/contracts/responses/changed-player-online-response";
import {
    GameHistoryMessage,
    IAddedGameHistoryMessageResponse
} from "shared/contracts/responses/added-game-history-message-response";



type RoomData = IInitRoomDataResponse;

export interface Game21State {
    inRoom: boolean;
    bank: number;
    gameStarted: boolean;
    roomData: RoomData;
    players: Array<Player>;
    gameHistory: Array<GameHistoryMessage>;
}

const initialState: Game21State = {
    inRoom: false,
    bank: 0,
    gameStarted: false,
    roomData: {
        id: "",
        roomSize: 0,
        roomName: "",
        startBid: 0,
        minBid: 0,
        maxBid: 0,
    },
    players: [],
    gameHistory: [],
}

export const game21Slice = createSlice({
    name: "game21",
    initialState,
    reducers: {
        initRoomData: (state, action: PayloadAction<IInitRoomDataResponse>) => {
            state.roomData = action.payload
            console.log(current(state));
        },
        setInRoom: (state, action: PayloadAction<boolean>) => {
            state.inRoom = action.payload;
        },
        setNewPlayer: (state, action: PayloadAction<IAddedPlayerResponse>) => {
            const nextState = produce(state, draft => {
                draft.players.push(action.payload.player);
            })
            // state.players = [...state.players, action.payload]
            state.players = nextState.players;
        },
        updatePlayersList: (state, action: PayloadAction<Array<IPlayerDto>>) => {
            const nextState = produce(state, draft => {
                draft.players = action.payload;
            });
            // state.players = [...state.players.filter(p => p.playerId !== action.payload), action.payload];
            // state.players = [...action.payload];
            state.players = nextState.players;
        },
        updatePlayerInPlayers: (state, action: PayloadAction<IPlayerDto>) => {
            const nextState = produce(state, draft => {
                const index = draft.players.findIndex(p => p.id === action.payload.id);
                draft.players[index] = action.payload;
            });
            state.players = nextState.players;
        },
        clearPlayersList: (state) => {
            state.players = [];
        },
        removePlayerFromPlayers: (state, action: PayloadAction<IRemovedPlayerResponse>) => {
            state.players = [...state.players.filter(p => p.id !== action.payload.playerId)];
        },
        updateRoomName: (state, action: PayloadAction<string>) => {
            state.roomData.roomName = action.payload;
        },
        updateBankValue: (state, action: PayloadAction<IChangedRoomBankResponse>) => {
            state.bank = action.payload.bank;
        },
        resetGame21State: (state) => {
            // state.bank = 0;
            // state.inRoom = false;
            // state.gameStarted = false;
            // state.players = [];
            // state.gameHistory = [];
            // state.roomData = {
            //     id: "",
            //     roomSize: 0,
            //     roomName: "",
            //     startBid: 0,
            //     minBid: 0,
            //     maxBid: 0
            // }
            return initialState;
        },
        setGameStarted: (state, action: PayloadAction<boolean>) => {
            const newState = produce(state, draft => {
                draft.gameStarted = action.payload;
            });
            state.gameStarted = newState.gameStarted;
        },
        // setPlayerInGame: (state, action: PayloadAction<PlayerInGame>) => {
        //     state.players.fil
        // }
        updateIsLeaderInPlayers: (state, action: PayloadAction<IChangedPlayerIsLeader>) => {
            const index = state.players.findIndex(p => p.id === action.payload.playerId);
            state.players[index].isLeader = action.payload.isLeader;
        },
        updateRoomNameInRoomData: (state, action: PayloadAction<IChangedRoomRoomNameResponse>) => {
            state.roomData.roomName = action.payload.roomName;
        },
        updateReadinessInPlayers: (state, action: PayloadAction<IChangedPlayerReadinessResponse>) => {
            const index = state.players.findIndex(p => p.id === action.payload.playerId);
            state.players[index].readiness = action.payload.readiness;
        },
        updateMoneyInPlayers: (state, action: PayloadAction<IChangedPlayerMoneyResponse>) => {
            const index = state.players.findIndex(p => p.id === action.payload.playerId);
            state.players[index].money = action.payload.money;
        },
        updateInGameInPlayers: (state, action: PayloadAction<IChangedPlayerInGameResponse>) => {
            const index = state.players.findIndex(p => p.id === action.payload.playerId);
            state.players[index].inGame = action.payload.inGame;
        },
        setCardInPlayersCards: (state, action: PayloadAction<IAddedCardResponse>) => {
            const index = state.players.findIndex(p => p.id === action.payload.playerId);
            state.players[index].cards.push(action.payload.cardDto);
        },
        updateOnlineInPlayers: (state, action: PayloadAction<IChangedPlayerOnlineResponse>) => {
            const index = state.players.findIndex(p => p.id === action.payload.playerId);
            state.players[index].online = action.payload.online;
        },
        setNewGameHistoryMessage: (state, action: PayloadAction<IAddedGameHistoryMessageResponse>) => {
            state.gameHistory.push(action.payload);
        },
        initGameHistory: (state, action: PayloadAction<Array<GameHistoryMessage>>) => {
            state.gameHistory = action.payload;
        }
    }
});

export const {
    setInRoom,
    initRoomData,
    setNewPlayer,
    updatePlayersList,
    updatePlayerInPlayers,
    clearPlayersList,
    removePlayerFromPlayers,
    updateRoomName,
    updateBankValue,
    resetGame21State,
    setGameStarted,
    updateIsLeaderInPlayers,
    updateRoomNameInRoomData,
    updateReadinessInPlayers,
    updateMoneyInPlayers,
    updateInGameInPlayers,
    setCardInPlayersCards,
    updateOnlineInPlayers,
    setNewGameHistoryMessage,
    initGameHistory,
} = game21Slice.actions;

export default game21Slice.reducer;
