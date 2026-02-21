import { useEffect, useState } from 'react';
import { format } from 'date-fns';

import Widget from '../';
import { Widget as WidgetType } from '../../../types/widget';

const Clock = ({ widget }: Props) => {
  const [time, setTime] = useState('');

  useEffect(() => {
    const interval = setInterval(() => {
      setTime(format(new Date(), 'HH:mm:ss'));
    }, 1000);

    return () => {
      clearInterval(interval);
    };
  }, []);

  return (
    <Widget widget={widget}>
      <div>{time}</div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default Clock;