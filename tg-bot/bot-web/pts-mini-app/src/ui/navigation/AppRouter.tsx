import {observer} from "mobx-react-lite";
import {Navigate, Route, Routes, useNavigate} from "react-router-dom";
import {routes} from "./routes";
import {AUTH_ROUTE, MAIN_ROUTE} from "./navigationConstants";
import {useContext, useEffect} from "react";
import {AppContext} from "../providers/AppProvider";

const AppRouter = observer(() => {
    const {isAuth} = useContext(AppContext)
    const navigate = useNavigate()

    useEffect(() => {
        if (!isAuth) navigate(AUTH_ROUTE)
    }, [isAuth])


    const components = routes.map(
        ({path, Component}) =>
            <Route path={path} key={path} element={<Component/>}/>
    )
    return (

        <Routes>
            {components}
            <Route path="*" element={
                <Navigate to={MAIN_ROUTE} replace={true}/>
            }/>
        </Routes>
    )
});

export default AppRouter;