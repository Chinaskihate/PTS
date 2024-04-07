import {Difficult, getTestDifficulties, getTestLanguages, Language, Test, updateTest} from "../models/Test";
import {$generateApi, $testApi} from "./index";
import {Theme} from "../models/Theme";
import {login} from "./authApi";
import {cookie} from "../store";

/*const tests: Test[] = [
    {
        id: 1,
        title: "Basic Programming Concepts",
        description: "This test covers basic programming concepts in different languages.",
        createdAt: new Date(),
        tasks: [
            {
                id: 1,
                title: "Hello World",
                description: "Write a program that prints 'Hello, World!' to the console.",
                specificDescription: "Specific",
                useSubstitutions: false,
                time: 30,
                difficult: Difficult.Easy,
                type: TaskType.Code,
                language: Language.Python,
                themes: ["cat1", "cat2"],
                testCases: [
                    { id: 1, input: "", output: "Hello, World!", visible: true },
                    { id: 2, input: "", output: "Hello, World!", visible: true }
                ]
            },
            {
                id: 2,
                title: "Basic Arithmetic",
                description: "Solve simple arithmetic problems.",
                specificDescription: "Include addition, subtraction, multiplication, and division.",
                useSubstitutions: true,
                time: 45,
                difficult: Difficult.Normal,
                type: TaskType.OneAnswer,
                language: Language.Java,
                themes: ["cat3", "cat4"],
                testCases: [
                    { id: 2, input: `{"answers": ["2", "3", "4", "5"]}`, output: "4", visible: true },
                ]
            },
            {
                id: 3,
                title: "String Manipulation",
                description: "Perform basic string operations.",
                specificDescription: "Concatenate, slice, and reverse a string.",
                useSubstitutions: false,
                time: 35,
                difficult: Difficult.Normal,
                type: TaskType.MultipleAnswer,
                language: Language.Python,
                themes: ["cat5", "cat6"],
                testCases: [
                    { id: 2, input: `{"answers": ["2", "3", "4", "5"]}`, output: "4", visible: true },
                ]
            },
            {
                id: 4,
                title: "Control Structures",
                description: "Use control structures to solve problems.",
                specificDescription: "Implement loops and conditional statements.",
                useSubstitutions: true,
                time: 40,
                difficult: Difficult.Normal,
                type: TaskType.Code,
                language: Language.Csharp,
                themes: ["cat7", "cat8"],
                testCases: [
                    { id: 5, input: "5", output: "1 2 3 4 5", visible: true }
                ]
            },
            {
                id: 5,
                title: "Data Types and Variables",
                description: "Demonstrate understanding of different data types and variables.",
                specificDescription: "Create examples of various data types.",
                useSubstitutions: false,
                time: 20,
                difficult: Difficult.Easy,
                type: TaskType.Text,
                language: Language.Java,
                themes: ["cat9", "cat10"],
                testCases: []
            },
            {
                id: 6,
                title: "Array Operations",
                description: "Perform operations on arrays.",
                specificDescription: "Sort, search, and manipulate arrays.",
                useSubstitutions: true,
                time: 45,
                difficult: Difficult.Normal,
                type: TaskType.Code,
                language: Language.Kotlin,
                themes: ["cat11", "cat12"],
                testCases: [
                    { id: 6, input: "[3, 1, 4, 1, 5, 9, 2, 6]", output: "[1, 1, 2, 3, 4, 5, 6, 9]", visible: true }
                ]
            },
            {
                id: 7,
                title: "Functions and Methods",
                description: "Write reusable code with functions and methods.",
                specificDescription: "Create a function that calculates the factorial of a number.",
                useSubstitutions: false,
                time: 50,
                difficult: Difficult.Hard,
                type: TaskType.Code,
                language: Language.Python,
                themes: ["cat13", "cat1"],
                testCases: [
                    { id: 7, input: "5", output: "120", visible: true }
                ]
            }
        ]
    },
    {
        id: 2,
        title: "Advanced Programming Challenges",
        description: "Advanced level tasks for experienced programmers.",
        createdAt: new Date(),
        tasks: [
            {
                id: 8,
                title: "Data Structures",
                description: "Implement a specific data structure.",
                specificDescription: "Create a binary search tree.",
                useSubstitutions: false,
                time: 60,
                difficult: Difficult.Hard,
                type: TaskType.Code,
                language: Language.Csharp,
                themes: ["cat5", "cat6"],
                testCases: [
                    { id: 4, input: "5 3 7 2 4 6 8", output: "2 3 4 5 6 7 8", visible: true }
                ]
            }
        ]
    },
    {
        id: 3,
        title: "Web Development Basics",
        description: "This test covers basic concepts of web development including HTML, CSS, and JavaScript.",
        createdAt: new Date(),
        tasks: [
            {
                id: 9,
                title: "HTML Structure",
                description: "Create a basic HTML page structure.",
                specificDescription: "Include doctype, head, title, and body tags.",
                useSubstitutions: false,
                time: 20,
                difficult: Difficult.Easy,
                type: TaskType.Text,
                language: Language.Java, // Assuming a generic approach for web tasks
                themes: ["cat7", "cat8"],
                testCases: [
                    { id: 5, input: "", output: "<!DOCTYPE html><html><head><title>Page Title</title></head><body></body></html>", visible: true }
                ]
            }
        ]
    },
    {
        id: 4,
        title: "Object-Oriented Programming",
        description: "Test on OOP concepts using Java and C#.",
        createdAt: new Date(),
        tasks: [
            {
                id: 10,
                title: "Inheritance in Java",
                description: "Demonstrate basic inheritance in Java.",
                specificDescription: "Create a class hierarchy with at least one parent and one child class.",
                useSubstitutions: true,
                time: 40,
                difficult: Difficult.Normal,
                type: TaskType.Text,
                language: Language.Java,
                themes: ["cat9", "cat10"],
                testCases: [
                    { id: 6, input: "Child class method call", output: "Method output from child class", visible: false }
                ]
            }
        ]
    },
    {
        id: 5,
        title: "Functional Programming in Python",
        description: "Challenges related to functional programming paradigms in Python.",
        createdAt: new Date(),
        tasks: [
            {
                id: 11,
                title: "Lambda Functions",
                description: "Use lambda functions to perform operations on a list.",
                specificDescription: "Filter and map a list of numbers.",
                useSubstitutions: false,
                time: 25,
                difficult: Difficult.Normal,
                type: TaskType.Code,
                language: Language.Python,
                themes: ["cat11", "cat12"],
                testCases: [
                    { id: 7, input: "[1, 2, 3, 4, 5]", output: "[2, 4]", visible: true } // Assuming a filtering operation for even numbers
                ]
            }
        ]
    },
    {
        id: 6,
        title: "Database Fundamentals",
        description: "Test covering basic SQL queries and database design.",
        createdAt: new Date(),
        tasks: [
            {
                id: 12,
                title: "SQL Queries",
                description: "Write SQL queries to interact with a database.",
                specificDescription: "Include SELECT, INSERT, UPDATE, and DELETE queries.",
                useSubstitutions: true,
                time: 50,
                difficult: Difficult.Normal,
                type: TaskType.Text,
                language: Language.Kotlin, // Assuming a broader context for database manipulation
                themes: ["cat13", "cat1"],
                testCases: [
                    { id: 8, input: "Insert data into table", output: "Data inserted successfully", visible: false }
                ]
            }
        ]
    }
];*/

