import {$historyApi} from "./index";
import {TestResult} from "../models/TestResult";

// const history: TestResult[] = [
//     {
//         testId: 1,
//         tasks: [
//             {
//                 taskId: 1,
//                 input: "print(\"Hello, World!\")",
//                 isCorrect: true,
//                 submissionAt: new Date()
//             },
//             {
//                 taskId: 2,
//                 input: "3",
//                 isCorrect: true,
//                 submissionAt: new Date()
//             },
//             {
//                 taskId: 3,
//                 input: "1\n2",
//                 isCorrect: false,
//                 submissionAt: new Date()
//             },
//             {
//                 taskId: 5,
//                 input: "hello world!",
//                 isCorrect: true,
//                 submissionAt: new Date()
//             },
//             {
//                 taskId: 6,
//                 input: "hello world!",
//                 isCorrect: false,
//                 submissionAt: new Date()
//             },
//             {
//                 taskId: 7,
//                 input: "hello world!",
//                 isCorrect: false,
//                 submissionAt: new Date()
//             }
//         ] as TaskResult[]
//     },
//     {
//         testId: 2,
//         tasks: [
//             {
//                 taskId: 8,
//                 input: "hello world! - false",
//                 isCorrect: false,
//                 submissionAt: new Date()
//             },
//             {
//                 taskId: 8,
//                 input: "hello world! - false",
//                 isCorrect: false,
//                 submissionAt: new Date()
//             }
//         ] as TaskResult[]
//     },
//     {
//         testId: 3,
//         tasks: [
//             {
//                 taskId: 9,
//                 input: "hello world!",
//                 isCorrect: true,
//                 submissionAt: new Date()
//             }
//         ] as TaskResult[]
//     },
//     {
//         testId: 5,
//         tasks: [
//             {
//                 taskId: 11,
//                 input: "",
//                 isCorrect: false,
//                 submissionAt: new Date()
//             },
//             {
//                 taskId: 11,
//                 input: "hello world!",
//                 isCorrect: true,
//                 submissionAt: new Date()
//             }
//         ] as TaskResult[]
//     },
//     {
//         testId: 6,
//         tasks: [
//             {
//                 taskId: 12,
//                 input: "hello world!",
//                 isCorrect: true,
//                 submissionAt: new Date()
//             },
//             {
//                 taskId: 12,
//                 input: "hello world! - true",
//                 isCorrect: true,
//                 submissionAt: new Date()
//             }
//         ] as TaskResult[]
//     }
// ]

export const getHistoryApi = async () => {
    const {data} = await $historyApi.get(process.env.REACT_APP_HISTORY_URL_HISTORY as string)

    let {result} = data
    let history = result as TestResult[]

    return {history: history}
}

export const startTestApi = async (testId: number) => {
    const history = (await getHistoryApi()).history.filter(it => it.test.id === testId)

    if (history.length !== 0) {
        console.log(JSON.stringify(history))
        return history[0].id
    }

    const {data} = await $historyApi.post(process.env.REACT_APP_HISTORY_URL_START as string, {testId: testId})
    let {result} = data
    let historyNew = result as TestResult
    console.log(JSON.stringify(historyNew))

    return historyNew.id
}

export const sendTaskApi = async (testId: number, taskId: number, answer: string) => {
    const resultId = await startTestApi(testId)

    const {data} = await $historyApi.post(process.env.REACT_APP_HISTORY_URL_SUBMIT as string, {
        testResultId: resultId,
        taskVersionId: taskId,
        answer: answer
    })
    let {result} = data
    let historyNew = result as TestResult

    return historyNew.id

}