import React, { useEffect, useState } from 'react';
import PubSub from 'pubsub-js';

import './index.scss';

const Modal = () => {
  const [children, setChildren] = useState<React.ReactElement | null>(null);
  const [title, setTitle] = useState('');

  useEffect(() => {
    PubSub.subscribe('show-modal', (topic: string, data) => {
      setChildren(data.component);
      setTitle(data.title);
    });

    return () => {
      PubSub.unsubscribe('show-modal');
    };
  }, []);

  const onClose = () => {
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
          {children}
        </div>
      </div>
    </div>
  );
};

export default Modal;