import { ApiResponse } from './api';

export type WeatherData = {
  location: string;
  now: {
    condition: string;
    temperature: number;
  };
  hourly: WeatherCondition[];
  daily: WeatherCondition[];
}

export type WeatherCondition = {
  condition: string;
  date: string;
  temperature: number;
};

export type WeatherApiResponse = ApiResponse<WeatherData>;