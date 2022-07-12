import React, { useState } from "react";
import { Link, Navigate } from "react-router-dom";

const Register = () => {
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const [repeat, setRepeat] = useState('');
    const [surname, setSurname] = useState('');
    const [name, setName] = useState('');
    const [redirect, setRedirect] = useState(false);

    const register = async (event) => {
        event.preventDefault();

        if (password === repeat) {
            await fetch('http://localhost:5243/api/account/register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    surname,
                    name,
                    login,
                    password
                })
            });

            setRedirect(true);
        }
    };

    if (redirect)
        return (<Navigate to='/login' />);

    return (
        <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', margin: '10px' }}>
            <h1>Регистрация</h1>

            <form onSubmit={register}>
                <div className="form-outline mb-4">
                    <input type="text" className="form-control" placeholder="Фамилия"
                        value={surname} onChange={event => setSurname(event.target.value)} />
                </div>

                <div className="form-outline mb-4">
                    <input type="text" className="form-control" placeholder="Имя"
                        value={name} onChange={event => setName(event.target.value)} />
                </div>

                <div className="form-outline mb-4">
                    <input type="email" className="form-control" placeholder="Эл. почта"
                        value={login} onChange={event => setLogin(event.target.value)} />
                </div>

                <div className="form-outline mb-4">
                    <input type="password" className="form-control" placeholder="Пароль"
                        value={password} onChange={event => setPassword(event.target.value)} />
                </div>

                <div className="form-outline mb-4">
                    <input type="password" className="form-control" placeholder="Подтверждение пароля"
                        value={repeat} onChange={event => setRepeat(event.target.value)} />
                </div>

                <button type="submit" class="btn btn-primary btn-block mb-4">Создать учетную запись</button>

                <div className="text-center">
                    <p>Уже зарегистрированы? <Link to='/login'>Вход в систему</Link></p>
                </div>
            </form>
        </div>
    );
}

export default Register;