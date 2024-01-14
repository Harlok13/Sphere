import {HistoryHead} from "components/shared/pages/main-page/Aside/History/HistoryHead/HistoryHead";
import {HistoryBody} from "components/shared/pages/main-page/Aside/History/HistoryBody/HistoryBody";
import {
    UserHistoryMsg
} from "components/shared/pages/main-page/Aside/History/HistoryBody/UserHistoryMsg/UserHistoryMsg";
import {
    Game21HistoryMsg
} from "components/shared/pages/main-page/Aside/History/HistoryBody/UserHistoryMsg/Game21HistoryMsg/Game21HistoryMsg";
import {HistoryShowMore} from "components/shared/pages/main-page/Aside/History/HistoryShowMore/HistoryShowMore";
import {History} from "components/shared/pages/main-page/Aside/History/History";
import React from "react";

export const GlobalHistory = () => {
    return (
        <History>
            <HistoryHead/>
            <HistoryBody>
                {/*<UserHistoryMsg gameResult="Win">*/}
                {/*    <Game21HistoryMsg gameResult={"Win"} userName={"Harlok"} opponentName={"Bot"} score={"21:17"}/>*/}
                {/*</UserHistoryMsg>*/}
            </HistoryBody>
            {/*<HistoryShowMore/>*/}
        </History>
    )
}