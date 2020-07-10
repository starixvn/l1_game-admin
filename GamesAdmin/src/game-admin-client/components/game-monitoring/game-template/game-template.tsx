import React from 'react';
import styles from './game-template.module.scss';
import { Notice } from '../../shared/notice/notice';

type Props = {
    renderMonitor: React.ReactElement,
    renderBetList: React.ReactElement,
    disabled: boolean
}

export default function GameTemplate(props: Props) {
    const { renderMonitor, renderBetList, disabled } = props;

    return (
        <div className={styles['game-template']}>
            <div className={styles['monitor']}>
                <div className={styles['background-img']}></div>
                {renderMonitor}
            </div>
            <div className={`${styles['bet-list']}`}>
                {renderBetList}
            </div>
            {disabled && <Notice isOpen={true} message="Game is disabled." />}
        </div>
    )
}