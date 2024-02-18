import {observer} from "mobx-react-lite";
import React, {useContext, useEffect, useState} from "react";
import {useNavigate, useParams} from "react-router-dom";
import {Box, CircularProgress, Container, CssBaseline, Divider, Stack, Typography} from "@mui/material";
import {AppContext} from "../../providers/AppProvider";
import {HISTORY_ROUTE, TASK_ROUTE_LESS_ID, TEST_ROUTE_LESS_ID} from "../../navigation/navigationConstants";
import {Test} from "../../../data/models/Test";
import {getTest} from "../../../domain/testHandler";
import TaskInfo from "./components/TaskInfo";
import {getTaskProgress, TestResult} from "../../../data/models/TestResult";
import {getHistory} from "../../../domain/historyHandler";
import {useInitData} from "@tma.js/sdk-react";

const TestPage = observer(() => {
    const {id} = useParams()
    const {setScreen} = useContext(AppContext)
    const [testResult, setTestResult] = useState({} as TestResult)
    const [test, setTest] = useState({} as Test)
    const [loading, setLoading] = useState(true)
    const navigate = useNavigate()

    const telegramData = useInitData()


    useEffect(() => {
        setScreen(TEST_ROUTE_LESS_ID + id)
    }, [id, setScreen]);

    useEffect(() => {
        if (id === undefined) {
            navigate(HISTORY_ROUTE)
            return
        }

        getTest(+id)
            .then(data => {
                if (data.test === undefined) {
                    navigate(HISTORY_ROUTE)
                    return
                }
                setTest(data.test)
                setLoading(false)
            })
            .catch(error => alert(error))

        getHistory(telegramData?.user?.id ?? 0)
            .then(data => setTestResult(data.history.filter(result => result.testId === +id)[0]))
            .catch(error => alert(error))
    }, [id]);


    return (
        <Container component="main" maxWidth="xs" sx={{mb: 8, flexGrow: 1, alignItems: "start"}}>
            <CssBaseline/>
            {loading && (<CircularProgress/>)}
            {!loading && (
                <Stack spacing={1}>
                    <Divider flexItem/>
                    <Box>
                        <Typography sx={{fontWeight: "bold"}} align={"left"} variant={"h5"}>Название</Typography>
                        <Typography align={"left"} variant={"h6"}>{test.title}</Typography>
                    </Box>
                    <Divider flexItem/>
                    <Box>
                        <Typography sx={{fontWeight: "bold"}} align={"left"} variant={"h5"}>Описание</Typography>
                        <Typography align={"left"} variant={"body1"}>{test.description}</Typography>
                    </Box>
                    <Divider flexItem/>
                    <Typography sx={{fontWeight: "bold"}} align={"left"} variant={"h5"}>Задания</Typography>
                    {test.tasks.map(it => (
                        <TaskInfo name={it.title}
                                  description={it.description}
                                  time={it.time}
                                  difficult={it.difficult}
                                  language={it.language}
                                  themes={it.themes}
                                  progress={getTaskProgress(testResult, it)}
                                  onClick={() => navigate(TASK_ROUTE_LESS_ID + id + "/" + it.id)}/>
                    ))}
                </Stack>
            )}

        </Container>
    );
})

export default TestPage