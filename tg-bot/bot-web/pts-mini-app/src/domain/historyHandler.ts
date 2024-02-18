import {getHistoryApi} from "../data/api/historyApi";


export const getHistory = async (telegramId: number) => {
    return getHistoryApi(telegramId)
}