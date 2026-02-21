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
const json = (statusCode: number, body: ResponseBody) => ({
  body: JSON.stringify({
    data: body,
  }),
  headers: corsHeaders,
  statusCode,
});

export const response = {
  ok: (body: any) => json(200, body),
  json,
};

type ResponseBody = {
  message: string;
  [key: string]: any;
}