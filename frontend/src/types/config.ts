import { ApiResponse } from './api';
import { Widget } from './widget';

export type DashboardConfig = {
  id: string;
  widgets: Widget[]
}

export type ConfigApiResponse = ApiResponse<DashboardConfig>