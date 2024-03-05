import axios from "axios";
import {cookie} from "../store";

const $authHost = axios.create(
    {
        baseURL: process.env.REACT_APP_AUTH_URL
    }
)


export {
    $authHost
}