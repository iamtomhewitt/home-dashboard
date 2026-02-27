import { APIGatewayProxyEvent } from 'aws-lambda';

import { BadRequestError, LambdaError, withErrorHandling } from '../../lib/error';
import { response } from '../../lib/response';

const main = async (e: APIGatewayProxyEvent) => {
  const { apiKey, projectId } = e.queryStringParameters || {};

  if (!apiKey || !projectId) {
    throw new BadRequestError('Missing \'apiKey\' or \'projectId\'');
  }

  const makeTodoistRequest = async (path?: string, options?: RequestInit) => {
    const { method = 'GET', body } = options || {};
    const response = await fetch(`https://api.todoist.com/api/v1/tasks${path}`, {
      method,
      headers: {
        Authorization: `Bearer ${apiKey}`,
        'Content-Type': 'application/json',
      },
      ...method !== 'GET' && {
        body: JSON.stringify(body),
      },
    });

    const json = await response.json();

    if (!response.ok) {
      throw new LambdaError('InternalServerError', json.error);
    }

    return json;
  };

  switch (e.httpMethod) {
    case 'GET': {
      const data = await makeTodoistRequest(`?project_id=${projectId}`)
        .then(({ results = [] }) => results.map((item: any) => item.content));

      return response.json(200, 'Success', data);
    }

    case 'POST': {
      if (!e.body) {
        throw new BadRequestError('Missing request body');
      }

      const data = await makeTodoistRequest('', {
        body: {
          ...JSON.parse(e.body),
          project_id: projectId,
        },
        method: 'POST',
      });

      return response.json(200, 'Task created', data);
    }

    default:
      throw new BadRequestError(`${e.httpMethod} is not supported`);
  }
};

export const handler = withErrorHandling(main);