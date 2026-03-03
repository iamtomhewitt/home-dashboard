import { ModalProps } from '../';

import './index.scss';

const Confirm = ({ message, onNo, onYes, onClose }: Props) => {
  const onSelectNo = () => {
    onNo();
    onClose?.();
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
  onNo: () => void;
  onYes: () => void;
} & ModalProps

export default Confirm;