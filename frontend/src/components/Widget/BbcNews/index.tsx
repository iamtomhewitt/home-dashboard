import { useEffect, useState } from 'react';

import Icon from '../../Icon';
import Widget from '../';
import { NewsApiResponse, NewsItem } from '../../../types/news';
import { Widget as WidgetType } from '../../../types/widget';
import { http } from '../../../lib/https';

import './index.scss';

const BbcNews = ({ widget }: Props) => {
  const [articles, setArticles] = useState<NewsItem[]>([]);
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
    const response = await http.get<NewsApiResponse>(`/news?apiKey=${widget.apiKey}`);
    setArticles(response.data);
  };

  return (
    <Widget onRefresh={onRefresh} widget={widget}>
      <div className='bbc-news'>
        {articles.length > 0 ?
          articles[index].title :
          <Icon
            animation='spin'
            name='circle-notch'
            size='2xl'
          />}
      </div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType;
}

export default BbcNews;