import { Navigate, Route, Routes } from 'react-router-dom';

import Dashboard from '../Dashboard';
import Login from '../Login';
import Menu from '../Menu';
import { credentials } from '../../lib/credentials';

import './index.scss';

const App = () => {
  const isLoggedIn = credentials.isLoggedIn();

  const withLoggedInRoute = (component: React.ReactElement) => {
    if (!isLoggedIn) {
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
      <Routes>
        <Route element={<Login />} path='/login' />

        <Route element={withLoggedInRoute(<Dashboard />)} path='/dashboard' />

        <Route element={<div>Not Found</div>} path='*' />

      </Routes>
    </div>
  );
};

export default App;
