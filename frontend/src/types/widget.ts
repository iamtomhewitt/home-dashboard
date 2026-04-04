export type Widget = {
  colour: string;
  height?: number;
  id: string;
  repeatRate: number;
  repeatTime: 'seconds' | 'minutes' | 'hours' | 'days';
  sleepEnd: string;
  sleepStart: string;
  title: string;
  width?: number;
  x?: number;
  y?: number;
  [key: string]: string | number | boolean | any;
}