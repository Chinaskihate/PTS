import {getTestApi, getTestsApi, getTestsByIdsApi} from "../data/api/testApi";
import {Difficult, Language} from "../data/models/Test";

export const getTests = async (searchTitle: string, categories: string[], difficulties: Difficult[], languages: Language[]) => {
    return getTestsApi(searchTitle, categories, difficulties, languages)
}

export const getTest = async (id: number) => {
    const [test] = (await getTestApi(id)).tests
    return {test: test}
}

export const getTestsByIds = async (ids: number[]) => {
    return getTestsByIdsApi(ids)
}