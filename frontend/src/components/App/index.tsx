import { useEffect } from 'react';
import { Route, Routes, useLocation } from 'react-router-dom';

import Login from '../Login';

import './index.scss';

const App = () => {
  const location = useLocation();

  useEffect(() => {
    console.log('path changed');
  }, [location.pathname]);

  return (
    <div>
      <button onClick={() => document.documentElement.requestFullscreen()}>Full Screen</button>

      <button onClick={() => document.exitFullscreen()}>Hide Full Screen</button>

      <Routes>
        <Route element={<Login />} path='/login' />

        <Route element={<div>Dashboard</div>} path='/dashboard' />

        <Route element={<div>Not Found</div>} path='*' />

      </Routes>
    </div>
  );
};

export default App;
