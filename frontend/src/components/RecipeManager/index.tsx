import { useEffect, useState } from 'react';

import Icon from '../Icon';
import { CookbookApiResponse, Recipe } from '../../types/food-planner';
import { http } from '../../lib/https';
import { sessionStorage } from '../../lib/session-storage';

import './index.scss';

const RecipeManager = () => {
  const [recipes, setRecipes] = useState<Recipe[]>([]);

  useEffect(() => {
    const config = sessionStorage.getDashboardConfig();

    const fetchRecipes = async () => {
      const response = await http.get<CookbookApiResponse>(`/food-planner/cookbook?id=${config.id}`);
      setRecipes(response.data.sort((a, b) => a.name.localeCompare(b.name)));
    };

    fetchRecipes();

    const body = document.getElementById('body');

    if (body) {
      body.style.backgroundColor = config.backgroundColour;
    }
    else {
      console.warn('Could not find element by ID \'body\'');
    }
  }, []);

  return (
    <div className='recipe-manager'>
      <h1>Recipe Manager</h1>

      <div className='recipe-manager-controls'>
        <input placeholder='Search...' />

        <Icon name='add' />

        <Icon name='refresh' />
      </div>

      <div className='recipe-manager-recipes'>
        {recipes.map((recipe, i) => (
          <div className='recipe-manager-recipe' key={i}>
            <div className='recipe-manager-recipe-title'>
              {recipe.name}
            </div>

            <div className='recipe-manager-recipe-buttons'>
              <Icon name='edit' size='xl' />

              <Icon
                name='eye'
                size='xl'
                style='regular'
              />
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default RecipeManager;