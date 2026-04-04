import { APIGatewayProxyEvent } from 'aws-lambda';
import { GetParameterCommand } from '@aws-sdk/client-ssm';
import { google } from 'googleapis';

import s3 from '../../lib/s3';
import ssm from '../../lib/ssm';
import { BadRequestError, UnauthorisedError, withErrorHandling } from '../../lib/error';
import { response } from '../../lib/response';

const main = async (e: APIGatewayProxyEvent) => {
  const { apiKey, gmail } = e.queryStringParameters || {};

  if (!apiKey || !gmail) {
    throw new BadRequestError('Missing \'apiKey\' or \'gmail\'');
  }

  // Slight 'auth' to stop you from finding out the url and passing in an email to get back events
  // The API key is just the config key for your dashboard
  const matchingApiKey = await s3.itemExists('home-dashboard-config', apiKey);

  if (!matchingApiKey) {
    throw new UnauthorisedError('Incorrect API key');
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
    maxResults: 12,
    orderBy: 'startTime',
    singleEvents: true,
    timeMin: new Date().toISOString(),
  });

  const events = (calendarResponse.data.items || []).map((event) => ({
    date: event.start?.dateTime || event.start?.date,
    name: event.summary,
  }));

  return response.ok({
    body: events, 
  });
};

export const handler = withErrorHandling(main);