import { useEffect, useState } from 'react';

import LoadingIcon from '../../Icons/Loading';
import Widget from '../';
import { Widget as WidgetType } from '../../../types/widget';
import { http } from '../../../lib/https';

const BbcNews = ({ widget }: Props) => {
  const [articles, setArticles] = useState<Article[]>([]);
  const [index, setIndex] = useState(0);

  useEffect(() => {
    const fetchArticles = async () => {
      const url = `https://newsapi.org/v2/top-headlines?sources=bbc-news&apiKey=${widget.apiKey}`;
      const response = await http.get<any>(url);
      setArticles(response.articles);
    };

    fetchArticles();
  }, []);

  useEffect(() => {
    if (articles.length === 0) {
      return;
    }

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

type Article = {
  author: string;
  content: string;
  description: string;
  publishedAt: string;
  title: string;
  url: string;
  urlToImage: string;
  source: {
    id: string;
    name: string;
  },
}

type Props = {
  widget: WidgetType;
}

export default BbcNews;