import { useState } from 'react';
import { format } from 'date-fns';

import Widget from '../';
import { WeatherResponse } from '../../../types/lambda';
import { Widget as WidgetType } from '../../../types/widget';
import { api } from '../../../lib/https';

import './index.scss';

const Weather = ({ widget }: Props) => {
  const [weather, setWeather] = useState<WeatherResponse['data']>();

  const onRefresh = async () => {
    const response = await api.get<WeatherResponse>(`/weather?apiKey=${widget.apiKey}&latitude=${widget.latitude}&longitude=${widget.longitude}`);
    setWeather(response.data);
  };

  return (
    <Widget onRefresh={onRefresh} widget={widget}>
      <div className='weather'>
        {weather?.location}
      </div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default Weather;