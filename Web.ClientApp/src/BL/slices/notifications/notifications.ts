import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {INotEnoughMoneyNotificationResponse, NotEnoughMoneyNotification} from "../../../contracts/not-enough-money-notification-response";
import {produce} from "immer";


interface NotificationsState {
    notifications: Array<NotEnoughMoneyNotification>;
}

const initialState: NotificationsState = {
    notifications: []
}

export const notificationsSlice = createSlice({
    name: "notifications",
    initialState,
    reducers: {
        setNewNotification: (state, action: PayloadAction<INotEnoughMoneyNotificationResponse>) => {
            const nextState = produce(state, draft => {
                draft.notifications.push(action.payload);
            });
            state.notifications = nextState.notifications;
        },
        removeNotification: (state, action: PayloadAction<string>) => {
            const nextState = produce(state, draft => {
               draft.notifications = draft.notifications.filter(n => n.notificationId !== action.payload);
            });
            state.notifications = nextState.notifications;
        }
    }
});

export const {
    setNewNotification,
    removeNotification,
} = notificationsSlice.actions;

export default notificationsSlice.reducer;