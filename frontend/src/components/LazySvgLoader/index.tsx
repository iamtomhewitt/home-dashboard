import { ComponentProps, FC, useEffect, useRef, useState } from 'react';

const useLazySvgImport = (name: string) => {
  const importRef = useRef<FC<ComponentProps<'svg'>>>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<Error>();

  useEffect(() => {
    setLoading(true);

    const importIcon = async () => {
      try {
        importRef.current = (await import(`../../icons/${name}.svg?react`)).default; // We use `?react` here following `vite-plugin-svgr`'s convention.
      }
      catch (err) {
        setError(err as Error);
      }
      finally {
        setLoading(false);
      }
    };

    importIcon();
  }, [name]);

  return {
    error,
    loading,
    Svg: importRef.current,
  };
};

/**
 * Source - https://stackoverflow.com/a/61472427
 * 
 * Posted by junwen-k, modified by community. See post 'Timeline' for change history
 * 
 * Retrieved 2026-02-25, License - CC BY-SA 4.0
 */
const LazySvg = ({ name, ...props }: LazySvgProps) => {
  const { loading, error, Svg } = useLazySvgImport(name);

  if (error) {
    return `${error}`;
  }

  if (loading) {
    return 'Loading...';
  }

  if (!Svg) {
    return null;
  }

  return <Svg {...props} />;
};

interface LazySvgProps extends ComponentProps<'svg'> {
  name: string;
}

export default LazySvg;