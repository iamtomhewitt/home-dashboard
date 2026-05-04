import { APIGatewayProxyEvent, APIGatewayProxyResult } from 'aws-lambda';
import { http } from '@iamtomhewitt/http';

export class LambdaError extends Error {
  code = 500;

  constructor (name: string, message: string, code = 500) {
    super(message);
    this.name = name;
    this.code = code;
    Object.setPrototypeOf(this, LambdaError.prototype);
  }
}

export class BadRequestError extends LambdaError {
  constructor (message: string) {
    super('BadRequest', message, 400);
  }
}

export class NotFoundError extends LambdaError {
  constructor (message: string) {
    super('NotFound', message, 404);
  }
}

export class UnauthorisedError extends LambdaError {
  constructor (message: string) {
    super('Unauthorised', message, 401);
  }
}

export const withErrorHandling = (handler: (e: APIGatewayProxyEvent) => Promise<APIGatewayProxyResult>) => async (e: APIGatewayProxyEvent) => {
  try {
    return await handler(e);
  }
  catch (err: any) {
    console.log(`Error: ${err}`);

    let { code } = err;

    if (!err.code) {
      const codeMap: any = {
        NoSuchKey: 404,
      };

      code = codeMap[err.name] || 500;
    }

    return http.response.json(code, {
      message: `${err.name}: ${err.message}`, 
    });
  }
};