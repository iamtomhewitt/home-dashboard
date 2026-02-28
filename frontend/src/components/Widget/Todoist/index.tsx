import { useState } from 'react';
import PubSub from 'pubsub-js';

import AddTodoistTask from '../../Modal/AddTodoistTask';
import Widget from '../';
import { TodoistApiResponse, TodoistItem } from '../../../types/todoist';
import { Widget as WidgetType } from '../../../types/widget';
import { api } from '../../../lib/https';

import './index.scss';

const Todoist = ({ widget }: Props) => {
  const { apiKey, colour, title, todoistId } = widget;
  const [tasks, setTasks] = useState<TodoistItem[]>([]);

  const fetchTasks = async () => {
    const response = await api.get<TodoistApiResponse>(`/todoist?apiKey=${apiKey}&projectId=${todoistId}`);
    setTasks(response.data);
  };

  const onAddTask = () => {
    PubSub.publish('show-modal', {
      component: <AddTodoistTask apiKey={apiKey} projectId={todoistId} />,
      onClose: () => fetchTasks,
      title: `Add to ${title}`,
    });
  };

  const onDeleteTask = async (task: TodoistItem) => {
    await api.delete(`/todoist?apiKey=${apiKey}&projectId=${todoistId}&id=${task.id}`);
    await fetchTasks();
  };

  return (
    <Widget onRefresh={fetchTasks} widget={widget}>
      <div className='todoist'>
        {tasks.map((task, i) => (
          <div
            className='todoist-item'
            key={i}
            onClick={() => onDeleteTask(task)}
          >
            <i className='fa-solid fa-xmark' />

            <div>{task.name}</div>
          </div>
        ))}

        <button
          onClick={onAddTask}
          style={{
            backgroundColor: colour,
          }}
        >
          Add
        </button>
      </div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default Todoist;