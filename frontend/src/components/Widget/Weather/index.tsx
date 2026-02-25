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
          <div key={i}>
            <span>{format(new Date(hourly.date), 'HH:mm')}</span>

            <LazySvg
              height='3em'
              name={toWeatherIcon(hourly.condition)}
              width='3em'
            />

            <div>
              <span>{hourly.temperature}°</span>

              <LazySvg
                height='3em'
                name={'thermometer-celsius'}
                width='3em'
              />
            </div>
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