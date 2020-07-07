## Let's code
1. Clone this repository
2. `npm install`

## Scripts
| Command | Description 
| :---: | :---- |
| `npm run dev` | Run development server |
| `npm test` | Run tests |
| `npm run test:watch` | Run tests with watch mode |
| `npm run coverage` | Generate coverage report |
| `npm start` | Start server |

## Structure
```
project
│   Readme.md
│   .env                            Global environment variables   
│   .env.[environment][.local]      [Ref 1]
│   .gitifnore
│   .editorconfig
│   babel.config.js                 [Ref 2]
│   jest.config.js                  [Ref 3]
│   tsconfig.json
│   next-env.d.ts
│   package.json
│   package-lock.json
│
└───components
│   └───common
│   │   │   *.scss                  Component-Level CSS [Ref 4]
│   │   │   *.tsx                   Components
│   │   └───*.test.tsx              Unit tests
│   │
│   │
│   └───games
│   │   └─── [game name]
│   │        │   [Same as common]
│   │
│   └───hoc                         Higher Order Component [Ref 5]
│   │   └───...
│   │
│   └───util
│       └───...
│
└───pages
│   │   _app.tsx                    Custom App [Ref 6]
│   └───...
│
└───public
│
└───styles                          Global stylesheet [Ref 7]
│
└───typings                         Define missing types or extends exists types
│
└───service-configs                 Use for deployment (Systemd service) [Ref 8]
```
### References
1. [Environment Variables](https://nextjs.org/docs/basic-features/environment-variables)
2. [Babel]((https://babeljs.io/docs/en/))
3. [Jest](https://jestjs.io/docs/en/getting-started)
4. [Component-Level CSS](https://nextjs.org/docs/basic-features/built-in-css-support#adding-component-level-css)
5. [Higher Order Component](https://reactjs.org/docs/higher-order-components.html)
6. [Custom App](https://nextjs.org/docs/advanced-features/custom-app)
7. [Global stylesheet](https://nextjs.org/docs/basic-features/built-in-css-support#adding-a-global-stylesheet)
8. [Systemd Service](https://linuxconfig.org/how-to-create-systemd-service-unit-in-linux)

## Install VS Code extension (optional)
 - EditorConfig for VS Code