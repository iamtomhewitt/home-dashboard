import { APIGatewayProxyEvent } from 'aws-lambda';
import { DeleteObjectCommand } from '@aws-sdk/client-s3';
import { http } from '@iamtomhewitt/http';

import s3 from '../../lib/s3';
import { BadRequestError, withErrorHandling } from '../../lib/error';

const main = async (e: APIGatewayProxyEvent) => {
  const bucketName = 'home-dashboard-config';
  const { id } = e.queryStringParameters || {};

  if (!id) {
    throw new BadRequestError('Missing \'id\'');
  }

  switch (e.httpMethod) {
    case 'GET': {
      const data = await s3.getObjectAsJson(bucketName, id);
      return http.response.ok({
        body: data,
      });
    }

    case 'PUT': {
      if (!e.body) {
        throw new BadRequestError('Missing request body');
      }

      const data = await s3.save(bucketName, id, e.body);
      return http.response.ok({
        body: data,
      });
    }

    case 'DELETE': {
      const data = await s3.send(new DeleteObjectCommand({
        Bucket: bucketName,
        Key: id,
      }));

      return http.response.ok({
        body: data,
      });
    }

    default:
      throw new BadRequestError(`${e.httpMethod} is not supported`);
  }
};

export const handler = withErrorHandling(main);