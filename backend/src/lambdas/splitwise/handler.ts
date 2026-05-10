import { APIGatewayProxyEvent } from 'aws-lambda';
import { BadRequestError, withErrorHandling } from '@iamtomhewitt/error';
import { http } from '@iamtomhewitt/http';

const main = async (e: APIGatewayProxyEvent) => {
  const { apiKey, groupId } = e.queryStringParameters || {};

  if (!apiKey || !groupId) {
    throw new BadRequestError('Missing \'apiKey\' or \'groupId\'');
  }

  const url = `https://secure.splitwise.com/api/v3.0/get_group/${groupId}`;
  const splitwiseResponse = await fetch(url, {
    headers: {
      Authorization: `Bearer ${apiKey}`,
      'Content-Type': 'application/json',
    },
  })
    .then(response => response.json());

  const { group } = splitwiseResponse;
  const members: any[] = group.members.map((member: any) => ({
    id: member.id,
    name: member.first_name,
  }));

  if (group.simplified_debts.length === 0) {
    return http.response.ok({
      body: {
        amount: 'n/a',
        owes: 'n/a',
        settledUp: true,
        who: 'n/a',
      },
    });
  }

  const debt = group.simplified_debts[0];
  const data = {
    amount: `£${parseFloat(debt.amount).toFixed(2)}`,
    owes: members.find(m => m.id === debt.to).name,
    settledUp: false,
    who: members.find(m => m.id === debt.from).name,
  };

  return http.response.ok({
    body: data,
  });
};

export const handler = withErrorHandling(
  main,
  (err, code) => {
    return http.response.json(code, {
      message: `${err.name}: ${err.message}`,
    });
  },
);