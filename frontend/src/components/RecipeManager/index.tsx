import { useEffect, useState } from 'react';

import Icon from '../Icon';
import { CookbookApiResponse, Recipe } from '../../types/food-planner';
import { http } from '../../lib/https';
import { sessionStorage } from '../../lib/session-storage';

import './index.scss';

const RecipeManager = () => {
  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [filteredRecipes, setFilteredRecipes] = useState<Recipe[]>([]);
  const [search, setSearch] = useState('');

  useEffect(() => {
    const config = sessionStorage.getDashboardConfig();

    const fetchRecipes = async () => {
      const response = await http.get<CookbookApiResponse>(`/food-planner/cookbook?id=${config.id}`);
      const returnedRecipes = response.data.sort((a, b) => a.name.localeCompare(b.name));
      setRecipes(returnedRecipes);
      setFilteredRecipes(returnedRecipes);
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

  useEffect(() => {
    const filtered = recipes.filter(recipe => recipe.name.toLowerCase().includes(search.toLowerCase()));
    setFilteredRecipes(filtered);
  }, [search]);

  const onChangeSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value);
  };

  return (
    <div className='recipe-manager'>
      <h1>Recipe Manager</h1>

      <div className='recipe-manager-controls'>
        <input onChange={onChangeSearch} placeholder='Search...' />

        <Icon name='add' />

        <Icon name='refresh' />
      </div>

      <div className='recipe-manager-recipes'>
        {filteredRecipes.map((recipe, i) => (
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