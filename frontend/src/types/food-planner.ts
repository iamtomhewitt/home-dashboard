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
  ingredients?: RecipeIngredient[];
  name: string;
  steps?: string[];
}

export type RecipeIngredient = {
  amount: number;
  name: string;
  weight: 'grams' | 'quantity' | 'tablespoon' | 'teaspoon';
}

export type CookbookApiResponse = ApiResponse<Recipe[]>;

export type FoodPlannerApiResponse = ApiResponse<FoodPlan>;

export type ShoppingListResponse = ApiResponse<string[]>;