import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

import { ConfigApiResponse } from '../../types/config';
import { credentials } from '../../lib/credentials';
import { http } from '../../lib/https';
import { sessionStorage } from '../../lib/session-storage';

import './index.scss';

const Login = () => {
  const [error, setError] = useState('');
  const [dashboardId, setDashboardId] = useState('');
  const navigate = useNavigate();

  const onChangeInput = (e: any) => {
    setDashboardId(e.target.value);
  };

  const onLogin = async () => {
    setError('');
    const response = await http.get<ConfigApiResponse>(`/config?id=${dashboardId}`);

    switch (response.status) {
      case 200:
        credentials.login(true);
        sessionStorage.setDashboardConfig({
          ...response.data,
          id: dashboardId,
        });
        navigate('/dashboard');
        break;
      default:
        setError(response.message);
        break;
    }
  };

  return (
    <div className='login'>
      <div className='login-box'>
        <h1>Home Dashboard</h1>

        <input onChange={onChangeInput} placeholder='Dashboard ID' />

        <button disabled={!dashboardId} onClick={onLogin}>
          Login
        </button>

        {error && <div>{error}</div>}
      </div>
    </div>
  );
};

export default Login;