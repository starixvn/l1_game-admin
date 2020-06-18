import React from 'react';
import KenoBase from './base';

type Props = {
  onMessage: Function
  sendMessage: Function
  isProduction: boolean
};

export default (gameName: string, sizeOfNumbers: number) => (props: Props) => {
  return <KenoBase
    gameName={gameName}
    sizeOfNumbers={sizeOfNumbers || 10}
    isMini={gameName.toLowerCase().indexOf('mini') !== -1}
    {...props}/>
}
