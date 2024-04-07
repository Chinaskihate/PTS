import {Theme} from "./Theme";

export enum TaskType {Code = "code", Text = "text", OneAnswer = "one answer", MultipleAnswer = "multiple answers"}

export enum Language { CsharpV9 = "c#_9", CsharpV10 = "c#_10", Python_3_1 = "python_3.1"}

export enum Difficult {Easy = "easy", Normal = "normal", Hard = "hard"}

export interface Test {
    id: number
    name: string
    description: string
    createdAt: Date
    taskVersions: Task[]
}

export interface Task {
    id: number
    name: string
    description: string
    avgTimeInMin: number
    inputCondition: string
    outputCondition: null
    codeTemplate: string
    time: number
    complexity: number
    difficult: Difficult
    type: number
    typeEnum: TaskType
    programmingLanguage: number
    language: Language
    themes: Theme[]
    testCases: TestCase[]
}

export interface TestCase {
    id: number
    input: string
    output: string
}

export function updateTest(test: Test) {
    test.taskVersions.forEach(it => {
        switch (it.type) {
            case 0:
                it.typeEnum = TaskType.OneAnswer
                break
            case 1:
                it.typeEnum = TaskType.MultipleAnswer
                break
            case 2:
                it.typeEnum = TaskType.Text
                break
            case 3:
                it.typeEnum = TaskType.Code
                break
            default:
                break
        }

        switch (it.type) {
            case 1:
                it.language = Language.CsharpV9
                break
            case 2:
                it.language = Language.CsharpV10
                break
            case 3:
                it.language = Language.Python_3_1
                break
            default:
                break
        }

        switch (it.complexity) {
            case 0:
                it.difficult = Difficult.Easy
                break
            case 5:
                it.difficult = Difficult.Normal
                break
            case 10:
                it.difficult = Difficult.Hard
                break
            default:
                break
        }

        it.time = it.avgTimeInMin
    })
}

export function getTestThemes(test: Test): Theme[] {
    return test.taskVersions.flatMap(it => it.themes).filter(it => it !== undefined).filter(
        (theme, i, arr) => arr.findIndex(t => t.id === theme.id) === i
    )
}

export function getTestDifficulties(test: Test): Difficult[] {
    return Array.from(new Set(test.taskVersions.map(it => it.difficult).filter(it => it !== undefined)))
}

export function getTestLanguages(test: Test): Language[] {
    return Array.from(new Set(test.taskVersions.map(it => it.language).filter(it => it !== undefined)))
}

export function getTestTime(test: Test): number {
    return test.taskVersions.reduce((sum, task) => sum + task.time, 0)
}