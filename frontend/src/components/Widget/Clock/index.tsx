import { useState } from 'react';
import { format } from 'date-fns';

import Widget from '../';
import { Widget as WidgetType } from '../../../types/widget';
import { refreshWidget } from '../../../hooks/refreshWidget';

const Clock = ({ widget }: Props) => {
  const [currentTime, setCurrentTime] = useState('');

  refreshWidget(widget, () => {
    setCurrentTime(format(new Date(), 'HH:mm:ss'));
  });

  return (
    <Widget widget={widget}>
      <div>{currentTime}</div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default Clock;