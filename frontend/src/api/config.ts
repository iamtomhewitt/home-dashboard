import { ConfigResponse } from '../types/lambda';
import { http } from '../lib/https';

const apiUrl = import.meta.env.VITE_API_URL;

const get = async (id: string): Promise<ConfigResponse> => {
  return http.get(`${apiUrl}/config?id=${id}`);
};

export const configApi = {
  get,
};