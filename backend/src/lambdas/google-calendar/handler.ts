import { APIGatewayProxyEvent } from 'aws-lambda';
import { GetParameterCommand } from '@aws-sdk/client-ssm';
import { HeadObjectCommand } from '@aws-sdk/client-s3';
import { google } from 'googleapis';

import s3 from '../../lib/s3';
import ssm from '../../lib/ssm';
import { LambdaError } from '../../lib/error';
import { response } from '../../lib/response';

export const handler = async (e: APIGatewayProxyEvent) => {
  try {
    const { apiKey, gmail } = e.queryStringParameters || {};

    if (!apiKey || !gmail) {
      throw new LambdaError('BadRequest', 'Missing \'apiKey\' or \'gmail\'');
    }

    // Slight 'auth' to stop you form finding out the url and passing in an email to get back events
    // The API key is just the config key for your dashboard
    const matchingApiKey = await s3
      .send(new HeadObjectCommand({
        Bucket: 'home-dashboard-config',
        Key: apiKey,
      }))
      .then(() => true)
      .catch((err) => {
        if (err.name === 'NotFound') {
          return false;
        }
        throw err;
      });

    if (!matchingApiKey) {
      throw new LambdaError('Unauthorised', 'Incorrect API key');
    }

    const credentials = await ssm
      .send(new GetParameterCommand({
        Name: 'home-dashboard-calendar-manager-key-file',
      }))
      .then(d => JSON.parse(d.Parameter?.Value || '{}')) as any;

    const auth = new google.auth.GoogleAuth({
      credentials,
      scopes: ['https://www.googleapis.com/auth/calendar.readonly'],
    });

    const calendar = google.calendar({
      auth,
      version: 'v3',
    });

    const calendarResponse = await calendar.events.list({
      calendarId: gmail,
      maxResults: 10,
      orderBy: 'startTime',
      singleEvents: true,
      timeMin: new Date().toISOString(),
    });

    const events = (calendarResponse.data.items || []).map((event) => ({
      date: event.start?.dateTime || event.start?.date,
      name: event.summary,
    }));

    return response.json(200, 'Success', events);
  }
  catch (err: any) {
    let responseCode = 500;

    switch (err.name) {
      case 'Unauthorised':
        responseCode = 401;
        break;

      case 'BadRequest':
        responseCode = 400;
        break;
    }

    return response.json(responseCode, err.message);
  }
};