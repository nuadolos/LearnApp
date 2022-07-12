import React, { useState, useContext } from "react";
import { Link, Navigate } from "react-router-dom";
import { AuthContext } from "../Context/AuthContext";

const Login = () => {
    const {setIsAuth, setUserId} = useContext(AuthContext)

    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const [redirect, setRedirect] = useState(false);

    const authorizate = async (event) => {
        event.preventDefault();

        const responce = await fetch('http://localhost:5243/api/account/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include',
            body: JSON.stringify({
                login,
                password
            })
        });

        const content = await responce.json();

        setIsAuth(true);
        setUserId(content.id);
        localStorage.setItem('auth', 'true');
        localStorage.setItem('usid', `${content.id}`);
        setRedirect(true);
    };

    if (redirect)
        return(<Navigate to='/home'/>);

    return (
        <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', margin: '10px' }}>
            <h1>Авторизация</h1>

            <form onSubmit={authorizate}>
                <div className="form-outline mb-4">
                    <input type="email" className="form-control" placeholder="Эл. почта"
                        value={login} onChange={event => setLogin(event.target.value)} />
                </div>

                <div className="form-outline mb-4">
                    <input type="password" className="form-control" placeholder="Пароль"
                        value={password} onChange={event => setPassword(event.target.value)} />
                </div>

                <div className="row mb-4">
                    <div className="col">
                        <a href='#'>Забыли пароль?</a>
                    </div>
                </div>

                <button type="submit" class="btn btn-primary btn-block mb-4">Войти</button>

                <div className="text-center">
                    <p>Вы еще не зарегистрированы? <Link to='/register'>Зарегистрироваться</Link></p>
                </div>
            </form>
        </div>
    );
}

export default Login;