import classNames from 'classnames';

import Icon from '../../Icon';

import './index.scss';

const MenuButton = ({ icon, label, isSelected, onClick }: Props) => {
  const classes = classNames({
    'menu-button': true,
    'menu-button-selected': isSelected,
  });

  return (
    <div className={classes} onClick={onClick}>
      <Icon name={icon} />

      <span>{label}</span>
    </div>
  );
};

type Props = {
  icon: string;
  label: string;
  isSelected: boolean;
  onClick: () => void;
}

export default MenuButton;