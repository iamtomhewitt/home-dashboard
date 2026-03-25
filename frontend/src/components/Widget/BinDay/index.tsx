import { useState } from 'react';
import * as dateFns from 'date-fns';

import Icon from '../../Icon';
import Widget from '../';
import { Widget as WidgetType } from '../../../types/widget';

import './index.scss';

const BinDay = ({ widget }: Props) => {
  const bins: Bin[] = widget.bins || [];
  const [display, setDisplay] = useState({
    colour: widget.noBinColour,
    message: 'No bins today!',
  });
  widget.colour = display.colour;

  const onRefresh = () => {
    const today = dateFns.startOfDay(new Date());
    const tomorrow = dateFns.startOfDay(dateFns.addDays(today, 1));
    let stopProcessing = false;

    const toDateString = (date: Date) => dateFns.format(date, 'dd-MM-yyyy');

    for (const bin of bins) {
      if (stopProcessing) {
        break;
      }

      const firstDate = dateFns.parse(bin.firstDate, 'dd-MM-yyyy', new Date());
      const lastBinDay = (() => {
        const days = dateFns.differenceInDays(today, firstDate);
        const remainder = days % bin.repeatRateInDays;
        return dateFns.startOfDay(dateFns.addDays(today, -remainder));
      })();
      const nextBinDay = dateFns.addDays(lastBinDay, bin.repeatRateInDays);
      const binToday = (toDateString(nextBinDay) === toDateString(today) || toDateString(lastBinDay) === toDateString(today));
      const binTomorrow = toDateString(nextBinDay) === toDateString(tomorrow);

      if (binToday) {
        stopProcessing = true;
        setDisplay({
          colour: bin.binColour,
          message: `${bin.name} bin today!`,
        });
      }
      else if (binTomorrow) {
        stopProcessing = true;
        setDisplay({
          colour: bin.binColour,
          message: `${bin.name} bin tomorrow!`,
        });
      }
    }
  };

  return (
    <Widget onRefresh={onRefresh} widget={widget}>
      <div className='bin-day'>
        <Icon name='trash-can' style='regular' />

        <div>{display?.message}</div>
      </div>
    </Widget>
  );
};

type Bin = {
  binColour: string;
  firstDate: string;
  name: string;
  repeatRateInDays: number;
}

type Props = {
  widget: WidgetType
}

export default BinDay;