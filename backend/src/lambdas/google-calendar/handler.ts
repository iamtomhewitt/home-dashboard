import { APIGatewayProxyEvent } from 'aws-lambda';
import { GetParameterCommand } from '@aws-sdk/client-ssm';
import { google } from 'googleapis';

import ssm from '../../lib/ssm';
import { LambdaError } from '../../lib/error';
import { response } from '../../lib/response';

export const handler = async (e: APIGatewayProxyEvent) => {
  try {
    const { apiKey, gmail } = e.queryStringParameters || {};

    if (!apiKey || !gmail) {
      throw new LambdaError('BadRequest', 'Missing \'apiKey\' or \'gmail\'');
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
    return response.json(500, err.message);
  }
};