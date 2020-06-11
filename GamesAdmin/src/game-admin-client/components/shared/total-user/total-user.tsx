import React from 'react';
import styles from './total-user.module.scss';

export default function TotalUser(props) {
  const { changeLanguage, totalUser } = props;

  return (
    <div className={styles.totalUser}>
      <span className={styles.icon} />
      {totalUser}
    </div>
  );
}
