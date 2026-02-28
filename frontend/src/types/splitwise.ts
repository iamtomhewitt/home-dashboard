import { ApiResponse } from './api';

export type SplitwiseGroup = {
  amount: string;
  owes: string;
  who: string;
}

export type SplitwiseApiResponse = ApiResponse<SplitwiseGroup>