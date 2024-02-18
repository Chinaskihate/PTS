import React, {useContext, useEffect, useRef, useState} from "react";
import {observer} from "mobx-react-lite";
import {MAIN_ROUTE, TEST_ROUTE_LESS_ID} from "../../navigation/navigationConstants";
import {AppContext} from "../../providers/AppProvider";
import {useNavigate} from "react-router-dom";
import {Container, CssBaseline, Stack} from "@mui/material";
import SearchBar from "./components/SearchBar";
import TestInfo from "./components/TestInfo";
import {
    Difficult,
    getTestDifficulties,
    getTestLanguages,
    getTestThemes,
    getTestTime,
    Language,
    Test
} from "../../../data/models/Test";
import {getTests} from "../../../domain/testHandler";
import {getThemes} from "../../../domain/themeHandler";


const Main = observer(() => {
    const {setScreen} = useContext(AppContext)
    const [themes, setThemes] = useState([] as string[])
    const [searchParam, setSearchParam] = useState("")
    const [searchThemes, setSearchThemes] = useState([] as string[])
    const [searchDifficulties, setSearchDifficulties] = useState([] as Difficult[])
    const [searchLanguages, setSearchLanguages] = useState([] as Language[])
    const [searchTests, setSearchTests] = useState([] as Test[])
    const navigate = useNavigate()
    const appBarRef = useRef<HTMLDivElement>(null)

    const [topPadding, setTopPadding] = useState(0)

    const updateTopPadding = () => {
        if (appBarRef.current) {
            setTopPadding(appBarRef.current.offsetHeight);
        }
    };

    useEffect(() => {
        setScreen(MAIN_ROUTE)
    }, []);

    useEffect(() => {
        getThemes().then(data => setThemes(data.themes))
    }, [])

    useEffect(() => {
        getTests(
            searchParam,
            searchThemes,
            searchDifficulties,
            searchLanguages
        )
            .then(data => setSearchTests(data.tests))
            .catch(error => alert(error))
    }, [searchParam, searchThemes, searchDifficulties, searchLanguages]);

    useEffect(() => {
        updateTopPadding();
        const handleResize = (): void => {
            updateTopPadding();
        };

        window.addEventListener('resize', handleResize);

        return () => window.removeEventListener('resize', handleResize);
    }, [appBarRef]);

    return (
        <>
            <SearchBar
                appBarRef={appBarRef}
                onSearchTitle={value => setSearchParam(value)}
                onSearchThemes={value => setSearchThemes(value)}
                onSearchDifficulties={value => setSearchDifficulties(value.map(it => it as Difficult))}
                onSearchLanguages={value => setSearchLanguages(value.map(it => it as Language))}
                themes={themes}
                difficulties={Object.values(Difficult)}
                currentSearch={searchParam}
                selectedDifficulties={searchDifficulties}
                selectedThemes={searchThemes}
                languages={Object.values(Language)}
                selectedLanguages={searchLanguages}/>

            <Container component="main" maxWidth="xs"
                       sx={{mt: `${topPadding + 10}px`, mb: 8, flexGrow: 1, alignItems: "start"}}>
                <CssBaseline/>
                <Stack spacing={1}>
                    {searchTests.map(it => (
                        <TestInfo name={it.title} description={it.description}
                                  languages={getTestLanguages(it)}
                                  themes={getTestThemes(it)} time={getTestTime(it)} createdAt={new Date()}
                                  difficulties={getTestDifficulties(it)}
                                  onClick={() => navigate(TEST_ROUTE_LESS_ID + it.id)}/>
                    ))}
                </Stack>
            </Container>
        </>
    );
})

export default Main