import React from 'react';
import renderer from 'react-test-renderer';
import Loading from './loading';

test('Render Component', () => {
  const component = renderer.create(<Loading />);
  const tree = component.toJSON();
  expect(tree).toMatchSnapshot();
});
