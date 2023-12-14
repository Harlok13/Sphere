// @ts-ignore
import style from "./style.module.css";
import Card from "../../../../../shared/components/Card/Card";
import {v4} from "uuid";

const Main = ({props}) => {
    return (
        <div className={style.main}>
            <div className={style.gameArea}>
                <div className={style.opponentSide}>
                    {props.game
                        ? (
                            <div className={style.opponentScore}>
                                <span>Bot score: </span>{props.opponentScore}<span></span>
                            </div>
                        )
                        : null
                    }

                    <div className={style.opponentCardsContainer}>
                        {props.opponentCards.length
                            ? props.opponentCards.map(cardData => (<Card key={v4()} cardData={cardData}/>))
                            : null
                        }
                    </div>
                </div>

                <div className={style.userSide}>
                    {props.game
                        ? (
                            <div className={style.userScore}>
                                <span>User score: </span>{props.userScore}<span></span>
                            </div>
                        )
                        : null
                    }

                    <div className={style.userCardsContainer}>
                        {props.userCards.length
                            ? props.userCards.map(cardData => (<Card key={`${cardData.x}${cardData.y}`} cardData={cardData}/>))
                            : null
                        }
                    </div>
                </div>
            </div>
        </div>
    )
}
export default Main;

// {userScoreValue: 14, opponentScoreValue: 22, returnedCards: '[{"x":2460,"y":939,"width":223,"height":313,"value…9,"width":223,"height":313,"value":10,"owner":2}]', cardsPlayed: '[{"x":0,"y":313,"width":225,"height":313,"value":1…9,"width":223,"height":313,"value":10,"owner":2}]', gameState: 'Win'}