const KEY = 'loggedIn';

const isLoggedIn = () => {
  return window.sessionStorage.getItem(KEY) === 'true';
};

const login = (key: boolean) => {
  window.sessionStorage.setItem(KEY, `${key}`);
};

const logout = () => {
  window.sessionStorage.removeItem(KEY);
};

export const credentials = {
  isLoggedIn,
  login,
  logout,
};