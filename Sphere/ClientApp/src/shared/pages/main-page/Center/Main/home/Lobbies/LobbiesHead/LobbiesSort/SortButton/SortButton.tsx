// @ts-ignore
import style from "./SortButton.module.css";

type SortButtonProps = {
    displayName: string;
    orderBy: string;  // TODO: const desc / asc
}

export const SortButton = (props: SortButtonProps) => {
    return (
        <button className={style.sortBtn}>
            {props.displayName}
            <span className="material-icons-outlined">
                {props.orderBy === "desc" ? "expand_less" : props.orderBy === "asc" ? "expand_more" : "minimize"}
            </span>
        </button>
    )
}


// <span className="material-icons-outlined">
// expand_more
// </span>

// <span className="material-icons-outlined">
// minimize
// </span>