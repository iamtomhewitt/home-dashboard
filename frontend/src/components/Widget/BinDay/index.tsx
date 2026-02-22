import { useState } from 'react';
import * as dateFns from 'date-fns';

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
    const today = new Date();
    const tomorrow = dateFns.addDays(today, 1);
    let stopProcessing = false;

    for (const bin of bins) {
      if (stopProcessing) {
        break;
      }

      const firstDate = dateFns.parse(bin.firstDate, 'dd-MM-yyyy', new Date());
      const lastBinDay = (() => {
        const days = dateFns.differenceInDays(today, firstDate);
        const remainder = days % bin.repeatRateInDays;
        return dateFns.addDays(today, -remainder);
      })();

      const nextBinDay = dateFns.addDays(lastBinDay, bin.repeatRateInDays);
      const binToday = nextBinDay === today || lastBinDay === today;
      const binTomorrow = nextBinDay === tomorrow || lastBinDay === tomorrow;

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
        <i className='fa-regular fa-trash-can' />

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