import React from 'react';
import renderer from 'react-test-renderer';
import TotalBet from './total-bet';

test('Render Component', () => {
  const component = renderer.create(<TotalBet type='tiger' total={1} />);
  const tree = component.toJSON();
  expect(tree).toMatchSnapshot();
});
