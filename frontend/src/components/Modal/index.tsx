import React, { useEffect, useState } from 'react';
import PubSub from 'pubsub-js';

import './index.scss';

const Modal = () => {
  const [children, setChildren] = useState<React.ReactElement<ModalProps> | null>(null);
  const [onChildClose, setOnChildClose] = useState<(() => void) | null>(null);
  const [title, setTitle] = useState('');

  useEffect(() => {
    PubSub.subscribe('show-modal', (topic: string, data) => {
      setChildren(data.component);
      setOnChildClose(data.onClose);
      setTitle(data.title);
    });

    return () => {
      PubSub.unsubscribe('show-modal');
    };
  }, []);

  const onClose = async () => {
    onChildClose && await onChildClose();
    setChildren(null);
  };

  if (!children) {
    return null;
  }

  return (
    <div className='modal'>
      <button className='modal-overlay' onClick={onClose} />

      <div className='modal-box'>

        <div className='modal-title'>
          <span>{title}</span>

          <i className='fa-solid fa-xmark' onClick={onClose} />
        </div>

        <div className='modal-children'>
          {React.cloneElement(children, {
            onClose,
          })}
        </div>
      </div>
    </div>
  );
};

export type ModalProps = {
  onClose?: () => void;
}

export default Modal;