import { ApiResponse } from './api';

export type SplitwiseGroup = {
  amount: string;
  owes: string;
  settledUp: boolean;
  who: string;
}

export type SplitwiseApiResponse = ApiResponse<SplitwiseGroup>