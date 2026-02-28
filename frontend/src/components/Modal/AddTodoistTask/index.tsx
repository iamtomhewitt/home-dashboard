import { useState } from 'react';

import { ApiResponse } from '../../../types/api';
import { api } from '../../../lib/https';

import './index.scss';

const AddTodoistTask = ({ apiKey, projectId }: Props) => {
  const [name, setName] = useState('');
  const [message, setMessage] = useState('');

  const onAdd = async () => {
    const response = await api.post<ApiResponse<null>>(`/todoist?apiKey=${apiKey}&projectId=${projectId}`, {
      content: name,
    });

    setMessage(response.message);
  };

  return (
    <div className='add-todoist-task'>
      <input
        id='name'
        onChange={(e) => setName(e.target.value)}
        value={name}
      />

      <button disabled={!name} onClick={onAdd}>
        Save
      </button>

      <div className='add-todoist-task-message'>
        {message}
      </div>

      <br />
    </div>
  );
};

type Props = {
  apiKey: string;
  projectId: string;
}

export default AddTodoistTask;