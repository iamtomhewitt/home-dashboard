import { useEffect, useState } from 'react';
import PubSub from 'pubsub-js';
import Icon from '../../Icon';
import { CookbookApiResponse, Recipe } from '../../../types/food-planner';
import { http } from '../../../lib/https';
import { sessionStorage } from '../../../lib/session-storage';

import './index.scss';
import RecipeDetails from '../RecipeDetails';

const ChangeRecipe = ({ day, recipe }: Props) => {
  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [selectedRecipe, setSelectedRecipe] = useState('');

  useEffect(() => {
    const fetchRecipes = async () => {
      const config = sessionStorage.getDashboardConfig();
      const response = await http.get<CookbookApiResponse>(`/food-planner/cookbook?id=${config.id}`);
      setRecipes(response.data.sort((a, b) => a.name.localeCompare(b.name)));
    };

    fetchRecipes();
  }, []);

  const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSelectedRecipe(e.target.value);
  };

  const onShowRecipeDetails = () => {
    PubSub.publish('show-modal', {
      component: <RecipeDetails day={day} recipe={recipe} />,
      onClose: () => () => PubSub.publish('show-modal', {
        component: <ChangeRecipe day={day} recipe={recipe} />,
        title: `Change ${day}`
      }),
      title: `Recipe Details`
    })
  }

  return (
    <div className='change-recipe'>
      <div className='change-recipe-control'>
        <input
          onChange={onChange}
          placeholder='Something else?'
          value={selectedRecipe}
        />

        <button>
          Save
        </button>
      </div>

      <div className='change-recipe-recipes'>
        {recipes.length === 0 && (
          <Icon
            animation='spin'
            name='circle-notch'
            size='2xl'
          />
        )}

        {recipes.map((recipe, i) => (
          <div className='change-recipe-item' key={i}>
            <button onClick={onShowRecipeDetails}>
              <Icon name='utensils' />
            </button>

            <span>{recipe.name}</span>
          </div>
        ))}
      </div>
    </div>
  );
};

type Props = {
  day: string;
  recipe: string;
}

export default ChangeRecipe;