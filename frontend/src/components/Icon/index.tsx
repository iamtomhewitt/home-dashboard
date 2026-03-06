import classnames from 'classnames';

const Icon = ({ animation, name, size, style = 'solid' }: Props) => {
  const classes = classnames({
    [`fa-${animation}`]: !!animation,
    [`fa-${name}`]: true,
    [`fa-${size}`]: !!size,
    [`fa-${style}`]: true,
  });

  return <i className={classes}  />;
};

type Props = {
  animation?: string;
  name: string;
  size?: '2xs' | 'xs' | 'sm' | 'lg' | 'xl' | '2xl';
  style?: 'regular' | 'solid';
}

export default Icon;