import { sessionStorage } from '../../lib/session-storage';

const Dashboard = () => {
  return (
    <div>
      <code>{JSON.stringify(sessionStorage.getDashboardConfig(), null, 2)}</code>
    </div>
  );
};

export default Dashboard;