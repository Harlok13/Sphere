import { MdGroupRemove } from "react-icons/md";
import {FC} from "react";

interface PlayerActionItemProps {
    playerId: string;
    actionName: string;
}


export const PlayerActionsItem: FC<PlayerActionItemProps> = ({playerId, actionName}) => {
    return (
        <li>
            {/*{playerId}*/}
            {actionName}
            <MdGroupRemove />
        </li>
    )
}
