import { NewsResponse } from '../types/lambda';
import { http } from '../lib/https';

const apiUrl = import.meta.env.VITE_API_URL;

const get = async (apiKey: string): Promise<NewsResponse> => {
  return http.get(`${apiUrl}/news?apiKey=${apiKey}`);
};

export const newsApi = {
  get,
};