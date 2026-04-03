import { useEffect, useState } from 'react';
import { GridStack } from 'gridstack';

import BbcNews from '../Widget/BbcNews';
import BinDay from '../Widget/BinDay';
import Clock from '../Widget/Clock';
import FoodPlan from '../Widget/FoodPlanner';
import Gmail from '../Widget/Gmail';
import Splitwise from '../Widget/Splitwise';
import Todoist from '../Widget/Todoist';
import Weather from '../Widget/Weather';
import { ConfigApiResponse } from '../../types/config';
import { api } from '../../lib/api';
import { sessionStorage } from '../../lib/session-storage';

import 'gridstack/dist/gridstack.min.css';

const Dashboard = () => {
  const [dashboardConfig, setDashboardConfig] = useState(sessionStorage.getDashboardConfig());
  const widgetLookup: any = {
    bbcNews: BbcNews,
    binDay: BinDay,
    clock: Clock,
    foodPlanner: FoodPlan,
    gmail: Gmail,
    splitwise: Splitwise,
    todoist: Todoist,
    weather: Weather,
  };

  useEffect(() => {
    GridStack.init();

    const fetchConfig = async () => {
      const response = await api.get<ConfigApiResponse>(`/config?id=${dashboardConfig.id}`);
      setDashboardConfig(response.data);
      sessionStorage.setDashboardConfig(response.data);
    };

    fetchConfig();

    const body = document.getElementById('body');

    if (body) {
      body.style.backgroundColor = dashboardConfig.backgroundColour;
    }
    else {
      console.warn('Could not find element by ID \'body\'');
    }
  }, []);

  return (
    <div className='grid-stack'>
      {dashboardConfig.widgets.map((widget, i) => {
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