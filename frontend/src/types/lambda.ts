import { DashboardConfig } from './config';

export type LambdaResponse = {
  data: any;
  message: string;
  status: number;
}

export interface NewsResponse extends LambdaResponse {
  data: {
    author: string;
    content: string;
    description: string;
    publishedAt: string;
    title: string;
    url: string;
    urlToImage: string;
    source: {
      id: string;
      name: string;
    },
  }[]
}

export interface ConfigResponse extends LambdaResponse {
  data: DashboardConfig;
}

export interface CalendarResponse extends LambdaResponse {
  data: {
    name: string;
    date: string;
  }[];
}