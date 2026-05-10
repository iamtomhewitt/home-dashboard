import { APIGatewayProxyEvent } from 'aws-lambda';
import { BadRequestError, withErrorHandling } from '@iamtomhewitt/error';
import { http } from '@iamtomhewitt/http';

/**
 * *Why don't you make this request from the browser?*
 * 
 * That is part of a paid plan for `newsapi`, free tiers must come from a backend, otherwise
 * you get a `HTTP 426`.
 */
const main = async (e: APIGatewayProxyEvent) => {
  const { apiKey } = e.queryStringParameters || {};

  if (!apiKey) {
    throw new BadRequestError('Missing \'apiKey\'');
  }

  const url = `https://newsapi.org/v2/top-headlines?sources=bbc-news&apiKey=${apiKey}`;
  const articles = await fetch(url)
    .then(response => response.json())
    .then(data => data.articles);
  return http.response.ok({
    body: articles, 
  });
};

export const handler = withErrorHandling(
  main,
  (err, code) => {
    return http.response.json(code, {
      message: `${err.name}: ${err.message}`,
    });
  },
);