import { DashboardConfig } from '../types/config';

const getDashboardConfig = (): DashboardConfig => {
  return JSON.parse(window.sessionStorage.getItem('config') || '{}');
};

const setDashboardConfig = (data: DashboardConfig) => {
  window.sessionStorage.setItem('config', JSON.stringify(data));
};

export const sessionStorage = {
  getDashboardConfig,
  setDashboardConfig,
};