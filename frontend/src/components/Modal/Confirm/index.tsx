import { useState } from 'react';

import { useModalStack } from '../../ModalStack';

import './index.scss';

const Confirm = ({ message, onNo, onYes }: Props) => {
  const [isLoading, setIsLoading] = useState(false);
  const modalstack = useModalStack();

  const onSelectNo = () => {
    onNo?.();
    modalstack.close();
  };

  const onSelectYes = async () => {
    setIsLoading(true);
    await onYes();
    modalstack.close();
    setIsLoading(false);
  };

  return (
    <div className='confirm'>
      <div className='confirm-message'>{message}</div>

      <button onClick={onSelectNo}>
        No
      </button>

      <button disabled={isLoading} onClick={onSelectYes}>
        {isLoading ? 'Please wait...' : 'Yes'}
      </button>
    </div>
  );
};

type Props = {
  message: string;
  onNo?: () => void;
  onYes: () => void;
}

export default Confirm;