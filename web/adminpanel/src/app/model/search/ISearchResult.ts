
export interface ISearchResult<T> {
    draw: number;
    total: number;
    filtered: number;
    items: Array<T>
}