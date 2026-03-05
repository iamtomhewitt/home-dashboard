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

export type Recipe = {
  ingredients?: {
    amount: number;
    category: 'dairy' | 'fruit' | 'vegetable' | 'meat' | 'other';
    name: string;
    weight: 'grams' | 'quantity' | 'tablespoon' | 'teaspoon';
  }[];
  name: string;
  steps?: string[];
}

export type FoodPlannerApiResponse = ApiResponse<FoodPlan>

export type CookbookApiResponse = ApiResponse<Recipe[]>