const request = async <T>(url: string, method: string, body?: any): Promise<T> => {
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
    status: response.status,
    ...json,
  };
};

const get = async <T>(url: string) => {
  return await request<T>(
    url,
    'GET',
    {},
  );
};

const put = async <T>(url: string, body?: any) => {
  return await request<T>(
    url,
    'PUT',
    body,
  );
};

const post = async <T>(url: string, body?: any) => {
  return await request<T>(
    url,
    'POST',
    body,
  );
};

const deleteRequest = async <T>(url: string) => {
  return await request<T>(
    url,
    'DELETE',
    {},
  );
};

export const http = {
  get,
  post,
  put,
  delete: deleteRequest,
};