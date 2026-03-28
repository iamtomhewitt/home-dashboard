import { useState } from 'react';

import { Recipe, RecipeIngredient } from '../../../types/food-planner';

import './index.scss';

const RecipeEditor = ({ recipe }: Props) => {
  const [ingredients, setIngredients] = useState<RecipeIngredient[]>(recipe?.ingredients || []);
  const [steps, setSteps] = useState(recipe?.steps || []);

  const onAddIngredient = () => {
    setIngredients(prev => [
      ...prev, {
        amount: 0,
        name: 'New Ingredient',
        weight: 'grams',
      },
    ]);
  };

  const onAddStep = () => {
    setSteps(prev => [
      ...prev,
      'New step',
    ]);
  };

  return (
    <div className='recipe-editor'>

      <div className='recipe-editor-ingredients'>
        <h4>Ingredients</h4>

        <div>
          {ingredients.map((ingredient, i) => (
            <div className='recipe-editor-ingredient' key={i}>
              <div>{ingredient.name}</div>

              <input type='number' value={ingredient.amount} />

              <select>
                <option selected={ingredient.weight === 'grams'}>grams</option>

                <option selected={ingredient.weight === 'quantity'}>quantity</option>

                <option selected={ingredient.weight === 'tablespoon'}>tablespoon</option>

                <option selected={ingredient.weight === 'teaspoon'}>teaspoon</option>
              </select>
            </div>
          ))}
        </div>

        <button onClick={onAddIngredient}>
          Add Ingredient
        </button>
      </div>

      <div className='recipe-editor-steps'>
        <h4>Steps</h4>

        <div>
          {steps.map((step, i) => (
            <textarea className='recipe-editor-step' key={i}>{step}</textarea>
          ))}
        </div>

        <button onClick={onAddStep}>
          Add Step
        </button>

      </div>

      <button>
        Save
      </button>
    </div>
  );
};

type Props = {
  recipe?: Recipe;
}

export default RecipeEditor;