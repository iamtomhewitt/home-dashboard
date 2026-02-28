import { ApiResponse } from './api';

export type WeatherData = {
  location: string;
  now: {
    condition: string;
    temperature: number;
  };
  hourly: {
    condition: string;
    date: string;
    temperature: number;
  }[];
  daily: {
    condition: string;
    date: string;
    temperature: number;
  }[];
}

export type WeatherApiResponse = ApiResponse<WeatherData>;