import { createLogger } from 'winston';
import BrowserConsole from 'winston-transport-browserconsole';
import * as Sentry from '@sentry/browser';
import SentryTransport from './sentry-transport';

Sentry.init({
  dsn: process.env.NEXT_PUBLIC_SENTRY_DSN,
});

const options = {
  console: {
    level: process.env.NEXT_PUBLIC_CONSOLE_LOG_LEVEL,
    handleExceptions: true,
  },
  sentry: {
    level: process.env.NEXT_PUBLIC_SENTRY_LOG_LEVEL,
  },
  winston: {
    exitOnError: false,
  },
};

const logger = createLogger(options.winston);

logger.add(new BrowserConsole(options.console));
logger.add(new SentryTransport(Sentry, options.sentry));

export default logger;
