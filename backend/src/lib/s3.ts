import { GetObjectCommand, HeadObjectCommand, PutObjectCommand, S3Client } from '@aws-sdk/client-s3';

const client = new S3Client({
  region: 'eu-west-1',
});

export default {
  send: client.send,
  getObjectAsJson: async (bucket: string, key: string) => {
    const response = await client.send(
      new GetObjectCommand({
        Bucket: bucket,
        Key: key,
      }),
    );

    const contents = await response.Body?.transformToString() ?? '{}';
    return JSON.parse(contents);
  },
  save: async (bucket: string, key: string, data: any) => {
    return await client.send(new PutObjectCommand({
      Body: data,
      Bucket: bucket,
      Key: key,
    }));
  },
  itemExists: async (bucket: string, key: string) => {
    return await client
      .send(new HeadObjectCommand({
        Bucket: bucket,
        Key: key,
      }))
      .then(() => true)
      .catch((err) => {
        if (err.name === 'NotFound') {
          return false;
        }
        throw err;
      });
  },
};