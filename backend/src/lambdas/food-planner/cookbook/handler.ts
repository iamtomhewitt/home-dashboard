import { APIGatewayProxyEvent } from 'aws-lambda';
import { http } from '@iamtomhewitt/http';

import s3 from '../../../lib/s3';
import { BadRequestError, withErrorHandling } from '../../../lib/error';

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
      return http.response.ok({
        body: data, 
      });
    }

    case 'PUT': {
      if (!e.body) {
        throw new BadRequestError('Missing request body');
      }

      const existingRecipes: any[] = await s3.getObjectAsJson(bucketName, cookbookKey);

      if (!await s3.itemExists(bucketName, cookbookKey)) {
        await s3.save(bucketName, cookbookKey, e.body);
        return http.response.ok({
          body: existingRecipes, 
        });
      }
      else {
        const requestBody = JSON.parse(e.body);
        const updatedRecipes = [
          ...existingRecipes.filter(r => r.name !== requestBody.name),
          requestBody,
        ];
        await s3.save(bucketName, cookbookKey, JSON.stringify(updatedRecipes));
        return http.response.ok({
          body: updatedRecipes, 
        });
      }
    }

    case 'DELETE': {
      const { recipeName } = e.queryStringParameters || {};
      const existingRecipes: any[] = await s3.getObjectAsJson(bucketName, cookbookKey);
      const withRecipeFilteredOut = existingRecipes.filter(recipe => recipe.name !== recipeName);
      await s3.save(bucketName, cookbookKey, JSON.stringify(withRecipeFilteredOut));
      return http.response.ok({
        body: withRecipeFilteredOut,
        message: `'${recipeName}' deleted`, 
      });
    }

    default:
      throw new BadRequestError(`${e.httpMethod} is not supported`);
  }
};

export const handler = withErrorHandling(main);