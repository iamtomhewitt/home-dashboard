import { useModalStack } from '../../ModalStack';

import './index.scss';

const Confirm = ({ message, onNo, onYes }: Props) => {
  const modalstack = useModalStack();

  const onSelectNo = () => {
    onNo?.();
    modalstack.close();
  };

  return (
    <div className='confirm'>
      <div className='confirm-message'>{message}</div>

      <button onClick={onSelectNo}>
        No
      </button>

      <button onClick={onYes}>
        Yes
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