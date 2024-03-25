import {getHistoryApi, sendTaskApi} from "../data/api/historyApi";


export const getHistory = async () => {
    return getHistoryApi()
}

export const sendTask = async (testId: number, taskId: number, answer: string) => {
    return sendTaskApi(testId, taskId, answer)
}