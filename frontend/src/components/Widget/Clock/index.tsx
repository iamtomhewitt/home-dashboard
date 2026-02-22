import { useState } from 'react';
import { format } from 'date-fns';

import Widget from '../';
import { Widget as WidgetType } from '../../../types/widget';

import './index.scss';

const Clock = ({ widget }: Props) => {
  const [currentTime, setCurrentTime] = useState('');

  const onRefresh = () => {
    setCurrentTime(format(new Date(), 'HH:mm:ss'));
  };

  return (
    <Widget onRefresh={onRefresh} widget={widget}>
      <div className='clock'>{currentTime}</div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default Clock;