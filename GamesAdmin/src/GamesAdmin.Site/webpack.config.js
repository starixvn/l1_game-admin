const path = require("path");

const commonConfig = {
    // TODO: Add common Configuration
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: ["babel-loader"],
            },
            {
                test: /\.(ts|tsx)$/,
                exclude: /node_modules/,
                use: [
                    {
                        loader: "ts-loader",
                        options: {
                            configFile: "tsconfig.webpack.json",
                        },
                    },
                ],
            },
            {
                test: /\.css$/,
                exclude: /node_modules/,
                use: ["style-loader", "css-loader"],
            },
        ],
    },
    resolve: {
        extensions: [".tsx", ".ts", ".jsx", ".js", ".css"],
    },
    watch: true,
};

var lobbyLayoutConfig = Object.assign({}, commonConfig, {
    name: "lobbyLayoutConfig",
    entry: "./wwwroot/js/lobby/index.tsx",
    output: {
        filename: "lobby.min.js",
        path: path.resolve(__dirname, "wwwroot/js/lobby/bundles"),
    },
});

module.exports = [lobbyLayoutConfig];
