import { SSMClient } from '@aws-sdk/client-ssm';

export default new SSMClient({
  region: 'eu-west-1',
});