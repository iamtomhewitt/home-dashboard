import { useState } from 'react';

import AddTodoistTask from '../../Modal/AddTodoistTask';
import Icon from '../../Icon';
import Widget from '../';
import { TodoistApiResponse, TodoistItem } from '../../../types/todoist';
import { Widget as WidgetType } from '../../../types/widget';
import { http } from '../../../lib/https';
import { useModalStack } from '../../ModalStack';

import './index.scss';

const Todoist = ({ widget }: Props) => {
  const { apiKey, colour, title, todoistId } = widget;
  const [tasks, setTasks] = useState<TodoistItem[]>([]);
  const modalstack = useModalStack();

  const fetchTasks = async () => {
    const response = await http.get<TodoistApiResponse>(`/todoist?apiKey=${apiKey}&projectId=${todoistId}`);
    setTasks(response.data);
  };

  const onAddTask = () => {
    modalstack.open(AddTodoistTask, {
      apiKey,
      projectId: todoistId,
      title: `Add to ${title}`, 
    });
  };

  const onDeleteTask = async (task: TodoistItem) => {
    await http.delete(`/todoist?apiKey=${apiKey}&projectId=${todoistId}&id=${task.id}`);
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
            <Icon name='xmark' />

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