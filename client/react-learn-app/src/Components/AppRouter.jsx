import React, { useContext } from "react";
import { Navigate, useRoutes } from "react-router-dom";
import { AuthContext } from "../Context/AuthContext";
import Loader from '../Components/UI/Loader/Loader';
import Home from '../Pages/Home';
import Login from '../Pages/Login';
import Register from '../Pages/Register';
import Error from '../Pages/Error';
import Note from '../Pages/Note';

const AppRouter = () => {
    const { isAuth, isLoading } = useContext(AuthContext);

    const publicRoute = useRoutes([
        { path: '/home', element: <Home /> },
        { path: '/login', element: <Login /> },
        { path: '/register', element: <Register /> },
        { path: '/error', element: <Error /> },
        { path: '/*', element: <Navigate to='/home' /> }
    ]);

    const privateRoute = useRoutes([
        { path: '/home', element: <Home /> },
        { path: '/note', element: <Note /> },
        { path: '/error', element: <Error /> },
        { path: '/*', element: <Navigate to='/home' /> }
    ]);

    if (isLoading) {
        return (
            <Loader/>
        );
    };
    console.log(isAuth);
    return (
        isAuth ? privateRoute : publicRoute
    );
}

export default AppRouter;