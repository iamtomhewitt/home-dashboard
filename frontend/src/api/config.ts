import { DashboardConfig } from '../types/config';
import { http } from '../lib/https';

const apiUrl = import.meta.env.VITE_CONFIG_API;

type Response = {
  data: DashboardConfig;
  message: string;
  status: number;
}

const get = async (id: string): Promise<Response> => {
  return http.get(`${apiUrl}?id=${id}`);
};

export const configApi = {
  get,
};