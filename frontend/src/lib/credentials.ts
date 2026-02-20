const DASHBOARD_KEY = 'dashboardKey';

const isLoggedIn = () => {
  return !!window.sessionStorage.getItem(DASHBOARD_KEY);
};

const login = (key: string) => {
  window.sessionStorage.setItem(DASHBOARD_KEY, key);
};

export const credentials = {
  isLoggedIn,
  login,
};