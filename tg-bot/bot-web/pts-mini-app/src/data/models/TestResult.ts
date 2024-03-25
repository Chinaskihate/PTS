import {Task, Test} from "./Test";

export enum Progress {OK = "Ок", Incorrect = "Неверно", InProgress = "В процессе" }

export interface TestResult {
    id: number
    test: TestInfo
    taskResults: TaskResult[]
}

export interface TestInfo {
    id: number,
    name: string,
    description: string,
    isEnabled: boolean
}

export interface TaskResult {
    id: number
    taskVersionId: number
    input: string
    isCorrect: boolean
}

export function getTestProgress(result: TestResult, test: Test) {
    console.log(result)
    console.log(test)
    if (result === undefined || result.taskResults === undefined) return Progress.InProgress

    const taskIds = Array.from(new Set(test.taskVersions.map(it => it.id)))
    let progress = Progress.InProgress


    for (let id of taskIds) {
        const [task] = result.taskResults.filter(it => it.taskVersionId === id)
        if (task === undefined) {
            progress = Progress.InProgress
            break;
        }

        if (!task.isCorrect) progress = Progress.Incorrect

        if (progress !== Progress.Incorrect) progress = Progress.OK
    }

    return progress
}

export function getTaskProgress(result: TestResult, task: Task) {
    if (result === undefined || result.taskResults === undefined) return Progress.InProgress

    const [taskResult] = result.taskResults.filter(it => it.taskVersionId === task.id)

    if (taskResult === undefined) return Progress.InProgress

    if (!taskResult.isCorrect) return Progress.Incorrect;

    return Progress.OK
}