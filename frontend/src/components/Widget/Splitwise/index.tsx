import { useState } from 'react';

import Widget from '../';
import { SplitwiseResponse } from '../../../types/lambda';
import { Widget as WidgetType } from '../../../types/widget';
import { api } from '../../../lib/https';

import './index.scss';

const Splitwise = ({ widget }: Props) => {
  const [debt, setDebt] = useState<SplitwiseResponse['data']>();
  const owesLabel = `${debt?.who} owes ${debt?.owes}`;

  const fetchData = async () => {
    const response = await api.get<SplitwiseResponse>(`/splitwise?apiKey=${widget.apiKey}&groupId=${widget.groupId}`);
    setDebt(response.data);
  };

  return (
    <Widget onRefresh={fetchData} widget={widget}>
      <div className='splitwise'>
        <i className='fa-solid fa-sterling-sign' />

        <div>
          {owesLabel}
        </div>

        <div className='splitwise-amount'>
          {debt?.amount}
        </div>
      </div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default Splitwise;