import { useState } from 'react';
import classNames from 'classnames';
import { Spin as Hamburger } from 'hamburger-react';
import { useLocation, useNavigate } from 'react-router-dom';

import MenuButton from './Button';
import { credentials } from '../../lib/credentials';

import './index.scss';

const Menu = () => {
  const [isFullScreen, setIsFullScreen] = useState(false);
  const [isOpen, setIsOpen] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();

  const menuClasses = classNames({
    menu: true,
    'menu-open': isOpen,
    'menu-closed': !isOpen,
  });

  const toggleButtonClasses = classNames({
    'menu-toggle-button': true,
    'menu-toggle-button-open': isOpen,
  });

  const onToggleOpen = () => {
    setIsOpen(!isOpen);
  };

  const navigationButtons = [{
    icon: 'house',
    label: 'Home',
    isSelected: location.pathname === '/dashboard',
    onClick: () => navigate('/dashboard'),
  }, {
    icon: 'utensils',
    label: 'Recipe Manager',
    isSelected: location.pathname === '/recipe-manager',
    onClick: () => navigate('/recipe-manager'),
  }, {
    icon: 'gear',
    label: 'Settings',
    isSelected: location.pathname === '/settings',
    onClick: () => navigate('/settings'),
  }];

  const actionButtons = [{
    icon: isFullScreen ? 'compress' : 'expand',
    label: isFullScreen ? 'Exit Full Screen' : 'Full Screen',
    isSelected: false,
    onClick: () => {
      if (document.fullscreenElement) {
        setIsFullScreen(false);
        document.exitFullscreen();
      }
      else {
        setIsFullScreen(true);
        document.documentElement.requestFullscreen();
      }
    },
  }, {
    icon: 'refresh',
    label: 'Refresh',
    isSelected: false,
    onClick: () => window.location.reload(),
  }, {
    icon: 'floppy-disk',
    label: 'Save',
    isSelected: false,
    onClick: () => alert('todo'),
  }, {
    icon: 'right-from-bracket',
    label: 'Logout',
    isSelected: false,
    onClick: () => {
      credentials.logout();
      setIsOpen(false);
      navigate('/login');
      window.location.reload();
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
            onClick={() => {
              b.onClick()
              setIsOpen(false)
            }}
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