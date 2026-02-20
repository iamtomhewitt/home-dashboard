import { http } from '../lib/https';

const get = async (id: string) => {
  return http.get(`${import.meta.env.VITE_CONFIG_API}?id=${id}`);
};

export const configApi = {
  get,
};