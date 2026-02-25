import { useState } from 'react';
import { format } from 'date-fns';

import LazySvg from '../../LazySvgLoader';
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

  const toWeatherIcon = (condition: string) => {
    switch (condition) {
      case 'patchy-rain-nearby':
        return 'raindrops';
      case 'light-drizzle':
        return 'raindrop';
      default:
        return condition;
    }
  };

  return (
    <Widget onRefresh={onRefresh} widget={widget}>
      <div className='weather'>
        {weather?.hourly.map((hourly, i) => (
          <div key={i}>
            <span>{format(new Date(hourly.date), 'HH:mm')}</span>

            <LazySvg
              height='3em'
              name={toWeatherIcon(hourly.condition)}
              width='3em'
            />

            <span>{hourly.temperature}</span>
          </div>
        ))}

      </div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default Weather;