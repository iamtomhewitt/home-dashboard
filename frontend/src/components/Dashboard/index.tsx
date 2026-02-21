import { useEffect } from 'react';
import { GridStack } from 'gridstack';

import BbcNews from '../Widget/BbcNews';
import { sessionStorage } from '../../lib/session-storage';

import 'gridstack/dist/gridstack.min.css';

const Dashboard = () => {
  const { widgets } = sessionStorage.getDashboardConfig();

  useEffect(() => {
    GridStack.init();
  }, []);

  return (
    <div className='grid-stack'>
      {/* todo loop through widgest and use a map object to lookup which component to render */}
      <BbcNews data={widgets[0]} />

    </div>

  );
};

export default Dashboard;