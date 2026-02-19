import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const Login = () => {
  const [token, setToken] = useState('');
  const navigate = useNavigate();

  const onChangeInput = (e: any) => {
    setToken(e.target.value);
  };

  const onLogin = () => {
    navigate('/dashboard');
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