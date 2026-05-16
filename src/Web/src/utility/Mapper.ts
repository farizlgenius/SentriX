export function mapFields<T, U>(obj: T, extra: U): T & U {
  return { ...obj, ...extra }
}

