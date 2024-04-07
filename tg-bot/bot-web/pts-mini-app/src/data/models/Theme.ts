export interface Theme {
    id: number,
    name: string,
    isBanned: boolean,
    subThemes: Theme[]
}