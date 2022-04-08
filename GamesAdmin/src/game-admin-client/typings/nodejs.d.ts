declare namespace NodeJS {
  interface ProcessEnv {
    readonly NEXT_PUBLIC_I18N_CONFIG_PATH: string;
    readonly NEXT_PUBLIC_PARTNERS: string;
    readonly NEXT_PUBLIC_ENV: string;
    readonly NEXT_PUBLIC_CONSOLE_LOG_LEVEL: string;
    readonly NEXT_PUBLIC_SENTRY_LOG_LEVEL: string;
    readonly NEXT_PUBLIC_SENTRY_DSN: string;
    readonly NEXT_PUBLIC_GAME_SERVER_ENDPOINT: string;
    readonly NEXT_PUBLIC_SEDIE_SERVER_ENDPOINT: string;
    readonly NEXT_PUBLIC_SEDIE_SERVER_PROXY_ROUTE: string;
    readonly NEXT_PUBLIC_VIP_SEDIE_SERVER_ENDPOINT: string;
    readonly NEXT_PUBLIC_VIP_SEDIE_SERVER_PROXY_ROUTE: string;
    readonly NEXT_PUBLIC_SPECIAL_SEDIE_SERVER_ENDPOINT: string;
    readonly NEXT_PUBLIC_SPECIAL_SEDIE_SERVER_PROXY_ROUTE: string;
    readonly NEXT_PUBLIC_TAIXIU_ORDER: string;
    readonly NEXT_PUBLIC_CHANLE_ORDER: string;
  }
}
