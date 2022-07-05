import React from "react";

const Home = () => {
    const getUsers = async () => {
        const responce = await fetch('http://localhost:5243/api/account', {
            method: 'GET',
            headers: {'Content-Types': 'application/json'},
            credentials: 'include'
        });

        const content = await responce.json();

        console.log(content);
    }

    return(
        <div>
            <button className="btn btn-primary" onClick={getUsers}>Получить пользователей</button>
        </div>
    );
}

export default Home;