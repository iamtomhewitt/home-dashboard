const toMilliseconds = (amount: number, format: 'seconds' | 'minutes' | 'hours' | 'days' = 'seconds') => {
  switch (format) {
    case 'minutes':
      return amount * 60 * 1000;

    case 'hours':
      return amount * 3600 * 1000;

    case 'days':
      return amount * 86400 * 1000;

    case 'seconds':
    default:
      return amount * 1000;
  }
};

export const time = {
  toMilliseconds,
};