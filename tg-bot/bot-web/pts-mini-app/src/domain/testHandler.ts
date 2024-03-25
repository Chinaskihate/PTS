import {getTestApi, getTestsApi, getTestsByIdsApi} from "../data/api/testApi";
import {Difficult, Language} from "../data/models/Test";
import {Theme} from "../data/models/Theme";

export const getTests = async (searchTitle: string, themes: Theme[], difficulties: Difficult[], languages: Language[], taskCount: number) => {
    return getTestsApi(searchTitle, themes, difficulties, languages, taskCount)
}

export const getTest = async (id: number) => {
    return await getTestApi(id)
}

export const getTestsByIds = async (ids: number[]) => {
    return getTestsByIdsApi(ids)
}