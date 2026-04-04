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
      const data = await s3.getObjectAsJson(bucketName, plannerKey);
      return response.ok({
        body: data, 
      });
    }

    case 'PUT': {
      if (!e.body) {
        throw new BadRequestError('Missing request body');
      }

      if (!await s3.itemExists(bucketName, plannerKey)) {
        await s3.save(bucketName, plannerKey, e.body);
      }
      else {
        const existingPlanner = await s3.getObjectAsJson(bucketName, plannerKey);
        const newPlanner = {
          ...existingPlanner,
          ...JSON.parse(e.body),
        };
        await s3.save(bucketName, plannerKey, JSON.stringify(newPlanner));
      }

      return response.ok({});
    }

    default:
      throw new BadRequestError(`${e.httpMethod} is not supported`);
  }
};

export const handler = withErrorHandling(main);