const apiUrl = import.meta.env.VITE_API_URL;

const request = async <T>(path: string, method: string, body?: any): Promise<T> => {
  const url = `${apiUrl}${path}`;
  const bodyToUse = body || {};
  const response = await fetch(url, {
    method,
    ...method !== 'GET' && {
      body: JSON.stringify(bodyToUse),
    },
  });

  const json = response.headers.get('Content-Type') === 'application/json' ?
    await response.json() :
    {};

  return {
    ...json,
    status: response.status,
  };
};

const get = async <T>(path: string) => {
  return await request<T>(
    path,
    'GET',
    {},
  );
};

const put = async <T>(path: string, body?: any) => {
  return await request<T>(
    path,
    'PUT',
    body,
  );
};

const post = async <T>(path: string, body?: any) => {
  return await request<T>(
    path,
    'POST',
    body,
  );
};

const deleteRequest = async <T>(path: string) => {
  return await request<T>(
    path,
    'DELETE',
    {},
  );
};

export const api = {
  get,
  post,
  put,
  delete: deleteRequest,
};