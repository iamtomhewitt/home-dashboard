import { useState } from 'react';
import { format } from 'date-fns';

import Widget from '../';
import { Calendar, CalendarApiResponse } from '../../../types/calendar';
import { Widget as WidgetType } from '../../../types/widget';
import { api } from '../../../lib/api';
import { sessionStorage } from '../../../lib/session-storage';

import './index.scss';

const Gmail = ({ widget }: Props) => {
  const [events, setEvents] = useState<Calendar[]>([]);

  const fetchEvents = async () => {
    const { id } = sessionStorage.getDashboardConfig();
    const response = await api.get<CalendarApiResponse>(`/calendar?apiKey=${id}&gmail=${widget.gmailAddress}`);
    setEvents(response.data);
  };

  return (
    <Widget onRefresh={fetchEvents} widget={widget}>
      <ul className='gmail'>
        {events.map((event, i) => (
          <li key={i}>
            <span>{event.name}</span>

            <span>{format(new Date(event.date), 'd MMM')}</span>
          </li>
        ))}
      </ul>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default Gmail;