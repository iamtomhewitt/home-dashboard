import { useEffect, useState } from 'react';

import './index.scss';

const ChangeRecipe = ({ day, recipe }: Props) => {
  const [recipes, setRecipes] = useState([]);
  const [selectedRecipe, setSelectedRecipe] = useState(recipe);

  useEffect(() => {
    console.log('make a request to the cookbook to fetch recipes');
  }, []);

  const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSelectedRecipe(e.target.value);
  };

  return (
    <div className='change-recipe'>
      <input
        onChange={(e) => onChange(e)}
        placeholder='Something else?'
        value={selectedRecipe}
      />

      <div>
        {recipes.map((recipe, i) => (
          <div key={i}>{recipe}</div>
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