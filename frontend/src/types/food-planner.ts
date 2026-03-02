import { ApiResponse } from './api';

export type FoodPlan = {
  Monday: string;
  Tuesday: string;
  Wednesday: string;
  Thursday: string;
  Friday: string;
  Saturday: string;
  Sunday: string;
}

export type FoodPlannerApiResponse = ApiResponse<FoodPlan>