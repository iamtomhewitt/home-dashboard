import { useState } from 'react';

import { CookbookApiResponse, Recipe, RecipeIngredient } from '../../../types/food-planner';
import { http } from '../../../lib/https';
import { sessionStorage } from '../../../lib/session-storage';

import './index.scss';

const RecipeEditor = ({ isEditing = false, recipe }: Props) => {
  const [name, setName] = useState(recipe?.name || '');
  const [ingredients, setIngredients] = useState<RecipeIngredient[]>(recipe?.ingredients || []);
  const [steps, setSteps] = useState<string[]>(recipe?.steps || []);
  const [isLoading, setIsLoading] = useState(false)

  const onChangeIngredient = (index: number, key: keyof RecipeIngredient, value: any) => {
    setIngredients(prev => {
      const updated = [...prev];
      updated[index] = {
        ...updated[index],
        [key]: value,
      };
      return updated;
    });
  };

  const onChangeStep = (index: number, value: string) => {
    setSteps(prev => {
      const updated = [...prev];
      updated[index] = value;
      return updated;
    });
  };

  const onAddIngredient = () => {
    setIngredients(prev => [
      ...prev,
      {
        amount: 0,
        name: 'New Ingredient',
        weight: 'grams',
      },
    ]);
  };

  const onAddStep = () => {
    setSteps(prev => [...prev, 'New step']);
  };

  const onSave = async () => {
    setIsLoading(true)
    const dashboardConfig = sessionStorage.getDashboardConfig();
    await http.put<CookbookApiResponse>(`/food-planner/cookbook?id=${dashboardConfig.id}`, {
      ingredients,
      name,
      steps,
    });

    setIsLoading(false)
  };

  return (
    <div className='recipe-editor'>
      <div className='recipe-editor-ingredients'>
        <h4>Ingredients</h4>

        {isEditing && <input onChange={(e) => setName(e.target.value)} value={name} />}

        <div>
          {ingredients.map((ingredient, i) => (
            <div className='recipe-editor-ingredient' key={i}>

              <input
                onChange={(e) => onChangeIngredient(i, 'name', e.target.value)}
                type='text'
                value={ingredient.name}
              />

              <input
                onChange={(e) => onChangeIngredient(i, 'amount', Number(e.target.value))}
                type='number'
                value={ingredient.amount}
              />

              <select onChange={(e) => onChangeIngredient(i, 'weight', e.target.value)} value={ingredient.weight}>
                <option value='grams'>grams</option>

                <option value='quantity'>quantity</option>

                <option value='tablespoon'>tablespoon</option>

                <option value='teaspoon'>teaspoon</option>
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
            <textarea
              className='recipe-editor-step'
              key={i}
              onChange={(e) => onChangeStep(i, e.target.value)}
              value={step}
            />
          ))}
        </div>

        <button onClick={onAddStep}>
          Add Step
        </button>
      </div>

      <button disabled={isLoading} onClick={onSave}>
        {isLoading ? 'Loading...' : 'Save'}
      </button>
    </div>
  );
};

type Props = {
  isEditing?: boolean;
  recipe?: Recipe;
}

export default RecipeEditor;