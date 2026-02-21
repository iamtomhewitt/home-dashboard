import { useEffect, useState } from 'react';

import LoadingIcon from '../../Icons/Loading';
import Widget from '../';
import { NewsResponse } from '../../../types/lambda';
import { Widget as WidgetType } from '../../../types/widget';
import { newsApi } from '../../../api/news';
import { refreshWidget } from '../../../hooks/refreshWidget';

const BbcNews = ({ widget }: Props) => {
  const [articles, setArticles] = useState<NewsResponse['data']>([]);
  const [index, setIndex] = useState(0);

  refreshWidget(widget, async () => {
    const response = await newsApi.get(widget.apiKey as string);
    setArticles(response.data);
  });

  useEffect(() => {
    const interval = setInterval(() => {
      setIndex(prev => prev >= articles.length - 1 ? 0 : prev + 1);
    }, (widget.secondsBetweenArticles as number) * 1000);

    return () => {
      clearInterval(interval);
    };
  }, [articles.length]);

  return (
    <Widget widget={widget}>
      <div>{articles.length > 0 ? articles[index].title : <LoadingIcon />}</div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType;
}

export default BbcNews;