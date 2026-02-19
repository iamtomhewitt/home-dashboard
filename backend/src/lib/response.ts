export const response = {
  ok: (body: any) => ({
    body: JSON.stringify(body),
    statusCode: 200,
  }),
};