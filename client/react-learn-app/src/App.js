import { useEffect, useState } from 'react';
import { BrowserRouter } from 'react-router-dom';
import { AuthContext } from './Context/AuthContext';
import Navbar from './Components/UI/Navbar/Navbar';
import './App.css';
import AppRouter from './Components/AppRouter';

function App() {
  const [isAuth, setIsAuth] = useState(false);
  const [userId, setUserId] = useState('');
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    if (localStorage.getItem('auth')) {
      setIsAuth(true);
      setUserId(localStorage.getItem('usid'));
    }
    setIsLoading(false);
  }, [])

  return (
    <AuthContext.Provider value={{ isAuth, setIsAuth, userId, setUserId, isLoading }}>
      <BrowserRouter>
        <Navbar />
        <AppRouter />
      </BrowserRouter>
    </AuthContext.Provider>
  );
}

export default App;
