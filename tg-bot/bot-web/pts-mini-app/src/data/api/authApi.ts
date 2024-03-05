import {cookie} from "../store";
import {$authHost} from "./index";

export const authorization = async (telegramId: number) => {
    return {isAuth: true, user: {email: "test@mail.ru", telegramId: telegramId}}
}

export const login = async (email: string, password: string, telegramId: number) => {
    const {data} = await $authHost.post(process.env.REACT_APP_AUTH_LOGIN as string, {userName: email, password: password})

    const {result, isSuccess} = data
    cookie.set("token", result?.token)
    console.log(JSON.stringify(data))

    return {isAuth: isSuccess, user: {email: result?.user?.email, telegramId: telegramId}}
}

export const registration = async (email: string, password: string, telegramId: number) => {
    const {data} = await $authHost.post(process.env.REACT_APP_AUTH_REGISTER as string, {email: email, telegramId: String(telegramId), phoneNumber: "", password: password, role: "ADMIN"})

    const {isSuccess} = data
    if (isSuccess) {
        const {isAuth, user} = await login(email, password, telegramId)
        return {isAuth: isAuth, user: user}
    } else {
        return {isAuth: false, user: {}}
    }
}