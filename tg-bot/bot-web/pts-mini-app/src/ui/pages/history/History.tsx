import React, {useContext, useEffect, useState} from "react";
import {AppContext} from "../../providers/AppProvider";
import {useNavigate} from "react-router-dom";
import {useInitData} from "@tma.js/sdk-react";
import {HISTORY_ROUTE, TEST_ROUTE_LESS_ID} from "../../navigation/navigationConstants";
import {Container, CssBaseline, Stack, Typography} from "@mui/material";
import {observer} from "mobx-react-lite";
import {getTestDifficulties, getTestLanguages, getTestThemes, getTestTime, Test} from "../../../data/models/Test";
import {getHistory} from "../../../domain/historyHandler";
import {getTestProgress, TestResult} from "../../../data/models/TestResult";
import TestInfo from "./components/TestInfo";
import {getTestsByIds} from "../../../domain/testHandler";

const History = observer(() => {
    const {setScreen} = useContext(AppContext)
    const [testResults, setTestResults] = useState([] as TestResult[])
    const [tests, setTests] = useState([] as Test[])
    const navigate = useNavigate()

    const telegramData = useInitData()

    useEffect(() => {
        setScreen(HISTORY_ROUTE)
    }, []);

    useEffect(() => {
        getHistory(telegramData?.user?.id ?? 0)
            .then(data => setTestResults(data.history))
            .catch(error => alert(error))
    }, []);

    useEffect(() => {
        getTestsByIds(testResults.map(it => it.testId))
            .then(data => setTests(data.tests))
            .catch(error => alert(error))
    }, [testResults]);

    return (
        <Container component="main" maxWidth="xs" sx={{mt: 1, mb: 8, flexGrow: 1, alignItems: "start"}}>
            <CssBaseline/>
            <Stack spacing={1}>
                <Typography sx={{fontWeight: "bold"}} align={"left"}
                            variant={"h6"}>История</Typography>
                {tests.map(it => (
                    <TestInfo name={it.title} description={it.description}
                              languages={getTestLanguages(it)}
                              themes={getTestThemes(it)}
                              time={getTestTime(it)}
                              progress={getTestProgress(testResults.filter(result => result.testId === it.id)[0], it)}
                              difficulties={getTestDifficulties(it)}
                              onClick={() => navigate(TEST_ROUTE_LESS_ID + it.id)}/>
                ))}
            </Stack>
        </Container>
    );
})

export default History