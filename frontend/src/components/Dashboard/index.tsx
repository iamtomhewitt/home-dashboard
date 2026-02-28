import { useEffect } from 'react';
import { GridStack } from 'gridstack';

import BbcNews from '../Widget/BbcNews';
import BinDay from '../Widget/BinDay';
import Clock from '../Widget/Clock';
import Gmail from '../Widget/Gmail';
import Modal from '../Modal';
import Todoist from '../Widget/Todoist';
import Weather from '../Widget/Weather';
import { sessionStorage } from '../../lib/session-storage';

import 'gridstack/dist/gridstack.min.css';

const Dashboard = () => {
  const { widgets } = sessionStorage.getDashboardConfig();
  const widgetLookup: any = {
    bbcNews: BbcNews,
    binDay: BinDay,
    clock: Clock,
    gmail: Gmail,
    todoist: Todoist,
    weather: Weather,
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

      <Modal />

    </div>
  );
};

export default Dashboard;