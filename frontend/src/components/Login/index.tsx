import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

import { configApi } from '../../api/config';

const Login = () => {
  const [token, setToken] = useState('');
  const navigate = useNavigate();

  const onChangeInput = (e: any) => {
    setToken(e.target.value);
  };

  const onLogin = async () => {
    const { data, status } = await configApi.get(token);
    console.log(status, data);
  };

  return (
    <div>
      <h1>Login</h1>

      <label>
        Token
        <input onChange={onChangeInput} />
      </label>

      <button disabled={!token} onClick={onLogin}>
        Login
      </button>
    </div>
  );
};

export default Login;