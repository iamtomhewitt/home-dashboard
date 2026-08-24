import { useEffect, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';

import { ConfigApiResponse } from '../../types/config';
import { api } from '../../lib/api';
import { credentials } from '../../lib/credentials';
import { dashboard } from '../../lib/dashboard';

import './index.scss';

const Login = () => {
  const [error, setError] = useState('');
  const [dashboardId, setDashboardId] = useState('');
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();

  useEffect(() => {
    const id = searchParams.get('id');
    if (id) {
      setDashboardId(id);
    }
  }, []);

  const onChangeInput = (e: any) => {
    setDashboardId(e.target.value);
  };

  const onLogin = async () => {
    try {
      setError('');
      const response = await api.get<ConfigApiResponse>(`/config?id=${dashboardId}`);
      credentials.login(true);
      dashboard.setConfig({
        ...response.data,
        id: dashboardId,
      });
      navigate('/dashboard');
    }
    catch (err: any) {
      setError(`${err.detail?.message || err.message}`);
    }
  };

  return (
    <div className='login'>
      <div className='login-box'>
        <h1>Home Dashboard</h1>

        <input
          onChange={onChangeInput}
          placeholder='Dashboard ID'
          value={dashboardId}
        />

        <button disabled={!dashboardId} onClick={onLogin}>
          Login
        </button>

        {error && <div className='login-error'>{error}</div>}

        <div className='login-version'>
          Version {import.meta.env.PACKAGE_VERSION}
        </div>
      </div>
    </div>
  );
};

export default Login;