export const getTestsApi = async (searchTitle: string, themes: Theme[], difficulties: Difficult[], languages: Language[], taskCount: number) => {
    let userToken = cookie.get("token")
    await login(process.env.REACT_APP_TG_BOT_EMAIL as string, process.env.REACT_APP_TG_BOT_PASSWORD as string)
    let themeIds = themes.length !== 0 ? themes.map(it => it.id) as number[] : null

    const {data} = await $testApi.post(process.env.REACT_APP_TEST_URL_FILTERED as string, {
        name: searchTitle,
        taskCount: taskCount,
        themeIds: themeIds
    })
    cookie.set("token", userToken)

    let {result} = data
    let tests = (result as Test[])
    tests.forEach(it => updateTest(it))

    if (difficulties.length !== 0) {
        tests = tests.filter(it => getTestDifficulties(it).some(difficult => difficulties.includes(difficult)))
    }

    if (languages.length !== 0) {
        tests = tests.filter(it => getTestLanguages(it).some(language => languages.includes(language)))
    }
    console.log(tests)

    return {tests: tests}
}

export const getTestApi = async (id: number) => {
    let userToken = cookie.get("token")
    await login(process.env.REACT_APP_TG_BOT_EMAIL as string, process.env.REACT_APP_TG_BOT_PASSWORD as string)
    const {data} = await $testApi.get(id.toString())
    cookie.set("token", userToken)

    let {result} = data
    let test = result as Test
    updateTest(test)

    return {test: test}
}

export const getTestsByIdsApi = async (ids: number[]) => {
    let tests = [] as Test[]
    for (let id of ids) {
        tests = [...tests, (await getTestApi(id)).test]
    }

    return {tests: tests}
}

export const generateTestApi = async (themes: Theme[], taskCount: number) => {
    console.log(themes)
    console.log(taskCount)
    let userToken = cookie.get("token")
    await login(process.env.REACT_APP_TG_BOT_EMAIL as string, process.env.REACT_APP_TG_BOT_PASSWORD as string)
    let themeIds = themes.length !== 0 ? themes.map(it => it.id) as number[] : null

    const {data} = await $generateApi.post(process.env.REACT_APP_GENERATOR_URL_GENERATE as string, {
        taskCount: taskCount,
        themeIds: themeIds
    })

    cookie.set("token", userToken)

    console.log(data)

    var {result} = data

    return {testId: +result}
}