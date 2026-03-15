import { useState } from 'react';
import { format } from 'date-fns';

import LazySvg from '../../LazySvgLoader';
import Widget from '../';
import { WeatherApiResponse, WeatherData } from '../../../types/weather';
import { Widget as WidgetType } from '../../../types/widget';
import { http } from '../../../lib/https';

import './index.scss';

const Weather = ({ widget }: Props) => {
  const [weather, setWeather] = useState<WeatherData>();

  const onRefresh = async () => {
    const response = await http.get<WeatherApiResponse>(`/weather?apiKey=${widget.apiKey}&latitude=${widget.latitude}&longitude=${widget.longitude}`);
    setWeather(response.data);
  };

  const toWeatherIcon = (condition: string) => {
    switch (condition) {
      case 'light-drizzle':
      case 'light-rain':
        return 'drizzle';

      case 'light-rain-shower':
      case 'moderate-rain':
      case 'patchy-rain-nearby':
        return 'rain';

      case 'partly-cloudy':
        return 'cloudy';

      case 'clear':
      case 'sunny':
        return 'clear-day';

      case 'thundery-outbreaks-in-nearby':
        return 'thunderstorms-rain';

      case 'light-sleet-showers':
        return 'sleet';

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