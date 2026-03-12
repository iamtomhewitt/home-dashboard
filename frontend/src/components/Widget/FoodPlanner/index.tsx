import { useState } from 'react';

import ChangeRecipe from '../../Modal/ChangeRecipe';
import Confirm from '../../Modal/Confirm';
import Widget from '../';
import { FoodPlan, FoodPlannerApiResponse, ShoppingListResponse } from '../../../types/food-planner';
import { Widget as WidgetType } from '../../../types/widget';
import { http } from '../../../lib/https';
import { sessionStorage } from '../../../lib/session-storage';
import { useModalStack } from '../../ModalStack';

import './index.scss';

const FoodPlanner = ({ widget }: Props) => {
  const [isLoading, setIsLoading] = useState(false);
  const [plan, setPlan] = useState<FoodPlan | null>();
  const modalstack = useModalStack();
  const { id } = sessionStorage.getDashboardConfig();

  const onAddToShoppingList = () => {
    modalstack.open(Confirm, {
      message: 'Add all ingredients to Shopping List?',
      onYes: async () => {
        setIsLoading(true);
        const response = await http.get<ShoppingListResponse>(`/food-planner/shoppingList?id=${id}`);
        for (const item of response.data) {
          console.log(item);
          await http.post(`/todoist?apiKey=${widget.todoist.apiKey}&projectId=${widget.todoist.id}`, {
            content: item,
          });
        }
        setIsLoading(false);
        document.location.reload();
      },
      title: 'Add to Shopping List?',
    });
  };

  const onChangeDay = (day: string) => {
    modalstack.open(ChangeRecipe, {
      day,
      title: 'Select Recipe',
      onClose: onRefresh,
    });
  };

  const onRefresh = async () => {
    const response = await http.get<FoodPlannerApiResponse>(`/food-planner/planner?id=${id}`);
    setPlan(response.data);
  };

  return (
    <Widget onRefresh={onRefresh} widget={widget}>
      <div className='food-planner'>
        {plan && Object.entries(plan).map(([day, value]) => (
          <div
            className='food-planner-card'
            key={day}
            onClick={() => onChangeDay(day)}
          >
            <div
              className='food-planner-card-day'
              style={{
                backgroundColor: widget.colour,
              }}
            >
              {day.substring(0, 3)}
            </div>

            <div className='food-planner-card-value'>{value}</div>
          </div>
        ))}

        {widget.todoist.id && (
          <button
            disabled={isLoading}
            onClick={onAddToShoppingList}
            style={{
              backgroundColor: widget.colour,
            }}
          >
            Add to Shopping List
          </button>
        )}
      </div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default FoodPlanner;