import React, {FC, RefObject, useState} from "react";
import {AppBar, Autocomplete, Box, Chip, Drawer, IconButton, Stack, TextField, Toolbar} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import {Search, SearchIconWrapper, StyledInputBase} from "./CustomSearch";
import {getDifficultColor, getRandomColor} from "../../../utils/colorsPicker";
import {Difficult} from "../../../../data/models/Test";

interface SearchProps {
    appBarRef: RefObject<HTMLDivElement>,
    onSearchTitle: (value: string) => void
    onSearchThemes: (value: string[]) => void
    onSearchDifficulties: (value: string[]) => void
    onSearchLanguages: (value: string[]) => void
    themes: string[]
    selectedThemes: string[]
    difficulties: string[]
    selectedDifficulties: string[]
    currentSearch: string,
    languages: string[],
    selectedLanguages: string[]
}


const SearchBar: FC<SearchProps> = ({
                                        appBarRef,
                                        onSearchTitle,
                                        onSearchThemes,
                                        onSearchDifficulties,
                                        onSearchLanguages,
                                        themes,
                                        difficulties,
                                        selectedThemes,
                                        selectedDifficulties,
                                        currentSearch,
                                        languages,
                                        selectedLanguages
                                    }) => {
    const [searchTerm, setSearchTerm] = useState(currentSearch)

    const [searchState, setSearchState] = useState(false);

    return (
        <>
            <AppBar ref={appBarRef} sx={{backgroundColor: 'white', color: 'black'}}>
                <Toolbar sx={{justifyContent: "flex-end"}}>
                    <IconButton
                        color="inherit"
                        aria-label="open drawer"
                        edge="end"
                        onClick={() => setSearchState(true)}
                    >
                        <SearchIcon/>
                    </IconButton>

                </Toolbar>

            </AppBar>
            <Drawer
                anchor={'bottom'}
                open={searchState}
                onClose={() => setSearchState(false)}
            >
                <Box
                    sx={{width: 'auto'}}
                >
                    <Stack spacing={2} sx={{width: "auto", maxHeight: "25%", overflow: "auto"}} alignItems={"center"}
                           margin={3}>
                        <Search>
                            <SearchIconWrapper>
                                <SearchIcon/>
                            </SearchIconWrapper>
                            <StyledInputBase
                                placeholder="Поиск…"
                                inputProps={{'aria-label': 'search'}}
                                value={searchTerm}
                                onChange={(e) => {
                                    const value = e.target.value
                                    setSearchTerm(value)
                                    onSearchTitle(value)
                                }
                                }
                            />
                        </Search>


                        <Autocomplete
                            multiple
                            limitTags={4}
                            size="small"
                            id="themes"
                            options={themes}
                            defaultValue={selectedThemes}
                            getOptionLabel={it => it}
                            renderInput={(params) => (
                                <TextField {...params} placeholder="Темы"/>
                            )}
                            renderTags={(tagValue, getTagProps) =>
                                tagValue.map((option, index) => (
                                    <Chip
                                        label={option}
                                        {...getTagProps({index})}
                                        variant="outlined"
                                        sx={{
                                            backgroundColor: getRandomColor(index),
                                            color: "white",
                                            '& .MuiChip-deleteIcon': {
                                                color: 'white',
                                            },
                                        }}
                                    />
                                ))
                            }
                            onChange={(_, value) => onSearchThemes(value)}
                            sx={{
                                width: '100%',
                            }}
                        />

                        <Autocomplete
                            multiple
                            limitTags={3}
                            size="small"
                            id="difficultyLevels"
                            options={difficulties}
                            getOptionLabel={it => it}
                            renderInput={(params) => (
                                <TextField {...params} placeholder="Сложность"/>
                            )}
                            defaultValue={selectedDifficulties}
                            renderTags={(tagValue, getTagProps) =>
                                tagValue.map((option, index) => (
                                    <Chip
                                        label={option}
                                        {...getTagProps({index})}
                                        variant="outlined"
                                        sx={{
                                            backgroundColor: getDifficultColor(option as Difficult),
                                            color: "white",
                                            '& .MuiChip-deleteIcon': {
                                                color: 'white',
                                            },
                                        }}
                                    />
                                ))
                            }
                            onChange={(_, value) =>
                                onSearchDifficulties(value)}
                            sx={{
                                width: '100%',
                            }}
                        />

                        <Autocomplete
                            multiple
                            limitTags={3}
                            size="small"
                            id="languages"
                            options={languages}
                            getOptionLabel={it => it}
                            renderInput={(params) => (
                                <TextField {...params} placeholder="Язык"/>
                            )}
                            defaultValue={selectedLanguages}
                            renderTags={(tagValue, getTagProps) =>
                                tagValue.map((option, index) => (
                                    <Chip
                                        label={option}
                                        {...getTagProps({index})}
                                        variant="outlined"
                                        sx={{
                                            backgroundColor: getRandomColor(index),
                                            color: "white",
                                            '& .MuiChip-deleteIcon': {
                                                color: 'white',
                                            },
                                        }}
                                    />
                                ))
                            }
                            onChange={(_, value) =>
                                onSearchLanguages(value)}
                            sx={{
                                width: '100%',
                            }}
                        />
                    </Stack>
                </Box>
            </Drawer>
        </>
    )
}

export default SearchBar