import { useState } from 'react';

import Icon from '../../Icon';
import { CookbookApiResponse, Recipe, RecipeIngredient } from '../../../types/food-planner';
import { api } from '../../../lib/api';
import { dashboard } from '../../../lib/dashboard';

import './index.scss';

const RecipeEditor = ({ recipe }: Props) => {
  const [name, setName] = useState(recipe?.name || '');
  const [ingredients, setIngredients] = useState<RecipeIngredient[]>(recipe?.ingredients || []);
  const [steps, setSteps] = useState<string[]>(recipe?.steps || []);
  const [isLoading, setIsLoading] = useState(false);
  const [message, setMessage] = useState('');

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

  const onRemoveIngredient = (name: string) => {
    setIngredients(ingredients.filter(x => x.name !== name));
  };

  const onAddStep = () => {
    setSteps(prev => [...prev, 'New step']);
  };

  const onSave = async () => {
    setMessage('');
    setIsLoading(true);
    const dashboardConfig = dashboard.getConfig();
    const response = await api.put<CookbookApiResponse>(`/food-planner/cookbook?id=${dashboardConfig.id}`, {
      ingredients,
      name,
      steps,
    });

    setMessage(response.message);
    setIsLoading(false);
  };

  return (
    <div className='recipe-editor'>
      <div className='recipe-editor-ingredients'>
        <input
          className='recipe-editor-name'
          onChange={(e) => setName(e.target.value)}
          placeholder='Recipe name'
          value={name}
        />

        <h4>Ingredients</h4>

        <div>
          {ingredients.map((ingredient, i) => (
            <div className='recipe-editor-ingredient' key={i}>
              <span onClick={() => onRemoveIngredient(ingredient.name)}>
                <Icon name='xmark' />
              </span>

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

      <div className='recipe-editor-message'>{message}</div>
    </div>
  );
};

type Props = {
  recipe?: Recipe;
}

export default RecipeEditor;