import { useEffect, useState } from 'react';
import casing from 'case';

import Confirm from '../Modal/Confirm';
import { ConfigApiResponse } from '../../types/config';
import { Widget } from '../../types/widget';
import { http } from '../../lib/https';
import { sessionStorage } from '../../lib/session-storage';
import { useModalStack } from '../ModalStack';

import './index.scss';

const Settings = () => {
  const [message, setMessage] = useState('');
  const [widgetConfig, setWidgetConfig] = useState<Widget[]>([]);
  const modalstack = useModalStack();
  const { backgroundColour, id, widgets } = sessionStorage.getDashboardConfig();

  useEffect(() => {
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
    setMessage('');
    modalstack.open(Confirm, {
      message: 'Are you sure you want to save this config?',
      onYes: async () => {
        const response = await http.get<ConfigApiResponse>(`/config?id=${id}`);
        setMessage(response.message);
      },
      title: 'Warning',
    });
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
            const title = casing.title(key);

            if (Array.isArray(value) || typeof (value) === 'object') {
              return (
                <div className='settings-item-value' key={key}>
                  <div>{title}</div>

                  <input disabled value={`Config "${key}" not editable at this time.`} />
                </div>
              );
            }

            return (
              <div className='settings-item-value' key={key}>
                <div>{title}</div>

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

      <button className='settings-save' onClick={onSave}>
        Save
      </button>

      {message && <div className='settings-message'>{message}</div>}
    </div>
  );
};

export default Settings;