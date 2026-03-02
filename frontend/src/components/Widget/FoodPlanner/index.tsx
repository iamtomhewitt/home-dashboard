import { useState } from 'react';

import Widget from '../';
import { FoodPlan, FoodPlannerApiResponse } from '../../../types/food-planner';
import { Widget as WidgetType } from '../../../types/widget';
import { api } from '../../../lib/https';

import './index.scss';

const FoodPlanner = ({ widget }: Props) => {
  console.log(widget);
  const [plan, setPlan] = useState<FoodPlan | null>();

  const onRefresh = async () => {
    const response = await api.get<FoodPlannerApiResponse>('/food-planner/planner?id=fc66bb59002b3866e04f4bcd73fe347c');
    setPlan(response.data);
  };

  return (
    <Widget
      height={10}
      onRefresh={onRefresh}
      widget={widget}
      width={6}
    >
      <div className='food-planner'>
        {plan && Object.entries(plan).map(([day, value]) => (
          <div className='food-planner-card' key={day}>
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
      </div>
    </Widget>
  );
};

type Props = {
  widget: WidgetType,
}

export default FoodPlanner;