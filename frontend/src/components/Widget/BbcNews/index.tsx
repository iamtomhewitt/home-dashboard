import { useEffect, useState } from 'react';

import Widget from '../';
import { NewsResponse } from '../../../types/lambda';
import { Widget as WidgetType } from '../../../types/widget';
import { api } from '../../../lib/https';

const BbcNews = ({ widget }: Props) => {
  const [articles, setArticles] = useState<NewsResponse['data']>([]);
  const [index, setIndex] = useState(0);

  useEffect(() => {
    const interval = setInterval(() => {
      setIndex(prev => prev >= articles.length - 1 ? 0 : prev + 1);
    }, (widget.secondsBetweenArticles as number) * 1000);

    return () => {
      clearInterval(interval);
    };
  }, [articles.length]);

  const onRefresh = async () => {
    const response = await api.get<NewsResponse>(`/news?apiKey=${widget.apiKey}`);
    setArticles(response.data);
  };

  return (
    <Widget onRefresh={onRefresh} widget={widget}>
      <div>
        {articles.length > 0 ?
          articles[index].title :
          <i className='fa-solid fa-circle-notch fa-spin fa-2xl' />}
      </div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType;
}

export default BbcNews;