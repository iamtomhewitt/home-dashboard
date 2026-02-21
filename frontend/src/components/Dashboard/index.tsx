import { useEffect } from 'react';
import { GridStack } from 'gridstack';

import 'gridstack/dist/gridstack.min.css';

const Dashboard = () => {
  useEffect(() => {
    GridStack.init();
  }, []);

  return (
    <div className='grid-stack'>
      <div
        className='grid-stack-item'
        gs-h={2} // eslint-disable-line react/no-unknown-property
        gs-w={2}// eslint-disable-line react/no-unknown-property
        gs-x={0}// eslint-disable-line react/no-unknown-property
        gs-y={0}// eslint-disable-line react/no-unknown-property
        style={{
          containerType: 'size',
        }}
      >
        <div
          className='grid-stack-item-content'
          style={{
            alignItems: 'center',
            backgroundColor: 'lightblue',
            borderRadius: '10px',
            color: 'white',
            cursor: 'pointer',
            display: 'inline-flex',
            fontSize: '15cqmin',
            inset: '3px !important',
            justifyContent: 'center',
            resize: 'both',
            textAlign: 'center',
          }}
        >
          This is some widget content
        </div>
      </div>

      <div
        className='grid-stack-item'
        gs-h={4} // eslint-disable-line react/no-unknown-property
        gs-w={4}// eslint-disable-line react/no-unknown-property
        gs-x={2}// eslint-disable-line react/no-unknown-property
        gs-y={2}// eslint-disable-line react/no-unknown-property
        style={{
          containerType: 'size',
        }}
      >
        <div
          className='grid-stack-item-content'
          style={{
            alignItems: 'center',
            backgroundColor: 'lightgreen',
            borderRadius: '10px',
            color: 'white',
            cursor: 'pointer',
            display: 'inline-flex',
            fontSize: '15cqmin',
            inset: '3px !important',
            justifyContent: 'center',
            resize: 'both',
            textAlign: 'center',
          }}
        >
          This is some widget content
        </div>
      </div>

    </div>

  );
};

export default Dashboard;