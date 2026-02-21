export type Widget = {
  colour: string;
  id: string;
  repeatRate: number;
  repeatTime: 'seconds' | 'minutes' | 'hours';
  sleepEnd: string;
  sleepStart: string;
  title: string;
  [key: string]: string | number | boolean;
}