
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
      <ul className='todoist'>
        {tasks.map((task, i) => (
          <li key={i}>{task}</li>
        ))}
      </ul>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default Todoist;