import { useModalStack } from '../../ModalStack';

import './index.scss';

const Ok = ({ message }: Props) => {
  const modalstack = useModalStack();
  const toDisplay = Array.isArray(message) ? message : [message];

  return (
    <div className='ok'>
      <div className='ok-message'>
        {toDisplay.map((m, i) => <div key={i}>{m}</div>)}
      </div>

      <button onClick={() => modalstack.close()}>
        OK
      </button>
    </div>
  );
};

type Props = {
  message: string | string[];
}

export default Ok;