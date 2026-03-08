import { ApiResponse } from './api';
import { Widget } from './widget';

export type DashboardConfig = {
  backgroundColour: string;
  id: string;
  widgets: Widget[]
}

export type ConfigApiResponse = ApiResponse<DashboardConfig>