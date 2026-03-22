import { useEffect } from 'react';
import { Navigate, Route, Routes, useLocation } from 'react-router-dom';

import Dashboard from '../Dashboard';
import IconsPage from '../IconsPage';
import Login from '../Login';
import Menu from '../Menu';
import ModalStack from '../ModalStack';
import RecipeManager from '../RecipeManager';
import Settings from '../Settings';
import { credentials } from '../../lib/credentials';

import './index.scss';

const App = () => {
  const location = useLocation();

  useEffect(() => {
    // Do nothing at the minute, just to rerender the app to allow the navigate to take place
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
    <ModalStack>
      <Routes>
        <Route element={<Login />} path='/login' />

        <Route element={withLoggedInRoute(<Dashboard />)} path='/dashboard' />

        <Route element={withLoggedInRoute(<IconsPage />)} path='/icons' />

        <Route element={withLoggedInRoute(<Settings />)} path='/settings' />

        <Route element={withLoggedInRoute(<RecipeManager />)} path='/recipe-manager' />

        <Route element={withLoggedInRoute(<div>Not Found</div>)} path='*' />

      </Routes>
    </ModalStack>
  );
};

export default App;
