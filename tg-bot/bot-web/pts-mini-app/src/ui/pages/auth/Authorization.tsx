import React, {useContext, useEffect, useState} from "react";
import {observer} from "mobx-react-lite";
import {signIn, signUp} from "../../../domain/authHandler";
import {AppContext} from "../../providers/AppProvider";
import {useNavigate} from "react-router-dom";
import {AUTH_ROUTE, MAIN_ROUTE} from "../../navigation/navigationConstants";
import {useInitData} from "@tma.js/sdk-react";
import {Box, Button, Container, CssBaseline, Grid, TextField, Typography} from "@mui/material";

const Authorization = observer(() => {
    const {setUser, isAuth, setIsAuth, setScreen, screen} = useContext(AppContext)
    const navigate = useNavigate()
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")

    const telegramData = useInitData()

    useEffect(() => {
        if (isAuth) {
            navigate(screen !== "" && screen !== AUTH_ROUTE ? screen : MAIN_ROUTE)
        }
    }, [isAuth])

    useEffect(() => {
        setScreen(AUTH_ROUTE)
    }, []);

    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline/>
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}
            >
                <Typography component="h1" variant="h5">
                    Добро пожаловать!
                </Typography>
                <Box sx={{mt: 1}}>
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        id="email"
                        label="Email"
                        name="email"
                        autoComplete="email"
                        autoFocus
                        onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                            setEmail(event.target.value);
                        }}
                    />
                    <TextField
                        margin="normal"
                        required
                        fullWidth
                        name="password"
                        label="Пароль"
                        type="password"
                        id="password"
                        autoComplete="current-password"
                        onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                            setPassword(event.target.value);
                        }}
                    />

                    <Grid container spacing={2}>
                        <Grid item xs={6} sm={6} md={6}>
                            <Button
                                fullWidth
                                variant="contained"
                                sx={{mt: 4, mb: 2}}
                                onClick={() => {
                                    signIn(email,
                                        password,
                                        telegramData?.user?.id ?? 0)
                                        .then(data => {
                                            if (data.isAuth)
                                                setUser(data.user)

                                            setIsAuth(data.isAuth)
                                        })
                                        .catch(error => console.error(error))
                                }}
                            >
                                Войти
                            </Button>
                        </Grid>
                        <Grid item xs={6} sm={6} md={6}>

                            <Button
                                fullWidth
                                variant="contained"
                                color="secondary"
                                sx={{mt: 4, mb: 2}}
                                onClick={() => {
                                    signUp(email,
                                        password,
                                        telegramData?.user?.id ?? 0)
                                        .then(data => {
                                            if (data.isAuth)
                                                setUser(data.user)

                                            setIsAuth(data.isAuth)
                                        })
                                        .catch(error => console.error(error))
                                }}
                            >
                                Зарегистироваться
                            </Button>
                        </Grid>
                    </Grid>
                </Box>
            </Box>
        </Container>
    );
})

export default Authorization