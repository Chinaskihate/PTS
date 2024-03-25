import {cookie} from "../store";
import {$authHost} from "./index";

export const authorization = async (telegramId: number) => {
    await login(process.env.REACT_APP_TG_BOT_EMAIL as string, process.env.REACT_APP_TG_BOT_PASSWORD as string)

    const {data} = await $authHost.post(`${telegramId}${process.env.REACT_APP_AUTH_AUTHORIZATION}`)

    const {result, isSuccess} = data
    cookie.set("token", result?.token)

    return {isAuth: isSuccess && result?.user !== null, user: {email: result?.user?.email, telegramId: result?.user?.telegramId}}
}

export const login = async (email: string, password: string) => {
    const {data} = await $authHost.post(process.env.REACT_APP_AUTH_LOGIN as string, {userName: email, password: password})

    const {result, isSuccess} = data
    cookie.set("token", result?.token)

    return {isAuth: isSuccess && result?.user !== null, user: {email: result?.user?.email, telegramId: result?.user?.telegramId}}
}

export const registration = async (email: string, password: string, telegramId: number) => {
    const {data} = await $authHost.post(process.env.REACT_APP_AUTH_REGISTER as string, {email: email, telegramId: String(telegramId), password: password})

    const {isSuccess} = data
    if (isSuccess) {
        const {isAuth, user} = await login(email, password)
        return {isAuth: isAuth, user: user}
    } else {
        return {isAuth: false, user: {}}
    }
}