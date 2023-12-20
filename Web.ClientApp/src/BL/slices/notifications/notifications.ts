import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {INotificationResponse, Notification} from "../../../contracts/notification-response";
import {produce} from "immer";


interface NotificationsState {
    notifications: Array<Notification>;
}

const initialState: NotificationsState = {
    notifications: []
}

export const notificationsSlice = createSlice({
    name: "notifications",
    initialState,
    reducers: {
        setNewNotification: (state, action: PayloadAction<INotificationResponse>) => {
            const nextState = produce(state, draft => {
                draft.notifications.push(action.payload);
            });
            state.notifications = nextState.notifications;
        },
        removeNotification: (state, action: PayloadAction<string>) => {
            const nextState = produce(state, draft => {
               draft.notifications = draft.notifications.filter(n => n.id !== action.payload);
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
