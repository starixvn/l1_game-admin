const fs = require('fs-extra');

fs.copyFile('./_build-files/.env.local', './.env.local');
