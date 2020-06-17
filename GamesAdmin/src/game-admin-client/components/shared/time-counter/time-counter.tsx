import React from 'react';
import styles from './time-counter.module.scss';
import { GameState } from '../common/GameState';

type timeCounterProps = {
  remainingTime: number,
  gameState?: number
}

export default class TimeCounter extends React.Component<timeCounterProps, { currentTime: number, gameState?: number }> {
  constructor(props) {
    super(props);

    this.state = {
      currentTime: props.remainingTime,
      gameState: props.gameState
    }
  }

  interval: any;

  componentDidMount() {
    if (this.state.gameState != undefined) {
      this.interval = setInterval(() => {
        if (this.state.currentTime > 0) {
          this.setState({
            currentTime: this.state.currentTime - 1
          });
        }
      }, 1000);
    }
  }

  static getDerivedStateFromProps(props, state) {
    if (state.gameState == undefined || props.gameState != state.gameState) {
      return {
        currentTime: props.remainingTime,
        gameState: props.gameState
      }
    }

    return null;
  }

  componentWillUnmount() {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }


  render() {
    const { currentTime, gameState } = this.state;

    return (
      <div className={styles.timeCounter} >
        <span className={`${currentTime > 5 || gameState != GameState.running ? '' : styles.warning}`}>
          {
            currentTime >= 10 ? currentTime : '0' + currentTime
          }
        </span>
      </div>
    );

  }
}
