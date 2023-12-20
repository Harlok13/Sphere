import {PropsWithChildren} from "react";
import style from "./GameArea.module.css";
import {v4} from "uuid";
import {Card} from "../../../../../../components/Card/Card";
import {useTimerSelector} from "../../../../../../../BL/slices/player/use-player-selector";

export const GameArea = ({children}: PropsWithChildren) =>
        <div className={style.gameArea}>
            {children}
        </div>


export const TestArea = ({props}) => {
    props.players = props.players.filter(p => p.id !== props.player.id);
    const timer = useTimerSelector();

    return (
        <div className={style.container}>
            <div className={style.raw}>
                <div className={style.cell}></div>
                <div className={style.cell}>
                    {props.players[0]
                        ? (
                            <>
                                <div>{props.players[0].playerName }</div>
                                <div>Money: {props.players[0].money}</div>
                                <div>
                                    {props.players[0].cards.length
                                        ? props.players[0].cards.map(cardData => (<Card key={v4()} cardData={cardData}/>))
                                        : null
                                    }
                                </div>
                            </>
                        )
                        : null}
                </div>
                <div className={style.cell}></div>
            </div>
            <div className={style.raw}>
                <div className={style.cell}>
                    {props.players[1]
                        ? (
                            <>
                                <div>{props.players[1].playerName}</div>
                                <div>Money: {props.players[1].money}</div>
                                <div>
                                    {props.players[1].cards.length
                                        ? props.players[1].cards.map(cardData => (<Card key={v4()} cardData={cardData}/>))
                                        : null
                                    }
                                </div>
                            </>
                        )
                        : null}
                </div>
                <div className={style.cell}>Bank: {props.bank}</div>
                <div className={style.cell}>
                    {props.players[2]
                        ? (
                            <>
                                <div>{props.players[2].playerName}</div>
                                <div>Money: {props.players[2].money}</div>
                                <div>
                                    {props.players[2].cards.length
                                        ? props.players[2].cards.map(cardData => (<Card key={v4()} cardData={cardData}/>))
                                        : null
                                    }
                                </div>
                            </>
                        )
                        : null}
                </div>
            </div>
            <div className={style.raw}>
                <div className={style.cell}></div>
                <div className={style.cell}>
                    <div className={style.cardZone}>
                        {props.player.cards.length
                            ? props.player.cards.map(cardData => (<Card key={v4()} cardData={cardData}/>))
                            : null
                        }
                    </div>
                    <div className={style.playerInfoZone}>
                        <div className={style.avatar}>
                            <img className={style.img} src={props.player.avatarUrl} alt=""/>
                            <svg>
                                {props.player.move
                                    ? (<circle className={`${style.timer}`} r="35" cx="40" cy="40"></circle>)
                                    : null}
                            </svg>
                        </div>
                        <div className={style.playerInfo}>
                            <div>{props.player.playerName}</div>
                            <div>Money: {props.player.money}</div>
                            <div>Timer: {timer}</div>
                        </div>
                    </div>
                </div>
                <div className={style.cell}></div>
            </div>

        </div>
    )
}

