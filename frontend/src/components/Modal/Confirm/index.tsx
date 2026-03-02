import './index.scss';

const Confirm = ({ message, onNo, onYes }: Props) => (
  <div className='confirm'>
    <div className='confirm-message'>{message}</div>

    <button onClick={onNo}>
      No
    </button>

    <button onClick={onYes}>
      Yes
    </button>
  </div>
);

type Props = {
  message: string;
  onNo: () => void;
  onYes: () => void;
}

export default Confirm;