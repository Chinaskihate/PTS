import {AUTH_ROUTE, MAIN_ROUTE, HISTORY_ROUTE, TEST_ROUTE, TASK_ROUTE} from "./navigationConstants";
import Authorization from "../pages/auth/Authorization";
import Main from "../pages/main/Main";
import History from "../pages/history/History";
import TestPage from "../pages/test/Test";
import TaskPage from "../pages/task/Task";

export const routes = [
    {
        path: AUTH_ROUTE,
        Component: Authorization
    },
    {
        path: MAIN_ROUTE,
        Component: Main
    },
    {
        path: HISTORY_ROUTE,
        Component: History
    },
    {
        path: TEST_ROUTE,
        Component: TestPage
    },
    {
        path: TASK_ROUTE,
        Component: TaskPage
    }
]
