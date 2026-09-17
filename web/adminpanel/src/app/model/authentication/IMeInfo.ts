import { IUser, UserRole } from "../IUser";

export interface IMeInfo {
    me: IUser;
    roles: Array<UserRole>;
}