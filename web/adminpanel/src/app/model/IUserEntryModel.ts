import { IUser, UserRole } from "./IUser";

export interface IUserEntryModel {
    user: IUser;
    roles: Array<UserRole>;
}