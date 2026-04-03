import { http } from './http';

const apiUrl = import.meta.env.VITE_API_URL;

const deleteRequest = async <T>(path: string) => await http.delete<T>(`${apiUrl}${path}`);
const get = async <T>(path: string) => await http.get<T>(`${apiUrl}${path}`);
const post = async <T>(path: string, body?: any) => await http.post<T>(`${apiUrl}${path}`, body);
const put = async <T>(path: string, body?: any) => await http.put<T>(`${apiUrl}${path}`, body);

export const api = {
  get,
  post,
  put,
  delete: deleteRequest,
};