import Aside from "./Aside/Aside";
import Center from "./Center/Center";
import RightSide from "./RightSide/RightSide";
// @ts-ignore
import style from "./style.module.css";
import {useEffect, useRef, useState} from "react";
import Game21Service from "../../../services/api/game21.service";
import {useDispatch, useSelector} from "react-redux";

const GamePage = () => {
    const [game, setGame] = useState(false);
    const [userScore, setUserScore] = useState("");
    const [opponentScore, setOpponentScore] = useState("");
    const [userCards, setUserCards] = useState([]);
    const [opponentCards, setOpponentCards] = useState([]);

    // const [money, setMoney] = useState("");
    const [level, setLevel] = useState("");
    const [exp, setExp] = useState("");

    const [userHistory, setUserHistory] = useState([]);

    const user = JSON.parse(localStorage.getItem("user"));
    const userName = user.userName;

    const [showChat, setShowChat] = useState(true);

    const [showMoreUserHistory, setShowMoreUserHistory] = useState(false);

    const getCardRef = useRef(null);
    const startGameRef = useRef(null);
    const passRef = useRef(null);

    // const [time, setTime] = useState("");
    const [gameHistoryMessages, setGameHistoryMessages] = useState([]);
    // const [history, setHistory] = useState([]);

    // @ts-ignore
    const money = useSelector(state => state.money.money);
    const dispatch = useDispatch();

    // const [reloadPage, setReloadPage] = useState(true);
    useEffect(() => {
        const fetchData = async () => {
            let userStatistic = Game21Service.getStatistic();
            let userHistory = Game21Service.getHistory();
            // @ts-ignore
            userStatistic = await userStatistic;
            userHistory = await userHistory;
            console.log(userHistory);

            // @ts-ignore
            // await setMoney(userStatistic.money);
            dispatch(incrementByAmount(userStatistic.money));
            // @ts-ignore
            await setLevel(userStatistic.level);
            // @ts-ignore
            await setExp(userStatistic.exp);

            // @ts-ignore
            await setUserHistory([...userHistory.map(h => {
                h.userName = user.userName;
                h.opponentName = "Bot";
                return h;
            })]);
        }
        fetchData();

        // setReloadPage(false);
        console.log("invoke effect");
    }, [])

    const currentTime = () =>{  // TODO: duplicate, relocate to service?
        const date = new Date();
        return `${String(date.getHours()).padStart(2, "0")}:${String(date.getMinutes()).padStart(2, "0")}`;
    }

    const deleteCards = async () => {
        await setUserCards([]);
        await setOpponentCards([]);
    }


    const updateUserStatistic = async () => {
        const user = JSON.parse(localStorage.getItem("user"));
        const userStatistic = JSON.parse(user.userStatistic);
        const result = await Game21Service.getStatistic();
        // @ts-ignore
        userStatistic.money = result.money;
        // @ts-ignore
        // await setMoney(result.money);
        dispatch(incrementByAmount(userStatistic.money));
        // @ts-ignore
        await setLevel(result.level);
        // @ts-ignore
        await setExp(result.exp);

        localStorage.setItem("user", JSON.stringify(user));
    }

    const endGame = async (gameResult) => {
        await setGame(false);
        await updateUserStatistic();
        await setGameHistoryMessages(prev => [...prev, {currentTime: currentTime(), historyMessageType: "gameState", gameState: "Game over"}]);  // TODO: relocate to endGame()

        // const result = await Game21Service.getHistory(user.token, user.userId);
        // console.log(result[0]);
        await setUserHistory(prev => [{gameResult: gameResult.gameState, userName: user.userName, opponentName: "Bot", score: `${gameResult.userScoreValue} : ${gameResult.opponentScoreValue}`}, ...prev]);
        // deleteCards();
        // showGameResultHistory();
    }

    const clearGame = async () => {
        await deleteCards();
        await setUserScore("0");
        await setOpponentScore("0");
    }



    const startGameHandler = async (event) => {
        event.preventDefault();
        const btn = startGameRef.current;
        btn.disabled = true;

        await clearGame();
        console.log(user.userId, "userId");
        const result = await Game21Service.startGame()
        await setGame(true);
        // @ts-ignore
        await setUserScore(result.userScoreValue);
        // @ts-ignore
        await setOpponentScore(result.opponentScoreValue);

        await setGameHistoryMessages(prev => [...prev, {currentTime: currentTime(), historyMessageType: "gameState", gameState: "Start game"}]);

        btn.disabled = false;
    }

    const getCardHandler = async (event) => {
        event.preventDefault();
        const passBtn = passRef.current;
        const getCardBtn = getCardRef.current;

        passBtn.disabled = true;
        getCardBtn.disabled = true;

        const result = await Game21Service.getCard();

        // @ts-ignore
        await setUserScore(result.userScoreValue);
        // @ts-ignore
        await setOpponentScore(result.opponentScoreValue);

        const userCards = JSON.parse(result.returnedCards).filter(card => card.owner === 1);
        const opponentCards = JSON.parse(result.returnedCards).filter(card => card.owner === 2);

        await setUserCards(prev => [...prev, ...userCards])
        await setOpponentCards(prev => [...prev, ...opponentCards])

        await setGameHistoryMessages(prev => [...prev, {currentTime: currentTime(), historyMessageType: "getCard", playerNameColor: "user", playerName: user.userName, cardValue: userCards[0]?.suitValue, playerScoreValue: result.userScoreValue}]);
        if(opponentCards.length){
            await setGameHistoryMessages(prev => [...prev, {currentTime: currentTime(), historyMessageType: "getCard", playerNameColor: "opponent", playerName: "Bot", cardValue: opponentCards[0]?.suitValue, playerScoreValue: result.opponentScoreValue}]);
        }

        if (result.gameState.toLowerCase() !== "process") {
            await endGame(result);
        }
        passBtn.disabled = false;
        getCardBtn.disabled = false;
    }

    const passHandler = async (event) => {
        event.preventDefault();
        const passBtn = passRef.current;
        const getCardBtn = getCardRef.current;

        passBtn.disabled = true;
        getCardBtn.disabled = true;

        const result = await Game21Service.pass();
        // @ts-ignore
        await setUserScore(result.userScoreValue);
        // @ts-ignore
        await setOpponentScore(result.opponentScoreValue);
        await setUserCards(prev => [...prev, ...JSON.parse(result.returnedCards).filter(card => card.owner === 1)]);
        await setOpponentCards(prev => [...prev, ...JSON.parse(result.returnedCards).filter(card => card.owner === 2)]);

        const userCards = JSON.parse(result.returnedCards).filter(card => card.owner === 1);
        const opponentCards = JSON.parse(result.returnedCards).filter(card => card.owner === 2);

        await setUserCards(prev => [...prev, ...userCards]);
        await setOpponentCards(prev => [...prev, ...opponentCards]);

        await setGameHistoryMessages(prev => [...prev, {currentTime: currentTime(), historyMessageType: "getCard", playerNameColor: "user", playerName: user.userName, cardValue: userCards[0]?.suitValue, playerScoreValue: result.userScoreValue}]);
        if(opponentCards.length){
            setGameHistoryMessages(prev => [...prev, {currentTime: currentTime(), historyMessageType: "getCard", playerNameColor: "opponent", playerName: "Bot", cardValue: opponentCards[0]?.suitValue, playerScoreValue: result.opponentScoreValue}]);
        }
        // setGame(false);
        await endGame(result);

        passBtn.disabled = false;
        getCardBtn.disabled = false;
    }

    const props = {
        game, setGame, userScore, setUserScore,
        opponentScore, setOpponentScore, startGameHandler,
        getCardHandler, passHandler, userCards, setUserCards,
        opponentCards, setOpponentCards, money, level, userName,
        gameHistoryMessages, exp, userHistory, setUserHistory,
        showChat, setShowChat, showMoreUserHistory,
        setShowMoreUserHistory, getCardRef, startGameRef, passRef

    }

    return (
        <>
            <div className={style.content}>
                <Aside props={props}/>
                <Center props={props}/>
                <RightSide props={props}/>
            </div>
        </>

    )
}

export default GamePage;