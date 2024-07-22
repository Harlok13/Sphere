import {useEffect, useRef} from "react";
import style from "./Card.module.css";

export const Card = ({cardData}) => {
    const canvasRef = useRef(null);

    useEffect(() => {
        const canvas = canvasRef.current;
        const context = canvas.getContext('2d');
        const image = new Image();

        image.onload = () => {

            const cropWidth = cardData.width;
            const cropHeight = cardData.height;

            canvas.width = cropWidth / 4;
            canvas.height = cropHeight / 4;

            // context.clearRect(0, 0, canvas.width, canvas.height);
            context.drawImage(
                image,
                cardData.x,
                cardData.y,
                cropWidth,
                cropHeight,
                0,
                0,
                cropWidth / 4,
                cropHeight / 4
            );
        };

        image.src = "/img/cards/cards.png";
    }, [cardData]);

    return <canvas className={style.card} ref={canvasRef} />;
}
