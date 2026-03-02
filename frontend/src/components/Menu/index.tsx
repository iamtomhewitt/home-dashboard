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

  const buttons = [{
    icon: 'house',
    onClick: () => navigate('/dashboard'),
  }, {
    icon: 'refresh',
    onClick: () => window.location.reload(),
  }, {
    icon: 'utensils',
    onClick: () => navigate('/recipe-manager'),
  }, {
    icon: 'gear',
    onClick: () => navigate('/settings'),
  }, {
    icon: isFullScreen ? 'compress' : 'expand',
    onClick: onToggleFullScreen,
  }, {
    icon: 'right-from-bracket',
    onClick: () => {
      credentials.logout();
      navigate('/login');
    },
  }];

  return (
    <div className='menu'>
      {buttons.map((b, i) => (
        <button key={i} onClick={b.onClick}>
          <i className={`fa-solid fa-${b.icon}`} />
        </button>
      ))}
    </div>
  );
};

export default Menu;