import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

import { ConfigApiResponse } from '../../types/config';
import { api } from '../../lib/https';
import { credentials } from '../../lib/credentials';
import { sessionStorage } from '../../lib/session-storage';

const Login = () => {
  const [error, setError] = useState('');
  const [dashboardId, setDashboardId] = useState('');
  const navigate = useNavigate();

  const onChangeInput = (e: any) => {
    setDashboardId(e.target.value);
  };

  const onLogin = async () => {
    setError('');
    const response = await api.get<ConfigApiResponse>(`/config?id=${dashboardId}`);

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
    <div>
      <h1>Login</h1>

      <label>
        Token
        <input onChange={onChangeInput} />
      </label>

      <button disabled={!dashboardId} onClick={onLogin}>
        Login
      </button>

      {error && <div>{error}</div>}
    </div>
  );
};

export default Login;