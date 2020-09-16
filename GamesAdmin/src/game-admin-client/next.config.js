module.exports = {
  webpack: (config, { isServer }) => {
    if (!isServer) {
      config.node = {
        fs: "empty",
      };
    }
    return config;
  },
  assetPrefix: "./",
  exportPathMap: async function (
    defaultPathMap,
    { dev, dir, outDir, distDir, buildId }
  ) {
    return {
      "/monitor": { page: "/" },
      "/xocxoc": { page: "/monitoring", query: { game: "xocxoc" } },
      "/vipsedie": { page: "/monitoring", query: { game: "vipsedie" } },
    };
  },
};
