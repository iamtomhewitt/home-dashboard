import { ApiResponse } from './api';

export type TodoistItem = {
  id: string;
  name: string;
}

export type TodoistApiResponse = ApiResponse<TodoistItem[]>