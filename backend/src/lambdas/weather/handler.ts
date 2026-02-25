import { APIGatewayProxyEvent } from 'aws-lambda';

import { LambdaError } from '../../lib/error';
import { response } from '../../lib/response';

export const handler = async (e: APIGatewayProxyEvent) => {
  try {
    const { apiKey, latitude, longitude, days = 4 } = e.queryStringParameters || {};

    if (!apiKey) {
      throw new LambdaError('BadRequest', 'Missing \'apiKey\'');
    }

    const url = `http://api.weatherapi.com/v1/forecast.json?key=${apiKey}&q=${latitude},${longitude}&days=${days}`;
    const result = await fetch(url)
      .then(response => response.json());

    const toKebabCase = (str: string) => str.split(' ').filter(Boolean).join('-').toLowerCase();

    const daily = result.forecast.forecastday
      .map((dayForecast: any) => ({
        condition: toKebabCase(dayForecast.day.condition.text),
        date: new Date(dayForecast.date),
        temperature: dayForecast.day.avgtemp_c,
      }));

    const hourly = result.forecast.forecastday
      .flatMap((dayForecast: any) => {
        return dayForecast.hour
          .filter((hourlyWeather: any) => new Date() < new Date(hourlyWeather.time))
          .flatMap((hourlyWeather: any) => ({
            condition: toKebabCase(hourlyWeather.condition.text),
            date: new Date(hourlyWeather.time),
            temperature: hourlyWeather.temp_c,
          }));
      })
      .slice(0, 5);

    const mappedWeather = {
      location: result.location.name,
      now: {
        condition: toKebabCase(result.current.condition.text),
        temperature: result.current.temp_c,
      },
      hourly,
      daily,
    };

    return response.json(200, 'Success', mappedWeather);
  }
  catch (err: any) {
    let responseCode = 500;

    switch (err.name) {
      case 'BadRequest':
        responseCode = 400;
        break;
    }

    return response.json(responseCode, err.message);
  }
};