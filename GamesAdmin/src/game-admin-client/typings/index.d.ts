/// <reference types="@types/jest" />
/// <reference types="nodejs" />

declare module '*.scss' {
  export interface Interface {
    readonly [key: string]: string
  }
}
