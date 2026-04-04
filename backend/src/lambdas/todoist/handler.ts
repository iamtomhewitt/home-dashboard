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

    if (!response.ok) {
      const json = await response.json();
      throw new LambdaError('InternalServerError', json.error);
    }

    return response;
  };

  switch (e.httpMethod) {
    case 'GET': {
      const data = await makeTodoistRequest(`?project_id=${projectId}`)
        .then(response => response.json())
        .then(({ results = [] }) => results.map((item: any) => ({
          name: item.content,
          id: item.id,
        })));

      return response.ok({
        body: data, 
      });
    }

    case 'POST': {
      if (!e.body) {
        throw new BadRequestError('Missing request body');
      }

      const requestBody = JSON.parse(e.body);

      await makeTodoistRequest('', {
        body: {
          ...requestBody,
          project_id: projectId,
        },
        method: 'POST',
      });

      return response.ok({
        message: `${requestBody.content} created`, 
      });
    }

    case 'DELETE': {
      const { id } = e.queryStringParameters || {};

      if (!id) {
        throw new BadRequestError('No "id" specified');
      }

      await makeTodoistRequest(`/${id}`, {
        method: 'DELETE',
      });

      return response.noContent({
        message: 'Task deleted', 
      });
    }

    default:
      throw new BadRequestError(`${e.httpMethod} is not supported`);
  }
};

export const handler = withErrorHandling(main);