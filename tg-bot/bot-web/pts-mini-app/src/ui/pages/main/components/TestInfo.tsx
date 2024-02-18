import React, {FC} from "react";
import {Chip, Container, Divider, Stack, Typography} from "@mui/material";
import AccessTimeFilledIcon from '@mui/icons-material/AccessTimeFilled';
import {getDifficultColor, getRandomColor} from "../../../utils/colorsPicker";
import TouchRipple from '@mui/material/ButtonBase/TouchRipple';
import ButtonBase from '@mui/material/ButtonBase';
import {Difficult} from "../../../../data/models/Test";

interface TestProps {
    name: string,
    description: string,
    languages: string[],
    themes: string[],
    time: number,
    createdAt: Date,
    difficulties: string[]
    onClick: () => void
}


const TestInfo: FC<TestProps> = ({name, description, languages, themes, time, createdAt, difficulties, onClick}) => {

    return (
        <ButtonBase>
            <Container sx={{borderRadius: "5px", boxShadow: 1, paddingY: 1}} onClick={onClick}>
                <Stack spacing={1}>
                    <Stack direction={"row"} spacing={1} sx={{alignItems: "center", overflow: "auto", scrollbarWidth: "none"}}>
                        <Typography variant={"body1"} fontWeight={"bold"} noWrap>{name.slice(0, 25)}</Typography>
                        {
                            difficulties.map((it, _) => (
                                <Chip sx={{backgroundColor: getDifficultColor(it as Difficult), color: "white", minWidth: "60px"}}
                                      label={it} size={"small"}/>
                            ))
                        }
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
                            languages.map((it, index) => (
                                <Chip sx={{
                                    minWidth: "60px", backgroundColor: getRandomColor(index), color: "white"
                                }}
                                      label={it} size={"small"} variant={"outlined"}
                                />
                            ))
                        }
                    </Stack>

                    <Divider/>

                    <Stack direction={"row"} spacing={1}
                           sx={{alignItems: "center", overflow: "auto", scrollbarWidth: "none"}}>
                        {
                            themes.map((it, index) => (
                                <Chip sx={{
                                    minWidth: "60px", borderColor: getRandomColor(index), color: getRandomColor(index)
                                }}
                                      label={it} size={"small"} variant={"outlined"}
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
                            color: "gray"
                        }}>{createdAt.toLocaleDateString()}</Typography>
                    </Stack>

                </Stack>
                <TouchRipple center/>
            </Container>
        </ButtonBase>
    );
};

export default TestInfo;