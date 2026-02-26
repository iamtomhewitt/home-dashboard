import { APIGatewayProxyEvent } from 'aws-lambda';

import { BadRequestError, withErrorHandling } from '../../lib/error';
import { response } from '../../lib/response';

const main = async (e: APIGatewayProxyEvent) => {
  const { apiKey, projectId } = e.queryStringParameters || {};

  if (!apiKey || !projectId) {
    throw new BadRequestError('Missing \'apiKey\' or \'projectId\'');
  }

  const url = `https://api.todoist.com/api/v1/tasks?project_id=${projectId}`;
  const data = await fetch(url, {
    headers: {
      Authorization: `Bearer ${apiKey}`,
    },
  })
    .then(response => response.json())
    .then(({ results = [] }) => results.map((item: any) => item.content));

  return response.json(200, 'Success', data);
};

export const handler = withErrorHandling(main);