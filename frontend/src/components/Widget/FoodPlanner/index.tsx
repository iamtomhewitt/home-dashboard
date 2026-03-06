import { useState } from 'react';

import ChangeRecipe from '../../Modal/ChangeRecipe';
import Confirm from '../../Modal/Confirm';
import Widget from '../';
import { FoodPlan, FoodPlannerApiResponse } from '../../../types/food-planner';
import { Widget as WidgetType } from '../../../types/widget';
import { http } from '../../../lib/https';
import { sessionStorage } from '../../../lib/session-storage';

import './index.scss';

const FoodPlanner = ({ widget }: Props) => {
  const [plan, setPlan] = useState<FoodPlan | null>();

  const onAddToShoppingList = () => {
    PubSub.publish('show-modal', {
      component: (
        <Confirm
          message='Add all ingredients to Shopping List?'
          onNo={() => { }}
          onYes={() => { }}
        />
      ),
      onClose: () => onRefresh,
      title: 'Add to Shopping List',
    });
  };

  const onChangeDay = (day: string, value: string) => {
    PubSub.publish('show-modal', {
      component: <ChangeRecipe day={day} recipe={value} />,
      onClose: () => onRefresh,
      title: `Change ${day}`,
    });
  };

  const onRefresh = async () => {
    const { id } = sessionStorage.getDashboardConfig();
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
            onClick={() => onChangeDay(day, value)}
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

        <button
          onClick={onAddToShoppingList}
          style={{
            backgroundColor: widget.colour,
          }}
        >
          Add to Shopping List
        </button>
      </div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default FoodPlanner;