import React from 'react';
import renderer from 'react-test-renderer';
import BetList from './bet-list';

test('Render Component', () => {
  const component = renderer.create(<BetList type="simple" titles={["ok"]} dataList={[{ name: "a", bet: 10 }]} />);
  const tree = component.toJSON();
  expect(tree).toMatchSnapshot();
});
