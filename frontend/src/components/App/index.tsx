import { Route, Routes } from 'react-router-dom';

import './index.scss';

const App = () => {
  return (
    <div>
      <Routes>
        <Route element={<div>Login</div>} path='login' />

        <Route element={<div>Dashboard</div>} path='dashboard' />

        <Route element={<div>Not Found</div>} path='*' />

      </Routes>
    </div>
  );
};

export default App;
