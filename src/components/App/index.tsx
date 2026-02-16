import { useEffect } from 'react';
import { GridStack } from 'gridstack';

import './index.scss';
import 'gridstack/dist/gridstack.min.css';

const App = () => {
  useEffect(() => {
    GridStack.init();
  }, []);

  return (
    <div className='app'>

      <div className='grid-stack'>
        <div
          className='grid-stack-item'
          style={{
            cursor: 'pointer', 
          }}
        >
          <div className='grid-stack-item-content'>Item 1</div>
        </div>

        <div
          className='grid-stack-item'
          gs-w='2'
          style={{
            cursor: 'pointer', 
          }}
        >
          <div className='grid-stack-item-content'>Item 2 wider</div>
        </div>
      </div>

    </div>
  );
};

export default App;
