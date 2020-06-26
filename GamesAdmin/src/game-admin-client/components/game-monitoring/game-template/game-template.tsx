import React from 'react';
import styles from './game-template.module.scss';

export default function GameTemplate(props) {
    const { renderMonitor, renderBetList } = props;

    return (
        <div className={styles['game-template']}>
            <div className={styles['monitor']}>
                <div className={styles['background-img']}></div>
                {renderMonitor}
            </div>
            <div className={`${styles['bet-list']}`}>
                {renderBetList}
            </div>
        </div>
    )
}