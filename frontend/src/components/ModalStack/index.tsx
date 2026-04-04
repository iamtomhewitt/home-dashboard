import React, { ComponentType, createContext, useContext, useState } from 'react';

import Icon from '../Icon';

import './index.scss';

type ModalComponent<P = any> = ComponentType<P>;

type ModalStackItem = {
  Component: ModalComponent<any>;
  props: any;
};

type ModalContextType = {
  close: () => void;
  open: <P>(Component: ModalComponent<P>, props?: P) => any;
};

const ModalContext = createContext<ModalContextType | null>(null);

const ModalStack = ({ children }: Props) => {
  const [stack, setStack] = useState<ModalStackItem[]>([]);
  const current = stack[stack.length - 1];

  const open = <P, >(Component: ModalComponent<P>, props?: P) => {
    setStack(prev => [
      ...prev,
      {
        Component,
        props,
      },
    ]);
  };

  const close = () => {
    setStack(prev => prev.slice(0, -1));
  };

  return (
    <ModalContext.Provider value={{
      close,
      open,
    }}
    >
      {children}

      {current && (
        <div className='modal-stack'>
          <button className='modal-stack-overlay' onClick={close} />

          <div className='modal-stack-box'>
            <div className='modal-stack-title'>
              <span>{current.props.title || ''}</span>

              <span onClick={close}>
                <Icon name='xmark' />
              </span>
            </div>

            <div className='modal-stack-children'>
              <current.Component
                {...current.props}
                closeModal={close}
              />
            </div>
          </div>
        </div>
      )}
    </ModalContext.Provider>
  );
};

type Props = {
  children: React.ReactElement,
}

export default ModalStack;

export const useModalStack = () => {
  const ctx = useContext(ModalContext);

  if (!ctx) {
    throw new Error('useModalStack must be used inside ModalStack');
  }

  return ctx;
};