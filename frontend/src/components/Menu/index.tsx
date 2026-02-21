import { useNavigate } from 'react-router-dom';

import { credentials } from '../../lib/credentials';

import './index.scss';

// TODO in future this could be wrapped in a Hamburger menu
const Menu = () => {
  const navigate = useNavigate();

  const onLogout = () => {
    credentials.logout();
    navigate('/login');
  };

  return (
    <div className='menu'>
      <button onClick={onLogout}>
        Logout
      </button>
    </div>
  );
};

export default Menu;