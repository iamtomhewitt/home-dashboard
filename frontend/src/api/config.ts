import { http } from '../lib/https';

const apiUrl = import.meta.env.VITE_CONFIG_API;

const get = async (id: string) => {
  return http.get(`${apiUrl}?id=${id}`);
};

export const configApi = {
  get,
};