import React, {useContext, useEffect, useState} from 'react';
import './App.css';
import {BrowserRouter} from "react-router-dom";
import AppRouter from "./navigation/AppRouter";
import {auth} from "../domain/authHandler";
import {AppContext} from "./providers/AppProvider";
import {useInitData, useMainButton, useMiniApp, useViewport} from "@tma.js/sdk-react";
import {CircularProgress, createTheme, ThemeProvider} from "@mui/material";

import BottomMenu from "./pages/bottomMenu/BottomMenu";


const defaultTheme = createTheme({
    typography: {
        button: {
            fontSize: '12px'
        },
    }
});

function App() {
    const [loading, setLoading] = useState(false)
    const {isAuth, setIsAuth, setUser} = useContext(AppContext)
    const miniApp = useMiniApp();
    const viewport = useViewport();
    const mainButton = useMainButton();
    const telegramData = useInitData()


    useEffect(() => {
        miniApp.ready();
    }, [miniApp]);

    useEffect(() => {
        if (!viewport.isExpanded) {
            viewport.expand()
        }
    }, [viewport]);


    useEffect(() => {
        mainButton.setText("Закрыть")
        mainButton.enable()
        mainButton.setBackgroundColor('#373E40')
        mainButton.setTextColor('#EFF9F0')
        mainButton.on("click", () => {
            miniApp.close()
        })
        mainButton.show()
    }, [mainButton]);

    useEffect(() => {
        auth(telegramData?.user?.id ?? 0).then(data => {
            setIsAuth(data.isAuth)
            if (data.isAuth)
                setUser(data.user)
        }).catch(error => {
            console.error(error)
        }).finally(() => {
            setLoading(false)
        })
    }, [setIsAuth, setUser])

    return (
        <div className="App">
            <ThemeProvider theme={defaultTheme}>
                {loading && (<CircularProgress/>)}
                {!loading && (<BrowserRouter>
                    <AppRouter/>
                    {isAuth && (<BottomMenu/>)}
                </BrowserRouter>)}
            </ThemeProvider>
        </div>
    );
}

export default App;