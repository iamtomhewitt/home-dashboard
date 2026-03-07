import { Recipe } from '../../../types/food-planner';

const RecipeDetails = ({ recipe }: Props) => {
  return (
    <div>
      {recipe.name}
    </div>
  );
};

type Props = {
  recipe: Recipe;
}

export default RecipeDetails;