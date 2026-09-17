
export enum ChangeType {
    added = 0,
    udpated,
    deleted
}

export interface IChnageNotification<TEntity> {
    entities: Array<TEntity>;
    changeType: ChangeType
}