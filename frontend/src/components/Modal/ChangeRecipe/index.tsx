import { useEffect, useState } from 'react';

import { CookbookApiResponse, Recipe } from '../../../types/food-planner';
import { api } from '../../../lib/https';
import { sessionStorage } from '../../../lib/session-storage';

import './index.scss';

const ChangeRecipe = ({ day, recipe }: Props) => {
  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [selectedRecipe, setSelectedRecipe] = useState(recipe);

  useEffect(() => {
    const fetchRecipes = async () => {
      const config = sessionStorage.getDashboardConfig();
      const response = await api.get<CookbookApiResponse>(`/food-planner/cookbook?id=${config.id}`);
      setRecipes(response.data.sort((a, b) => a.name.localeCompare(b.name)));
    };

    fetchRecipes();
  }, []);

  const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSelectedRecipe(e.target.value);
  };

  return (
    <div className='change-recipe'>
      <div className='change-recipe-control'>
        <input
          onChange={(e) => onChange(e)}
          placeholder='Something else?'
          value={selectedRecipe}
        />

        <button>
          Save
        </button>
      </div>

      <div className='change-recipe-recipes'>
        {recipes.map((recipe, i) => (
          <div className='change-recipe-item' key={i}>
            <button>
              <i className='fa-solid fa-utensils' />
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