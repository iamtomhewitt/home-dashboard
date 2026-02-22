export type Widget = {
  colour: string;
  id: string;
  repeatRate: number;
  repeatTime: 'seconds' | 'minutes' | 'hours' | 'days';
  sleepEnd: string;
  sleepStart: string;
  title: string;
  [key: string]: string | number | boolean | any;
}