
const AddTodoistTask = ({ projectId }: Props) => (
  <div>
    Add Task to {projectId}
  </div>
);

type Props = {
  projectId: string;
}

export default AddTodoistTask;