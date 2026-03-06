import ChangeRecipe from "../ChangeRecipe"


const RecipeDetails = ({ day, recipe }: Props) => {

  const onClose = () => {
    PubSub.publish('show-modal', {
      component: <ChangeRecipe day={day} recipe={recipe} />,
      title: recipe
    })
  }

  return (
    <div>
      {recipe}
    </div>
  )
}

type Props = {
  onClose?: () => void;
  day: string;
  recipe: string;
}

export default RecipeDetails