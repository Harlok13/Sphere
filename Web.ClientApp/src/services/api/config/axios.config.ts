import axios from "axios";

const baseURL: string = "https://localhost:7170";

export const API = axios.create({
    baseURL,
    withCredentials: true
});

API.interceptors.request.use(
    function (req) {
        const token = JSON.parse(localStorage.getItem("token"));
        if (token) {
            req.headers["auth-token"] = token;  // TODO: fix header
            req.headers["Authorization"] = token;
        }
        return req;
    },
    function (error) {
        return Promise.reject(error);
    }
);