import React from 'react';
import styles from './styles.module.scss';

export default function Title(props: { title: string }) {
    const { title } = props;
    return (<div className={styles.title}>{title}</div>)
}