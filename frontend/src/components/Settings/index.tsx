import { useEffect, useState } from 'react';
import casing from 'case';

import { Widget } from '../../types/widget';
import { sessionStorage } from '../../lib/session-storage';

import './index.scss';

const Settings = () => {
  const [widgetConfig, setWidgetConfig] = useState<Widget[]>([]);

  useEffect(() => {
    const { backgroundColour, widgets } = sessionStorage.getDashboardConfig();
    setWidgetConfig(widgets);

    const body = document.getElementById('body');

    if (body) {
      body.style.backgroundColor = backgroundColour;
    }
    else {
      console.warn('Could not find element by ID \'body\'');
    }
  }, []);

  const onChangeWidgetConfig = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { id: widgetId, name: key, value } = e.target;
    setWidgetConfig(prev =>
      prev.map(widget => widget.id === widgetId ?
        {
          ...widget,
          [key]: value,
        } :
        widget,
      ),
    );
  };

  const onSave = () => {
    console.log(widgetConfig);
  };

  return (
    <div className='settings'>
      <h1>Settings</h1>

      <h2>Device</h2>

      <div>
        <span>Width: {window.innerWidth}px </span>

        <span>Height: {window.innerHeight}px </span>
      </div>

      <h2>Widgets</h2>

      {widgetConfig.map(widget => (
        <div className='settings-item' key={widget.id}>
          {Object.entries(widget).map(([key, value]) => {
            return (
              <div className='settings-item-value' key={key}>
                <div>{casing.title(key)}</div>

                <input
                  id={widget.id}
                  name={key}
                  onChange={onChangeWidgetConfig}
                  value={value}
                />
              </div>
            );
          })}
        </div>
      ))}

      <button onClick={onSave}>Save</button>
    </div>
  );
};

export default Settings;