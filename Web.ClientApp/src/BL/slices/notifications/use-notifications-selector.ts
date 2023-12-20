import {useTypedSelector} from "../../use-typed-selector";

export const useNotificationsSelector = () => {

    return useTypedSelector(state => state.notifications.notifications);
}