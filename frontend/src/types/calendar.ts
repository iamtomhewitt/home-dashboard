import { ApiResponse } from './api';

export type Calendar = {
  name: string;
  date: string;
}

export type CalendarApiResponse = ApiResponse<Calendar[]>