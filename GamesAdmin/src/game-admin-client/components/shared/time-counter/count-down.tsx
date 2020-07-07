import React from 'react';
import styles from './time-counter.module.scss';

type Props = {
  remainingTime: number
  placingBetTime: number
  lockingBetTime: number
};

type State = {
  currentTime: number
};

export default class TimeCounter extends React.Component<Props, State> {
  private interval: NodeJS.Timeout;
  private listeners: Array<Function>;
  constructor(props) {
    super(props);

    this.listeners = [];
    this.state = {
      currentTime: this.props.remainingTime,
    };
  }

  componentDidMount() {
    this.interval = setInterval(() => {
      if (this.state.currentTime > 0) {
        this.listeners.forEach(listenner => listenner(this.state.currentTime - 1));
        this.setState({currentTime: this.state.currentTime - 1});
      }
    }, 1000);
  }

  public setTime(time: number) {
    this.setState({currentTime: time});
  }

  public onCountDown(handler: (time: number) => void) {
    if (handler) {
      this.listeners.push(handler);
    }
  }

  componentWillUnmount() {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }


  render() {
    const { currentTime } = this.state;
    const isInBlockBetTime = currentTime <= this.props.lockingBetTime;
    const zeroPad = (num: number, places: number) => String(num).padStart(places, '0');
    const time = currentTime % (this.props.placingBetTime + 1) || 0;

    return (
      <div className={styles.timeCounter} >
        <span className={`${isInBlockBetTime ? styles.warning : ''}`}>
          {zeroPad(time, 2)}
        </span>
      </div>
    );
  }
}
