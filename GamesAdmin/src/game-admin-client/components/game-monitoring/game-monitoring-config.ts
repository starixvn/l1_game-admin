const loadOrderFromConfigString = (config: string) => {
  if (config) {
    return config.split(',');
  }

  return null;
}

export const types = {
  taixiu: "taixiu",
  chanle: "chanle",
  clturbo: "clturbo"
};

export const gameMapping = {
  taixiu: loadOrderFromConfigString(process.env.NEXT_PUBLIC_TAIXIU_ORDER) || ['tai', 'xiu'],
  chanle: loadOrderFromConfigString(process.env.NEXT_PUBLIC_CHANLE_ORDER) || ['chan', 'le'],
  clturbo: loadOrderFromConfigString(process.env.NEXT_PUBLIC_CHANLE_ORDER) || ['chan', 'le'],
};

export const gameFlagMapping = {
  taixiu: {
    0: 'xiu',
    1: 'tai',
  },
  chanle: {
    0: 'le',
    1: 'chan',
  },
  clturbo: {
    0: 'le',
    1: 'chan',
  },
};
