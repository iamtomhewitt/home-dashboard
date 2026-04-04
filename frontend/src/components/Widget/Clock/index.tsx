import { useState } from 'react';
import { format } from 'date-fns';

import Widget from '../';
import { Widget as WidgetType } from '../../../types/widget';

import './index.scss';

const Clock = ({ widget }: Props) => {
  const [currentTime, setCurrentTime] = useState('');
  const [currentDate, setCurrentDate] = useState('');

  const onRefresh = () => {
    const now = new Date();
    setCurrentTime(format(now, 'HH:mm:ss'));
    setCurrentDate(format(now, 'dd MMM yyyy'));
  };

  return (
    <Widget onRefresh={onRefresh} widget={widget}>
      <div>
        <div className='clock-time'>{currentTime}</div>

        <div className='clock-date'>{currentDate}</div>
      </div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default Clock;