export enum TaskType {Code = "code", Text = "text"}

export enum Language {Java = "java", Csharp = "c#", Python = "python", Kotlin = "kotlin"}

export enum Difficult {Easy = "easy", Normal = "normal", Hard = "hard"}

export interface Test {
    id: number
    title: string
    description: string
    createdAt: Date
    tasks: Task[]
}

export interface Task {
    id: number
    title: string
    description: string
    specificDescription: string
    useSubstitutions: boolean
    time: number
    difficult: Difficult
    type: TaskType
    language: Language
    themes: string[]
    testCases: TestCase[]
}

export interface TestCase {
    id: number
    visible: boolean
    input: string
    output: string
}

export function getTestThemes(test: Test): string[] {
    return Array.from(new Set(test.tasks.flatMap(it => it.themes)))
}

export function getTestDifficulties(test: Test): Difficult[] {
    return Array.from(new Set(test.tasks.map(it => it.difficult)))
}

export function getTestLanguages(test: Test): Language[] {
    return Array.from(new Set(test.tasks.map(it => it.language)))
}

export function getTestTime(test: Test): number {
    return test.tasks.reduce((sum, task) => sum + task.time, 0)
}