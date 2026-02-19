const json = (statusCode: number, body: any) => ({
  body: JSON.stringify(body),
  statusCode,
});

export const response = {
  ok: (body: any) => json(200, body),
  json,
};