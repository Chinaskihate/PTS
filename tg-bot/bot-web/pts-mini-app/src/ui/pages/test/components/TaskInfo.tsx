import React, {FC} from "react";
import {Chip, Container, Divider, Stack, Typography} from "@mui/material";
import {getDifficultColor, getProgressColor, getRandomColor} from "../../../utils/colorsPicker";
import AccessTimeFilledIcon from "@mui/icons-material/AccessTimeFilled";
import TouchRipple from "@mui/material/ButtonBase/TouchRipple";
import ButtonBase from "@mui/material/ButtonBase";
import {Difficult} from "../../../../data/models/Test";
import {Progress} from "../../../../data/models/TestResult";

interface TaskProps {
    name: string
    description: string
    time: number
    difficult: string
    language: string
    progress: string
    themes: string[]
    onClick: () => void
}

const TaskInfo: FC<TaskProps> = ({name, description, language, progress, themes, time, difficult, onClick}) => {
    return (
        <ButtonBase sx={{width: "100%"}}>
            <Container sx={{borderRadius: "5px", boxShadow: 1, paddingY: 1}} onClick={onClick}>
                <Stack spacing={1}>
                    <Stack direction={"row"} spacing={1}
                           sx={{alignItems: "center", overflow: "auto", scrollbarWidth: "none"}}>
                        <Typography variant={"body1"} fontWeight={"bold"}>{name.slice(0, 25)}</Typography>
                        <Chip sx={{backgroundColor: getDifficultColor(difficult as Difficult), color: "white", minWidth: "60px"}}
                              label={difficult} size={"small"}/>
                    </Stack>

                    <Divider/>

                    <Stack direction={"row"} spacing={1} sx={{alignItems: "center"}}>
                        <Typography align={"left"} variant={"body2"} sx={{
                            display: '-webkit-box',
                            overflow: 'hidden',
                            WebkitBoxOrient: 'vertical',
                            WebkitLineClamp: 2
                        }}>{description}</Typography>
                    </Stack>

                    <Divider/>

                    <Stack direction={"row"} spacing={1}
                           sx={{alignItems: "center", overflow: "auto", scrollbarWidth: "none"}}>
                        {
                            language !== undefined && (
                                <Chip sx={{minWidth: "60px", backgroundColor: "blue", color: "white"}}
                                      label={language} size={"small"} variant={"outlined"}
                                />
                            )
                        }


                        {
                            themes.map((it, index) => (
                                <Chip
                                    sx={{
                                        minWidth: "60px",
                                        borderColor: getRandomColor(index),
                                        color: getRandomColor(index)
                                    }}
                                    label={it} size={"small"}
                                    variant={"outlined"}
                                />
                            ))
                        }
                    </Stack>


                    <Divider/>

                    <Stack direction={"row"} spacing={1} sx={{alignItems: "center", justifyContent: "space-between"}}>
                        <Stack direction={"row"} spacing={1} sx={{alignItems: "center"}}>

                            <AccessTimeFilledIcon/>
                            <Typography align={"left"} variant={"body2"} sx={{
                                display: '-webkit-box',
                                overflow: 'hidden',
                                WebkitBoxOrient: 'vertical',
                                WebkitLineClamp: 2
                            }}>{time} мин</Typography>
                        </Stack>

                        <Typography align={"right"} variant={"body2"} sx={{
                            display: '-webkit-box',
                            overflow: 'hidden',
                            WebkitBoxOrient: 'vertical',
                            WebkitLineClamp: 2,
                            color: getProgressColor(progress as Progress)
                        }}>{progress}</Typography>
                    </Stack>

                </Stack>
                <TouchRipple center/>
            </Container>
        </ButtonBase>
    )
}

export default TaskInfo;
