import { IErrorInfo } from "./IErrorInfo";

export interface IAPIResponse<DT> {
    isSuccess: boolean;
    errorInfo: IErrorInfo;
    data: DT;
}