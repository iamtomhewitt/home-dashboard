const corsHeaders = {
  'Access-Control-Allow-Origin': '*',
  'Access-Control-Allow-Credentials': true,
  'Access-Control-Allow-Headers': 'Content-Type,Authorization',
  'Access-Control-Allow-Methods': 'GET,PUT,POST,DELETE,OPTIONS',
};

/**
 * `body` will be wrapped in a json object, so passing:
 * ```
 * { foo: 'bar' }
 * ```
 * would return a body with:
 * ```
 * { data: { foo: 'bar' } }
 * ```
 */
const json = (statusCode: number, message: string, body?: ResponseBody) => {
  if (!message) {
    throw new Error('No "message" property in response body');
  }

  return {
    body: JSON.stringify({
      message,
      data: body,
    }),
    headers: corsHeaders,
    statusCode,
  };
};

export const response = {
  json,
};

type ResponseBody = {
  [key: string]: any;
}