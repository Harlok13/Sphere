import React, {PropsWithChildren, useEffect, useRef} from "react";
import style from "./GameArea.module.css";
import {v4} from "uuid";
import {useTimerSelector} from "store/player/use-player-selector";
import {Card} from "components/shared/components/Card/Card";

export const GameArea = ({children}: PropsWithChildren) =>
        <div className={style.gameArea}>
            {children}
        </div>


export const TestArea = ({props}) => {
    props.players = props.players.filter(p => p.id !== props.player.id);
    const timer = useTimerSelector();

    const s = useRef<SVGCircleElement>()  // TODO: ref, relocate
    useEffect(() => {
        if (s.current){
            s.current.style.strokeDashoffset = `${22 * timer}`;
            if (timer <= 5) {
                s.current.style.stroke = 'green';
            } else if (timer <= 7) {
                s.current.style.stroke = 'yellow';
            } else {
                s.current.style.stroke = 'red';
            }
        }
    }, [timer])

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
                                    ? (<circle ref={s} className={`${style.timer}`} r="35" cx="40" cy="40"></circle>)
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

