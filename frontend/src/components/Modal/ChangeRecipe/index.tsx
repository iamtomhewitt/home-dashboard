import { useEffect, useState } from 'react';

import Icon from '../../Icon';
import RecipeDetails from '../RecipeDetails';
import { CookbookApiResponse, Recipe } from '../../../types/food-planner';
import { api } from '../../../lib/api';
import { sessionStorage } from '../../../lib/session-storage';
import { useModalStack } from '../../ModalStack';

import './index.scss';

const ChangeRecipe = ({ day, onClose }: Props) => {
  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [selectedRecipe, setSelectedRecipe] = useState('');
  const modalstack = useModalStack();

  useEffect(() => {
    const fetchRecipes = async () => {
      const config = sessionStorage.getDashboardConfig();
      const response = await api.get<CookbookApiResponse>(`/food-planner/cookbook?id=${config.id}`);
      setRecipes(response.data.sort((a, b) => a.name.localeCompare(b.name)));
    };

    fetchRecipes();

    return () => {
      onClose();
    };
  }, []);

  const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSelectedRecipe(e.target.value);
  };

  const onShowRecipeDetails = (recipe: Recipe) => {
    modalstack.open(RecipeDetails, {
      recipe,
      title: recipe.name,
    });
  };

  const onSelectRecipe = async (recipe: string) => {
    const { id } = sessionStorage.getDashboardConfig();
    await api.put(`/food-planner/planner?id=${id}`, {
      [day]: recipe,
    });
    modalstack.close();
  };

  return (
    <div className='change-recipe'>
      <div className='change-recipe-control'>
        <input
          onChange={onChange}
          placeholder='Something else?'
          value={selectedRecipe}
        />

        <button onClick={() => onSelectRecipe(selectedRecipe)}>
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
            <button onClick={() => onShowRecipeDetails(recipe)}>
              <Icon name='utensils' />
            </button>

            <span onClick={() => onSelectRecipe(recipe.name)}>{recipe.name}</span>
          </div>
        ))}
      </div>
    </div>
  );
};

type Props = {
  day: string;
  onClose: () => void;
}

export default ChangeRecipe;