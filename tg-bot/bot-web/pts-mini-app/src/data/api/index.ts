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

const $taskApi = axios.create(
    {
        baseURL: process.env.REACT_APP_TASK_URL
    }
)

const $testApi = axios.create(
    {
        baseURL: process.env.REACT_APP_TEST_URL
    }
)

const $historyApi = axios.create(
    {
        baseURL: process.env.REACT_APP_HISTORY_URL
    }
)

const authInterceptor = function (config: any) {
    config.headers.authorization = `Bearer ${cookie.get('token')}`
    return config
}

$authHost.interceptors.request.use(authInterceptor)
$themeApi.interceptors.request.use(authInterceptor)
$taskApi.interceptors.request.use(authInterceptor)
$testApi.interceptors.request.use(authInterceptor)
$historyApi.interceptors.request.use(authInterceptor)


export {
    $authHost, $themeApi, $testApi, $taskApi, $historyApi
}