import { useEffect, useState } from 'react';

import Confirm from '../Modal/Confirm';
import Icon from '../Icon';
import RecipeDetails from '../Modal/RecipeDetails';
import RecipeEditor from '../Modal/RecipeEditor';
import { CookbookApiResponse, Recipe } from '../../types/food-planner';
import { http } from '../../lib/https';
import { sessionStorage } from '../../lib/session-storage';
import { useModalStack } from '../ModalStack';

import './index.scss';

const RecipeManager = () => {
  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [filteredRecipes, setFilteredRecipes] = useState<Recipe[]>([]);
  const [search, setSearch] = useState('');
  const modalstack = useModalStack();
  const config = sessionStorage.getDashboardConfig();

  useEffect(() => {
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

  const fetchRecipes = async () => {
    setRecipes([]);
    setFilteredRecipes([]);
    const response = await http.get<CookbookApiResponse>(`/food-planner/cookbook?id=${config.id}`);
    const returnedRecipes = response.data.sort((a, b) => a.name.localeCompare(b.name));
    setRecipes(returnedRecipes);
    setFilteredRecipes(returnedRecipes);
  };

  const onChangeSearch = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearch(e.target.value);
  };

  const onAddRecipe = () => {
    modalstack.open(RecipeEditor, {
      recipe: undefined,
      title: 'Add New Recipe',
    });
  };

  const onEditRecipe = (recipe: Recipe) => {
    modalstack.open(RecipeEditor, {
      recipe,
      title: `Edit ${recipe.name}`,
    });
  };

  const onViewRecipe = (recipe: Recipe) => {
    modalstack.open(RecipeDetails, {
      recipe,
      title: recipe.name,
    });
  };

  const onDeleteRecipe = async (recipe: Recipe) => {
    modalstack.open(Confirm, {
      message: `Delete ${recipe.name}?`,
      onYes: async () => {
        await http.delete<CookbookApiResponse>(`/food-planner/cookbook?id=${config.id}&recipeName=${recipe.name}`);
        await fetchRecipes();
      },
      title: 'Warning',
    });
  };

  return (
    <div className='recipe-manager'>
      <h1>Recipe Manager</h1>

      <div className='recipe-manager-controls'>
        <input onChange={onChangeSearch} placeholder='Search...' />

        <div className='recipe-manager-controls-spacer' />

        <span onClick={onAddRecipe}><Icon name='add' /></span>

        <span onClick={fetchRecipes}><Icon name='refresh' /></span>
      </div>

      <div className='recipe-manager-recipes'>
        {filteredRecipes.map((recipe, i) => (
          <div className='recipe-manager-recipe' key={i}>
            <div className='recipe-manager-recipe-title'>
              {recipe.name}
            </div>

            <div className='recipe-manager-recipe-buttons'>
              <div onClick={() => onEditRecipe(recipe)}>
                <Icon name='edit' size='xl' />
              </div>

              <div onClick={() => onViewRecipe(recipe)}>
                <Icon
                  name='eye'
                  size='xl'
                  style='regular'
                />
              </div>

              <div onClick={() => onDeleteRecipe(recipe)}>
                <Icon name='trash' size='xl' />
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default RecipeManager;