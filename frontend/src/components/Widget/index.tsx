import { Widget as WidgetType } from '../../types/widget';

import './index.scss';

/**
 * Base widget, other widgets should wrap this component.
 */
const Widget = ({ x = 0, y = 0, width = 2, height = 2, children, widget }: Props) => (
  <div
    className='grid-stack-item widget'
    gs-h={height} // eslint-disable-line react/no-unknown-property
    gs-w={width}// eslint-disable-line react/no-unknown-property
    gs-x={x}// eslint-disable-line react/no-unknown-property
    gs-y={y}// eslint-disable-line react/no-unknown-property
  >
    {widget.title && (
      <div className='widget-title'>
        {widget.title}
      </div>
    )}

    <div
      className='grid-stack-item-content widget-content'
      style={{
        backgroundColor: widget.colour,
      }}
    >
      {children}
    </div>
  </div>
);

type Props = {
  children: React.ReactElement;
  height?: number;
  widget: WidgetType;
  width?: number;
  x?: number;
  y?: number;
}

export default Widget;