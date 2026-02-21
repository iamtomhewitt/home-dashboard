type Response = {
  data: {
    message: string;
    [key: string]: any;
  };
  status: number;
}

const request = async (url: string, method: string, body?: any): Promise<Response> => {
  const bodyToUse = body || {};
  const response = await fetch(url, {
    method,
    headers: {
      'Content-Type': 'application/json',
    },
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

const get = async (url: string) => {
  return await request(
    url,
    'GET',
    {},
  );
};

const put = async (url: string, body?: any) => {
  return await request(
    url,
    'PUT',
    body,
  );
};

const post = async (url: string, body?: any) => {
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