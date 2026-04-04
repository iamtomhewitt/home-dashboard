import { useEffect, useState } from 'react';

import Icon from '../../Icon';
import Widget from '../';
import { NewsApiResponse, NewsItem } from '../../../types/news';
import { Widget as WidgetType } from '../../../types/widget';
import { api } from '../../../lib/api';

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
    const response = await api.get<NewsApiResponse>(`/news?apiKey=${widget.apiKey}`);
    setArticles(response.data);
  };

  const onClick = (article: NewsItem) => {
    window.open(article.url, '_blank');
  };

  return (
    <Widget onRefresh={onRefresh} widget={widget}>
      <div className='bbc-news'>
        {articles.length > 0 ?
          <span onClick={() => onClick(articles[index])}>
            {articles[index].title}
          </span> :
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