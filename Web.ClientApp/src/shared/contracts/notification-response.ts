export interface INotificationResponse {
    notificationId: string;
    notificationText: string;
    // TODO: datetime
}

export type Notification = INotificationResponse;