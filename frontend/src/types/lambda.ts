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

export interface WeatherResponse extends LambdaResponse {
  data: {
    location: string;
    now: {
      condition: string;
      temperature: number;
    };
    hourly: {
      condition: string;
      date: string;
      temperature: number;
    }[];
    daily: {
      condition: string;
      date: string;
      temperature: number;
    }[];
  }
}

export interface TodoistResponse extends LambdaResponse {
  data: {
    id: string;
    name: string;
  }[];
}

export interface SplitwiseResponse extends LambdaResponse {
  data: {
    amount: string;
    owes: string;
    who: string;
  }
}