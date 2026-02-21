import { useEffect, useRef } from 'react';

import { Widget } from '../types/widget';
import { time } from '../lib/time';

/**
 * Use this hook to refresh a widget. Each widget will have a function that
 * causes it to update itself. Instead of writing out the same `useEffect`
 * function over and over, you can supply a function to this hook to do
 * it for you.
 */
export const refreshWidget = (
  widget: Widget,
  callback: () => void,
  dependencies: React.DependencyList = [],
) => {
  const savedCallback = useRef(callback);

  // Always keep latest callback
  useEffect(() => {
    savedCallback.current = callback;
  }, [callback]);

  useEffect(() => {
    const interval = setInterval(() => {
      savedCallback.current();
    }, time.toMilliseconds(widget.repeatRate, widget.repeatTime));

    return () => clearInterval(interval);
  }, [
    widget.repeatRate,
    widget.repeatTime,
    ...dependencies,
  ]);
};