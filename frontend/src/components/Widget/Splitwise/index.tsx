import { useState } from 'react';

import Icon from '../../Icon';
import Widget from '../';
import { SplitwiseApiResponse, SplitwiseGroup } from '../../../types/splitwise';
import { Widget as WidgetType } from '../../../types/widget';
import { api } from '../../../lib/api';

import './index.scss';

const Splitwise = ({ widget }: Props) => {
  const [debt, setDebt] = useState<SplitwiseGroup>();
  const owesLabel = `${debt?.who} owes ${debt?.owes}`;

  const fetchData = async () => {
    const response = await api.get<SplitwiseApiResponse>(`/splitwise?apiKey=${widget.apiKey}&groupId=${widget.groupId}`);
    setDebt(response.data);
  };

  return (
    <Widget onRefresh={fetchData} widget={widget}>
      <div className='splitwise'>
        <Icon name='sterling-sign' />

        {debt?.settledUp ?
          <div>All settled up!</div> :
          <>
            <div>
              {owesLabel}
            </div>

            <div className='splitwise-amount'>
              {debt?.amount}
            </div>
          </>}
      </div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default Splitwise;