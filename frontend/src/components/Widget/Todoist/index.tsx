import { useState } from 'react';
import PubSub from 'pubsub-js';

import AddTodoistTask from '../../Modal/AddTodoistTask';
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

  const onAddTask = () => {
    PubSub.publish('show-modal', {
      component: <AddTodoistTask projectId={widget.todoistId} />,
      title: `Add to ${widget.title}`,
    });
  };

  const onDeleteTask = async (task: TodoistResponse['data'][number]) => {
    await api.delete(`/todoist?apiKey=${widget.apiKey}&projectId=${widget.todoistId}&id=${task.id}`);
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
            backgroundColor: widget.colour,
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