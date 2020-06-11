import Transport from 'winston-transport';
import _ from 'lodash';

type LevelMaps ={
  silly: string,
  verbose: string,
  info: string,
  debug: string,
  warn: string,
  error: string
}

export default class SentryTransport extends Transport {
  private sentry: any;

  private readonly levelsMap: LevelMaps;

  protected tags: { [s: string]: any };

  constructor(sentry: any, options: any) {
    super(options);
    this.sentry = sentry;
    this.tags = options.tags;
    this.levelsMap = {
      silly: 'debug',
      verbose: 'debug',
      info: 'info',
      debug: 'debug',
      warn: 'warning',
      error: 'error',
    };
  }

  log(info, next) {
    if (this.silent || !(info.level in this.levelsMap)) {
      return next(null, true);
    }

    const { level, fingerprint, message } = info;
    const meta = { ..._.omit(info, ['level', 'message', 'label']) };
    setImmediate(() => {
      this.emit('logged', level);
    });

    const context = {
      level: this.levelsMap[level],
      extra: _.omit(meta, ['user']),
      fingerprint: [fingerprint, process.env.NEXT_PUBLIC_ENV],
    };

    this.sentry.configureScope((scope) => {
      const user = _.get(meta, 'user');

      if (_.has(context, 'extra')) {
        Object.keys(context.extra).forEach((key) => {
          scope.setExtra(key, context.extra[key]);
        });
      }

      if (!_.isEmpty(this.tags)) {
        Object.keys(this.tags).forEach((key) => {
          scope.setTag(key, this.tags[key]);
        });
      }

      if (user) {
        scope.setUser(user);
      }

      if (context.level === 'error' || context.level === 'fatal') {
        let error;
        if (_.isNativeError(info) === true) {
          error = info;
        } else {
          error = new Error(message);
          if (info.stack) {
            error.stack = info.stack;
          }
        }
        this.sentry.captureException(error);
        return next(null, true);
      }

      this.sentry.captureMessage(
        typeof message === 'string' ? message : JSON.stringify(message),
        context.level,
      );
      return next(null, true);
    });
  }
}
