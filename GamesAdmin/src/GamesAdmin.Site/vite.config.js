import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import * as path from "path";

export default defineConfig({
    plugins: [react()],
    resolve: {
        extensions: [".tsx", ".ts", ".jsx", ".js", ".css"],
    },
    build: {
        rollupOptions: {
            input: {
                ["bet-limit-suggestion"]: path.resolve(__dirname, "wwwroot/js/bet-limit-suggestion/index.tsx"),
            },
            output: {
                entryFileNames: "[name].min.js",
                dir: "wwwroot/js/bet-limit-suggestion/bundles",
                format: "es",
            },
        },
    },
});
