import {Task, Test} from "./Test";

export enum Progress {OK = "Ок", Incorrect = "Неверно", InProgress = "В процессе" }

export interface TestResult {
    testId: number
    tasks: TaskResult[]
}

export interface TaskResult {
    taskId: number
    input: string
    isCorrect: boolean
    submissionAt: Date
}

export function getTestProgress(result: TestResult, test: Test) {
    if (result === undefined || result.tasks === undefined) return Progress.InProgress

    const taskIds = Array.from(new Set(test.tasks.map(it => it.id)))
    let progress = Progress.InProgress

    for (let id of taskIds) {
        const [task] = result.tasks.filter(it => it.taskId === id)
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
    if (result === undefined || result.tasks === undefined) return Progress.InProgress

    const [taskResult] = result.tasks.filter(it => it.taskId === task.id)

    if (taskResult === undefined) return Progress.InProgress

    if (!taskResult.isCorrect) return Progress.Incorrect;

    return Progress.OK
}