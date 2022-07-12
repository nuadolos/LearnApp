import React, { useContext } from "react";
import { Link } from "react-router-dom";
import { AuthContext } from "../../../Context/AuthContext";

const Navbar = () => {
    const { isAuth, setIsAuth, setUserId } = useContext(AuthContext);

    const logout = async () => {
        await fetch('http://localhost:5243/api/account/logout', {
            method: 'POST',
            headers: { 'Content-Types': 'application/json' },
            credentials: 'include'
        });
        setIsAuth(false);
        setUserId('');
        localStorage.removeItem('auth');
        localStorage.removeItem('usid');
    };

    const authNav = () => {
        return (
            <ul className="nav justify-content-end">
                <li className="nav-item">
                    <Link className="nav-link" to="/note">Заметки</Link>
                </li>
                <li className="nav-item">
                    <a href='#' onClick={() => logout()} className="nav-link">Выход</a>
                </li>
            </ul>
        );
    };

    const unauthNav = () => {
        return (
            <ul className="nav justify-content-end">
                <li className="nav-item">
                    <Link className="nav-link" to="/login">Войти</Link>
                </li>
            </ul>
        );
    };

    return (
        <nav className="navbar navbar-expand-lg navbar-light bg-light">
            <div className="container-fluid">
                <Link className="navbar-brand" to="/home">Learn</Link>
                {isAuth
                    ?
                    authNav()
                    :
                    unauthNav()
                }
            </div>
        </nav>
    );
}

export default Navbar;