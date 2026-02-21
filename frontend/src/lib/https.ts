const request = async (url: string, method: string, body?: any) => {
  const bodyToUse = body || {};
  const response = await fetch(url, {
    method,
    ...method !== 'GET' && {
      body: JSON.stringify(bodyToUse),
    },
  });

  const json = await response.json();
  return {
    ...json,
    status: response.status,
  };
};

const get = async <T>(url: string): Promise<T> => {
  return await request(
    url,
    'GET',
    {},
  );
};

const put = async <T>(url: string, body?: any): Promise<T> => {
  return await request(
    url,
    'PUT',
    body,
  );
};

const post = async <T>(url: string, body?: any): Promise<T> => {
  return await request(
    url,
    'POST',
    body,
  );
};

export const http = {
  get,
  post,
  put,
};