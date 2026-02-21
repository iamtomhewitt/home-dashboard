import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

import { configApi } from '../../api/config';
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
    const response = await configApi.get(dashboardId);

    switch (response.status) {
      case 200:
        credentials.login(true);
        sessionStorage.setDashboardConfig(response.data);
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