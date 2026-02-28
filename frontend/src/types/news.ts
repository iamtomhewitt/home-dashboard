import { ApiResponse } from './api';

export type NewsItem = {
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
}

export type NewsApiResponse = ApiResponse<NewsItem[]>