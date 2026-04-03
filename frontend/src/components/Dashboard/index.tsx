import { useEffect, useState } from 'react';
import { GridStack } from 'gridstack';

import BbcNews from '../Widget/BbcNews';
import BinDay from '../Widget/BinDay';
import Clock from '../Widget/Clock';
import Confirm from '../Modal/Confirm';
import FoodPlan from '../Widget/FoodPlanner';
import Gmail from '../Widget/Gmail';
import Splitwise from '../Widget/Splitwise';
import Todoist from '../Widget/Todoist';
import Weather from '../Widget/Weather';
import pkg from '../../../package.json';
import { ConfigApiResponse } from '../../types/config';
import { api } from '../../lib/api';
import { dashboard } from '../../lib/dashboard';
import { http } from '../../lib/http';
import { sessionStorage } from '../../lib/session-storage';
import { time } from '../../lib/time';
import { useModalStack } from '../ModalStack';

import 'gridstack/dist/gridstack.min.css';

const Dashboard = () => {
  const [dashboardConfig, setDashboardConfig] = useState(sessionStorage.getDashboardConfig());
  const modalstack = useModalStack();
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

    dashboard.setBackgroundColour(dashboardConfig.backgroundColour);
  }, []);

  useEffect(() => {
    const checkVersion = async () => {
      const response = await http.get<any[]>('https://api.github.com/repos/iamtomhewitt/home-dashboard/tags');
      const latestTag: string = response[0].name;
      if (latestTag !== pkg.version) {
        modalstack.open(Confirm, {
          message: `There is a new version available (${latestTag}). Refresh to update?`,
          onYes: () => window.location.reload(),
          title: 'New Version Available',
        });
      }
    };

    checkVersion();

    const interval = setInterval(() => {
      checkVersion();
    }, time.toMilliseconds(7, 'days'));

    return () => clearInterval(interval);
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