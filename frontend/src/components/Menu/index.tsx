import { useState } from 'react';
import classNames from 'classnames';
import { Spin as Hamburger } from 'hamburger-react';
import { useNavigate } from 'react-router-dom';

import MenuButton from './Button';
import { credentials } from '../../lib/credentials';

import './index.scss';

const Menu = () => {
  const [isFullScreen, setIsFullScreen] = useState(false);
  const [isOpen, setIsOpen] = useState(true);

  const menuClasses = classNames({
    menu: true,
    'menu-open': isOpen,
    'menu-closed': !isOpen,
  });

  const toggleButtonClasses = classNames({
    'menu-toggle-button': true,
    'menu-toggle-button-open': isOpen,
  });

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

  const onToggleOpen = () => {
    setIsOpen(!isOpen);
  };

  const navigationButtons = [{
    icon: 'house',
    label: 'Home',
    isSelected: true,
    onClick: () => navigate('/dashboard'),
  }, {
    icon: 'utensils',
    label: 'Recipe Manager',
    isSelected: false,
    onClick: () => navigate('/recipe-manager'),
  }, {
    icon: 'gear',
    label: 'Settings',
    isSelected: false,
    onClick: () => navigate('/settings'),
  }];

  const actionButtons = [{
    icon: isFullScreen ? 'compress' : 'expand',
    label: 'Full Screen',
    isSelected: false,
    onClick: onToggleFullScreen,
  }, {
    icon: 'refresh',
    label: 'Refresh',
    isSelected: false,
    onClick: () => window.location.reload(),
  }, {
    icon: 'right-from-bracket',
    label: 'Logout',
    isSelected: false,
    onClick: () => {
      credentials.logout();
      navigate('/login');
    },
  }];

  return (
    <div className={menuClasses}>
      <button className={toggleButtonClasses} onClick={onToggleOpen}>
        <Hamburger size={12} toggled={isOpen} />
      </button>

      <div>
        {navigationButtons.map((b, i) => (
          <MenuButton
            icon={b.icon}
            isSelected={b.isSelected}
            key={i}
            label={b.label}
            onClick={b.onClick}
          />
        ))}
      </div>

      <hr />

      <div>
        {actionButtons.map((b, i) => (
          <MenuButton
            icon={b.icon}
            isSelected={b.isSelected}
            key={i}
            label={b.label}
            onClick={b.onClick}
          />
        ))}
      </div>
    </div>
  );
};

export default Menu;