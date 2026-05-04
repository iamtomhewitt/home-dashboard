import { http } from '@iamtomhewitt/http';

const apiUrl = import.meta.env.VITE_API_URL;

const _delete = async <T>(path: string) => await http.request.delete<T>(`${apiUrl}${path}`);
const get = async <T>(path: string) => await http.request.get<T>(`${apiUrl}${path}`);
const post = async <T>(path: string, body?: any) => await http.request.post<T>(`${apiUrl}${path}`, body);
const put = async <T>(path: string, body?: any) => await http.request.put<T>(`${apiUrl}${path}`, body);

export const api = {
  get,
  post,
  put,
  delete: _delete,
};