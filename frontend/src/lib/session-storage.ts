const getDashboardConfig = () => {
  return JSON.parse(window.sessionStorage.getItem('config') || '{}');
};

const setDashboardConfig = (data: any) => {
  window.sessionStorage.setItem('config', JSON.stringify(data));
};

export const sessionStorage = {
  getDashboardConfig,
  setDashboardConfig,
};