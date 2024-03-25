import {$themeApi} from "./index";
import {Theme} from "../models/Theme";

// const themes = ["cat1", "cat2", "cat3", "cat4", "cat5", "cat6", "cat7", "cat8", "cat9", "cat10", "cat11", "cat12", "cat13"]

export const getThemesApi = async () => {
    const {data} = await $themeApi.get(process.env.REACT_APP_THEME_AVAILABLE as string)
    const {result} = data
    const theme = result as Theme

    return {themes: getThemes(theme)}
}

function getThemes(theme: Theme) {
    let themes = [theme]
    for (const subTheme of theme.subThemes) {
        themes = Array.from(new Set([...getThemes(subTheme) ,...themes]))
    }

    return themes
}

