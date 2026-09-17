
export enum UserRole {
    Admin = 0,
    Editor = 1,
    Viewer = 2
}

export interface IUser {
    id: number
    username: string;
}