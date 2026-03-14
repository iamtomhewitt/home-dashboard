import './index.scss';

const Settings = () => {
  return (
    <div className='settings'>
      <h1>Settings</h1>

      <div>Width: {window.innerWidth}</div>

      <div>Height: {window.innerHeight}</div>

    </div>
  );
};

export default Settings;