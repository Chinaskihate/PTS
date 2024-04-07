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
        progress = Progress.InProgress
        var taskResults = result.taskResults.filter(it => it.taskVersionId === id)

        if (taskResults.length === 0) {
            progress = Progress.InProgress
            break
        }

        let progressTask = Progress.Incorrect
        for (let taskResult of taskResults) {
            if (!taskResult.isCorrect && progressTask !== Progress.OK) {
                progressTask = Progress.Incorrect
            }

            if (taskResult.isCorrect) {
                progressTask = Progress.OK
            }
        }

        if (progressTask === Progress.Incorrect) {
            progress = Progress.Incorrect
            continue
        }

        // @ts-ignore
        if (progress !== Progress.Incorrect) {
            progress = Progress.OK
        }

    }

    return progress
}

export function getTaskProgress(result: TestResult, task: Task) {
    if (result === undefined || result.taskResults === undefined) return Progress.InProgress

    let taskResults = result.taskResults.filter(it => it.taskVersionId === task.id)
    let progressTask = Progress.InProgress
    for (let taskResult of taskResults) {
        if (!taskResult.isCorrect && progressTask !== Progress.OK) {
            progressTask = Progress.Incorrect
        }

        if (taskResult.isCorrect) {
            progressTask = Progress.OK
        }
    }

    return progressTask
}