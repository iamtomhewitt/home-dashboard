import { APIGatewayProxyEvent } from 'aws-lambda';

import { LambdaError } from '../lib/error';
import { response } from '../lib/response';

export const handler = async (e: APIGatewayProxyEvent) => {
  try {
    const { apiKey } = e.queryStringParameters || {};

    if (!apiKey) {
      throw new LambdaError('BadRequest', 'Missing \'apiKey\'');
    }

    const url = `https://newsapi.org/v2/top-headlines?sources=bbc-news&apiKey=${apiKey}`;
    const articles = await fetch(url)
      .then(response => response.json())
      .then(data => data.articles);
    return response.json(200, 'Success', articles);
  }
  catch (err: any) {
    return response.json(500, err.message);
  }
};