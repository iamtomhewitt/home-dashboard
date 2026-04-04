import { DashboardConfig } from '../types/config';

const setBackgroundColour = (colour?: string) => {
  const body = document.getElementById('body');

  if (body && colour) {
    body.style.backgroundColor = colour;
  }
  else {
    console.warn('Could not find element by ID \'body\', or no colour parameter supplied');
  }
};

const getConfig = (): DashboardConfig => {
  return JSON.parse(window.sessionStorage.getItem('config') || '{}');
};

const setConfig = (data: DashboardConfig) => {
  window.sessionStorage.setItem('config', JSON.stringify(data));
};

export const dashboard = {
  getConfig,
  setBackgroundColour,
  setConfig,
};