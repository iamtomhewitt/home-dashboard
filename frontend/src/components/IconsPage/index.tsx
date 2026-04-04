import LazySvg from '../LazySvgLoader';

/**
 * Helper page to show all the icons in a folder.
 */
const IconsPage = () => {
  const icons = [
    'barometer',
    'celsius',
    'clear-day',
    'clear-night',
    'cloudy',
    'compass',
    'drizzle',
    'dust-day',
    'dust-night',
    'dust-wind',
    'dust',
    'fahrenheit',
    'falling-stars',
    'fog-day',
    'fog-night',
    'fog',
    'hail',
    'haze-day',
    'haze-night',
    'haze',
    'horizon',
    'humidity',
    'hurricane',
    'lightning-bolt',
    'mist',
    'moon-first-quarter',
    'moon-full',
    'moon-last-quarter',
    'moon-new',
    'moon-waning-crescent',
    'moon-waning-gibbous',
    'moon-waxing-crescent',
    'moon-waxing-gibbous',
    'moonrise',
    'moonset',
    'not-available',
    'overcast-day',
    'overcast-night',
    'overcast',
    'partly-cloudy-day-drizzle',
    'partly-cloudy-day-fog',
    'partly-cloudy-day-hail',
    'partly-cloudy-day-haze',
    'partly-cloudy-day-rain',
    'partly-cloudy-day-sleet',
    'partly-cloudy-day-smoke',
    'partly-cloudy-day-snow',
    'partly-cloudy-day',
    'partly-cloudy-night-drizzle',
    'partly-cloudy-night-fog',
    'partly-cloudy-night-hail',
    'partly-cloudy-night-haze',
    'partly-cloudy-night-rain',
    'partly-cloudy-night-sleet',
    'partly-cloudy-night-smoke',
    'partly-cloudy-night-snow',
    'partly-cloudy-night',
    'pressure-high-alt',
    'pressure-high',
    'pressure-low-alt',
    'pressure-low',
    'rain',
    'raindrop',
    'raindrops',
    'sleet',
    'smoke-particles',
    'smoke',
    'snow',
    'snowflake',
    'solar-eclipse',
    'star',
    'starry-night',
    'sunrise',
    'sunset',
    'thermometer-celsius',
    'thermometer-colder',
    'thermometer-fahrenheit',
    'thermometer-glass-celsius',
    'thermometer-glass-fahrenheit',
    'thermometer-glass',
    'thermometer-mercury-cold',
    'thermometer-mercury',
    'thermometer-warmer',
    'thermometer',
    'thunderstorms-day-rain',
    'thunderstorms-day-snow',
    'thunderstorms-day',
    'thunderstorms-night-rain',
    'thunderstorms-night-snow',
    'thunderstorms-night',
    'thunderstorms-rain',
    'thunderstorms-snow',
    'thunderstorms',
    'tornado',
    'umbrella',
    'uv-index-1',
    'uv-index-10',
    'uv-index-11',
    'uv-index-2',
    'uv-index-3',
    'uv-index-4',
    'uv-index-5',
    'uv-index-6',
    'uv-index-7',
    'uv-index-8',
    'uv-index-9',
    'uv-index',
    'wind-beaufort-0',
    'wind-beaufort-1',
    'wind-beaufort-10',
    'wind-beaufort-11',
    'wind-beaufort-12',
    'wind-beaufort-2',
    'wind-beaufort-3',
    'wind-beaufort-4',
    'wind-beaufort-5',
    'wind-beaufort-6',
    'wind-beaufort-7',
    'wind-beaufort-8',
    'wind-beaufort-9',
    'wind',
    'windsock',
  ];

  return (
    <div style={{
      display: 'grid',
      gridTemplateColumns: 'auto auto auto',
      padding: '10px',
    }}
    >
      {icons.map(icon => (
        <div
          key={icon}
          style={{
            textAlign: 'center',
            margin: '20px',
          }}
        >
          <div>{icon}</div>

          <LazySvg
            height='8em'
            name={icon}
            width='8em'
          />
        </div>
      ))}
    </div>
  );
};

export default IconsPage;