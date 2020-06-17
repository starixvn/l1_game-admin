import React from 'react';
import KenoBase from './base';

type Props = {
  onMessage: Function,
  sendMessage: Function,
};

export default (gameName: string) => (props: Props) => {
  return <KenoBase gameName={gameName} {...props}/>
}
