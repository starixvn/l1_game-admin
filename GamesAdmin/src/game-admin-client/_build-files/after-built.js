const fs = require('fs-extra');
const rimraf = require('rimraf');
const movingItems = [
  { from: './out/_next', to: './out/game-admin/_next' },
  { from: './out/404.html', to: './out/game-admin/404.html' },
  { from: './out/monitor.html', to: './out/game-admin/monitor.html' },
  { from: './out/game-admin', to: '../GamesAdmin.Site/wwwroot/game-admin' }
];

rimraf('../GamesAdmin.Site/wwwroot/game-admin', () => {
  movingItems.forEach(async movingItem => {
    await fs.moveSync(movingItem.from, movingItem.to);
    console.log(`Moved from ${movingItem.from} to ${movingItem.to}`);
  });
});
