import { useEffect, useRef, useState } from 'react';

import Icon from '../Icon';
import { Widget as WidgetType } from '../../types/widget';
import { time } from '../../lib/time';

import './index.scss';

/**
 * Base widget, other widgets should wrap this component.
 */
const Widget = ({
  children,
  height = 1,
  onRefresh,
  widget,
  width = 1,
  x = 0,
  y = 0,
}: Props) => {
  const refreshRef = useRef(onRefresh);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    refreshRef.current = onRefresh;
  }, [onRefresh]);

  const runRefresh = async () => {
    setIsLoading(true);

    try {
      const result = refreshRef.current();

      if (result instanceof Promise) {
        await result;
      }
    }
    finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    runRefresh();

    const interval = setInterval(() => {
      runRefresh();
    }, time.toMilliseconds(widget.repeatRate, widget.repeatTime));

    return () => clearInterval(interval);
  }, [widget.repeatRate, widget.repeatTime]);

  return (
    <div
      className='grid-stack-item widget'
      gs-h={(widget.height || height) * 2} // eslint-disable-line react/no-unknown-property
      gs-w={(widget.width || width) * 2} // eslint-disable-line react/no-unknown-property 
      gs-x={(widget.x || x) * 2} // eslint-disable-line react/no-unknown-property 
      gs-y={(widget.y || y) * 2} // eslint-disable-line react/no-unknown-property
    >
      {widget.title && <div className='widget-title'>{widget.title}</div>}

      <div
        className='grid-stack-item-content widget-content'
        style={{
          background: widget.colour,
        }}
      >
        {/* TODO now we have a loading state var we can set a last update state var once it completes */}
        {isLoading ?
          <Icon
            animation='spin'
            name='circle-notch'
            size='2xl'
          /> :
          children}
      </div>
    </div>
  );
};

type Props = {
  /** `onRefresh` should be a function that changes widgets' internal state,
   * like making API requests. It should not do things like fading animations
   * via state management (e.g. BBC News widget).
   */
  onRefresh: () => void | Promise<void>;
  children: React.ReactElement;
  height?: number;
  widget: WidgetType;
  width?: number;
  x?: number;
  y?: number;
};

export default Widget;