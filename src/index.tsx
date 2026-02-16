import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';

import App from './components/App';

import './index.css';

const container = document.getElementById('root');

if (!container) {
  throw new Error('No container!');
}

const root = createRoot(container);
root.render(
  <StrictMode>
    <App />
  </StrictMode>,
);
