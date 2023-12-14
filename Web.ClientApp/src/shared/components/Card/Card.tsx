import {useEffect, useRef} from "react";
// @ts-ignore
import style from "./Card.module.css";

const Card = ({cardData}) => {
    const canvasRef = useRef(null);

    useEffect(() => {
        const canvas = canvasRef.current;
        const context = canvas.getContext('2d');
        const image = new Image();

        image.onload = () => {

            const cropWidth = cardData.width;
            const cropHeight = cardData.height;

            canvas.width = cropWidth / 2;
            canvas.height = cropHeight / 2;

            // context.clearRect(0, 0, canvas.width, canvas.height);
            context.drawImage(
                image,
                cardData.x,
                cardData.y,
                cropWidth,
                cropHeight,
                0,
                0,
                cropWidth / 2,
                cropHeight / 2
            );
        };

        image.src = "/img/cards/cards.png";
    }, [cardData]);

    return <canvas className={style.card} ref={canvasRef} />;
}

export default Card;
