import React from 'react';
import Head from 'next/head';

type Props = {
  title: string,
  icon: string
};

export default function AppHead(props: Props) {
  const { title, icon } = props;
  return (
    <Head>
      <title>{title}</title>
      <link rel="icon" href={icon} />
    </Head>
  );
}
