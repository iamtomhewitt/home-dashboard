import { useState } from 'react';
import classNames from 'classnames';
import { Spin as Hamburger } from 'hamburger-react';
import { useNavigate } from 'react-router-dom';

import Icon from '../Icon';
import { credentials } from '../../lib/credentials';

import './index.scss';

const Menu = () => {
  const [isFullScreen, setIsFullScreen] = useState(false);
  const [isOpen, setIsOpen] = useState(false);
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

  const menuClasses = classNames({
    menu: true,
    'menu-open': isOpen,
    'menu-closed': !isOpen,
  });

  const navigationButtons = [{
    icon: 'house',
    label: 'Home',
    isSelected: true,
    onClick: () => navigate('/dashboard'),
  }, {
    icon: 'utensils',
    isSelected: false,
    label: 'Recipe Manager',
    onClick: () => navigate('/recipe-manager'),
  }, {
    icon: 'gear',
    isSelected: false,
    label: 'Settings',
    onClick: () => navigate('/settings'),
  }];

  const actionButtons = [{
    icon: isFullScreen ? 'compress' : 'expand',
    isSelected: false,
    label: 'Full Screen',
    onClick: onToggleFullScreen,
  }, {
    icon: 'refresh',
    label: 'Refresh',
    isSelected: false,
    onClick: () => window.location.reload(),
  }, {
    icon: 'right-from-bracket',
    isSelected: false,
    label: 'Logout',
    onClick: () => {
      credentials.logout();
      navigate('/login');
    },
  }];

  return (
    <div>
      <button className='menu-toggle-button' onClick={onToggleOpen}>
        <Hamburger toggled={isOpen} />
      </button>

      <div className={menuClasses}>
        <div>
          {navigationButtons.map((b, i) => {
            const classes = classNames({
              'menu-item': true,
              'menu-item-selected': b.isSelected,
            });
            return (
              <div
                className={classes}
                key={i}
                onClick={b.onClick}
              >
                <Icon name={b.icon} />

                <span>{b.label}</span>
              </div>
            );
          })}
        </div>

        <hr />

        <div>
          {actionButtons.map((b, i) => {
            const classes = classNames({
              'menu-item': true,
              'menu-item-selected': b.isSelected,
            });
            return (
              <div
                className={classes}
                key={i}
                onClick={b.onClick}
              >
                <Icon name={b.icon} />

                <span>{b.label}</span>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};

export default Menu;