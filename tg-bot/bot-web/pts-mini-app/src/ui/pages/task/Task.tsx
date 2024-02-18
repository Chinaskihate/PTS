import {observer} from "mobx-react-lite";
import {useNavigate, useParams} from "react-router-dom";
import React, {useContext, useEffect, useRef, useState} from "react";
import {AppContext} from "../../providers/AppProvider";
import ContentCopyIcon from '@mui/icons-material/ContentCopy';
import CodeMirror from "@uiw/react-codemirror";
import {HISTORY_ROUTE, TASK_ROUTE_LESS_ID, TEST_ROUTE_LESS_ID} from "../../navigation/navigationConstants";
import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Box, Button,
    Chip,
    CircularProgress,
    Container,
    CssBaseline,
    Divider,
    Grid,
    IconButton,
    Stack,
    TextField,
    Typography
} from "@mui/material";
import {Language, Task, TaskType, Test} from "../../../data/models/Test";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import {getTest} from "../../../domain/testHandler";
import TaskBar from "./components/TaskBar";
import {langs} from "@uiw/codemirror-extensions-langs";
import {Progress, TaskResult} from "../../../data/models/TestResult";
import {getHistory} from "../../../domain/historyHandler";
import {useInitData} from "@tma.js/sdk-react";
import {getProgressColor} from "../../utils/colorsPicker";
import {signUp} from "../../../domain/authHandler";

function getLanguageExtensions(lang: Language) {
    switch (lang) {
        case Language.Java:
            return langs.java();
        case Language.Python:
            return langs.python();
        case Language.Kotlin:
            return langs.kotlin();
        case Language.Csharp:
            return langs.csharp();
        default:
            return langs.c();
    }
}

