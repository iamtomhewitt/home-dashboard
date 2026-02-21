import { useEffect } from 'react';
import { GridStack } from 'gridstack';

import BbcNews from '../Widget/BbcNews';
import Clock from '../Widget/Clock';
import { sessionStorage } from '../../lib/session-storage';

import 'gridstack/dist/gridstack.min.css';

const Dashboard = () => {
  const { widgets } = sessionStorage.getDashboardConfig();
  const widgetLookup: any = {
    bbcNews: BbcNews,
    clock: Clock,
  };

  useEffect(() => {
    GridStack.init();
  }, []);

  return (
    <div className='grid-stack'>
      {widgets.map((widget, i) => {
        const Component = widgetLookup[widget.id];

        if (!Component) {
          console.warn('Could not find component for', widget.id || widget);
          return null;
        }

        return <Component key={i} widget={widget} />;
      })}

    </div>
  );
};

export default Dashboard;