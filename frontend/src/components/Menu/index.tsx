import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

import { credentials } from '../../lib/credentials';

import './index.scss';

// TODO in future this could be wrapped in a Hamburger menu
const Menu = () => {
  const [isFullScreen, setIsFullScreen] = useState(false);
  const navigate = useNavigate();

  const onToggleFullScreen = () => {
    if (document.fullscreenElement) {
      setIsFullScreen(false);
      document.exitFullscreen();
    }
    else {
      setIsFullScreen(true);
      document.documentElement.requestFullscreen();
    }
  };

  const onLogout = () => {
    credentials.logout();
    navigate('/login');
  };

  return (
    <div className='menu'>
      <button onClick={onToggleFullScreen}>
        {isFullScreen ? 'Hide' : 'Show'} Full Screen
      </button>

      <button onClick={onLogout}>
        Logout
      </button>
    </div>
  );
};

export default Menu;