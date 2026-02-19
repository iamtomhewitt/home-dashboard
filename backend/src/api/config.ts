import { ListBucketsCommand } from '@aws-sdk/client-s3';
import { response } from '../lib/response';
import s3 from '../lib/s3'

export const handler = async () => {
  try {
    const r = await s3.send(new ListBucketsCommand({
      Prefix: 'home-dashboard'
    }))
    return response.ok({
      message: 'Hi world!',
      r,
    });
  } catch (err) {
    return response.ok({
      error: `${err}`
    })
  }
};