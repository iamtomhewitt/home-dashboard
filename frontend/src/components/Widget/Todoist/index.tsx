
import { useState } from 'react';

import Widget from '../';
import { TodoistResponse } from '../../../types/lambda';
import { Widget as WidgetType } from '../../../types/widget';
import { api } from '../../../lib/https';

import './index.scss';

const Todoist = ({ widget }: Props) => {
  const [tasks, setTasks] = useState<TodoistResponse['data']>([]);

  const fetchTasks = async () => {
    const response = await api.get<TodoistResponse>(`/todoist?apiKey=${widget.apiKey}&projectId=${widget.todoistId}`);
    setTasks(response.data);
  };

  return (
    <Widget onRefresh={fetchTasks} widget={widget}>
      <div className='todoist'>
        {tasks.map((task, i) => (
          <div className='todoist-item' key={i}>{task}</div>
        ))}
      </div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default Todoist;