import { FaSearch } from "react-icons/fa";
import style from "./RoomsSearch.module.css";
import {useRef} from "react";

export const RoomsSearch = () => {
    const searchRef = useRef<HTMLInputElement>();

    const searchHandler = () => {  // TODO: relocate the logic
        searchRef.current.focus();
    }

    return (
        <div>
            <FaSearch onClick={searchHandler} className={style.searchIcon}/>
            <input ref={searchRef} className={style.search} placeholder="Search by room name" type="text"/>
        </div>
    )
}