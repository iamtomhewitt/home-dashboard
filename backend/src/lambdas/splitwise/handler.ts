import { APIGatewayProxyEvent } from 'aws-lambda';

import { BadRequestError, withErrorHandling } from '../../lib/error';
import { response } from '../../lib/response';

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

  const debt = group.simplified_debts[0];
  const data = {
    amount: `£${parseFloat(debt.amount).toFixed(2)}`,
    owes: members.find(m => m.id === debt.to).name,
    who: members.find(m => m.id === debt.from).name,
  };

  return response.json(200, 'Success', data);
};

export const handler = withErrorHandling(main);