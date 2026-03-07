import { Recipe } from '../../../types/food-planner';

import './index.scss';

const RecipeDetails = ({ recipe }: Props) => {
  const { ingredients = [], steps = [] } = recipe;

  return (
    <div className='recipe-details'>
      <h4>Ingredients</h4>

      <ul>
        {ingredients.map((ingredient, i) => {
          const label = `${ingredient.amount} ${ingredient.weight} ${ingredient.name}`.replace(' quantity', ' ');
          return <li key={i}>{label}</li>;
        })}
      </ul>

      {steps.length > 0 && (
        <>
          <h4>Steps</h4>

          <ol>{steps.map((step, i) => <li key={i}>{step}</li>)}</ol>
        </>
      )}
    </div>
  );
};

type Props = {
  recipe: Recipe;
}

export default RecipeDetails;