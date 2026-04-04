import { APIGatewayProxyEvent } from 'aws-lambda';

import s3 from '../../../lib/s3';
import { BadRequestError, withErrorHandling } from '../../../lib/error';
import { response } from '../../../lib/response';

const main = async (e: APIGatewayProxyEvent) => {
  const { id } = e.queryStringParameters || {};
  const bucketName = 'home-dashboard-food-planner';
  const plannerKey = `${id}/planner`;
  const cookbookKey = `${id}/cookbook`;

  if (!id) {
    throw new BadRequestError('Missing \'id\'');
  }

  switch (e.httpMethod) {
    case 'GET': {
      const planner = await s3.getObjectAsJson(bucketName, plannerKey);
      const cookbook = await s3.getObjectAsJson(bucketName, cookbookKey);
      const recipes = Object.values(planner);
      const allIngredients = recipes.flatMap((recipe: any) => {
        const matchingRecipeFromCookbook = cookbook.find((cookbookRecipe: any) => cookbookRecipe.name === recipe);
        return matchingRecipeFromCookbook ? matchingRecipeFromCookbook.ingredients : [];
      });

      const groupedIngredients = Object.values(
        allIngredients.reduce((acc, item) => {
          const key = `${item.name.trim()}_${item.weight}`;

          if (!acc[key]) {
            acc[key] = {
              ...item,
              name: item.name.trim(),
              amount: 0,
            };
          }

          acc[key].amount += Number(item.amount);

          return acc;
        }, {}),
      );

      const shoppingList = groupedIngredients.map((ingredient: any) => {
        let amountSuffix = ingredient.weight;

        switch (ingredient.weight) {
          case 'quantity':
            amountSuffix = '';
            break;
          case 'grams':
            amountSuffix = 'g of';
            break;
          case 'teaspoon':
            amountSuffix = 'tsp of';
            break;
          case 'tablespoon':
            amountSuffix = 'tbsp of';
            break;
          default:
            break;
        }

        return `${ingredient.amount}${amountSuffix} ${ingredient.name}`;
      });

      return response.ok({
        body: shoppingList, 
      });
    }

    default:
      throw new BadRequestError(`${e.httpMethod} is not supported`);
  }
};

export const handler = withErrorHandling(main);