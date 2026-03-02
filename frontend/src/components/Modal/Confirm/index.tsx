import './index.scss';

const Confirm = ({ message, onYes, onNo }: Props) => (
  <div className='confirm'>
    <div>{message}</div>

    <button>
      No
    </button>

    <button>
      Yes
    </button>

    <br />
  </div>
);

type Props = {
  message: string;
  onYes: () => void;
  onNo: () => void;
}

export default Confirm;