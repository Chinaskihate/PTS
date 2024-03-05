import {observer} from "mobx-react-lite";
import {
    Avatar,
    BottomNavigation,
    BottomNavigationAction,
    Box,
    Button, Drawer,
    Paper,
    Stack,
    Typography
} from "@mui/material";
import HomeIcon from "@mui/icons-material/Home";
import BarChartIcon from '@mui/icons-material/BarChart';
import PersonIcon from "@mui/icons-material/Person";
import React, {useContext, useState} from "react";
import {useInitData} from "@tma.js/sdk-react";
import {deepOrange} from "@mui/material/colors";
import {AppContext} from "../../providers/AppProvider";
import {useNavigate} from "react-router-dom";
import {MAIN_ROUTE, HISTORY_ROUTE} from "../../navigation/navigationConstants";


const BottomMenu = observer(() => {
    const [profileState, setProfileState] = useState(false);
    const {setIsAuth, user, screen, setScreen} = useContext(AppContext)
    const navigate = useNavigate()
    const telegramData = useInitData()

    const handleScreenChange = (newValue: string) => {

        switch (newValue) {
            case MAIN_ROUTE: {
                setScreen(newValue);
                navigate(MAIN_ROUTE)
                break
            }
            case HISTORY_ROUTE: {
                setScreen(newValue);
                navigate(HISTORY_ROUTE)
                break
            }
            case "profile": 
                setProfileState(true)
                break
        }
    };

    return (
        <Paper sx={{position: 'fixed', bottom: 0, left: 0, right: 0}} elevation={3}>
            <BottomNavigation value={screen} onChange={(_, value) => handleScreenChange(value)}>
                <BottomNavigationAction
                    label="Главная"
                    value={MAIN_ROUTE}
                    icon={<HomeIcon/>}
                />
                <BottomNavigationAction
                    label="История"
                    value={HISTORY_ROUTE}
                    icon={<BarChartIcon/>}
                />
                <BottomNavigationAction
                    label="Профиль"
                    value="profile"
                    icon={<PersonIcon/>}
                />
            </BottomNavigation>
            <Drawer
                anchor={'bottom'}
                open={profileState}
                onClose={() => setProfileState(false)}
            >
                <Box
                    sx={{width: 'auto'}}
                >
                    <Stack spacing={2} sx={{width: "auto"}} alignItems={"center"} margin={3}>
                        <Avatar
                            sx={{bgcolor: deepOrange[500]}}
                            src="/broken-image.jpg"
                        >
                            {telegramData?.user?.firstName.slice(0, 1)}
                        </Avatar>

                        <Typography variant={"h4"}>
                            {telegramData?.user?.firstName}
                        </Typography>

                        <Typography variant={"h6"}>
                            {user?.email}
                        </Typography>

                        <Button
                            size="large"
                            variant="contained"
                            color="error"
                            sx={{mt: 4, mb: 2}}
                            onClick={() => {
                                setIsAuth(false)
                                setProfileState(false)
                            }}
                        >
                            Выйти
                        </Button>
                    </Stack>
                </Box>
            </Drawer>
        </Paper>
    )
})

export default BottomMenu;