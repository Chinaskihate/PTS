import {cookie} from "../store";

export const authorization = async (telegramId: number) => {
    cookie.set("email", "test@mail.ru")
    return {isAuth: true, user: {email: "test@mail.ru", telegramId: telegramId}}
}

export const login = async (email: string, password: string, telegramId: number) => {
    cookie.set("email", email)
    return {isAuth: true, user: {email: email, telegramId: telegramId}}
}

export const registration = async (email: string, password: string, telegramId: number) => {
    cookie.set("email", email)
    return {isAuth: true, user: {email: email, telegramId: telegramId}}
}