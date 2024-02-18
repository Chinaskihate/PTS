import {createContext, Dispatch, ReactNode, SetStateAction, useState} from "react";

type AppContextProviderProps = {
    children: ReactNode
}


export interface User {
    email: string
    telegramId: number
}

interface AppContextType {
    user: User
    setUser: Dispatch<SetStateAction<User>>
    isAuth: boolean
    setIsAuth:  Dispatch<SetStateAction<boolean>>
    screen: string
    setScreen: Dispatch<SetStateAction<string>>
}

export const AppContext = createContext({} as AppContextType)


export const AppContextProvider = ({children}: AppContextProviderProps) => {
    const [user, setUser] = useState<User>({} as User)
    const [isAuth, setIsAuth] = useState<boolean>(false)
    const [screen, setScreen] = useState<string>("")

    const value = {
        user,
        setUser,
        isAuth,
        setIsAuth,
        screen,
        setScreen
    }

    return <AppContext.Provider value={value}>{children}</AppContext.Provider>
}
