import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {produce} from "immer";
import {INotificationResponse} from "shared/contracts/notification-response";


interface NotificationsState {
    notifications: Array<INotificationResponse>;
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
                if (draft.notifications.some(n => n.notificationText === action.payload.notificationText)){
                    draft.notifications.filter(n => n.notificationText !== action.payload.notificationText)
                }
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