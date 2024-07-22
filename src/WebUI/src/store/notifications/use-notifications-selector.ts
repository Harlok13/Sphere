import {useTypedSelector} from "hooks/use-typed-selector";

export const useNotificationsSelector = () => {

    return useTypedSelector(state => state.notifications.notifications);
}