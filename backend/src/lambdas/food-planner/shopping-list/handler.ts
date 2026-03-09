import { APIGatewayProxyEvent } from 'aws-lambda';

import s3 from '../../../lib/s3';
import { BadRequestError, withErrorHandling } from '../../../lib/error';
import { response } from '../../../lib/response';

const main = async (e: APIGatewayProxyEvent) => {
  const { id } = e.queryStringParameters || {};
  const bucketName = 'home-dashboard-food-planner';
  const plannerKey = `${id}/planner`;

  if (!id) {
    throw new BadRequestError('Missing \'id\'');
  }

  switch (e.httpMethod) {
    case 'GET': {
      const planner = await s3.getObjectAsJson(bucketName, plannerKey);
      return response.json(200, 'Success', {
        planner, 
      });
    }

    default:
      throw new BadRequestError(`${e.httpMethod} is not supported`);
  }
};

export const handler = withErrorHandling(main);