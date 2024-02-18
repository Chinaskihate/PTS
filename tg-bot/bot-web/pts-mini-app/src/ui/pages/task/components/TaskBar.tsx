import React, {FC, RefObject, useState} from "react";
import {AppBar, Box, Button, Divider, Drawer, IconButton, Stack, Toolbar, Typography} from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';

interface TaskBarProps {
    appBarRef: RefObject<HTMLDivElement>,
    title: string,
    items: { name: string, id: number }[]
    onNavClick: (id: number) => void,
    onBackClick: () => void
}


const TaskBar: FC<TaskBarProps> = ({
                                       appBarRef,
                                       title,
                                       items,
                                       onNavClick,
                                       onBackClick
                                   }) => {

    const [searchState, setSearchState] = useState(false);

    return (
        <>
            <AppBar ref={appBarRef} sx={{backgroundColor: 'white', color: 'black'}}>
                <Toolbar sx={{justifyContent: "space-between"}}>
                    <Typography variant={"h6"} fontWeight={"bold"}>
                        {title}
                    </Typography>
                    <IconButton
                        color="inherit"
                        aria-label="open drawer"
                        edge="end"
                        onClick={() => setSearchState(true)}
                    >
                        <MenuIcon/>
                    </IconButton>

                </Toolbar>

            </AppBar>
            <Drawer
                anchor={'right'}
                open={searchState}
                onClose={() => setSearchState(false)}
            >
                <Box
                    sx={{width: 'auto'}}
                >
                    <Stack spacing={1} sx={{width: "auto", overflow: "auto"}} alignItems={"center"}
                           justifyContent={"flex-start"}
                           divider={<Divider flexItem/>}
                           margin={3}>
                        {
                            items.map(item =>
                                <Button
                                    sx={{justifyContent: "flex-start", fontWeight: "bold"}}
                                    fullWidth={true}
                                    color={"inherit"}
                                    onClick={() => onNavClick(item.id)}>
                                    {item.name}
                                </Button>
                            )
                        }

                        <Button
                            fullWidth
                            size={"large"}
                            sx={{justifyContent: "flex-start", fontWeight: "bold"}}
                            onClick={() => onBackClick()}
                            color={"error"}
                        >
                            К тесту
                        </Button>
                    </Stack>
                </Box>
            </Drawer>
        </>
    )
}

export default TaskBar