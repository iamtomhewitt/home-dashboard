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

  const onRefresh = () => {
    window.location.reload();
  };

  const onSettings = () => {
    navigate('/settings');
  };

  const onHome = () => {
    navigate('/dashboard');
  };

  return (
    <div className='menu'>
      <button onClick={onToggleFullScreen}>
        <i className={`fa-solid fa-${isFullScreen ? 'compress' : 'expand'}`} />
      </button>

      <button onClick={onRefresh}>
        <i className='fa-solid fa-refresh' />
      </button>

      <button onClick={onSettings}>
        <i className='fa-solid fa-gear' />
      </button>

      <button onClick={onHome}>
        <i className='fa-solid fa-house' />
      </button>

      <button onClick={onLogout}>
        <i className='fa-solid fa-arrow-right-from-bracket' />
      </button>

    </div>
  );
};

export default Menu;