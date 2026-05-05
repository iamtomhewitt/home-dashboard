import { StrictMode } from 'react';
import * as dateFns from 'date-fns';
import { BrowserRouter } from 'react-router';
import { createRoot } from 'react-dom/client';

import App from './components/App';

import './index.scss';

const container = document.getElementById('root');

if (!container) {
  throw new Error('No container!');
}

const originalFetch = window.fetch;

window.fetch = async (...args) => {
  const [url, options] = args;
  const response = await originalFetch(...args);

  if (!response.ok) {
    const logs: string[] = JSON.parse(sessionStorage.getItem('logs') || '[]');
    const formattedUrl = url.toString().includes('amazonaws.com') ? url.toString().split('amazonaws.com')[1] : url;
    logs.push(`${dateFns.format(new Date(), 'dd/MM/yy HH:mm:ssa')} | ${options?.method} | ${response.status} | ${formattedUrl}`);
    sessionStorage.setItem('logs', JSON.stringify(logs));
  }

  return response;
};

const root = createRoot(container);
root.render(
  <StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </StrictMode>,
);
