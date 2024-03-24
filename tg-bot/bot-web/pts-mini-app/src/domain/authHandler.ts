import {User} from "../ui/providers/AppProvider";
import {authorization, login, registration} from "../data/api/authApi";

export const auth = async (telegramId: number) => {
    const {isAuth, user} = await authorization(telegramId)
    return {isAuth: isAuth, user: (user as User)}
}

export const signIn = async (email: string, password: string) => {
    const {isAuth, user} = await login(email, password)

    return {isAuth: isAuth, user: (user as User)}
}

export const signUp = async (email: string, password: string, telegramId: number) => {
    const {isAuth, user} = await registration(email, password, telegramId)

    return {isAuth: isAuth, user: (user as User)}}