const TaskPage = observer(() => {
    const {testId, taskId} = useParams()
    const {setScreen, screen} = useContext(AppContext)
    const [loading, setLoading] = useState(true)
    const [test, setTest] = useState({} as Test)
    const [task, setTask] = useState({} as Task)
    const [taskResults, setTaskResults] = useState([] as TaskResult[])
    const appBarRef = useRef<HTMLDivElement>(null)
    const [topPadding, setTopPadding] = useState(0)
    const [taskInput, setTaskInput] = useState("")
    const navigate = useNavigate()

    const telegramData = useInitData()

    useEffect(() => {
        setScreen(TASK_ROUTE_LESS_ID + testId + "/" + taskId)
    }, []);

    useEffect(() => {
        setTaskInput("")
    }, [taskId, testId]);

    useEffect(() => {
        if (testId === undefined || taskId === undefined) {
            navigate(HISTORY_ROUTE)
            return
        }

        getTest(+testId).then(data => {
            if (data.test === undefined || data.test.tasks === undefined || data.test.tasks.filter(it => it.id === +taskId).length === 0) {
                navigate(HISTORY_ROUTE)
                return
            }
            const [task] = data.test.tasks.filter(it => it.id === +taskId)
            setTest(data.test)
            setTask(task)

            setLoading(false)
        }).catch(error => alert(error))

        getHistory(telegramData?.user?.id ?? 0)
            .then(data => {
                const [testResult] = data.history.filter(result => result.testId === +testId)
                if (testResult === undefined || testResult.tasks === undefined) {
                    setTaskResults([])
                    return
                }

                setTaskResults(testResult.tasks.filter(it => it.taskId === +taskId))
            }).catch(error => alert(error))
    }, [taskId, testId]);

    const updateTopPadding = () => {
        if (appBarRef.current) {
            setTopPadding(appBarRef.current.offsetHeight);
        }
    };

    useEffect(() => {
        updateTopPadding();
        const handleResize = (): void => {
            updateTopPadding();
        };

        window.addEventListener('resize', handleResize);

        return () => window.removeEventListener('resize', handleResize);
    }, [appBarRef, loading]);


    return (
        <>
            {loading && (<CircularProgress/>)}
            {!loading && (
                <>
                    <TaskBar
                        appBarRef={appBarRef}
                        title={task.title}
                        items={test.tasks.map(it => ({name: it.title, id: it.id}))}
                        onNavClick={(navTaskId) => {
                            navigate(TASK_ROUTE_LESS_ID + testId + "/" + navTaskId)
                        }}
                        onBackClick={() => {
                            navigate(TEST_ROUTE_LESS_ID + testId)
                        }}/>
                    <Container component="main" maxWidth="xs"
                               sx={{
                                   mt: `${10 + topPadding}px`,
                                   mb: 8,
                                   flexGrow: 1,
                                   alignItems: "start",
                               }}>
                        <CssBaseline/>
                        <Stack spacing={1}>
                            <Divider flexItem/>
                            <Box>
                                <Typography sx={{fontWeight: "bold"}} align={"left"} variant={"h6"}>Условие</Typography>
                                <Typography align={"left"} variant={"body1"}>{task.description}</Typography>
                            </Box>
                            {task.specificDescription !== "" && (
                                <>
                                    <Divider flexItem/>
                                    <Box>
                                        <Typography sx={{fontWeight: "bold"}} align={"left"}
                                                    variant={"h6"}>Дополнительно</Typography>
                                        <Typography align={"left"}
                                                    variant={"body1"}>{task.specificDescription}</Typography>
                                    </Box>
                                </>
                            )}
                            {task.type === TaskType.Code && (
                                <>
                                    <Divider flexItem/>
                                    <Box>
                                        <Typography sx={{fontWeight: "bold", mb: 2}} align={"left"}
                                                    variant={"h6"}>Примеры</Typography>
                                        <Stack>
                                            {task.testCases.filter(it => it.visible).map(it => (
                                                <Grid container sx={{mb: 1}}>
                                                    <Grid xs={6}>
                                                        <Stack spacing={1} divider={<Divider/>}>
                                                            <Typography variant={"body1"} align={"left"}
                                                                        fontWeight={"bold"}>Ввод</Typography>
                                                            <Typography variant={"body2"}
                                                                        align={"left"}>{it.input}</Typography>
                                                        </Stack>

                                                    </Grid>
                                                    <Grid xs={6}>
                                                        <Stack spacing={1} divider={<Divider/>}>
                                                            <Typography variant={"body1"} align={"left"}
                                                                        fontWeight={"bold"}>Вывод</Typography>
                                                            <Typography variant={"body2"}
                                                                        align={"left"}>{it.output}</Typography>
                                                        </Stack>
                                                    </Grid>
                                                </Grid>
                                            ))}
                                        </Stack>


                                    </Box>
                                </>
                            )}
                            <Divider flexItem/>
                            <Stack direction={"row"} spacing={1} alignItems={"center"} justifyContent={"space-between"}>
                                <Stack direction={"row"} spacing={1} alignItems={"center"}>
                                    <Typography sx={{fontWeight: "bold", mb: 2}} align={"left"}
                                                variant={"h6"}>Ваш ответ</Typography>

                                    <IconButton
                                        color="inherit"
                                        aria-label="open drawer"
                                        edge="end"
                                        onClick={() => navigator.clipboard.writeText(taskInput)}
                                    >
                                        <ContentCopyIcon/>
                                    </IconButton>
                                </Stack>

                                <Chip sx={{minWidth: "60px", borderColor: "gray", color: "gray"}}
                                      label={task.language} size={"small"} variant={"outlined"}
                                />
                            </Stack>


                            {task.type === TaskType.Text && (
                                <TextField
                                    margin="normal"
                                    fullWidth
                                    multiline
                                    value={taskInput}
                                    name="text"
                                    label="Ответ"
                                    type="text"
                                    onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                                        setTaskInput(event.target.value);
                                    }}
                                />
                            )}
                            {task.type === TaskType.Code && (
                                <CodeMirror
                                    height={"30vh"}
                                    value={taskInput}
                                    style={{textAlign: "left"}}
                                    extensions={[getLanguageExtensions(task.language)]}
                                    onChange={(value) => setTaskInput(value)}
                                />
                            )}

                            <Button
                                fullWidth
                                variant="contained"
                                color="primary"
                                sx={{mt: 4, mb: 2}}
                                onClick={() => {

                                }}
                            >
                                Отправить
                            </Button>

                            <Divider flexItem/>
                            <Typography sx={{fontWeight: "bold", mb: 2}} align={"left"} variant={"h6"}>
                                История
                            </Typography>

                            {taskResults.map((it, index) => (
                                <Accordion>
                                    <AccordionSummary
                                        expandIcon={<ExpandMoreIcon/>}
                                        aria-controls="panel1-content"
                                        id="panel1-header"
                                    >
                                        <Stack direction={"row"} spacing={2} alignItems={"center"}>
                                            <Typography>Ответ {index + 1}</Typography>
                                            <Chip sx={{
                                                minWidth: "60px",
                                                backgroundColor: getProgressColor(it.isCorrect ? Progress.OK : Progress.Incorrect),
                                                color: "white"
                                            }}
                                                  label={it.isCorrect ? Progress.OK : Progress.Incorrect} size={"small"}
                                                  variant={"filled"}
                                            />
                                        </Stack>
                                    </AccordionSummary>
                                    <AccordionDetails>
                                        {task.type === TaskType.Text && (
                                            <Typography sx={{fontWeight: "bold", mb: 2}} align={"left"}
                                                        variant={"h6"}>{it.input}</Typography>
                                        )}
                                        {task.type === TaskType.Code && (
                                            <CodeMirror
                                                height={"30vh"}
                                                value={it.input}
                                                readOnly
                                                style={{textAlign: "left"}}
                                                extensions={[getLanguageExtensions(task.language)]}
                                            />
                                        )}
                                    </AccordionDetails>
                                </Accordion>
                            ))}
                        </Stack>
                    </Container>
                </>
            )}

        </>

    )
})

export default TaskPage