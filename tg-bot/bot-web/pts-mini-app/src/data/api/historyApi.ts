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