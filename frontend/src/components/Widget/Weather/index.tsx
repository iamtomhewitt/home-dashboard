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
      case 'light-drizzle':
      case 'moderate-rain':
        return 'rain';
      case 'partly-cloudy':
        return 'cloudy';
      default:
        return condition;
    }
  };

  return (
    <Widget onRefresh={onRefresh} widget={widget}>
      <div className='weather'>
        {weather?.hourly.map((hourly, i) => (
          <div className='weather-item' key={i}>
            <div>{format(new Date(hourly.date), 'HH')}</div>

            <LazySvg
              height='3em'
              name={toWeatherIcon(hourly.condition)}
              width='3em'
            />

            <span>{hourly.temperature.toFixed(0)}°</span>
          </div>
        ))}

        {weather?.daily.map((daily, i) => (
          <div className='weather-item' key={i}>
            <div>{format(new Date(daily.date), 'eee')}</div>

            <LazySvg
              height='3em'
              name={toWeatherIcon(daily.condition)}
              width='3em'
            />

            <span>{daily.temperature.toFixed(0)}°</span>
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