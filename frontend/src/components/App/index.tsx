import { useEffect } from 'react';
import { Navigate, Route, Routes, useLocation } from 'react-router-dom';

import Dashboard from '../Dashboard';
import Login from '../Login';
import Menu from '../Menu';
import { credentials } from '../../lib/credentials';

import './index.scss';

const App = () => {
  const location = useLocation();

  useEffect(() => {
    console.log('path changed');
  }, [location.pathname]);

  const withLoggedInRoute = (component: React.ReactElement) => {
    if (!credentials.isLoggedIn()) {
      return <Navigate to='/login' />;
    }

    return (
      <div>
        {component}

        <Menu />
      </div>
    );
  };

  return (
    <div>
      {/* <button onClick={() => document.documentElement.requestFullscreen()}>Full Screen</button> */}

      {/* <button onClick={() => document.exitFullscreen()}>Hide Full Screen</button> */}

      <Routes>
        <Route element={<Login />} path='/login' />

        <Route element={withLoggedInRoute(<Dashboard />)} path='/dashboard' />

        <Route element={<div>Not Found</div>} path='*' />

      </Routes>
    </div>
  );
};

export default App;
