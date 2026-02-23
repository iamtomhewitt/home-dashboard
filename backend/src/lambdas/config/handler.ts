import { APIGatewayProxyEvent } from 'aws-lambda';
import { DeleteObjectCommand, GetObjectCommand, PutObjectCommand } from '@aws-sdk/client-s3';

import s3 from '../../lib/s3';
import { LambdaError } from '../../lib/error';
import { response } from '../../lib/response';

export const handler = async (e: APIGatewayProxyEvent) => {
  try {
    const bucketName = 'home-dashboard-config';
    const { id } = e.queryStringParameters || {};

    if (!id) {
      throw new LambdaError('BadRequest', 'Missing \'id\'');
    }

    switch (e.httpMethod) {
      case 'GET': {
        const file = await s3.send(new GetObjectCommand({
          Bucket: bucketName,
          Key: id,
        }));
        const contents = await file.Body?.transformToString() || '{}';
        const data = JSON.parse(contents);
        return response.json(200, 'Success', data);
      }

      case 'PUT': {
        if (!e.body) {
          throw new LambdaError('BadRequest', 'Missing request body');
        }

        const data = await s3.send(new PutObjectCommand({
          Bucket: bucketName,
          Body: e.body,
          Key: id,
        }));

        return response.json(200, 'Success', data);
      }

      case 'DELETE': {
        const data = await s3.send(new DeleteObjectCommand({
          Bucket: bucketName,
          Key: id,
        }));

        return response.json(200, 'Success', data);
      }

      default:
        throw new LambdaError('BadRequest', `${e.httpMethod} is not supported`);
    }
  }
  catch (err: any) {
    let responseCode = 500;

    switch (err.name) {
      case 'NoSuchKey':
        responseCode = 404;
        break;

      case 'BadRequest':
        responseCode = 400;
        break;
    }

    return response.json(responseCode, err.message);
  }
};