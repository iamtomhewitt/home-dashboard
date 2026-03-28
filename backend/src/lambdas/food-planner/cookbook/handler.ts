import { APIGatewayProxyEvent } from 'aws-lambda';

import s3 from '../../../lib/s3';
import { BadRequestError, withErrorHandling } from '../../../lib/error';
import { response } from '../../../lib/response';

const main = async (e: APIGatewayProxyEvent) => {
  const { id } = e.queryStringParameters || {};
  const bucketName = 'home-dashboard-food-planner';
  const cookbookKey = `${id}/cookbook`;

  if (!id) {
    throw new BadRequestError('Missing \'id\'');
  }

  switch (e.httpMethod) {
    case 'GET': {
      const data = await s3.getObjectAsJson(bucketName, cookbookKey);
      return response.json(200, 'Success', data);
    }

    case 'PUT': {
      if (!e.body) {
        throw new BadRequestError('Missing request body');
      }

      if (!await s3.itemExists(bucketName, cookbookKey)) {
        await s3.save(bucketName, cookbookKey, e.body);
      }
      else {
        const existingRecipes = await s3.getObjectAsJson(bucketName, cookbookKey);
        const newRecipes = {
          ...existingRecipes,
          ...JSON.parse(e.body),
        };
        await s3.save(bucketName, cookbookKey, JSON.stringify(newRecipes));
      }

      return response.json(200, 'Success');
    }

    default:
      throw new BadRequestError(`${e.httpMethod} is not supported`);
  }
};

export const handler = withErrorHandling(main);