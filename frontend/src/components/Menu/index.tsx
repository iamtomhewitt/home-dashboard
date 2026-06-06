import { useState } from 'react';
import classNames from 'classnames';
import { Spin as Hamburger } from 'hamburger-react';
import { useLocation, useNavigate } from 'react-router-dom';

import MenuButton from './Button';
import Ok from '../Modal/Ok';
import { credentials } from '../../lib/credentials';
import { useModalStack } from '../ModalStack';

import './index.scss';

const Menu = () => {
  const [isFullScreen, setIsFullScreen] = useState(false);
  const [isOpen, setIsOpen] = useState(false);
  const location = useLocation();
  const modalstack = useModalStack();
  const navigate = useNavigate();

  const menuClasses = classNames({
    menu: true,
    'menu-closed': !isOpen,
    'menu-open': isOpen,
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
    isSelected: location.pathname === '/dashboard',
    label: 'Home',
    onClick: () => navigate('/dashboard'),
  }, {
    icon: 'utensils',
    isSelected: location.pathname === '/recipe-manager',
    label: 'Recipe Manager',
    onClick: () => navigate('/recipe-manager'),
  }, {
    icon: 'gear',
    isSelected: location.pathname === '/settings',
    label: 'Settings',
    onClick: () => navigate('/settings'),
  }];

  const actionButtons = [{
    icon: isFullScreen ? 'compress' : 'expand',
    isSelected: false,
    label: isFullScreen ? 'Exit Full Screen' : 'Full Screen',
    onClick: () => {
      if (document.fullscreenElement) {
        setIsFullScreen(false);
        document.exitFullscreen();
      }
      else {
        setIsFullScreen(true);
        setIsOpen(false);
        document.documentElement.requestFullscreen();
      }
    },
  }, {
    icon: 'refresh',
    isSelected: false,
    label: 'Refresh',
    onClick: () => window.location.reload(),
  }, {
    icon: 'file-pen',
    isSelected: false,
    label: 'Logs',
    onClick: () => modalstack.open(Ok, {
      message: JSON.parse(sessionStorage.getItem('logs') || '[]'),
      title: 'Logs',
    }),
  }, {
    icon: 'right-from-bracket',
    isSelected: false,
    label: 'Logout',
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
              b.onClick();
              setIsOpen(false);
            }}
          />
        ))}
      </div>

      <hr />

      <div className='menu-action-buttons'>
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

      <div className='menu-version'>
        {import.meta.env.PACKAGE_VERSION}
      </div>
    </div>
  );
};

export default Menu;