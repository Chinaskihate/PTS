import axios from "axios";
import {cookie} from "../store";

const $authHost = axios.create(
    {
        baseURL: process.env.REACT_APP_AUTH_URL
    }
)

const $themeApi = axios.create(
    {
        baseURL: process.env.REACT_APP_THEME_URL
    }
)

const authInterceptor = function (config: any) {
    config.headers.authorization = `Bearer ${cookie.get('token')}`
    return config
}

$themeApi.interceptors.request.use(authInterceptor)


export {
    $authHost, $themeApi
}